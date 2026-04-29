using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using ProcessEdit.Page;
using System;
using System.IO;
using System.Threading;
using Tests.Helpers;

namespace ProcessEdit.Flows
{
    public class ProcessEditFlows
    {
        private readonly Application _app;
        private readonly UIA3Automation _automation;
        private readonly Window _mainWindow;
        private readonly ConditionFactory _cf;

        public ProcessEditFlows(Application app, UIA3Automation automation, Window mainWindow, ConditionFactory cf)
        {
            _app = app;
            _automation = automation;
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public void EditProcess(string testName)
        {

            string testEditNum = CommonHelper.RandomBetween(1, 5).ToString();
            string testEditDec = CommonHelper.GenerateRandomString(8, 12);

            for (int a = 1; a <= 2; a++)
            {
                UiActions.ClickElementByName(_mainWindow, _cf, "工艺管理");
                Thread.Sleep(500);
                var row = UiActions.FindRecipeRowByName(_mainWindow, _cf, $"{testName}");
                //  编辑
                var ProcessEditElement = row.FindFirstDescendant(_cf.ByName($"编辑"));
                Assert.IsNotNull(ProcessEditElement, $"新增失败：在页面上未找到名为 '编辑' 的元素");
                ProcessEditElement.Click();

                var processEditPage = new ProcessEditPage(_mainWindow, _cf);
                processEditPage.BaseMation(testEditNum, testEditDec);
                processEditPage.ViewStep(a);

                var newProcessTextElement = _mainWindow.FindFirstDescendant(_cf.ByName($"{testName}"));
                Assert.IsNotNull(newProcessTextElement, $"新增失败：在页面上未找到名为 '{testName}' 的元素");
                // 查看
                var row2 = UiActions.FindRecipeRowByName(_mainWindow, _cf, $"{testName}");

                var ProcessEditElement2 = row2.FindFirstDescendant(_cf.ByName($"查看"));
                Assert.IsNotNull(ProcessEditElement2, $"在页面上未找到名为 '查看' 的元素");
                ProcessEditElement2.Click();

                processEditPage.EditData(a, testEditNum, testEditDec);

                var CloseButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("ViewRecipe_CloseButton"));
                Assert.IsNotNull(CloseButton, "没有找到关闭按钮");
                CloseButton.Click();
            }
        }
    }
}