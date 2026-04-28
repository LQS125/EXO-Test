using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Tests.Helpers;
using static System.Net.Mime.MediaTypeNames;

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
    }
}
