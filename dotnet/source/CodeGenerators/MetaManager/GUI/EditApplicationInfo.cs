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
    public partial class EditApplicationInfo : MdiChildForm
    {
        private Cdc.MetaManager.DataAccess.Domain.Application application;
        private Cdc.MetaManager.DataAccess.Domain.Schema schema;
        private IModelService modelService;
        private IConfigurationManagementService configurationManagementService;

        public EditApplicationInfo()
        {
            InitializeComponent();
            modelService = MetaManagerServices.GetModelService();
            configurationManagementService = MetaManagerServices.GetConfigurationManagementService();
        }

        private void EditApplicationInfo_Load(object sender, EventArgs e)
        {
            application = modelService.GetInitializedDomainObject<Cdc.MetaManager.DataAccess.Domain.Application>(ContaindDomainObjectIdAndType.Key);
            if (application.IsFrontend == false)
            {
                schema = modelService.GetAllDomainObjectsByApplicationId<Schema>(BackendApplication.Id)[0];
                ConnectionStringTextBox.Visible = true;
                connectionStringLabel.Visible = true;
                ConnectionStringTextBox.Text = schema.ConnectionString;
            }
            else
            {
                ConnectionStringTextBox.Visible = false;
                connectionStringLabel.Visible = false;
            }

            IsEditable = application.IsLocked && application.LockedBy == Environment.UserName;

            NameTextBox.Text = application.Name;
            NamespaceTextBox.Text = application.Namespace;
            VersionTextBox.Text = application.Version;


            if (!IsEditable)
            {
                NameTextBox.ReadOnly = true;
                NamespaceTextBox.ReadOnly = true;
                VersionTextBox.ReadOnly = true;
                ConnectionStringTextBox.ReadOnly = true;
                OkButton.Visible = false;

            }
            else
            {
                EnableDisableButtons();
            }

            Cursor.Current = Cursors.Default;
            NamespaceTextBox.Focus();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (IsEditable)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (application.IsFrontend == false)
                {
                    if (ConnectionStringTextBox.Text != schema.ConnectionString) 
                    {
                        if (MessageBox.Show("You have changed the connection string. This change cannot be cancelled. Are you sure that you want to save the change?", "Question", MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Question) == DialogResult.Cancel)
                        {
                            return;
                        }
                        else
                        {
                            //check out schema
                            try
                            {
                                MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(schema.Id, modelService.GetDomainObjectType(schema));
                            }
                            catch (ConfigurationManagementException ex)
                            {

                                Cursor.Current = Cursors.Default;
                                MessageBox.Show(ex.Message);
                                return;
                            }

                            schema = schema = modelService.GetAllDomainObjectsByApplicationId<Schema>(BackendApplication.Id)[0];

                            schema.ConnectionString = ConnectionStringTextBox.Text;
                        
                            // Save schema
                            schema = (Cdc.MetaManager.DataAccess.Domain.Schema)modelService.SaveDomainObject(schema);

                            // checkin
                            try
                            {
                                MetaManagerServices.GetConfigurationManagementService().CheckInDomainObject(schema.Id, modelService.GetDomainObjectType(schema), BackendApplication);
                            }
                            catch (ConfigurationManagementException ex)
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show(ex.Message);
                            }
                        
                        }
                    
                    }
                   
                }


                

                if (!NamingGuidance.CheckName(NameTextBox.Text, "Application Name", true))
                {
                    return;
                }


                application.Name = NameTextBox.Text;
                application.Namespace = NamespaceTextBox.Text;
                application.Version = VersionTextBox.Text;


                application = (Cdc.MetaManager.DataAccess.Domain.Application)modelService.SaveDomainObject(application);

                

                ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(application.Id, typeof(Cdc.MetaManager.DataAccess.Domain.Application));

                Cursor.Current = Cursors.Default;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void EnableDisableButtons()
        {

            OkButton.Enabled = false;

            if ((!string.IsNullOrEmpty(NameTextBox.Text.Trim())) &&
               (!string.IsNullOrEmpty(NamespaceTextBox.Text.Trim())) &&
               (!string.IsNullOrEmpty(VersionTextBox.Text.Trim())) )
            {
                if (application.IsFrontend == true)
                {
                    OkButton.Enabled = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(ConnectionStringTextBox.Text.Trim()))
                    {
                        OkButton.Enabled = true;
                    }
                }
            }

        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void NamespaceTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void VersionTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }



    }
}
