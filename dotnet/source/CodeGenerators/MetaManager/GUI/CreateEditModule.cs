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
    public partial class CreateEditModule : MdiChildForm
    {
        private Module module;
        private IModelService modelService;
        private IConfigurationManagementService configurationManagementService;

        public CreateEditModule()
        {
            InitializeComponent();
            modelService = MetaManagerServices.GetModelService();
            configurationManagementService = MetaManagerServices.GetConfigurationManagementService();
        }

        private void CreateEditModule_Load(object sender, EventArgs e)
        {
            if (ContaindDomainObjectIdAndType.Key == Guid.Empty)
            {
                module = new Module();
                module.Application = FrontendApplication;
                IsEditable = true;
            }
            else
            {
                module = modelService.GetInitializedDomainObject<Module>(ContaindDomainObjectIdAndType.Key);

                IsEditable = module.IsLocked && module.LockedBy == Environment.UserName;

                NameTextBox.Text = module.Name;
            }

            if (!IsEditable)
            {
                NameTextBox.ReadOnly = true;
                OkButton.Visible = false;
            }

            Cursor.Current = Cursors.Default;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            bool isNew = false;

            if (IsEditable)
            {
                Cursor.Current = Cursors.WaitCursor;
                isNew = module.Id == Guid.Empty;

                if (!NamingGuidance.CheckNameEmptyFocus(NameTextBox, "Module Name", true))
                {
                    return;
                }

                if (string.IsNullOrEmpty(module.Name) || module.Name.ToUpper() != NameTextBox.Text.Trim().ToUpper())
                {
                    IEnumerable<string> moduleNames = modelService.GetAllDomainObjectsByApplicationId<Module>(FrontendApplication.Id).Select<Module, string>(s => s.Name);

                    if (!NamingGuidance.CheckNameNotInList(NameTextBox.Text.Trim(), "Module Name", "List of Modules", moduleNames, false, true))
                        return;
                }

                module.Name = NameTextBox.Text;

                module = (Module)modelService.SaveDomainObject(module);

                if (isNew)
                {
                    try
                    {
                        configurationManagementService.CheckOutDomainObject(module.Id, typeof(Module));
                    }
                    catch (Exception ex)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }

                ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(module.Id, typeof(Module));

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
    }
}
