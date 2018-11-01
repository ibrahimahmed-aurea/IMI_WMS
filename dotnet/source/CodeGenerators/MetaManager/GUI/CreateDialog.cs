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
    public partial class CreateDialog : MdiChildForm
    {
        public Dialog Dialog { get; set; }
        public String startupModule = null;

        private IDialogService dialogService = null;
        private IModelService modelService = null;

        public Cdc.MetaManager.DataAccess.Domain.View SelectedView { get; set; }

        public bool FixedModule { get; set; }

        public CreateDialog()
        {
            InitializeComponent();
            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
        }

        private void CreateDialog_Load(object sender, EventArgs e)
        {
            foreach (DialogType type in Enum.GetValues(typeof(DialogType)))
            {
                typeCbx.Items.Add(type.ToString());
            }

            tbCreatorName.Text = Environment.UserName;

            PopulateModuleCombobox();

            EnableDisableButtons();

            moduleCbx.Enabled = !FixedModule;

            Cursor.Current = Cursors.Default;
        }

        private void EnableDisableButtons()
        {

            btnOK.Enabled = false;

            if ((!string.IsNullOrEmpty(tbName.Text.Trim())) &&
               (!string.IsNullOrEmpty(tbTitle.Text.Trim())) &&
               (!string.IsNullOrEmpty(moduleCbx.Text.Trim())) &&
                typeCbx.SelectedItem != null &&
                (!string.IsNullOrEmpty(tbCreatorName.Text.Trim())) &&
                (!string.IsNullOrEmpty(tbViewName.Text.Trim())))
            {
                btnOK.Enabled = true;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Check name.
            if (!NamingGuidance.CheckDialogName(tbName.Text.Trim(), true))
            {
                return;
            }

            // Check Title
            if (!NamingGuidance.CheckCaptionFocus(tbTitle, "Dialog title", true))
                return;

            foreach (Module item in moduleCbx.Items.Cast<Module>())
            {
                if (item.Name == moduleCbx.Text)
                    moduleCbx.SelectedItem = item;
            }

            // The name must be unique within the same module

            IList<Cdc.MetaManager.DataAccess.Domain.Dialog> dialogList = dialogService.FindDialogsByNameAndModule(FrontendApplication.Id, moduleCbx.Text, tbName.Text);


            if (dialogList.Count > 0)
            {
                MessageBox.Show("The name of the dialog must be unique within the same module, please change the value for the dialog name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbName.SelectAll();
                tbName.Focus();
                return;
            }





            Module module = moduleCbx.SelectedItem as Module;

            try
            {
                if (module == null)
                {
                    if (string.IsNullOrEmpty(moduleCbx.Text))
                    {
                        MessageBox.Show("You must select or enter a module.");
                        return;
                    }
                    else
                    {
                        module = new Module();
                        module.Application = FrontendApplication;
                        module.Name = moduleCbx.Text;

                        module = (Module)modelService.SaveDomainObject(module);

                        PopulateModuleCombobox();

                        foreach (Module item in moduleCbx.Items.Cast<Module>())
                        {
                            if (item.Id == module.Id)
                                moduleCbx.SelectedItem = item;
                        }
                    }

                }

                Dialog = new Dialog();
                Dialog.Name = tbName.Text.Trim();
                Dialog.Title = tbTitle.Text.Trim();
                Dialog.CreatorName = tbCreatorName.Text.Trim();                
                Dialog.Type = (DialogType)Enum.Parse(typeof(DialogType), typeCbx.SelectedItem.ToString());

                //Check that Views that are used in create or modify dialogs not is used in other dialogs 

                if (Dialog.Type == DialogType.Create || Dialog.Type == DialogType.Modify)
                {
                    long noOfViews = dialogService.CountViewNodes(SelectedView);

                    if (noOfViews > 0)
                    {
                        //We will not allow to use the same view in several dialogs. 
                        MessageBox.Show("The view is already used by another dialog. Please, select another view for this dialog!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tbViewName.SelectAll();
                        tbViewName.Focus();
                        return;
                    }

                }



                    // Create Rote ViewNode
                    ViewNode rootNode = new Cdc.MetaManager.DataAccess.Domain.ViewNode();
                    rootNode.View = SelectedView;
                    if (Dialog.Type == DialogType.Overview || Dialog.Type == DialogType.Drilldown)
                    {
                        rootNode.Title = "Overview";
                    }
                    else if (Dialog.Type == DialogType.Create || Dialog.Type == DialogType.Modify || Dialog.Type == DialogType.Find)
                    {
                        rootNode.Title = Dialog.Title;
                    }
                    rootNode.Sequence = 1;
                    rootNode.Dialog = Dialog;
                    Dialog.ViewNodes.Add(rootNode);
                    Dialog.RootViewNode = rootNode;
                    Dialog.InterfaceView = SelectedView;
                    Dialog.Module = moduleCbx.SelectedItem as Module;

                    // Save Dialog
                    modelService.MergeSaveDomainObject(Dialog);


                    // Check out the dialog
                    CheckOutInObject(Dialog, true);

                    ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(Dialog.Id, typeof(Dialog));
                    DialogResult = DialogResult.OK;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnCreateView_Click(object sender, EventArgs e)
        {
            using (CreateView createView = new CreateView())
            {
                createView.FrontendApplication = FrontendApplication;
                createView.BackendApplication = BackendApplication;
                createView.Owner = this;

                if (createView.ShowDialog() == DialogResult.OK)
                {
                    SelectedView = modelService.GetInitializedDomainObject<DataAccess.Domain.View>(createView.NewView.Id);
                    PopulateViewFields();
                    EnableDisableButtons();
                }

            }
        }

        private void PopulateViewFields()
        {
            if (SelectedView != null)
            {
                // Set the view information
                tbViewName.Text = SelectedView.Name;
                tbViewTitle.Text = SelectedView.Title;
            }
            else
            {
                tbViewName.Text = string.Empty;
                tbViewTitle.Text = string.Empty;
            }


        }

        private void btnSelectView_Click(object sender, EventArgs e)
        {
            using (FindViewForm findView = new FindViewForm())
            {
                findView.FrontendApplication = FrontendApplication;
                findView.BackendApplication = BackendApplication;

                if (findView.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("Do you want to create a copy of the selected view?\r\nNOTE - Only the layout will be copied.", "Copy View", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        try
                        {
                            Cursor = Cursors.WaitCursor;
                            SelectedView = MetaManagerServices.Helpers.ViewHelper.CopyView(findView.View);
                        }
                        finally
                        {
                            Cursor = Cursors.Default;
                        }
                    }
                    else
                    {
                        SelectedView = findView.View;
                    }

                    PopulateViewFields();
                    EnableDisableButtons();
                }
            }
        }

        private void PopulateModuleCombobox()
        {

            IList<Module> modules = dialogService.GetAllModules(FrontendApplication.Id);

            moduleBindingSource.DataSource = modules.OrderBy(module => module.Name);

            // Find the last imported dialog module
            if (!string.IsNullOrEmpty(startupModule) && moduleCbx.Items.Count > 0)
            {
                foreach (Module item in moduleCbx.Items.Cast<Module>())
                {
                    if (item.Name == startupModule)
                    {
                        moduleCbx.SelectedItem = item;
                    }
                }
            }



        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();

        }

        private void tbTitle_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void moduleCbx_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void typeCbx_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void tbCreatorName_TextChanged(object sender, EventArgs e)
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
