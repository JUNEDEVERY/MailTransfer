//using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
//using MailKit.Net.Imap;
//using MailKit.Search;
//using MailKit;
//using simpleMailTransferProtocolExample.classes;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace simpleMailTransferProtocolExample
//{
//    /// <summary>
//    /// Логика взаимодействия для vh2.xaml
//    /// </summary>
//    public partial class vh2 : Window
//    {
//        public vh2()
//        {
//            InitializeComponent();
//        }
//        private void btnDeleteSelected_Click(object sender, RoutedEventArgs e)
//        {
//            var selectedEmails = dataGridSentEmails.SelectedItems.Cast<SentEmail>();
//            var messageIds = selectedEmails.Select(email => email.MessageId).ToList();
//            DeleteSentEmails(host, port, useSsl, username, password, messageIds);
//        }
//        private void DeleteSentEmails(string host, int port, bool useSsl, string username, string password, List<string> messageIds)
//        {
//            try
//            {
//                using (var client = new ImapClient())
//                {
//                    client.Connect(host, port, useSsl);
//                    client.Authenticate(username, password);

//                    // Search for the emails by Message-Id header
//                    var searchQuery = SearchQuery.HeaderContains("Message-Id", messageIds);
//                    var sentFolder = client.GetFolder(SpecialFolder.Sent);

//                    // Mark the matching emails as deleted
//                    sentFolder.Open(FolderAccess.ReadWrite);
//                    var uids = sentFolder.Search(searchQuery);
//                    sentFolder.AddFlags(uids, MessageFlags.Deleted, true);

//                    // Expunge the deleted emails
//                    sentFolder.Expunge();

//                    // Refresh the data grid to reflect the changes
//                    GetSentEmails(host, port, useSsl, username, password);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error deleting sent emails: {ex.Message}");
//            }
//        }
//    }
//}
