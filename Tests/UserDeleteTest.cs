using UserManageTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserDelete.Flows;

namespace User.Tests
{
    [TestClass]
    public class UserDeleteTest : TestBase
    {
        [TestMethod]
        public void Test_UserDelete()
        {
            var userDeleteFlows = new UserDeleteFlows(_app, _automation, _mainWindow, _cf);
            userDeleteFlows.UserDelete();
        }
    }
}
