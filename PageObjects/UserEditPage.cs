using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using Tests.Helpers;


namespace UserEdit.PageObjects
{

    public class UserEditPage
    {
        private readonly ConditionFactory _cf;
        private Window _mainWindow;

        public UserEditPage(Window mainWindow, ConditionFactory cf)
        {
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public void FillEditInfo(string DisplayName, string Email, string Phone)
        {
            EnterText("EditUser_DisplayNameInput", DisplayName);
            EnterText("EditUser_EmailInput", Email);
            EnterText("EditUser_PhoneInput", Phone);
        }

        private void EnterText(string automationId, string text)
        {
            var textBox = _mainWindow.FindFirstDescendant(_cf.ByAutomationId(automationId)).AsTextBox();
            string originalDisplayName = textBox.Text;
            UiActions.ClearAndInput(textBox, text);
            Assert.IsNotNull(textBox, "修改失败");
            Thread.Sleep(1000);
        }

        public string EditName(string name)
        {
            var nameBox = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("EditUser_UsernameDisplay")).AsTextBox();
            Assert.IsNotNull(nameBox, "未找到用户名输入框");
            string originalName = nameBox.Text;
            try
            {
                nameBox.Focus();
                nameBox.Text = name;        // 尝试用代码直接改值/用键盘强行敲入新内容
                nameBox.Enter(name);
            }
            catch (Exception) { }
            Thread.Sleep(500);
            Assert.AreEqual(originalName, nameBox.Text, $"用户名称被成功修改！");
            return originalName;
        }

        public void EditUser(string selectedRole)
        {
            var roleCombo = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("EditUser_RoleSelect")).AsComboBox();
            roleCombo.Expand();
            Thread.Sleep(500);
            var roleItem = _mainWindow.FindFirstDescendant(_cf.ByName(selectedRole));
            Assert.IsNotNull(roleItem, $"在下拉列表中找不到选项：{selectedRole}");
            roleItem.Click(); 
            Thread.Sleep(1000);

        }

        public void OkButton()
        {
            var okButton = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("EditUser_ConfirmButton")).AsButton();
            Assert.IsNotNull(okButton, "okButton");
            okButton.Click();
            Thread.Sleep(1000);
        }

    }
}
