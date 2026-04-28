using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace UserManageTest.Base
{
    [TestClass]
    public class TestBase
    {
        protected Application _app;
        protected UIA3Automation _automation;
        protected Window _mainWindow;
        protected ConditionFactory _cf;
        protected AutomationElement _window;

        [TestInitialize]
        public void Setup()
        {
            var processes = Process.GetProcessesByName("CytoNiche.Exo");
            _app = Application.Attach(processes[0].Id);
            _automation = new UIA3Automation();
            _cf = _automation.ConditionFactory; // 初始化成员变量

            // 获取主窗口，否则 _mainWindow 为 null
            _mainWindow = _app.GetMainWindow(_automation);
            Assert.IsNotNull(_mainWindow, "未能获取主窗口");
        }
        [TestCleanup]
        public void TearDown()
        {
            _automation?.Dispose();
        }
    }
}

