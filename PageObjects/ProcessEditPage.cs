using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using Tests.Helpers;
using System.IO;

namespace ProcessEdit.Page
{
    public class ProcessEditPage
    {
        private readonly ConditionFactory _cf;
        private Window _mainWindow;

        public ProcessEditPage(Window mainWindow, ConditionFactory cf)
        {
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public void BaseMation(string testEditNum, string testEditDec)
        {
            var createUserDialog = _mainWindow.FindFirstDescendant(_cf.ByName("编辑配方"))?.AsWindow();
            Assert.IsNotNull(createUserDialog, "未能找到'编辑配方'弹窗");

            var SampleVolumeInput = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("EditProcess_SampleVolumeInput")).AsTextBox();
            Assert.IsNotNull(SampleVolumeInput, "未找到样本体积控件。");
            SampleVolumeInput.Enter(testEditNum.ToString());
            Thread.Sleep(200);

            var DescriptionInput = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("EditProcess_DescriptionInput")).AsTextBox();
            Assert.IsNotNull(DescriptionInput, "未找到配方描述。");
            DescriptionInput.Enter(testEditDec);
            Thread.Sleep(200);

            var nextButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("EditProcess_NextStepButton")).AsButton();
            Assert.IsNotNull(nextButton, "没找到下一步");
            nextButton.Click();
        }

        public void ViewStep(int a) {
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

        }

        public void EditData(int a,string testEditNum,string testEditDec) {

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
        }
    }
}
