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
    public partial class FindCheckOuts : MdiChildForm
    {
        private IModelService _modelService = null;

        public FindCheckOuts()
        {
            InitializeComponent();
            _modelService = MetaManagerServices.GetModelService();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            MetaManagerServices.GetConfigurationManagementService().StatusChanged += new StatusChangedDelegate(FindCheckOuts_StatusChanged);
            checkoutComboBox.SelectedIndex = 0;
            checkoutComboBox.Focus();
        }

        private void FindCheckOuts_StatusChanged(string message, int value, int min, int max)
        {
            progressBar.Minimum = min;
            progressBar.Maximum = max;
            progressBar.Value = value;

            AddToProgressText(message);
        }
                
        private void checkOutscomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkoutComboBox.SelectedIndex == 2)
            {
                Usernamelabel.Visible = true;
                UserNameTextBox.Visible = true;
            }
            else
            {
                Usernamelabel.Visible = false;
                UserNameTextBox.Visible = false;
            }
        }

        private void resultListView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (resultListView.Items[e.Index].SubItems[4].Text != Environment.UserName)
            {
                e.NewValue = CheckState.Unchecked;
            }
        }

        private void selectAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in resultListView.Items)
            {
                item.Checked = selectAllcheckBox.Checked;
            }
        }

        private void checkinButton_Click(object sender, EventArgs e)
        {
            try
            {
                progressTextBox.Text = "";

                if (resultListView.CheckedItems.Count > 0)
                {
                    Cursor = Cursors.WaitCursor;
                    searchButton.Enabled = false;
                    progressBar.Maximum = resultListView.CheckedItems.Count;
                    progressBar.Minimum = 0;
                    progressBar.Value = 0;

                    System.Windows.Forms.Application.DoEvents();

                    IList<IVersionControlled> selectedObjects = new List<IVersionControlled>();

                    foreach (ListViewItem item in resultListView.CheckedItems)
                    {
                        IVersionControlled domainObject = ((KeyValuePair<IVersionControlled, DataAccess.Domain.Application>)item.Tag).Key;
                        
                        if (GetChildFormsForDomainObject(domainObject).Count > 0)
                        {
                            MessageBox.Show("You must close all windows handling the " + _modelService.GetDomainObjectType(domainObject).Name + " before checking in.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }

                    foreach (ListViewItem item in resultListView.CheckedItems)
                    {
                        IVersionControlled domainObject = ((KeyValuePair<IVersionControlled, DataAccess.Domain.Application>)item.Tag).Key;
                        selectedObjects.Add(domainObject);
                    }

                    MetaManagerServices.GetConfigurationManagementService().CheckInDomainObjects(selectedObjects);
                }
            }
            catch (Exception ex)
            {
                progressBar.Value = 0;
                AddToProgressText("Check in failed.");
                AddToProgressText(ex.ToString());
            }
            finally
            {
                Cursor = Cursors.Default;
                searchButton.Enabled = true;
                searchButton_Click(sender, e);
           }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                resultListView.Items.Clear();
                selectAllcheckBox.Checked = false;
                System.Windows.Forms.Application.DoEvents();

                resultListView.Items.Clear();
                resultListView.BeginUpdate();

                addObjectsToListView(FrontendApplication.Id, "Frontend", FrontendApplication);
                addObjectsToListView(BackendApplication.Id, "Backend", BackendApplication);
                
                resultListView.EndUpdate();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        
        private void addObjectsToListView(Guid applicationId, string applicationName, DataAccess.Domain.Application application)
        {
            List<IVersionControlled> checkedOutObjectsList = new List<IVersionControlled>();
            checkedOutObjectsList.Clear();
            checkedOutObjectsList.AddRange(_modelService.GetAllVersionControlledObjectsInApplication(applicationId));
            checkedOutObjectsList.RemoveAll(item => item == null);
            checkedOutObjectsList = checkedOutObjectsList.OrderBy(o => _modelService.GetDomainObjectType(o).Name).ToList();

            if (checkedOutObjectsList != null && checkedOutObjectsList.Count > 0)
            {
                foreach (IVersionControlled vobj in checkedOutObjectsList)
                {
                    if (vobj != null)
                    {
                        Boolean addToList = false;

                        if (checkoutComboBox.SelectedItem.ToString() == "My" && vobj.IsLocked && vobj.LockedBy == Environment.UserName)
                        {
                            addToList = true;
                        }

                        if (checkoutComboBox.SelectedItem.ToString() == "All" && vobj.IsLocked)
                        {
                            addToList = true;
                        }

                        if (checkoutComboBox.SelectedItem.ToString() == "Users" && vobj.IsLocked && vobj.LockedBy == UserNameTextBox.Text)
                        {
                            addToList = true;
                        }

                        if (addToList)
                        {
                            ListViewItem item = null;

                            IDomainObject theObject = null;

                            theObject = _modelService.GetDomainObject(vobj.Id, _modelService.GetDomainObjectType(vobj));

                            Dictionary<string, System.Reflection.PropertyInfo> propertyDic = theObject.GetType().GetProperties().ToDictionary(o => o.Name.ToUpper(), o => o);

                            item = resultListView.Items.Add(applicationName);

                            item.SubItems.Add(_modelService.GetDomainObjectType(theObject).Name);
                            item.SubItems.Add(theObject.Id.ToString());

                            if (propertyDic.ContainsKey("NAME") && propertyDic["NAME"].GetValue(theObject, null) != null)
                            {
                                item.SubItems.Add(propertyDic["NAME"].GetValue(theObject, null).ToString());
                            }
                            else
                            {
                                item.SubItems.Add("<Noname>");
                            }
                            item.SubItems.Add(vobj.LockedBy);

                            if (vobj.State.ToString() == "New")
                                item.SubItems.Add("Yes");
                            else
                                item.SubItems.Add("No");

                            item.Tag = new KeyValuePair<IVersionControlled, DataAccess.Domain.Application>(vobj, application);

                        }
                    }
                }
            }
        }


        public List<MdiChildForm> GetChildFormsForDomainObject(DataAccess.IDomainObject domainObject)
        {
            List<MdiChildForm> childList = new List<MdiChildForm>();
            Guid containedObjectId;
            Type containedObjectType;

            foreach (Form child in this.ParentForm.MdiChildren)
            {
                if (child is MdiChildForm)
                {
                    containedObjectId = ((MdiChildForm)child).ContaindDomainObjectIdAndType.Key;
                    containedObjectType = ((MdiChildForm)child).ContaindDomainObjectIdAndType.Value;

                    if (containedObjectType != null)
                    {
                        if (containedObjectId == domainObject.Id && containedObjectType == _modelService.GetDomainObjectType(domainObject))
                        {
                            childList.Add((MdiChildForm)child);
                        }
                    }
                }
            }

            return childList;
        }

        private void AddToProgressText(string text)
        {
            progressTextBox.Text += text + Environment.NewLine;

            progressTextBox.Select(progressTextBox.Text.Length + 1, 2);
            progressTextBox.ScrollToCaret();

            System.Windows.Forms.Application.DoEvents();
        }

        private void undoCheckoutButton_Click(object sender, EventArgs e)
        {
            try
            {
                progressTextBox.Text = "";

                if (resultListView.CheckedItems.Count > 0)
                {
                    Cursor = Cursors.WaitCursor;
                    searchButton.Enabled = false;
                    progressBar.Maximum = resultListView.CheckedItems.Count;
                    progressBar.Minimum = 0;
                    progressBar.Value = 0;
                    
                    foreach (ListViewItem item in resultListView.CheckedItems)
                    {
                        
                        System.Windows.Forms.Application.DoEvents();

                        //Do check in

                        IVersionControlled domainObject = ((KeyValuePair<IVersionControlled, DataAccess.Domain.Application>)item.Tag).Key;
                        DataAccess.Domain.Application application = ((KeyValuePair<IVersionControlled, DataAccess.Domain.Application>)item.Tag).Value;
                        
                        AddToProgressText(string.Format("Undoing check out for \"{0}\".", domainObject.ToString()));

                        MetaManagerServices.GetConfigurationManagementService().UndoCheckOutDomainObject(domainObject.Id, domainObject.GetType(), application);

                        progressBar.Value++;
                        System.Windows.Forms.Application.DoEvents();
                    }
                    
                    AddToProgressText("Undo check out completed successfully.");
                }
            }
            catch (Exception ex)
            {
                AddToProgressText("Undo check out failed.");
                AddToProgressText(ex.ToString());
            }
            finally
            {
                Cursor = Cursors.Default;
                searchButton.Enabled = true;
                searchButton_Click(sender, e);
            }
        }
                
        private void compareWithLatestVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (resultListView.SelectedItems.Count > 0)
            {
                Cursor = Cursors.WaitCursor;
                try
                {
                    KeyValuePair<IVersionControlled, DataAccess.Domain.Application> item = ((KeyValuePair<IVersionControlled, DataAccess.Domain.Application>)resultListView.SelectedItems[0].Tag);

                    MetaManagerServices.GetConfigurationManagementService().DiffWithPreviousVersion(item.Key, item.Value);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void jumpToObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (resultListView.SelectedItems.Count > 0)
            {
                Main parent = (Main)this.MdiParent;

                parent.JumpToDomainObject(((KeyValuePair<IVersionControlled, DataAccess.Domain.Application>)resultListView.SelectedItems[0].Tag).Key, false);
            }
        }
    }
}
