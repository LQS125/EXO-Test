using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProcessCreate.Flows;
using ProcessEdit.Flows;
using ProcessView.Flows;
using UserManageTest.Base;
using static System.Net.Mime.MediaTypeNames;

namespace ProcessManage.Tests
{
    [TestClass]
    public class TotalProcessTest : TestBase
    {
        [TestMethod]
        public void Test_TotalProcess()
        {
            var processCreate = new ProcessCreateFlows(_app, _automation, _mainWindow, _cf);
            (string testName, string testNum, string test) = processCreate.CreateProcess();

            _mainWindow = _app.GetMainWindow(_automation);
            Assert.IsNotNull(_mainWindow, "未能获取主窗口");

            var newProcessTextElement = _mainWindow.FindFirstDescendant(_cf.ByName($"{testName}"));
            Assert.IsNotNull(newProcessTextElement, $"新增失败：在页面上未找到名为 '{testName}' 的元素");

            var processView = new processViewFlows(_app, _automation, _mainWindow, _cf);
            processView.ViewProcess(testName, testNum, test);

            var processEdit = new ProcessEditFlows(_app, _automation, _mainWindow, _cf);
            processEdit.EditProcess(testName);
        }
    }
}
