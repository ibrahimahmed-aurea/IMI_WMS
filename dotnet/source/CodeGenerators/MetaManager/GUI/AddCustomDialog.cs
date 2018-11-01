using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;


namespace Cdc.MetaManager.GUI
{
    public partial class AddCustomDialog : MdiChildForm
    {
        private CustomDialog CustomDialog;

        private IModelService modelService;

        private Boolean newDialog = false;

        public CustomDialog getCustomDialog
        {
            get { return CustomDialog; }
        }

        public AddCustomDialog()
        {
            InitializeComponent();

            modelService = MetaManagerServices.GetModelService();

        }

        private void AddCustomDialog_Load(object sender, EventArgs e)
        {
            if (ContaindDomainObjectIdAndType.Key == Guid.Empty)
            {
                CustomDialog = new CustomDialog();
                CustomDialog.Application = FrontendApplication;
                this.IsEditable = true;
                newDialog = true;
                this.Text = "Add " + this.Text;
            }
            else
            {
                // Read the dialog                
                CustomDialog = modelService.GetInitializedDomainObject<CustomDialog>(ContaindDomainObjectIdAndType.Key);
                this.IsEditable = CustomDialog.IsLocked && CustomDialog.LockedBy == Environment.UserName;

                this.Text = "Edit " + this.Text;

                tbName.Text = CustomDialog.Name;
                tbTopic.Text = CustomDialog.Topic;
                tbDLLName.Text = CustomDialog.DLLName;
            }

            EnableDisableButtons();
            EnableDisableFields();

            Cursor.Current = Cursors.Default;
        }

        private void EnableDisableButtons()
        {
            btnOK.Enabled = false;

            if (!string.IsNullOrEmpty(tbName.Text) &&
                !string.IsNullOrEmpty(tbTopic.Text) &&
                !string.IsNullOrEmpty(tbDLLName.Text))
            {
                btnOK.Enabled = this.IsEditable;
            }
        }

        private void EnableDisableFields()
        {
            tbName.ReadOnly = !this.IsEditable;
            tbTopic.ReadOnly = !this.IsEditable;
            tbDLLName.ReadOnly = !this.IsEditable;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!NamingGuidance.CheckNameFocus(tbName, "Custom Dialog Name", true))
                return;

            if (!NamingGuidance.CheckCaptionFocus(tbTopic, "Topic", true))
                return;

            if (!NamingGuidance.CheckDLLName(tbDLLName.Text.Trim(), "DLL Name", true))
            {
                tbDLLName.Focus();
                return;
            }

            CustomDialog.Name = tbName.Text.Trim();
            CustomDialog.Topic = tbTopic.Text.Trim();
            CustomDialog.DLLName = tbDLLName.Text.Trim();

            // Save the Custom Dialog to database
            modelService.SaveDomainObject(CustomDialog);

            // if this is a new dialog leave the dialog as checked out
            if (newDialog == true)
            {
                CheckOutInObject(CustomDialog, true);

                ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(CustomDialog.Id, typeof(CustomDialog));
            }

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void tbTopic_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void tbDLLName_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void CheckOutInObject(DataAccess.IVersionControlled checkOutObject, bool trueCheckOut_falseCheckIn)
        {
            DataAccess.IVersionControlled domainObject = null;
            domainObject = checkOutObject;

            if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(domainObject.GetType()))
            {
                if (trueCheckOut_falseCheckIn)
                {
                    try
                    {
                        MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(domainObject.Id, domainObject.GetType());
                    }
                    catch (ConfigurationManagementException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        MetaManagerServices.GetConfigurationManagementService().CheckInDomainObject(domainObject.Id, domainObject.GetType(), FrontendApplication);
                        Cursor.Current = Cursors.Default;
                    }
                    catch (ConfigurationManagementException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }

    }
}
