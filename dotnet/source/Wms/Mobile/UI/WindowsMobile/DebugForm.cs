using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Imi.Wms.Mobile.UI
{
    public partial class DebugForm : BaseForm
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

        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}