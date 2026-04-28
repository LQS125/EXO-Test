using UserManageTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserCreate.Flows;

namespace User.Tests
{
    [TestClass]
    public class CreateUserTest : TestBase
    {
        [TestMethod]
        public void Test_CreateNewUser()
        {
            var userCreateFlows = new UserCreateFlows(_app, _automation, _mainWindow, _cf);
            string userName = userCreateFlows.CreateRandomUser();
            Assert.IsNotNull(userName, "用户创建失败");
        }
    }
}
