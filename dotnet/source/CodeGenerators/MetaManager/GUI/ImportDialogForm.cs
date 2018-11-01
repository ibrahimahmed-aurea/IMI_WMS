using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DelphiXmlImporter;
using Cdc.MetaManager.BusinessLogic;
using Spring.Context;
using Spring.Context.Support;
using Domain = Cdc.MetaManager.DataAccess.Domain;
using System.Diagnostics;
using System.Collections;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.LayoutAnalyzer;
using Cdc.Common.GUI.Library;

namespace Cdc.MetaManager.GUI
{
    
    public partial class ImportDialogForm : MdiChildForm
    {
        private static IApplicationContext ctx;

        public ImportDialogForm()
        {
            InitializeComponent();
            ctx = ContextRegistry.GetContext();
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            openXMLFileDialog.FileName = filenameTbx.Text;
            
            if (openXMLFileDialog.ShowDialog() == DialogResult.OK)
            {
                filenameTbx.Text = openXMLFileDialog.FileName;
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            foreach (Module item in moduleCbx.Items.Cast<Module>())
            {
                if (item.Name == moduleCbx.Text)
                    moduleCbx.SelectedItem = item;
            }

            Module module = moduleCbx.SelectedItem as Module;

            IDialogService dialogService = MetaManagerServices.GetDialogService();

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
                        module = dialogService.CreateModule(moduleCbx.Text, FrontendApplication);
                        
                        PopulateModuleCombobox();

                        foreach (Module item in moduleCbx.Items.Cast<Module>())
                        {
                            if (item.Id == module.Id)
                                moduleCbx.SelectedItem = item;
                        }
                    }
                }

                if (beCbx.SelectedItem == null)
                {
                    MessageBox.Show("You must select a business entity.");
                    return;
                }

                // Save selected Module and Business Entity
                Config.Frontend.LastImportDialogModule = module.Name;
                Config.Frontend.LastImportDialogBusinessEntity = (beCbx.SelectedItem as BusinessEntity).Name;
                Config.Save();

                DialogFactory f = new DialogFactory(FrontendApplication);
                List<Dialog> dialogs = f.CreateDialogs(filenameTbx.Text, beCbx.SelectedItem as BusinessEntity, cbAddDrilldownViews.Checked, tbUniqueness.Text.Trim()); 
                
                string addReport = "";

                if (!previewCb.Checked)
                {
                    SelectImportDialogs selectDialogForm = new SelectImportDialogs();
                    selectDialogForm.Dialogs = dialogs;
                    selectDialogForm.MaySelectDialogs = !cbAddDrilldownViews.Checked;
                    DialogResult dr = selectDialogForm.ShowDialog(this);
                    dialogs = selectDialogForm.SelectedDialogs;

                    if (dialogs.Count > 0)
                    {
                        // Persist module, cascading down to dialog etc.
                        dialogService.AddDialogsToModule(module, dialogs);
                        addReport = "\nDatabase was updated.";
                    }
                }

                if (dialogs.Count > 0)
                {
                    string end = "";
                    if (dialogs.Count > 1)
                    {
                        end = "s";
                    }

                    string dialogNames = "";

                    foreach (Dialog dialog in dialogs)
                    {
                        dialogNames += "\n      " + dialog.Name;
                    }

                    MessageBox.Show(string.Format("The import created {0} new dialog{1} :\n{2}\n{3}\n", dialogs.Count, end, dialogNames, addReport));

                    if (previewCb.Checked)
                    {
                        foreach (Dialog d in dialogs)
                        {
                            using (DialogObjectViewer viewer = new DialogObjectViewer())
                            {
                                viewer.Owner = this;
                                viewer.Dialog = d;
                                viewer.ShowDialog();
                            }
                        }
                    }

                    filenameTbx.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

                
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PopulateModuleCombobox()
        {
            IModuleDao moduleDao = ctx["ModuleDao"] as IModuleDao;

            var modules = from Module module in moduleDao.FindAll()
                          where module.Application.Id == FrontendApplication.Id
                          select module;

            moduleBindingSource.DataSource = modules.OrderBy(m => m.Name).ToArray<Module>();

            // Find the last imported dialog module
            if (!string.IsNullOrEmpty(Config.Frontend.LastImportDialogModule) &&
                moduleCbx.Items.Count > 0)
            {
                foreach (Module item in moduleCbx.Items.Cast<Module>())
                {
                    if (item.Name == Config.Frontend.LastImportDialogModule)
                        moduleCbx.SelectedItem = item;
                }
            }
        }

        private void PopulateBECombobox()
        {
            IBusinessEntityDao beDao = ctx["BusinessEntityDao"] as IBusinessEntityDao;
            
            IList<BusinessEntity> bes = beDao.FindAll(BackendApplication.Id);

            IEnumerable<BusinessEntity> sortedBes = from b in bes
                                                    orderby b.Application, b.Name, b.Id
                                                    select b;

            beBindingSource.DataSource = sortedBes;

            // Find the last imported dialog module
            if (!string.IsNullOrEmpty(Config.Frontend.LastImportDialogBusinessEntity) &&
                beCbx.Items.Count > 0)
            {
                foreach (BusinessEntity item in beCbx.Items.Cast<BusinessEntity>())
                {
                    if (item.Name == Config.Frontend.LastImportDialogBusinessEntity)
                        beCbx.SelectedItem = item;
                }
            }
        }

        private void ImportDialogForm_Load(object sender, EventArgs e)
        {
            PopulateModuleCombobox();
            PopulateBECombobox();
        }

        private void fieldTextChanged(object sender, EventArgs e)
        {
            okBtn.Enabled = ((!string.IsNullOrEmpty(filenameTbx.Text) && (!string.IsNullOrEmpty(moduleCbx.Text))));
            
        }


    }

}
