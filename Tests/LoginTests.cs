using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoginTest.Base;
using LoginTest.Flows;

namespace Login.Tests
{
    [TestClass]
    public class LoginTests : TestBase   //继承命名空间：Tests.BaseTest 中的 TestBase
    {

        [TestMethod]
        public void LoginTest()
        {

            var mainWindow = LoginFlow.PerformLogin(_app, _automation, _cf, _mainWindow, "admin", "admin");

            //Thread.Sleep(2000);

            bool isSuccessful = LoginFlow.IsLoginSuccessful(mainWindow, _cf);
            Assert.IsTrue(isSuccessful, "登录失败");

        }
    }
}
