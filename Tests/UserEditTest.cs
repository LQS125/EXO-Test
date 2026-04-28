using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManageTest.Base;
using UserEdit.Flows;

namespace User.Tests
{
    [TestClass]
    public class EditUserTest : TestBase
    {
        [TestMethod]
        public void Test_EditNewUser()
        {
            var userEditFlows = new UserEditFlows(_app, _automation, _mainWindow, _cf);
            userEditFlows.EditUser();
        }
    }
}
