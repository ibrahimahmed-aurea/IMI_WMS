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
using System.IO;
using Imi.SupplyChain.UX.Infrastructure;

namespace Cdc.MetaManager.GUI
{
    public partial class GenerateCode : MdiChildForm
    {
        private IModelService modelService;
        private IConfigurationManagementService configurationManagementService;
        private DateTime generateStartTime;
        private bool resolveDependencies = true;

        System.Threading.Thread workerThread;

        public GenerateCode()
        {
            InitializeComponent();

            modelService = MetaManagerServices.GetModelService();
            configurationManagementService = MetaManagerServices.GetConfigurationManagementService();

            modelService.StatusChanged += new StatusChangedDelegate(StatusChanged);
            configurationManagementService.StatusChanged += new StatusChangedDelegate(StatusChanged);
        }

        private void Generatebutton_Click(object sender, EventArgs e)
        {
            if (Generatebutton.Text == "Start")
            {
                // Check if N:\ is mapped
                string drive = Path.GetPathRoot("N:\\");
                if (!Directory.Exists(drive))
                {
                    MessageBox.Show("Drive " + drive + " not found or inaccessible. Please map drive.", "Error");
                    return;
                }

                IgnoreCheckOutscheckBox_CheckedChanged(this, null);
                Overall2label.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption);
                Overall3label.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption);

                Config.Backend.GenerationFilter = ServiceslistViewSort.CheckedItems.Cast<ListViewItem>().Select(i => ((Service)i.Tag).Id.ToString()).ToList();
                Config.Frontend.GenerationFilter = ModulesListViewSort.CheckedItems.Cast<ListViewItem>().Select(i => ((Module)i.Tag).Id.ToString()).ToList();
                Config.Save();

                Dictionary<Guid, List<Guid>> selectedModulesAndServices = new Dictionary<Guid, List<Guid>>();
                selectedModulesAndServices.Add(FrontendApplication.Id, ModulesListViewSort.CheckedItems.Cast<ListViewItem>().Select(i => ((Module)i.Tag).Id).ToList());
                selectedModulesAndServices.Add(BackendApplication.Id, ServiceslistViewSort.CheckedItems.Cast<ListViewItem>().Select(i => ((Service)i.Tag).Id).ToList());

                Generatebutton.Text = "Cancel";

                workerThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(ThreadWork));

                workerThread.Start(selectedModulesAndServices);
            }
            else
            {   
                if (workerThread != null)
                {
                    DataAccess.DomainXmlSerializationHelper.WaitingForCache = false;
                    workerThread.Abort();
                }
            }
        }


        private void ThreadWork(object state)
        {
            generateStartTime = DateTime.Now;

            try
            {
                Dictionary<Guid, List<Guid>> selectedModulesAndServices = ((Dictionary<Guid, List<Guid>>)state);

                modelService.GenerateApplication(new List<DataAccess.Domain.Application>() { FrontendApplication, BackendApplication }, IgnoreCheckOutscheckBox.Checked, selectedModulesAndServices);

                StatusChanged("Code Generation Complete!", 0, 0, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (this.InvokeRequired)
                {
                    this.Invoke(new System.Action(() =>
                    {
                        Generatebutton.Text = "Start";
                    }));
                }
                else
                {
                    Generatebutton.Text = "Start";
                }
            }

            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action(() =>
                {
                    Generatebutton.Text = "Start";
                }));
            }
            else
            {
                Generatebutton.Text = "Start";
            }
        }

        void StatusChanged(string message, int value, int min, int max)
        {
            TimeSpan x = new TimeSpan();
            if (generateStartTime != null)
            {
                x = DateTime.Now - generateStartTime;
            }

            if (message.StartsWith("[") & message.EndsWith("]"))
            {
                Color color;
                string[] info = message.Split(',');

                if (info[2].StartsWith("START"))
                {
                    color = Color.FromKnownColor(KnownColor.LemonChiffon);
                }
                else
                {
                    color = Color.FromKnownColor(KnownColor.LightGreen);
                }

                if (this.InvokeRequired)
                {
                    this.Invoke(new System.Action(() =>
                    {
                        switch (info[1])
                        {
                            case "1":
                                Overall1label.BackColor = color;
                                break;
                            case "2":
                                Overall2label.BackColor = color;
                                break;
                            case "3":
                                Overall3label.BackColor = color;
                                break;
                        }
                    })
                    );
                }
                else
                {
                    switch (info[1])
                    {
                        case "1":
                            Overall1label.BackColor = color;
                            break;
                        case "2":
                            Overall2label.BackColor = color;
                            break;
                        case "3":
                            Overall3label.BackColor = color;
                            break;
                    }
                }
            }
            else
            {

                if (this.InvokeRequired)
                {
                    this.Invoke(new System.Action(() =>
                    {
                        Statuslabel.Text = message;
                        GenerateProgressBar.Minimum = min;
                        GenerateProgressBar.Maximum = max;
                        GenerateProgressBar.Value = value;
                        if (generateStartTime != null)
                        {
                            TimeElapsedlabel.Text = string.Format("{0}:{1}:{2}", x.Hours.ToString("00"), x.Minutes.ToString("00"), x.Seconds.ToString("00"));
                        }
                    })
                    );
                }
                else
                {
                    Statuslabel.Text = message;
                    GenerateProgressBar.Minimum = min;
                    GenerateProgressBar.Maximum = max;
                    GenerateProgressBar.Value = value;
                    if (generateStartTime != null)
                    {
                        TimeElapsedlabel.Text = string.Format("{0}:{1}:{2}", x.Hours.ToString("00"), x.Minutes.ToString("00"), x.Seconds.ToString("00"));
                    }
                }
            }
        }

        private void GenerateCode_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadModulesAndServices();
            Cursor.Current = Cursors.Default;
        }

        private void LoadModulesAndServices()
        {
            IEnumerable<Module> moduleList = modelService.GetAllDomainObjectsByApplicationId<Module>(FrontendApplication.Id).OrderBy(m => (m.Name));
            IEnumerable<Service> serviceList = modelService.GetAllDomainObjectsByApplicationId<Service>(BackendApplication.Id).OrderBy(s => (s.Name));

            resolveDependencies = false;
            
            ModulesListViewSort.BeginUpdate();
            ServiceslistViewSort.BeginUpdate();
            ModulesListViewSort.Items.Clear();

            foreach (Module module in moduleList)
            {
                ListViewItem item = new ListViewItem(module.Name);
                item.Checked = Config.Frontend.GenerationFilter.Contains(module.Id.ToString());
                item.Tag = module;
                ModulesListViewSort.Items.Add(item);
            }

            ServiceslistViewSort.Items.Clear();

            foreach (Service service in serviceList)
            {
                ListViewItem item = new ListViewItem(service.Name);
                item.Checked = Config.Backend.GenerationFilter.Contains(service.Id.ToString());
                item.Tag = service;
                ServiceslistViewSort.Items.Add(item);
            }
            
            ModulesListViewSort_ItemChecked(this, null);
            ServiceslistViewSort_ItemChecked(this, null);
            
            ModulesListViewSort.EndUpdate();
            ServiceslistViewSort.EndUpdate();

            resolveDependencies = true;
        }

        private void ModulesListViewSort_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e == null || e.Item.Checked)
            {
                if (resolveDependencies)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    resolveDependencies = false;

                    List<Module> dependantModules = new List<Module>();
                    List<Service> dependantServices = new List<Service>();

                    MetaManagerServices.Helpers.ModuleHelper.FindAllModulesAndServicesReferancedByModule(((Module)e.Item.Tag), dependantModules, dependantServices);

                    foreach (ListViewItem item in ModulesListViewSort.Items)
                    {
                        if (dependantModules.Where(m => m.Id == ((DataAccess.IDomainObject)item.Tag).Id).Count() > 0)
                        {
                            item.Checked = true;
                        }
                    }

                    foreach (ListViewItem item in ServiceslistViewSort.Items)
                    {
                        if (dependantServices.Where(m => m.Id == ((DataAccess.IDomainObject)item.Tag).Id).Count() > 0)
                        {
                            item.Checked = true;
                        }
                    }

                    resolveDependencies = true;
                    Cursor.Current = Cursors.Default;
                }

                SelectAllModulescheckBox.CheckedChanged -= SelectAllModulescheckBox_CheckedChanged;
                SelectAllModulescheckBox.Checked = ModulesListViewSort.CheckedItems.Count == ModulesListViewSort.Items.Count;
                SelectAllModulescheckBox.CheckedChanged += SelectAllModulescheckBox_CheckedChanged;
            }
        }

        private void SelectAllModulescheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ModulesListViewSort.ItemChecked -= ModulesListViewSort_ItemChecked;

            foreach (ListViewItem item in ModulesListViewSort.Items)
            {
                item.Checked = SelectAllModulescheckBox.Checked;
            }


            ModulesListViewSort.ItemChecked += ModulesListViewSort_ItemChecked;
        }

        private void ServiceslistViewSort_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            SelectAllServicescheckBox.CheckedChanged -= SelectAllServicescheckBox_CheckedChanged;
            SelectAllServicescheckBox.Checked = ServiceslistViewSort.CheckedItems.Count == ServiceslistViewSort.Items.Count;
            SelectAllServicescheckBox.CheckedChanged += SelectAllServicescheckBox_CheckedChanged;
        }

        private void SelectAllServicescheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ServiceslistViewSort.ItemChecked -= ServiceslistViewSort_ItemChecked;

            foreach (ListViewItem item in ServiceslistViewSort.Items)
            {
                item.Checked = SelectAllServicescheckBox.Checked;
            }


            ServiceslistViewSort.ItemChecked += ServiceslistViewSort_ItemChecked;
        }

        private void GenerateCode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Generatebutton.Text == "Cancel")
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                }
            }
        }

        private void IgnoreCheckOutscheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Generatebutton.Text == "Start")
            {
                if (IgnoreCheckOutscheckBox.Checked)
                {
                    Overall1label.BackColor = Color.FromKnownColor(KnownColor.ControlLight);
                }
                else
                {
                    Overall1label.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption);
                }
            }
        }
    }
}
