using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
using MimeKit;
using Redmine.Net.Api.Types;
using System.Collections.Specialized;

namespace simpleMailTransferProtocolExample.classes
{
    public class RedmineUnit
    {

        public static string heck(string subject)
        {
            if (Regex.IsMatch(subject, "[(][0-9]{3,6}[)]$") && values.isAuth)
            {
                string sub = subject;
                sub = sub.Substring(sub.LastIndexOf('('));
                sub = sub.Replace("(", "");
                sub = sub.Replace(")", "");
                return sub;
            }

            else
            {
                return null;
            }

        }
        public class AuthInfo
        {
            public static AuthInfo infoAuth = new AuthInfo();
            public string loginRm { get; set; }
            public string passwordRm { get; set; }
            public bool isAuthForum { get; set; }
        }

        public class Values
        {
            public static Values values = new Values();
            public bool isAuth { get; set; }
        }
        public static bool UserExists(string host, string login, string password, string userName)
        {
            RedmineManager manager = new RedmineManager(host, login, password);
            List<User> users = manager.GetObjects<User>(new NameValueCollection {
        { "name", userName }
    }).ToList();

            return users.Count > 0;
        }

    }

}
