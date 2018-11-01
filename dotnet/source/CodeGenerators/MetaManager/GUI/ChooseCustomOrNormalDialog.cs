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
    public partial class ChooseCustomOrNormalDialog : Form
    {
        public bool OverviewDialogSelected 
        {
            get
            {
                return rbtnOverview.Checked;
            }
        }

        public ChooseCustomOrNormalDialog()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {


            DialogResult = DialogResult.OK;
        }
    }
}
