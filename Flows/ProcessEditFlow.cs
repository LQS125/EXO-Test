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
//using static System.Net.Mime.MediaTypeNames;


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
                //var row = FindRecipeRowByName($"PFng");
                //  --------------------------编辑-------------------------------
                var ProcessEditElement = row.FindFirstDescendant(_cf.ByName($"编辑"));
                Assert.IsNotNull(ProcessEditElement, $"新增失败：在页面上未找到名为 '编辑' 的元素");
                ProcessEditElement.Click();

                var processEditPage = new ProcessEditPage(_mainWindow, _cf);
                processEditPage.BaseMation(testEditNum, testEditDec);


                JArray processData = JArray.Parse(File.ReadAllText("ProcessCreate_ValueId.json"));

                foreach (var step in processData)
                {
                    string title = step["title"].ToString();
                    Console.WriteLine($"步骤：{title}");

                    JArray fields = (JArray)step["fields"];
                    foreach (var field in fields)
                    {
                        string name = field["name"].ToString();
                        string originalValue = field["testValue"].ToString();
                        // 如果 originalValue 为空则跳过该字段
                        if (string.IsNullOrEmpty(originalValue))
                        {
                            continue;
                        }
                        string testValue;
                        if (a == 1)
                        {
                            int numericValue = int.Parse(originalValue) + 1;
                            testValue = numericValue.ToString();
                        }
                        else
                        {
                            int numericValue = int.Parse(originalValue) - 1;
                            if (numericValue < 0)
                            {
                                numericValue = int.Parse(originalValue);
                            }
                            testValue = numericValue.ToString();
                        }
                        string automationId = field["automationId"].ToString();

                        if (!string.IsNullOrEmpty(testValue))
                        {
                            var testBox = _mainWindow.FindFirstDescendant(_cf.ByAutomationId($"{automationId}")).AsTextBox();
                            Assert.IsNotNull(testBox, $"未找到{title}_{name}控件。");
                            testBox.Enter(testValue);
                            Thread.Sleep(200);
                        }
                        else
                        {
                            var buttonBox = _mainWindow.FindFirstDescendant(_cf.ByAutomationId($"{automationId}")).AsTextBox();
                            Assert.IsNotNull(buttonBox, $"未找到{title}_{name}控件。");
                            buttonBox.Click();
                        }
                    }

                    var nextButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("EditProcess_NextStepButton")).AsButton();
                    if (nextButton != null)
                    {
                        nextButton.Click();
                        Thread.Sleep(500);
                    }
                    else
                    {
                        UiActions.ClickElementByName(_mainWindow, _cf, "保存配方");
                        Thread.Sleep(1000);
                    }

                }

                //// 获取主窗口
                //_mainWindow = _app.GetMainWindow(_automation);
                //Assert.IsNotNull(_mainWindow, "未能获取主窗口");

                var newProcessTextElement = _mainWindow.FindFirstDescendant(_cf.ByName($"{testName}"));
                Assert.IsNotNull(newProcessTextElement, $"新增失败：在页面上未找到名为 '{testName}' 的元素");
                //var newProcessTextElement = _mainWindow.FindFirstDescendant(_cf.ByName($"PFng"));
                //Assert.IsNotNull(newProcessTextElement, $"新增失败：在页面上未找到名为 'PFng' 的元素");
                // ---------------------------------------查看---------------------------------------------
                var row2 = UiActions.FindRecipeRowByName(_mainWindow, _cf, $"{testName}");
                //var row2 = FindRecipeRowByName($"PFng");

                var ProcessEditElement2 = row2.FindFirstDescendant(_cf.ByName($"查看"));
                Assert.IsNotNull(ProcessEditElement2, $"在页面上未找到名为 '查看' 的元素");
                ProcessEditElement2.Click();

                //_mainWindow = _app.GetMainWindow(_automation);
                //Assert.IsNotNull(_mainWindow, "未能找到主窗口");

                var MainScrollViewer = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("ViewRecipe_MainScrollViewer"));

                // 验证样本体积和描述
                var SampleVolume = MainScrollViewer.FindFirstDescendant(_cf.ByAutomationId("ViewRecipe_Basic_SampleVolumeInput")).AsTextBox();
                Assert.IsNotNull(SampleVolume, "未找到样本体积控件");
                string SampleVolumeInput = SampleVolume.Text;
                Assert.AreEqual(SampleVolumeInput, testEditNum, "样本体积数据不一致");
                var Description = MainScrollViewer.FindFirstDescendant(_cf.ByAutomationId("ViewRecipe_Basic_DescriptionInput")).AsTextBox();
                Assert.IsNotNull(Description, "未找到描述控件");
                string DescriptionInput = Description.Text;
                Assert.AreEqual(DescriptionInput, testEditDec, "描述不一致");
                Thread.Sleep(500);

                JArray processViewData = JArray.Parse(File.ReadAllText("ProcessView_ValueId.json"));
                int i = 1;

                foreach (var Step in processViewData)
                {
                    string title = Step["title"].ToString();
                    Console.WriteLine($"步骤：{title}");

                    var stepButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId($"ViewRecipe_StepNav_{i}_Button")).AsButton();
                    stepButton.Click();
                    i++;

                    JArray fields = (JArray)Step["fields"];
                    foreach (var field in fields)
                    {
                        string name = field["name"].ToString();
                        string originalValue = field["testValue"].ToString();
                        // 如果 originalValue 为空则跳过该字段
                        if (string.IsNullOrEmpty(originalValue))
                        {
                            continue;
                        }
                        string testValue;
                        if (a == 1)
                        {
                            int numericValue = int.Parse(originalValue) + 1;
                            testValue = numericValue.ToString();
                        }
                        else
                        {
                            int numericValue = int.Parse(originalValue) - 1;
                            if (numericValue < 0)
                            {
                                numericValue = int.Parse(originalValue);
                            }
                            testValue = numericValue.ToString();
                        }
                        string automationId = field["automationId"].ToString();

                        if (!string.IsNullOrEmpty(testValue))
                        {
                            var testBox = MainScrollViewer.FindFirstDescendant(_cf.ByAutomationId($"{automationId}")).AsTextBox();
                            Assert.IsNotNull(testBox, $"未找到{title}_{name}控件。");
                            string testBox_Actual = testBox.Text;
                            Assert.AreEqual(testBox_Actual, testValue, $"{title}_{name}不一致");
                            Thread.Sleep(100);
                        }
                        else
                        {
                            var ButtonBox = _mainWindow.FindFirstDescendant(_cf.ByAutomationId($"{automationId}")).AsButton();
                            Assert.IsNotNull(ButtonBox, $"未找到{title}_{name}控件。");
                            ButtonBox.Click();
                        }
                    }
                }
                var CloseButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("ViewRecipe_CloseButton"));
                Assert.IsNotNull(CloseButton, "没有找到关闭按钮");
                CloseButton.Click();
            }
        }
    }
}