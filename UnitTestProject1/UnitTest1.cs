using Microsoft.VisualStudio.TestTools.UnitTesting;
using simpleMailTransferProtocolExample;
using simpleMailTransferProtocolExample.classes;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Redmine.Net.Api;
using Redmine.Net.Api.Async;
using Redmine.Net.Api.Types;
using System.Net;
using System.Net.Mail;
using System.Collections.Specialized;
using System.ComponentModel;
using static simpleMailTransferProtocolExample.MainWindow;
using System.Security.Policy;
using System.IO;

namespace UnitTestProject1
{

    [TestClass]

    public class UnitTest1
    {


        /// <summary>
        /// создаем менеджер Redmine с корректными учетными данными и проверяем, что GetCurrentUser() 
        /// возвращает не-null значение и имя пользователя совпадает с ожидаемым.
        /// </summary>
        private const string TestHost = "http://testred.ru/";

        [TestMethod]
        public void Login_Success_ReturnsCurrentUser()
        {

            var username = "morozov_av";
            var password = "bKL4U7Q5hS";
            var manager = new RedmineManager(TestHost, username, password);


            var currentUser = manager.GetCurrentUser();


            Assert.IsNotNull(currentUser);
            Assert.AreEqual(username, currentUser.Login);
            using (StreamWriter sw = new StreamWriter(@"C:\test\test_results.txt"))
            {
                sw.WriteLine("TestMethod Login_Success_ReturnsCurrentUser = Passed");

            }

        }
        /// <summary>
        ///  создаем менеджер Redmine с некорректными учетными данными и проверяем, что GetCurrentUser() выбрасывает исключение RedmineException.
        /// </summary>
        [TestMethod]
        public void Login_InvalidCredentials_ThrowsException()
        {

            var username = "invaliduser";
            var password = "invalidpassword";
            var manager = new RedmineManager(TestHost, username, password);
            using (StreamWriter sw = new StreamWriter(@"C:\test\test_results.txt"))
            {
                Assert.ThrowsException<WebException>(() => manager.GetCurrentUser());

                sw.WriteLine("TestMethod Login_InvalidCredentials_ThrowsException = Passed");


            }
        }
        [TestMethod]
        public void TestHeckMethod()
        {

            var subject = "(175)";
            values.isAuth = true;

            var result = RedmineUnit.heck(subject);

            Assert.AreEqual("175", result);
            using (StreamWriter sw = new StreamWriter(@"C:\test\test_results.txt"))
            {
                sw.WriteLine("TestMethod TestHeckMethod = Passed");

            }
        }
        [TestMethod]
        public void TestHeckMethod1()
        {

            var subject = "(12312)";
            values.isAuth = true;

            var result = RedmineUnit.heck(subject);

            Assert.IsInstanceOfType(result, typeof(string));
            using (StreamWriter sw = new StreamWriter(@"C:\test\test_results.txt"))
            {
                sw.WriteLine("TestMethod TestHeckMethod1 = Passed");

            }
        }
        [TestMethod]
        public void TestHeckWithInvalidSubject()
        {
            // Arrange
            string subject = "Этот заголовок не содержит номер обращения ";

            // Act
            string result = RedmineUnit.heck(subject);

            // Assert
            Assert.IsNull(result);
            using (StreamWriter sw = new StreamWriter(@"C:\test\test_results.txt"))
            {
                sw.WriteLine("TestMethod TestHeckWithInvalidSubject = Passed");
            }
        }
        [TestMethod]
        public void TestHeckWithInvalidSubject_Two()
        {
            // Arrange
            string subject = "Этот заголовок не содержит номер обращения, ну почти (0000000)";

            // Act
            string result = RedmineUnit.heck(subject);

            // Assert
            Assert.IsNull(result);
            using (StreamWriter sw = new StreamWriter(@"C:\test\test_results.txt"))
            {
                sw.WriteLine("TestMethod TestHeckWithInvalidSubject_Two = Passed");
            }
        }
        [TestMethod]
        public void TestEmailAuthentication()
        {

            string email = "helpdesk.smtp@bk.ru";
            string password = "LFJJwEqp2hKsFDq9Jqrm";


            SmtpClient client = new SmtpClient("smtp.bk.ru", 587);
            client.Credentials = new System.Net.NetworkCredential(email, password);
            client.EnableSsl = true;
            bool isAuthenticated = false;

            try
            {
                client.Send(new MailMessage(email, email, "Тестовая аутентификация", "Тестовая почта"));
                isAuthenticated = true;
            }
            catch (SmtpException)
            {
                isAuthenticated = false;
            }

            Assert.IsTrue(isAuthenticated);
            using (StreamWriter sw = new StreamWriter(@"C:\test\test_results.txt"))
            {
                sw.WriteLine("TestMethod TestEmailAuthentication = Passed");
            }
        }
        [TestMethod]
        public void TestRedmineCommentExists()
        {

            // Arrange
            string host = "http://testred.ru/";
            string issueId = "180"; // здесь указать реальный id задачи в редмайне
            string login = "morozov_av";
            string password = "bKL4U7Q5hS";
            var redmineManager = new RedmineManager(host, login, password);
            var parameters = new NameValueCollection();
            parameters.Add("issue_id", issueId);
            parameters.Add("include", "journals");

            // Act
            var issue = redmineManager.GetObject<Issue>(issueId, parameters);

            // Assert
            var comment = issue.Journals.FirstOrDefault(j => j.Notes.Contains("1"));


            Assert.IsNotNull(comment);
            using (StreamWriter sw = new StreamWriter(@"C:\test\test_results.txt"))
            {
                sw.WriteLine("TestMethod TestRedmineCommentExists = Passed");
            }
        }
        [TestMethod]
        public void TestRedmineComment()
        {

            // Arrange
            string host = "http://testred.ru/";
            string issueId = "180"; 
            string login = "morozov_av";
            string password = "bKL4U7Q5hS";
            var redmineManager = new RedmineManager(host, login, password);
            var parameters = new NameValueCollection();
            parameters.Add("issue_id", issueId);
            parameters.Add("include", "journals");

            // Act
            var issue = redmineManager.GetObject<Issue>(issueId, parameters);

            // Assert
            Assert.IsNotNull(issue, "Issue is null");
            Assert.IsNotNull(issue.Journals, "Journals are null");
            Assert.IsTrue(issue.Journals.Any(), "No comments found in the issue");
            using (StreamWriter sw = new StreamWriter(@"C:\test\test_results.txt"))
            {
                sw.WriteLine("TestMethod TestRedmineComment = Passed");
            }
        }
        private RedmineManager _manager;




        private readonly string _redmineHost = "http://testred.ru/";
        private readonly string _redmineLogin = "user1";
        private readonly string _redminePassword = "12345678";
        [TestMethod]
        public void TestUserExists()
        {




            string userName = "morozov_av";


            bool exists = RedmineUnit.UserExists(_redmineHost, _redmineLogin, _redminePassword, userName);


            Assert.IsTrue(exists);
            using (StreamWriter sw = new StreamWriter(@"C:\test\test_results.txt"))
            {
                sw.WriteLine("TestMethod TestUserExists = Passed");

            }
        }



    }
}
