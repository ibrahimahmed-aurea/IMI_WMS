using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain = Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Context;
using Spring.Context.Support;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess;
using Spring.Data.NHibernate.Support;
using CrossHost;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using System.Workflow.Activities.Rules.Design;
using Cdc.Framework.ExtensionMethods;

using System.Reflection;
using NHibernate;



namespace Cdc.MetaManager.GUI
{
    public partial class DialogObjectViewer : MdiChildForm
    {
        private static IApplicationContext ctx;

        public Domain.Dialog Dialog { get; set; }
        public bool IgnoreViewNodes { get; set; }
        private IDialogService dialogService;
        private IApplicationService appService;
        private IUXActionService actionService;
        private IAnalyzeService analyzeService = null;
        private IModelService modelService;

        public DialogObjectViewer()
        {
            InitializeComponent();

            IgnoreViewNodes = false;

            dialogService = MetaManagerServices.GetDialogService();
            appService = MetaManagerServices.GetApplicationService();
            actionService = MetaManagerServices.GetUXActionService();
            analyzeService = MetaManagerServices.GetAnalyzeService();
            modelService = MetaManagerServices.GetModelService();

            ctx = ContextRegistry.GetContext();

            EnableDisableButtons();


        }

        private void ObjectTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            object showObject = e.Node.Tag;

            if (((IDomainObject)showObject).Id == Guid.Empty)
            {
                showObject = null;
            }
            else
            {
                TypeDescriptor.AddAttributes(showObject, new Attribute[] { new ReadOnlyAttribute(true) });
            }


            ObjectPropertyGrid.SelectedObject = showObject;

            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            // Fetch the selected object.
            object selectedObject = objectTreeView.SelectedNode == null ? null : objectTreeView.SelectedNode.Tag;

            if (selectedObject != null && FrontendApplication != null)
            {
                if (selectedObject is Domain.View)
                {
                    Domain.View selectedView = selectedObject as Domain.View;

                    connectServiceMethodBtn.Enabled = false;
                    editLayoutBtn.Enabled = false;
                    editCustomViewBtn.Enabled = false;
                    editViewBtn.Enabled = false;
                    setAsDialogInterfaceBtn.Enabled = false;
                    editSearchPanelBtn.Enabled = false;
                    editViewInterface.Enabled = false;
                    checkOutViewBtn.Enabled = false;
                    checkInViewBtn.Enabled = false;
                    checkoutSearchPanelBtn.Enabled = false;
                    undoCheckOutSearchPanelBtn.Enabled = false;
                    checkinSearchPanelBtn.Enabled = false;

                    // Check if parent ViewNode is AnnoyingNode.
                    if (objectTreeView.SelectedNode.Parent.Tag is ViewNode)
                    {
                        if (selectedView.Type == ViewType.Standard)
                        {
                            connectServiceMethodBtn.Enabled = (selectedView.IsLocked && (selectedView.LockedBy == Environment.UserName));
                            editLayoutBtn.Enabled = true;
                            editViewBtn.Enabled = (selectedView.IsLocked && (selectedView.LockedBy == Environment.UserName));
                            checkOutViewBtn.Enabled = (!selectedView.IsLocked);
                            undoCheckoutViewBtn.Enabled = (selectedView.IsLocked && (selectedView.LockedBy == Environment.UserName));
                            checkInViewBtn.Enabled = (selectedView.IsLocked && (selectedView.LockedBy == Environment.UserName));

                            if (selectedView.ServiceMethod != null)
                            {
                                editViewInterface.Enabled = true;
                            }

                            if (Dialog.InterfaceView != null)
                            {
                                if ((Dialog.Type == DialogType.Overview) || (Dialog.Type == DialogType.Find))
                                {
                                    editSearchPanelBtn.Enabled = ((selectedObject as Cdc.MetaManager.DataAccess.Domain.View).Id == Dialog.InterfaceView.Id);
                                    if (Dialog.SearchPanelView != null)
                                    {
                                        checkoutSearchPanelBtn.Enabled = (!Dialog.SearchPanelView.IsLocked);
                                        undoCheckOutSearchPanelBtn.Enabled = (Dialog.SearchPanelView.IsLocked && (Dialog.SearchPanelView.LockedBy == Environment.UserName));
                                        checkinSearchPanelBtn.Enabled = (Dialog.SearchPanelView.IsLocked && (Dialog.SearchPanelView.LockedBy == Environment.UserName));
                                    }
                                }
                                else
                                {
                                    editSearchPanelBtn.Visible = false;
                                    checkoutSearchPanelBtn.Visible = false;
                                    undoCheckOutSearchPanelBtn.Visible = false;
                                    checkinSearchPanelBtn.Visible = false;
                                    foreach (ToolStripSeparator separator in viewToolStrip1.Items.Cast<ToolStripItem>().Where(c => c is ToolStripSeparator))
                                    {
                                        separator.Visible = false;
                                    }
                                }

                                setAsDialogInterfaceBtn.Enabled = (Dialog.InterfaceView.Id != selectedView.Id) && (this.Dialog.IsLocked && (this.Dialog.LockedBy == Environment.UserName));
                            }
                            else
                            {
                                setAsDialogInterfaceBtn.Enabled = (this.Dialog.IsLocked && (this.Dialog.LockedBy == Environment.UserName));
                            }

                        }
                        else if (selectedView.Type == ViewType.Custom)
                        {
                            editCustomViewBtn.Enabled = (selectedView.IsLocked && (selectedView.LockedBy == Environment.UserName));
                            editViewInterface.Enabled = true;
                        }
                    }

                    if (!(objectTreeView.SelectedNode.Parent.Parent.Tag is ViewNode))
                    {
                        viewToolStrip1.Visible = true;
                    }
                    else
                    {
                        viewToolStrip1.Visible = false;
                    }

                    ActionstoolStrip.Visible = false;
                    actionToolStrip.Visible = false;
                    viewToolStrip2.Visible = true;
                    viewNodeToolStrip.Visible = false;
                    dialogToolStrip.Visible = false;
                }
                else if (selectedObject is Domain.ViewAction)
                {
                    ViewAction selectedViewAction = selectedObject as Domain.ViewAction;
                    TreeNode prevNode = objectTreeView.SelectedNode.PrevNode;
                    TreeNode nextNode = objectTreeView.SelectedNode.NextNode;

                    setDrilldownField.Enabled = false;
                    removeActionBtn.Enabled = false;
                    editActionMapBtn.Enabled = false;
                    editActionBtn.Enabled = false;
                    moveActionUpBtn.Enabled = false;
                    moveActionDownBtn.Enabled = false;
                    quickAddActionBtn.Enabled = false;
                    checkOutActionBtn.Enabled = false;
                    undoCheckoutActionBtn.Enabled = false;
                    checkInActionBtn.Enabled = false;
                    actionToolStrip.Visible = false;
                    ActionstoolStrip.Visible = false;


                    // Check if parents parent is a ViewNode and that it isn't AnnoyingNode.
                    if (objectTreeView.SelectedNode.Parent != null &&
                         objectTreeView.SelectedNode.Parent.Tag is DataAccess.Domain.View &&
                         objectTreeView.SelectedNode.Parent.Parent != null &&
                         objectTreeView.SelectedNode.Parent.Parent.Tag is ViewNode)
                    {
                        quickAddActionBtn.Enabled = (this.Dialog.IsLocked && (this.Dialog.LockedBy == Environment.UserName));
                        ActionstoolStrip.Visible = true;
                    }

                    if (selectedViewAction.Action != null)
                    {
                        if ((selectedViewAction.Type == ViewActionType.Drilldown) || (selectedViewAction.Type == ViewActionType.JumpTo))
                        {
                            setDrilldownField.Enabled = (this.Dialog.IsLocked && (this.Dialog.LockedBy == Environment.UserName));
                        }

                        if (selectedViewAction.Action != null)
                        {
                            removeActionBtn.Enabled = (this.Dialog.IsLocked && (this.Dialog.LockedBy == Environment.UserName));

                            if (selectedViewAction.Action.MappedToObject != null)
                                editActionMapBtn.Enabled = true;

                            // Make it not possible to edit a PlaceHolder action since it's no use.
                            if (selectedViewAction.Type != ViewActionType.PlaceHolder)
                                editActionBtn.Enabled = true;

                            checkOutActionBtn.Enabled = !selectedViewAction.Action.IsLocked;
                            undoCheckoutActionBtn.Enabled = (selectedViewAction.Action.IsLocked && (selectedViewAction.Action.LockedBy == Environment.UserName));
                            checkInActionBtn.Enabled = (selectedViewAction.Action.IsLocked && (selectedViewAction.Action.LockedBy == Environment.UserName));
                        }

                        moveActionUpBtn.Enabled = prevNode != null && editActionBtn.Enabled && (this.Dialog.IsLocked && this.Dialog.LockedBy == Environment.UserName);
                        moveActionDownBtn.Enabled = nextNode != null && editActionBtn.Enabled && (this.Dialog.IsLocked && this.Dialog.LockedBy == Environment.UserName);

                        actionToolStrip.Visible = true;
                    }

                    
                    viewToolStrip1.Visible = false;
                    viewToolStrip2.Visible = false;
                    viewNodeToolStrip.Visible = false;
                    dialogToolStrip.Visible = false;
                }
                else if (selectedObject is Domain.ViewNode)
                {
                    ViewNode selectedViewNode = selectedObject as ViewNode;
                    TreeNode prevNode = objectTreeView.SelectedNode.PrevNode;
                    TreeNode nextNode = objectTreeView.SelectedNode.NextNode;

                    moveViewNodeDownBtn.Enabled = false;
                    moveViewNodeUpBtn.Enabled = false;
                    editMapBtn.Enabled = false;
                    deleteMapBtn.Enabled = false;
                    deleteViewNodeBtn.Enabled = false;
                    addViewBtn.Enabled = false;
                    changeViewNodeTitleBtn.Enabled = false;
                    btnEditViewNodeRule.Enabled = false;

                    addViewBtn.Enabled = (this.Dialog.IsLocked && (this.Dialog.LockedBy == Environment.UserName));
                    changeViewNodeTitleBtn.Enabled = (this.Dialog.IsLocked && (this.Dialog.LockedBy == Environment.UserName));

                    ViewNode parentNode = selectedViewNode.Parent;

                    while (parentNode != null)
                    {
                        if ((parentNode.View.ResponseMap != null) && (parentNode.View.ResponseMap.MappedProperties.Count > 0))
                        {
                            btnEditViewNodeRule.Enabled = true;
                            break;
                        }

                        parentNode = parentNode.Parent;
                    }

                    if (selectedViewNode.Parent != null && selectedViewNode.Parent.Children.Count > 1)
                    {
                        if (prevNode != null && prevNode.Tag is ViewNode)
                            moveViewNodeUpBtn.Enabled = this.Dialog.IsLocked && this.Dialog.LockedBy == Environment.UserName;

                        if (nextNode != null && nextNode.Tag is ViewNode)
                            moveViewNodeDownBtn.Enabled = this.Dialog.IsLocked && this.Dialog.LockedBy == Environment.UserName;
                    }

                    if (selectedViewNode.Id != Dialog.RootViewNode.Id)
                        editMapBtn.Enabled = true;

                    if (selectedViewNode.ViewMap != null)
                        deleteMapBtn.Enabled = (this.Dialog.IsLocked && (this.Dialog.LockedBy == Environment.UserName));

                    if (selectedViewNode.Parent != null && selectedViewNode.Children.Count == 0)
                        deleteViewNodeBtn.Enabled = (this.Dialog.IsLocked && (this.Dialog.LockedBy == Environment.UserName));

                    ActionstoolStrip.Visible = false;
                    actionToolStrip.Visible = false;
                    viewToolStrip1.Visible = false;
                    viewToolStrip2.Visible = false;
                    viewNodeToolStrip.Visible = true;
                    dialogToolStrip.Visible = false;
                }
                else if (selectedObject is Domain.Dialog)
                {
                    ActionstoolStrip.Visible = false;
                    actionToolStrip.Visible = false;
                    viewToolStrip1.Visible = false;
                    viewToolStrip2.Visible = false;
                    viewNodeToolStrip.Visible = false;
                    dialogToolStrip.Visible = true;
                }
            }
            else
            {
                ActionstoolStrip.Visible = false;
                actionToolStrip.Visible = false;
                viewToolStrip1.Visible = false;
                viewToolStrip2.Visible = false;
                viewNodeToolStrip.Visible = false;
            }

        }

        private void DialogObjectViewer_Load(object sender, EventArgs e)
        {
            BuildDialogTree(true);
            ExpandViewNodes();
            editDialog.Enabled = (this.Dialog.IsLocked && (this.Dialog.LockedBy == Environment.UserName));
            EnableDisableButtons();

            this.Text += string.Format(" - {0}", Dialog.Name);

            Cursor.Current = Cursors.Default;
        }


        private void BuildDialogTreeRememberLocation(bool forceReadFromDatabase)
        {
            try
            {
                objectTreeView.BeginUpdate();

                TreeNode currNode = objectTreeView.SelectedNode;

                Stack<int> nodePath = new Stack<int>();

                while ((currNode != objectTreeView.TopNode) && (currNode != null))
                {
                    nodePath.Push(currNode.Index);
                    currNode = currNode.Parent;
                }

                BuildDialogTree(forceReadFromDatabase);

                currNode = objectTreeView.TopNode;

                while (nodePath.Count > 0)
                {
                    int idx = nodePath.Pop();
                    try
                    {
                        currNode = currNode.Nodes[idx];
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }

                objectTreeView.ExpandAll();
                objectTreeView.SelectedNode = currNode;
            }
            finally
            {
                objectTreeView.EndUpdate();
            }

        }

        private void BuildDialogTree(bool forceReadFromDatabase)
        {
            objectTreeView.Nodes.Clear();

            // Dialog identity is set. Read the dialog before showing.
            if ((forceReadFromDatabase || Dialog == null) && ContaindDomainObjectIdAndType.Key != Guid.Empty)
            {
                Dialog = dialogService.GetDialogWithViewTree(ContaindDomainObjectIdAndType.Key);
            }

            if (Dialog != null)
            {
                // Dialog
                DialogNameLabel.Text = Dialog.Name;

                TreeNode dialogTreeNode = new TreeNode();
                dialogTreeNode.Tag = Dialog;
                dialogTreeNode.Text = Dialog.Name;

                BuildViewTree(Dialog.RootViewNode, dialogTreeNode, IgnoreViewNodes);

                objectTreeView.Nodes.Add(dialogTreeNode);
            }
        }

        private void BuildViewTree(Domain.ViewNode viewNode, TreeNode parentTreeNode, bool ignoreViewNode)
        {
            TreeNode currentTreeNode = new TreeNode();

            if (!ignoreViewNode)
            {
                currentTreeNode.Tag = viewNode;

                SetViewNodeText(viewNode, currentTreeNode);

                if ((viewNode.RuleSet != null) && (viewNode.RuleSet.Rules.Count > 0))
                    currentTreeNode.ForeColor = Color.Blue;
                else
                    currentTreeNode.ForeColor = Color.Black;
            }

            if (viewNode.View != null)
            {
                if (!ignoreViewNode)
                {
                    TreeNode viewTreeNode = new TreeNode();
                    viewTreeNode.Tag = viewNode.View;

                    if (viewNode.Dialog.InterfaceView != null && (viewNode.Dialog.InterfaceView.Id == viewNode.View.Id))
                    {
                        viewTreeNode.Text = "(View) <Dialog Interface>";
                    }
                    else
                    {
                        viewTreeNode.Text = "(View)";
                    }

                    if (viewNode.View.Type == Cdc.MetaManager.DataAccess.Domain.ViewType.Custom)
                    {
                        viewTreeNode.Text = "(Custom) " + viewTreeNode.Text;
                    }

                    BuildActionNode(viewNode, viewTreeNode);
                    currentTreeNode.Nodes.Add(viewTreeNode);
                }
                else
                {
                    currentTreeNode.Tag = viewNode.View;

                    if (viewNode.Dialog.InterfaceView != null && (viewNode.Dialog.InterfaceView.Id == viewNode.View.Id))
                    {
                        currentTreeNode.Text = viewNode.View.Title + " (View) <Dialog Interface>";
                    }
                    else
                    {
                        currentTreeNode.Text = viewNode.View.Title + " (View)";
                    }

                    BuildActionNode(viewNode, currentTreeNode);
                }
            }

            if (viewNode.Children.Count > 0)
            {
                IEnumerable<ViewNode> sortedViewNodes = from ViewNode node in viewNode.Children
                                                        orderby node.Sequence
                                                        select node;

                foreach (ViewNode vChild in sortedViewNodes)
                {
                    BuildViewTree(vChild, currentTreeNode, ignoreViewNode);
                }
            }

            parentTreeNode.Nodes.Add(currentTreeNode);
        }

        private static void SetViewNodeText(Domain.ViewNode viewNode, TreeNode currentTreeNode)
        {
            if ((string.IsNullOrEmpty(viewNode.Title)) && (viewNode.View != null))
                currentTreeNode.Text = "  " + viewNode.View.Title + " (ViewNode)";
            else
                currentTreeNode.Text = "  " + viewNode.Title + " (ViewNode)";
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void viewMapBtn_Click(object sender, EventArgs e)
        {

        }

        private void BuildActionNode(Domain.ViewNode viewNode, TreeNode currentTreeNode)
        {
            TreeNode actionsNode = new TreeNode("Actions");
            Domain.ViewAction dummyAction = new Domain.ViewAction();
            dummyAction.ViewNode = viewNode;
            actionsNode.Tag = dummyAction;

            IEnumerable<Domain.ViewAction> sortedActions =
                from Domain.ViewAction va in viewNode.ViewActions
                orderby va.Sequence
                select va;

            foreach (Domain.ViewAction viewAction in sortedActions)
            {
                TreeNode actionNode = new TreeNode();
                string text = viewAction.Action.Caption;

                if (viewAction.Type == ViewActionType.PlaceHolder)
                    text += " [PlaceHolder]";

                actionNode.Text = text;
                actionNode.Tag = viewAction;
                actionsNode.Nodes.Add(actionNode);
            }

            currentTreeNode.Nodes.Add(actionsNode);
        }

        private void connectServiceMethodBtn_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Domain.View view = (Domain.View)ObjectPropertyGrid.SelectedObject;

            using (SelectServiceMethod form = new SelectServiceMethod())
            {
                form.FrontendApplication = FrontendApplication;
                form.BackendApplication = BackendApplication;
                form.ViewName = view.Name;

                bool redraw = false;

                Cursor = Cursors.WaitCursor;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    bool ok = true;

                    // Read the view
                    Domain.View readView = dialogService.GetViewById(view.Id);

                    // Check if a ServiceMethod is already set.
                    if (readView.ServiceMethod != null)
                    {
                        // Check if selected servicemethod id is different from the one already set for the View
                        if (readView.ServiceMethod.Id != form.SelectedServiceMethod.Id)
                        {
                            ok = MessageBox.Show("This View is already connected to another ServiceMethod.\n" +
                                                 "This action will make all components loose their connections to the interface properties.\n" +
                                                 "All dialogs where this View is connected to a ViewNode will stop working and the ViewNodes\n" +
                                                 "maps needs to be remapped.\n\n" +
                                                 "Do you want to continue to connect the selected ServiceMethod to the View?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                        }
                        else
                        {
                            ok = MessageBox.Show("This View is already connected to the same ServiceMethod with the same identity.\n" +
                                                 "This action will make all components loose their connections to the interface properties.\n" +
                                                 "All dialogs where this View is connected to a ViewNode will stop working and the ViewNodes\n" +
                                                 "maps needs to be remapped.\n\n" +
                                                 "Are you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                        }
                    }

                    if (ok)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        // Disconnect the current components from 
                        dialogService.UnmapViewComponents(readView);

                        // Create the request and response maps for the ServiceMethod
                        readView = MetaManagerServices.Helpers.ViewHelper.ConnectServiceMethodToView(readView.Id, form.SelectedServiceMethod.Id);

                        // Set the new selected object
                        ObjectPropertyGrid.SelectedObject = readView;

                        // Set the treeview object
                        if (objectTreeView.SelectedNode.Tag is Domain.View)
                        {
                            objectTreeView.SelectedNode.Tag = readView;
                        }

                        redraw = true;

                        this.Cursor = Cursors.Default;
                    }
                }

                if (redraw)
                {
                    BuildDialogTreeRememberLocation(true);
                }
            }

            // Update toolstrips
            EnableDisableButtons();
        }

        private void showNodesChk_CheckedChanged(object sender, EventArgs e)
        {
            IgnoreViewNodes = !showNodesChk.Checked;

            BuildDialogTree(false);
            ExpandViewNodes();
        }

        private void ExpandViewNodes()
        {
            try
            {
                objectTreeView.BeginUpdate();
                objectTreeView.TopNode.Expand();
                ExpandViewNode(objectTreeView.TopNode);
                objectTreeView.SelectedNode = objectTreeView.TopNode;
            }
            finally
            {
                objectTreeView.EndUpdate();
            }
        }

        private void ExpandViewNode(TreeNode node)
        {
            if (node.Text.ToLower().Contains("(view"))
                node.Expand();

            foreach (TreeNode child in node.Nodes)
            {
                ExpandViewNode(child);
            }
        }

        private void editMapBtn_Click(object sender, EventArgs e)
        {
            if (objectTreeView.SelectedNode != null)
            {
                Domain.ViewNode node = objectTreeView.SelectedNode.Tag as Domain.ViewNode;

                if (node != null)
                {
                    using (PropertyMapForm2 form = new PropertyMapForm2())
                    {
                        form.Owner = this;

                        Domain.PropertyMap viewNodeMap = null;
                        IList<IMappableProperty> sourceProperties = null;
                        IList<IMappableProperty> targetProperties = null;

                        dialogService.GetViewNodeMap(node, out viewNodeMap, out sourceProperties, out targetProperties);

                        form.PropertyMap = viewNodeMap;
                        form.SourceProperties = sourceProperties;
                        form.TargetProperties = targetProperties;
                        form.AllowNonUniquePropertyNames = true;
                        form.IsEditable = this.Dialog.IsLocked && this.Dialog.LockedBy == Environment.UserName;

                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                dialogService.SaveOrUpdateViewNodeMap(node, form.PropertyMap);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            finally
                            {
                                BuildDialogTreeRememberLocation(true);
                            }
                        }
                    }
                }
            }
        }

        private void removeActionBtn_Click(object sender, EventArgs e)
        {
            Domain.ViewAction viewAction = objectTreeView.SelectedNode.Tag as Domain.ViewAction;

            TreeNode node = objectTreeView.SelectedNode;

            if (MessageBox.Show("Are you sure you want to remove the selected action from this View?", "Remove Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ViewNode vn = viewAction.ViewNode;

                    Guid actionId = viewAction.Action.Id;

                    // Remove the ViewAction
                    modelService.DeleteDomainObject(viewAction);

                    // Check if the action is connected to any other views. If not it's deleted.
                    Dictionary<string, object> propsAndValues = new Dictionary<string, object>();
                    propsAndValues.Add("Action.Id", actionId);
                    IList<ViewAction> viewActionList = modelService.GetAllDomainObjectsByPropertyValues<ViewAction>(propsAndValues);

                    // Check if there are any menuitems connected to the action
                    IList<Cdc.MetaManager.DataAccess.Domain.MenuItem> menuItemList = MetaManagerServices.GetMenuService().FindAllMenuItemsByActionId(actionId);

                    if (viewActionList.Count == 0 && menuItemList.Count == 0)
                    {
                        if (viewAction.Type == ViewActionType.PlaceHolder || MessageBox.Show("This Action is not connected to any other Views, do you want to delete it?", "Remove Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            // Delete the action
                            modelService.DeleteDomainObject(viewAction.Action);
                        }
                    }

                    vn.ViewActions.Remove(viewAction);
                    node.Remove();
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

        private void editActionMapBtn_Click(object sender, EventArgs e)
        {
            Domain.ViewAction viewAction = objectTreeView.SelectedNode.Tag as Domain.ViewAction;

            if (viewAction != null)
            {
                using (PropertyMapForm2 form = new PropertyMapForm2())
                {
                    form.Owner = this;

                    Domain.PropertyMap viewToActionMap = null;
                    IList<IMappableProperty> sourceProperties = null;
                    IList<IMappableProperty> targetProperties = null;

                    dialogService.GetViewToActionMap(viewAction, out viewToActionMap, out sourceProperties, out targetProperties);

                    form.PropertyMap = viewToActionMap;
                    form.SourceProperties = sourceProperties;
                    form.TargetProperties = targetProperties;
                    form.AllowNonUniquePropertyNames = true;
                    form.IsEditable = (this.Dialog.IsLocked && (this.Dialog.LockedBy == Environment.UserName));

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            viewAction.ViewToActionMap = form.PropertyMap;
                            viewAction = (ViewAction)modelService.MergeSaveDomainObject(viewAction);
                            RebuildActionNode(objectTreeView.SelectedNode.Parent, viewAction.ViewNode);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }

        }


        private void tsbtnShowWPF_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Domain.View view = (Domain.View)ObjectPropertyGrid.SelectedObject;

            Domain.View readView = dialogService.GetViewById(view.Id);

            using (XamlViewerForm xf = new XamlViewerForm())
            {
                xf.RenderView = readView;
                xf.FrontendApplication = FrontendApplication;
                xf.BackendApplication = BackendApplication;
                xf.Dialog = Dialog;

                Cursor = Cursors.Default;
                if (xf.ShowDialog() == DialogResult.OK)
                {
                    view = xf.RenderView;
                    ObjectPropertyGrid.SelectedObject = view;
                }
            }
        }

        private void editActionBtn_Click(object sender, EventArgs e)
        {
            Domain.ViewAction vaction = objectTreeView.SelectedNode.Tag as Domain.ViewAction;
            Domain.UXAction action = vaction.Action;

            if (action != null)
            {
                using (EditUXAction editDialog = new EditUXAction())
                {
                    editDialog.FrontendApplication = FrontendApplication;
                    editDialog.BackendApplication = BackendApplication;
                    editDialog.Owner = this.MdiParent;
                    

                    editDialog.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(action.Id, typeof(UXAction));

                    DialogResult dr = editDialog.ShowDialog();

                    if (dr == DialogResult.OK)
                    {
                        vaction.Action = editDialog.getUXAction;
                        RebuildActionNode(objectTreeView.SelectedNode.Parent, vaction.ViewNode);
                    }
                }
            }
        }

        private void tsbtnSetAsDialogInterface_Click(object sender, EventArgs e)
        {
            Domain.View view = objectTreeView.SelectedNode.Tag as Domain.View;

            // Set interface and save
            dialogService.SetDialogInterface(Dialog, view);

            // Set interface loosely.
            Dialog.InterfaceView = view;

            // Rewrite tree
            BuildDialogTreeRememberLocation(false);
            EnableDisableButtons();
        }

        private void QuickAddActionClick(object sender, EventArgs e)
        {
            using (SelectUXActionType selectForm = new SelectUXActionType())
            {
                selectForm.FrontendApplication = this.FrontendApplication;
                selectForm.BackendApplication = this.BackendApplication;

                bool noDrilldown = true;

                IList<MappedProperty> drilldownFields = GetDrilldownFields(false);

                if (drilldownFields.Count > 0)
                    noDrilldown = false;

                selectForm.NoDrilldowns = noDrilldown;

                drilldownFields = GetDrilldownFields(true);

                if (selectForm.ShowDialog() == DialogResult.OK)
                {
                    UXAction action = selectForm.SelectedAction;

                    ViewActionType viewActionType = ViewActionType.Normal;

                    // Only when Drilldown/JumpTo type should ViewAction has another type
                    if (selectForm.SelectedActionType == SelectedUXActionType.DrillDown)
                        viewActionType = ViewActionType.Drilldown;

                    if (selectForm.SelectedActionType == SelectedUXActionType.JumpTo)
                        viewActionType = ViewActionType.JumpTo;

                    if (action != null)
                    {
                        LocalAddAction(action, viewActionType, drilldownFields);
                    }
                }
            }
        }

        private IList<MappedProperty> GetDrilldownFields(bool useResponseMap)
        {
            // Find out if there are any fields mapped currently to the grid in the View that this
            // Action belongs to.
            Cdc.MetaManager.DataAccess.Domain.View currentView = null;
            TreeNode currentNode = objectTreeView.SelectedNode;
            List<MappedProperty> drilldownFields = new List<MappedProperty>();

            while (currentView == null && currentNode != null)
            {
                if (currentNode.Tag is Cdc.MetaManager.DataAccess.Domain.View)
                {
                    currentView = (Cdc.MetaManager.DataAccess.Domain.View)currentNode.Tag;
                }
                else
                {
                    currentNode = currentNode.Parent;
                }
            }

            if (currentView != null)
            {
                using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
                {
                    currentView = dialogService.GetViewById(currentView.Id);

                    if (currentView.VisualTree != null)
                    {
                        // Find DockPanel with its UXDataGrid as only child
                        if (currentView.VisualTree is UXDockPanel)
                        {
                            UXComponent component = currentView.VisualTree.Children.First();

                            if (component is UXDataGrid)
                            {
                                UXDataGrid dataGrid = component as UXDataGrid;

                                foreach (UXDataGridColumn column in dataGrid.Children)
                                {
                                    if (column.MappedProperty != null)
                                    {
                                        drilldownFields.Add(column.MappedProperty);
                                    }
                                }
                            }
                        }

                        if (useResponseMap)
                        {
                            if (drilldownFields.Count() == 0)
                            {
                                drilldownFields.AddRange(currentView.ResponseMap.MappedProperties.Where(x =>
                                       x.Type == typeof(string)
                                    || x.Type == typeof(int)
                                    || x.Type == typeof(long)
                                    || x.Type == typeof(decimal)
                                    || x.Type == typeof(double)
                                  ).OrderBy(x => x.Name));
                            }
                        }
                    }
                }
            }

            return drilldownFields;
        }

        private void LocalAddAction(Domain.UXAction action, ViewActionType viewActionType, IList<MappedProperty> validDrilldownFields)
        {
            ViewAction viewAction = objectTreeView.SelectedNode.Tag as Domain.ViewAction;

            TreeNode node = objectTreeView.SelectedNode;

            if (viewAction.Action != null)
                node = node.Parent;

            ViewNode viewNode = viewAction.ViewNode;
            MappedProperty selectedMappedField = null;

            // if SelectUXActionType is a Drilldown then we need to add additional information to the ViewAction
            if ((viewActionType == ViewActionType.Drilldown) || (viewActionType == ViewActionType.JumpTo))
            {
                using (SelectMappedPropertyFromPropertyMap selectField = new SelectMappedPropertyFromPropertyMap())
                {
                    // Set the responsemap for the Dialog Actions InterfaceViews ResponseMap.
                    selectField.ValidMappedPropertyList = validDrilldownFields;

                    if (selectField.ShowDialog() == DialogResult.OK)
                    {
                        selectedMappedField = selectField.SelectedMappedProperty;
                    }
                    else
                    {
                        // User pushed Cancel when mapping field. Then don't add ViewAction to the dialog.
                        return;
                    }
                }
            }

            // Create the ViewAction
            ViewAction addedAction = actionService.AddToView(action, viewNode, viewActionType, selectedMappedField);

            // Update treeview

            TreeNode actionNode = node.Nodes.Add(action.Caption);
            actionNode.Tag = addedAction;
            objectTreeView.SelectedNode = actionNode;

            RebuildActionNode(node, addedAction.ViewNode);
        }

        private void deleteMapBtn_Click(object sender, EventArgs e)
        {
            if (objectTreeView.SelectedNode != null)
            {
                Domain.ViewNode node = objectTreeView.SelectedNode.Tag as Domain.ViewNode;

                if (node != null && (node.ViewMap != null))
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        modelService.DeleteDomainObject(node.ViewMap);
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

        private void moveUpBtnClick(object sender, EventArgs e)
        {
            TreeNode moveNode = objectTreeView.SelectedNode;
            TreeNode staticNode = moveNode.PrevNode;

            if (staticNode != null)
            {
                Movem(staticNode, moveNode);
            }
        }

        private void moveDownBtn_Click(object sender, EventArgs e)
        {
            TreeNode staticNode = objectTreeView.SelectedNode;
            TreeNode moveNode = staticNode.NextNode;

            if (moveNode != null)
            {
                Movem(staticNode, moveNode);
            }
        }

        private void Movem(TreeNode staticNode, TreeNode moveNode)
        {
            if (moveNode.Tag is ViewAction)
            {
                ViewAction moverAction = moveNode.Tag as ViewAction;
                ViewAction staticAction = staticNode.Tag as ViewAction;

                dialogService.MoveUp(moverAction.Id, staticAction.Id, out moverAction, out staticAction);

                RebuildActionNode(staticNode.Parent, staticAction.ViewNode);
            }
            else if (moveNode.Tag is ViewNode)
            {
                ViewNode moverViewNode = moveNode.Tag as ViewNode;
                ViewNode staticViewNode = staticNode.Tag as ViewNode;

                dialogService.MoveUp(moverViewNode.Id, staticViewNode.Id, out moverViewNode, out staticViewNode);

                //// Set the changed objects back as tags on the treenodes
                (moveNode.Tag as ViewNode).Sequence = moverViewNode.Sequence;
                (staticNode.Tag as ViewNode).Sequence = staticViewNode.Sequence;

                TreeNode parentTreeNode = staticNode.Parent;

                try
                {
                    parentTreeNode.TreeView.BeginUpdate();

                    // Since moverViewNode has larger sequence it should be below ths staticViewNode
                    // Fetch their current indexes to know where to insert them back.
                    int lowestIndex = parentTreeNode.Nodes.IndexOf(moveNode);

                    if (lowestIndex > parentTreeNode.Nodes.IndexOf(staticNode))
                        lowestIndex = parentTreeNode.Nodes.IndexOf(staticNode);

                    // Remove the nodes
                    parentTreeNode.Nodes.Remove(moveNode);
                    parentTreeNode.Nodes.Remove(staticNode);

                    // Insert them back
                    if (moverViewNode.Sequence > staticViewNode.Sequence)
                    {
                        parentTreeNode.Nodes.Insert(lowestIndex, moveNode);
                        parentTreeNode.Nodes.Insert(lowestIndex, staticNode);
                    }
                    else
                    {
                        parentTreeNode.Nodes.Insert(lowestIndex, staticNode);
                        parentTreeNode.Nodes.Insert(lowestIndex, moveNode);
                    }


                }
                finally
                {
                    parentTreeNode.TreeView.EndUpdate();
                }
            }
        }


        private void RebuildActionNode(TreeNode actionNode, Domain.ViewNode viewNode)
        {
            TreeNode actionParentNode = actionNode.Parent;
            TreeView treeView = actionParentNode.TreeView;
            Domain.ViewAction selectedViewAction = treeView.SelectedNode.Tag as Domain.ViewAction;

            try
            {
                treeView.BeginUpdate();

                actionNode.Remove();

                viewNode = dialogService.GetViewNodeById(viewNode.Id);

                BuildActionNode(viewNode, actionParentNode);

                actionParentNode.Nodes[0].ExpandAll();

                IEnumerable<TreeNode> choosenChild =
                    from TreeNode t in actionParentNode.Nodes[0].Nodes
                    where (t.Tag as Domain.ViewAction).Id == selectedViewAction.Id
                    select t;

                if (choosenChild.Count() > 0)
                    actionParentNode.TreeView.SelectedNode = choosenChild.First();
                else
                    actionParentNode.TreeView.SelectedNode = actionParentNode.Nodes[0].Nodes[0];
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        private void tsbtnEditCustomView_Click(object sender, EventArgs e)
        {
            if (objectTreeView.SelectedNode != null)
            {
                Domain.View view = objectTreeView.SelectedNode.Tag as Domain.View;

                if (view != null)
                {
                    using (CreateCustomView customView = new CreateCustomView())
                    {
                        customView.FrontendApplication = FrontendApplication;
                        customView.BackendApplication = BackendApplication;
                        customView.EditView = view;

                        if (customView.ShowDialog() == DialogResult.OK)
                        {
                            // Update the ViewNode for this View since the title may have changed
                            BuildDialogTreeRememberLocation(true);
                        }
                    }
                }
            }
        }

        private void removeCustomView_Click(object sender, EventArgs e)
        {
            if (objectTreeView.SelectedNode != null)
            {
                Domain.ViewNode viewNode = objectTreeView.SelectedNode.Tag as Domain.ViewNode;

                if (viewNode != null)
                {
                    if (MessageBox.Show("Are you sure you want to remove the currently selected ViewNode?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;

                            // Get the view id
                            Guid viewId = viewNode.View.Id;

                            // Disconnect the View and delete ViewNode

                            modelService.DeleteDomainObject(viewNode);

                            // Check if View that was disconnected from the ViewNode now isn't connected to any ViewNodes.
                            IList<ViewNode> viewNodeList = dialogService.GetViewNodesByViewId(viewId);

                            if (viewNodeList != null && viewNodeList.Count == 0)
                            {
                                // Check if the View should be deleted also
                                if (MessageBox.Show("The View that was disconnected is not connected to any Dialogs, do you want to delete it?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    modelService.DeleteDomainObject(viewNode.View);
                                }
                            }
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
                        finally
                        {
                            BuildDialogTree(true);
                            ExpandViewNodes();
                            EnableDisableButtons();
                        }
                    }
                }
            }
        }



        private void btnCheckDialog_Click(object sender, EventArgs e)
        {

            ShowIssueList showIssueForm = new ShowIssueList(analyzeService.CheckSingleDialog(FrontendApplication.Id, Dialog.Id));

            showIssueForm.Show();
        }

        private void editSearchPanelBtn_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {

                Dialog = dialogService.CreateOrUpdateSearchPanelView(Dialog);

            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(ex.ToString());
                return;
            }

            if (Dialog.SearchPanelView == null)
            {
                Cursor = Cursors.Default;
                return;
            }

            EnableDisableButtons();

            Domain.View readView = dialogService.GetViewById(Dialog.SearchPanelView.Id);


            using (XamlViewerForm xf = new XamlViewerForm())
            {
                xf.RenderView = readView;
                xf.FrontendApplication = FrontendApplication;
                xf.BackendApplication = BackendApplication;
                xf.IsEditable = readView.IsLocked && readView.LockedBy == Environment.UserName;

                Cursor = Cursors.Default;
                if (xf.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        modelService.SaveDomainObject(readView);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void setViewNodeTitle_Click(object sender, EventArgs e)
        {
            using (EditViewNodeTitle form = new EditViewNodeTitle())
            {
                form.ViewNode = objectTreeView.SelectedNode.Tag as Domain.ViewNode;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Update the ViewNode since it has been changed
                    objectTreeView.SelectedNode.Tag = form.ViewNode;
                    ObjectPropertyGrid.SelectedObject = objectTreeView.SelectedNode.Tag;

                    SetViewNodeText(form.ViewNode, objectTreeView.SelectedNode);
                }
            }
        }

        private void editDialog_Click(object sender, EventArgs e)
        {
            using (EditDialog editDialog = new EditDialog())
            {
                editDialog.Owner = this;
                editDialog.FrontendApplication = FrontendApplication;
                editDialog.Dialog = objectTreeView.SelectedNode.Tag as Domain.Dialog;

                if (editDialog.ShowDialog() == DialogResult.OK)
                {
                    // Update the dialog since it has been changed
                    objectTreeView.SelectedNode.Tag = editDialog.Dialog;
                    ObjectPropertyGrid.SelectedObject = objectTreeView.SelectedNode.Tag;

                    // Change the name of the selected node in tree
                    objectTreeView.SelectedNode.Text = editDialog.Dialog.Name;
                    Dialog = editDialog.Dialog;
                }
            }
        }

        private void addView_Click(object sender, EventArgs e)
        {
            using (AddViewToViewNode addView = new AddViewToViewNode())
            {
                addView.FrontendApplication = FrontendApplication;
                addView.BackendApplication = BackendApplication;
                addView.SelectedView = null;
                addView.ParentViewNode = objectTreeView.SelectedNode.Tag as ViewNode;

                if (addView.ShowDialog() == DialogResult.OK)
                {
                    // Update tree
                    BuildDialogTreeRememberLocation(true);
                    EnableDisableButtons();
                }
            }
        }

        private void setDrilldownField_Click(object sender, EventArgs e)
        {
            Domain.ViewAction vaction = objectTreeView.SelectedNode.Tag as Domain.ViewAction;
            Domain.UXAction action = vaction.Action;

            if ((vaction.Type == ViewActionType.Drilldown) || (vaction.Type == ViewActionType.JumpTo))
            {
                using (SelectMappedPropertyFromPropertyMap selectField = new SelectMappedPropertyFromPropertyMap())
                {
                    selectField.ValidMappedPropertyList = GetDrilldownFields(true);
                    selectField.SelectedMappedProperty = vaction.DrilldownFieldMappedProperty;

                    if (selectField.ShowDialog() == DialogResult.OK)
                    {
                        if (vaction.DrilldownFieldMappedProperty.Id != selectField.SelectedMappedProperty.Id)
                        {
                            vaction.DrilldownFieldMappedProperty = selectField.SelectedMappedProperty;

                            // Save the field change
                            modelService.MergeSaveDomainObject(vaction);

                            // Update the selected object
                            objectTreeView.SelectedNode.Tag = vaction;
                            ObjectPropertyGrid.SelectedObject = objectTreeView.SelectedNode.Tag;
                        }
                    }
                }
            }

        }

        private void moveViewNodeUpBtn_Click(object sender, EventArgs e)
        {
            TreeNode moveNode = objectTreeView.SelectedNode;
            TreeNode staticNode = moveNode.PrevNode;

            if (staticNode != null)
            {
                Movem(staticNode, moveNode);
            }

            // Set the moving node to selected again
            moveNode.TreeView.SelectedNode = moveNode;
        }

        private void moveViewNodeDownBtn_Click(object sender, EventArgs e)
        {
            TreeNode moveNode = objectTreeView.SelectedNode;
            TreeNode staticNode = moveNode.NextNode;

            if (moveNode != null)
            {
                Movem(moveNode, staticNode);
            }

            // Set the moving node to selected again
            moveNode.TreeView.SelectedNode = moveNode;
        }

        private void editViewBtn_Click(object sender, EventArgs e)
        {
            using (EditView form = new EditView())
            {
                form.Owner = this;
                form.FrontendApplication = FrontendApplication;
                form.BackendApplication = BackendApplication;

                form.View = objectTreeView.SelectedNode.Tag as Domain.View;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    objectTreeView.SelectedNode.Tag = form.View;
                    ObjectPropertyGrid.SelectedObject = objectTreeView.SelectedNode.Tag;

                    // Update the ViewNodes View with the correct one
                    ViewNode viewNode = objectTreeView.SelectedNode.Parent.Tag as ViewNode;
                    viewNode.View = form.View;

                    // Update the ViewNode text on the parent ViewNode
                    SetViewNodeText(viewNode, objectTreeView.SelectedNode.Parent);
                }
            }
        }

        private void btnEditViewNodeRule_Click(object sender, EventArgs e)
        {
            TreeNode node = objectTreeView.SelectedNode;
            Domain.ViewNode viewNode = node.Tag as Domain.ViewNode;

            viewNode = dialogService.GetViewNodeById(viewNode.Id);


            Type context = RuleContextFactory.CreateViewContext(dialogService.GetParentViewNode(viewNode));

            using (RuleSetDialog ruleSetDialog = new RuleSetDialog(context, null, viewNode.RuleSet))
            {
                ruleSetDialog.Text += string.Format(" [{0}]", viewNode.View.Name);

                if (ruleSetDialog.ShowDialog() == DialogResult.OK)
                {
                    if (this.Dialog.IsLocked && this.Dialog.LockedBy == Environment.UserName)
                    {
                        if (ruleSetDialog.RuleSet.Rules.Count == 0)
                            viewNode.RuleSet = null;
                        else
                            viewNode.RuleSet = ruleSetDialog.RuleSet;

                        dialogService.SaveViewNode(viewNode);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Your changes will not be saved since the dialog is not locked for editing. Check out the dialog and try again");
                        return;
                    }

                    BuildDialogTreeRememberLocation(true);
                }
            }
        }

        private void editViewInterface_Click(object sender, EventArgs e)
        {
            if (objectTreeView.SelectedNode != null)
            {
                Domain.View view = objectTreeView.SelectedNode.Tag as Domain.View;

                using (EditInterfaceMap form = new EditInterfaceMap())
                {
                    form.Owner = this;
                    form.FrontendApplication = FrontendApplication;
                    form.BackendApplication = BackendApplication;
                    form.RequestMap = view.RequestMap;
                    form.ResponseMap = view.ResponseMap;
                    form.IsEditable = view.IsLocked && view.LockedBy == Environment.UserName;
                    form.CanRequestMap = view.Type == ViewType.Standard;
                    form.Text = string.Format("Edit View Interface - {0}", view.ToString());

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        try
                        {
                            view.ResponseMap = form.ResponseMap;
                            view.RequestMap = form.RequestMap;

                            if (form.AddedMappedProperties.Count > 0 || form.DeletedMappedProperties.Count > 0)
                            {
                                modelService.StartSynchronizePropertyMapsInObjects(view, new List<IDomainObject>(), form.DeletedMappedProperties.Cast<IDomainObject>().ToList());
                            }

                            modelService.MergeSaveDomainObject(view.ResponseMap);
                            modelService.MergeSaveDomainObject(view.RequestMap);


                            view = modelService.GetDomainObject<Cdc.MetaManager.DataAccess.Domain.View>(view.Id);

                            objectTreeView.SelectedNode.Tag = view;

                            if (view.Type == ViewType.Standard)
                            {
                                if ((Dialog.InterfaceView != null) &&
                                    (view.Id == Dialog.InterfaceView.Id) &&
                                    (Dialog.SearchPanelView != null))
                                {


                                    if (form.searchableParameterUpdated == true)
                                    {

                                        if (!NHibernateUtil.IsInitialized(Dialog))
                                        {
                                            Dialog = (DataAccess.Domain.Dialog)modelService.GetInitializedDomainObject(Dialog.Id, modelService.GetDomainObjectType(Dialog));
                                        }

                                        Domain.View readView = modelService.GetDomainObject<DataAccess.Domain.View>(Dialog.SearchPanelView.Id);

                                        CheckOutInObject(readView, true);

                                        readView = modelService.GetDomainObject<DataAccess.Domain.View>(Dialog.SearchPanelView.Id);

                                        if (readView.IsLocked && readView.LockedBy == Environment.UserName)
                                        {
                                            dialogService.CreateOrUpdateSearchPanelView(Dialog);

                                            editSearchPanelBtn_Click(sender, e);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }

                        Cursor = Cursors.Default;
                    }
                }
            }
        }

        private void checkoutViewBtn_Click(object sender, EventArgs e)
        {

            CheckOutCheckInMenuOption(sender, true);

            BuildDialogTreeRememberLocation(true);
            EnableDisableButtons();
        }

        private void checkInViewBtn_Click(object sender, EventArgs e)
        {
            CheckOutCheckInMenuOption(sender, false);

            BuildDialogTreeRememberLocation(true);
            EnableDisableButtons();
        }

        private void checkOutActionBtn_Click(object sender, EventArgs e)
        {
            CheckOutCheckInMenuOption(sender, true);

            BuildDialogTreeRememberLocation(true);
            EnableDisableButtons();
        }

        private void checkInActionBtn_Click(object sender, EventArgs e)
        {
            CheckOutCheckInMenuOption(sender, false);

            BuildDialogTreeRememberLocation(true);
            EnableDisableButtons();

        }

        private void undoCheckoutViewBtn_Click(object sender, EventArgs e)
        {
            UndoCheckOutMenuOption(sender);
            BuildDialogTreeRememberLocation(true);
            EnableDisableButtons();

        }

        private void undoCheckoutActionBtn_Click(object sender, EventArgs e)
        {
            UndoCheckOutMenuOption(sender);
            BuildDialogTreeRememberLocation(true);
            EnableDisableButtons();
        }

        private void checkoutSearchPanelBtn_Click(object sender, EventArgs e)
        {
            if (!NHibernateUtil.IsInitialized(Dialog))
            {
                Dialog = (DataAccess.Domain.Dialog)modelService.GetInitializedDomainObject(Dialog.Id, modelService.GetDomainObjectType(Dialog));
            }
            Domain.View readView = modelService.GetDomainObject<DataAccess.Domain.View>(Dialog.SearchPanelView.Id);
            CheckOutInObject(readView, true);
            // Rewrite tree
            BuildDialogTreeRememberLocation(true);
            EnableDisableButtons();
        }


        private void undoCheckOutSearchPanelBtn_Click(object sender, EventArgs e)
        {

            if (!NHibernateUtil.IsInitialized(Dialog))
            {
                Dialog = (DataAccess.Domain.Dialog)modelService.GetInitializedDomainObject(Dialog.Id, modelService.GetDomainObjectType(Dialog));
            }

            Domain.View readView = modelService.GetDomainObject<DataAccess.Domain.View>(Dialog.SearchPanelView.Id);

            if (!NHibernateUtil.IsInitialized(readView))
            {
                readView = (DataAccess.Domain.View)modelService.GetInitializedDomainObject(readView.Id, modelService.GetDomainObjectType(readView));
            }

            DataAccess.IVersionControlled domainObject = readView;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                MetaManagerServices.GetConfigurationManagementService().UndoCheckOutDomainObject(domainObject.Id, domainObject.GetType(), FrontendApplication);
                Cursor.Current = Cursors.Default;
            }
            catch (ConfigurationManagementException ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Rewrite tree
            BuildDialogTreeRememberLocation(true);
            EnableDisableButtons();
        }

        private void checkinSearchPanelBtn_Click(object sender, EventArgs e)
        {
            if (!NHibernateUtil.IsInitialized(Dialog))
            {
                Dialog = (DataAccess.Domain.Dialog)modelService.GetInitializedDomainObject(Dialog.Id, modelService.GetDomainObjectType(Dialog));
            }

            Domain.View readView = modelService.GetDomainObject<DataAccess.Domain.View>(Dialog.SearchPanelView.Id);
            CheckOutInObject(readView, false);

            // Rewrite tree
            BuildDialogTreeRememberLocation(true);
            EnableDisableButtons();
        }

        #region Configuration Management Functions

        private void UndoCheckOutMenuOption(object sender)
        {
            if (objectTreeView.SelectedNode != null && objectTreeView.SelectedNode.Tag != null)
            {
                DataAccess.IVersionControlled domainObject = null;

                if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(objectTreeView.SelectedNode.Tag.GetType()))
                {
                    domainObject = ((DataAccess.IVersionControlled)objectTreeView.SelectedNode.Tag);
                }
                else
                {
                    if (objectTreeView.SelectedNode.Tag is Domain.ViewAction)
                    {
                        ViewAction selectedViewAction = (ViewAction)objectTreeView.SelectedNode.Tag;
                        domainObject = (DataAccess.IVersionControlled)selectedViewAction.Action;
                    }
                }

                if (domainObject != null)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        MetaManagerServices.GetConfigurationManagementService().UndoCheckOutDomainObject(domainObject.Id, domainObject.GetType(), FrontendApplication);
                        Cursor.Current = Cursors.Default;
                    }
                    catch (ConfigurationManagementException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    objectTreeView.SelectedNode.Tag = modelService.GetDomainObject(domainObject.Id, modelService.GetDomainObjectType(domainObject));
                }
            }

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

        private void CheckOutCheckInMenuOption(object sender, bool trueCheckOut_falseCheckIn)
        {
            if (objectTreeView.SelectedNode != null && objectTreeView.SelectedNode.Tag != null)
            {

                DataAccess.IVersionControlled domainObject = null;

                if (objectTreeView.SelectedNode.Tag is Domain.ViewAction)
                {
                    ViewAction selectedViewAction = (ViewAction)objectTreeView.SelectedNode.Tag;
                    domainObject = (DataAccess.IVersionControlled)selectedViewAction.Action;

                }
                else
                {
                    domainObject = ((DataAccess.IVersionControlled)objectTreeView.SelectedNode.Tag);
                }


                if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(domainObject.GetType()))
                {
                    if (trueCheckOut_falseCheckIn)
                    {
                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(domainObject.Id, domainObject.GetType());
                            Cursor.Current = Cursors.Default;
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

                    objectTreeView.SelectedNode.Tag = modelService.GetDomainObject(domainObject.Id, modelService.GetDomainObjectType(domainObject));
                }
            }


        }


        #endregion

    }
}
