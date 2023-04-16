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
using Redmine.Net.Api;
using Redmine.Net.Api.Async;
using Redmine.Net.Api.Types;
namespace simpleMailTransferProtocolExample
{
    /// <summary>
    /// Логика взаимодействия для rmAuthWindow.xaml
    /// </summary>
    public partial class rmAuthWindow : Window
    {
        public rmAuthWindow()
        {
            InitializeComponent();
        }
        private void enterBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var host = "http://testred.ru/";
                var a = new RedmineManager(host, loginTB.Text, passwordPB.Password);
                User currUser = a.GetCurrentUser();
                infoAuth.isAuthForum = true;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    infoAuth.loginRm = loginTB.Text;
                    infoAuth.passwordRm = passwordPB.Password;
                    values.isAuth = true;
                    this.Close();
                });

            }
            catch
            {
                MessageBox.Show("Проверьте введенные данные!");
            }
        }
    }
}
