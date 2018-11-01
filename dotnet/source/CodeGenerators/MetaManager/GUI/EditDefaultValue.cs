using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cdc.MetaManager.GUI
{
    public partial class EditDefaultValue : Form
    {
        public string DefaultValue { get; set; }

        public EditDefaultValue()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DefaultValue = tbDefaultValue.Text.Trim();
            DialogResult = DialogResult.OK;
        }

        private void EditDefaultValue_Load(object sender, EventArgs e)
        {
            tbDefaultValue.Text = DefaultValue;
        }
    }
}
