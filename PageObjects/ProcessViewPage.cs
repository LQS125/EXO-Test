using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using Tests.Helpers;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Linq;

namespace ProcessView.Page
{
    public class ProcessViewPage
    {
        private readonly ConditionFactory _cf;
        private Window _mainWindow;

        public ProcessViewPage(Window mainWindow, ConditionFactory cf)
        {
            _mainWindow = mainWindow;
            _cf = cf;
        }

        public void ViewBasicInfo()
        {

        }

    }
}
