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
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Net.Imap;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using MailKit;
using System.IO;
using AngleSharp.Html.Parser;
using System.Text.RegularExpressions;

namespace simpleMailTransferProtocolExample
{
    /// <summary>
    /// Логика взаимодействия для messageWindow.xaml
    /// </summary>
    public partial class messageWindow : Window
    {
        MimeMessage m;

        public messageWindow(MimeMessage m)
        {
            InitializeComponent();

            this.m = m;

            fromTB.Text = m.From.ToString();
            subject.Text = m.Subject.ToString();
            if (m.TextBody != null)
            {
                bodyTB.Text = m.TextBody.ToString();
            }

            if (m.Subject != null && Regex.IsMatch(m.Subject.ToString(), "[(][0-9]{6}[)]$"))
            {
                openRmTaskBTN.Visibility = Visibility.Visible;
            }
        }

        private void openRmTaskBTN_Click(object sender, RoutedEventArgs e)
        {
            string sub = m.Subject.ToString();
            sub = sub.Substring(sub.LastIndexOf('('));
            sub = sub.Replace("(", "");
            sub = sub.Replace(")", "");
            System.Diagnostics.Process.Start($"http://red.transset.ru/issues/{sub}");
        }
    }
}
