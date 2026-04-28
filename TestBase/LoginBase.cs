using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.IO;


namespace LoginTest.Base
{
    [TestClass]
    public abstract class TestBase
    {
        protected Application _app;
        protected UIA3Automation _automation;
        protected Window _mainWindow;
        protected ConditionFactory _cf;
        protected AutomationElement _window;

        protected const string AppPath = @"D:\huakan\Exo\Slots\B\Client\CytoNiche.Exo.exe";

        [TestInitialize]
        public virtual void Setup()
        {
            Assert.IsTrue(File.Exists(AppPath), $"找不到程序：{AppPath}");
            _app = Application.Launch(AppPath);
            _automation = new UIA3Automation();
            _cf = _automation.ConditionFactory;
            Thread.Sleep(2000);
            _mainWindow = _app.GetMainWindow(_automation);
            Assert.IsNotNull(_mainWindow, "没有找到主窗口");

        }

        [TestCleanup]
        public void TearDown()
        {
            _automation?.Dispose();
        }       

    }
}

