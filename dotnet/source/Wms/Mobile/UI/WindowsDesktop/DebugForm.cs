using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Imi.Wms.Mobile.UI
{
    public partial class DebugForm : Form
    {
        private DebugPresenter _presenter;

        public DebugForm()
        {
            InitializeComponent();

            _presenter = new DebugPresenter(this);
        }

        public void ShowDebugInfo(string info)
        {
            debugTextBox.Text = info;
        }
    }
}
