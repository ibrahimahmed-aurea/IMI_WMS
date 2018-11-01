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
    public partial class AddLabelToGroupbox : Form
    {
        public string Caption { get; private set; }

        public AddLabelToGroupbox()
        {
            InitializeComponent();
        }

        private void AddLabelToGroupbox_Load(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void tbCaption_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnOK.Enabled = false;

            if (!string.IsNullOrEmpty(tbCaption.Text))
                btnOK.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!NamingGuidance.CheckCaptionFocus(tbCaption, "Label Caption", true))
                return;

            Caption = tbCaption.Text.Trim();

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
