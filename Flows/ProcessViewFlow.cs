using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Tests.Helpers;
using ProcessView.Page;

namespace ProcessView.Flows
{
    public class processViewFlows
    {
        private readonly Application _app;
        private readonly UIA3Automation _automation;
        private readonly Window _mainWindow;
        private readonly ConditionFactory _cf;

        public processViewFlows(Application app, UIA3Automation automation, Window mainWindow, ConditionFactory cf)
        {
            _app = app;
            _automation = automation;
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public void ViewProcess(string testName, string testNum, string test)
        {

            UiActions.ClickElementByName(_mainWindow, _cf, "工艺管理");
            Thread.Sleep(500);
            var row = UiActions.FindRecipeRowByName(_mainWindow, _cf, $"{testName}");

            var ProcessTextElement = row.FindFirstDescendant(_cf.ByName($"查看"));
            Assert.IsNotNull(ProcessTextElement, $"新增失败：在页面上未找到名为 '查看' 的元素");
            ProcessTextElement.Click();
            Thread.Sleep(1000);

            var processViewPage = new ProcessViewPage(_mainWindow,_cf);
            processViewPage.ViewBasicInfo(testNum, test);

            processViewPage.ViewData();

        }
    }
}