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
    public partial class KeyValueEditor : MdiChildForm
    {
        public string CaptionDialog { private get; set; }
        public string CaptionKeyColumn { private get; set; }
        public string CaptionValueColumn { private get; set; }
        public int MinimumKeysEntered { private get; set; }

        public IDictionary<string, string> KeyValues 
        {
            get
            {
                Dictionary<string, string> keyValues = new Dictionary<string,string>();

                for (int i = 0; i < dgvKeyValues.Rows.Count; i++)
                {
                    if (!dgvKeyValues.Rows[i].IsNewRow)
                        keyValues.Add((string)dgvKeyValues[0, i].Value, (string)dgvKeyValues[1, i].Value);
                }

                return keyValues;
            }
        }

        public KeyValueEditor()
        {
            InitializeComponent();
        }

        public void PopulateList(IDictionary<string, string> keyValues)
        {
            // Clear rows
            dgvKeyValues.Rows.Clear();

            if (keyValues != null)
            {
                foreach (KeyValuePair<string, string> keyValue in keyValues)
                {
                    AddRow(keyValue.Key, keyValue.Value);
                }
            }
        }

        public void PopulateList(IList<string> keys, IList<string> values)
        {
            // Clear rows
            dgvKeyValues.Rows.Clear();

            if (keys != null &&
                keys.Count > 0 &&
                values != null &&
                values.Count > 0)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    // Only add values up until there are no more keys or values
                    if (i >= values.Count)
                    {
                        break;
                    }

                    AddRow(keys[i], values[i]);
                }
            }
        }

        private void AddRow(string key, string value)
        {
            // Add the row
            int index = dgvKeyValues.Rows.Add();

            // Set the data on the new row
            dgvKeyValues[0, index].Value = key;
            dgvKeyValues[1, index].Value = value;
        }

        private void KeyValueEditor_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CaptionDialog))
            {
                this.Text = CaptionDialog;
            }

            if (!string.IsNullOrEmpty(CaptionKeyColumn))
            {
                dgvKey.HeaderText = CaptionKeyColumn;
            } 

            if (!string.IsNullOrEmpty(CaptionValueColumn))
            {
                dgvValue.HeaderText = CaptionValueColumn;
            }
            btnOK.Enabled = this.IsEditable;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Check rowcount - 1 since the last one is the "new" row that is empty.
            if (dgvKeyValues.RowCount - 1 < MinimumKeysEntered)
            {
                MessageBox.Show(string.Format("You need to enter a minimum of {0} row(s).", MinimumKeysEntered), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void dgvKeyValues_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Check row contents that both Key and Value is entered. Don't check new rows.
            if (!dgvKeyValues.Rows[e.RowIndex].IsNewRow &&
                (string.IsNullOrEmpty((string)dgvKeyValues[0, e.RowIndex].Value) ||
                 string.IsNullOrEmpty((string)dgvKeyValues[1, e.RowIndex].Value)))
            {
                MessageBox.Show("You need to enter values in both columns before you are able to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }

            // Check row that key is unique, don't include new rows
            if (!dgvKeyValues.Rows[e.RowIndex].IsNewRow)
            {
                IList<string> uniqueList = AllKeys(e.RowIndex);

                if (uniqueList.Count(key => key.ToUpper() == ((string)dgvKeyValues[0, e.RowIndex].Value).ToUpper()) > 0)
                {
                    MessageBox.Show(string.Format("The {0} is not unique. Please enter another value!", dgvKey.HeaderText), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }
            }
        }

        private IList<string> AllKeys(int ignoreRowIndex)
        {
            List<string> allKeys = new List<string>();

            for (int i = 0; i < dgvKeyValues.Rows.Count; i++)
            {
                if (!dgvKeyValues.Rows[i].IsNewRow &&
                    i != ignoreRowIndex)
                {
                    allKeys.Add((string)dgvKeyValues[0, i].Value);
                }
            }

            return allKeys;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
