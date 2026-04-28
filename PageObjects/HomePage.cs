//using FlaUI.Core.AutomationElements;
//using FlaUI.Core.Conditions;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Threading;
//using Tests.Helpers;
//using static System.Net.Mime.MediaTypeNames;

//namespace Home.Page
//{
//    public class HomePage
//    {
//        private readonly ConditionFactory _cf;
//        private Window _mainWindow;

//        public HomePage(Window mainWindow, ConditionFactory cf)
//        {
//            _mainWindow = mainWindow;
//            _cf = cf;
//        }

//        public void FillBasicInfo(string testName, string testNum, string test)
//        {
//            EnterText("AddProcess_RecipeNameInput", testName);
//            EnterText("AddProcess_SampleVolumeInput", testNum);
//            EnterText("AddProcess_DescriptionInput", test);
//        }

//        private void EnterText(string automationId, string text)
//        {
//            var SampleVolumeInput = _mainWindow.FindFirstDescendant(_cf.ByAutomationId(automationId)).AsTextBox();
//            Assert.IsNotNull(SampleVolumeInput, "未找到样本体积控件。");
//            SampleVolumeInput.Enter(text.ToString());
//            Thread.Sleep(200);
//        }

//    }
//}
