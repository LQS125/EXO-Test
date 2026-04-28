using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManageTest.Base;
using UserUpdateRole.Flows;

namespace User.Tests
{
    [TestClass]
    public class UserUpdateRole : TestBase
    {
        [TestMethod]
        public void Test_UpdateRole()
        {
            var userUpdateROleFlows = new UserUpdateRoleFlows(_app, _automation, _mainWindow, _cf,_window);

        }
    }
}
