using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Context;
using Spring.Context.Support;
using System.Diagnostics;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;
using NHibernate;
using System.IO;
using ICSharpCode.TextEditor.Util;
using Domain = Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.GUI.Workflow;
using System.Configuration;
using System.Collections.Specialized;

namespace Cdc.MetaManager.GUI
{
    public partial class Main : Form
    {
        private IApplicationService appService = null;
        private IAnalyzeService analyzeService = null;
        private List<DeploymentGroup> deploymentGroups = null;
        private IDialogService dialogService = null;

        private IModelService modelService = null;
        private IModelChangeNotificationService modelChangeNotificationService = null;

        private DataTable searchTable = null;
        private DataRow[] searchResult = null;
        private bool SearchTableLocked = false;

        private bool pauseLoadThread = false;
        private bool stopLoading = false;
        private System.Threading.Thread loadThread = null;
        private List<System.Threading.Thread> workerThreads = new List<System.Threading.Thread>();

        private bool loadingSearchTable = false;

        private DataAccess.IVersionControlled objectToMove = null;

        private delegate void ThreadJumpDelegate();


        public Main()
        {
            this.Hide();
            SplashForm splashForm = new SplashForm();
            System.Threading.Thread ft = new System.Threading.Thread(new System.Threading.ThreadStart(splashForm.Show));

            ft.Start();
            System.Threading.Thread.Sleep(0); // Yield

            InitializeComponent();

            MetaManagerServices.GetContext();

            // Get service context
            appService = MetaManagerServices.GetApplicationService();
            analyzeService = MetaManagerServices.GetAnalyzeService();
            dialogService = MetaManagerServices.GetDialogService();
            
            modelService = MetaManagerServices.GetModelService();
            
            modelChangeNotificationService = MetaManagerServices.GetModelChangeNotificationService();

            // Create the list for the DeploymentGroups
            deploymentGroups = new List<DeploymentGroup>();
                        
            modelChangeNotificationService.DomainObjectChange += new DomainObjectChangeDelegate(modelChangeNotificationService_DomainObjectChanged);

            // Show connectionstring in statusbar
            IApplicationContext ctx = ContextRegistry.GetContext();
            ISessionFactory sessionFactory = ctx["SessionFactory"] as ISessionFactory;
            ISession openSession = sessionFactory.OpenSession();
            IDbConnection connection = openSession.Connection;
            tslblConnectionString.Text = connection.ConnectionString;
            connection.Close();

            CleanWorkflows();

            DateTime stop = DateTime.Now.AddSeconds(12);
            splashForm.Fade();
            System.Threading.Thread.Sleep(0); // Yield

            // The timepsan logic is needed for remote desktop
            // where the fade doesn't happen every time for
            // some reason and the splash does not disappear.
            while (splashForm.Opacity > 0)
            {
                System.Threading.Thread.Sleep(10);
                if (DateTime.Now > stop)
                {
                    ft.Interrupt();
                }
            }

            this.WindowState = FormWindowState.Maximized;
        }

        void modelChangeNotificationService_DomainObjectChanged(Guid Id, Type objectType, ObjectChangeTypes objectChangeType)
        {
            string searchCriteria = string.Empty;
            searchCriteria = "Id = '" + Id.ToString() + "' AND Type = '" + objectType.Name + "'";
            DataRow[] result = null;

            switch (objectChangeType)
            {
                case ObjectChangeTypes.Changed:
                    result = SearchLoadedNodes(searchCriteria, false, true);
                    if (result.Length > 0)
                    {
                        UpdateNode((TreeNode)result[0]["TreeNode"]);
                    }
                    break;
                case ObjectChangeTypes.Added:
                    result = SearchLoadedNodes(searchCriteria, false, true);
                    if (result.Length > 0)
                    {
                        UpdateNode((TreeNode)result[0]["TreeNode"]);
                    }
                    else
                    {
                        bool foundParent = false;
                        bool isFrontend;
                        DataAccess.IDomainObject obj = modelService.GetInitializedDomainObject(Id, objectType);
                        List<DataAccess.IDomainObject> parents = new List<DataAccess.IDomainObject>();
                        IList<DataAccess.IVersionControlled> vparents = modelService.GetVersionControlledParent(obj, out parents);
                        foreach (DataAccess.IVersionControlled vparent in vparents)
                        {
                            searchCriteria = "Id = '" + vparent.Id.ToString() + "' AND Type = '" + modelService.GetDomainObjectType(vparent).Name + "'";
                            result = SearchLoadedNodes(searchCriteria, false, true);
                            if (result.Length > 0)
                            {
                                foreach (TreeNode cnode in ((TreeNode)result[0]["TreeNode"]).Nodes)
                                {
                                    if (cnode.Tag is KeyValuePair<string, Type>)
                                    {
                                        if (((KeyValuePair<string, Type>)cnode.Tag).Value == objectType)
                                        {
                                            TreeNode newNode = CreateNode(obj, out isFrontend);

                                            if (cnode.Nodes.Count == 0)
                                            {
                                                cnode.Nodes.Add(newNode);
                                                cnode.Expand();
                                            }
                                            else
                                            {
                                                for (int i = 0; i < cnode.Nodes.Count; i++)
                                                {
                                                    if (string.Compare(newNode.Text, cnode.Nodes[i].Text) < 1)
                                                    {
                                                        cnode.Nodes.Insert(i, newNode);
                                                        break;
                                                    }
                                                    if (i + 1 == cnode.Nodes.Count)
                                                    {
                                                        cnode.Nodes.Add(newNode);
                                                        break;
                                                    }
                                                }
                                            }

                                            AddToSerachTable(newNode);
                                            foundParent = true;
                                        }
                                    }

                                    if (foundParent) { break; }
                                }
                            }

                            if (foundParent) { break; }
                        }
                    }
                    break;
                case ObjectChangeTypes.Deleted:
                    result = SearchLoadedNodes(searchCriteria, false, true);
                    if (result.Length > 0)
                    {
                        ((TreeNode)result[0]["TreeNode"]).Parent.Nodes.Remove((TreeNode)result[0]["TreeNode"]);
                        RemoveFromSerachTable(result[0]);
                    }
                    break;
            }
        }

        private void CleanWorkflows()
        {
            string[] files = Directory.GetFiles(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Workflow_*.dll");

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                }
            }
        }

        public Cdc.MetaManager.DataAccess.Domain.Application BackendApplication
        {
            get
            {
                DeploymentGroup deploymentGroup = (DeploymentGroup)tscbDeploymentGroup.SelectedItem;

                return deploymentGroup == null || deploymentGroup.BackendApplication == null ? null : deploymentGroup.BackendApplication;
            }
        }

        public Cdc.MetaManager.DataAccess.Domain.Application FrontendApplication
        {
            get
            {
                DeploymentGroup deploymentGroup = (DeploymentGroup)tscbDeploymentGroup.SelectedItem;

                return deploymentGroup == null || deploymentGroup.FrontendApplication == null ? null : deploymentGroup.FrontendApplication;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            VersiontoolStripStatusLabel.Text = "Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            
            try
            {
                System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();

                string rootPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();

                if (string.IsNullOrEmpty(rootPath))
                {
                    MessageBox.Show("The repository path is missing in your configuration. Your changes in the metadata will not be version controlled by the integrated configuration management tool", "Warning", MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The repository path is missing in your configuration. Your changes in the metadata will not be version controlled by the integrated configuration management tool", "Warning", MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
            }
        }

        #region DeploymentGroup Functions
        private void PopulateDeploymentGroupCombobox()
        {
            deploymentGroups.Clear();

            // Add all found deploymentgroups from database
            deploymentGroups.AddRange(appService.GetAllDeploymentGroups());

            tscbDeploymentGroup.Items.Clear();
            tscbDeploymentGroup.ComboBox.DisplayMember = "Name";
            //tscbDeploymentGroup.ComboBox.ValueMember = "Application";

            foreach (DeploymentGroup deploymentGroup in deploymentGroups)
            {
                tscbDeploymentGroup.Items.Add(deploymentGroup);
            }

            if (tscbDeploymentGroup.Items.Count > 0)
            {
                string lastSelectedDeploymentGroup = Config.Global.LastSelectedDeploymentGroup;

                if (string.IsNullOrEmpty(lastSelectedDeploymentGroup))
                {
                    tscbDeploymentGroup.SelectedIndex = 0;

                    SaveSelectedDeploymentGroup();
                }
                else
                {
                    IEnumerable<DeploymentGroup> dpgs = (from DeploymentGroup deploymentGroup in tscbDeploymentGroup.Items
                                                         where deploymentGroup.Id.ToString() == lastSelectedDeploymentGroup
                                                         select deploymentGroup);
                    DeploymentGroup x = null;

                    if (dpgs.Count() > 0)
                    {
                        x = dpgs.First();
                    }

                    if (x != null)
                    {
                        tscbDeploymentGroup.SelectedItem = x;
                    }
                    else
                    {
                        tscbDeploymentGroup.SelectedItem = tscbDeploymentGroup.Items[0];
                    }
                }
            }
            else
            {
                if (MessageBox.Show("You seem to have opened MetaManager with an empty database. Do you want to import metadata from a repository to fill the database?", "MetaManager - Empty DataBase", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    using (GetAllMetadataFromCM form = new GetAllMetadataFromCM())
                    {
                        form.ShowDialog();
                    }

                    PopulateDeploymentGroupCombobox();
                    return;

                    
                }
            }

            ShowSelectedDeploymentGroup();
        }


        private void EnableDisableAllToolStripButtons(ToolStrip toolStrip, bool enable)
        {
            foreach (ToolStripItem item in toolStrip.Items)
            {
                if (item is ToolStripButton)
                {
                    (item as ToolStripButton).Enabled = enable;
                }
            }
        }

        private void SaveSelectedDeploymentGroup()
        {
            if (tscbDeploymentGroup.SelectedItem != null)
            {
                Config.Global.LastSelectedDeploymentGroup = ((DeploymentGroup)tscbDeploymentGroup.SelectedItem).Id.ToString();
                Config.Save();
            }
        }

        private void tscbDeploymentGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            ShowSelectedDeploymentGroup();
            SaveSelectedDeploymentGroup();

            stopLoading = true;

            while (this.MdiChildren.Length > 0)
            {
                this.MdiChildren[0].Close();
            }

            //foreach (System.Threading.Thread t in workerThreads)
            //{
            //    t.Abort();
            //}

            while (workerThreads.Count > 0)
            {
                //for (int i = 0; i < workerThreads.Count; i++)
                //{
                //    if (!workerThreads[i].IsAlive)
                //    {
                //        workerThreads.RemoveAt(i);
                //    }
                //}
                //System.Threading.Thread.Sleep(100);
                System.Windows.Forms.Application.DoEvents();
            }

            if (loadThread != null)
            {

                //loadThread.Abort();
                while (loadThread.IsAlive)
                {
                    //System.Threading.Thread.Sleep(25);
                    System.Windows.Forms.Application.DoEvents();
                }

            }

            stopLoading = false;

            BuildApplicationTree(((Domain.DeploymentGroup)tscbDeploymentGroup.SelectedItem));

            loadThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadLazyTreeNodesInBackgroud));
            loadThread.IsBackground = true;
            loadThread.Start(null);

            Cursor.Current = Cursors.Default;

        }

        private void ShowSelectedDeploymentGroup()
        {
            if (FrontendApplication != null && BackendApplication != null)
            {
                // Set the prefixes for the config file so we read the correct keys
                Config.SetPrefixes(BackendApplication.GetPrefix(),
                                   FrontendApplication.GetPrefix());
            }
        }

        #endregion

        #region Old Menu button clicks


        private void tsbAnalyze_Click(object sender, EventArgs e)
        {
            using (Analyze form = new Analyze())
            {
                form.BackendApplication = BackendApplication;
                form.FrontendApplication = FrontendApplication;
                form.ShowDialog();
            }
        }

        private void tsbIssueList_Click(object sender, EventArgs e)
        {
            //using (HandleIssues form = new HandleIssues())
            //{
            //    form.BackendApplication = BackendApplication;
            //    form.FrontendApplication = FrontendApplication;

            //    form.ShowDialog();
            //}
        }

        #endregion

        #region Tree View Functions

        private void BuildApplicationTree(Domain.DeploymentGroup deploymentGroup)
        {
            if (searchTable == null)
            {
                searchTable = new DataTable("AllNodes");

                searchTable.ExtendedProperties.Add("SearchCriteria", "");
                searchTable.ExtendedProperties.Add("ResultIndex", 0);
                searchTable.ExtendedProperties.Add("LastSearchAdvanced", false);

                searchTable.Columns.Add("Id", typeof(string));
                searchTable.Columns.Add("Name", typeof(string));
                searchTable.Columns.Add("Type", typeof(string));
                searchTable.Columns.Add("FreeText", typeof(string));
                searchTable.Columns.Add("TreeNode", typeof(TreeNode));
                searchTable.Columns.Add("ParentName", typeof(string));

                AFindTypecomboBox.Items.Clear();
                AFindTypecomboBox.Items.Add("");
            }

            searchTable.Clear();
            searchResult = null;

            applicationTreeView.Nodes.Clear();

            TreeNode applicationTreeNode = applicationTreeView.Nodes.Add(deploymentGroup.Name);
            applicationTreeNode.Tag = deploymentGroup;
            UpdateApplicationTreeViewNodeIcon(applicationTreeNode);

            AddToolTipToNode(applicationTreeNode);
            AddToSerachTable(applicationTreeNode);

            BuildChildNodes(applicationTreeNode, typeof(Domain.Application), false, false);

            //applicationTreeNode.Tag = new KeyValuePair<string, DataAccess.IVersionControlled>("TOPLEVEL", deploymentGroup);

        }

        private void BuildChildNodes(TreeNode currentTreeNode, Type childDomainType, bool update, bool AddOverviewNode, bool recursionNotLacy = false)
        {

            TreeNode nodeToAddChildTo;
            TreeNode selectedNode = null;
            bool initial = false;
            bool pausedLoader = false;
            bool holdingSearchLoadingLock = false;
            List<string> parentNames = new List<string>();

            if (System.Threading.Thread.CurrentThread == loadThread && pauseLoadThread)
            {
                while (pauseLoadThread)
                {
                    System.Threading.Thread.Sleep(500);
                }
            }

            lock (searchTable)
            {
                if (!loadingSearchTable)
                {
                    loadingSearchTable = true;
                    holdingSearchLoadingLock = true;
                }
            }

            if (AddOverviewNode)
            {
                if (!update)
                {
                    string groupNodeName;
                    groupNodeName = childDomainType.Name.EndsWith("y") ? childDomainType.Name.Remove(childDomainType.Name.Length - 1) + "ies" : childDomainType.Name + "s";
                    int index = 0;

                    if (applicationTreeView.InvokeRequired)
                    {
                        applicationTreeView.Invoke(new ThreadJumpDelegate(delegate { index = currentTreeNode.Nodes.Add(new TreeNode(groupNodeName)); }));
                    }
                    else
                    {
                        index = currentTreeNode.Nodes.Add(new TreeNode(groupNodeName));
                    }

                    string name = string.Empty;

                    if (!recursionNotLacy)
                    {
                        name = "OVERVIEW#LAZY";
                    }
                    else
                    {
                        name = "OVERVIEW";
                        initial = true;
                    }

                    currentTreeNode.Nodes[index].Tag = new KeyValuePair<string, Type>(name, childDomainType);
                    currentTreeNode.Nodes[index].Name = "TreeNode:" + Guid.NewGuid().ToString();
                    UpdateApplicationTreeViewNodeIcon(currentTreeNode.Nodes[index]);
                    nodeToAddChildTo = currentTreeNode.Nodes[index];

                    if (!recursionNotLacy)
                    {
                        if (applicationTreeView.InvokeRequired)
                        {
                            applicationTreeView.Invoke(new ThreadJumpDelegate(delegate { nodeToAddChildTo.Nodes.Add("Not loaded. Please wait..."); }));
                        }
                        else
                        {
                            nodeToAddChildTo.Nodes.Add("Not loaded. Please wait...");
                        }

                        return;
                    }

                }
                else
                {

                    if (currentTreeNode.Tag is KeyValuePair<string, Type> && ((KeyValuePair<string, Type>)currentTreeNode.Tag).Key.StartsWith("OVERVIEW"))
                    {
                        GetAllParentNodeNames(currentTreeNode, parentNames);
                        if (applicationTreeView.InvokeRequired)
                        {
                            applicationTreeView.Invoke(new ThreadJumpDelegate(delegate
                                {
                                    currentTreeNode.Nodes.Clear();
                                }));
                        }
                        else
                        {
                            currentTreeNode.Nodes.Clear();
                        }

                        if (((KeyValuePair<string, Type>)currentTreeNode.Tag).Key.EndsWith("LAZY"))
                        {
                            initial = true;
                        }

                        currentTreeNode.Tag = new KeyValuePair<string, Type>("OVERVIEW", ((KeyValuePair<string, Type>)currentTreeNode.Tag).Value);
                        nodeToAddChildTo = currentTreeNode;
                        currentTreeNode = currentTreeNode.Parent;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                nodeToAddChildTo = currentTreeNode;
                initial = true;
                if (update)
                {
                    return;
                }
            }

            lock (nodeToAddChildTo)
            {

                if (loadThread != null && System.Threading.Thread.CurrentThread != loadThread)
                {
                    lock (loadThread)
                    {
                        if (!pauseLoadThread)
                        {
                            pauseLoadThread = true;
                            pausedLoader = true;
                        }
                    }
                }

                DataAccess.IDomainObject currentDomainObject = ((DataAccess.IDomainObject)currentTreeNode.Tag);


                //Read all objects     
                IEnumerable<DataAccess.IVersionControlled> sortedDomainNodes = modelService.GetVersionControlledDomainObjectsForParent(childDomainType, currentDomainObject.Id).OrderBy(o => o.GetType().GetProperty("Name") == null ? o.Id : o.GetType().GetProperty("Name").GetValue(o, null));

                //Add All nodes
                List<TreeNode> nodes = new List<TreeNode>();

                foreach (DataAccess.IVersionControlled domainObject in sortedDomainNodes)
                {
                    List<Type> usedPropertyTypes = new List<Type>();
                    bool isfrontend = false;
                    TreeNode childNode = CreateNode(domainObject, out isfrontend);

                    nodes.Add(childNode);

                    if (applicationTreeView.Tag is KeyValuePair<Guid, Type>)
                    {
                        Guid keyId = ((KeyValuePair<Guid, Type>)applicationTreeView.Tag).Key;
                        Type keyType = ((KeyValuePair<Guid, Type>)applicationTreeView.Tag).Value;

                        if (domainObject.Id == keyId && modelService.GetDomainObjectType(domainObject) == keyType)
                        {
                            selectedNode = childNode;
                            applicationTreeView.Tag = null;
                        }
                    }



                    foreach (System.Reflection.PropertyInfo property in childDomainType.GetProperties())
                    {
                        bool onlyfrontend = property.GetCustomAttributes(typeof(Domain.ApplicationOnlyFrontendAttribute), true).Count() > 0;
                        bool onlybackend = property.GetCustomAttributes(typeof(Domain.ApplicationOnlyBackendAttribute), true).Count() > 0;
                        bool notonly = !onlybackend & !onlyfrontend;

                        bool addchild = (onlyfrontend & isfrontend) | (onlybackend & !isfrontend) | (notonly);

                        if (addchild)
                        {
                            if (property.PropertyType.Name == typeof(IList<object>).Name)
                            {
                                if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(property.PropertyType.GetGenericArguments()[0]))
                                {
                                    if (!usedPropertyTypes.Contains(property.PropertyType.GetGenericArguments()[0]))
                                    {
                                        if (property.PropertyType.GetGenericArguments()[0] != modelService.GetDomainObjectType(currentDomainObject))
                                        {
                                            BuildChildNodes(childNode, property.PropertyType.GetGenericArguments()[0], false, true, recursionNotLacy);
                                        }
                                        usedPropertyTypes.Add(property.PropertyType.GetGenericArguments()[0]);
                                    }
                                }
                            }
                            else if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(property.PropertyType))
                            {

                                if (!usedPropertyTypes.Contains(property.PropertyType))
                                {
                                    if (property.PropertyType != modelService.GetDomainObjectType(currentDomainObject))
                                    {
                                        BuildChildNodes(childNode, property.PropertyType, false, false, recursionNotLacy);
                                    }
                                    usedPropertyTypes.Add(property.PropertyType);
                                }

                            }
                        }
                    }
                }

                if (applicationTreeView.InvokeRequired)
                {
                    applicationTreeView.Invoke(new ThreadJumpDelegate(delegate
                        {
                            nodeToAddChildTo.Nodes.AddRange(nodes.ToArray());
                            if (selectedNode != null)
                            {
                                applicationTreeView.SelectedNode = selectedNode;
                            }
                        }));
                }
                else
                {
                    nodeToAddChildTo.Nodes.AddRange(nodes.ToArray());
                    if (selectedNode != null)
                    {
                        applicationTreeView.SelectedNode = selectedNode;
                    }
                }

                bool searchLock = false;

                lock (searchTable)
                {
                    if (SearchTableLocked)
                    {
                        searchLock = true;
                    }
                    else
                    {
                        SearchTableLocked = true;
                    }
                }

                while (searchLock)
                {
                    lock (searchTable)
                    {
                        if (!SearchTableLocked)
                        {
                            SearchTableLocked = true;
                            searchLock = false;
                        }
                    }
                    System.Threading.Thread.Sleep(20);
                }

                if (!initial)
                {
                    foreach (string parentName in parentNames)
                    {
                        DataRow[] rows = searchTable.Select("ParentName = '" + parentName + "'");

                        foreach (DataRow row in rows)
                        {
                            searchTable.Rows.Remove(row);
                        }
                    }
                }

                foreach (TreeNode node in nodes)
                {
                    AddToSerachTable(node);
                }

                lock (searchTable)
                {
                    SearchTableLocked = false;
                }

                if (holdingSearchLoadingLock)
                {
                    loadingSearchTable = false;
                }

                if (pausedLoader)
                {
                    pauseLoadThread = false;
                }
            }
        }

        private void GetAllParentNodeNames(TreeNode node, List<string> parentNames)
        {
            if (node.Tag is KeyValuePair<string, Type> && ((KeyValuePair<string, Type>)node.Tag).Key.StartsWith("OVERVIEW"))
            {
                parentNames.Add(node.Name);
            }

            foreach (TreeNode cnode in node.Nodes)
            {
                GetAllParentNodeNames(cnode, parentNames);
            }
        }


        private TreeNode CreateNode(DataAccess.IDomainObject domainObject, out bool isfrontend)
        {
            TreeNode childNode = new TreeNode();

            string text = domainObject.GetType().GetProperty("Name") == null ? modelService.GetDomainObjectType(domainObject).Name : domainObject.GetType().GetProperty("Name").GetValue(domainObject, null).ToString();

            isfrontend = false;


            if (domainObject is Domain.Application)
            {
                if (((Domain.Application)domainObject).IsFrontend.Value)
                {
                    text += " (Frontend)";
                    isfrontend = true;
                }
                else
                {
                    text += " (Backend)";
                    isfrontend = false;
                }
            }


            childNode.Text = text;
            childNode.Tag = domainObject;
            childNode.Name = "TreeNode:" + Guid.NewGuid().ToString();

            UpdateApplicationTreeViewNodeIcon(childNode);

            AddToolTipToNode(childNode);

            return childNode;
        }

        private void AddToSerachTable(TreeNode nodeWithDomainObject)
        {
            DataAccess.IDomainObject domainObject = (DataAccess.IDomainObject)nodeWithDomainObject.Tag;

            Dictionary<string, System.Reflection.PropertyInfo> propertyDic = domainObject.GetType().GetProperties().ToDictionary(o => o.Name.ToUpper(), o => o);
            List<System.Reflection.PropertyInfo> freeTextProperties = domainObject.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(DataAccess.SearchAttribute), true).Count() > 0 && ((DataAccess.SearchAttribute)p.GetCustomAttributes(typeof(DataAccess.SearchAttribute), true)[0]).SearchType == DataAccess.SearchTypes.FreeText).ToList();

            DataRow newRow = searchTable.NewRow();
            newRow["Id"] = domainObject.Id.ToString();
            newRow["Type"] = modelService.GetDomainObjectType(domainObject).Name;
            newRow["TreeNode"] = nodeWithDomainObject;
            newRow["ParentName"] = nodeWithDomainObject.Parent == null ? "" : nodeWithDomainObject.Parent.Name;

            if (propertyDic.ContainsKey("NAME") && propertyDic["NAME"].GetValue(domainObject, null) != null)
            {
                newRow["Name"] = propertyDic["NAME"].GetValue(domainObject, null).ToString();
            }
            else
            {
                newRow["Name"] = "";
            }

            string freeTestStr = string.Empty;
            foreach (System.Reflection.PropertyInfo pi in freeTextProperties)
            {
                if (typeof(DataAccess.IDomainObject).IsAssignableFrom(pi.PropertyType))
                {
                    DataAccess.IDomainObject chObj = (DataAccess.IDomainObject)pi.GetValue(domainObject, null);
                    if (chObj != null)
                    {
                        List<System.Reflection.PropertyInfo> childPropList = modelService.GetDomainObjectType(chObj).GetProperties().Where(p => p.GetCustomAttributes(typeof(DataAccess.SearchAttribute), true).Count() > 0 && ((DataAccess.SearchAttribute)p.GetCustomAttributes(typeof(DataAccess.SearchAttribute), true)[0]).SearchType == DataAccess.SearchTypes.FreeText).ToList();
                        if (childPropList.Count > 0)
                        {
                            if (childPropList[0].PropertyType == typeof(string))
                            {
                                chObj = modelService.GetInitializedDomainObject(chObj.Id, modelService.GetDomainObjectType(chObj));
                                if (childPropList[0].GetValue(chObj, null) != null)
                                {
                                    freeTestStr += childPropList[0].GetValue(chObj, null).ToString() + " ¤ ";
                                }
                            }
                        }
                    }
                }
                else if (pi.PropertyType == typeof(string))
                {
                    if (pi.GetValue(domainObject, null) != null)
                    {
                        freeTestStr += pi.GetValue(domainObject, null).ToString() + " ¤ ";
                    }
                }
            }

            newRow["FreeText"] = freeTestStr;

            searchTable.Rows.Add(newRow);

            if (!AFindTypecomboBox.Items.Contains(modelService.GetDomainObjectType(domainObject).Name))
            {
                if (AFindTypecomboBox.InvokeRequired)
                {
                    AFindTypecomboBox.Invoke(new ThreadJumpDelegate(delegate
                        {
                            AFindTypecomboBox.Items.Add(modelService.GetDomainObjectType(domainObject).Name);
                        }));
                }
                else
                {
                    AFindTypecomboBox.Items.Add(modelService.GetDomainObjectType(domainObject).Name);
                }
            }
        }

        private void RemoveFromSerachTable(DataRow deletedRow)
        {

            searchTable.Rows.Remove(deletedRow);

        }
        private void AddToolTipToNode(TreeNode nodeWithDomainObject)
        {
            string tooltiptext = string.Empty;

            DataAccess.IDomainObject domainObject = (DataAccess.IDomainObject)nodeWithDomainObject.Tag;
            if (!NHibernateUtil.IsInitialized(domainObject))
            {
                domainObject = modelService.GetInitializedDomainObject(domainObject.Id, modelService.GetDomainObjectType(domainObject));
            }
            Dictionary<string, System.Reflection.PropertyInfo> propertyDic = domainObject.GetType().GetProperties().ToDictionary(o => o.Name.ToUpper(), o => o);


            tooltiptext = "Id: " + domainObject.Id.ToString();
            tooltiptext += "\r\nType: " + modelService.GetDomainObjectType(domainObject).Name;


            if (propertyDic.ContainsKey("NAME"))
            {
                tooltiptext += "\r\nName: " + propertyDic["NAME"].GetValue(domainObject, null).ToString();
            }

            if (propertyDic.ContainsKey("VERSION"))
            {
                tooltiptext += "\r\nVersion: " + propertyDic["VERSION"].GetValue(domainObject, null).ToString();
            }

            if (propertyDic.ContainsKey("STATUS"))
            {
                tooltiptext += "\r\nStatus: " + propertyDic["STATUS"].GetValue(domainObject, null).ToString();
            }

            if (propertyDic.ContainsKey("TYPE"))
            {
                tooltiptext += "\r\nType: " + propertyDic["TYPE"].GetValue(domainObject, null).ToString();
            }

            if (propertyDic.ContainsKey("LOCKEDBY"))
            {
                tooltiptext += "\r\nLocked By: " + (propertyDic["LOCKEDBY"].GetValue(domainObject, null) == null ? "" : propertyDic["LOCKEDBY"].GetValue(domainObject, null).ToString());
            }

            nodeWithDomainObject.ToolTipText = tooltiptext;
        }

        private void UpdateApplicationTreeViewNodeIcon(TreeNode node)
        {
            int imageIndex = 0;
            int selectedImage = 0;

            int normIndex = 0;
            int privateIndex = 0;
            int lockedIndex = 0;
            int newIndex = 0;
            int deleteIndex = 0;

            if (node.Tag is KeyValuePair<string, Type> && ((KeyValuePair<string, Type>)node.Tag).Key.StartsWith("OVERVIEW"))
            {
                imageIndex = 0;
                selectedImage = 0;
            }
            else
            {
                if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(node.Tag.GetType()))
                {
                    DataAccess.IVersionControlled domainObject = ((DataAccess.IVersionControlled)node.Tag);
                    if (!NHibernateUtil.IsInitialized(domainObject))
                    {
                        domainObject = (DataAccess.IVersionControlled)modelService.GetInitializedDomainObject(domainObject.Id, modelService.GetDomainObjectType(domainObject));
                    }

                    if (domainObject is Domain.Application)
                    {
                        normIndex = 1;
                        privateIndex = 2;
                        lockedIndex = 3;
                        newIndex = 40;
                        deleteIndex = 53;
                    }
                    else if (domainObject is Module)
                    {
                        normIndex = 10;
                        privateIndex = 11;
                        lockedIndex = 12;
                        newIndex = 41;
                        deleteIndex = 54;
                    }
                    else if (domainObject is Dialog)
                    {
                        normIndex = 4;
                        privateIndex = 5;
                        lockedIndex = 6;
                        newIndex = 42;
                        deleteIndex = 55;
                    }
                    else if (domainObject is Domain.Workflow)
                    {
                        normIndex = 19;
                        privateIndex = 20;
                        lockedIndex = 21;
                        newIndex = 43;
                        deleteIndex = 56;
                    }
                    else if (domainObject is UXAction)
                    {
                        normIndex = 13;
                        privateIndex = 14;
                        lockedIndex = 15;
                        newIndex = 44;
                        deleteIndex = 57;
                    }
                    else if (domainObject is Domain.View)
                    {
                        normIndex = 16;
                        privateIndex = 17;
                        lockedIndex = 18;
                        newIndex = 45;
                        deleteIndex = 58;
                    }
                    else if (domainObject is Domain.Menu)
                    {
                        normIndex = 7;
                        privateIndex = 8;
                        lockedIndex = 9;
                        newIndex = 46;
                        deleteIndex = 59;
                    }
                    else if (domainObject is Service)
                    {
                        normIndex = 31;
                        privateIndex = 32;
                        lockedIndex = 33;
                        newIndex = 47;
                        deleteIndex = 60;
                    }
                    else if (domainObject is ServiceMethod)
                    {
                        normIndex = 34;
                        privateIndex = 35;
                        lockedIndex = 36;
                        newIndex = 48;
                        deleteIndex = 61;
                    }
                    else if (domainObject is BusinessEntity)
                    {
                        normIndex = 25;
                        privateIndex = 26;
                        lockedIndex = 27;
                        newIndex = 49;
                        deleteIndex = 62;
                    }
                    else if (domainObject is Domain.Action)
                    {
                        normIndex = 22;
                        privateIndex = 23;
                        lockedIndex = 24;
                        newIndex = 50;
                        deleteIndex = 63;
                    }
                    else if (domainObject is HintCollection)
                    {
                        normIndex = 28;
                        privateIndex = 29;
                        lockedIndex = 30;
                        newIndex = 51;
                        deleteIndex = 64;
                    }
                    else if (domainObject is Report)
                    {
                        normIndex = 37;
                        privateIndex = 38;
                        lockedIndex = 39;
                        newIndex = 52;
                        deleteIndex = 65;
                    }

                    if (domainObject.State == DataAccess.VersionControlledObjectStat.New)
                    {
                        imageIndex = newIndex;
                        selectedImage = newIndex;
                    }
                    else if (domainObject.State == DataAccess.VersionControlledObjectStat.Deleted)
                    {
                        imageIndex = deleteIndex;
                        selectedImage = deleteIndex;
                    }
                    else
                    {
                        if (domainObject.IsLocked)
                        {
                            if (domainObject.LockedBy == Environment.UserName)
                            {
                                imageIndex = privateIndex;
                                selectedImage = privateIndex;
                            }
                            else
                            {
                                imageIndex = lockedIndex;
                                selectedImage = lockedIndex;
                            }
                        }
                        else
                        {
                            imageIndex = normIndex;
                            selectedImage = normIndex;
                        }
                    }
                }
            }

            if (applicationTreeView.InvokeRequired)
            {
                applicationTreeView.Invoke(new ThreadJumpDelegate(delegate
                {
                    node.ImageIndex = imageIndex;
                    node.SelectedImageIndex = selectedImage;
                }));
            }
            else
            {
                node.ImageIndex = imageIndex;
                node.SelectedImageIndex = selectedImage;
            }
        }

        private void applicationTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            applicationTreeView.SelectedNode = e.Node;
        }

        private void applicationTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                applicationTreeView.ContextMenuStrip = null;
            }
            else if (e.Node.Tag is DeploymentGroup)
            {
                cmsLevelZero.Tag = applicationTreeView;
                applicationTreeView.ContextMenuStrip = cmsLevelZero;
            }
            else if (e.Node.Tag is KeyValuePair<string, Type> && ((KeyValuePair<string, Type>)e.Node.Tag).Key.StartsWith("OVERVIEW"))
            {
                cmsLevelOne.Tag = applicationTreeView;
                applicationTreeView.ContextMenuStrip = cmsLevelOne;
            }
            else if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(e.Node.Tag.GetType()))
            {
                cmsLevelTwo.Tag = applicationTreeView;
                applicationTreeView.ContextMenuStrip = cmsLevelTwo;
            }
            else
            {
                applicationTreeView.ContextMenuStrip = null;
            }
        }

        private void applicationTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (applicationTreeView.ContextMenuStrip != null)
            {
                applicationTreeView.ContextMenuStrip.Items[0].PerformClick();
            }
        }

        private void UpdateSelectedNode()
        {
            TreeNode selectedNode = applicationTreeView.SelectedNode;

            UpdateNode(selectedNode);
        }

        private void UpdateNode(TreeNode node)
        {
            if (node.Tag is KeyValuePair<string, Type>)
            {
                Cursor.Current = Cursors.WaitCursor;
                BuildChildNodes(node, ((KeyValuePair<string, Type>)node.Tag).Value, true, true, true);
                Cursor.Current = Cursors.Default;
            }
            else if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(node.Tag.GetType()))
            {
                node.Tag = modelService.GetDomainObject(((DataAccess.IDomainObject)node.Tag).Id, modelService.GetDomainObjectType(((DataAccess.IDomainObject)node.Tag)));
                UpdateApplicationTreeViewNodeIcon(node);
                AddToolTipToNode(node);
            }
        }

        private void applicationTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

            System.Threading.Thread workThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadLazySelectedNode));
            workThread.Start(e.Node);
        }

        private void LoadLazySelectedNode(object obj)
        {
            workerThreads.Add(System.Threading.Thread.CurrentThread);
            TreeNode node = (TreeNode)obj;

            if (node.Tag is KeyValuePair<string, Type>)
            {
                if (((KeyValuePair<string, Type>)node.Tag).Key.EndsWith("LAZY"))
                {
                    this.Invoke(new ThreadJumpDelegate(delegate
                    {
                        UseWaitCursor = true;
                    }));

                    BuildChildNodes(node, ((KeyValuePair<string, Type>)node.Tag).Value, true, true, true);

                    this.Invoke(new ThreadJumpDelegate(delegate
                    {
                        UseWaitCursor = false;
                    }));
                }
            }

            workerThreads.Remove(System.Threading.Thread.CurrentThread);
        }

        #endregion

        #region Tree Context Menu Functions

        private void cmsLevelOne_Opening(object sender, CancelEventArgs e)
        {
            pasteToolStripMenuItem.Enabled = false;
            //bool IOk = false;

            tsmiImportDelphiDialog.Enabled = false;

            if (cmsLevelOne.Tag is TreeView)
            {
                TreeNode selectedNode = ((TreeView)cmsLevelOne.Tag).SelectedNode;

                if (selectedNode.Tag is KeyValuePair<string, Type> && ((KeyValuePair<string, Type>)selectedNode.Tag).Key.StartsWith("OVERVIEW"))
                {
                    //if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(selectedNode.Parent.Tag.GetType()))
                    //{
                    //    DataAccess.IVersionControlled domainObject = ((DataAccess.IVersionControlled)selectedNode.Parent.Tag);
                    //    domainObject = (DataAccess.IVersionControlled)modelService.GetInitializedDomainObject(domainObject.Id, modelService.GetDomainObjectType(domainObject));
                    //    if (domainObject.IsLocked && domainObject.LockedBy == Environment.UserName)
                    //    {
                    //        IOk = true;
                    //    }
                    //}

                    //tsmiAddNew.Enabled = IOk;


                    Type childType = ((KeyValuePair<string, Type>)selectedNode.Tag).Value;

                    if (objectToMove != null && childType == modelService.GetDomainObjectType(objectToMove))
                    {
                        pasteToolStripMenuItem.Enabled = true;
                    }

                    if (childType == typeof(Domain.Dialog))
                    {
                        tsmiImportDelphiDialog.Enabled = true;
                    }
                }
            }
        }

        private void tsmiHandle_Click(object sender, EventArgs e)
        {
            return;
            //Cursor.Current = Cursors.WaitCursor;
            //if (cmsLevelOne.Tag is TreeView)
            //{
            //    TreeNode selectedNode = ((TreeView)cmsLevelOne.Tag).SelectedNode;

            //    if (selectedNode.Tag is KeyValuePair<string, Type> && ((KeyValuePair<string, Type>)selectedNode.Tag).Key.StartsWith("OVERVIEW"))
            //    {
            //        Type childType = ((KeyValuePair<string, Type>)selectedNode.Tag).Value;

            //        if (childType == typeof(Domain.Dialog))
            //        {
            //            SelectDialog form = new SelectDialog();
            //            Config.Frontend.HandleDialogsLastSearchedModule = selectedNode.Parent.Text;
            //            Config.Save();
            //            form.MdiParent = this;
            //            form.FrontendApplication = FrontendApplication;
            //            form.BackendApplication = BackendApplication;
            //            form.WindowState = FormWindowState.Maximized;
            //            form.Show();
            //        }
            //        else if (childType == typeof(Domain.Workflow))
            //        {
            //            FindWorkflowForm form = new FindWorkflowForm();
            //            Config.Frontend.HandleDialogsLastSearchedModule = selectedNode.Parent.Text;
            //            Config.Save();
            //            form.MdiParent = this;
            //            form.FrontendApplication = FrontendApplication;
            //            form.BackendApplication = BackendApplication;
            //            form.WindowState = FormWindowState.Maximized;
            //            form.Show();
            //        }
            //        else if (childType == typeof(Domain.CustomDialog))
            //        {
            //            SelectCustomDialog form = new SelectCustomDialog();
            //            form.FrontendApplication = FrontendApplication;
            //            form.BackendApplication = BackendApplication;
            //            form.MdiParent = this;
            //            form.WindowState = FormWindowState.Maximized;
            //            form.Show();
            //        }
            //        else if (childType == typeof(Domain.UXAction))
            //        {
            //            HandleUXAction form = new HandleUXAction();
            //            form.FrontendApplication = FrontendApplication;
            //            form.BackendApplication = BackendApplication;
            //            form.MdiParent = this;
            //            form.WindowState = FormWindowState.Maximized;
            //            form.Show();
            //        }
            //        else if (childType == typeof(Domain.View))
            //        {
            //            FindViewForm form = new FindViewForm();
            //            form.FrontendApplication = FrontendApplication;
            //            form.BackendApplication = BackendApplication;
            //            form.MdiParent = this;
            //            form.WindowState = FormWindowState.Maximized;
            //            form.Show();
            //        }
            //        else if (childType == typeof(Domain.BusinessEntity))
            //        {
            //            using (DataModelImportForm form = new DataModelImportForm())
            //            {
            //                form.Owner = this;
            //                form.BackendApplication = BackendApplication;
            //                form.ShowDialog();
            //            }
            //        }

            //    }
            //}

            //Cursor.Current = Cursors.Default;
        }

        private void tsmiAddNew_Click(object sender, EventArgs e)
        {
            bool openObjectAfterSave = false;

            Cursor.Current = Cursors.WaitCursor;

            if (cmsLevelOne.Tag is TreeView)
            {
                TreeNode selectedNode = ((TreeView)cmsLevelOne.Tag).SelectedNode;

                if (selectedNode.Tag is KeyValuePair<string, Type> && ((KeyValuePair<string, Type>)selectedNode.Tag).Key.StartsWith("OVERVIEW"))
                {
                    Type childType = ((KeyValuePair<string, Type>)selectedNode.Tag).Value;

                    if (childType == typeof(Domain.Module))
                    {
                        using (CreateEditModule form = new CreateEditModule())
                        {
                            form.FrontendApplication = FrontendApplication;
                            form.BackendApplication = BackendApplication;
                            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SaveSelectedNodeKyes(((MdiChildForm)form).ContaindDomainObjectIdAndType);
                            }
                        }
                    }
                    else if (childType == typeof(Domain.Dialog))
                    {
                        using (CreateDialog form = new CreateDialog())
                        {
                            Module selectedModule = (Module)selectedNode.Parent.Tag;

                            if (!NHibernate.NHibernateUtil.IsInitialized(selectedModule))
                            {
                                selectedModule = modelService.GetInitializedDomainObject<Module>(selectedModule.Id);
                            }

                            form.Owner = this;
                            form.startupModule = selectedModule.Name;
                            form.FixedModule = true;
                            form.FrontendApplication = FrontendApplication;
                            form.BackendApplication = BackendApplication;
                            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SaveSelectedNodeKyes(((MdiChildForm)form).ContaindDomainObjectIdAndType);
                                openObjectAfterSave = true;
                            }
                        }
                    }
                    else if (childType == typeof(Domain.Workflow))
                    {

                        using (CreateWorkflow form = new CreateWorkflow())
                        {
                            Module selectedModule = (Module)selectedNode.Parent.Tag;

                            if (!NHibernate.NHibernateUtil.IsInitialized(selectedModule))
                            {
                                selectedModule = modelService.GetInitializedDomainObject<Module>(selectedModule.Id);
                            }


                            form.FrontendApplication = FrontendApplication;
                            form.startupModule = selectedModule.Name;
                            form.FixedModule = true;

                            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SaveSelectedNodeKyes(((MdiChildForm)form).ContaindDomainObjectIdAndType);
                            }
                        }


                    }
                    else if (childType == typeof(Domain.Service))
                    {
                        using (NewEditService form = new NewEditService())
                        {
                            form.BackendApplication = BackendApplication;
                            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SaveSelectedNodeKyes(((MdiChildForm)form).ContaindDomainObjectIdAndType);
                            }
                        }
                    }
                    else if (childType == typeof(Domain.ServiceMethod))
                    {
                        using (CreateEditServiceMethod form = new CreateEditServiceMethod())
                        {
                            form.Owner = this;
                            form.FrontendApplication = FrontendApplication;
                            form.BackendApplication = BackendApplication;
                            form.Service = (Service)selectedNode.Parent.Tag;
                            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SaveSelectedNodeKyes(((MdiChildForm)form).ContaindDomainObjectIdAndType);
                            }
                        }
                    }
                    else if (childType == typeof(Domain.Action))
                    {
                        using (CreateEditActionWizard form = new CreateEditActionWizard())
                        {
                            form.Owner = this;
                            form.FrontendApplication = FrontendApplication;
                            form.BackendApplication = BackendApplication;
                            form.BusinessEntity = (BusinessEntity)selectedNode.Parent.Tag;
                            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SaveSelectedNodeKyes(((MdiChildForm)form).ContaindDomainObjectIdAndType);
                            }
                        }
                    }



                    else if (childType == typeof(Domain.CustomDialog))
                    {
                        using (AddCustomDialog form = new AddCustomDialog())
                        {
                            form.FrontendApplication = FrontendApplication;
                            form.BackendApplication = BackendApplication;
                            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SaveSelectedNodeKyes(((MdiChildForm)form).ContaindDomainObjectIdAndType);
                            }
                        }
                    }
                    else if (childType == typeof(Domain.UXAction))
                    {
                        // Create the edit action dialog
                        using (EditUXAction form = new EditUXAction())
                        {
                            form.BackendApplication = BackendApplication;
                            form.FrontendApplication = FrontendApplication;
                            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SaveSelectedNodeKyes(((MdiChildForm)form).ContaindDomainObjectIdAndType);
                            }
                        }
                    }
                    else if (childType == typeof(Domain.View))
                    {
                        using (CreateView form = new CreateView())
                        {
                            form.FrontendApplication = FrontendApplication;
                            form.BackendApplication = BackendApplication;
                            form.Owner = this;
                            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SaveSelectedNodeKyes(((MdiChildForm)form).ContaindDomainObjectIdAndType);
                            }
                        }
                    }
                    else if (childType == typeof(Domain.BusinessEntity))
                    {
                        using (CreateEditBusinessEntity form = new CreateEditBusinessEntity())
                        {
                            form.FrontendApplication = FrontendApplication;
                            form.BackendApplication = BackendApplication;
                            form.Owner = this;
                            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SaveSelectedNodeKyes(((MdiChildForm)form).ContaindDomainObjectIdAndType);
                            }
                        }
                    }
                    else if (childType == typeof(Domain.Report))
                    {
                        CreateEditReport form = new CreateEditReport();

                        form.FrontendApplication = FrontendApplication;
                        form.BackendApplication = BackendApplication;
                        form.Owner = this;
                        ShowChildWindow(form);
                        /*
                        if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            SaveSelectedNodeKyes(((MdiChildForm)form).ContaindDomainObjectIdAndType);
                        }
                         */
                    }

                    if (applicationTreeView.SelectedNode != null && openObjectAfterSave)
                    {
                        SelectNodeFRomSavedKeys();
                        if (applicationTreeView.Tag == null)
                        {
                            applicationTreeView.ContextMenuStrip.Items[0].PerformClick();
                        }
                    }
                }

            }

            Cursor.Current = Cursors.Default;
        }

        private void SelectNodeFRomSavedKeys()
        {
            if (applicationTreeView.Tag is KeyValuePair<Guid, Type>)
            {
                Guid Id = ((KeyValuePair<Guid, Type>)applicationTreeView.Tag).Key;
                Type objectType = ((KeyValuePair<Guid, Type>)applicationTreeView.Tag).Value;
                string searchCriteria = string.Empty;
                searchCriteria = "Id = '" + Id.ToString() + "' AND Type = '" + objectType.Name + "'";
                DataRow[] result = null;
                result = SearchLoadedNodes(searchCriteria, true, true);
                if (result.Length > 0)
                {
                    applicationTreeView.SelectedNode = (TreeNode)result[0]["TreeNode"];
                }

                applicationTreeView.Tag = null;
            }
        }

        private void SaveSelectedNodeKyes(KeyValuePair<Guid, Type> keys)
        {
            if (keys.Key != Guid.Empty)
            {
                applicationTreeView.Tag = keys;
            }
        }


        private void cmsLevelTwo_Opening(object sender, CancelEventArgs e)
        {
            DataAccess.IVersionControlled domainObject = null;
            cutToolStripMenuItem.Enabled = false;




            if (cmsLevelTwo.Tag is TreeView)
            {
                TreeView theTree = ((TreeView)cmsLevelTwo.Tag);
                if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(theTree.SelectedNode.Tag.GetType()))
                {
                    domainObject = ((DataAccess.IVersionControlled)theTree.SelectedNode.Tag);
                    if (!NHibernateUtil.IsInitialized(domainObject))
                    {
                        domainObject = (DataAccess.IVersionControlled)modelService.GetInitializedDomainObject(domainObject.Id, modelService.GetDomainObjectType(domainObject));
                    }
                }
            }

            if (domainObject != null)
            {
                if (domainObject.IsLocked)
                {
                    CheckOutToolStripMenuItem.Enabled = false;
                    compareWithLatestVersionToolStripMenuItem.Enabled = true;
                }
                else
                {
                    CheckOutToolStripMenuItem.Enabled = true;
                    compareWithLatestVersionToolStripMenuItem.Enabled = false;
                }

                if (domainObject.IsLocked && domainObject.LockedBy == Environment.UserName)
                {
                    checkInToolStripMenuItem.Enabled = true;
                    tsmiDelete.Enabled = true;
                    undoCheckOutToolStripMenuItem.Enabled = true;

                    if (IsMovableObject(domainObject))
                    {
                        cutToolStripMenuItem.Enabled = true;
                    }
                }
                else
                {
                    checkInToolStripMenuItem.Enabled = false;
                    tsmiDelete.Enabled = false;
                    undoCheckOutToolStripMenuItem.Enabled = false;
                }

                if (domainObject is Domain.ServiceMethod)
                {
                    jumpToActionToolStripMenuItem.Visible = true;
                }
                else
                {
                    jumpToActionToolStripMenuItem.Visible = false;
                }

                generateReportsToolStripMenuItem.Visible = ((domainObject is Domain.Application) && !((Domain.Application)domainObject).IsFrontend.Value);
                importUpdateDataModelMenuItem.Visible = ((domainObject is Domain.Application) && !((Domain.Application)domainObject).IsFrontend.Value);

            }


        }

        private bool IsMovableObject(DataAccess.IVersionControlled domainObject)
        {
            Type type = domainObject.GetType();
            Type attType = typeof(ModelDesignerAttribute);

            ModelDesignerAttribute modelDesigner = Attribute.GetCustomAttribute(type, attType) as ModelDesignerAttribute;
            if (modelDesigner != null)
            {
                return modelDesigner.IsMovable;
            }
            return false;
        }

        private void miImportDelphiDialog_Click(object sender, EventArgs e)
        {
            return;
            //using (ImportDialogForm form = new ImportDialogForm())
            //{
            //    form.FrontendApplication = FrontendApplication;
            //    form.BackendApplication = BackendApplication;
            //    form.Owner = this;
            //    form.ShowDialog();
            //}
        }

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (cmsLevelTwo.Tag is TreeView)
            {
                TreeNode selectedNode = ((TreeView)cmsLevelTwo.Tag).SelectedNode;

                if (selectedNode.Tag is Domain.Application)
                {
                    EditApplicationInfo form = new EditApplicationInfo();
                    form.FrontendApplication = FrontendApplication;
                    form.BackendApplication = BackendApplication;
                    form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, typeof(Domain.Application));
                    ShowChildWindow(form);
                }
                else if (selectedNode.Tag is Domain.Module)
                {
                    CreateEditModule form = new CreateEditModule();
                    form.FrontendApplication = FrontendApplication;
                    form.BackendApplication = BackendApplication;
                    form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, typeof(Domain.Module));
                    ShowChildWindow(form);
                }
                else if (selectedNode.Tag is Domain.Dialog)
                {
                    //StartDialog.Dialog(dialog.Id, FrontendApplication, BackendApplication);
                    DialogObjectViewer form = new DialogObjectViewer();
                    form.FrontendApplication = FrontendApplication;
                    form.BackendApplication = BackendApplication;
                    form.IgnoreViewNodes = false;
                    form.WindowState = FormWindowState.Maximized;

                    form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));

                    ShowChildWindow(form);
                }
                else if (selectedNode.Tag is Domain.Workflow)
                {
                    WorkflowDesignerForm form = new WorkflowDesignerForm();
                    form.FrontendApplication = FrontendApplication;
                    form.BackendApplication = BackendApplication;
                    form.Workflow = ((Domain.Workflow)selectedNode.Tag);
                    form.WindowState = FormWindowState.Maximized;

                    form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));

                    ShowChildWindow(form);
                }
                else if (selectedNode.Tag is Domain.Service)
                {
                    using (NewEditService form = new NewEditService())
                    {
                        form.BackendApplication = BackendApplication;
                        form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));
                        form.Owner = this;

                        form.ShowDialog();
                    }
                }
                else if (selectedNode.Tag is Domain.ServiceMethod)
                {
                    using (CreateEditServiceMethod form = new CreateEditServiceMethod())
                    {

                        form.BackendApplication = BackendApplication;
                        form.FrontendApplication = FrontendApplication;
                        form.Owner = this;

                        form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));
                        form.ShowDialog();

                    }

                }
                else if (selectedNode.Tag is Domain.Action)
                {
                    using (CreateEditActionWizard form = new CreateEditActionWizard())
                    {

                        form.Owner = this;
                        form.FrontendApplication = FrontendApplication;
                        form.BackendApplication = BackendApplication;
                        form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));

                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            SaveSelectedNodeKyes(((MdiChildForm)form).ContaindDomainObjectIdAndType);
                        }
                    }
                }
                else if (selectedNode.Tag is Domain.Menu)
                {
                    using (AddMenu form = new AddMenu())
                    {
                        form.Owner = this;
                        form.FrontendApplication = FrontendApplication;
                        form.BackendApplication = BackendApplication;

                        form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));

                        form.ShowDialog();
                    }

                }
                else if (selectedNode.Tag is Domain.CustomDialog)
                {
                    using (AddCustomDialog form = new AddCustomDialog())
                    {

                        form.FrontendApplication = FrontendApplication;
                        form.BackendApplication = BackendApplication;
                        form.Owner = this;
                        form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));

                        form.ShowDialog();
                    }
                }
                else if (selectedNode.Tag is Domain.UXAction)
                {
                    using (EditUXAction form = new EditUXAction())
                    {
                        form.FrontendApplication = FrontendApplication;
                        form.BackendApplication = BackendApplication;
                        form.Owner = this;
                        form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));

                        form.ShowDialog();
                    }
                }
                else if (selectedNode.Tag is Domain.View)
                {
                    if (((Domain.View)selectedNode.Tag).Type == ViewType.Custom)
                    {
                        using (CreateCustomView form = new CreateCustomView())
                        {
                            form.EditView = dialogService.GetViewById(((Domain.View)selectedNode.Tag).Id);
                            form.BackendApplication = BackendApplication;
                            form.FrontendApplication = FrontendApplication;
                            form.Owner = this;
                            form.ShowDialog();
                        }
                    }
                    else
                    {
                        using (CrossHost.XamlViewerForm form = new CrossHost.XamlViewerForm())
                        {
                            form.RenderView = dialogService.GetViewById(((Domain.View)selectedNode.Tag).Id);
                            form.FrontendApplication = FrontendApplication;
                            form.BackendApplication = BackendApplication;
                            form.Dialog = null;
                            form.Owner = this;

                            form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));

                            form.ShowDialog();
                        }
                    }
                }
                else if (selectedNode.Tag is Domain.HintCollection)
                {
                    FindHintForm form = new FindHintForm();

                    form.Owner = this;
                    form.FrontendApplication = FrontendApplication;
                    form.BackendApplication = BackendApplication;
                    form.WindowState = FormWindowState.Maximized;

                    form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));

                    ShowChildWindow(form);

                }
                else if (selectedNode.Tag is Domain.BusinessEntity)
                {
                    CreateEditBusinessEntity form = new CreateEditBusinessEntity();

                    form.Owner = this;
                    form.FrontendApplication = FrontendApplication;
                    form.BackendApplication = BackendApplication;

                    form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));

                    ShowChildWindow(form);
                }
                else if (selectedNode.Tag is Domain.Report)
                {
                    using (CreateEditReport form = new CreateEditReport())
                    {
                        form.Owner = this;
                        form.FrontendApplication = FrontendApplication;
                        form.BackendApplication = BackendApplication;
                        form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));
                        form.ShowDialog();
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void tsmiDependencies_Click(object sender, EventArgs e)
        {
            if (cmsLevelTwo.Tag is TreeView)
            {
                TreeNode selectedNode = ((TreeView)cmsLevelTwo.Tag).SelectedNode;

                ShowDependencies form = new ShowDependencies();
                form.Owner = this;
                form.FrontendApplication = FrontendApplication;
                form.BackendApplication = BackendApplication;
                form.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(((DataAccess.IDomainObject)selectedNode.Tag).Id, modelService.GetDomainObjectType((DataAccess.IDomainObject)selectedNode.Tag));
                form.WindowState = FormWindowState.Maximized;
                ShowChildWindow(form);
            }
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Owner.Tag.GetType() == typeof(TreeView))
            {
                TreeView theTree = ((TreeView)((ToolStripMenuItem)sender).Owner.Tag);
                if (theTree.SelectedNode != null && theTree.SelectedNode.Tag != null)
                {
                    if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(theTree.SelectedNode.Tag.GetType()))
                    {
                        DataAccess.IVersionControlled domainObject = ((DataAccess.IVersionControlled)theTree.SelectedNode.Tag);

                        domainObject = (DataAccess.IVersionControlled)modelService.GetInitializedDomainObject(domainObject.Id, modelService.GetDomainObjectType(domainObject));

                        if (domainObject.IsLocked && domainObject.LockedBy == Environment.UserName)
                        {
                            try
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                modelService.DeleteDomainObject(domainObject);
                                Cursor.Current = Cursors.Default;
                            }
                            catch (Exception ex)
                            {
                                Cursor.Current = Cursors.Default;
                                string mess = ex.Message;

                                if (ex is ModelAggregatedException)
                                {
                                    string ids = string.Empty;
                                    foreach (string id in ((ModelAggregatedException)ex).Ids)
                                    {
                                        ids += id + "\r\n";
                                    }

                                    Clipboard.SetText(ids);
                                    mess += "\r\n\r\nThe Ids has been copied to the clipboard";
                                }

                                MessageBox.Show(mess);
                            }
                        }
                    }
                }
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateSelectedNode();
        }

        private void CheckOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckOutCheckInMenuOption(sender, true);
        }

        private void checkInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckOutCheckInMenuOption(sender, false);
        }

        private void undoCheckOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoCheckOutMenuOption(sender);
        }

        private void copyIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmsLevelTwo.Tag is TreeView)
            {
                TreeNode selectedNode = ((TreeView)cmsLevelTwo.Tag).SelectedNode;
                if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(selectedNode.Tag.GetType()))
                {
                    DataAccess.IVersionControlled domainObject = ((DataAccess.IVersionControlled)selectedNode.Tag);

                    Clipboard.SetText(domainObject.Id.ToString());
                }
            }
        }

        #endregion

        #region Configuration Management Functions

        private void UndoCheckOutMenuOption(object sender)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (((ToolStripMenuItem)sender).Owner.Tag.GetType() == typeof(TreeView))
            {
                TreeView theTree = ((TreeView)((ToolStripMenuItem)sender).Owner.Tag);
                if (theTree.SelectedNode != null && theTree.SelectedNode.Tag != null)
                {
                    if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(theTree.SelectedNode.Tag.GetType()))
                    {
                        DataAccess.IVersionControlled domainObject = ((DataAccess.IVersionControlled)theTree.SelectedNode.Tag);
                        Domain.Application owningApplication = GetApplicationForTreeNodeObject(theTree.SelectedNode);

                        if (GetChildFormsForDomainObject(domainObject).Count == 0)
                        {

                            try
                            {
                                Guid id = domainObject.Id;
                                Type classType = domainObject.GetType();
                                MetaManagerServices.GetConfigurationManagementService().UndoCheckOutDomainObject(id, classType, owningApplication);
                            }
                            catch (ConfigurationManagementException ex)
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show(ex.ToString());
                            }
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("You must close all windows handling the " + modelService.GetDomainObjectType(domainObject).Name + " before undoing the check out.", System.Windows.Forms.Application.OpenForms[0].Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void CheckOutCheckInMenuOption(object sender, bool trueCheckOut_falseCheckIn)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (((ToolStripMenuItem)sender).Owner.Tag.GetType() == typeof(TreeView))
            {
                TreeView theTree = ((TreeView)((ToolStripMenuItem)sender).Owner.Tag);
                if (theTree.SelectedNode != null && theTree.SelectedNode.Tag != null)
                {
                    if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(theTree.SelectedNode.Tag.GetType()))
                    {
                        DataAccess.IVersionControlled domainObject = ((DataAccess.IVersionControlled)theTree.SelectedNode.Tag);
                        Domain.Application owningApplication = GetApplicationForTreeNodeObject(theTree.SelectedNode);

                        if (GetChildFormsForDomainObject(domainObject).Count == 0)
                        {
                            if (trueCheckOut_falseCheckIn)
                            {
                                try
                                {
                                    MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(domainObject.Id, modelService.GetDomainObjectType(domainObject));
                                }
                                catch (ConfigurationManagementException ex)
                                {
                                    Cursor.Current = Cursors.Default;
                                    MessageBox.Show(ex.ToString());
                                }
                            }
                            else
                            {
                                try
                                {
                                    MetaManagerServices.GetConfigurationManagementService().CheckInDomainObject(domainObject.Id, modelService.GetDomainObjectType(domainObject), owningApplication);
                                }
                                catch (ConfigurationManagementException ex)
                                {
                                    Cursor.Current = Cursors.Default;
                                    MessageBox.Show(ex.ToString());
                                }
                            }
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("You must close all windows handling the " + modelService.GetDomainObjectType(domainObject).Name + " before checking in/out.", System.Windows.Forms.Application.OpenForms[0].Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void CompareWithPreviousMenuOption(object sender)
        {
            if (((ToolStripMenuItem)sender).Owner.Tag.GetType() == typeof(TreeView))
            {
                TreeView theTree = ((TreeView)((ToolStripMenuItem)sender).Owner.Tag);
                if (theTree.SelectedNode != null && theTree.SelectedNode.Tag != null)
                {
                    if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(theTree.SelectedNode.Tag.GetType()))
                    {
                        Cursor = Cursors.WaitCursor;
                        try
                        {
                            DataAccess.IVersionControlled domainObject = ((DataAccess.IVersionControlled)theTree.SelectedNode.Tag);
                            Domain.Application owningApplication = GetApplicationForTreeNodeObject(theTree.SelectedNode);

                            MetaManagerServices.GetConfigurationManagementService().DiffWithPreviousVersion(domainObject, owningApplication);
                        }
                        finally
                        {
                            Cursor = Cursors.Default;
                        }
                    }
                }
            }
        }

        private Domain.Application GetApplicationForTreeNodeObject(TreeNode currentNode)
        {
            if (currentNode.Tag is Domain.Application)
            {
                return modelService.GetInitializedDomainObject<Domain.Application>(((Domain.Application)currentNode.Tag).Id);
            }
            else if (currentNode.Parent == null)
            {
                return null;
            }
            else
            {
                return GetApplicationForTreeNodeObject(currentNode.Parent);
            }
        }

        #endregion

        #region Tree Search Functions
        private void FindlinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (((bool)searchTable.ExtendedProperties["LastSearchAdvanced"]) && string.IsNullOrEmpty(FindNametextBox.Text))
            {
                AFindlinkLabel_LinkClicked(sender, e);
            }
            else
            {
                string searchCriteria;
                Guid dummy = new Guid();

                if (Guid.TryParse(FindNametextBox.Text, out dummy))
                {
                    searchCriteria = "Id = '" + FindNametextBox.Text + "'";
                }
                else
                {
                    searchCriteria = FindNametextBox.Tag.ToString() + " LIKE '" + FindNametextBox.Text + "%'";
                }

                searchTable.ExtendedProperties["LastSearchAdvanced"] = false;

                SearchApplicationTreeView(searchCriteria);
            }
        }

        private void AdvancedFindlinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AFindIdtextBox.Text = string.Empty;
            AFindNametextBox.Text = string.Empty;
            AFindTypecomboBox.Text = string.Empty;

            FindNametextBox.Visible = false;
            FindlinkLabel.Visible = false;
            AdvancedFindlinkLabel.Visible = false;

            applicationTreeView.Top = AdvancedFindgroupBox.Bottom + 6;
            AdvancedFindgroupBox.Visible = true;
        }

        private void AFindlinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string searchCriteria = string.Empty;

            if (!string.IsNullOrEmpty(AFindIdtextBox.Text))
            {
                searchCriteria = AFindIdtextBox.Tag.ToString() + " = '" + AFindIdtextBox.Text + "'";
            }

            if (!string.IsNullOrEmpty(AFindNametextBox.Text))
            {
                searchCriteria += string.IsNullOrEmpty(searchCriteria) ? "" : " AND ";
                searchCriteria += AFindNametextBox.Tag.ToString() + " LIKE '" + AFindNametextBox.Text + "%'";
            }

            if (!string.IsNullOrEmpty(AFindTypecomboBox.Text))
            {
                searchCriteria += string.IsNullOrEmpty(searchCriteria) ? "" : " AND ";
                searchCriteria += AFindTypecomboBox.Tag.ToString() + " = '" + AFindTypecomboBox.Text + "'";
            }

            if (!string.IsNullOrEmpty(AFindFreetextBox.Text))
            {
                searchCriteria += string.IsNullOrEmpty(searchCriteria) ? "" : " AND ";
                searchCriteria += AFindFreetextBox.Tag.ToString() + " LIKE '%" + AFindFreetextBox.Text + "%'";
            }

            FindNametextBox.Text = string.Empty;
            searchTable.ExtendedProperties["LastSearchAdvanced"] = true;

            SearchApplicationTreeView(searchCriteria);

            ACancellinkLabel_LinkClicked(sender, e);
        }

        private void ACancellinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            applicationTreeView.Top = FindNametextBox.Bottom + 6;
            AdvancedFindgroupBox.Visible = false;

            FindNametextBox.Visible = true;
            FindlinkLabel.Visible = true;
            AdvancedFindlinkLabel.Visible = true;
        }

        private void SearchApplicationTreeView(string searchCriteria)
        {
            lock (searchTable)
            {
                if (SearchTableLocked)
                {
                    return;
                }
                else
                {
                    SearchTableLocked = true;
                }
            }

            lock (searchTable)
            {
                if (!loadingSearchTable)
                {
                    IEnumerable<DataRow> deleterows = searchTable.Rows.OfType<DataRow>().Where(r => ((TreeNode)r["TreeNode"]).TreeView == null);
                    while (deleterows.Count() > 0)
                    {
                        if (searchResult != null)
                        {
                            if (searchResult.Contains(deleterows.ElementAt(0)))
                            {
                                searchTable.ExtendedProperties["SearchCriteria"] = string.Empty;
                            }
                        }
                        searchTable.Rows.Remove(deleterows.ElementAt(0));

                    }
                }
            }

            if (searchTable.ExtendedProperties["SearchCriteria"].ToString() == searchCriteria && searchResult != null)
            {
                if (searchResult.Length > 0)
                {
                    int resultIndex = ((int)searchTable.ExtendedProperties["ResultIndex"]);

                    resultIndex += 1;

                    if (searchResult.Length <= resultIndex)
                    {
                        resultIndex = 0;
                    }

                    searchTable.ExtendedProperties["ResultIndex"] = resultIndex;

                    applicationTreeView.SelectedNode = ((TreeNode)searchResult[resultIndex]["TreeNode"]);
                    applicationTreeView.SelectedNode.EnsureVisible();
                    applicationTreeView.Focus();
                }
            }
            else
            {
                searchTable.ExtendedProperties["SearchCriteria"] = searchCriteria;
                searchResult = SearchLoadedNodes(searchCriteria, true, false);
                searchTable.ExtendedProperties["ResultIndex"] = 0;

                SearchtoolStripStatusLabel.Text = "Search result: " + searchResult.Length + " objects";

                if (searchResult.Length > 0)
                {

                    applicationTreeView.SelectedNode = ((TreeNode)searchResult[0]["TreeNode"]);
                    applicationTreeView.SelectedNode.EnsureVisible();
                    applicationTreeView.Focus();
                }
            }

            lock (searchTable)
            {
                SearchTableLocked = false;
            }

        }

        private DataRow[] SearchLoadedNodes(string searchCriteria, bool onlyInTreeViewSorted, bool onLockWait)
        {
            if (searchTable == null)
            {
                return new DataRow[]{};
            }

            bool searchLock = false;

            if (onLockWait)
            {
                lock (searchTable)
                {
                    if (SearchTableLocked)
                    {
                        searchLock = true;
                    }
                    else
                    {
                        SearchTableLocked = true;
                    }
                }

                while (searchLock)
                {
                    lock (searchTable)
                    {
                        if (!SearchTableLocked)
                        {
                            SearchTableLocked = true;
                            searchLock = false;
                        }
                    }
                    System.Threading.Thread.Sleep(20);
                }
            }

            DataRow[] result;
            result = searchTable.Select(searchCriteria);
            if (onlyInTreeViewSorted)
            {
                result = result.Where(o => ((TreeNode)o["TreeNode"]).TreeView != null).ToArray<DataRow>();
                result = result.OrderBy(o => ((TreeNode)o["TreeNode"]).FullPath).ToArray();
            }

            if (onLockWait)
            {
                lock (searchTable)
                {
                    SearchTableLocked = false;
                }
            }

            return result;
        }

        #endregion

        private void LoadLazyTreeNodesInBackgroud(object node)
        {
            bool first = false;
            TreeNodeCollection nodes;
            if (node == null)
            {
                nodes = applicationTreeView.Nodes;
                first = true;
            }
            else
            {
                nodes = ((TreeNode)node).Nodes;
            }

            foreach (TreeNode n in nodes)
            {
                if (stopLoading) 
                { return; }
                System.Threading.Thread.Sleep(25);

                if (n.Tag is KeyValuePair<string, Type>)
                {
                    if (((KeyValuePair<string, Type>)n.Tag).Key.EndsWith("LAZY"))
                    {
                        applicationTreeView.Invoke(new ThreadJumpDelegate(delegate
                        {
                            LoadStatustoolStripStatusLabel.Text = "Loading: " + ((KeyValuePair<string, Type>)n.Tag).Value.Name;
                            System.Windows.Forms.Application.DoEvents();
                        }));
                        BuildChildNodes(n, ((KeyValuePair<string, Type>)n.Tag).Value, true, true, true);
                    }
                }
                else
                {
                    LoadLazyTreeNodesInBackgroud(n);
                }
            }

            if (first)
            {
                if (stopLoading) 
                { return; }
                statusStrip1.Invoke(new ThreadJumpDelegate(delegate { LoadStatustoolStripStatusLabel.Text = "Loading: Done"; }));
            }
        }

        public List<MdiChildForm> GetChildFormsForDomainObject(DataAccess.IDomainObject domainObject)
        {
            List<MdiChildForm> childList = new List<MdiChildForm>();
            Guid containedObjectId;
            Type containedObjectType;

            foreach (Form child in this.MdiChildren)
            {
                if (child is MdiChildForm)
                {
                    containedObjectId = ((MdiChildForm)child).ContaindDomainObjectIdAndType.Key;
                    containedObjectType = ((MdiChildForm)child).ContaindDomainObjectIdAndType.Value;

                    if (containedObjectType != null)
                    {
                        if (containedObjectId == domainObject.Id && containedObjectType == modelService.GetDomainObjectType(domainObject))
                        {
                            childList.Add((MdiChildForm)child);
                        }
                    }
                }
            }

            return childList;
        }

        private void ShowChildWindow(Form childForm)
        {
            if (childForm is MdiChildForm)
            {
                IList<Form> childs = this.MdiChildren.Where(f => f.GetType() == childForm.GetType() && ((MdiChildForm)f).ContaindDomainObjectIdAndType.Key == ((MdiChildForm)childForm).ContaindDomainObjectIdAndType.Key).ToList();

                if (childs.Count == 1)
                {
                    if (((MdiChildForm)childs[0]).IsEditable != ((MdiChildForm)childForm).IsEditable)
                    {
                        childs[0].Close();
                        childForm.MdiParent = this;
                        childForm.Show();
                    }
                    else
                    {
                        childForm.Dispose();
                        this.ActivateMdiChild(childs[0]);
                        childs[0].BringToFront();
                    }
                }
                else if (childs.Count == 0)
                {
                    childForm.MdiParent = this;
                    childForm.Show();
                }
                else
                {
                    while (childs.Count > 0)
                    {
                        childs[0].Close();
                        childs.RemoveAt(0);
                    }

                    childForm.MdiParent = this;
                    childForm.Show();
                }
            }
        }

        private void addAllToSourceControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmsLevelZero.Tag is TreeView)
            {
                if (((TreeView)cmsLevelZero.Tag).SelectedNode.Tag is DeploymentGroup)
                {
                    using (StatusAddToSourceControl form = new StatusAddToSourceControl(((DeploymentGroup)((TreeView)cmsLevelZero.Tag).SelectedNode.Tag)))
                    {
                        form.ShowDialog();
                    }
                }
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                FindlinkLabel_LinkClicked(sender, null);
            }
            else if (e.KeyCode == Keys.Return)
            {
                if (FindNametextBox.Focused)
                {
                    FindlinkLabel_LinkClicked(sender, null);
                }
                else if (AFindIdtextBox.Focused || AFindNametextBox.Focused || AFindTypecomboBox.Focused || AFindFreetextBox.Focused)
                {
                    AFindlinkLabel_LinkClicked(sender, null);
                }
            }
        }

        private void jumpToActionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = ((TreeView)cmsLevelTwo.Tag).SelectedNode;
            Domain.ServiceMethod selectedServiceMethod = null;

            DataAccess.IVersionControlled domainObject = null;
            domainObject = (DataAccess.IVersionControlled)selectedNode.Tag;

            selectedServiceMethod = modelService.GetInitializedDomainObject<Domain.ServiceMethod>(domainObject.Id);
            //string searchCriteria = "Id = " + selectedServiceMethod.MappedToAction.Id.ToString();
            //SearchApplicationTreeView(searchCriteria);

            AFindIdtextBox.Text = selectedServiceMethod.MappedToAction.Id.ToString();

            AFindlinkLabel_LinkClicked(sender, null);

        }

        private void findCheckOutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeView theTree = ((TreeView)((ToolStripMenuItem)sender).Owner.Tag);

            if (theTree.SelectedNode != null && theTree.SelectedNode.Tag != null)
            {
                FindCheckOuts form = new FindCheckOuts();
                form.FrontendApplication = FrontendApplication;
                form.BackendApplication = BackendApplication;
                form.WindowState = FormWindowState.Maximized;
                ShowChildWindow(form);
            }
        }

        public void JumpToDomainObject(DataAccess.IDomainObject obj, bool checkOut)
        {
            string searchCriteria = string.Empty;

            searchResult = null;
            searchCriteria = "Id = '" + obj.Id.ToString() + "' AND Type = '" + modelService.GetDomainObjectType(obj).Name + "'";
            SearchApplicationTreeView(searchCriteria);


            if (applicationTreeView.SelectedNode.Tag is DataAccess.IDomainObject)
            {
                if (((DataAccess.IDomainObject)applicationTreeView.SelectedNode.Tag).Id == obj.Id)
                {
                    if (checkOut)
                    {
                        try
                        {
                            MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(obj.Id, modelService.GetDomainObjectType(obj));
                            applicationTreeView.SelectedNode.Tag = modelService.GetDomainObject(obj.Id, modelService.GetDomainObjectType(obj));
                            UpdateSelectedNode();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    tsmiOpen_Click(this, null);
                }
            }
        }

        private void importUpdateDataModelToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            if (cmsLevelTwo.Tag is TreeView)
            {
                TreeNode selectedNode = ((TreeView)cmsLevelTwo.Tag).SelectedNode;

                if (selectedNode.Tag is Domain.Application)
                {
                    Domain.Application app = (Domain.Application)selectedNode.Tag;
                    if (!app.IsFrontend.Value)
                    {

                        using (DataModelImportForm form = new DataModelImportForm())
                        {
                            form.Owner = this;
                            form.BackendApplication = BackendApplication;
                            form.ShowDialog();
                        }
                    }
                }

            }
            Cursor.Current = Cursors.Default;
        }

        private void ChildWindowstoolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MdiChildForm form = ((MdiChildForm)ChildWindowstoolStripComboBox.Items[ChildWindowstoolStripComboBox.SelectedIndex]);
            form.Activate();
        }

        private void generateReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmsLevelTwo.Tag is TreeView)
            {
                TreeNode selectedNode = ((TreeView)cmsLevelTwo.Tag).SelectedNode;

                if (selectedNode.Tag is Domain.Application)
                {
                    Domain.Application app = (Domain.Application)selectedNode.Tag;
                    if (!app.IsFrontend.Value)
                    {
                        using (GenerateReports form = new GenerateReports())
                        {
                            form.Owner = this;
                            form.BackendApplication = app;
                            form.ShowDialog();
                        }
                    }
                }
            }
        }

        private void importXMLDataToolStripMenuItem_Click(object sender, EventArgs e)
        {

            TreeView theTree = ((TreeView)((ToolStripMenuItem)sender).Owner.Tag);

            if (theTree.SelectedNode != null && theTree.SelectedNode.Tag != null)
            {
                ImportSelectedObjects form = new ImportSelectedObjects();
                form.FrontendApplication = FrontendApplication;
                form.BackendApplication = BackendApplication;
                form.WindowState = FormWindowState.Maximized;

                ShowChildWindow(form);
            }


        }

        private void Main_Shown(object sender, EventArgs e)
        {
            if (VerifyRepositoryConfiguration())
            {
                if (VerifyDatabaseStructure())
                {
                    PopulateDeploymentGroupCombobox();
                }
            }
        }

        private static bool VerifyRepositoryConfiguration()
        {
            NameValueCollection section = ConfigurationManager.GetSection("databaseSettings") as NameValueCollection;
            string connectionString = section["db.connectionstring"];

            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = Path.Combine(ConfigurationManager.AppSettings["RepositoryPath"], "Metadata.config");

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            ConnectionStringSettings connSettings = config.ConnectionStrings.ConnectionStrings["Metadata"];

            if (connSettings == null || connSettings.ConnectionString.ToLower() != connectionString.ToLower())
            {
                if (MessageBox.Show(string.Format("The correct CM repository could not be verified, please check your repository path. Do you want to continue anyway?", map.ExeConfigFilename), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    System.Windows.Forms.Application.Exit();
                    return false;
                }
            }

            return true;
        }

        private bool VerifyDatabaseStructure()
        {

            if (!appService.MMDbExsisting())
            {
                                                    
                if (MessageBox.Show("The database tables are missing. Do you want to create them?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                {
                    System.Windows.Forms.Application.Exit();
                    return false;
                }

                NameValueCollection section = ConfigurationManager.GetSection("databaseSettings") as NameValueCollection;
                string connectionString = section["db.connectionstring"];

                if (MessageBox.Show(string.Format("The database with this connectionstring\r\n{0}\r\n will be replaced. \r\nDo you want to continue?", connectionString), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    System.Windows.Forms.Application.Exit();
                    return false;
                }

                MetaManager.CreateMetaDb.Program.MakeDb();  
            }

            return true;
        }

        private void FindNametextBox_DoubleClick(object sender, EventArgs e)
        {
            FindNametextBox.SelectAll();
        }

        private void AFindIdtextBox_DoubleClick(object sender, EventArgs e)
        {
            AFindIdtextBox.SelectAll();
        }

        private void AFindNametextBox_DoubleClick(object sender, EventArgs e)
        {
            AFindNametextBox.SelectAll();
        }

        private void AFindFreetextBox_DoubleClick(object sender, EventArgs e)
        {
            AFindFreetextBox.SelectAll();
        }

        private void compareWithLatestVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompareWithPreviousMenuOption(sender);
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (applicationTreeView.SelectedNode.Tag is DataAccess.IDomainObject)
            {
                objectToMove = ((DataAccess.IVersionControlled)applicationTreeView.SelectedNode.Tag);
            }
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (applicationTreeView.SelectedNode.Tag is KeyValuePair<string, Type> && ((KeyValuePair<string, Type>)applicationTreeView.SelectedNode.Tag).Value == modelService.GetDomainObjectType(objectToMove))
                {
                    if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(applicationTreeView.SelectedNode.Parent.Tag.GetType()))
                    {
                        DataAccess.IVersionControlled newParentObject = ((DataAccess.IVersionControlled)applicationTreeView.SelectedNode.Parent.Tag);

                        modelService.MoveDomainObject(objectToMove, newParentObject);
                        objectToMove = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void generateCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form formToClose in this.MdiChildren.Where(f => f.GetType() == typeof(FindCheckOuts) || f.GetType() == typeof(ImportSelectedObjects)))
            {
                formToClose.Close();
            }

            GenerateCode form = new GenerateCode();
            form.FrontendApplication = FrontendApplication;
            form.BackendApplication = BackendApplication;

            form.ShowDialog();
        }

        private void VersiontoolStripStatusLabel_Click(object sender, EventArgs e)
        {
            ReleaseNotes form = new ReleaseNotes();
            form.ShowDialog();
        }

        private void importChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeView theTree = ((TreeView)((ToolStripMenuItem)sender).Owner.Tag);

            if (theTree.SelectedNode != null && theTree.SelectedNode.Tag != null)
            {
                ImportChange form = new ImportChange();
                form.FrontendApplication = FrontendApplication;
                form.BackendApplication = BackendApplication;
                form.WindowState = FormWindowState.Maximized;

                ShowChildWindow(form);
            }

        }

        

   
    }
}


