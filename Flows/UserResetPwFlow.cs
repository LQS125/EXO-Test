using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Helpers;
using UserResetPw.PageObjects;

namespace UserResetPw.Flows
{
    public class UserResetPwFlows
    {
        private readonly Application _app;
        private readonly UIA3Automation _automation;
        private readonly Window _mainWindow;
        private readonly ConditionFactory _cf;

        public UserResetPwFlows(Application app, UIA3Automation automation, Window mainWindow, ConditionFactory cf)
        {
            _app = app;
            _automation = automation;
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public string ResetPw() {
            AutomationElement firstRow = UiActions.FindFirstdescendant(_mainWindow, _cf);

            var cells = firstRow.FindAllDescendants(c => c.ByControlType(ControlType.Text));
            string userName = cells[0].Name;

            // 在第一行内查找编辑按钮
            var editButton = firstRow.FindFirstDescendant(_cf.ByAutomationId("UserList_ResetPasswordButton"));
            if (editButton == null)
                editButton = firstRow.FindFirstDescendant(_cf.ByName("重置密码"));
            Assert.IsNotNull(editButton, "未找到重置按钮");
            editButton.Click();

            var resetPw = new UserResetPwPage(_mainWindow, _cf);
            resetPw.ResetPw();

            var gridConditionAfter = _cf.ByAutomationId("UserList_DataGrid").Or(_cf.ByControlType(ControlType.DataGrid));

            var userGridAfter = _mainWindow.FindFirstDescendant(gridConditionAfter);
            Assert.IsNotNull(userGridAfter, "未找到用户列表表格控件");

            var firstRowAfter = userGridAfter.FindFirstDescendant(c => c.ByControlType(ControlType.DataItem));
            var cellsAfter = firstRowAfter.FindAllDescendants(c => c.ByControlType(ControlType.Text));
            string userNameAfter = cellsAfter[0].Name;
            Assert.IsNotNull(userNameAfter, "未找到用户列表表格控件");

            return userName;
        }
    }
}
