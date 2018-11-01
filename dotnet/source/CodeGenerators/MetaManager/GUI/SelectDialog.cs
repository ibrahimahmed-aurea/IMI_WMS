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
    public partial class SelectDialog : MdiChildForm
    {
        private IDialogService dialogService = null;
        private IModelService modelService = null;

        public SelectDialog()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
        }
                
        public Dialog SelectedDialog { get; set; }

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
            criteriaDictionary.Add("Name", tbDialogName.Text);
            criteriaDictionary.Add("Title", tbTitle.Text);
            criteriaDictionary.Add("OriginalDialogName", tbOrginalDialogName.Text);
            criteriaDictionary.Add("Module.Name", cbModules.Text);

            IList<Dialog> dialogs = modelService.GetAllDomainObjectsByPropertyValues<Dialog>(criteriaDictionary, true, true);

            // Save last searched Module value
            Config.Frontend.HandleDialogsLastSearchedModule = cbModules.Text;
            Config.Save();

            PopulateResult(dialogs);
            EnableDisableButtons();
        }

        private void PopulateResult(IList<Dialog> dialogs)
        {
            lvResult.Items.Clear();

            if (dialogs != null)
            {
                foreach (Dialog dialog in dialogs)
                {
                    ListViewItem item = new ListViewItem(dialog.Id.ToString());
                    item.SubItems.Add(dialog.Name);
                    item.SubItems.Add(dialog.Title);
                    item.SubItems.Add(dialog.OriginalDialogName);
                    item.SubItems.Add(dialog.Type.ToString());                    
                    item.SubItems.Add(dialog.Module.Name);
                    item.SubItems.Add(dialog.LockedBy);
                    item.SubItems.Add(dialog.CreatorName);

                    item.Tag = dialog;                    
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
            if (lvResult.SelectedItems.Count == 1)
            {
                btnOK.Enabled = true;
            }
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

        }

        private void lvResult_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            SelectedDialog = (Dialog)lvResult.SelectedItems[0].Tag;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
                
        private void tbTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, null);
            }
        }
    }
}
