using Microsoft.VisualStudio.TestTools.UnitTesting;
using Home.Flows;
using UserManageTest.Base;

namespace Home.Tests
{
    [TestClass]
    public class HomeTest : TestBase
    {
        [TestMethod]
        public void Test_Home()
        {
            var homeView = new HomeFlows(_app, _automation, _mainWindow, _cf);
            homeView.HomeTest();
        }
    }
}
