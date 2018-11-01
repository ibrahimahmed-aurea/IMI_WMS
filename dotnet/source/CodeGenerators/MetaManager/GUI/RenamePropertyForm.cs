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
    public partial class RenamePropertyForm : Form
    {
        public int MaxPropertyLength { get; set; }

        public string FormCaption { get; set; }
        public string OldName { private get; set; }
        public string NewName { get; private set; }
        public string NameSuffix { private get; set; }
        public bool NameUpperCase { private get; set; }

        public RenamePropertyForm()
        {
            InitializeComponent();

            MaxPropertyLength = 0;

            OldName = string.Empty;
            NewName = string.Empty;
            FormCaption = string.Empty;
            NameSuffix = string.Empty;
            NameUpperCase = false;
        }

        private void RenamePropertyForm_Load(object sender, EventArgs e)
        {
            tbNewName.MaxLength = MaxPropertyLength;
            tbOldName.Text = OldName;
            tbNewName.Text = OldName;

            if (!string.IsNullOrEmpty(FormCaption))
                this.Text = FormCaption;

            if (NameUpperCase)
            {
                tbNewName.Text = tbNewName.Text.ToUpper();
                tbNewName.CharacterCasing = CharacterCasing.Upper;
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (!NamingGuidance.CheckMappedPropertyName(tbNewName.Text.Trim(), true))
            {
                tbNewName.SelectAll();
                tbNewName.Focus();
                return;
            }

            if (MaxPropertyLength > 0 && 
                tbNewName.Text.Trim().Length > MaxPropertyLength)
            {
                MessageBox.Show("The new name is too long! Maximum " + MaxPropertyLength.ToString() + " characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrEmpty(NameSuffix) &&
                !tbNewName.Text.Trim().EndsWith(NameSuffix, false, null))
            {
                MessageBox.Show(string.Format("The new name is missing the suffix (\"{0}\")!", NameSuffix), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NewName = tbNewName.Text.Trim();

            DialogResult = DialogResult.OK;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void tbOldName_TextChanged(object sender, EventArgs e)
        {
            if (MaxPropertyLength > 0)
                lblOldNameChars.Text = string.Format("[{0} / {1}]", tbOldName.Text.Trim().Length.ToString(), MaxPropertyLength);
            else
                lblOldNameChars.Text = string.Format("[{0}]", tbOldName.Text.Trim().Length.ToString());
        }

        private void tbNewName_TextChanged(object sender, EventArgs e)
        {
            if (MaxPropertyLength > 0)
                lblNewNameChars.Text = string.Format("[{0} / {1}]", tbNewName.Text.Trim().Length.ToString(), MaxPropertyLength);
            else
                lblNewNameChars.Text = string.Format("[{0}]", tbNewName.Text.Trim().Length.ToString());
        }
    }
}
