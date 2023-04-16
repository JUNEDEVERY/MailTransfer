    using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using MailKit.Net.Imap;
using Microsoft.Win32;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace simpleMailTransferProtocolExample
{
    /// <summary>
    /// Логика взаимодействия для messageAndrew.xaml
    /// </summary>
    public partial class messageAndrew : Window
    {

        string mail = "helpdesk.smtp@bk.ru";
        string password = "LFJJwEqp2hKsFDq9Jqrm";
        ImapClient client = new ImapClient();
        MimeMessage m;

        bool isReply = false;
        public messageAndrew()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }
        public messageAndrew(MimeMessage m)
        {
            InitializeComponent();
            this.m = m;
            string fromMail = m.From.ToString();
            fromMail = fromMail.Substring(fromMail.LastIndexOf('<'));
            fromMail = fromMail.Replace("<", "");
            fromMail = fromMail.Replace(">", "");
            toTB.Text = fromMail;
            subjectTB.Text = "Re: " + m.Subject.ToString();
            messageBodyTB.Text = "\n\n-----------------\n" + m.TextBody;
            isReply = true;
        }
        private void sendBTN_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument document = rtbEditor.Document;

            string richText = new TextRange(document.ContentStart, document.ContentEnd).Text;

            SendEmailAsync(toTB.Text, subjectTB.Text, richText);
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            string one = "<html><body>";
            string two = "</body></html>";
            bool isBody = false;

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("helpdesk", mail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            TextPart htmlPart = new TextPart("html");
            if (btnBold.IsChecked == true)
            {
                one += "<b>";
                two = "</b>" + two;
                emailMessage.Body = new Multipart("alternative")
            {
               htmlPart
            };
                isBody = true;
            }
            if (btnItalic.IsChecked == true)
            {
                one += "<i>";
                two = "</i>" + two;

                emailMessage.Body = new Multipart("alternative")
            {
               htmlPart
            };
                isBody = true;
            }

            if (btnUnderline.IsChecked == true)
            {
                one += "<u>";
                two = "</u>" + two;
                emailMessage.Body = new Multipart("alternative")
                {
               htmlPart
                };
                isBody = true;
            }

            if (cmbFontSize.SelectedItem != null)
            {
                one += "<font size = \"" + cmbFontSize.SelectedItem.ToString() + "\" >";
                two = "</font> " + two;
                emailMessage.Body = new Multipart("alternative")
                 {
                htmlPart
                 };
                isBody = true;
            }
            if (cmbFontFamily.SelectedItem != null)
            {
                one += "<p style=\"font-family:" + cmbFontFamily.SelectedItem.ToString() + ";\">";
                two = "</p>" + two;


                emailMessage.Body = new Multipart("alternative")
                 {
                htmlPart
                 };
                isBody = true;
            }
            htmlPart.Text = one + message + two;
            //if (isBody)
            //{
            //    htmlPart.Text = one + message + two;

            //}
            //else
            //{
            //    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            //    {
            //        Text = message
            //    };
            //}
            ////htmlPart.Text =   message ;
            ////emailMessage.Body = new Multipart("alternative")
            ////    {
            ////   htmlPart
            ////    };







            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, true);
                await client.AuthenticateAsync(mail, password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
            this.Close(); 
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Text);
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }
    }
}
