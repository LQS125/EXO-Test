using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using LoginHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Tests.Helpers;
using UserUpdateRole.Page;

namespace UserUpdateRole.Flows
{
    public class UserUpdateRoleFlows
    {
        private readonly Application _app;
        private readonly UIA3Automation _automation;
        private readonly Window _mainWindow;
        private readonly ConditionFactory _cf;
        private AutomationElement _window;

        public UserUpdateRoleFlows(Application app, UIA3Automation automation, 
            Window mainWindow, ConditionFactory cf, AutomationElement window)
        {
            _app = app;
            _automation = automation;
            _mainWindow = mainWindow;
            _cf = cf;
            _window = window;
        }

        public void UpdataUserrole()
        {
            // 定义角色及其预期结果
            var roles = new[]
            {
                new { Name = "管理员",   ShouldHaveMenu = true },
                new { Name = "超级管理员", ShouldHaveMenu = true },
                new { Name = "工艺员",    ShouldHaveMenu = true },
                new { Name = "操作员",    ShouldHaveMenu = false }
            };

            // 循环遍历每个角色
            foreach (var role in roles)
            {
                // 导航到用户列表
                UiActions.ClickElementByName(_mainWindow,_cf,"设置");
                Thread.Sleep(500);
                UiActions.ClickElementByName(_mainWindow, _cf, "用户管理");
                Thread.Sleep(500);
                UiActions.ClickElementByName(_mainWindow, _cf, "用户列表");
                Thread.Sleep(1000);

                AutomationElement firstRowBefore = UiActions.FindFirstdescendant(_mainWindow, _cf);

                // 在第一行内查找编辑按钮
                var editButton = firstRowBefore.FindFirstDescendant(_cf.ByAutomationId("EditButton"));
                if (editButton == null)
                    editButton = firstRowBefore.FindFirstDescendant(_cf.ByName("编辑"));
                Assert.IsNotNull(editButton, "未找到编辑按钮");
                editButton.Click();

                // 获取用户账号
                Thread.Sleep(1000);
                string name = role.Name;

                var createDialog = new UserSelectRolePage(_mainWindow, _cf);
                string userName = createDialog.SelectRole(name);

                // 2. 退出登录
                loginHelper.Logout(_app, _automation, _mainWindow, _cf);

                // 3. 重新登录
                loginHelper.Login(_app, _automation, _mainWindow, _cf, userName);

                // 4. 等待主窗口出现
                var mainWindow = _app.GetMainWindow(_automation);
                Assert.IsNotNull(mainWindow, "未能获取主窗口");
                Thread.Sleep(2000);

                // 5. 检查“设置”菜单是否存在
                _window = _app.GetMainWindow(_automation);
                var userManagement = _window.FindFirstDescendant(_cf.ByName("设置"));
                bool isMenuPresent = (userManagement != null);
                // 6. 断言结果与预期一致
                Assert.AreEqual(role.ShouldHaveMenu, isMenuPresent,
                    $"角色 '{role.Name}' 的用户管理菜单可见性错误：期望 {role.ShouldHaveMenu}，实际 {isMenuPresent}");

                Thread.Sleep(500);
            }
            loginHelper.Logout(_app, _automation, _mainWindow, _cf);
            loginHelper.LoginAdmin(_app, _automation, _mainWindow, _cf);
        }
    }
}