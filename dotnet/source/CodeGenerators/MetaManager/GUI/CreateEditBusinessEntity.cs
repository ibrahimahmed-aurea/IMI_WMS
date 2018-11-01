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
    public partial class CreateEditBusinessEntity : MdiChildForm
    {
        private BusinessEntity businessEntity;
        private IModelService modelService;
        private IConfigurationManagementService configurationManagementService;

        public CreateEditBusinessEntity()
        {
            InitializeComponent();
            modelService = MetaManagerServices.GetModelService();
            configurationManagementService = MetaManagerServices.GetConfigurationManagementService();
        }

        private void CreateEditBusinessEntity_Load(object sender, EventArgs e)
        {
            if (ContaindDomainObjectIdAndType.Key == Guid.Empty)
            {
                businessEntity = new BusinessEntity();
                businessEntity.Application = modelService.GetInitializedDomainObject<Cdc.MetaManager.DataAccess.Domain.Application>(BackendApplication.Id);                
                IsEditable = true;
                PropertieslinkLabel.Visible = false;
                this.Text = "Create Bussiness Entity";
            }
            else
            {
                businessEntity = modelService.GetInitializedDomainObject<BusinessEntity>(ContaindDomainObjectIdAndType.Key);
                IsEditable = businessEntity.IsLocked && businessEntity.LockedBy == Environment.UserName;
                NameTextBox.Text = businessEntity.Name;

                PropertieslinkLabel.Visible = true;
                this.Text = "Edit Bussiness Entity";
            }

            if (!IsEditable)
            {
                NameTextBox.ReadOnly = true;
                this.AcceptButton = CancelButton;

            }
            else
            {
                NameTextBox.SelectAll();                
                NameTextBox.Focus();                   
            }
            
            OkButton.Enabled = this.IsEditable;
            


            Cursor.Current = Cursors.Default;
            
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            bool isNew = false;

            if (IsEditable)
            {
                Cursor.Current = Cursors.WaitCursor;
                isNew = businessEntity.Id == Guid.Empty;

                if (!NamingGuidance.CheckNameEmptyFocus(NameTextBox, "Module Name", true))
                {
                    return;
                }

                if (string.IsNullOrEmpty(businessEntity.Name) || businessEntity.Name.ToUpper() != NameTextBox.Text.Trim().ToUpper())
                {
                    IEnumerable<string> businessEntityNames = modelService.GetAllDomainObjectsByApplicationId<BusinessEntity>(FrontendApplication.Id).Select<BusinessEntity, string>(s => s.Name);

                    if (!NamingGuidance.CheckNameNotInList(NameTextBox.Text.Trim(), "BusinessEntity Name", "List of BusinessEntity", businessEntityNames, false, true))
                        return;
                }

                

                if (!isNew)
                {
                    businessEntity = modelService.GetInitializedDomainObject<BusinessEntity>(businessEntity.Id);
                }

                businessEntity.Name = NameTextBox.Text;

                businessEntity = (BusinessEntity)modelService.SaveDomainObject(businessEntity);

                if (isNew)
                {
                    try
                    {                        
                        configurationManagementService.CheckOutDomainObject(businessEntity.Id, typeof(BusinessEntity));
                    }
                    catch (Exception ex)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }

                ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(businessEntity.Id, typeof(BusinessEntity));

                Cursor.Current = Cursors.Default;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void PropertieslinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (FindPropertyForm form = new FindPropertyForm())
            {
                form.FrontendApplication = FrontendApplication;
                form.BackendApplication = BackendApplication;
                form.BusinessEntity = businessEntity;
                form.CanShowCustomProperties = true;
                form.ShowDialog();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
