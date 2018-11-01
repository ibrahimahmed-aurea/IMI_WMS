using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Imi.SupplyChain.Deployment.Wrappers;
using Imi.SupplyChain.Deployment.Entities;

namespace Imi.SupplyChain.Deployment.UI
{
    public partial class NewInstanceForm : Form
    {
        public ProductStandard Product { get; set; }
        public Instance EditInstance { get; set; }
        public bool ProductVersionChanged { get; private set; }
        private string PreviousProductVersion { get; set; }
        bool? moveFocusForward = null;

        public NewInstanceForm()
        {
            InitializeComponent();
        }

        private VersionWrapper SelectedRunningVersion 
        {
            get
            {
                return (VersionWrapper)cbRunningAppVersion.SelectedItem;
            }
        }

        private void NewInstance_Load(object sender, EventArgs e)
        {
            // Clear the datasource
            versionWrapperBindingSource.DataSource = null;

            // Set product information
            tbProductName.Text = Product.ProductName;

            // Check if editing
            if (EditInstance != null)
            {
                tbInstanceName.Text = EditInstance.Name;
                tbInstanceName.ReadOnly = true;
                tbInstanceName.TabStop = false;

                tbCaption.Text = EditInstance.Caption;
                tbDescription.Text = EditInstance.Description;

                // Save previous version
                PreviousProductVersion = EditInstance.ProductVersion;
            }

            // Set the datasource to the versions.
            versionWrapperBindingSource.DataSource = Product.Versions;

            // Check if editing 
            if (EditInstance != null)
            {
                // Select the current version
                for (int i = 0; i < cbRunningAppVersion.Items.Count; i++)
                {
                    if (EditInstance.VersionPath == ((VersionWrapper)cbRunningAppVersion.Items[i]).Path)
                    {
                        cbRunningAppVersion.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                // Check if only one version exists. In that case select the only version.
                if (Product.Versions.Count == 1)
                {
                    // Select the first in the list
                    cbRunningAppVersion.SelectedIndex = 0;
                }
            }

            PopulateParameters();

            CheckEnableOK();
        }

        private void PopulateParameters()
        {
            // Temp dictionary
            Dictionary<string, string> tempDict = new Dictionary<string, string>();

            // Save all values from grid to be able to reuse them again
            foreach (DataGridViewRow row in dgvParameters.Rows)
            {
                tempDict.Add((string)row.Cells["colName"].Value, (string)row.Cells["colValue"].Value);
            }

            // Clear parameterlist
            dgvParameters.Rows.Clear();

            // Get parameters from product
            if (Product.Parameters.Count > 0)
            {
                foreach (Parameter param in Product.Parameters)
                {
                    // Check if parameter should be shown depending on versions
                    if (!string.IsNullOrEmpty(param.ProductVersionIntroduced) && 
                        VersionHandler.IsVersion(param.ProductVersionIntroduced))
                    {
                        // If selected version is less than the version it was introduced then don't show it
                        if (VersionHandler.VersionStringCompare(SelectedRunningVersion.Name, param.ProductVersionIntroduced) < 0)
                            continue;
                    }

                    if (!string.IsNullOrEmpty(param.ProductVersionLastUsed) &&
                        VersionHandler.IsVersion(param.ProductVersionLastUsed))
                    {
                        // If selected version is greater than the version it was last used then don't show it
                        if (VersionHandler.VersionStringCompare(SelectedRunningVersion.Name, param.ProductVersionLastUsed) > 0)
                            continue;
                    }

                    // Check if we were editing an instance and if the current parameter value could be reused.
                    if (tempDict.ContainsKey(param.Name) && !string.IsNullOrEmpty(tempDict[param.Name]))
                    {
                        AddParameterRow(param.Name, tempDict[param.Name], param.IsMandatory, param.Description);
                    }
                    else if (EditInstance != null && EditInstance.Parameters.ContainsKey(param.Name))
                    {
                        AddParameterRow(param.Name, EditInstance.Parameters[param.Name], param.IsMandatory, param.Description);
                    }
                    else
                    {
                        AddParameterRow(param.Name, param.Default, param.IsMandatory, param.Description);
                    }
                }
            }
        }

        private void AddParameterRow(string key, string value, bool isMandatory, string tooltiptext)
        {
            // Add the row
            int index = dgvParameters.Rows.Add();

            // Set the data on the new row
            dgvParameters["colName", index].Value = key;
            dgvParameters["colName", index].ToolTipText = tooltiptext;
            dgvParameters["colMandatory", index].Value = isMandatory;
            dgvParameters["colMandatory", index].ToolTipText = tooltiptext;
            dgvParameters["colValue", index].Value = value;
            dgvParameters["colValue", index].ToolTipText = tooltiptext;
        }

        private void SetParameters(Instance instance)
        {
            // Clear all old parameters
            instance.Parameters.Clear();

            // Save all values from grid to be able to reuse them again
            foreach (DataGridViewRow row in dgvParameters.Rows)
            {
                instance.Parameters.Add((string)row.Cells["colName"].Value, (string)row.Cells["colValue"].Value);
            }
        }

        private void tbInstanceName_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }

        private void tbCaption_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }

        private void cbRunningAppVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateParameters();

            CheckEnableOK();
        }

        private void tbClickOnceVersion_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }

        private void CheckEnableOK()
        {
            btnOk.Enabled = false;

            if (AllParametersOK() &&
                !string.IsNullOrEmpty(tbInstanceName.Text.Trim()) &&
                !string.IsNullOrEmpty(tbCaption.Text.Trim()) &&
                cbRunningAppVersion.SelectedIndex >= 0)
            {
                btnOk.Enabled = true;
            }
        }

        private bool AllParametersOK()
        {
            bool allOk = true;

            foreach (DataGridViewRow row in dgvParameters.Rows)
            {
                // Find the parameter in the product
                foreach(Parameter parameter in Product.Parameters)
                {
                    // Check if we found the parameter
                    if (parameter.Name == (string)row.Cells["colName"].Value)
                    {
                        string value = (string)row.Cells["colValue"].Value;

                        row.Cells["colValue"].ErrorText = string.Empty;

                        // If not mandatory it's ok if it's empty
                        if (!parameter.IsMandatory)
                        {
                            if (string.IsNullOrEmpty(value))
                                continue;
                            else if (CheckValidationRegEx(value, parameter.ValidationRegEx))
                                continue;
                            else
                            {
                                allOk = false;

                                if (string.IsNullOrEmpty(parameter.ValidationErrorText))
                                    row.Cells["colValue"].ErrorText = "The format of this parameter is invalid.";
                                else
                                    row.Cells["colValue"].ErrorText = parameter.ValidationErrorText;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(value))
                            {
                                allOk = false;
                                row.Cells["colValue"].ErrorText = "This parameter is mandatory.";
                            }
                            else if (string.IsNullOrEmpty(parameter.ValidationRegEx))
                                continue;
                            else if (CheckValidationRegEx(value, parameter.ValidationRegEx))
                                continue;
                            else
                            {
                                allOk = false;

                                if (string.IsNullOrEmpty(parameter.ValidationErrorText))
                                    row.Cells["colValue"].ErrorText = "The format of this parameter is invalid.";
                                else
                                    row.Cells["colValue"].ErrorText = parameter.ValidationErrorText;
                            }
                        }

                        break;
                    }
                }
            }

            return allOk;
        }

        private bool CheckValidationRegEx(string value, string validationRegEx)
        {
            bool result = true;

            if (!string.IsNullOrEmpty(validationRegEx))
            {
                result = Regex.IsMatch(value, validationRegEx, RegexOptions.Singleline);
            }

            return result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!AllParametersOK())
                return;

            if (EditInstance == null)
            {
                // Check if instance already exist with that name
                foreach (Instance instance in Product.Instances)
                {
                    if (instance.Name.ToUpper() == tbInstanceName.Text.Trim().ToUpper())
                    {
                        MessageBox.Show("An instance with this name already exists! Select another name and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbInstanceName.SelectAll();
                        tbInstanceName.Focus();
                        return;
                    }
                }

                EditInstance = new Instance();
                EditInstance.Name = tbInstanceName.Text.Trim();
                EditInstance.DeployManifestFile = string.Format("{0}.application", EditInstance.Name);
                EditInstance.VersionPath = SelectedRunningVersion.Path;
                EditInstance.ProductVersion = SelectedRunningVersion.Name;
                EditInstance.ApplicationManifestFile = SelectedRunningVersion.FoundFile;
                EditInstance.Description = tbDescription.Text.Trim();
                EditInstance.Caption = tbCaption.Text.Trim();

                SetParameters(EditInstance);

                ProductVersionChanged = true;
            }
            else
            {
                // Editing an existing instance
                EditInstance.Description = tbDescription.Text.Trim();
                EditInstance.Caption = tbCaption.Text.Trim();

                SetParameters(EditInstance);

                // Check if product version has changed.
                if (VersionHandler.VersionStringCompare(PreviousProductVersion, SelectedRunningVersion.Name) != 0)
                {
                    EditInstance.VersionPath = SelectedRunningVersion.Path;
                    EditInstance.ProductVersion = SelectedRunningVersion.Name;
                    EditInstance.ApplicationManifestFile = SelectedRunningVersion.FoundFile;

                    ProductVersionChanged = true;
                }
                else
                {
                    ProductVersionChanged = false;
                }
            }

            DialogResult = DialogResult.OK;
        }


        private void tbHostServername_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }

        private void tbHostPort_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }

        private void dgvParameters_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            CheckEnableOK();
        }

        private delegate void DelegateSetCellSelection(int colIndex, int rowIndex);

        private void dgvParameters_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2)
                dgvParameters.BeginInvoke(new DelegateSetCellSelection(SetNextCellSelection), new object[] { 2, e.RowIndex });
        }

        private void SetNextCellSelection(int colIndex, int rowIndex)
        {
            if (moveFocusForward != null)
            {
                if (moveFocusForward == false)
                {
                    if (rowIndex > 0)
                        dgvParameters[colIndex, rowIndex - 1].Selected = true;
                    else
                    {
                        dgvParameters[2, 0].Selected = true;
                        FocusPrevious(dgvParameters);
                    }
                }

                moveFocusForward = null;
            }
            else if (dgvParameters.SelectedCells.Count == 0 ||
                (dgvParameters.SelectedCells.Count > 0 &&
                 dgvParameters.SelectedCells[0].ColumnIndex != 2))
            {
                dgvParameters[colIndex, rowIndex].Selected = true;
            }
        }

        private void FocusPrevious(Control control)
        {
            if (control != null)
            {
                int insanityCount = 0;

                Control tryControl = this.GetNextControl(dgvParameters, false);

                while (insanityCount < 100 && (!tryControl.CanFocus || !tryControl.TabStop))
                {
                    tryControl = this.GetNextControl(tryControl, false);
                    insanityCount++;
                }

                if (insanityCount < 100)
                    tryControl.Select();
            }
        }

        private void dgvParameters_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.KeyCode == Keys.Tab)
                moveFocusForward = false;
        }

        private void tbInstanceName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbInstanceName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(tbInstanceName, "The instance name is mandatory.");
                tbInstanceName.Focus();
            }
            else if (!VerifyString.IsWord(tbInstanceName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(tbInstanceName, "The instance name is not valid. Valid characters are a-z, A-Z, 0-9 and underscore.");
                tbInstanceName.Focus();
            }
        }

        private void tbInstanceName_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(tbInstanceName, string.Empty);
        }

        private void tbCaption_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbCaption.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(tbCaption, "The caption is mandatory.");
                tbCaption.Focus();
            }
        }

        private void tbCaption_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(tbCaption, string.Empty);
        }
    }
}
