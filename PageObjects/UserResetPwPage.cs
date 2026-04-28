using FlaUI.Core.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using FlaUI.Core.AutomationElements;

namespace UserResetPw.PageObjects
{
    public class UserResetPwPage
    {
        private readonly ConditionFactory _cf;
        private Window _mainWindow;

        public UserResetPwPage(Window mainWindow, ConditionFactory cf)
        {
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public void ResetPw()
        {
            var confirmDialog = _mainWindow.FindFirstDescendant(_cf.ByName("确认重置密码"))?.AsWindow();
            Assert.IsNotNull(confirmDialog, "未出现弹窗"); ;
            Thread.Sleep(1000);
            var okButton = confirmDialog.FindFirstDescendant(_cf.ByName("确定"));
            okButton.Click();

            var confirmDialog2 = _mainWindow.FindFirstDescendant(_cf.ByName("成功"))?.AsWindow();
            Assert.IsNotNull(confirmDialog2, "未出现弹窗"); ;
            Thread.Sleep(1000);
            var okButton2 = confirmDialog2.FindFirstDescendant(_cf.ByName("确定"));
            okButton2.Click();
        }
    }
}
