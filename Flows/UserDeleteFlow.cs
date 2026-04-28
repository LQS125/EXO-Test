using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using Tests.Helpers;
using UserCreate.PageObjects;
using UserDelete.PageObjects;
using UserEdit.PageObjects;

namespace UserDelete.Flows
{
    public class UserDeleteFlows
    {
        private readonly Application _app;
        private readonly UIA3Automation _automation;
        private readonly Window _mainWindow;
        private readonly ConditionFactory _cf;

        public UserDeleteFlows(Application app, UIA3Automation automation, Window mainWindow, ConditionFactory cf)
        {
            _app = app;
            _automation = automation;
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public void UserDelete()
        {
            // 导航到用户列表
            UiActions.ClickElementByName(_mainWindow, _cf,"设置");
            Thread.Sleep(500);
            UiActions.ClickElementByName(_mainWindow, _cf, "用户管理");
            Thread.Sleep(500);
            UiActions.ClickElementByName(_mainWindow, _cf, "用户列表");
            Thread.Sleep(1000);

            AutomationElement firstRowBefore = UiActions.FindFirstdescendant(_mainWindow, _cf);

            var cellsBefore = firstRowBefore.FindAllDescendants(c => c.ByControlType(ControlType.Text));
            string userNameBefore = cellsBefore[0].Name;
            Assert.IsNotNull(userNameBefore, "未找到删除前用户姓名");

            // 在第一行内查找删除按钮
            var deleteButton = firstRowBefore.FindFirstDescendant(_cf.ByAutomationId("DeleteButton"));
            if (deleteButton == null)
                deleteButton = firstRowBefore.FindFirstDescendant(_cf.ByName("删除"));
            Assert.IsNotNull(deleteButton, "未找到删除按钮");

            string beforeUserName = firstRowBefore?.FindFirstDescendant(c => c.ByControlType(ControlType.Text))?.Name ?? "";
            Assert.IsNotNull(beforeUserName, "未找到用户姓名");

            // 点击删除
            deleteButton.Click();
            Thread.Sleep(1000);

            var userDelete = new UserDeletePage(_mainWindow, _cf);
            string userNameAfter = userDelete.DeleteUser();

            Assert.IsNotNull(userNameAfter, "未找到删除后用户姓名");
            Assert.AreNotEqual(beforeUserName, userNameAfter, $"删除失败");
        }
    }
}