using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Tests.Helpers;

namespace UserDelete.PageObjects
{
    public class UserDeletePage
    {
        private readonly ConditionFactory _cf;
        private Window _mainWindow;

        public UserDeletePage(Window mainWindow, ConditionFactory cf)
        {
            _mainWindow = mainWindow;
            _cf = cf;
        }


        public string DeleteUser()
        {
            // 处理确认弹窗
            var confirmDialog1 = _mainWindow.FindFirstDescendant(_cf.ByName("确认删除"))?.AsWindow();
            Assert.IsNotNull(confirmDialog1, "未出现确认删除弹窗"); ;
            Thread.Sleep(1000);
            var okButton1 = confirmDialog1.FindFirstDescendant(_cf.ByName("确定"));
            okButton1?.Click();

            var confirmDialog2 = _mainWindow.FindFirstDescendant(_cf.ByName("成功"))?.AsWindow();
            Assert.IsNotNull(confirmDialog2, "未出现成功弹窗"); ;
            Thread.Sleep(1000);
            var okButton2 = confirmDialog2.FindFirstDescendant(_cf.ByName("确定"));
            okButton2?.Click();

            AutomationElement firstRowAfter = UiActions.FindFirstdescendant(_mainWindow, _cf);
            var cellsAfter = firstRowAfter.FindAllDescendants(c => c.ByControlType(ControlType.Text));
            string userNameAfter = cellsAfter[0].Name;

            return userNameAfter;

        }

    }
}
