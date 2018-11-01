using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
using System.Collections;
using NHibernate;

using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.GUI
{
    public partial class GenerateBackendCodeForm : MdiChildForm
    {
        private static IApplicationContext ctx;

        private IConfigurationManagementService configurationManagementService;
        private IModelService modelService;
        
        private DateTime generateStartTime;

        private bool doBreak = false;

        public GenerateBackendCodeForm()
        {
            InitializeComponent();
                        
            ctx = ContextRegistry.GetContext();

            configurationManagementService = MetaManagerServices.GetConfigurationManagementService();
            modelService = MetaManagerServices.GetModelService();

            configurationManagementService.StatusChanged += new StatusChangedDelegate(confMgnService_StatusChanged);
        }

        private void LoadServices()
        {
            IEnumerable<Service> serviceList = modelService.GetAllDomainObjectsByApplicationId<Service>(BackendApplication.Id).OrderBy(s => (s.Name));
                        
            lvServices.Items.Clear();

            foreach (Service service in serviceList)
            {
                ListViewItem item = new ListViewItem(service.Name);
                item.Checked = Config.Backend.GenerationFilter.Contains(service.Id.ToString());
                item.Tag = service;
                lvServices.Items.Add(item);
            }
        }
        
        private void browseSolutionBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(solutionPathTbx.Text.Trim()) || !Directory.Exists(solutionPathTbx.Text))
                folderDlg.SelectedPath = Path.GetFullPath(@"..\..\..\..\..\..\source\Auto\BE");
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
            
            //Config.Backend.SolutionName = solutionNameTbx.Text;
            Config.Backend.SolutionFolder = solutionPathTbx.Text;
            //Config.Backend.ReferenceFolder = referencePathTbx.Text;
            
            List<string> selectedServiceIds = (from ListViewItem item in lvServices.CheckedItems
                                              select ((Service)item.Tag).Id.ToString()).ToList();
            Config.Backend.GenerationFilter = selectedServiceIds;
            Config.Save();

            System.Threading.ThreadPool.QueueUserWorkItem(ThreadWork, selectedServiceIds);
        }

        private void ThreadWork(object state)
        {
            try
            {
                try
                {
                    using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
                    {

                        ((DataAccess.DomainInterceptor)MetaManagerServices.GetDomainInterceptor()).GetDataFromConfigurationManagement = true;

                        generateStartTime = DateTime.Now;

                        var selectedServices = (from id in (List<string>)state
                                               select modelService.GetDomainObject<Service>(Guid.Parse(id))).ToList();

                        // Get which business entities we need to generate depending on the selected services.
                        Dictionary<Guid, BusinessEntity> entityDictionary = new Dictionary<Guid, BusinessEntity>();

                        //Dictionary<Guid, IDomainObject> loadedObjects = new Dictionary<Guid, IDomainObject>();
                        List<Service> loadedServices = new List<Service>();

                        foreach (Service service in selectedServices)
                        {
                            //Service loadedService = (Service)loadedObjects[service.Id];
                            Service loadedService = modelService.GetDomainObject<Service>(service.Id);

                            loadedServices.Add(loadedService);

                            foreach (ServiceMethod method in loadedService.ServiceMethods)
                            {
                                if (method != null)
                                {
                                    if (!entityDictionary.ContainsKey(method.MappedToAction.BusinessEntity.Id))
                                        entityDictionary.Add(method.MappedToAction.BusinessEntity.Id, method.MappedToAction.BusinessEntity);
                                }
                            }
                        }
                        
                        ApplicationTemplate template = new ApplicationTemplate();
                        template.entities = entityDictionary.Values.ToList<BusinessEntity>();
                        template.services = loadedServices;
                        template.solutionFileName = Path.Combine(solutionPathTbx.Text, solutionNameTbx.Text + ".sln");
                        template.referenceDirectory = referencePathTbx.Text;
                        template.templateCallback = CodeSmithTemplateCallback;
                        
                        template.Render(TextWriter.Null);

                        UpdateStatusAndTime("Done!", 0, 0, 0);
                    }

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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error in Codegeneration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                ((DataAccess.DomainInterceptor)MetaManagerServices.GetDomainInterceptor()).GetDataFromConfigurationManagement = false;

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

        void confMgnService_StatusChanged(string message, int value, int min, int max)
        {
            UpdateStatusAndTime(message, value, min, max);
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

        private void EnableDisableButtons()
        {
            okBtn.Enabled = false;
            cancelBtn.Enabled = true;

            if (!string.IsNullOrEmpty(solutionNameTbx.Text) &&
                !string.IsNullOrEmpty(solutionPathTbx.Text) &&
                !string.IsNullOrEmpty(referencePathTbx.Text) &&
                lvServices.CheckedItems.Count > 0)
            {
                okBtn.Enabled = true;
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GenerateCodeForm_Load(object sender, EventArgs e)
        {
            tbApp.Text = string.Format("{0}", BackendApplication.Name);

            //solutionNameTbx.Text = Config.Backend.SolutionName;
            solutionPathTbx.Text = Config.Backend.SolutionFolder;
            //referencePathTbx.Text = Config.Backend.ReferenceFolder;

            if (string.IsNullOrEmpty(solutionPathTbx.Text))
            {
                solutionPathTbx.Text = Path.Combine(@"N:\source\Auto\BE\", tbApp.Text);
            }

            LoadServices();

            pProgress.Visible = false;
            EnableDisableButtons();
        }
                
        private void lvServices_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            EnableDisableButtons();

            cbSelectAll.CheckedChanged -= cbSelectAll_CheckedChanged;
            cbSelectAll.Checked = lvServices.CheckedItems.Count == lvServices.Items.Count;
            cbSelectAll.CheckedChanged += cbSelectAll_CheckedChanged;
        }

        private void btnStopGeneration_Click(object sender, EventArgs e)
        {
            btnStopGeneration.Enabled = false;
            doBreak = true;
        }

        private void solutionPathTbx_TextChanged(object sender, EventArgs e)
        {
            referencePathTbx.Text = Path.Combine(solutionPathTbx.Text, "Temp");
        }

        private string secretword = string.Empty;

        private void GenerateBackendCodeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && secretword == "timbul")
            {
                browseSolutionBtn.Visible = true;
                browseRefernceBtn.Visible = true;
            }
        }

        private void GenerateBackendCodeForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            secretword += e.KeyChar;
            if (!"timbul".StartsWith(secretword))
            {
                secretword = string.Empty;
            }
        }

        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            lvServices.ItemChecked -= lvServices_ItemChecked;
            
            foreach (ListViewItem item in lvServices.Items)
            {
                item.Checked = cbSelectAll.Checked;
            }


            lvServices.ItemChecked += lvServices_ItemChecked;
            
            EnableDisableButtons();
        }
    }
}
