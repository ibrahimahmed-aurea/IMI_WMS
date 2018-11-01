using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Spring.Context;
using Spring.Context.Support;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess.Dao;
using System.Diagnostics;
using System.IO;
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplates;
using Cdc.CodeGeneration.Caching;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using System.Reflection;
using Cdc.MetaManager.DataAccess;
using Domain = Cdc.MetaManager.DataAccess.Domain;

using NHibernate;

namespace Cdc.MetaManager.GUI
{
    public partial class GenerateFrontendCodeForm : MdiChildForm
    {
        private IApplicationContext ctx;
        private IDialogService dialogService = null;
        private IMenuService menuService = null;
        private bool resolveDependencies = true;
        private IModelService modelService;
        private DateTime generateStartTime;

        private bool doBreak = false;

        private void LoadModules()
        {
            IEnumerable<Domain.Module> moduleList = modelService.GetAllDomainObjectsByApplicationId<Domain.Module>(FrontendApplication.Id).OrderBy(m => (m.Name));

            resolveDependencies = false;

            lvModules.Items.Clear();

            foreach (Domain.Module module in moduleList)
            {
                ListViewItem item = new ListViewItem(module.Name);
                item.Checked = Config.Frontend.GenerationFilter.Contains(module.Id.ToString());
                item.Tag = module;
                lvModules.Items.Add(item);
            }

            resolveDependencies = true;

            cbSelectAll.CheckedChanged -= cbSelectAll_CheckedChanged;
            cbSelectAll.Checked = lvModules.CheckedItems.Count == lvModules.Items.Count;
            cbSelectAll.CheckedChanged += cbSelectAll_CheckedChanged;
        }

        public GenerateFrontendCodeForm()
        {
            InitializeComponent();

            ctx = ContextRegistry.GetContext();
            dialogService = MetaManagerServices.GetDialogService();
            menuService = MetaManagerServices.GetMenuService();

            modelService = MetaManagerServices.GetModelService();

            modelService.StatusChanged += new StatusChangedDelegate(modelService_StatusChanged);
        }

        void modelService_StatusChanged(string message, int value, int min, int max)
        {
            UpdateStatusAndTime(message, value, min, max);
            if (doBreak)
                throw new Exception("USER STOPPED GENERATION!");
        }



        private void browseSolutionBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(solutionPathTbx.Text.Trim()) || !Directory.Exists(solutionPathTbx.Text))
                folderDlg.SelectedPath = Path.GetFullPath(@"..\..\..\..\..\..\source\Auto\FE");
            else if (Directory.Exists(solutionPathTbx.Text))
                folderDlg.SelectedPath = solutionPathTbx.Text;
            else
                folderDlg.SelectedPath = string.Empty;

            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                solutionPathTbx.Text = folderDlg.SelectedPath;

                ShowSolutionPathWarning();
            }
        }

        private bool ShowSolutionPathWarning()
        {
            if (solutionPathTbx.Text.Length > 100)
            {
                MessageBox.Show("The Solution Folder path is over 100 characters and you can expect to get\n" +
                                "problems with the codegeneration since the directory structure combined with\n" +
                                "the filenames will probably go over the limits.\n\n" +
                                "Recommended action is to change to a shorter path!",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return true;
            }
            return false;
        }

        private void browseRefernceBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(referencePathTbx.Text.Trim()) || !Directory.Exists(referencePathTbx.Text))
                folderDlg.SelectedPath = Path.GetFullPath(@"..\..\..\..\..\..\references");
            else if (Directory.Exists(referencePathTbx.Text))
                folderDlg.SelectedPath = referencePathTbx.Text;
            else
                folderDlg.SelectedPath = string.Empty;

            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                referencePathTbx.Text = folderDlg.SelectedPath;
            }
        }

        private void CodeSmithTemplateCallback(string text, int percentageDone)
        {
            UpdateStatusAndTime(text, percentageDone, 0, 100);
            System.Windows.Forms.Application.DoEvents();
            if (doBreak)
                throw new Exception("USER STOPPED GENERATION!");
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            // First check length of solution folder
            if (ShowSolutionPathWarning())
            {
                if (MessageBox.Show("Do you want to continue?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
            }

            if (!CheckPaths())
                return;

            okBtn.Enabled = false;
            cancelBtn.Enabled = false;
            btnStopGeneration.Enabled = true;
            doBreak = false;

            // Reset progressbar and make the panel visible
            progressBar.Value = 0;
            pProgress.Visible = true;
            lblTimeElapsed.Text = string.Empty;

            lblProgressText.Text = "Extracting Data from Database...";
            System.Windows.Forms.Application.DoEvents();

            Cursor.Current = Cursors.WaitCursor;
            System.Windows.Forms.Application.DoEvents();

            // Save to config file
            //Config.Frontend.SolutionName = solutionNameTbx.Text;
            Config.Frontend.SolutionFolder = solutionPathTbx.Text;
            //Config.Frontend.ReferenceFolder = referencePathTbx.Text;

            List<string> selectedModuleIds = (from ListViewItem item in lvModules.CheckedItems
                                              select ((Domain.Module)item.Tag).Id.ToString()).ToList();
            Config.Frontend.GenerationFilter = selectedModuleIds;
            Config.Save();

            System.Threading.ThreadPool.QueueUserWorkItem(ThreadWork, selectedModuleIds);
        }

        private void ThreadWork(object state)
        {
            try
            {
                generateStartTime = DateTime.Now;

                var selectedModules = (from id in (List<string>)state
                                       select modelService.GetDomainObject<Domain.Module>(Guid.Parse(id))).ToList();

                string referencePath = referencePathTbx.Text;
                string solutionFileNameAndPath = Path.Combine(solutionPathTbx.Text, solutionNameTbx.Text + ".sln");

                modelService.GenerateFrontendFromGUI(selectedModules.ToList<Domain.Module>(), FrontendApplication, solutionFileNameAndPath, referencePath);

                UpdateStatusAndTime("Done!", 0, 0, 0);

                string info = string.Format("Code generation complete.\r\n{0} files generated, {1} files written to disk", FileCacheManager.GetWrites(), FileCacheManager.GetWritesToDisk());

                if (this.InvokeRequired)
                {
                    this.Invoke(new System.Action(() =>
                    {
                        ShowGenerationResult.Show(this, "Generation Result", info, "Files written to disk", FileCacheManager.GetWrittenFilesToDisk());
                    })
                    );
                }
                else
                {
                    ShowGenerationResult.Show(this, "Generation Result", info, "Files written to disk", FileCacheManager.GetWrittenFilesToDisk());
                }
            }
            finally
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new System.Action(() =>
                    {
                        Cursor.Current = Cursors.Default;
                        pProgress.Visible = false;
                        EnableDisableButtons();
                    })
                    );
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    pProgress.Visible = false;
                    EnableDisableButtons();
                }
            }
        }



        private void UpdateStatusAndTime(string message, int value, int min, int max)
        {
            TimeSpan x = new TimeSpan();
            if (generateStartTime != null)
            {
                x = DateTime.Now - generateStartTime;
            }

            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action(() =>
                {
                    lblProgressText.Text = message;
                    progressBar.Minimum = min;
                    progressBar.Maximum = max;
                    progressBar.Value = value;
                    if (generateStartTime != null)
                    {
                        lblTimeElapsed.Text = string.Format("{0}:{1}:{2}", x.Hours.ToString("00"), x.Minutes.ToString("00"), x.Seconds.ToString("00"));
                    }
                })
                );
            }
            else
            {
                lblProgressText.Text = message;
                progressBar.Minimum = min;
                progressBar.Maximum = max;
                progressBar.Value = value;
                if (generateStartTime != null)
                {
                    lblTimeElapsed.Text = string.Format("{0}:{1}:{2}", x.Hours.ToString("00"), x.Minutes.ToString("00"), x.Seconds.ToString("00"));
                }
            }
        }

        private bool CheckPaths()
        {
            if (!Directory.Exists(solutionPathTbx.Text))
            {
                MessageBox.Show("The path to the solution folder does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                solutionPathTbx.SelectAll();
                solutionPathTbx.Focus();
                return false;
            }
            else if (!Directory.Exists(referencePathTbx.Text))
            {
                MessageBox.Show("The path to the reference folder does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                referencePathTbx.SelectAll();
                referencePathTbx.Focus();
                return false;
            }
            return true;
        }

        private void FindComponentServiceReferences(UXComponent component, IDictionary<Guid, Service> references)
        {
            if (component != null)
            {
                foreach (PropertyInfo info in component.GetType().GetProperties())
                {
                    if (info.GetCustomAttributes(typeof(DomainReferenceAttribute), false).Count() > 0)
                    {
                        if (info.PropertyType == typeof(ServiceMethod))
                        {
                            ServiceMethod method = info.GetValue(component, null) as ServiceMethod;

                            if (method != null)
                            {
                                if (method.Service != null)
                                {
                                    if (!references.ContainsKey(method.Service.Id))
                                        references.Add(method.Service.Id, method.Service);
                                }
                            }
                        }
                    }
                }

                UXContainer container = null;

                if (component is UXGroupBox)
                    container = (component as UXGroupBox).Container;
                else
                    container = component as UXContainer;

                if (container != null)
                {
                    foreach (UXComponent child in container.Children)
                        FindComponentServiceReferences(child, references);
                }
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GenerateFrontendCodeForm_Load(object sender, EventArgs e)
        {
            tbApp.Text = string.Format("{0}", FrontendApplication.Name);

            //solutionNameTbx.Text = Config.Frontend.SolutionName;
            solutionPathTbx.Text = Config.Frontend.SolutionFolder;
            //referencePathTbx.Text = Config.Frontend.ReferenceFolder;

            if (string.IsNullOrEmpty(solutionPathTbx.Text))
            {
                solutionPathTbx.Text = Path.Combine(@"N:\source\Auto\FE\", tbApp.Text);
            }

            LoadModules();

            pProgress.Visible = false;
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            okBtn.Enabled = false;
            cancelBtn.Enabled = true;

            if (!string.IsNullOrEmpty(solutionNameTbx.Text) &&
                !string.IsNullOrEmpty(solutionPathTbx.Text) &&
                !string.IsNullOrEmpty(referencePathTbx.Text) &&
                lvModules.CheckedItems.Count > 0)
            {
                okBtn.Enabled = true;
            }
        }

        private void solutionNameTbx_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void solutionPathTbx_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
            referencePathTbx.Text = Path.Combine(solutionPathTbx.Text, "Temp");
        }

        private void referencePathTbx_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void ResolveModuleDependencies(Cdc.MetaManager.DataAccess.Domain.Module module)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (Cdc.MetaManager.DataAccess.Domain.Module depModule in dialogService.ResolveModuleDependencies(module))
                {
                    foreach (ListViewItem item in lvModules.Items)
                    {
                        if (((Domain.Module)item.Tag).Id == depModule.Id)
                            item.Checked = true;
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void lvModules_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (resolveDependencies)
            {
                if (e.Item.Checked)
                {
                    resolveDependencies = false;
                    ResolveModuleDependencies((Domain.Module)e.Item.Tag);
                    resolveDependencies = true;
                }

                EnableDisableButtons();

                cbSelectAll.CheckedChanged -= cbSelectAll_CheckedChanged;
                cbSelectAll.Checked = lvModules.CheckedItems.Count == lvModules.Items.Count;
                cbSelectAll.CheckedChanged += cbSelectAll_CheckedChanged;
            }
        }

        private void btnStopGeneration_Click(object sender, EventArgs e)
        {
            btnStopGeneration.Enabled = false;
            doBreak = true;
        }

        private string secretword = string.Empty;

        private void GenerateFrontendCodeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && secretword == "timbul")
            {
                browseSolutionBtn.Visible = true;
                browseRefernceBtn.Visible = true;
            }
        }

        private void GenerateFrontendCodeForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            secretword += e.KeyChar;
            if (!"timbul".StartsWith(secretword))
            {
                secretword = string.Empty;
            }
        }

        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            resolveDependencies = false;

            foreach (ListViewItem item in lvModules.Items)
            {
                item.Checked = cbSelectAll.Checked;
            }

            resolveDependencies = true;

            EnableDisableButtons();
        }
    }
}
