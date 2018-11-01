using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;


namespace Cdc.MetaManager.GUI
{
    public partial class AddMenu : MdiChildForm
    {
        private IMenuService menuService = null;

        public AddMenu()
        {
            InitializeComponent();

            // Get application service context
            menuService = MetaManagerServices.GetMenuService();
        }

        public Cdc.MetaManager.DataAccess.Domain.MenuItem CurrentMenuItem 
        {
            get
            {
                if (tvMenu.SelectedNode != null)
                    return (Cdc.MetaManager.DataAccess.Domain.MenuItem)tvMenu.SelectedNode.Tag;
                else
                    return null;
            }
        }

        private void PopulateTreeView()
        {
            
            // Check if the application has a topmenuitem
            Cdc.MetaManager.DataAccess.Domain.Menu menu = menuService.GetMenuByApplicationId(FrontendApplication.Id);

            // If a menu didn't exist then create it with one node.
            if (menu == null)
            {
                menu = new Cdc.MetaManager.DataAccess.Domain.Menu();

                menu.Application = FrontendApplication;

                Cdc.MetaManager.DataAccess.Domain.MenuItem topMenuItem = new Cdc.MetaManager.DataAccess.Domain.MenuItem();

                topMenuItem.Name = string.Empty;
                topMenuItem.Caption = "Menu Caption (change me!)";
                topMenuItem.Menu = null;
                topMenuItem.Parent = null;

                topMenuItem = menuService.SaveMenuItem(topMenuItem);

                menu.TopMenuItem = topMenuItem;

                menuService.SaveMenu(menu);

                topMenuItem.Menu = menu;

                topMenuItem = menuService.SaveMenuItem(topMenuItem);
            }

            if (menu != null)
            {
                // Fetch the tree of menuitems
                Cdc.MetaManager.DataAccess.Domain.MenuItem topMenuItem = menuService.GetMenuItemById(menu.TopMenuItem.Id);

                // Check if we found any items
                if (topMenuItem != null)
                {
                    TreeNode dialogTreeNode = new TreeNode();

                    BuildViewTree(topMenuItem, dialogTreeNode);

                    tvMenu.Nodes.Add(dialogTreeNode);

                    // Expand the whole menu
                    tvMenu.ExpandAll();
                }
            }
        }

        private static void BuildViewTree(Cdc.MetaManager.DataAccess.Domain.MenuItem menuItem, TreeNode parentTreeNode)
        {
            parentTreeNode.Tag = menuItem;
            UpdateTreeNode(parentTreeNode);

            if (menuItem.Children.Count > 0)
            {
                foreach (Cdc.MetaManager.DataAccess.Domain.MenuItem vChild in menuItem.Children.OrderBy(item => item.Sequence))
                {
                    TreeNode child = new TreeNode();

                    BuildViewTree(vChild, child);

                    parentTreeNode.Nodes.Add(child);
                }
            }
        }

        private void AddMenu_Load(object sender, EventArgs e)
        {
            PopulateTreeView();
            Cdc.MetaManager.DataAccess.Domain.Menu menu = menuService.GetMenuByApplicationId(FrontendApplication.Id);
            this.IsEditable = menu.IsLocked && menu.LockedBy == Environment.UserName;

            Cursor.Current = Cursors.Default;
        }

        private void tvMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            tsbtnDelete.Enabled = false;
            tsbtnAddSibling.Enabled = false;
            tsbtnAddChild.Enabled = false;
            tsbtnEdit.Enabled = false;
            tsbtnMoveDown.Enabled = false;
            tsbtnMoveUp.Enabled = false;

            if (tvMenu.SelectedNode != null)
            {
                tsbtnMoveUp.Enabled = tvMenu.SelectedNode.PrevNode != null && this.IsEditable;
                tsbtnMoveDown.Enabled = tvMenu.SelectedNode.NextNode != null  && this.IsEditable;
                tsbtnAddChild.Enabled = this.IsEditable;
                tsbtnEdit.Enabled = this.IsEditable;

                if (tvMenu.SelectedNode.Parent != null)
                {
                    tsbtnAddSibling.Enabled = this.IsEditable;
                    tsbtnDelete.Enabled = this.IsEditable;
                }

            }
        }

        private bool AddMenuItem(TreeNode parentNode)
        {
            // Get parents MenuItem
            Cdc.MetaManager.DataAccess.Domain.MenuItem parentMenuItem = (Cdc.MetaManager.DataAccess.Domain.MenuItem)parentNode.Tag;

            // Create the new menuitem
            Cdc.MetaManager.DataAccess.Domain.MenuItem newItem = new Cdc.MetaManager.DataAccess.Domain.MenuItem();
            
            // Edit the MenuItem
            using (AddMenuItem editMenuItem = new AddMenuItem())
            {
                editMenuItem.FrontendApplication = FrontendApplication;
                editMenuItem.BackendApplication = BackendApplication;
                editMenuItem.CurrentMenuItem = newItem;

                if (editMenuItem.ShowDialog() == DialogResult.OK)
                {
                    // Add the item to the parentitem
                    newItem.Menu = parentMenuItem.Menu;
                    newItem.Parent = parentMenuItem;

                    // Set sequence
                    newItem.Sequence = SetSequence(parentNode);

                    // Save the menuitem created
                    newItem = menuService.SaveMenuItem(newItem);

                    // Add the child to the parent menuitem
                    parentMenuItem.Children.Add(newItem);

                    // Add a node to the treeview
                    TreeNode createdNode = CreateNewNode(parentNode, newItem);

                    // Set the new node to be the selected one.
                    tvMenu.SelectedNode = createdNode;

                    return true;
                }
            }

            return false;
        }

        private int SetSequence(TreeNode parentNode)
        {
            if (parentNode.Nodes.Count > 0)
            {
                List<Cdc.MetaManager.DataAccess.Domain.MenuItem> menuItems = new List<Cdc.MetaManager.DataAccess.Domain.MenuItem>();

                foreach (TreeNode childNode in parentNode.Nodes)
                {
                    menuItems.Add((Cdc.MetaManager.DataAccess.Domain.MenuItem)childNode.Tag);
                }

                return menuItems.Max(item => item.Sequence) + 1;
            }
            else
            {
                return 1;
            }
        }

        private TreeNode CreateNewNode(TreeNode parentNode, Cdc.MetaManager.DataAccess.Domain.MenuItem currentItem)
        {
            TreeNode newNode = new TreeNode();

            newNode.Tag = currentItem;
            UpdateTreeNode(newNode);

            parentNode.Nodes.Add(newNode);
            UpdateTreeNode(parentNode);

            return newNode;
        }

        private static void UpdateTreeNode(TreeNode updateNode)
        {
            if (updateNode != null)
            {
                Cdc.MetaManager.DataAccess.Domain.MenuItem currentItem = (Cdc.MetaManager.DataAccess.Domain.MenuItem)updateNode.Tag;

                if (currentItem != null)
                {
                    updateNode.Text = string.Format("{0}{1}"
                                                       , currentItem.Caption
                                                       , string.IsNullOrEmpty(currentItem.Name) ?
                                                                string.Empty :
                                                                string.Format(" [{0}]", currentItem.Name)
                                                    );

                    updateNode.ForeColor = currentItem.Action == null ? 
                                                (currentItem.Children.Count > 0 ? 
                                                      Color.Green : 
                                                      Color.Black
                                                ) 
                                                : Color.Blue;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsbtnDelete_Click(object sender, EventArgs e)
        {
            if (CurrentMenuItem != null && tvMenu.SelectedNode.Parent != null)
            {
                bool doDelete = false;

                if (CurrentMenuItem.Children.Count > 0)
                {
                    doDelete = MessageBox.Show("Are you sure you want to delete the complete subtree of nodes?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
                }
                else
                {
                    doDelete = MessageBox.Show("Are you sure you want to delete this node?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
                }

                if (doDelete)
                {
                    TreeNode parentNode = tvMenu.SelectedNode.Parent;

                    // Delete the item from the parents list
                    CurrentMenuItem.Parent.Children.Remove(CurrentMenuItem);

                    // Delete the item
                    menuService.DeleteMenuItem(CurrentMenuItem);

                    // Remove the node from the tree
                    tvMenu.SelectedNode.Remove();

                    // Update the Parentnode
                    UpdateTreeNode(parentNode);
                }
            }
        }

        private void tsbtnAddSibling_Click(object sender, EventArgs e)
        {
            if (CurrentMenuItem != null && CurrentMenuItem.Parent != null)
            {
                AddMenuItem(tvMenu.SelectedNode.Parent);
            }
        }

        private void tsbtnAddChild_Click(object sender, EventArgs e)
        {
            if (CurrentMenuItem != null)
            {
                Cdc.MetaManager.DataAccess.Domain.MenuItem clearAction = null;

                // Check if an action is attached to the node. In that case ask user to remove it before continuing.
                if (CurrentMenuItem.Action != null)
                {
                    if (MessageBox.Show("A MenuItem with an attached Action cannot have childrens.\nThe Action on the selected MenuItem will be removed when you save the child.\n\nDo you want to continue?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        clearAction = CurrentMenuItem;
                    }
                    else
                    {
                        return;
                    }
                }

                if (AddMenuItem(tvMenu.SelectedNode) && (clearAction != null))
                {
                    clearAction.Action = null;
                    menuService.SaveMenuItem(clearAction);
                }
            }
        }

        private void tsbtnEdit_Click(object sender, EventArgs e)
        {
            EditCurrentSelectedMenuItem();
        }

        private void EditCurrentSelectedMenuItem()
        {
            if (CurrentMenuItem != null)
            {
                // Edit the MenuItem
                using (AddMenuItem editMenuItem = new AddMenuItem())
                {
                    editMenuItem.FrontendApplication = FrontendApplication;
                    editMenuItem.BackendApplication = BackendApplication;
                    editMenuItem.CurrentMenuItem = CurrentMenuItem;
                    editMenuItem.IsRootMenuItem = tvMenu.SelectedNode.Parent == null;

                    if (editMenuItem.ShowDialog() == DialogResult.OK)
                    {
                        // Save the menuitem created
                        menuService.SaveMenuItem(editMenuItem.CurrentMenuItem);

                        // Set the new menuitem to the tag.
                        tvMenu.SelectedNode.Tag = editMenuItem.CurrentMenuItem;

                        // Update selected node
                        UpdateTreeNode(tvMenu.SelectedNode);
                    }
                }
            }
        }

        private void tsbtnMoveUp_Click(object sender, EventArgs e)
        {
            TreeNode moveNode = tvMenu.SelectedNode;
            TreeNode staticNode = moveNode.PrevNode;

            if (staticNode != null)
            {
                Movem(staticNode, moveNode);
            }

            // Set the moving node to selected again
            moveNode.TreeView.SelectedNode = moveNode;
        }

        private void tsbtnMoveDown_Click(object sender, EventArgs e)
        {
            TreeNode moveNode = tvMenu.SelectedNode;
            TreeNode staticNode = moveNode.NextNode;

            if (moveNode != null)
            {
                Movem(moveNode, staticNode);
            }

            // Set the moving node to selected again
            moveNode.TreeView.SelectedNode = moveNode;
        }

        private void Movem(TreeNode staticNode, TreeNode moveNode)
        {
            Cdc.MetaManager.DataAccess.Domain.MenuItem moverMenuItem = moveNode.Tag as Cdc.MetaManager.DataAccess.Domain.MenuItem;
            Cdc.MetaManager.DataAccess.Domain.MenuItem staticMenuItem = staticNode.Tag as Cdc.MetaManager.DataAccess.Domain.MenuItem;

            menuService.MoveUp(moverMenuItem.Id, staticMenuItem.Id, out moverMenuItem, out staticMenuItem);

            //// Set the changed objects back as tags on the treenodes
            (moveNode.Tag as Cdc.MetaManager.DataAccess.Domain.MenuItem).Sequence = moverMenuItem.Sequence;
            (staticNode.Tag as Cdc.MetaManager.DataAccess.Domain.MenuItem).Sequence = staticMenuItem.Sequence;

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
                if (moverMenuItem.Sequence > staticMenuItem.Sequence)
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

        private void tvMenu_DragOver(object sender, DragEventArgs e)
        {
            DragDropEffects current = DragDropEffects.None;

            TreeNode draggedOver = tvMenu.GetNodeAt(tvMenu.PointToClient(new Point(e.X, e.Y)));

            if (draggedOver != null && e.Data.GetDataPresent(typeof(TreeNode)))
            {
                // Fetch the TreeNode from the sent data
                TreeNode sentTreeNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                
                // Check that the treenodes are different and that they don't have same parents
                // and that the hitted treenode isn't the parent to the sent child.
                if (sentTreeNode != draggedOver &&
                    draggedOver != sentTreeNode.Parent)
                {
                    current = DragDropEffects.Move;
                }
            }

            e.Effect = current;
        }

        private void tvMenu_DragDrop(object sender, DragEventArgs e)
        {
            if (sender is TreeView)
            {
                // Get treenode where item was dropped
                TreeNode hittedItem = tvMenu.GetNodeAt((sender as TreeView).PointToClient(new Point(e.X, e.Y)));

                if (hittedItem != null)
                {
                    // Get the TreeNode that was sent.
                    if (e.Data.GetDataPresent(typeof(TreeNode)))
                    {
                        // Fetch the TreeNode from the sent data
                        TreeNode sentTreeNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                        
                        // Check that the treenodes are different and that they don't have same parents
                        // and that the hitted treenode isn't the parent to the sent child.
                        if (sentTreeNode != hittedItem && 
                            hittedItem != sentTreeNode.Parent)
                        {
                            Cdc.MetaManager.DataAccess.Domain.MenuItem newParent = (Cdc.MetaManager.DataAccess.Domain.MenuItem)hittedItem.Tag;
                            Cdc.MetaManager.DataAccess.Domain.MenuItem child = (Cdc.MetaManager.DataAccess.Domain.MenuItem)sentTreeNode.Tag;

                            if (newParent != null && child != null)
                            {
                                Cdc.MetaManager.DataAccess.Domain.MenuItem clearAction = null;

                                // Check if an action is attached to the new parent node. In that case ask user to remove it before continuing.
                                if (newParent.Action != null)
                                {
                                    if (MessageBox.Show("A MenuItem with an attached Action cannot have childrens.\nThe Action on the MenuItem where you released the mousebutton will be removed.\n\nDo you want to continue?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                    {
                                        clearAction = newParent;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }

                                // Remove the child from it's parent
                                if (child.Parent != null)
                                    child.Parent.Children.Remove(child);

                                // Change parent on the child
                                child.Parent = newParent;

                                // Add the child to the parents children
                                child.Parent.Children.Add(child);

                                // Set the sequence
                                child.Sequence = 0;
                                child.Sequence = child.Parent.Children.Max(item => item.Sequence) + 1;

                                // Save the child
                                menuService.SaveMenuItem(child);

                                // If the action should be cleared on the parent then do it
                                if (clearAction != null)
                                {
                                    clearAction.Action = null;
                                    menuService.SaveMenuItem(clearAction);
                                }

                                // Update the hitteditem
                                UpdateTreeNode(hittedItem);

                                TreeNode sentTreeNodeParent = sentTreeNode.Parent;

                                // Move the treenode to the new parent node
                                sentTreeNode.Parent.Nodes.Remove(sentTreeNode);
                                hittedItem.Nodes.Add(sentTreeNode);

                                tvMenu.SelectedNode = sentTreeNode;

                                UpdateTreeNode(sentTreeNodeParent);
                                UpdateTreeNode(sentTreeNode);
                            }
                        }
                    }
                }

            }
        }

        private void tvMenu_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TreeNode draggedItem = (TreeNode)e.Item;

                // Check if we are dragging something that isn't the root node.
                if (draggedItem != null && draggedItem.Parent != null)
                {
                    DragDropEffects dde = DoDragDrop(draggedItem, DragDropEffects.Move);
                }
            }
        }

        private void tvMenu_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditCurrentSelectedMenuItem();
        }

    }
}
