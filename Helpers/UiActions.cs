using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;


namespace Tests.Helpers
{

    public static class UiActions
    {
        /// 通过控件的 Name 属性点击元素
        public static void ClickElementByName(Window mainWindow, ConditionFactory cf, string name)
        {
            var element = mainWindow.FindFirstDescendant(cf.ByName(name));
            Assert.IsNotNull(element, $"找不到名为 '{name}' 的UI元素");
            element.Click();
        }

        /// <summary>
        /// 通过 AutomationId 获取输入框并输入文本
        /// </summary>
        public static void EnterTextByAutomationId(Window window, ConditionFactory cf, string automationId, string text, int timeoutSeconds = 5)
        {
            var textBox = window.FindFirstDescendant(cf.ByAutomationId(automationId)).AsTextBox();
            Assert.IsNotNull(textBox, $"未找到 AutomationId 为 '{automationId}' 的输入框");
            textBox.Enter(text);
        }

        // 获取第一行元素
        public static AutomationElement FindFirstdescendant(Window mainWindow, ConditionFactory cf)
        {
            // 获取表格控件
            var gridCondition = cf.ByAutomationId("UserList_DataGrid").Or(cf.ByControlType(ControlType.DataGrid));

            var userGrid = mainWindow.FindFirstDescendant(gridCondition);
            Assert.IsNotNull(userGrid, "未找到用户列表表格控件");

            // 记录编辑前的第一行用户
            var firstRow = userGrid.FindFirstDescendant(c => c.ByControlType(ControlType.DataItem));
            Assert.IsNotNull(firstRow, "表格中没有数据行");

            return firstRow;

        }
        // 编辑用户信息中的清空并输入
        public static void ClearAndInput(TextBox textBox, string newText)
        {
            if (textBox == null) return;
            textBox.Focus();
            textBox.Text = "";
            textBox.Enter(newText);
            Thread.Sleep(1000);
        }

        public static AutomationElement FindRecipeRowByName(Window _mainWindow, ConditionFactory _cf,string recipeName)
        {
            // 定位表格控件
            var recipeGrid = _mainWindow.FindFirstDescendant(_cf.ByAutomationId("RecipeManagement_DataGrid"));
            if (recipeGrid == null)
                throw new Exception("未找到配方表格控件");

            // 获取所有数据行（假设每行的 ControlType 为 DataItem）
            var rows = recipeGrid.FindAllDescendants(c => c.ByControlType(ControlType.DataItem));
            foreach (var row in rows)
            {
                // 获取该行所有文本单元格
                var cells = row.FindAllDescendants(c => c.ByControlType(ControlType.Text));
                // 假设配方名称在第一列（索引 0），可根据实际调整
                if (cells.Length > 0 && cells[0].Name == recipeName)
                {
                    return row;
                }
            }
            return null; // 未找到
        }

    }
}
