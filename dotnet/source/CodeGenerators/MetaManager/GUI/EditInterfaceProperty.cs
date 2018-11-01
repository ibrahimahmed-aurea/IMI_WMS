using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain;


namespace Cdc.MetaManager.GUI
{
    public partial class EditInterfaceProperty : MdiChildForm
    {
        public bool IsRequestProperty { get; set; }
        public IList<MappedProperty> EditMappedProperties { get; set; }
        public IList<UXSessionProperty> SessionProperties { get; set; }
        public bool SearchableParameterUpdated = false;

        private bool DisableDefaultValueEditing { get; set; }
        private Type DefaultValueType { get; set; }
        

        public EditInterfaceProperty()
        {
            InitializeComponent();
        }

        private MappedProperty EditMappedProperty
        {
            get
            {
                if (EditMappedProperties != null && EditMappedProperties.Count == 1)
                    return EditMappedProperties[0];
                else
                    return null;
            }
        }

        private void EditInterfaceProperty_Load(object sender, EventArgs e)
        {
            if (EditMappedProperties != null)
            {
                // Check if we need to do multichecks (more than one property)
                if (EditMappedProperty == null)
                {
                    DefaultValueType = null;

                    // Check datatypes of the properties. If they are not all the same
                    // then editing default value is not possible.
                    foreach (MappedProperty prop in EditMappedProperties)
                    {
                        if (DefaultValueType == null)
                        {
                            DefaultValueType = prop.IsCustom ? prop.Target.Type : prop.Source.Type;
                            continue;
                        }
                        else if (DefaultValueType != (prop.IsCustom ? prop.Target.Type : prop.Source.Type))
                        {
                            DisableDefaultValueEditing = true;
                            DefaultValueType = null;
                            break;
                        }
                    }

                    if (DisableDefaultValueEditing)
                    {
                        rbDefaultNone.Enabled = false;
                        rbDefaultSession.Enabled = false;
                        rbDefaultText.Enabled = false;

                        MessageBox.Show("Since one or more of the selected fields have different datatypes, default values cannot be changed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    DefaultValueType = EditMappedProperty.IsCustom ? EditMappedProperty.Target.Type : EditMappedProperty.Source.Type;
                }

                PopulateSessionProperties(DefaultValueType);

                if (cbDefaultSession.Items.Count == 0)
                {
                    rbDefaultSession.Enabled = false;
                    cbDefaultSession.Enabled = false;
                    cbDefaultSession.TabStop = false;
                }

                if (IsRequestProperty)
                {
                    cbSearchable.Enabled = true;
                    tbDisplayFormat.Enabled = false;
                    btnBrowseDisplayFormat.Enabled = false;
                }
                else
                {
                    cbSearchable.Enabled = false;
                    tbDisplayFormat.Enabled = true;
                    btnBrowseDisplayFormat.Enabled = true;
                }

                tbId.Text = EditMappedProperty != null ? EditMappedProperty.Id.ToString() : "<Multi>";
                tbName.Text = EditMappedProperty != null ? EditMappedProperty.Name : "<Multi>";

                cbSearchable.Checked = EditMappedProperty != null ? EditMappedProperty.IsSearchable : IsRequestProperty ? true : false;
                tbDisplayFormat.Text = EditMappedProperty != null ? EditMappedProperty.DisplayFormat : string.Empty;

                if (!DisableDefaultValueEditing)
                {
                    if (EditMappedProperty != null)
                    {
                        if (!string.IsNullOrEmpty(EditMappedProperty.DefaultValue))
                        {
                            rbDefaultText.Checked = true;
                            tbDefaultValue.Text = EditMappedProperty.DefaultValue;
                        }
                        else if (EditMappedProperty.DefaultSessionProperty != null)
                        {
                            rbDefaultSession.Checked = true;

                            // Find the object to set in the combobox
                            foreach (UXSessionProperty prop in cbDefaultSession.Items)
                            {
                                if (prop.Id == EditMappedProperty.DefaultSessionProperty.Id)
                                {
                                    cbDefaultSession.SelectedItem = prop;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            rbDefaultNone.Checked = true;
                        }
                    }
                }

                // If custom field then make it possible to edit the name
                if (EditMappedProperty != null && EditMappedProperty.IsCustom)
                {
                    tbName.ReadOnly = false;
                    tbName.TabStop = true;
                }
            }
        }

        private void PopulateSessionProperties(Type defaultValueType)
        {
            if (SessionProperties != null && SessionProperties.Count > 0)
            {
                foreach (UXSessionProperty prop in SessionProperties.OrderBy(s => s.Name))
                {
                    if (defaultValueType == null ||
                        (defaultValueType != null && defaultValueType == prop.Type))
                    {
                        cbDefaultSession.Items.Add(prop);
                    }
                }
            }
        }

        private bool CheckDefaultValue(string defaultValue, Type defaultType)
        {
            try
            {
                Convert.ChangeType(defaultValue, defaultType);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(string.Format("Default Value is not valid: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show(string.Format("Default Value is not valid: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (EditMappedProperty != null)
            {
                string defaultValue = tbDefaultValue.Text.Trim();

                if (rbDefaultText.Checked)
                {
                    // Check default value
                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        if (!CheckDefaultValue(defaultValue, DefaultValueType))
                            return;
                    }
                }

                if (EditMappedProperty.IsCustom)
                {
                    // Check name
                    if (!NamingGuidance.CheckNameFocus(tbName, "Property Name", true))
                        return;

                    EditMappedProperty.Name = tbName.Text.Trim();
                }

                if (rbDefaultText.Checked)
                {
                    EditMappedProperty.DefaultValue = defaultValue;
                    EditMappedProperty.DefaultSessionProperty = null;
                }
                else if (rbDefaultSession.Checked)
                {
                    EditMappedProperty.DefaultValue = null;
                    EditMappedProperty.DefaultSessionProperty = (UXSessionProperty)cbDefaultSession.SelectedItem;
                }
                else
                {
                    EditMappedProperty.DefaultValue = null;
                    EditMappedProperty.DefaultSessionProperty = null;
                }

                if (EditMappedProperty.IsSearchable != cbSearchable.Checked)
                {
                    SearchableParameterUpdated = true;
                }

                EditMappedProperty.IsSearchable = cbSearchable.Checked;
                EditMappedProperty.DisplayFormat = tbDisplayFormat.Text.Trim();
            }
            else
            {
                // Save for multiselect
                if (!DisableDefaultValueEditing)
                {
                    string defaultValue = tbDefaultValue.Text.Trim();

                    // Check default value
                    if (rbDefaultText.Checked && !string.IsNullOrEmpty(defaultValue))
                    {
                        if (!CheckDefaultValue(defaultValue, DefaultValueType))
                            return;
                    }

                    // Update defaults
                    foreach (MappedProperty prop in EditMappedProperties)
                    {
                        if (rbDefaultText.Checked)
                        {
                            prop.DefaultValue = defaultValue;
                            prop.DefaultSessionProperty = null;
                        }
                        else if (rbDefaultSession.Checked)
                        {
                            prop.DefaultValue = null;
                            prop.DefaultSessionProperty = (UXSessionProperty)cbDefaultSession.SelectedItem;
                        }
                        else
                        {
                            prop.DefaultValue = null;
                            prop.DefaultSessionProperty = null;
                        }
                    }
                }

                // Update searchable and displayformat depending on if requestprop or not
                foreach (MappedProperty prop in EditMappedProperties)
                {
                    if (prop.IsSearchable != cbSearchable.Checked)
                    {
                        SearchableParameterUpdated = true;
                    }

                    if (IsRequestProperty)
                        prop.IsSearchable = cbSearchable.Checked;
                    else
                        prop.DisplayFormat = tbDisplayFormat.Text.Trim();
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void rbDefaultText_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDefaultText.Checked)
            {
                tbDefaultValue.ReadOnly = false;
                tbDefaultValue.TabStop = true;
                tbDefaultValue.Enabled = true;
                tbDefaultValue.SelectAll();
                tbDefaultValue.Focus();
            }
            else
            {
                tbDefaultValue.ReadOnly = true;
                tbDefaultValue.TabStop = false;
                tbDefaultValue.Enabled = false;
            }
        }

        private void rbDefaultSession_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDefaultSession.Checked)
            {
                cbDefaultSession.TabStop = true;
                cbDefaultSession.Enabled = true;
                cbDefaultSession.Focus();
            }
            else
            {
                cbDefaultSession.TabStop = false;
                cbDefaultSession.Enabled = false;
            }
        }

        private void btnBrowseDisplayFormat_Click(object sender, EventArgs e)
        {
            using (SelectDisplayFormats form = new SelectDisplayFormats())
            {
                form.DisplayFormat = tbDisplayFormat.Text;
                form.DisplayFormatDataType = EditMappedProperty.Type;
                form.IsEditable = this.IsEditable;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    tbDisplayFormat.Text = form.DisplayFormat;
                }
            }
        }
    }
}
