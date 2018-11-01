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
    public partial class EditDialog : MdiChildForm
    {
        public Dialog Dialog { get; set; }

        private IDialogService dialogService = null;
        private IModelService modelService = null;

        public EditDialog()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
        }

        private void EditDialog_Load(object sender, EventArgs e)
        {
            if (Dialog != null)
            {
                // Read the dialog to get the latest info from database
                Dialog = dialogService.GetDialogWithViewTree(Dialog.Id);

                if (Dialog != null)
                {
                    tbName.Text = Dialog.Name;
                    tbTitle.Text = Dialog.Title;
                    tbCreatorName.Text = Dialog.CreatorName;
                    tbCheckedOutBy.Text = Dialog.LockedBy;
                    tbCheckedOutBy.ReadOnly = true;

                    foreach (DialogType type in Enum.GetValues(typeof(DialogType)))
                    {
                        typeCbx.Items.Add(type.ToString());
                    }

                    typeCbx.SelectedIndex = (int)Dialog.Type;

                    IList<Module> modules = dialogService.GetAllModules(FrontendApplication.Id);

                    moduleBindingSource.DataSource = modules.OrderBy(module => module.Name);

                    moduleCbx.SelectedValue = Dialog.Module.Id;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Check name of the Dialog
            if (!NamingGuidance.CheckDialogName(tbName.Text.Trim(), true))
                return;

            // Check if Title is set
            if (!NamingGuidance.CheckCaptionFocus(tbTitle, "Dialog Title", true))
                return;

            bool nameChange = (Dialog.Name != tbName.Text.Trim());

            Dialog.Name = tbName.Text.Trim();
            Dialog.Title = tbTitle.Text.Trim();
            Dialog.CreatorName = tbCreatorName.Text.Trim();            
            Dialog.Type = (DialogType)Enum.Parse(typeof(DialogType), typeCbx.SelectedItem.ToString());
            Dialog.Module = moduleCbx.SelectedItem as Module;

            // Save Dialog
            modelService.SaveDomainObject(Dialog);

            if (Dialog.SearchPanelView != null && nameChange)
            {
                MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(Dialog.SearchPanelView.Id, typeof(DataAccess.Domain.View));

                // Changing name of SearchPanelView
                DataAccess.Domain.View searchPanel = modelService.GetDomainObject<DataAccess.Domain.View>(Dialog.SearchPanelView.Id);
                searchPanel.Name = string.Format("{0}SearchPanel", Dialog.Name);

                // Save SearchPanelView

                modelService.SaveDomainObject(Dialog.SearchPanelView);
            }

            //Update workflows since dialog name might have changed
            dialogService.UpdateWorkflows(Dialog.Module);
            
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}
