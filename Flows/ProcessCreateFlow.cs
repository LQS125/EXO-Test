using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProcessCreate.Page;
using System.Threading;
using Tests.Helpers;


namespace ProcessCreate.Flows
{
    public class ProcessCreateFlows
    {
        private readonly Application _app;
        private readonly UIA3Automation _automation;
        private readonly Window _mainWindow;
        private readonly ConditionFactory _cf;

        public ProcessCreateFlows(Application app, UIA3Automation automation, Window mainWindow, ConditionFactory cf)
        {
            _app = app;
            _automation = automation;
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public (string testName, string testNum, string test) CreateProcess()
        {
            string testName = CommonHelper.GenerateRandomString(4, 6);
            string testNum = CommonHelper.RandomBetween(1, 5).ToString(); // RandomBetween 返回 int，转为 string
            string test = CommonHelper.GenerateRandomString(8, 12);

            UiActions.ClickElementByName(_mainWindow,_cf,"工艺管理");
            Thread.Sleep(500);
            UiActions.ClickElementByName(_mainWindow, _cf, "新增");
            Thread.Sleep(500);

            var createUserDialog = _mainWindow.FindFirstDescendant(_cf.ByName("添加配方"))?.AsWindow();
            Assert.IsNotNull(createUserDialog, "未能找到'添加配方'弹窗");

            var createDialog = new ProcessCreatePage(_mainWindow, _cf);
            createDialog.FillBasicInfo( testName,  testNum,  test);

            var nextButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("AddProcess_NextStepButton")).AsButton();
            Assert.IsNotNull(nextButton, "没找到下一步");
            nextButton.Click();
            Thread.Sleep(200);

            createDialog.CreateStep();

            return (testName, testNum, test);
        }
    }
}