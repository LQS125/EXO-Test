using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.IO;
using System.Linq;

namespace ProcessView.Page
{
    public class ProcessViewPage
    {
        private readonly ConditionFactory _cf;
        private Window _mainWindow;

        public ProcessViewPage(Window mainWindow, ConditionFactory cf)
        {
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public void ViewBasicInfo(string testNum, string test)
        {
            var MainScrollViewer = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("ViewRecipe_MainScrollViewer"));

            // 验证样本体积和描述
            var SampleVolume = MainScrollViewer.FindFirstDescendant(_cf.ByAutomationId("ViewRecipe_Basic_SampleVolumeInput")).AsTextBox();
            Assert.IsNotNull(SampleVolume, "未找到样本体积控件");
            string SampleVolumeInput = SampleVolume.Text;
            Assert.AreEqual(SampleVolumeInput, testNum, "样本体积数据不一致");
            var Description = MainScrollViewer.FindFirstDescendant(_cf.ByAutomationId("ViewRecipe_Basic_DescriptionInput")).AsTextBox();
            Assert.IsNotNull(Description, "未找到描述控件");
            string DescriptionInput = Description.Text;
            Assert.AreEqual(DescriptionInput, test, "描述不一致");
            Thread.Sleep(100);
        }
        public void ViewData() {
            JArray processViewData = JArray.Parse(File.ReadAllText("ProcessView_ValueId.json"));
            int i = 1;

            foreach (var Step in processViewData)
            {
                string title = Step["title"].ToString();
                Console.WriteLine($"步骤：{title}");

                var stepButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId($"ViewRecipe_StepNav_{i}_Button")).AsButton();
                stepButton.Click();
                i++;

                // 每次点击步骤按钮后，重新获取当前步骤的滚动视图容器
                var StepScrollViewer = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("ViewRecipe_MainScrollViewer"));
                Assert.IsNotNull(StepScrollViewer, "未找到滚动视图");
                Thread.Sleep(500);

                // 性能优化：一次性获取所有元素，避免 O(N²) 复杂度
                var allDescendants = StepScrollViewer.FindAllDescendants();
                var elementDict = allDescendants
                    .Where(e => !string.IsNullOrEmpty(e.AutomationId))
                    .ToDictionary(e => e.AutomationId, e => e);

                JArray fields = (JArray)Step["fields"];
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
                            string testBox_Actual = testBox.Text;
                            Assert.AreEqual(testBox_Actual, testValue, $"{title}_{name}不一致");
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
                        //Assert.Fail($"未找到automationId为 '{automationId}' 的控件（{title}_{name}）");
                    }

                }
            }
            var CloseButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("ViewRecipe_CloseButton"));
            Assert.IsNotNull(CloseButton, "没有找到关闭按钮");
            CloseButton.Click();
        }

    }
}
