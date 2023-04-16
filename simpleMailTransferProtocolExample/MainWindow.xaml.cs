using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Mail;
using System.Net;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Net.Imap;
using HtmlAgilityPack;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using MailKit;
using MailKit.Security;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using Redmine.Net.Api;
using Redmine.Net.Api.Async;
using Redmine.Net.Api.Types;
using System.IO;

namespace simpleMailTransferProtocolExample
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;

        List<messageList> list = new List<messageList>();
        ImapClient client = new ImapClient();
        int countMessages = 0;
        IMailFolder inbox;

        string mail = "helpdesk.smtp@bk.ru";
        string password = "LFJJwEqp2hKsFDq9Jqrm";
      
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Tick += timerMethod;
            timer.Start();

            client.Connect("imap.mail.ru", 993, SecureSocketOptions.SslOnConnect);
            client.Authenticate(mail, password);
            inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            countMessages = inbox.Count;

            refreshList();
            messagesLV.ItemsSource = Enumerable.Reverse(list);
            messagesLV.SelectedValuePath = "id";
        }

        public struct Userss
        {
            public int Id { get; set; }
            public string UserName { get; set; }

        }

        async void timerMethod(object sender, EventArgs e)
        {
            inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            if (inbox.Count != countMessages)
            {
                await Task.Run(() => refreshList());
                countMessages = inbox.Count;

                messagesLV.ItemsSource = Enumerable.Reverse(list);
                messagesLV.SelectedValuePath = "id";
            }
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("helpdesk", mail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = message
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, true);
                await client.AuthenticateAsync(mail, password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
        
        void refreshList()
        {
            list.Clear();
            using (ImapClient client = new ImapClient())
            {
                client.Connect("imap.mail.ru", 993, true);
                client.Authenticate(mail, password);
                IMailFolder inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                for (int i = 0; i < inbox.Count; i++)
                {
                    MimeMessage m = inbox.GetMessage(i);
                    list.Add(new messageList()
                    {
                        id = i,
                        from = m.From.ToString(),
                        subject = m.Subject.ToString(),
                        body = m.HtmlBody,   
                        message = m
                    }); ;
                }
            }
        }

        private void refreshListBTN_Click(object sender, RoutedEventArgs e)
        {
            refreshList();
        }

        private void isSeenCB_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void openSendLetterWindowBTN_Click(object sender, RoutedEventArgs e)
        {
            messageAndrew window = new messageAndrew();
            window.Show();
        }

        private void replyLetterBTN_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(messagesLV.SelectedValue);
            MimeMessage m = list.FirstOrDefault(x => x.id == id).message;

            messageAndrew window = new messageAndrew(m);
            window.Show();
        }

        private void openRmTaskBTN_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(messagesLV.SelectedValue);
            MimeMessage m = list.FirstOrDefault(x => x.id == id).message;

            string sub = m.Subject.ToString();
            sub = sub.Substring(sub.LastIndexOf('('));
            sub = sub.Replace("(", "");
            sub = sub.Replace(")", "");
            System.Diagnostics.Process.Start($"http://testred.ru/issues/{sub}");
        }

        string issueID;

        private void messagesLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            replyLetterBTN.Visibility = Visibility.Visible;
            int id = Convert.ToInt32(messagesLV.SelectedValue);
            MimeMessage m = list.FirstOrDefault(x => x.id == id).message;

            if (m.Subject != null && Regex.IsMatch(m.Subject.ToString(), "[(][0-9]{3,6}[)]$") && values.isAuth)
            {
                openRmTaskBTN.Visibility = Visibility.Visible;
                addCommentToIssueBTN.Visibility = Visibility.Visible;
                createIssueBTN.Visibility = Visibility.Collapsed;
                string sub = m.Subject.ToString();
                sub = sub.Substring(sub.LastIndexOf('('));
                sub = sub.Replace("(", "");
                sub = sub.Replace(")", "");
                issueID = sub;
            }
            else
            {
                openRmTaskBTN.Visibility = Visibility.Collapsed;
                addCommentToIssueBTN.Visibility = Visibility.Collapsed;
                if (values.isAuth)
                {
                    createIssueBTN.Visibility = Visibility.Visible;
                }   
                issueID = null;
            }

            fromTB.Text = $"От: {m.From}";
            subjectTB.Text = $"Тема: {m.Subject}";
            bodyTB.Text = $"Письмо:\n{m.TextBody}";
        }

        private void addCommentToIssueBTN_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(messagesLV.SelectedValue);
            MimeMessage m = list.FirstOrDefault(x => x.id == id).message;

            var host = "http://testred.ru/";
            var a = new RedmineManager(host, infoAuth.loginRm, infoAuth.passwordRm);
            var parameters = new NameValueCollection();
            User currentUser = a.GetCurrentUser();
            parameters.Add("issue_id", issueID);
            parameters.Add("include", "journals");
            Issue issues = a.GetObject<Issue>(issueID, parameters);
            string fromMail = m.From.ToString();
            fromMail = fromMail.Substring(fromMail.LastIndexOf('<'));
            fromMail = fromMail.Replace("<", "");
            fromMail = fromMail.Replace(">", "");
            issues.Notes = $"{fromMail}\n{m.TextBody}";
      
            a.UpdateObject(issues.Id.ToString(), issues);
        }

        public List<Userss> allUsers()
        {
            List<Userss> users = new List<Userss>();

            var host = "http://testred.ru/";
            var parameters = new NameValueCollection();
            RedmineManager a = new RedmineManager(host, infoAuth.loginRm, infoAuth.passwordRm);
            IList<User> us = a.GetObjects<User>();
            foreach (User user in us)
            {
                Userss u = new Userss();
                u.Id = user.Id;
                u.UserName = user.Login;
                users.Add(u);
            }
            return users;
        }

        int id;

        public void CreateIss(string idLRPP, string MiniInf, string FullInf, string idRespond)
        {
            var host = "http://testred.ru/";
            var redmine = new RedmineManager(host, infoAuth.loginRm, infoAuth.passwordRm);

            redmine.ImpersonateUser = infoAuth.loginRm;
            User user = redmine.GetCurrentUser();
            IList<IssueCustomField> fields = new List<IssueCustomField>();
            IList<CustomFieldValue> fieldss = new List<CustomFieldValue>();
            IssueCustomField cs = new IssueCustomField();
            CustomFieldValue cf = new CustomFieldValue();

            cf.Info = idLRPP;
            cs.Id = 10;
            fieldss.Add(cf);
            cs.Values = fieldss;
            fields.Add(cs);
            Issue redminetask = new Issue()
            {

                AssignedTo = new IdentifiableName() { Id = 12 },
                Author = new IdentifiableName() { Id = user.Id },
                Subject = MiniInf,
                Description = FullInf,
                Project = new IdentifiableName { Id = 5 },
                StartDate = DateTime.Now,
                CreatedOn = DateTime.Now,
                Tracker = new IdentifiableName { Id = 1 },
                Status = new IdentifiableName { Id = 1 },
                Priority = new IdentifiableName { Id = 1 },
                CustomFields = fields,
                IsPrivate = false,
                Category = new IdentifiableName { Id = 1 },
            };
            redmine.CreateObject(redminetask);
            id = redminetask.Id;
        }

        private void authRmBTN_Click(object sender, RoutedEventArgs e)
        {
            rmAuthWindow window = new rmAuthWindow();
            window.Show();

            window.Closing += (obj, args) =>
            {
                if (values.isAuth)
                {
                    authRmBTN.Visibility = Visibility.Collapsed;
                }
            };
        }

        private void createIssueBTN_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(messagesLV.SelectedValue);
            MimeMessage m = list.FirstOrDefault(x => x.id == id).message;

            CreateIss(DateTime.Now.Year.ToString(), "Нужно заполнить", m.TextBody, "0");
            string fromMail = m.From.ToString();
            fromMail = fromMail.Substring(fromMail.LastIndexOf('<'));
            fromMail = fromMail.Replace("<", "");
            fromMail = fromMail.Replace(">", "");
            MessageBox.Show(id.ToString());
            SendEmailAsync(fromMail, $"Re: {m.Subject} ({id})", $"Здравствуйте!\nВаше обращение зарегистрировано под номером {id} и взято в работу.\n--\n{m.TextBody}");
        }
    }
}


