using FlaUI.Core;
using FlaUI.UIA3;
using LoginTest.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using FlaUI.Core.AutomationElements;  
using FlaUI.Core.Conditions;

namespace LoginTest.Flows
{
    public static class LoginFlow
    {
        public static Window PerformLogin(Application app, UIA3Automation automation, 
            ConditionFactory cf, Window loginWindow, string username, string password)
        {
            var loginPage = new LoginPage(loginWindow, cf);
            loginPage.Login(username, password);

            Thread.Sleep(2000);

            var mainWindow = app.GetMainWindow(automation);
            Assert.IsNotNull(mainWindow, "登录后未能获取到主窗口");

            return mainWindow;
        }

        public static bool IsLoginSuccessful(Window mainWindow, ConditionFactory cf)
        {
            var adminElement = mainWindow.FindFirstDescendant(c => c.ByName("管理员"));
            var operatorElement = mainWindow.FindFirstDescendant(c => c.ByName("主界面"));
            return (adminElement != null) || (operatorElement != null);
        }
    }
}
