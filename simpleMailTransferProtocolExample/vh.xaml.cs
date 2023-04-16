using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;
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
using System.Windows.Shapes;
using System.Web.UI.WebControls;
using simpleMailTransferProtocolExample.classes;

namespace simpleMailTransferProtocolExample
{
    /// <summary>
    /// Логика взаимодействия для vh.xaml
    /// </summary>
    public partial class vh : Window
    {
        public vh()
        {
            InitializeComponent();

        }
       public static string mail = "helpdesk.smtp@bk.ru";
        public static string password = "LFJJwEqp2hKsFDq9Jqrm";
      

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            var sentEmailHeaders = GetSentEmailHeaders("imap.mail.ru", 993, true, mail, password);
            sentEmailsList.Items.Clear();
            sentEmailsList.ItemsSource = sentEmailHeaders;
            btnDelete.Visibility = Visibility.Visible;
         
        }

        public static List<string> GetSentEmailHeaders(string host, int port, bool useSsl, string username, string password)
        {
            using (var client = new ImapClient())
            {
                client.Connect(host, port, useSsl);
                client.Authenticate(username, password);
                client.Inbox.Open(FolderAccess.ReadOnly);

                var query = SearchQuery.SentSince(DateTime.Now.AddDays(-30));
                var uids = client.Inbox.Search(query);

                var headers = new List<string>();

                foreach (var uid in uids)
                {
                    var message = client.Inbox.GetMessage(uid);
                    var header = message.Subject + " - " + message.From + " - " + message.Date.ToString("yyyy/MM/dd HH:mm:ss");
                    headers.Add(header);
                }

                return headers;
            }
        }
       


      
   
 

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            

        }
        

    }
    
}
