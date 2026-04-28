using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using Tests.Helpers;
using UserCreate.PageObjects;

namespace UserCreate.Flows
{
    public class UserCreateFlows
    {
        private readonly Application _app;
        private readonly UIA3Automation _automation;
        private readonly Window _mainWindow;
        private readonly ConditionFactory _cf;

        public UserCreateFlows(Application app, UIA3Automation automation, Window mainWindow, ConditionFactory cf)
        {
            _app = app;
            _automation = automation;
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public string CreateRandomUser()
        {
            // 生成随机测试数据
            string testName = CommonHelper.GenerateRandomString(3, 11);
            string testDisplayName = "Test_" + CommonHelper.GenerateRandomString(3, 6);
            string testPwd = CommonHelper.GenerateRandomString(8, 11);
            string testPhone = CommonHelper.GenerateRandomPhone();
            string testEmail = CommonHelper.GenerateRandomEmail();

            Console.WriteLine($"创建用户: {testName}, {testDisplayName}, {testPhone}, {testEmail}");

            // 导航到用户列表
            UiActions.ClickElementByName(_mainWindow, _cf, "设置");
            System.Threading.Thread.Sleep(1000);
            UiActions.ClickElementByName(_mainWindow, _cf, "用户管理");
            System.Threading.Thread.Sleep(1000);
            UiActions.ClickElementByName(_mainWindow, _cf, "用户列表");
            System.Threading.Thread.Sleep(1000);
            UiActions.ClickElementByName(_mainWindow, _cf, "新增");
            System.Threading.Thread.Sleep(1000);

            // 获取“创建用户”弹窗对象
            var createUserDialog = _mainWindow.FindFirstDescendant(_cf.ByName("创建用户"))?.AsWindow();
            Assert.IsNotNull(createUserDialog, "未能找到'创建用户'弹窗");

            var createDialog = new UserCreatePage(_mainWindow, _cf);
            createDialog.FillBasicInfo(testName, testDisplayName, testPwd, testPhone, testEmail);

            // 随机选择角色
            string[] roles = { "管理员", "超级管理员", "工艺员", "操作员" };
            Random rand = new Random();
            string selectedRole = roles[rand.Next(roles.Length)];
            createDialog.SelectRole(selectedRole);

            createDialog.ToggleAlarmPush(true);
            createDialog.ConfirmAndWait();

            // 获取“创建用户”弹窗对象
            var createSuccess = _mainWindow.FindFirstDescendant(_cf.ByName("成功"))?.AsWindow();
            Assert.IsNotNull(createUserDialog, "没有创建成功弹窗");
            var successBtn = createSuccess.FindFirstDescendant(_cf.ByName("确定")).AsButton();
            successBtn.Click();
            Thread.Sleep(500);

            var newUserTextElement = _mainWindow.FindFirstDescendant(_cf.ByName($"{testName}"));
            Assert.IsNotNull(newUserTextElement, $"在页面上未找到名为 '{testName}' 的元素");

            return testName;
        }
    }
}