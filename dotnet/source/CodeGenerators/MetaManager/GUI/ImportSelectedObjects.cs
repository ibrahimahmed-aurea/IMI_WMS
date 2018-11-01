using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Context;
using Spring.Context.Support;
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.BusinessLogic;
using NHibernate;


namespace Cdc.MetaManager.GUI
{
    public partial class ImportSelectedObjects : MdiChildForm
    {
        private IModelService modelService = null;
        private IConfigurationManagementService configurationManagementService = null;

        public ImportSelectedObjects()
        {
            InitializeComponent();
            modelService = MetaManagerServices.GetModelService();
            configurationManagementService = MetaManagerServices.GetConfigurationManagementService();
        }

        private void ImportSelectedObjects_Load(object sender, EventArgs e)
        {
            ResultlistView.Items.Clear();
            configurationManagementService.StatusChanged += new StatusChangedDelegate(confMgnService_StatusChanged);
        }


        private void SelectAllcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ResultlistView.Items)
            {
                item.Checked = SelectAllcheckBox.Checked;
            }
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "xml files (*.xml)|*.xml";
            dialog.RestoreDirectory = true;
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    SelectAllcheckBox.Checked = false;
                    System.Windows.Forms.Application.DoEvents();


                    ResultlistView.BeginUpdate();

                    foreach (String file in dialog.FileNames)
                    {

                        ListViewItem item = null;

                        item = ResultlistView.Items.Add(file);

                    }
                    ResultlistView.EndUpdate();

                }
                finally
                {
                    Cursor = Cursors.Default;

                }

            }
        }

        private void clearListBtn_Click(object sender, EventArgs e)
        {
            ResultlistView.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ResultlistView.CheckedItems.Count > 0)
                {
                    Cursor = Cursors.WaitCursor;
                    addFilesBtn.Enabled = false;
                    StatusprogressBar.Maximum = ResultlistView.CheckedItems.Count * 2;
                    StatusprogressBar.Minimum = 0;
                    StatusprogressBar.Value = 0;
                    Boolean stopped = false;

                    foreach (ListViewItem item in ResultlistView.CheckedItems)
                    {
                        AddToProgressText("\nCotrolls if the object is available: " + item.SubItems[0].Text);
                        System.Windows.Forms.Application.DoEvents();

                        Type classType = MetaManagerServices.GetConfigurationManagementService().GetClassTypeAndIdForXML(item.SubItems[0].Text).Key;
                        Guid id = MetaManagerServices.GetConfigurationManagementService().GetClassTypeAndIdForXML(item.SubItems[0].Text).Value;

                        DataAccess.IVersionControlled readDomainObjectfromDB = ((DataAccess.IVersionControlled)modelService.GetDomainObject(id, classType));


                        if (readDomainObjectfromDB != null)
                        {
                            if (readDomainObjectfromDB.IsLocked && readDomainObjectfromDB.LockedBy != Environment.UserName)
                            {
                                MessageBox.Show("The selected object " + item.SubItems[0].Text + "is checked out by another user it is not possible import this");
                                AddToProgressText("\nImport stopped. The selected object " + item.SubItems[0].Text + "is checked out by another user. It is not possible import this.\n");
                                stopped = true;
                            }
                            if (stopped)
                            {
                                break;
                            }

                            StatusprogressBar.Value++;
                            System.Windows.Forms.Application.DoEvents();


                        }
                    }

                    if (stopped)
                    {
                        return;
                    }

                    List<string> existingObjectsPaths = new List<string>();
                    List<string> newObjectsPaths = new List<string>();
                   

                    foreach (ListViewItem item in ResultlistView.CheckedItems)
                    {
                        Type classType = MetaManagerServices.GetConfigurationManagementService().GetClassTypeAndIdForXML(item.SubItems[0].Text).Key;
                        Guid id = MetaManagerServices.GetConfigurationManagementService().GetClassTypeAndIdForXML(item.SubItems[0].Text).Value;

                        DataAccess.IVersionControlled readDomainObjectfromDB = ((DataAccess.IVersionControlled)modelService.GetDomainObject(id, classType));


                        if (readDomainObjectfromDB != null)
                        {
                            //Existing object
                            AddToProgressText("\nCheck out the object: " + item.SubItems[0].Text);
                            System.Windows.Forms.Application.DoEvents();

                            try
                            {
                                MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(readDomainObjectfromDB.Id, readDomainObjectfromDB.GetType());
                            }

                            catch (ConfigurationManagementException ex)
                            {
                                MessageBox.Show(ex.Message);
                                AddToProgressText("\nImport stopped. The selected object " + item.SubItems[0].Text + "is checked out by another user. It is not possible import this.\n");
                                stopped = true;
                            }
                            if (stopped)
                            {
                                break;
                            }
                            AddToProgressText("\nChecked out the selected object " + item.SubItems[0].Text);

                            existingObjectsPaths.Add(item.SubItems[0].Text);
                        }
                        else
                        {
                            //New object
                            AddToProgressText("\nFound new object: " + item.SubItems[0].Text);
                            System.Windows.Forms.Application.DoEvents();
                            newObjectsPaths.Add(item.SubItems[0].Text);
                        }

                        StatusprogressBar.Value++;
                        System.Windows.Forms.Application.DoEvents();
                    }

                    if (stopped == false)
                    {

                        List<string>[] pathLists = new List<string>[2];

                        pathLists[0] = newObjectsPaths;
                        pathLists[1] = existingObjectsPaths;

                        System.Threading.ThreadPool.QueueUserWorkItem(ThreadWork, pathLists);
                        
                    }

                }
            }

            catch (BusinessLogic.ConfigurationManagementException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
                addFilesBtn.Enabled = true;

            }

        }

        private bool CheckSelectedObjects()
        {
            throw new NotImplementedException();
        }

        void confMgnService_StatusChanged(string message, int value, int min, int max)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action(() =>
                {
                    AddToProgressText("\n" + message);
                    StatusprogressBar.Minimum = min;
                    StatusprogressBar.Maximum = max;
                    StatusprogressBar.Value = value;
                })
                );
            }
            else
            {
                AddToProgressText("\n" + message);
                StatusprogressBar.Minimum = min;
                StatusprogressBar.Maximum = max;
                StatusprogressBar.Value = value;
            }
        }

        private void ThreadWork(object state)
        {
            try
            {
                List<String>[] pathLists = (List<String>[])state;

                if (pathLists[0].Count > 0)
                {
                    configurationManagementService.ImportDomainObjects(pathLists[0], true, false);
                }

                if (pathLists[1].Count > 0)
                {
                    configurationManagementService.ImportDomainObjects(pathLists[1], true, false);
                }

                confMgnService_StatusChanged("All done!", 0, 0, 0);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                //MessageBox.Show(ex.Message);
                confMgnService_StatusChanged(ex.Message, 0, 0, 0);
            }
        }

        private void AddToProgressText(string text)
        {
            tbProgress.Text += text.Replace("\n", Environment.NewLine);

            tbProgress.Select(tbProgress.Text.Length + 1, 2);
            tbProgress.ScrollToCaret();
        }



        private void addFilesBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "xml files (*.xml)|*.xml";
            dialog.RestoreDirectory = true;
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    SelectAllcheckBox.Checked = false;
                    System.Windows.Forms.Application.DoEvents();


                    ResultlistView.BeginUpdate();

                    foreach (String file in dialog.FileNames)
                    {

                        ListViewItem item = null;

                        item = ResultlistView.Items.Add(file);

                    }
                    ResultlistView.EndUpdate();

                }
                finally
                {
                    Cursor = Cursors.Default;

                }

            }
        }


    }
}
