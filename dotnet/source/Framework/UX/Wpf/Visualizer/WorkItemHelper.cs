using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.CompositeUI;
using Utility = Microsoft.Practices.CompositeUI.Utility;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.Windows.Media;
using Microsoft.Practices.CompositeUI.EventBroker;
using System.Windows.Input;
using Microsoft.Practices.CompositeUI.Commands;

namespace Imi.Framework.UX.Wpf.Visualizer
{
    internal class WorkItemHelper
    {
        #region Fields

        Style treeViewItemStyle;
        Style activeWorkItemStyle;
        Style workItemStyle;
        TreeView treeView;

        #endregion

        internal EventHandler<Utility.DataEventArgs<object>> OnSelectedItem;

        public WorkItemHelper(TreeView treeView, Style activeStyle, Style itemStyle, Style workItemStyle)
        {
            if (treeView == null)
                throw new ArgumentNullException("treeView");

            this.treeView = treeView;
            this.activeWorkItemStyle = activeStyle;
            this.treeViewItemStyle = itemStyle;
            this.workItemStyle = workItemStyle;

            treeView.MouseDoubleClick += treeView_MouseDoubleClick;
        }

        internal void Run(WorkItem workItem)
        {
            WorkItem_Added(this, new Utility.DataEventArgs<WorkItem>(workItem));
        }

        #region TreeViewItem Create, Add, Get, Remove Methods

        private static TreeViewItem CreateTreeViewItem(string header, Style style)
        {
            return CreateTreeViewItem(header, style, null);
        }

        private static TreeViewItem CreateTreeViewItem(string header, Style style, object tag)
        {
            TreeViewItem node = new TreeViewItem();
            node.Header = header;
            node.Style = style;
            node.Tag = tag;
            node.DataContext = null;
            
            return node;
        }


        private void CreateWorkItemsNodes(WorkItem wi, TreeViewItem node)
        {
            TreeViewItem workItemsNode = CreateTreeViewItem("WorkItems", treeViewItemStyle, wi);
            node.Items.Add(workItemsNode);

            foreach (KeyValuePair<string, WorkItem> container in wi.WorkItems)
            {
                AddWorkItem(workItemsNode.Items, container.Value);
            }
        }
        
        private void CreateWorkspacesNodes(WorkItem wi, TreeViewItem node)
        {
            if (wi.Workspaces.Count > 0)
            {
                TreeViewItem itemsNode = CreateTreeViewItem("Workspaces", treeViewItemStyle);

                KeyValuePair<string, IWorkspace> container;

                foreach (object workspace in wi.Workspaces)
                {
                    container = (KeyValuePair<string, IWorkspace>)workspace;
                    container.Value.SmartPartActivated += workspace_SmartPartActivated;
                    container.Value.SmartPartClosing += workspace_SmartPartClosing;
                   
                    TreeViewItem workspaceNode = CreateTreeViewItem("Workspace", treeViewItemStyle, container.Value);
                    workspaceNode.DataContext = new ItemInfo("Name: " + container.Key, "Type: " + container.Value.GetType().Name);

                    CreateSmartPartsNodes(container.Value, workspaceNode);
                    
                    itemsNode.Items.Add(workspaceNode);
                }

                node.Items.Add(itemsNode);
            }
        }

        private void CreateSmartPartsNodes(IWorkspace wks, TreeViewItem node)
        {
            if (wks.SmartParts.Count > 0)
            {
                TreeViewItem itemsNode = CreateTreeViewItem("SmartParts", treeViewItemStyle);

                foreach (object smartPart in wks.SmartParts)
                {
                    TreeViewItem smartPartHeader = CreateTreeViewItem(smartPart.GetType().Name, treeViewItemStyle, smartPart);
                    itemsNode.Items.Add(smartPartHeader);
                }

                node.Items.Add(itemsNode);
            }
        }

        private void CreateSmartPartsNodes(WorkItem wi, TreeViewItem node)
        {
            if (wi.SmartParts.Count > 0)
            {
                TreeViewItem itemsNode = CreateTreeViewItem("SmartParts", treeViewItemStyle);

                KeyValuePair<string, object> container;

                foreach (object smartPart in wi.SmartParts)
                {
                    container = (KeyValuePair<string, object>)smartPart;

                    TreeViewItem smartPartHeader = CreateTreeViewItem(container.Value.GetType().Name, treeViewItemStyle, container.Value);
                    itemsNode.Items.Add(smartPartHeader);
                }

                node.Items.Add(itemsNode);
            }
        }

        private TreeViewItem CreateItemNode(object item)
        {
            TreeViewItem itemNode;

            if (item as EventTopic != null || item as WorkItem != null || 
                item.GetType().IsSubclassOf(typeof(WorkItem)) || item is IWorkspace)
            {
                return null;                    
            }

            if (item as Command != null)
            {
                Command command = (Command)item;

                itemNode = CreateTreeViewItem("Command: " + command.Name, treeViewItemStyle, item);

            }
            else
                itemNode = CreateTreeViewItem(item.GetType().Name, treeViewItemStyle, item);

            return itemNode;
        }

        private void CreateItemsNodes(WorkItem wi, TreeViewItem node)
        {
            if (wi.Items.Count > 0)
            {
                TreeViewItem itemsNode = CreateTreeViewItem("Items", treeViewItemStyle);

                KeyValuePair<string, object> container;
                
                foreach (object item in wi.Items)
                {
                    container = (KeyValuePair<string, object>)item;
                    TreeViewItem itemNode = CreateItemNode(container.Value);

                    if (itemNode != null)
                       itemsNode.Items.Add(itemNode);
                }

                node.Items.Add(itemsNode);
            }
        }

        private void CreateServicesNodes(WorkItem wi, TreeViewItem node)
        {
            if (wi.Services != null)
            {
                TreeViewItem itemsNode = CreateTreeViewItem("Services", treeViewItemStyle);

                KeyValuePair<Type, object> container;

                foreach (object service in wi.Services)
                {
                    container = (KeyValuePair<Type, object>)service;

                    if (container.Value as WorkItem == null && !container.Value.GetType().IsSubclassOf(typeof(WorkItem)))
                    {
                        TreeViewItem serviceNode = CreateTreeViewItem(container.Value.GetType().Name, treeViewItemStyle, container.Value);
                        itemsNode.Items.Add(serviceNode);
                    }
                }

                node.Items.Add(itemsNode);
            }
        }

        private void CreateEventTopicsNodes(WorkItem wi, TreeViewItem node)
        {
            if (wi.EventTopics.Count > 0)
            {
                TreeViewItem itemsNode = CreateTreeViewItem("Event Topics", treeViewItemStyle);

                KeyValuePair<string, EventTopic> container;

                foreach (object eventTopic in wi.EventTopics)
                {
                    container = (KeyValuePair<string, EventTopic>)eventTopic;

                    TreeViewItem eventTopicNode = CreateTreeViewItem(container.Value.Name, treeViewItemStyle, container.Value);
                    eventTopicNode.DataContext = new ItemInfo("Publication Count: " + container.Value.PublicationCount, "Subscription Count: " + container.Value.SubscriptionCount);
                    itemsNode.Items.Add(eventTopicNode);
                }

                node.Items.Add(itemsNode);
            }
        }

        private TreeViewItem CreateWorkItemTreeViewItem(WorkItem wi)
        {
            bool isController = false;
            string assembly = wi.GetType().Module.Name;
            string type = wi.GetType().FullName;
            string header = "WorkItem";

            if ((wi.GetType().IsSubclassOf(typeof(WorkItem))))
            {
                if (wi.GetType().IsGenericType)
                {
                    isController = true;

                    type = wi.GetType().GetGenericArguments()[0].FullName;
                    assembly = wi.GetType().GetGenericArguments()[0].Module.Name;

                    header = "ControlledWorkItem";
                }
            }

            TreeViewItem item = CreateTreeViewItem(header, workItemStyle, wi);
            item.DataContext = new WorkItemInfo(assembly, type, isController);

            CreateWorkItemsNodes(wi, item);
            CreateWorkspacesNodes(wi, item);
            CreateSmartPartsNodes(wi, item);
            CreateItemsNodes(wi, item);
            CreateServicesNodes(wi, item);
            CreateEventTopicsNodes(wi, item);

            return item;
        }

        private TreeViewItem GetTreeViewItem(object item)
        {
            return GetTreeViewItem(item, treeView.Items, false);
        }

        private TreeViewItem GetTreeViewItem(object item, bool getWorkItemsNode)
        {
            return GetTreeViewItem(item, treeView.Items, getWorkItemsNode);
        }

        private TreeViewItem GetTreeViewItem(object item, ItemCollection nodes, bool getWorkItemsNode)
        {
            TreeViewItem tempNode = null;

            if (item == null)
                return null;

            if (nodes == null)
                return null;

            foreach (TreeViewItem node in nodes)
            {
                if (node != null && node.Tag == item)
                    tempNode = node;
            }

            if (tempNode == null)
            {
                foreach (TreeViewItem node in nodes)
                {
                    TreeViewItem itemNode = GetTreeViewItem(item, node.Items, getWorkItemsNode);

                    if (itemNode != null)
                        tempNode = itemNode;
                }
            }

            if (tempNode != null)
            {
                if (getWorkItemsNode && !tempNode.Items.IsEmpty)
                    return tempNode.Items[0] as TreeViewItem;

                return tempNode;
            }

            return null;
        }

        private void AddWorkItem(ItemCollection collection, WorkItem wi)
        {
            if (wi.WorkItems != null)
            {
                wi.WorkItems.Added += WorkItem_Added;
                wi.WorkItems.Removed += WorkItem_Removed;
            }

            if (wi.SmartParts != null)
            {
                wi.SmartParts.Added += SmartParts_Added;
                wi.SmartParts.Removed += SmartParts_Removed;
            }

            if (wi.Items != null)
            {
                wi.Items.Added += Items_Added;
                wi.Items.Removed += Items_Removed;
            }

            if (wi.EventTopics != null)
            {
                wi.EventTopics.Added += EventTopics_Added;
                wi.EventTopics.Removed += EventTopics_Removed;
            }

            if (wi.Workspaces != null)
            {
                wi.Workspaces.Added += Workspaces_Added;
                wi.Workspaces.Removed += Workspaces_Removed;
            }

            wi.IdChanged += WorkItem_IdChanged;
            wi.Activated += WorkItem_Activated;
            wi.Deactivated += WorkItem_Deactivated;
            wi.Disposed += WorkItem_Disposed;

            collection.Add(CreateWorkItemTreeViewItem(wi));
        }

        private TreeViewItem RemoveTreeViewItem(object item)
        {
            TreeViewItem tt = RemoveTreeViewItem(item, treeView.Items);

            return tt;
        }

        private TreeViewItem RemoveTreeViewItem(object item, ItemCollection nodes)
        {
            TreeViewItem nodeToRemove = null;
            ItemCollection collection = null;

            foreach (TreeViewItem node in nodes)
            {
                if (node != null && node.Tag == item)
                {
                    nodeToRemove = node;
                    collection = nodes;

                    if (nodeToRemove != null)
                    {
                        collection.Remove(nodeToRemove);

                        return nodeToRemove;
                    }
                }
            }

            if (nodeToRemove == null)
            {
                foreach (TreeViewItem node in nodes)
                {
                    TreeViewItem itemNode = RemoveTreeViewItem(item, node.Items);

                    if (itemNode != null)
                    {
                        return itemNode;
                    }
                }
            }

            if (nodeToRemove != null)
            {
                collection.Remove(nodeToRemove);
            }

            return nodeToRemove;
        }

        #endregion

        #region TreeView Events

        void treeView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TreeViewItem item = FindVisualParent<TreeViewItem, TreeView>(e.OriginalSource as DependencyObject);

            if (item != null)
            {
                if (OnSelectedItem != null)
                    OnSelectedItem(this, new Utility.DataEventArgs<object>(item.Tag));
            }
        }

        private static TParent FindVisualParent<TParent, TLimit>(DependencyObject obj) where TParent : DependencyObject
        {
            TParent parent = obj as TParent;

            while (obj != null && (parent == null))
            {
                if (obj is TLimit)
                    return null;

                obj = VisualTreeHelper.GetParent(obj);

                parent = obj as TParent;
            }

            return parent;
        }

        #endregion

        #region WorkItem Events

        private WorkItem GetWorkItem(object sender)
        {
            Type type = sender.GetType();

            return type.GetField("workItem", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).GetValue(sender) as WorkItem;
        }

        private void RemoveItem(object sender, object data)
        {
            WorkItem workItem = GetWorkItem(sender);

            if (workItem != null)
            {
                TreeViewItem item = GetTreeViewItem(workItem);
                RemoveTreeViewItem(data, item.Items);
            }
        }

        private void WorkItem_Added(object sender, Utility.DataEventArgs<WorkItem> e)
        {
            ItemCollection collection = null;
            WorkItem wi = e.Data;
            TreeViewItem node;

            if (wi.Parent == null)
                collection = treeView.Items;
            else
            {
                node = GetTreeViewItem(wi.Parent, true);

                if (node != null)
                    collection = node.Items;
            }

            if (collection != null)
            {
                AddWorkItem(collection, e.Data);
            }
        }

        void WorkItem_Removed(object sender, Utility.DataEventArgs<WorkItem> e)
        {
            if (e.Data == null)
                return;

            WorkItem wi = e.Data;

            wi.IdChanged -= WorkItem_IdChanged;
            wi.Activated -= WorkItem_Activated;
            wi.Deactivated -= WorkItem_Deactivated;

            if (wi.WorkItems != null)
            {
                wi.WorkItems.Added -= WorkItem_Added;
                wi.WorkItems.Removed -= WorkItem_Removed;
            }

            if (wi.SmartParts != null)
            {
                wi.SmartParts.Added -= SmartParts_Added;
                wi.SmartParts.Removed -= SmartParts_Removed;
            }

            if (wi.Items != null)
            {
                wi.Items.Added -= Items_Added;
                wi.Items.Removed -= Items_Removed;
            }

            if (wi.EventTopics != null)
            {
                wi.EventTopics.Added -= EventTopics_Added;
                wi.EventTopics.Removed -= EventTopics_Removed;
            }

            if (wi.Workspaces != null)
            {
                wi.Workspaces.Added -= Workspaces_Added;
                wi.Workspaces.Removed -= Workspaces_Removed;
            }

            wi.Disposed -= WorkItem_Disposed;

            RemoveTreeViewItem(wi);
        }

        void Workspaces_Removed(object sender, Utility.DataEventArgs<IWorkspace> e)
        {
            RemoveItem(sender, e.Data);
        }

        void Workspaces_Added(object sender, Utility.DataEventArgs<IWorkspace> e)
        {
            WorkItem workItem = GetWorkItem(sender);

            if (workItem != null)
            {
                TreeViewItem item = GetTreeViewItem(workItem);
                TreeViewItem workspaceHeader = CreateTreeViewItem("Workspaces", treeViewItemStyle);
                TreeViewItem workspaceItem = CreateTreeViewItem("Workspace", treeViewItemStyle, e.Data);
                workspaceItem.DataContext = new ItemInfo("Name: " + e.Data.GetType().Name, "Type: " + e.Data.GetType().Name);

                e.Data.SmartPartActivated += workspace_SmartPartActivated;
                e.Data.SmartPartClosing += workspace_SmartPartClosing;

                CreateSmartPartsNodes(e.Data, workspaceItem);

                if (!item.Items.IsEmpty)
                {
                    TreeViewItem tempItem = null;

                    foreach (TreeViewItem temp in item.Items)
                    {
                        if (String.Compare(temp.Header.ToString(), "workspaces", StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            tempItem = temp;

                            break;
                        }
                    }

                    if (tempItem != null)
                    {
                        TreeViewItem existItem = GetTreeViewItem(e.Data, tempItem.Items, false);

                        if (existItem != null)
                            return;

                        tempItem.Items.Add(workspaceItem);

                        return;
                    }
                }

                workspaceHeader.Items.Add(workspaceItem);
                item.Items.Add(workspaceHeader);
            }
        }

        void SmartParts_Removed(object sender, Utility.DataEventArgs<object> e)
        {
            RemoveItem(sender, e.Data);
        }

        void SmartParts_Added(object sender, Utility.DataEventArgs<object> e)
        {
            WorkItem workItem = GetWorkItem(sender);

            if (workItem != null)
            {
                TreeViewItem item = GetTreeViewItem(workItem);
                TreeViewItem smartPartHeader = CreateTreeViewItem("SmartParts", treeViewItemStyle);
                TreeViewItem smartPartItem = CreateTreeViewItem(e.Data.GetType().Name, treeViewItemStyle, e.Data);
            
                if (!item.Items.IsEmpty)
                {
                    TreeViewItem tempItem = null;

                    foreach (TreeViewItem temp in item.Items)
                    {
                        if (String.Compare(temp.Header.ToString(), "smartparts", StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            tempItem = temp;

                            break;
                        }
                    }

                    if (tempItem != null)
                    {
                        TreeViewItem existItem = GetTreeViewItem(e.Data, tempItem.Items, false);

                        if (existItem != null)
                            return;

                        tempItem.Items.Add(smartPartItem);

                        return;
                    }
                }

                smartPartHeader.Items.Add(smartPartItem);
                item.Items.Add(smartPartHeader);
            }
        }

        void Items_Removed(object sender, Utility.DataEventArgs<object> e)
        {
            RemoveItem(sender, e.Data);
        }

        void Items_Added(object sender, Utility.DataEventArgs<object> e)
        {
            WorkItem workItem = GetWorkItem(sender);

            if (workItem != null)
            {
                TreeViewItem item = GetTreeViewItem(workItem);
                TreeViewItem itemHeader = CreateTreeViewItem("Items", treeViewItemStyle);
                TreeViewItem itemNode = CreateItemNode(e.Data);

                if (itemNode == null)
                    return;

                if (!item.Items.IsEmpty)
                {
                    TreeViewItem tempItem = null;

                    foreach (TreeViewItem temp in item.Items)
                    {
                        if (String.Compare(temp.Header.ToString(), "items", StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            tempItem = temp;

                            break;
                        }
                    }

                    if (tempItem != null)
                    {
                        TreeViewItem existItem = GetTreeViewItem(e.Data, tempItem.Items, false);

                        if (existItem != null)
                            return;

                        tempItem.Items.Add(itemNode);

                        return;
                    }
                }

                itemHeader.Items.Add(itemNode);
                item.Items.Add(itemHeader);
            }
        }

        void EventTopics_Removed(object sender, Utility.DataEventArgs<EventTopic> e)
        {
            RemoveItem(sender, e.Data);
        }

        void EventTopics_Added(object sender, Utility.DataEventArgs<EventTopic> e)
        {
            WorkItem workItem = GetWorkItem(sender);

            if (workItem != null)
            {
                TreeViewItem item = GetTreeViewItem(workItem);
                TreeViewItem eventTopicHeader = CreateTreeViewItem("Event Topics", treeViewItemStyle);
               
                TreeViewItem eventTopicItem = CreateTreeViewItem(e.Data.Name, treeViewItemStyle, e.Data);
                eventTopicItem.DataContext = new ItemInfo("Publication Count: " + e.Data.PublicationCount, "Subscription Count: " + e.Data.SubscriptionCount);

                if (!item.Items.IsEmpty)
                {
                    TreeViewItem tempItem = null;

                    foreach (TreeViewItem temp in item.Items)
                    {
                        if (String.Compare(temp.Header.ToString(), "event topics", StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            tempItem = temp;

                            break;
                        }
                    }

                    if (tempItem != null)
                    {
                        TreeViewItem existItem = GetTreeViewItem(e.Data, tempItem.Items, false);

                        if (existItem != null)
                            return;

                        tempItem.Items.Add(eventTopicItem);

                        return;
                    }
                }

                eventTopicHeader.Items.Add(eventTopicItem);
                item.Items.Add(eventTopicHeader);
            }
        }

        private void WorkItem_Disposed(object sender, EventArgs e)
        {
            WorkItem_Removed(sender, new Utility.DataEventArgs<WorkItem>(sender as WorkItem));
        }

        private void WorkItem_Activated(object sender, EventArgs e)
        {
            Update((WorkItem)sender);
        }

        private void WorkItem_Deactivated(object sender, EventArgs e)
        {
            Update((WorkItem)sender);
        }

        private void WorkItem_IdChanged(object sender, Utility.DataEventArgs<string> e)
        {
            Update((WorkItem)sender);
        }

        #endregion

        #region Workspace Events

        void workspace_SmartPartClosing(object sender, WorkspaceCancelEventArgs e)
        {
            RemoveTreeViewItem(e.SmartPart);
        }

        void workspace_SmartPartActivated(object sender, WorkspaceEventArgs e)
        {
            TreeViewItem item = GetTreeViewItem(sender);
            TreeViewItem smartPartHeader = CreateTreeViewItem("SmartParts", treeViewItemStyle);
            TreeViewItem smartPartItem = CreateTreeViewItem(e.SmartPart.GetType().Name, treeViewItemStyle, e.SmartPart);
            
           if ((!item.Items.IsEmpty))
            {          
                TreeViewItem tempItem = item.Items[0] as TreeViewItem;

                if (tempItem != null && String.Compare(tempItem.Header.ToString(),"smartparts", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    TreeViewItem existItem = GetTreeViewItem(e.SmartPart, item.Items, false);

                    if (existItem != null)
                        return;

                    tempItem.Items.Add(smartPartItem);
                    
                    return;
                }
            }

            smartPartHeader.Items.Add(smartPartItem);
            item.Items.Add(smartPartHeader);
        }

        #endregion

        private void Update(WorkItem wi)
        {
            TreeViewItem node = GetTreeViewItem(wi);

            if (wi.Status == WorkItemStatus.Active)
            {
                node.Style = activeWorkItemStyle;
                TreeViewItem parentItem = node.Parent as TreeViewItem; 
                ExpandParents(parentItem);

                node.IsExpanded = true;
                
            }
            else
                node.Style = workItemStyle;
        }

        private void ExpandParents(TreeViewItem parentItem)
        {
            if (parentItem == null)
                return;

           ExpandParents(parentItem.Parent as TreeViewItem);

           parentItem.IsExpanded = true;
        }
    }

    #region Info Classes

    public class WorkItemInfo
    {
        private string type;
        private string assembly;
 
        public WorkItemInfo(string assembly, string type, bool isController)
        {
            this.type = ((isController) ? "Controller Type: " : "WorkItem Type: ") + type;
            this.assembly = "Assembly: " + assembly;
        }

        public string Type
        {
            get { return type; }
        }

        public string Assembly
        {
            get { return assembly; }
        }
    }

    public class ItemInfo
    {
        private string type;
        private string name;

        public ItemInfo(string name, string type)
        {
            this.name = name;
            this.type = type;
        }

        public string Name
        {
            get { return name; }
        }

        public string Type
        {
            get { return type; }
        }
    }

    #endregion
}
