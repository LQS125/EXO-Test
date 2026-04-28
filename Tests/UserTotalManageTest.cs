using LoginHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using UserCreate.Flows;
using UserDelete.Flows;
using UserEdit.Flows;
using UserManageTest.Base;
using UserResetPw.Flows;
using UserUpdateRole.Flows;

namespace User.Tests
{
    [TestClass]
    public class TotalUserTest : TestBase
    {
        [TestMethod]
        public void Test_TotalUser()
        {
            var userCreateFlows = new UserCreateFlows(_app, _automation, _mainWindow, _cf);
            string userName = userCreateFlows.CreateRandomUser();
            Assert.IsNotNull(userName, "用户创建失败");

            var userEditFlows = new UserEditFlows(_app, _automation, _mainWindow, _cf);
            userEditFlows.EditUser();

            var userResetPwFlows = new UserResetPwFlows(_app, _automation, _mainWindow, _cf);
            string userName1 = userResetPwFlows.ResetPw();
            Assert.IsNotNull(userName1, "用户创建失败");
            loginHelper.Logout(_app, _automation, _mainWindow, _cf);
            loginHelper.Login(_app, _automation, _mainWindow, _cf, userName1);
            Thread.Sleep(2000);
            var mainWindow = _app.GetMainWindow(_automation);
            Assert.IsNotNull(mainWindow, "未能获取主窗口");
            Thread.Sleep(2000);
            var element = mainWindow.FindFirstDescendant(_cf.ByName("主界面"));
            Assert.IsNotNull(element, "没找到主界面失败");
            loginHelper.Logout(_app, _automation, _mainWindow, _cf);
            loginHelper.LoginAdmin(_app, _automation, _mainWindow, _cf);

            var userUpdateROleFlows = new UserUpdateRoleFlows(_app, _automation, _mainWindow, _cf, _window);

            var userDeleteFlows = new UserDeleteFlows(_app, _automation, _mainWindow, _cf);
            userDeleteFlows.UserDelete();
        }
    }
}
