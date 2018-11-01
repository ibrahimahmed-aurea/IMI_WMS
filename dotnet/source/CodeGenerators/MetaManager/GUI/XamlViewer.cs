using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Markup;
using System.IO;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.BusinessLogic.Helpers;
using Cdc.MetaManager.GUI;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplates;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Workflow.Activities.Rules.Design;
using System.Workflow.Activities.Rules;
using System.Windows;
using System.Reflection;

using NHibernate;

namespace CrossHost
{
    public partial class XamlViewerForm : MdiChildForm
    {
        private string xamlSource;
        private Cdc.MetaManager.DataAccess.Domain.View renderView;
        private bool isLoaded = false;
        private string lastRender;
        private IDialogService dialogService;
        private IApplicationService applicationService;
        private IList<UXComponent> componentList;

        private IModelService modelService = null;
        

        public string XamlSource
        {
            get
            {
                return xamlSource;
            }
            set
            {
                xamlSource = value;

                if (isLoaded)
                {
                    RenderXaml(xamlSource);
                    XamlTextBox.Text = xamlSource;
                }
            }
        }

        public Dialog Dialog { get; set; }
        public string VisualTreeXml { get; set; }

        private TreeNode TopNode
        {
            get
            {
                if ((viewTreeView.Nodes != null) && (viewTreeView.Nodes.Count > 0))
                    return viewTreeView.Nodes[0];
                else
                    return null;
            }
        }

        public Cdc.MetaManager.DataAccess.Domain.View RenderView
        {
            get
            {
                return renderView;
            }
            set
            {
                renderView = value;
                VisualTreeXml = value.VisualTreeXml;
                XamlSource = GenerateXaml(renderView);
                BuildTreeView(RenderView.VisualTree);
            }
        }

        private DataSource SelectedDataSource
        {
            get
            {
                if (lvDatasources.SelectedItems.Count == 1)
                {
                    return (DataSource)lvDatasources.SelectedItems[0].Tag;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (lvDatasources.SelectedItems.Count == 1)
                    lvDatasources.SelectedItems[0].Tag = value;
            }
        }

        private string GenerateXaml(Cdc.MetaManager.DataAccess.Domain.View renderView)
        {
            ViewXamlTemplate viewXamlTemplate = new ViewXamlTemplate();
            viewXamlTemplate.view = renderView;
            viewXamlTemplate.debugOutput = true;
            string source = viewXamlTemplate.RenderToString();

            return source;
        }

        List<UXComponent> componentLookup = new List<UXComponent>();

        private void BuildTreeView(UXContainer uxContainer)
        {
            viewTreeView.BeginUpdate();
            viewTreeView.ShowNodeToolTips = true;

            try
            {
                viewTreeView.Nodes.Clear();
                componentLookup.Clear();
                TreeNode root = AddNode(uxContainer, null);
                viewTreeView.Nodes.Add(root);
                componentLookup.Add(uxContainer);
                root.Expand();

                if (root.Nodes.Count > 0)
                {
                    if (root.Nodes[0].Tag is UXExpander)
                    {
                        foreach (TreeNode n in root.Nodes)
                        {
                            n.Expand();
                        }
                    }
                }
            }
            finally
            {
                viewTreeView.EndUpdate();
                viewTreeView.SelectedNode = TopNode;
            }

        }


        private string GetMappedPropertyName(IBindable bindable)
        {
            string name = "";

            if (bindable.MappedProperty != null)
                name = bindable.MappedProperty.Name;

            return name;
        }

        private TreeNode AddNode(UXComponent child, TreeNode parent)
        {
            return AddNode(child, parent, -1);
        }

        private TreeNode AddNode(UXComponent child, TreeNode parent, int index)
        {
            TreeNode node = new TreeNode();
            node.Tag = child;
            node.Text = child.Name;
            node.ToolTipText = string.Format("{0} ({1})", child.Name, child.GetType().Name);
            componentLookup.Add(child);

            if (child.RuleSetWrapper.RuleSet != null)
                node.ForeColor = Color.Blue;

            if (child is IBindable)
            {
                node.SelectedImageIndex = 1;

                if ((child as IBindable).MappedProperty == null)
                    node.ForeColor = Color.Red;
            }

            if (parent != null)
            {
                if (index > -1)
                    parent.Nodes.Insert(index, node);
                else
                    parent.Nodes.Add(node);
            }

            if (child is UXLabel)
            {
                UXLabel label = child as UXLabel;
                node.Text = label.Caption;
                node.ImageIndex = 0;
                node.SelectedImageIndex = 0;
            }
            else if (child is UXComboDialog)
            {
                node.ImageIndex = 12;
                node.SelectedImageIndex = 12;
            }
            else if (child is UXTextBox)
            {
                node.ImageIndex = 1;
                node.SelectedImageIndex = 1;
            }
            else if (child is UXRadioGroup)
            {
                node.ImageIndex = 5;
                node.SelectedImageIndex = 5;
            }
            else if (child is UXCheckBox)
            {
                node.ImageIndex = 2;
                node.SelectedImageIndex = 2;
            }
            else if (child is UXDataGridColumn)
            {
                node.ImageIndex = 9;
                node.SelectedImageIndex = 9;
                node.Text = (child as UXDataGridColumn).Caption;
            }
            else if (child is UXDataGrid)
            {
                node.ImageIndex = 8;
                node.SelectedImageIndex = 8;

                UXDataGrid grid = child as UXDataGrid;

                foreach (UXDataGridColumn column in grid.Children.Cast<UXDataGridColumn>())
                {
                    AddNode(column, node);
                }
            }
            else if (child is UXComboBox)
            {
                node.ImageIndex = 3;
                node.SelectedImageIndex = 3;
            }
            else if (child is UXGroupBox)
            {
                UXGroupBox box = child as UXGroupBox;
                node.Text = box.Caption;

                if (box.Container != null)
                    AddNode(box.Container, node);
                
                node.ImageIndex = 4;
                node.SelectedImageIndex = 4;
            }

            else if (child is UXExpander)
            {
                UXExpander exp = child as UXExpander;
                node.Text = exp.Caption;
                node.ImageIndex = 13;
                node.SelectedImageIndex = 13;

                foreach (UXComponent c in exp.Children)
                {
                    AddNode(c, node);
                }
            }
            else if (child is UXLayoutGrid)
            {
                node.ImageIndex = 7;
                node.SelectedImageIndex = 7;

                UXLayoutGrid layoutGrid = child as UXLayoutGrid;

                IEnumerable<UXComponent> sortedComponents =
                    from UXLayoutGridCell cell in layoutGrid.Cells
                    orderby cell.Row, cell.Column
                    select cell.Component;

                foreach (UXComponent component in sortedComponents)
                {
                    AddNode(component, node);
                }
            }
            else if (child is UXSearchPanel)
            {
                node.ImageIndex = 11;
                node.SelectedImageIndex = 11;

                UXContainer container = child as UXContainer;

                foreach (UXComponent c in container.Children)
                {
                    AddNode(c, node);
                }
            }
            else if (child is UXSearchPanelItem)
            {
                node.ImageIndex = 10;
                node.SelectedImageIndex = 10;

                UXSearchPanelItem item = child as UXSearchPanelItem;
                node.Text = item.Caption;

                foreach (UXComponent c in item.Children)
                {
                    AddNode(c, node);
                }

            }
            else if (child is UXContainer)
            {
                node.ImageIndex = 6;
                node.SelectedImageIndex = 6;

                UXContainer container = child as UXContainer;

                foreach (UXComponent c in container.Children)
                {
                    AddNode(c, node);
                }
            }
            else if (child is UXHyperDialog)
            {
                node.ImageIndex = 14;
                node.SelectedImageIndex = 14;
            }

            return node;
        }

        public XamlViewerForm()
        {
            InitializeComponent();
            dialogService = MetaManagerServices.GetDialogService();
            applicationService = MetaManagerServices.GetApplicationService();

            modelService = MetaManagerServices.GetModelService();

            Assembly.Load("Xceed.Wpf.Controls.v6.0");
        }


        private void RenderTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            if (RenderTabControl.SelectedTab == xamlViewerTab)
            {
                RenderXaml(XamlTextBox.Text);
            }
        }

        private void RenderXaml(string source)
        {
            if (string.IsNullOrEmpty(source))
                return;

            if ((!string.IsNullOrEmpty(lastRender)) && lastRender.Equals(source))
                return;

            try
            {
                InnerRender(source);
                lastRender = source;
            }
            catch (Exception ex)
            {
                string xamlError = string.Format("<WrapPanel xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"><TextBlock>{0}</TextBlock></WrapPanel>", ex.Message);
                try
                {
                    InnerRender(xamlError);
                }
                catch (Exception) { }
            }

            return;
        }

        private void InnerRender(string source)
        {
            Stream s = new MemoryStream(UTF8Encoding.UTF8.GetBytes(source));
            XamlReader r = new XamlReader();
            
            UIElement uiElement = XamlReader.Load(s) as UIElement;
            
            ElementHost.Child = uiElement;
            ElementHost.Child.Dispatcher.UnhandledException += (sender, e) =>
                {
                    e.Handled = true;
                };
        }

        private void XamlViewerFormLoad(object sender, EventArgs e)
        {
            
            if (isLoaded == false)
            {
                if ((xamlSource == null) && (renderView == null))
                {
                    xamlSource = XamlTextBox.Text;
                }
                else
                {
                    if (renderView != null)
                    {
                        xamlSource = GenerateXaml(renderView);
                    }
                }
            }

            this.IsEditable = this.renderView.IsLocked && this.renderView.LockedBy == Environment.UserName;

            isLoaded = true;

            RenderXaml(xamlSource);
            XamlTextBox.Text = xamlSource;
                        
            componentList =  ViewHelper.GetAllComponents<UXComponent>(RenderView.VisualTree);

            BuildTreeView(RenderView.VisualTree);

            PopulateDataSources();

            EnableDisableToolstripButtons();

            Cursor.Current = Cursors.Default;
        }

        private void viewTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid.SelectedObject = viewTreeView.SelectedNode.Tag;

            if (propertyGrid.SelectedObject != null)
            {
                UXComponent component = viewTreeView.SelectedNode.Tag as UXComponent;

                if (component != null)
                    componentLabel.Text = string.Format("{0} ({1})", component.Name, component.GetType().Name);

                UXDataGridColumn column = viewTreeView.SelectedNode.Tag as UXDataGridColumn;

                if (column != null)
                    componentLabel.Text = string.Format("{0} ({1})", column.Caption, column.GetType().Name);
            }
            else
            {
                componentLabel.Text = "none";
            }

            EnableDisableToolstripButtons();
        }

        private void EnableDisableToolstripButtons()
        {
            okBtn.Enabled = this.IsEditable;

            editGroupboxLayout.Enabled = false;
            upBtn.Enabled = false;
            downBtn.Enabled = false;
            
            addExpanderItem.Enabled = false;
            removeExpanderItem.Enabled = false;
            addGroupboxItem.Enabled = false;
            removeGroupboxItem.Enabled = false;

            
            addColumnToolStripMenuItem.Enabled = false;
            removeColumnToolStripMenuItem.Enabled = false;


            if (viewTreeView.SelectedNode != null &&
                viewTreeView.SelectedNode.Tag != null)
            {               

               
                if (viewTreeView.SelectedNode == TopNode)
                {
                    addExpanderItem.Enabled = this.IsEditable;
                    addGroupboxItem.Enabled = (TopNode.Tag is UXWrapPanel && this.IsEditable);
                }
                else
                {
                    addGroupboxItem.Enabled = (viewTreeView.SelectedNode.Tag is UXExpander && this.IsEditable);
                }

                // Groupbox or expander
                if ((viewTreeView.SelectedNode.Tag is UXGroupBox) ||
                     (viewTreeView.SelectedNode.Tag is UXExpander))
                {
                    UXContainer selectedContainer = viewTreeView.SelectedNode.Tag as UXContainer;
                    int currentIdx = selectedContainer.Parent.Children.IndexOf(selectedContainer);

                    if (currentIdx > 0)
                        upBtn.Enabled = true;

                    if (currentIdx < selectedContainer.Parent.Children.Count - 1)
                        downBtn.Enabled = true;
                }

                if (viewTreeView.SelectedNode.Tag is UXGroupBox)
                {
                    editGroupboxLayout.Enabled = this.IsEditable;
                    removeGroupboxItem.Enabled = this.IsEditable;
                }
                if (viewTreeView.SelectedNode.Tag is UXLayoutGrid)
                {
                    editGroupboxLayout.Enabled = this.IsEditable;
                }
                else if (viewTreeView.SelectedNode.Tag is UXSearchPanelItem)
                {
                    upBtn.Enabled = viewTreeView.SelectedNode.PrevNode != null;
                    downBtn.Enabled = viewTreeView.SelectedNode.NextNode != null;
                }
                else if (viewTreeView.SelectedNode.Tag is UXExpander)
                {
                    removeExpanderItem.Enabled = this.IsEditable;
                }
                else if (viewTreeView.SelectedNode.Tag is UXDataGrid)
                {                   
                    addColumnToolStripMenuItem.Enabled = this.IsEditable;            
                }
                else if (viewTreeView.SelectedNode.Tag is UXDataGridColumn)
                {
                    removeColumnToolStripMenuItem.Enabled = this.IsEditable;
                }
                    
            }
        }

        private void editGroupboxLayout_Click(object sender, EventArgs e)
        {
            using (EditGroupboxLayout groupBoxLayoutForm = new EditGroupboxLayout())
            {
                UXGroupBox groupBox = viewTreeView.SelectedNode.Tag as UXGroupBox;

                if (groupBox == null)
                {
                    UXLayoutGrid layoutGrid = viewTreeView.SelectedNode.Tag as UXLayoutGrid;

                    if (layoutGrid != null)
                    {
                        groupBox = viewTreeView.SelectedNode.Parent.Tag as UXGroupBox;
                    }
                }

                // Legacy code
                if (groupBox == null)
                {
                    groupBox = new UXGroupBox();
                    groupBox.Container = (UXLayoutGrid)viewTreeView.SelectedNode.Tag;
                    groupBox.Parent = (UXContainer)TopNode.Tag;
                }


                var componentNames = (from c in ViewHelper.GetAllComponents<UXComponent>(RenderView.VisualTree)
                                        select c.Name).ToList();

                groupBoxLayoutForm.Owner = this;
                groupBoxLayoutForm.EditGroupbox = groupBox;
                groupBoxLayoutForm.PropertyMap = RenderView.ResponseMap;
                groupBoxLayoutForm.ComponentNames = componentNames;
                groupBoxLayoutForm.FrontendApplication = FrontendApplication;
                groupBoxLayoutForm.BackendApplication = BackendApplication;
                groupBoxLayoutForm.IsEditable = IsEditable;

                // Set read-only property depending on dilaog type
                if (Dialog != null)
                {
                    groupBoxLayoutForm.IsViewReadOnly = (Dialog.Type == DialogType.Drilldown ||
                                                                   Dialog.Type == DialogType.Overview);
                }

                if (groupBoxLayoutForm.ShowDialog() == DialogResult.OK)
                {
                    xamlSource = GenerateXaml(renderView);
                    RenderXaml(xamlSource);
                    UpdateNode(viewTreeView.SelectedNode, true, true);
                    XamlTextBox.Text = xamlSource;
                }
            }
        }

        private void upBtn_Click(object sender, EventArgs e)
        {
            UXComponent comp = viewTreeView.SelectedNode.Tag as UXComponent;

            MoveComponent(comp, true);
        }

        private void downBtn_Click(object sender, EventArgs e)
        {
            UXComponent comp = viewTreeView.SelectedNode.Tag as UXComponent;

            MoveComponent(comp, false);
        }

        private Stack<int> nodePath = new Stack<int>();

        private void MoveComponent(UXComponent moveComponent, bool moveUp)
        {
            int currentIdx = moveComponent.Parent.Children.IndexOf(moveComponent);

            int newIdx = moveUp ? currentIdx - 1 : currentIdx + 1;

            if (newIdx < moveComponent.Parent.Children.Count && newIdx >= 0)
            {
                PushSelectedNodePath(newIdx);

                UXComponent otherComp = moveComponent.Parent.Children[newIdx];
                moveComponent.Parent.Children[newIdx] = moveComponent;
                moveComponent.Parent.Children[currentIdx] = otherComp;

                BuildTreeView(RenderView.VisualTree);

                PopNodePath();
            }
        }

        #region Refresh Helpers

        private void PopNodePath()
        {
            TreeNode n = TopNode;

            nodePath.Pop();

            while (nodePath.Count > 0)
            {
                int idx = nodePath.Pop();
                n = n.Nodes[idx];
            }

            viewTreeView.SelectedNode = n;
        }

        private void PushSelectedNodePath(int newIdx)
        {
            TreeNode n = viewTreeView.SelectedNode;

            nodePath.Clear();

            bool first = true;

            while (n != null)
            {
                if (first)
                {
                    first = false;
                    nodePath.Push(newIdx);
                }
                else
                    nodePath.Push(n.Index);

                n = n.Parent;
            }
        }

        #endregion

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            Render();
        }

        private void Render()
        {
            XamlSource = GenerateXaml(renderView);
            BuildTreeView(RenderView.VisualTree);
        }

        private void contextMenuStripOpening(object sender, CancelEventArgs e)
        {
            mapComponentMenuItem.Enabled = false;
            changeComponentTypeMenuItem.Enabled = false;
            componentMapMenuItem.Enabled = false;
            editRuleSetItem.Enabled = false;
            jumpToActionToolStripMenuItem.Enabled = false;
            assignHintToolStripMenuItem.Enabled = false;
            editXMLToolStripMenuItem.Visible = false;
            moveToGroupboxToolStripMenuItem.Visible = false;
            changeDisplayFormatToolStripMenuItem.Visible = false;
            changeDefaultValueToolStripMenuItem.Visible = false;
            gridColumnToolStripMenuItem.Visible = false;
            groupboxToolStripMenuItem.Visible = false;
            
            expanderToolStripMenuItem.Visible = false;
            
            if (viewTreeView.SelectedNode.Parent == null)
                editXMLToolStripMenuItem.Visible = this.IsEditable;

            UXServiceComponent serviceComponent = viewTreeView.SelectedNode.Tag as UXServiceComponent;

            if (serviceComponent != null)
            {
                componentMapMenuItem.Enabled = (serviceComponent.ServiceMethod != null);
                jumpToActionToolStripMenuItem.Enabled = (serviceComponent.ServiceMethod != null);
            }

            IBindable bindable = viewTreeView.SelectedNode.Tag as IBindable;

            if (bindable != null)
            {
                if (RenderView.ResponseMap != null)
                {
                    mapComponentMenuItem.Enabled = true;
                    AddPropertiesToMenu(bindable.MappedProperty);
                }

                if (bindable.MappedProperty != null)
                {
                    if (bindable.MappedProperty.CanChangeDisplayFormat() &&
                        viewTreeView.SelectedNode.Tag.GetType().GetProperty("DisplayFormat") != null)
                    {
                        changeDisplayFormatToolStripMenuItem.Visible = true;
                        changeDisplayFormatToolStripMenuItem.Enabled = true;
                    }

                    if (viewTreeView.SelectedNode.Tag.GetType().GetProperty("DefaultValue") != null)
                    {
                        changeDefaultValueToolStripMenuItem.Visible = true;
                        changeDefaultValueToolStripMenuItem.Enabled = this.IsEditable;
                    }
                }
            }

            UXComponent component = viewTreeView.SelectedNode.Tag as UXComponent;

            if (component != null)
            {
                editRuleSetItem.Enabled = true;
                assignHintToolStripMenuItem.Enabled = this.IsEditable;
                changeComponentTypeMenuItem.Enabled = (component.GetType().GetCustomAttributes(typeof(VisualDesignerAttribute), false).Count() > 0);

                if (!(component is UXContainer))
                {
                    TreeNode current = FindGroupBoxNode(viewTreeView.SelectedNode);

                    PopulateMoveToGroupboxes(current);

                    moveToGroupboxToolStripMenuItem.Visible = true;
                }
            }
                        
            if (viewTreeView.SelectedNode.Tag is UXDataGridColumn || 
                viewTreeView.SelectedNode.Tag is UXDataGrid)
            {
                gridColumnToolStripMenuItem.Visible = true;
            }
            if (viewTreeView.SelectedNode.Tag is UXWrapPanel || 
                viewTreeView.SelectedNode.Tag is UXStackPanel || 
                viewTreeView.SelectedNode.Tag is UXExpander)            
            {
                expanderToolStripMenuItem.Visible = true;
            }
            if (viewTreeView.SelectedNode.Tag is UXWrapPanel || 
                viewTreeView.SelectedNode.Tag is UXStackPanel || 
                viewTreeView.SelectedNode.Tag is UXExpander || 
                viewTreeView.SelectedNode.Tag is UXGroupBox)
            {
                groupboxToolStripMenuItem.Visible = true;
            }
        }

        private TreeNode FindGroupBoxNode(TreeNode current)
        {
            do
            {
                current = current.Parent;

                if (current != null && current.Tag is UXGroupBox)
                    break;
            }
            while (current != null);

            return current;
        }

        private IList<TreeNode> FindAllGroupBoxNodes(TreeNode currentNode)
        {
            List<TreeNode> groupBoxNodes = new List<TreeNode>();

            if (currentNode.Tag != null &&
                currentNode.Tag is UXGroupBox)
            {
                groupBoxNodes.Add(currentNode);
            }
            else
            {
                if (currentNode.Nodes.Count > 0)
                {
                    foreach (TreeNode childNode in currentNode.Nodes)
                    {
                        groupBoxNodes.AddRange(FindAllGroupBoxNodes(childNode));
                    }
                }
            }

            return groupBoxNodes;
        }

        private void PopulateMoveToGroupboxes(TreeNode ignoreGroupBoxNode)
        {
            moveToGroupboxToolStripMenuItem.DropDownItems.Clear();

            // Get all GroupBox treenodes
            IList<TreeNode> groupBoxNodes = FindAllGroupBoxNodes(TopNode);

            foreach (TreeNode node in groupBoxNodes)
            {
                if (node.Tag != null && 
                    node.Tag is UXGroupBox)
                {
                    UXGroupBox groupBox = node.Tag as UXGroupBox;

                    ToolStripItem menuItem = moveToGroupboxToolStripMenuItem.DropDownItems.Add(groupBox.Caption);
                    menuItem.Tag = node; // Set the tag as the TreeNode for the UXGroupBox
                    menuItem.Click += new EventHandler(moveToGroupBoxMenuItem_Click);

                    // Grey out the ignore groupbox
                    if (ignoreGroupBoxNode != null &&
                        ignoreGroupBoxNode.Tag != null &&
                        ignoreGroupBoxNode.Tag is UXGroupBox &&
                        node.Tag.Equals(ignoreGroupBoxNode.Tag))
                    {
                        menuItem.Enabled = false;
                    }
                    else
                    {
                        menuItem.Enabled = this.IsEditable;
                    }
                }
            }
        }

        void moveToGroupBoxMenuItem_Click(object sender, EventArgs e)
        {
            UXComponent component = viewTreeView.SelectedNode.Tag as UXComponent;

            if (component != null &&
                !(component is UXContainer) &&
                sender is ToolStripItem &&
                ((ToolStripItem)sender).Tag is TreeNode)
            {
                // Find GroupBox from node
                TreeNode fromNode = FindGroupBoxNode(viewTreeView.SelectedNode);
                TreeNode toNode = (TreeNode)((ToolStripItem)sender).Tag;

                if (fromNode != null && 
                    fromNode.Tag is UXGroupBox &&
                    ((UXGroupBox)fromNode.Tag).Container is UXLayoutGrid &&
                    toNode.Tag is UXGroupBox &&
                    ((UXGroupBox)toNode.Tag).Container is UXLayoutGrid)
                {
                    UXLayoutGrid.MoveComponent((UXLayoutGrid)((UXGroupBox)fromNode.Tag).Container,
                                               (UXLayoutGrid)((UXGroupBox)toNode.Tag).Container,
                                               component, 
                                               true);

                    // Update both from and to groupboxes
                    UpdateNode(fromNode, true, true);
                    UpdateNode(toNode);

                    // Rewrite the xaml
                    XamlSource = GenerateXaml(renderView);
                }
            }
        }

        private void UpdateNode(TreeNode node)
        {
            UpdateNode(node, false, false);
        }

        private void UpdateNode(TreeNode node, bool doExpand)
        {
            UpdateNode(node, doExpand, false);
        }

        private void UpdateNode(TreeNode node, bool doExpand, bool doSelectNode)
        {
            if (node != null)
            {
                TreeNode parentNode = node.Parent;
                int index = node.Index;

                parentNode.Nodes[index].Remove();

                // Add the nodes again
                TreeNode newGroupBoxNode = AddNode(node.Tag as UXComponent, parentNode, index);

                if (doExpand)
                {
                    newGroupBoxNode.ExpandAll();
                }

                if (doSelectNode)
                    viewTreeView.SelectedNode = newGroupBoxNode;
            }
        }

        Dictionary<UXLabel, UXComponent> labelDictionary = new Dictionary<UXLabel, UXComponent>();
        Dictionary<UXComponent, UXLabel> componentLabelDictionary = new Dictionary<UXComponent, UXLabel>();
        Dictionary<UXComponent, IBindable> componentMasterDictionary = new Dictionary<UXComponent, IBindable>();

        private void AddGroupBoxLabelsToMenu(UXComponent currentComponent)
        {
            //UXContainer container = currentComponent.Parent;
            labelConnectMenuItem.DropDownItems.Clear();

            UXContainer container = currentComponent.Parent;
            if (!(container is UXLayoutGrid))
                container = container.Parent;

            UXLayoutGrid layoutGrid = container as UXLayoutGrid;

            if (layoutGrid != null)
            {
                IEnumerable<UXComponent> sortedComponents =
                    from UXLayoutGridCell cell in layoutGrid.Cells
                    orderby cell.Row, cell.Column
                    select cell.Component;

                foreach (UXComponent comp in sortedComponents)
                {
                    if (comp is UXLabel)
                    {
                        UXLabel label = comp as UXLabel;
                        string menuString = string.Format("{0} ({1})", label.Caption, label.Name);

                        if (labelDictionary.ContainsKey(label))
                        {
                            menuString = string.Format("{0} -> {1} ({2})", menuString, GetMappedPropertyName(labelDictionary[label] as IBindable), labelDictionary[label].Name);
                        }

                        ToolStripItem ti = labelConnectMenuItem.DropDownItems.Add(menuString);
                        ti.Tag = comp;
                        ti.Click += new EventHandler(ConnectToLabelClick);
                        ti.Enabled = this.IsEditable;
                    }
                }
            }
        }

        private void ConnectToLabelClick(object sender, EventArgs e)
        {
            UXComponent comp = viewTreeView.SelectedNode.Tag as UXComponent;
            ToolStripItem ti = sender as ToolStripItem;
            if (componentLabelDictionary.ContainsKey(comp))
            {
                UXLabel oldLabel = componentLabelDictionary[comp];
                componentLabelDictionary.Remove(comp);
                labelDictionary.Remove(oldLabel);
            }

            UXLabel newLabel = ti.Tag as UXLabel;


            labelDictionary[newLabel] = comp;
            componentLabelDictionary[comp] = newLabel;
        }

        void ConnectToMasterClick(object sender, EventArgs e)
        {
            UXServiceComponent serviceComponent = viewTreeView.SelectedNode.Tag as UXServiceComponent;
            serviceComponent.MasterComponent = (sender as ToolStripItem).Tag as UXServiceComponent;
        }

        private void AddPropertiesToMenu(MappedProperty currentProperty)
        {
            mapComponentMenuItem.DropDownItems.Clear();

            if (RenderView.ResponseMap != null)
            {
                ToolStripMenuItem clearMappingItem = new ToolStripMenuItem("(Clear Mapping)");
                clearMappingItem.Click += ClearMappingMenuItemClick;
                clearMappingItem.Enabled = IsEditable;
                mapComponentMenuItem.DropDownItems.Add(clearMappingItem);

                ToolStripMenuItem mainSourceItem = new ToolStripMenuItem("Main Source");
                mapComponentMenuItem.DropDownItems.Add(mainSourceItem);

                foreach (MappedProperty property in this.RenderView.ResponseMap.MappedProperties.OrderBy(property => property.Name))
                {
                    DbProperty origin = MetaManagerServices.Helpers.MappedPropertyHelper.GetOrigin(property);

                    ToolStripMenuItem item = new ToolStripMenuItem(string.Format("{0}{1}{2}", 
                                                                   property.Name, 
                                                                   property.Type != null ? string.Format(" ({0})", property.Type.Name) : string.Empty,
                                                                   origin != null ? string.Format(" [{0}]", origin.Name) : string.Empty));
                    mainSourceItem.DropDownItems.Add(item);
                    item.Tag = new PropertyDataSourceTag(property);
                    item.Click += MapComponentClick;
                    item.Checked = property == currentProperty;
                    item.Enabled = this.IsEditable;
                }

                foreach (DataSource dataSource in RenderView.DataSources)
                {
                    ToolStripMenuItem dataSourceItem = new ToolStripMenuItem(string.Format("DataSource - {0}", dataSource.Name));
                    mapComponentMenuItem.DropDownItems.Add(dataSourceItem);

                    foreach (MappedProperty property in dataSource.ServiceMethod.ResponseMap.MappedProperties.OrderBy(property => property.Name))
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(string.Format("{0}{1}", property.Name, property.Type != null ? string.Format(" ({0})", property.Type.Name) : string.Empty));
                        dataSourceItem.DropDownItems.Add(item);
                        item.Tag = new PropertyDataSourceTag(property, dataSource);
                        item.Click += MapComponentClick;
                        item.Checked = property == currentProperty;
                        item.Enabled = this.IsEditable;
                    }
                }
            }
        }

        void ClearMappingMenuItemClick(object sender, EventArgs e)
        {
            IBindable bindable = viewTreeView.SelectedNode.Tag as IBindable;

            if (bindable != null)
            {
                bindable.MappedProperty = null;
                bindable.DataSource = null;
            }

            propertyGrid.SelectedObject = bindable;
        }

        void MapComponentClick(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;

            PropertyDataSourceTag tag = item.Tag as PropertyDataSourceTag;

            IBindable bindable = viewTreeView.SelectedNode.Tag as IBindable;

            bindable.MappedProperty = tag.MappedProperty;
            bindable.DataSource = tag.DataSource;

            propertyGrid.SelectedObject = bindable;
        }

        private void changeComponentTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UXComponent component = viewTreeView.SelectedNode.Tag as UXComponent;

            if (component == null)
                return;

            if (component is UXTwoWayListBox)
            {
                UXTwoWayListBox listBox = component as UXTwoWayListBox;

                using (ConfigureTwoWayListBoxForm form = new ConfigureTwoWayListBoxForm())
                {
                    form.BackendApplication = BackendApplication;
                    form.FrontendApplication = FrontendApplication;
                    form.Owner = this;
                    form.ListBox = listBox;
                    form.View = RenderView;
                    form.IsEditable = this.IsEditable;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        XamlSource = GenerateXaml(renderView);
                        UpdateNode(FindGroupBoxNode(viewTreeView.SelectedNode), true, true);
                    }
                }
            }
            else if (component is UXComboDialog)
            {
                UXComboDialog combo = component as UXComboDialog;

                using (ConfigureComboDialogForm form = new ConfigureComboDialogForm())
                {
                    form.BackendApplication = BackendApplication;
                    form.FrontendApplication = FrontendApplication;
                    form.Owner = this;
                    form.ComboDialog = combo;
                    form.View = RenderView;
                    form.IsEditable = this.IsEditable;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        XamlSource = GenerateXaml(renderView);
                        UpdateNode(FindGroupBoxNode(viewTreeView.SelectedNode), true, true);
                    }
                }
            }
            else if (component is UXHyperDialog)
            {
                UXHyperDialog hyper = component as UXHyperDialog;

                using (ConfigureHyperDialogForm form = new ConfigureHyperDialogForm())
                {
                    form.BackendApplication = BackendApplication;
                    form.FrontendApplication = FrontendApplication;
                    form.Owner = this;
                    form.HyperDialog = hyper;
                    form.View = RenderView;
                    form.IsEditable = this.IsEditable;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        XamlSource = GenerateXaml(renderView);
                        UpdateNode(FindGroupBoxNode(viewTreeView.SelectedNode), true, true);
                    }
                }
            }
            else
            {
                using (ConfigureComponentForm form = new ConfigureComponentForm())
                {
                    var componentNames = (from c in ViewHelper.GetAllComponents<UXComponent>(RenderView.VisualTree)
                                          select c.Name).ToList();

                    form.Owner = this;
                    form.BackendApplication = BackendApplication;
                    form.UXComponent = component;
                    form.OnlyBindables = true;
                    form.ComponentNames = componentNames;
                    form.IsEditable = this.IsEditable;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        if (form.UXComponent != component)
                        {
                            ViewHelper.ReplaceComponentInVisualTree(component, form.UXComponent);
                            viewTreeView.SelectedNode.Tag = form.UXComponent;
                            XamlSource = GenerateXaml(renderView);
                            UpdateNode(viewTreeView.SelectedNode, true, true);
                        }
                    }
                }
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                IList<UXComponent> newComponentList = ViewHelper.GetAllComponents<UXComponent>(RenderView.VisualTree);
                                
                MetaManagerServices.Helpers.ViewHelper.SetHints(RenderView);

                this.RenderView = (Cdc.MetaManager.DataAccess.Domain.View)modelService.SaveDomainObject(this.RenderView);

                foreach (UXComponent deletedComponent in componentList.Except(newComponentList))
                {
                    dialogService.DeleteUXComponent(deletedComponent);
                }

                // Remove the datasources from the views datasourcelist
                RenderView.DataSources.Clear();
                Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            renderView.VisualTree = null;
            renderView.VisualTreeXml = VisualTreeXml;

            Close();
        }
                        
        private void fieldConnectMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void componentMapMenuItem_Click(object sender, EventArgs e)
        {
            UXServiceComponent serviceComponent = viewTreeView.SelectedNode.Tag as UXServiceComponent;

            PropertyMap componentMap = null;
            IList<IMappableProperty> sourceProperties = null;
            IList<IMappableProperty> targetProperties = null;

            dialogService.GetServiceComponentMap(renderView, serviceComponent, out componentMap, out sourceProperties, out targetProperties);

            using (PropertyMapForm2 form = new PropertyMapForm2())
            {
                form.PropertyMap = componentMap;
                form.SourceProperties = sourceProperties;
                form.TargetProperties = targetProperties;
                form.AllowNonUniquePropertyNames = true;
                form.IsEditable = this.IsEditable;

                form.Owner = this;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    serviceComponent.ComponentMap = form.PropertyMap;

                    if (serviceComponent.ComponentMap != null)
                        serviceComponent.ComponentMap = applicationService.SaveAndMergePropertyMap(serviceComponent.ComponentMap);
                }
            }
        }

        private void editRuleSetItem_Click(object sender, EventArgs e)
        {
            UXComponent component = viewTreeView.SelectedNode.Tag as UXComponent;

            if (component != null)
            {
                if (component.Name == UXComponent.DefaultName)
                {
                    System.Windows.Forms.MessageBox.Show("You have to name the component before you can apply any rules.");
                    return;
                }

                Type context = null;

                IBindable bindable = viewTreeView.SelectedNode.Tag as IBindable;

                if (bindable != null)
                {
                    if (bindable.MappedProperty == null)
                    {
                        System.Windows.Forms.MessageBox.Show("You must map the component to a property before you can apply any rules.");
                        return;
                    }

                    context = RuleContextFactory.CreateBindableComponentContext(this.RenderView, bindable.MappedProperty);
                }
                else
                {
                    context = RuleContextFactory.CreateComponentContext(this.RenderView);
                }

                using (RuleSetDialog ruleSetDialog = new RuleSetDialog(context, null, component.RuleSetWrapper.RuleSet))
                {
                    ruleSetDialog.Text += string.Format(" [{0}]", component.Name);


                    if (ruleSetDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (this.IsEditable)
                        {
                            if (ruleSetDialog.RuleSet.Rules.Count == 0)
                                component.RuleSetWrapper.RuleSet = null;
                            else
                                component.RuleSetWrapper.RuleSet = ruleSetDialog.RuleSet;
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Your changes will not be saved since your view is not locked for editing. Check out the view and try again");
                            return;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UnMap(RenderView.VisualTree);
        }

        private void EnableDisableDatasourceButtons()
        {
            tsbDSAdd.Enabled = this.IsEditable;
            tsbDSDelete.Enabled = false;
            tsbDSEdit.Enabled = false;
            tsbDSEditRequestMap.Enabled = false;
            tsbDSEditResponseMap.Enabled = false;

            
            if (SelectedDataSource != null)
            {
                tsbDSDelete.Enabled = this.IsEditable;
                tsbDSEdit.Enabled = this.IsEditable;
                tsbDSEditRequestMap.Enabled = true;
                tsbDSEditResponseMap.Enabled = true;
            }
        }

        private void tsbDSEditRequestMap_Click(object sender, EventArgs e)
        {
            DataSource editDataSource = SelectedDataSource;

            if (editDataSource != null && editDataSource.RequestMap != null)
            {
                using (PropertyMapForm2 form = new PropertyMapForm2())
                {
                    form.Owner = this;

                    PropertyMap requestMap = null;
                    IList<IMappableProperty> sourceProperties = null;
                    IList<IMappableProperty> targetProperties = null;

                    dialogService.GetDataSourceToViewRequestMap(editDataSource, RenderView, out requestMap, out sourceProperties, out targetProperties);

                    if (requestMap != null)
                    {
                        form.PropertyMap = requestMap;
                        form.SourceProperties = sourceProperties;
                        form.TargetProperties = targetProperties;
                        form.AllowNonUniquePropertyNames = true;
                        form.IsEditable = this.IsEditable;

                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                int index = RenderView.DataSources.IndexOf(editDataSource);

                                DataSource updatedDS = dialogService.SaveOrUpdateDataSourceMaps(editDataSource, form.PropertyMap, null);

                                updatedDS = dialogService.GetDataSourceById(updatedDS.Id);

                                SelectedDataSource = updatedDS;

                                RenderView.DataSources[index] = updatedDS;
                            }
                            catch (Exception ex)
                            {
                                System.Windows.Forms.MessageBox.Show(ex.ToString());
                            }
                        }
                    }
                }
            }
        }

        private void tsbDSEditResponseMap_Click(object sender, EventArgs e)
        {
            DataSource editDataSource = SelectedDataSource;

            if (editDataSource != null && editDataSource.RequestMap != null)
            {
                using (PropertyMapForm2 form = new PropertyMapForm2())
                {
                    form.Owner = this;

                    PropertyMap responseMap = null;
                    IList<IMappableProperty> sourceProperties = null;
                    IList<IMappableProperty> targetProperties = null;

                    dialogService.GetDataSourceToViewResponseMap(editDataSource, RenderView, out responseMap, out sourceProperties, out targetProperties);

                    if (responseMap != null)
                    {
                        form.PropertyMap = responseMap;
                        form.SourceProperties = sourceProperties;
                        form.TargetProperties = targetProperties;
                        form.EnablePropertiesByDefault = false;
                        form.AllowNonUniquePropertyNames = true;
                        form.IsEditable = this.IsEditable;

                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                int index = RenderView.DataSources.IndexOf(editDataSource);

                                DataSource updatedDS = dialogService.SaveOrUpdateDataSourceMaps(editDataSource, null, form.PropertyMap);

                                updatedDS = dialogService.GetDataSourceById(updatedDS.Id);

                                SelectedDataSource = updatedDS;

                                RenderView.DataSources[index] = updatedDS;
                            }
                            catch (Exception ex)
                            {
                                System.Windows.Forms.MessageBox.Show(ex.ToString());
                            }
                        }
                    }
                }
            }
        }

        private void tsbDSAdd_Click(object sender, EventArgs e)
        {
            using (AddDataSource form = new AddDataSource())
            {
                form.Owner = this;
                form.View = RenderView;
                form.FrontendApplication = FrontendApplication;
                form.BackendApplication = BackendApplication;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Read Datasource from database
                    DataSource dataSource = dialogService.GetDataSourceById(form.DataSource.Id);

                    // Add the newly created DataSource to the View
                    RenderView.DataSources.Add(dataSource);

                    // Update the Datasources listview
                    PopulateDataSources();
                }
            }
        }

        private void PopulateDataSources()
        {
            // Clear items in list
            lvDatasources.Items.Clear();

            if (RenderView.DataSources.Count > 0)
            {
                foreach (DataSource dataSource in RenderView.DataSources)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = dataSource.Id.ToString();
                    item.SubItems.Add(dataSource.Name);
                    item.SubItems.Add(string.Format("({0}) {1}", dataSource.ServiceMethod.Id.ToString(), dataSource.ServiceMethod.Name));
                    item.Tag = dataSource;

                    lvDatasources.Items.Add(item);
                }
            }

            EnableDisableDatasourceButtons();
        }

        private void tsbDSEdit_Click(object sender, EventArgs e)
        {
            if (SelectedDataSource != null)
            {
                using (AddDataSource form = new AddDataSource())
                {
                    form.Owner = this;
                    form.DataSource = SelectedDataSource;
                    form.View = RenderView;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // Update the selected datasource in the list
                        lvDatasources.SelectedItems[0].SubItems[1].Text = form.DataSource.Name;

                        propertyGrid.Refresh();
                    }
                }
            }
        }

        private void tsbDSDelete_Click(object sender, EventArgs e)
        {
            if (SelectedDataSource != null)
            {
                if (System.Windows.Forms.MessageBox.Show("Do you want to delete the DataSource?\n" +
                                                         "All mapped components to this DataSource will be unmapped.\n\n" +
                                                         "You can NOT cancel this operation afterwards!",
                                                         "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    // Unmap all components connected to the datasource
                    UnMap(RenderView.VisualTree, SelectedDataSource);

                    // Delete the datasource
                    RenderView.DataSources.Remove(SelectedDataSource);
                    modelService.DeleteDomainObject(SelectedDataSource);

                    // Save view
                    modelService.SaveDomainObject(RenderView);

                    // Repopulate datasources list
                    PopulateDataSources();
                }
            }
        }

        private void lvDatasources_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            EnableDisableDatasourceButtons();
        }

        private void UnMap(UXComponent component)
        {
            UnMap(component, null);
        }

        private void UnMap(UXComponent component, DataSource dataSource)
        {
            IBindable bindable = component as IBindable;

            if (bindable != null)
            {
                if (dataSource == null ||
                    (bindable.DataSource != null && bindable.DataSource == dataSource))
                {
                    bindable.DataSource = null;
                    bindable.MappedProperty = null;
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
                    UnMap(child, dataSource);
            }
        }
                        
        private void addExpanderItem_Click(object sender, EventArgs e)
        {
            // Add expander
            // If first expander add all group boxes to it
            // If not first add it at the end of the topnode.children (use moveto)
            UXContainer topContainer = TopNode.Tag as UXContainer;

            bool foundExpander = topContainer.Children.Where(child => child is UXExpander).Count() > 0;

            UXExpander expander = new UXExpander();
            expander.Caption = "[Change this]";
            
            var componentNames = (from c in ViewHelper.GetAllComponents<UXComponent>(RenderView.VisualTree)
                                  select c.Name).ToList();

            expander.Name = ViewHelper.GetDefaultComponentName(componentNames, typeof(UXExpander));
                        
            if (!foundExpander)
            {
                if (topContainer is UXWrapPanel)
                {
                    topContainer = ReplaceTopComponent(new UXStackPanel("LayoutRoot") { Orientation = UXPanelOrientation.Vertical });
                }

                foreach (UXComponent c in topContainer.Children)
                {
                    c.Parent = expander;
                    expander.Children.Add(c);
                }

                topContainer.Children.Clear();

                topContainer.Children.Add(expander);
            }
            else
            {
                topContainer.Children.Add(expander);
            }
                        
            Render();
        }

        private UXContainer ReplaceTopComponent(UXContainer newTop)
        {
            foreach (UXComponent child in renderView.VisualTree.Children)
            {
                newTop.Children.Add(child);
            }

            renderView.VisualTree.Children.Clear();
            renderView.VisualTree = newTop;
            return newTop;
        }

        private void removeExpanderItem_Click(object sender, EventArgs e)
        {
            // Remove expander
            // Move contained groupboxes to expander above
            // if none above use below
            // if none available use the root node
            // Add expander
            // If first expander add all group boxes to it
            // If not first add it at the end of the topnode.children (use moveto)
            UXContainer topContainer = TopNode.Tag as UXContainer;
            UXExpander removeExpander = viewTreeView.SelectedNode.Tag as UXExpander;
            UXContainer targetContainer = null;

            bool foundExpander = topContainer.Children.Where(child => child is UXExpander && child != removeExpander).Count() > 0;


            if (!foundExpander)
            {
                targetContainer = removeExpander.Parent;
            }
            else
            {
                int removeIndex = removeExpander.Parent.Children.IndexOf(removeExpander);

                if (removeIndex > 0)
                {
                    // Another expander above
                    targetContainer = removeExpander.Parent.Children[removeIndex - 1] as UXContainer;
                }
                else
                {
                    // Another expander below
                    targetContainer = removeExpander.Parent.Children[removeIndex + 1] as UXContainer;
                }

            }

            if (targetContainer != null)
            {
                if (targetContainer is UXStackPanel)
                {
                    targetContainer = ReplaceTopComponent(new UXWrapPanel("LayoutRoot"));
                }

                foreach (UXComponent c in removeExpander.Children)
                {
                    c.Parent = targetContainer;
                    targetContainer.Children.Add(c);
                }

                removeExpander.Children.Clear();
                removeExpander.Parent.Children.Remove(removeExpander);
            }


            Render();

        }

        private void groupBoxToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            moveToToolStripMenuItem.DropDownItems.Clear();
            moveToToolStripMenuItem.Enabled = false;

            UXContainer topContainer = TopNode.Tag as UXContainer;
            bool foundExpander = topContainer.Children.Where(child => child is UXExpander).Count() > 0;

            if (foundExpander)
            {
                if (viewTreeView.SelectedNode.Tag is UXGroupBox)
                {
                    UXGroupBox box = viewTreeView.SelectedNode.Tag as UXGroupBox;

                    moveToToolStripMenuItem.Enabled = this.IsEditable;

                    foreach (UXComponent child in topContainer.Children)
                    {
                        UXExpander exp = child as UXExpander;

                        if (exp != box.Parent)
                        {
                            string menuString = string.Format("{0} ({1})", exp.Caption, exp.Name);

                            ToolStripItem ti = moveToToolStripMenuItem.DropDownItems.Add(menuString);
                            ti.Tag = exp;
                            ti.Click += new EventHandler(MoveToItem_Click);
                            ti.Enabled = this.IsEditable;
                        }
                    }
                }
            }

        }

        private void MoveToItem_Click(object sender, EventArgs e)
        {
            // Move the groupbox
            UXGroupBox box = viewTreeView.SelectedNode.Tag as UXGroupBox;

            box.Parent.Children.Remove(box);
            UXExpander newExpander = (sender as ToolStripItem).Tag as UXExpander;

            box.Parent = newExpander;
            newExpander.Children.Add(box);

            Render();
        }

        private void addGroupboxItem_Click(object sender, EventArgs e)
        {
            UXGroupBox groupBox = new UXGroupBox();

            UXLayoutGrid grid = new UXLayoutGrid();
            groupBox.Container = grid;
            groupBox.Caption = "[Change this]";

            var componentNames = (from c in ViewHelper.GetAllComponents<UXComponent>(RenderView.VisualTree)
                                  select c.Name).ToList();

            groupBox.Name = ViewHelper.GetDefaultComponentName(componentNames, typeof(UXGroupBox));

            UXContainer addContainer = viewTreeView.SelectedNode.Tag as UXContainer;
            addContainer.Children.Add(groupBox);

            Render();
        }


        private void removeGroupboxItem_Click(object sender, EventArgs e)
        {
            UXGroupBox groupBox = (viewTreeView.SelectedNode.Tag as UXGroupBox);

            if (groupBox.Container.Children.Count > 0)
            {
                System.Windows.Forms.MessageBox.Show("You may not remove the groupbox until it's empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            groupBox.Parent.Children.Remove(groupBox);
            Render();
        }
                
        private void editXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (EditXML form = new EditXML())
            {
                form.View = RenderView;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    xamlSource = GenerateXaml(renderView);

                    RenderXaml(xamlSource);

                    BuildTreeView(RenderView.VisualTree);

                    PopulateDataSources();

                    EnableDisableToolstripButtons();
                }
            }
        }

        private void changeDisplayFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IBindable bindable = viewTreeView.SelectedNode.Tag as IBindable;

            using (SelectDisplayFormats form = new SelectDisplayFormats())
            {
                form.DisplayFormatDataType = bindable.MappedProperty.Type;
                form.DisplayFormat = bindable.MappedProperty.DisplayFormat;
                form.IsEditable = this.IsEditable;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    bindable.MappedProperty.DisplayFormat = form.DisplayFormat;

                    // Update the mapped property in database
                    modelService.SaveDomainObject(bindable.MappedProperty);

                    propertyGrid.Refresh();
                }
            }
        }

        private void changeDefaultValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IBindable bindable = viewTreeView.SelectedNode.Tag as IBindable;

            using (EditDefaultValue form = new EditDefaultValue())
            {
                form.DefaultValue = bindable.MappedProperty.DefaultValue;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    bindable.MappedProperty.DefaultValue = form.DefaultValue;

                    // Update the mapped property in database
                    modelService.SaveDomainObject(bindable.MappedProperty);

                    propertyGrid.Refresh();
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UXDataGridColumn column = viewTreeView.SelectedNode.Tag as UXDataGridColumn;

            if (column != null)
            {
                // Remove the child from the parent
                column.Parent.Children.Remove(column);
                column.Parent = null;

                // Remove the node
                viewTreeView.SelectedNode.Remove();
            }
        }

        private void assignHintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UXComponent component = viewTreeView.SelectedNode.Tag as UXComponent;

            if (component == null)
                return;

            using (FindHintForm form = new FindHintForm())
            {
                form.FrontendApplication = FrontendApplication;
                form.BackendApplication = BackendApplication;
                form.IsSelect = true;

                if (component.Hint != null)
                {
                    form.hintCollection = component.Hint.HintCollection;
                }
                else
                {
                    form.hintCollection = BackendApplication.HintCollection;
                }

                form.SelectedHint = component.Hint;
                
                if (form.ShowDialog() == DialogResult.OK)
                {
                    component.Hint = form.SelectedHint;
                }
            }
        }

        private void addColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UXDataGridColumn column = new UXDataGridColumn();

            column.Caption = "[Change this]";
            column.Width = 200;

            var componentNames = (from c in ViewHelper.GetAllComponents<UXComponent>(RenderView.VisualTree)
                                  select c.Name).ToList();

            column.Name = ViewHelper.GetDefaultComponentName(componentNames, typeof(UXDataGridColumn));

            TreeNode selectedNode = viewTreeView.SelectedNode;

            UXDataGrid addDataGrid = viewTreeView.SelectedNode.Tag as UXDataGrid;
            column.Sequence = addDataGrid.Children.Count();
            addDataGrid.Children.Add(column);
            
            Render();

            SetSelectedNodeInViewTreeNode(viewTreeView, viewTreeView.Nodes, selectedNode);
        }

        private void SetSelectedNodeInViewTreeNode(TreeView tv, TreeNodeCollection tnc, TreeNode selectedNode)
        {
            foreach (TreeNode tn in tnc)
            {
                if (tn.Text == selectedNode.Text)
                {
                    tv.SelectedNode = tn;
                    tn.ExpandAll();
                    break;
                }
                if (tn.Nodes.Count > 0)
                {
                    SetSelectedNodeInViewTreeNode(tv, tn.Nodes, selectedNode);
                }
            }
        }
    }

    public class PropertyDataSourceTag
    {
        public MappedProperty MappedProperty { get; set; }
        public DataSource DataSource { get; set; }

        public PropertyDataSourceTag(MappedProperty mappedProperty) : this(mappedProperty, null) {}

        public PropertyDataSourceTag(MappedProperty mappedProperty, DataSource dataSource)
        {
            MappedProperty = mappedProperty;
            DataSource = dataSource;
        }

    }
}
