using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using UserManageTest.Base;
using UserResetPw.Flows;
using LoginHelper;

namespace User.Tests
{
    [TestClass]
    public class ResetPwUserTest : TestBase
    {
        [TestMethod]
        public void Test_ResetPwUser()
        {
            var userResetPwFlows = new UserResetPwFlows(_app, _automation, _mainWindow, _cf);
            string userName = userResetPwFlows.ResetPw();

            Assert.IsNotNull(userName, "用户创建失败");

            loginHelper.Logout(_app, _automation, _mainWindow, _cf);

            loginHelper.Login(_app, _automation, _mainWindow, _cf,userName);
            Thread.Sleep(2000);

            var mainWindow = _app.GetMainWindow(_automation);
            Assert.IsNotNull(mainWindow, "未能获取主窗口");
            Thread.Sleep(2000);
            var element = mainWindow.FindFirstDescendant(_cf.ByName("主界面"));
            Assert.IsNotNull(element, "没找到主界面失败");

            loginHelper.Logout(_app, _automation, _mainWindow, _cf);
            loginHelper.LoginAdmin(_app, _automation, _mainWindow, _cf);
        }
    }
}
