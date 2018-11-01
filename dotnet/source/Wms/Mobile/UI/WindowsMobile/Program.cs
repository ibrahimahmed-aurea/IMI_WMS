using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Imi.Wms.Mobile.UI;

namespace Imi.Wms.Mobile.UI.WindowsMobile
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }
    }
}