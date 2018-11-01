using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using Cdc.MetaManager.DataAccess;


namespace Cdc.MetaManager.GUI
{
    public partial class AddComponentToGroupbox : MdiChildForm
    {
        public UXComponent Component { get; set; }
        public List<string> ComponentNames { private get; set; }

        public string DefaultLabelText{ get; set; }
        
        public string LabelText
        {
            get
            {
                return tbLabelText.Text;
            }
        }

        public AddComponentToGroupbox()
        {
            InitializeComponent();
        }

        private void AddComponentToGroupbox_Load(object sender, EventArgs e)
        {
            tbLabelText.Text = DefaultLabelText;
            EnableDisableButtons();
        }
       
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!NamingGuidance.CheckCaptionFocus(tbLabelText, "Label Caption", true))
                return;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void tbLabelText_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnOK.Enabled = false;

            if (Component != null &&
                !string.IsNullOrEmpty(tbLabelText.Text))
            {
                btnOK.Enabled = true;
            }
        }

        private void AddComponentToGroupbox_Shown(object sender, EventArgs e)
        {
            tbLabelText.Focus();
        }
    }
}
