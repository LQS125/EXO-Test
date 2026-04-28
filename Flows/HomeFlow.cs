using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using ProcessCreate.Page;
using System;
using System.Linq;
using System.Threading;
using Tests.Helpers;
using System.IO;

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

            // 生成随机数据并赋值给类的公共属性
            string testName = CommonHelper.GenerateRandomString(4, 6);
            string testNum = CommonHelper.RandomBetween(1, 5).ToString();
            string test = CommonHelper.GenerateRandomString(8, 12);

            UiActions.ClickElementByName(_mainWindow, _cf,"主界面");
            Thread.Sleep(100);

            var startButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("Home_PrimaryProcessButton")).AsButton();
            Assert.IsNotNull(startButton, "没找到开始按键");
            startButton.Invoke();
            Thread.Sleep(500);

            var nameInput = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("Home_StartRunPanel_ProcessName_Input")).AsTextBox();
            Assert.IsNotNull(nameInput, "未找到工艺名称控件。");
            nameInput.Enter(testName);
            Thread.Sleep(200);

            var SampleVolumeInput = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("Home_StartRunPanel_SampleVolume_Input")).AsTextBox();
            Assert.IsNotNull(SampleVolumeInput, "未能找到样本体积输入框");
            SampleVolumeInput.Enter(testNum.ToString());
            Thread.Sleep(200);

            var nextButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("Home_StartRunPanel_NextButton")).AsButton();
            Assert.IsNotNull(nextButton, "没找到下一步");
            nextButton.Click();
            Thread.Sleep(200);

            for (int i = 1; i < 10; i++)
            {
                var nextButton2 = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("Home_StartRunPanel_WizardNextOrFinishButton")).AsButton();
                Assert.IsNotNull(nextButton2, "没找到下一步/开始按钮");
                nextButton2.Click();
                Thread.Sleep(200);
            }

            Thread.Sleep(20000);

            for (int i = 1; i < 9; i++)
            {

                var startText = _mainWindow.FindFirstDescendant(_cf.ByName("运行信息"))?.AsWindow();
                Assert.IsNotNull(startText, "未能找到'运行信息'按钮");
                // 获取它的边界矩形
                var rect = startText.BoundingRectangle;
                // 计算点击坐标：右侧外 10 像素，垂直居中
                int x = (int)(rect.Right + 20);
                int y = (int)(rect.Top + rect.Height / 2);
                // 先移动，再点击
                FlaUI.Core.Input.Mouse.MoveTo(x, y);
                FlaUI.Core.Input.Mouse.Click(FlaUI.Core.Input.MouseButton.Left);
                Thread.Sleep(500);

                AutomationElement EndButton = null;
                if (i < 5)
                {
                    EndButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId(EndButtonID[i])).AsButton();
                    Assert.IsNotNull(EndButton, "未能找到'结束'按钮");
                    EndButton.Click();
                    Thread.Sleep(500);
                }
                else
                {
                    EndButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId(EndButtonID[5])).AsButton();
                    Assert.IsNotNull(EndButton, "未能找到'结束'按钮");
                    EndButton.Click();
                    Thread.Sleep(500);

                }

                //// 获取主窗口
                //_mainWindow = _app.GetMainWindow(_automation);
                //Assert.IsNotNull(_mainWindow, "未能获取主窗口");
                Thread.Sleep(3000);

                if (i < 8)
                {
                    var ContinueButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("Home_ContinuePrompt_ContinueButton")).AsButton();
                    Assert.IsNotNull(ContinueButton, "未能找到'继续'按钮");
                    ContinueButton.Invoke();
                    Thread.Sleep(500);
                    UiActions.ClickElementByName(_mainWindow, _cf, "主界面");
                    Thread.Sleep(20000);
                }
                else
                {
                    var ContinueButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("Home_RunCompletedDialog_FinishButton")).AsButton();
                    Assert.IsNotNull(ContinueButton, "未能找到'结束'按钮");
                    ContinueButton.Invoke();
                    Thread.Sleep(500);
                    UiActions.ClickElementByName(_mainWindow, _cf, "主界面");
                    Thread.Sleep(2000);
                }

            }

            //// 获取主窗口
            //_mainWindow = _app.GetMainWindow(_automation);
            //Assert.IsNotNull(_mainWindow, "未能获取主窗口");

            UiActions.ClickElementByName(_mainWindow, _cf, "批次记录");
            Thread.Sleep(100);

            var newProcessTextElement = _mainWindow.FindFirstDescendant(_cf.ByName($"{testName}"));
            Assert.IsNotNull(newProcessTextElement, $"新增失败：在页面上未找到名为 '{testName}' 的元素");

            Thread.Sleep(500);
            var row = UiActions.FindRecipeRowByName(_mainWindow, _cf, $"{testName}");
            //var row = FindRecipeRowByName($"5zOj");
            Assert.IsNotNull(row, $"在页面上未找到名{testName}所在行");


            var ProcessTextElement = row.FindFirstDescendant(_cf.ByName($"查看"));
            Assert.IsNotNull(ProcessTextElement, $"在页面上未找到名{testName}为 '查看' 的元素");
            ProcessTextElement.Click();
            Thread.Sleep(1000);


            var clockButton = _mainWindow.FindFirstDescendant(_cf.ByName("关闭")).AsButton();
            Assert.IsNotNull(clockButton, "未能找到'结束'按钮");
            clockButton.Invoke();
            Thread.Sleep(1000);

            //_mainWindow = _app.GetMainWindow(_automation);
            //Assert.IsNotNull(_mainWindow, "未能找到主窗口");

            UiActions.ClickElementByName(_mainWindow, _cf, "主界面");
            Thread.Sleep(100);
        }
    }
}