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
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplates;
using System.Windows.Markup;
using System.Windows;
using Cdc.MetaManager.BusinessLogic.Helpers;

namespace Cdc.MetaManager.GUI
{
    public partial class ImportChange : MdiChildForm
    {
        private IModelService modelService = null;
        private IConfigurationManagementService configurationManagementService = null;
        private BusinessLogic.Helpers.IImportChangeHelper importChangeHelper = null;

        private List<DeltaListEntry> deltaList = null;

        public ImportChange()
        {
            InitializeComponent();
            modelService = MetaManagerServices.GetModelService();
            configurationManagementService = MetaManagerServices.GetConfigurationManagementService();
            importChangeHelper = MetaManagerServices.Helpers.ImportChangeHelper;
        }

        private void deltabutton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string issueID = IssueIdtextBox.Text;
                string additionalInfo = string.Empty;
                deltaList = importChangeHelper.GetChange(issueID, ImportViewPathtextBox.Text, out additionalInfo);

                if (deltaList != null)
                {
                    List<string> errorList = importChangeHelper.GetCurrentVersionOfObjects(deltaList, FrontendApplication, BackendApplication);

                    if (errorList.Count > 0)
                    {
                        string text = "Error: Unable to import issue \n\r\n\r";
                        foreach (string error in errorList)
                        {
                            text += error + "\n\r";
                        }


                        System.Windows.Forms.Clipboard.SetText(text);
                        Cursor = Cursors.Default;
                        System.Windows.Forms.MessageBox.Show(text);

                        return;
                    }

                    Dictionary<DeltaListEntry, Dictionary<Guid, Type>> changedObjectsWithMissingReferences = new Dictionary<DeltaListEntry, Dictionary<Guid, Type>>();
                    deltaList = importChangeHelper.GetSortedDeltaList(deltaList, out changedObjectsWithMissingReferences);

                    if (changedObjectsWithMissingReferences.Count > 0)
                    {
                        string errorText = "Error: Unable to import issue \n\rMissing references from issues prior to this listed below: \n\r\n\r";

                        foreach (KeyValuePair<DeltaListEntry, Dictionary<Guid, Type>> entry in changedObjectsWithMissingReferences)
                        {
                            string objectName = string.Empty;
                            Type objectType;
                            object entryobj = null;

                            if (entry.Key.CurrentObject_CurrentVersion != null)
                            {
                                entryobj = entry.Key.CurrentObject_CurrentVersion;
                            }
                            else if (entry.Key.CurrentObject_NewVersion != null)
                            {
                                entryobj = entry.Key.CurrentObject_NewVersion;
                            }

                            if (entryobj != null)
                            {
                                objectType = entryobj.GetType();
                                objectName = objectType.GetProperty("Name") != null && objectType.GetProperty("Name").GetValue(entryobj, null) != null ? objectType.GetProperty("Name").GetValue(entryobj, null).ToString() : "NoName";

                                errorText += "[" + objectType.Name + "] " + objectName;

                                if (typeof(IDomainObject).IsAssignableFrom(objectType))
                                {
                                    errorText += " Id: " + ((IDomainObject)entryobj).Id.ToString();
                                }

                                errorText += "\n\r";

                                foreach (KeyValuePair<Guid, Type> refEntry in entry.Value)
                                {
                                    errorText += "\t[" + refEntry.Value.Name + "] Id: " + refEntry.Key.ToString() + "\n\r";
                                }

                                errorText += "\n\r";
                            }
                        }

                        Cursor = Cursors.Default;
                        System.Windows.Forms.Clipboard.SetText(errorText);
                        System.Windows.Forms.MessageBox.Show(errorText);

                        return;
                    }


                    importChangeHelper.GetConflicts(deltaList);
                    //update GUI with conflicts
                }

                PresentChanges(issueID + "[" + additionalInfo + "]");

                ApplyChangesbutton.Enabled = true;
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void PresentChanges(string issueId)
        {
            ObjectstreeView.Nodes.Clear();

            TreeNode rootNode = ObjectstreeView.Nodes.Add("Issue: " + issueId);

            List<DeltaListEntry> postProcessQue = new List<DeltaListEntry>();

            if (deltaList != null)
            {
                foreach (DeltaListEntry delta in deltaList)
                {
                    WalkDeltaList(rootNode, delta, postProcessQue);
                }

                foreach (DeltaListEntry delta in postProcessQue)
                {
                    WalkDeltaList(rootNode, delta, null);
                }
            }
        }

        private void WalkDeltaList(TreeNode currentNode, DeltaListEntry currentListEntry, List<DeltaListEntry> postProcessQue)
        {
            if (currentListEntry != null)
            {
                //If CurrentListEntry is a component map
                //find the parent view and set currentNode to componentmap node for the view
                //if the view is not added yet put currentListEntry in que for later processing
                //------------------------------------------------------------------------------
                if (currentListEntry.OwnerId != Guid.Empty)
                {
                    Guid parentId = currentListEntry.OwnerId;
                    TreeNode parentNode = null;

                    foreach (TreeNode childNode in currentNode.Nodes)
                    {
                        if (childNode.Tag != null && typeof(DeltaListEntry).IsAssignableFrom(childNode.Tag.GetType()))
                        {
                            DeltaListEntry tmpDelta = (DeltaListEntry)childNode.Tag;

                            object deltaObject = (tmpDelta.CurrentObject_NewVersion == null ? tmpDelta.CurrentObject_OldVersion : tmpDelta.CurrentObject_NewVersion);

                            if (typeof(IDomainObject).IsAssignableFrom(deltaObject.GetType()))
                            {
                                IDomainObject deltaDomainObject = (IDomainObject)deltaObject;

                                if (deltaDomainObject.Id == parentId)
                                {
                                    if (childNode.Nodes.ContainsKey("Component Maps"))
                                    {
                                        parentNode = childNode.Nodes["Component Maps"];
                                        break;
                                    }
                                    else
                                    {
                                        parentNode = childNode.Nodes.Add("Component Maps");
                                        parentNode.Name = "Component Maps";
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (parentNode != null)
                    {
                        currentNode = parentNode;
                    }
                    else
                    {
                        if (postProcessQue != null)
                        {
                            postProcessQue.Add(currentListEntry);
                            return;
                        }
                    }
                }
                //------------------------------------------------------------------------------

                string nodeTitle = "";

                if (currentListEntry.CurrentObject_NewVersion != null)
                {
                    Type objectType = currentListEntry.CurrentObject_NewVersion.GetType();
                    string objectName = objectType.GetProperty("Name") != null && objectType.GetProperty("Name").GetValue(currentListEntry.CurrentObject_NewVersion, null) != null ? objectType.GetProperty("Name").GetValue(currentListEntry.CurrentObject_NewVersion, null).ToString() : "No Name";
                    int numChanges = 0;

                    foreach (ObjectChangeDescription change in currentListEntry.Changes)
                    {
                        if (change.ChangeType == ChangeTypes.Changed || change.ChangeType == ChangeTypes.Moved)
                        {
                            numChanges++;
                        }
                    }


                    if (currentListEntry.CurrentObject_CurrentVersion != null)
                    {
                        nodeTitle = "[" + objectType.Name.Split('.').LastOrDefault() + "] " + objectName + (numChanges > 0 ? " (" + numChanges.ToString() + " Changes)" : "");
                    }
                    else
                    {
                        nodeTitle = "[" + objectType.Name.Split('.').LastOrDefault() + "] " + objectName + " (New)";
                    }
                }
                else if (currentListEntry.CurrentObject_OldVersion != null)
                {
                    Type objectType = currentListEntry.CurrentObject_OldVersion.GetType();
                    string objectName = objectType.GetProperty("Name") != null && objectType.GetProperty("Name").GetValue(currentListEntry.CurrentObject_NewVersion, null) != null ? objectType.GetProperty("Name").GetValue(currentListEntry.CurrentObject_OldVersion, null).ToString() : "No Name";
                    nodeTitle = "[" + objectType.Name.Split('.').LastOrDefault() + "] " + objectName + " (Deleted)";
                }

                TreeNode newNode = currentNode.Nodes.Add(nodeTitle);
                newNode.Tag = currentListEntry;

                Dictionary<string, List<TreeNode>> changes = new Dictionary<string, List<TreeNode>>();

                foreach (ObjectChangeDescription change in currentListEntry.Changes)
                {

                    if (change.ChangeType == ChangeTypes.New || change.ChangeType == ChangeTypes.Deleted)
                    {
                        string propertyKey;

                        if (change.ChangedProperty != null)
                        {
                            propertyKey = change.ChangedProperty.Name;
                        }
                        else
                        {
                            propertyKey = "Children";
                        }

                        if (!changes.ContainsKey(propertyKey))
                        {
                            changes.Add(propertyKey, new List<TreeNode>());
                        }


                        object childObj = null;

                        if (change.NewValue != null)
                        {
                            childObj = change.NewValue;
                        }
                        else if (change.OldValue != null)
                        {
                            childObj = change.OldValue;
                        }

                        Type childObjectType = childObj.GetType();
                        string childObjectName = childObjectType.GetProperty("Name") != null && childObjectType.GetProperty("Name").GetValue(childObj, null) != null ? childObjectType.GetProperty("Name").GetValue(childObj, null).ToString() : "No Name";

                        string childTitle = "[" + childObjectType.Name.Split('.').LastOrDefault() + "] " + childObjectName + " (" + change.ChangeType.ToString() + ")";

                        TreeNode newChildNode = new TreeNode(childTitle);
                        newChildNode.Tag = change;

                        changes[propertyKey].Add(newChildNode);
                    }
                    else
                    {
                        if (change.conflict)
                        {
                            newNode.ForeColor = System.Drawing.Color.Red;
                            newNode.EnsureVisible();
                        }
                    }
                }

                foreach (List<TreeNode> children in changes.Values)
                {
                    newNode.Nodes.AddRange(children.ToArray());
                }

                foreach (DeltaListEntry child in currentListEntry.ChildObjects)
                {
                    WalkDeltaList(newNode, child, null);
                }

            }
        }

        private void ClearTabs()
        {
            //Object description
            ObjectPropertyGrid_newVersion.SelectedObject = null;
            ObjectPropertyGrid_oldVersion.SelectedObject = null;
            ObjectPropertyGrid_currentVersion.SelectedObject = null;
            
            //Changed Properties
            changedPropertieslistView.Clear();
            ApplyValueButton.Visible = false;
            OldValuetextBox.Clear();
            NewValuetextBox.Clear();
            CurrentValuetextBox.Clear();

            //Layout
            tabControl1.TabPages.Remove(tabPage3);
            OldLayoutElementHost.Child = null;
            NewLayoutElementHost.Child = null;
            CurrentLayoutElementHost.Child = null;

        }

        private void ObjectstreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ClearTabs();

            if (e.Node.Tag != null)
            {
                if (e.Node.Tag.GetType() == typeof(DeltaListEntry))
                {
                    DeltaListEntry delta = (DeltaListEntry)e.Node.Tag;

                    if (e.Node.Text.EndsWith("Changes)"))
                    {
                        if (tabControl1.TabPages.Count == 1 || (tabControl1.TabPages.Count > 1 && tabControl1.TabPages[1] != tabPage2))
                        {
                            tabControl1.TabPages.Insert(1, tabPage2);
                        }
                    }
                    else
                    {
                        if (tabControl1.TabPages.Count > 1 && tabControl1.TabPages[1] == tabPage2)
                        {
                            tabControl1.TabPages.RemoveAt(1);
                        }
                    }

                    ObjectPropertyGrid_newVersion.SelectedObject = delta.CurrentObject_NewVersion;
                    ObjectPropertyGrid_oldVersion.SelectedObject = delta.CurrentObject_OldVersion;
                    if (delta.CurrentObject_CurrentVersion != null)
                    {
                        ObjectPropertyGrid_currentVersion.SelectedObject = delta.CurrentObject_CurrentVersion;
                    }

                    foreach (ObjectChangeDescription change in delta.Changes)
                    {
                        string oldValue = string.Empty;
                        string newValue = string.Empty;
                        string currentValue = string.Empty;

                        if (change.ChangeType == ChangeTypes.Changed)
                        {
                            if (change.ChangedProperty != null)
                            {
                                if (typeof(DataAccess.IDomainObject).IsAssignableFrom(change.ChangedProperty.PropertyType))
                                {
                                    string typeName = change.ChangedProperty.PropertyType.Name.Split('.').LastOrDefault();

                                    if (change.OldValue != null)
                                    {
                                        oldValue = "[" + typeName + "] " + (change.OldValue is Guid ? ((Guid)change.OldValue).ToString() : ((IDomainObject)change.OldValue).Id.ToString());
                                    }

                                    if (change.NewValue != null)
                                    {
                                        newValue = "[" + typeName + "] " + (change.NewValue is Guid ? ((Guid)change.NewValue).ToString() : ((IDomainObject)change.NewValue).Id.ToString());
                                    }

                                    if (delta.CurrentObject_CurrentVersion != null)
                                    {
                                        if (delta.CurrentObject_CurrentVersion.GetType().GetProperties().Contains(change.ChangedProperty))
                                        {
                                            object tmpValue = change.ChangedProperty.GetValue(delta.CurrentObject_CurrentVersion, null);

                                            if (tmpValue != null)
                                            {
                                                currentValue = "[" + typeName + "]" + ((IDomainObject)tmpValue).Id.ToString();
                                            }
                                        }
                                    }




                                    ListViewItem newItem = changedPropertieslistView.Items.Add(change.ChangedProperty.Name);
                                    if (change.conflict)
                                    {
                                        newItem.ForeColor = System.Drawing.Color.Red;
                                    }
                                    newItem.Tag = new Dictionary<string, string>() { { "OLD", oldValue }, { "NEW", newValue }, { "CURRENT", currentValue } };
                                }
                                else
                                {
                                    if (change.OldValue != null)
                                    {
                                        oldValue = change.OldValue.ToString();
                                    }

                                    if (change.NewValue != null)
                                    {
                                        newValue = change.NewValue.ToString();
                                    }

                                    if (delta.CurrentObject_CurrentVersion != null)
                                    {
                                        if (delta.CurrentObject_CurrentVersion.GetType().GetProperties().Contains(change.ChangedProperty))
                                        {
                                            object tmpValue = change.ChangedProperty.GetValue(delta.CurrentObject_CurrentVersion, null);

                                            if (tmpValue != null)
                                            {
                                                currentValue = tmpValue.ToString();
                                            }
                                        }
                                    }

                                    ListViewItem newItem = changedPropertieslistView.Items.Add(change.ChangedProperty.Name);
                                    if (change.conflict)
                                    {
                                        newItem.ForeColor = System.Drawing.Color.Red;
                                    }
                                    newItem.Tag = new Dictionary<string, string>() { { "OLD", oldValue }, { "NEW", newValue }, { "CURRENT", currentValue } };

                                    
                                }
                            }
                            else
                            {
                                if (delta.CurrentObject_NewVersion != null)
                                {
                                    if (typeof(DataAccess.Domain.VisualModel.UXComponent).IsAssignableFrom(delta.CurrentObject_NewVersion.GetType()))
                                    {
                                        Type oldType = delta.CurrentObject_OldVersion.GetType();
                                        Type newType = delta.CurrentObject_NewVersion.GetType();
                                        Type currentType = null;

                                        if (delta.CurrentObject_CurrentVersion != null)
                                        {
                                            currentType = delta.CurrentObject_CurrentVersion.GetType();
                                        }


                                        ListViewItem newItem = changedPropertieslistView.Items.Add("Type");
                                        if (change.conflict)
                                        {
                                            newItem.ForeColor = System.Drawing.Color.Red;
                                        }
                                        newItem.Tag = new Dictionary<string, string>() { { "OLD", oldType.Name }, { "NEW", newType.Name }, { "CURRENT", (currentType != null ? currentType.Name : string.Empty) } };
                                    }
                                }
                            }
                        }
                        else if (change.ChangeType == ChangeTypes.Moved)
                        {
                            oldValue = change.OldValue.ToString();
                            newValue = change.NewValue.ToString();
                            ListViewItem newItem = changedPropertieslistView.Items.Add("Component Moved");
                            if (change.conflict)
                            {
                                newItem.ForeColor = System.Drawing.Color.Red;
                            }
                            newItem.Tag = new Dictionary<string, string>() { { "OLD", oldValue }, { "NEW", newValue }};
                        }
                    }

                    //View

                    string newXamlSource = string.Empty;
                    string oldXamlSource = string.Empty;
                    string currentXamlSource = string.Empty;

                    if (delta.CurrentObject_NewVersion is DataAccess.Domain.View)
                    {
                        tabControl1.TabPages.Add(tabPage3);

                        newXamlSource = GenerateXaml((DataAccess.Domain.View)delta.CurrentObject_NewVersion);
                        oldXamlSource = GenerateXaml((DataAccess.Domain.View)delta.CurrentObject_OldVersion);

                        if (delta.CurrentObject_CurrentVersion != null)
                        {
                            currentXamlSource = GenerateXaml((DataAccess.Domain.View)delta.CurrentObject_CurrentVersion);
                        }
                    }

                    if (newXamlSource != string.Empty)
                    {
                        RenderXaml(newXamlSource, NewLayoutElementHost);
                    }

                    if (oldXamlSource != string.Empty)
                    {
                        RenderXaml(oldXamlSource, OldLayoutElementHost);
                    }
                    
                    if (currentXamlSource != string.Empty)
                    {
                        RenderXaml(currentXamlSource, CurrentLayoutElementHost);
                    }
                    


                    //Action
                    NewSQLrichTextBox.Text = "";
                    OldSQLrichTextBox.Text = "";

                    if (delta.CurrentObject_NewVersion is DataAccess.Domain.Action)
                    {
                        DataAccess.Domain.Action newAction = (DataAccess.Domain.Action)delta.CurrentObject_NewVersion;
                        DataAccess.Domain.Action oldAction = null;

                        if (delta.CurrentObject_OldVersion != null)
                        {
                            oldAction = (DataAccess.Domain.Action)delta.CurrentObject_OldVersion;
                        }

                        if (newAction.Query != null)
                        {
                            NewSQLrichTextBox.Text = newAction.Query.SqlStatement;

                            if (oldAction != null)
                            {
                                OldSQLrichTextBox.Text = oldAction.Query.SqlStatement;
                            }
                        }
                    }

                }
                else
                {
                    ObjectChangeDescription change = (ObjectChangeDescription)e.Node.Tag;

                    ObjectPropertyGrid_newVersion.SelectedObject = change.NewValue;
                    ObjectPropertyGrid_oldVersion.SelectedObject = change.OldValue;

                }
            }

            Cursor = Cursors.Default;
        }

        private string GenerateXaml(Cdc.MetaManager.DataAccess.Domain.View renderView)
        {
            string source = string.Empty;

            if (renderView != null)
            {
                ViewXamlTemplate viewXamlTemplate = new ViewXamlTemplate();
                viewXamlTemplate.view = renderView;
                viewXamlTemplate.debugOutput = true;
                source = viewXamlTemplate.RenderToString();
            }

            return source;
        }

        private void RenderXaml(string source, System.Windows.Forms.Integration.ElementHost elementHost)
        {
            if (string.IsNullOrEmpty(source))
                return;

            try
            {
                InnerRender(source, elementHost);
            }
            catch (Exception ex)
            {
                string xamlError = string.Format("<WrapPanel xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"><TextBlock>{0}</TextBlock></WrapPanel>", ex.Message);
                try
                {
                    InnerRender(xamlError, elementHost);
                }
                catch (Exception) { }
            }

            return;
        }

        private void InnerRender(string source, System.Windows.Forms.Integration.ElementHost elementHost)
        {
            Stream s = new MemoryStream(UTF8Encoding.UTF8.GetBytes(source));
            XamlReader r = new XamlReader();

            UIElement uiElement = XamlReader.Load(s) as UIElement;

            elementHost.Child = uiElement;
            elementHost.Child.Dispatcher.UnhandledException += (sender, e) =>
            {
                e.Handled = true;
            };
        }

        private TreeNode previousSelectedNode = null;

        private void ObjectstreeView_Validating(object sender, CancelEventArgs e)
        {
            if (ObjectstreeView.SelectedNode != null)
            {
                ObjectstreeView.SelectedNode.BackColor = System.Drawing.SystemColors.Highlight;
                if (ObjectstreeView.SelectedNode.Tag is DeltaListEntry && ((DeltaListEntry)ObjectstreeView.SelectedNode.Tag).HasConflicts)
                {
                    ObjectstreeView.SelectedNode.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ObjectstreeView.SelectedNode.ForeColor = Color.White;
                }

                previousSelectedNode = ObjectstreeView.SelectedNode;
            }

        }

        private void ObjectstreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (previousSelectedNode != null)
            {
                previousSelectedNode.BackColor = ObjectstreeView.BackColor;
                if (previousSelectedNode.Tag is DeltaListEntry && ((DeltaListEntry)previousSelectedNode.Tag).HasConflicts)
                {
                    previousSelectedNode.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    previousSelectedNode.ForeColor = ObjectstreeView.ForeColor;
                }
            }

        }

        private void ApplyChangesbutton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                foreach (DeltaListEntry delta in deltaList)
                {
                    if (!delta.PreviewMode)
                    {
                        delta.SwitchCurrentOjects();
                    }
                }

                importChangeHelper.ApplyChange(deltaList, false);

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            if (deltaList != null)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    foreach (DeltaListEntry delta in deltaList)
                    {
                        if (delta.PreviewMode)
                        {
                            delta.SwitchCurrentOjects();
                        }
                    }
                    
                    //Try to check out
                    foreach (DeltaListEntry delta in deltaList)
                    {
                        if (delta.CurrentObject_CurrentVersion != null && typeof(IVersionControlled).IsAssignableFrom(delta.CurrentObject_CurrentVersion.GetType()))
                        {
                            configurationManagementService.CheckOutDomainObject(((IVersionControlled)delta.CurrentObject_CurrentVersion).Id, delta.CurrentObject_CurrentVersion.GetType());
                        }
                    }

                    importChangeHelper.ApplyChange(deltaList, true);

                    Cursor = Cursors.Default;
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
        }

        private void importViewbutton_Click(object sender, EventArgs e)
        {
            //System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
            //appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();
            importview_folderBrowserDialog.ShowDialog();

            ImportViewPathtextBox.Text = importview_folderBrowserDialog.SelectedPath;

            Config.Global.LastSelectedImportChangeSourceView = ImportViewPathtextBox.Text;
            Config.Save();
        }

        private void changedPropertieslistView_SelectedIndexChanged(object sender, EventArgs e)
        {
            OldValuetextBox.Clear();
            NewValuetextBox.Clear();
            CurrentValuetextBox.Clear();
            ApplyValueButton.Visible = false;

            if (changedPropertieslistView.SelectedItems.Count > 0)
            {
                if (changedPropertieslistView.SelectedItems[0].Tag != null)
                {
                    foreach (string key in ((Dictionary<string, string>)changedPropertieslistView.SelectedItems[0].Tag).Keys)
                    {
                        switch (key)
                        {
                            case "NEW":
                                NewValuetextBox.Text = ((Dictionary<string, string>)changedPropertieslistView.SelectedItems[0].Tag)[key];
                                break;
                            case "OLD":
                                OldValuetextBox.Text = ((Dictionary<string, string>)changedPropertieslistView.SelectedItems[0].Tag)[key];
                                break;
                            case "CURRENT":
                                CurrentValuetextBox.Text = ((Dictionary<string, string>)changedPropertieslistView.SelectedItems[0].Tag)[key];
                                break;
                        }
                    }

                    if (changedPropertieslistView.SelectedItems[0].Text == "SqlStatement" && ((DeltaListEntry)ObjectstreeView.SelectedNode.Tag).HasConflicts)
                    {
                        NewValuetextBox.ReadOnly = false;
                        ApplyValueButton.Visible = true;
                    }
                }
            }
        }

        private void ImportViewPathtextBox_TextChanged(object sender, EventArgs e)
        {
            deltabutton.Enabled = !string.IsNullOrEmpty(ImportViewPathtextBox.Text);
        }

        private void ApplyValueButton_Click(object sender, EventArgs e)
        {
            if (ObjectstreeView.SelectedNode.Tag != null)
            {
                if (ObjectstreeView.SelectedNode.Tag.GetType() == typeof(DeltaListEntry))
                {
                    DeltaListEntry delta = (DeltaListEntry)ObjectstreeView.SelectedNode.Tag;

                    if (delta.Changes.Where(c => c.ChangedProperty != null && c.ChangedProperty.Name == changedPropertieslistView.SelectedItems[0].Text).Count() > 0)
                    {
                        ObjectChangeDescription change = delta.Changes.Where(c => c.ChangedProperty != null && c.ChangedProperty.Name == changedPropertieslistView.SelectedItems[0].Text).First();

                        change.NewValue = NewValuetextBox.Text;

                        change.ChangedProperty.SetValue(delta.CurrentObject_NewVersion, NewValuetextBox.Text, null);
                    }
                }
            }
        }

        private void ImportChange_Load(object sender, EventArgs e)
        {
            ImportViewPathtextBox.Text = Config.Global.LastSelectedImportChangeSourceView;
        }
    }
}
