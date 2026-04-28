using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using FlaUI.Core;
using Tests.Helpers;


namespace LoginHelper
{
    public static class loginHelper
    {
        public static void Login(Application _app, UIA3Automation _automation,Window _window, ConditionFactory _cf, string userName)
        {
            //获取应用程序的主窗口，用自动化实例为参数
            _window = _app.GetMainWindow(_automation);
            Assert.IsNotNull(_window, "没有找到主窗口");
            // 查找控件
            var usernameBox = _window.FindFirstDescendant(cf => cf.ByAutomationId("Login_UsernameInput"))?.AsTextBox();   //在主窗口的后代元素中查找第一个符合条件的元素
            var passwordBox = _window.FindFirstDescendant(cf => cf.ByAutomationId("Login_PasswordInput"))?.AsTextBox();  //cf => cf.ByAutomationId(...) 表示按 AutomationId 查找
            var loginButton = _window.FindFirstDescendant(cf => cf.ByAutomationId("Login_SubmitButton"))?.AsButton();  //找到后调用 .AsTextBox() 将其转换为具体的控件类型

            // 断言控件存在
            Assert.IsNotNull(usernameBox, "没有找到用户名输入框");

            // 模拟用户点击和输入
            usernameBox.Enter($"{userName}");
            Thread.Sleep(1000);
            passwordBox.Enter("88888888");
            Thread.Sleep(1000);
            loginButton.Invoke();  // 点击

        }
        public static void Logout(Application _app, UIA3Automation _automation,  Window _mainWindow, ConditionFactory _cf)
        {
            _mainWindow = _app.GetMainWindow(_automation);
            Assert.IsNotNull(_mainWindow, "未能找到主窗口");
            UiActions.ClickElementByName(_mainWindow,_cf,"LOGOUT 退出");
            Thread.Sleep(500);
        }

        public static void LoginAdmin(Application _app, UIA3Automation _automation,  Window _window, ConditionFactory _cf)
        {
            //获取应用程序的主窗口，用自动化实例为参数
            _window = _app.GetMainWindow(_automation);
            Assert.IsNotNull(_window, "没有找到主窗口");
            // 查找控件
            var usernameBox = _window.FindFirstDescendant(cf => cf.ByAutomationId("Login_UsernameInput"))?.AsTextBox();
            var passwordBox = _window.FindFirstDescendant(cf => cf.ByAutomationId("Login_PasswordInput"))?.AsTextBox();
            var loginButton = _window.FindFirstDescendant(cf => cf.ByAutomationId("Login_SubmitButton"))?.AsButton();

            // 模拟用户点击和输入
            usernameBox.Enter($"admin");
            Thread.Sleep(1000);
            passwordBox.Enter("admin");
            Thread.Sleep(1000);
            loginButton.Invoke();  // 点击
            Thread.Sleep(2000);
        }
    }
}
