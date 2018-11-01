using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Imi.SupplyChain.Deployment.UI
{
    public partial class AskForPasswordForm : Form
    {
        public AskForPasswordForm()
        {
            InitializeComponent();
        }

        public static bool Show(Form parent, out string certificatePassword)
        {
            using (AskForPasswordForm askForPassword = new AskForPasswordForm())
            {
                askForPassword.Owner = parent;

                certificatePassword = string.Empty;

                if (askForPassword.ShowDialog() == DialogResult.OK)
                {
                    certificatePassword = askForPassword.tbCertificatePassword.Text;
                    return true;
                }
                return false;
            }
        }

        private void cbShowHidePassword_CheckedChanged(object sender, EventArgs e)
        {
            UpdateShowHidePassword();
        }

        private void UpdateShowHidePassword()
        {
            tbCertificatePassword.UseSystemPasswordChar = !cbShowHidePassword.Checked;
        }

        private void AskForPassword_Load(object sender, EventArgs e)
        {
            UpdateShowHidePassword();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
