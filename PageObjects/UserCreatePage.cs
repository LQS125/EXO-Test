using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Tests.Helpers;

namespace UserCreate.PageObjects
{
    public class UserCreatePage
    {
        private readonly ConditionFactory _cf;
        private Window _mainWindow;

        public UserCreatePage(Window mainWindow, ConditionFactory cf)
        {
            _mainWindow = mainWindow;
            _cf = cf ;
        }

        public void FillBasicInfo(string name, string displayName, string password, string phone, string email)
        {
            EnterText("CreateUser_UsernameInput", name);
            EnterText("CreateUser_DisplayNameInput", displayName);
            EnterText("CreateUser_PasswordInput", password);
            EnterText("CreateUser_ConfirmPasswordInput", password);
            EnterText("CreateUser_PhoneInput", phone);
            EnterText("CreateUser_EmailInput", email);
        }

        private void EnterText(string automationId, string text)
        {
            var textBox = _mainWindow.FindFirstDescendant(_cf.ByAutomationId(automationId)).AsTextBox();
            Assert.IsNotNull(textBox, $"未找到字段 {automationId}");
            textBox.Enter(text);
            Thread.Sleep(500);
        }

        public void SelectRole(string roleName)
        {
            var roleCombo = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("CreateUser_RoleSelect")).AsComboBox();
            Assert.IsNotNull(roleCombo, "未找到角色下拉框");
            roleCombo.Expand();
            UiActions.ClickElementByName(_mainWindow, _cf, $"{roleName}");
            Thread.Sleep(1000);

        }

        public void ToggleAlarmPush(bool? expectedChecked = true)
        {
            var checkbox = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("CreateUser_AlarmPushCheck")).AsCheckBox();
            if (checkbox != null && checkbox.IsChecked != expectedChecked)
                checkbox.Click();
        }

        public void ConfirmAndWait()
        {
            var confirmBtn = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("CreateUser_ConfirmButton")).AsButton();
            Assert.IsNotNull(confirmBtn, "未找到确定按钮");
            if (!confirmBtn.IsEnabled)
                Assert.Fail("确定按钮不可用");
            confirmBtn.Click();
        }

    }
}
