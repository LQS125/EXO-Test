using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using Home.Page;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Tests.Helpers;

namespace Home.Flows
{
    public class HomeFlows
    {
        private readonly Application _app;
        private readonly UIA3Automation _automation;
        private readonly Window _mainWindow;
        private readonly ConditionFactory _cf;

        public HomeFlows(Application app, UIA3Automation automation, Window mainWindow, ConditionFactory cf)
        {
            _app = app;
            _automation = automation;
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public void HomeTest()
        {
            string[] EndButtonID = {
                "",
                "Home_RuntimeInfoDrawer_StepTest_IntegrityFlux_EndButton",
                "Home_RuntimeInfoDrawer_StepTest_Clarification_EndButton",
                "Home_RuntimeInfoDrawer_StepTest_Diafiltration_EndButton",
                "Home_RuntimeInfoDrawer_StepTest_Harvest_EndButton",
                "Home_RuntimeInfoDrawer_StepTest_Cleaning_EndButton"
            };

            string testName = CommonHelper.GenerateRandomString(4, 6);
            string testNum = CommonHelper.RandomBetween(1, 5).ToString();
            string test = CommonHelper.GenerateRandomString(8, 12);

            UiActions.ClickElementByName(_mainWindow, _cf,"主界面");
            Thread.Sleep(100);

            var homePage = new HomePage(_mainWindow,_cf);
            homePage.FillBasicInfo(testName, testNum);

            Thread.Sleep(20000);

            homePage.HandEnd(EndButtonID);

            UiActions.ClickElementByName(_mainWindow, _cf, "批次记录");
            Thread.Sleep(100);

            var newProcessTextElement = _mainWindow.FindFirstDescendant(_cf.ByName($"{testName}"));
            Assert.IsNotNull(newProcessTextElement, $"新增失败：在页面上未找到名为 '{testName}' 的元素");

            Thread.Sleep(500);
            var row = UiActions.FindRecipeRowByName(_mainWindow, _cf, $"{testName}");;
            Assert.IsNotNull(row, $"在页面上未找到名{testName}所在行");

            var ProcessTextElement = row.FindFirstDescendant(_cf.ByName($"查看"));
            Assert.IsNotNull(ProcessTextElement, $"在页面上未找到名{testName}为 '查看' 的元素");
            ProcessTextElement.Click();
            Thread.Sleep(1000);

            var clockButton = _mainWindow.FindFirstDescendant(_cf.ByName("关闭")).AsButton();
            Assert.IsNotNull(clockButton, "未能找到'结束'按钮");
            clockButton.Invoke();
            Thread.Sleep(1000);

            UiActions.ClickElementByName(_mainWindow, _cf, "主界面");
            Thread.Sleep(100);
        }
    }
}