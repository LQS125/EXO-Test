using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Tests.Helpers;

namespace UserUpdateRole.Page
{
    public class UserSelectRolePage
    {
        private readonly ConditionFactory _cf;
        private Window _mainWindow;

        public UserSelectRolePage(Window mainWindow, ConditionFactory cf)
        {
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public string SelectRole(string Name)
        {
            var gridConditionAfter = _cf.ByAutomationId("UserList_DataGrid").Or(_cf.ByControlType(ControlType.DataGrid));
            var userGridAfter = _mainWindow.FindFirstDescendant(gridConditionAfter);
            Assert.IsNotNull(userGridAfter, "未找到用户列表表格控件");
            // 获取第一行所有文本单元格
            var firstRowAfter = userGridAfter.FindFirstDescendant(c => c.ByControlType(ControlType.DataItem));
            var cellsAfter = firstRowAfter.FindAllDescendants(c => c.ByControlType(ControlType.Text));
            string userName = cellsAfter[0].Name;

            // 处理确认弹窗
            var editDialog = _mainWindow.FindFirstDescendant(_cf.ByName("编辑用户"))?.AsWindow();
            Assert.IsNotNull(editDialog, "未出现编辑弹窗"); ;
            Thread.Sleep(1000);

            // 角色（下拉框）
            var roleCombo = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("EditUser_RoleSelect")).AsComboBox();
            roleCombo.Expand();
            Thread.Sleep(500);
            // 寻找名为 selectedRole 的选项
            var roleItem = _mainWindow.FindFirstDescendant(_cf.ByName(Name));
            Assert.IsNotNull(roleItem, $"在下拉列表中找不到选项：{Name}");
            roleItem.Click();
            Thread.Sleep(500);

            // 点击确定
            var okButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("EditUser_ConfirmButton")).AsButton();
            Assert.IsNotNull(okButton, "okButton");
            okButton.Click();
            Thread.Sleep(500);

            var confirmDialog2 = _mainWindow.FindFirstDescendant(_cf.ByName("成功"))?.AsWindow();
            Assert.IsNotNull(confirmDialog2, "未出现成功弹窗"); ;
            Thread.Sleep(500);
            var okButton2 = confirmDialog2.FindFirstDescendant(_cf.ByName("确定"));
            okButton2.Click();

            return userName;
        }

    }
}
