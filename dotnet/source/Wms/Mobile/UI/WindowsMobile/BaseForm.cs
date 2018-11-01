using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;

namespace Imi.Wms.Mobile.UI
{
    public class BaseForm : Form
    {
        public BaseForm()
        {
            if (SystemSettings.Platform == WinCEPlatform.WinCEGeneric)
            {
                WindowState = FormWindowState.Maximized;
            }
        }
    }
}
