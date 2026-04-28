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
            // 生成随机数据并赋值给类的公共属性
            string testName = CommonHelper.GenerateRandomString(4, 6);
            string testNum = CommonHelper.RandomBetween(1, 5).ToString(); // RandomBetween 返回 int，转为 string
            string test = CommonHelper.GenerateRandomString(8, 12);

            //_mainWindow = _app.GetMainWindow(_automation);
            //Assert.IsNotNull(_mainWindow, "未能找到主窗口");

            UiActions.ClickElementByName(_mainWindow,_cf,"工艺管理");
            Thread.Sleep(500);
            UiActions.ClickElementByName(_mainWindow, _cf, "新增");
            Thread.Sleep(500);

            // 获取“添加配方”弹窗对象
            var createUserDialog = _mainWindow.FindFirstDescendant(_cf.ByName("添加配方"))?.AsWindow();
            Assert.IsNotNull(createUserDialog, "未能找到'添加配方'弹窗");

            var createDialog = new ProcessCreatePage(_mainWindow, _cf);
            createDialog.FillBasicInfo( testName,  testNum,  test);

            var nextButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("AddProcess_NextStepButton")).AsButton();
            Assert.IsNotNull(nextButton, "没找到下一步");
            nextButton.Click();
            Thread.Sleep(200);

            JArray processData = JArray.Parse(File.ReadAllText("ProcessCreate_ValueId.json"));

            int stepIndex = 0;
            foreach (var step in processData)
            {
                stepIndex++;
                string title = step["title"].ToString();
                Console.WriteLine($"步骤：{title}");

                JArray fields = (JArray)step["fields"];

                // 性能优化：一次性获取所有元素，避免 O(N²) 复杂度
                var allDescendants = _mainWindow.FindAllDescendants();
                var elementDict = allDescendants
                    .Where(e => !string.IsNullOrEmpty(e.AutomationId))
                    .ToDictionary(e => e.AutomationId, e => e);

                foreach (var field in fields)
                {
                    string name = field["name"].ToString();
                    string testValue = field["testValue"].ToString();
                    string automationId = field["automationId"].ToString();

                    if (elementDict.TryGetValue(automationId, out var element))
                    {
                        if (!string.IsNullOrEmpty(testValue))
                        {
                            var testBox = element.AsTextBox();
                            Assert.IsNotNull(testBox, $"未找到{title}_{name}控件。");
                            testBox.Enter(testValue);
                            Thread.Sleep(100);
                        }
                        else
                        {
                            var buttonBox = element.AsTextBox();
                            Assert.IsNotNull(buttonBox, $"未找到{title}_{name}控件。");
                            buttonBox.Click();
                        }
                    }
                    else
                    {
                        Assert.Fail($"未找到automationId为 '{automationId}' 的控件（{title}_{name}）");
                    }
                }
                if (stepIndex > 5)
                {
                    var startText = _mainWindow.FindFirstDescendant(_cf.ByName("启用"))?.AsWindow();
                    Assert.IsNotNull(startText, "未能找到'启用'按钮");
                    // 获取它的边界矩形
                    var rect = startText.BoundingRectangle;
                    // 计算点击坐标：右侧外 10 像素，垂直居中
                    int x = (int)(rect.Right + 25);
                    int y = (int)(rect.Top + rect.Height / 2);
                    // 先移动，再点击
                    FlaUI.Core.Input.Mouse.MoveTo(x, y);
                    FlaUI.Core.Input.Mouse.Click(FlaUI.Core.Input.MouseButton.Left);
                    Thread.Sleep(500);
                }

                var NextButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("AddProcess_NextStepButton")).AsButton();
                if (NextButton != null)
                {
                    NextButton.Click();
                    Thread.Sleep(200);
                }
                else
                {
                    //throw new Exception("没找到下一步");
                    UiActions.ClickElementByName(_mainWindow, _cf, "保存配方");
                    Thread.Sleep(1000);
                }
            }

            return (testName, testNum, test);
        }
    }
}