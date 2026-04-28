using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace LoginTest.PageObjects
{
    public class LoginPage
    {
        private AutomationElement _window;   //应用程序的主窗口
        private readonly ConditionFactory _cf;

        public LoginPage(Window window, ConditionFactory cf)
        {
            _window = window;
            _cf = cf;
        }

        public void Login(string username, string password)
        {
            var usernameBox = _window.FindFirstDescendant(cf => cf.ByAutomationId("Login_UsernameInput"))?.AsTextBox();  
            var passwordBox = _window.FindFirstDescendant(cf => cf.ByAutomationId("Login_PasswordInput"))?.AsTextBox(); 
            var loginButton = _window.FindFirstDescendant(cf => cf.ByAutomationId("Login_SubmitButton"))?.AsButton();  
            Assert.IsNotNull(usernameBox, "没有找到用户名输入框");
            Assert.IsNotNull(passwordBox, "没有找到密码输入框");
            Assert.IsNotNull(loginButton, "没有找到登录按钮");

            usernameBox.Enter(username);
            Thread.Sleep(1000);
            passwordBox.Enter(password);
            Thread.Sleep(1000);
            loginButton.Invoke();
        }
    
    }
}
