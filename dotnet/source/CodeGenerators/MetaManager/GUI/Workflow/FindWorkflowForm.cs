using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain;


namespace Cdc.MetaManager.GUI
{
    public partial class FindWorkflowForm : MdiChildForm
    {
        private IDialogService dialogService = null;
        private IModelService modelService = null;
        
        public FindWorkflowForm()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
        }

        public Cdc.MetaManager.DataAccess.Domain.Workflow SelectedWorkflow { get; set; }

        private void ConnectViewToService_Load(object sender, EventArgs e)
        {
            cbModules.Text = Config.Frontend.HandleDialogsLastSearchedModule;

            PopulateModuleCombobox();

            if (!string.IsNullOrEmpty(cbModules.Text))
                btnSearch_Click(this, null);

            cbModules.Focus();

            EnableDisableButtons();

            Cursor.Current = Cursors.Default;
        }

        private void PopulateModuleCombobox()
        {
            string tmp = cbModules.Text;

            IList<Module> modules = dialogService.GetAllModules(FrontendApplication.Id);

            moduleBindingSource.DataSource = modules.OrderBy(module => module.Name);

            cbModules.Text = tmp;
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var criteriaDictionary = new Dictionary<string, object>();
            criteriaDictionary.Add("Module.Application.Id", FrontendApplication.Id);
            criteriaDictionary.Add("Name", tbWorkflowName.Text);
            criteriaDictionary.Add("Module.Name", cbModules.Text);

            IList<Cdc.MetaManager.DataAccess.Domain.Workflow> workflows = modelService.GetAllDomainObjectsByPropertyValues<Cdc.MetaManager.DataAccess.Domain.Workflow>(criteriaDictionary, true, true);
                        
            PopulateResult(workflows);
            EnableDisableButtons();
        }

        private void PopulateResult(IList<Cdc.MetaManager.DataAccess.Domain.Workflow> workflows)
        {
            lvResult.Items.Clear();

            if (workflows != null)
            {
                foreach (Cdc.MetaManager.DataAccess.Domain.Workflow workflow in workflows)
                {
                    ListViewItem item = new ListViewItem(workflow.Id.ToString());
                    item.SubItems.Add(workflow.Name);
                    item.SubItems.Add(workflow.Description);
                    item.SubItems.Add(workflow.Module.Name);

                    item.Tag = workflow;
                                        
                    lvResult.Items.Add(item);
                }
            }

            // Select the first item in list
            if (lvResult.Items.Count > 0)
            {
                lvResult.Items[0].Selected = true;
            }
        }

        private void EnableDisableButtons()
        {
                okBtn.Visible = true;
        }

        private void lvResult_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            EnableDisableButtons();
        }

        private void cbModules_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, null);
            }
        }

        private void tbDialogName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, null);
            }
        }

        private void tbOrginalDialogName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, null);
            }
        }

        private void lvResult_DoubleClick(object sender, EventArgs e)
        {
            if (lvResult.SelectedItems[0].Tag != null)
            {
                okBtn_Click(this, null);
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            SelectedWorkflow = (Cdc.MetaManager.DataAccess.Domain.Workflow)lvResult.SelectedItems[0].Tag;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
