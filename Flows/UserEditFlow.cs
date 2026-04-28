using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using Tests.Helpers;
using UserEdit.PageObjects;


namespace UserEdit.Flows
{
    public class UserEditFlows
    {
        private readonly Application _app;
        private readonly UIA3Automation _automation;
        private readonly Window _mainWindow;
        private readonly ConditionFactory _cf;

        public UserEditFlows(Application app, UIA3Automation automation, Window mainWindow, ConditionFactory cf)
        {
            _app = app;
            _automation = automation;
            _mainWindow = mainWindow;
            _cf = cf;
        }
        public void EditUser() {
            // 生成随机新数据
            string newName = CommonHelper.RandomString(3, 9);
            string newDisplayName = "Disp_" + CommonHelper.RandomString(3, 6);
            string newEmail = CommonHelper.RandomString(3, 6) + "@test.com";
            string newPhone = $"1{CommonHelper._random.Next(100000000, 999999999)}";
            string[] roles = { "管理员", "超级管理员", "工艺员", "操作员" };
            string selectedRole = roles[CommonHelper._random.Next(roles.Length)];
            Console.WriteLine($"创建用户: {newName}, {newDisplayName}, {newEmail}, {newPhone},{selectedRole}");

            AutomationElement firstRowBefore = UiActions.FindFirstdescendant(_mainWindow, _cf);

            // 在第一行内查找编辑按钮
            var editButton = firstRowBefore.FindFirstDescendant(_cf.ByAutomationId("EditButton"));
            if (editButton == null)
                editButton = firstRowBefore.FindFirstDescendant(_cf.ByName("编辑"));
            Assert.IsNotNull(editButton, "未找到编辑按钮");
            editButton.Click();

            // 处理确认弹窗
            var editDialog = _mainWindow.FindFirstDescendant(_cf.ByName("编辑用户"))?.AsWindow();
            Assert.IsNotNull(editDialog, "未出现编辑弹窗"); ;
            Thread.Sleep(1000);

            // 生成对象
            var EditDialog = new UserEditPage(_mainWindow, _cf);
            string originalName = EditDialog.EditName(newName);
            EditDialog.FillEditInfo(newDisplayName, newEmail, newPhone);
            EditDialog.EditUser(selectedRole);
            EditDialog.OkButton();

            var confirmDialog2 = _mainWindow.FindFirstDescendant(_cf.ByName("成功"))?.AsWindow();
            Assert.IsNotNull(confirmDialog2, "未出现成功弹窗"); ;
            Thread.Sleep(1000);
            var okButton2 = confirmDialog2.FindFirstDescendant(_cf.ByName("确定"));
            okButton2.Click();

            // 验证修改成功
            Thread.Sleep(2000);
            var gridConditionAfter = _cf.ByAutomationId("UserList_DataGrid").Or(_cf.ByControlType(ControlType.DataGrid));
            var userGridAfter = _mainWindow.FindFirstDescendant(gridConditionAfter);
            Assert.IsNotNull(userGridAfter, "未找到用户列表表格控件");
            // 获取第一行所有文本单元格
            var firstRowAfter = userGridAfter.FindFirstDescendant(c => c.ByControlType(ControlType.DataItem));
            var cellsAfter = firstRowAfter.FindAllDescendants(c => c.ByControlType(ControlType.Text));
            string userNameAfter = cellsAfter[0].Name;          // 用户名（应不变）
            string displayNameAfter = cellsAfter[1].Name;
            string emailAfter = cellsAfter[4].Name;
            string phoneAfter = cellsAfter[3].Name;
            string roleAfter = cellsAfter[2].Name;
            // 验证：第一行的用户名与编辑前相同（确保是同一个用户）
            Assert.AreEqual(originalName, userNameAfter, "编辑后第一行的用户名不匹配，可能不是原用户");
            Assert.AreEqual(newDisplayName, displayNameAfter, "显示名修改失败");
            Assert.AreEqual(newEmail, emailAfter, "邮箱修改失败");
            Assert.AreEqual(newPhone, phoneAfter, "手机号修改失败");
            Assert.AreEqual(selectedRole, roleAfter, "角色修改失败");
        }
    }
}