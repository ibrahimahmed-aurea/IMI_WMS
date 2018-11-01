using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.ComponentModel;
using System.Windows.Data;

namespace Imi.Framework.UX.Wpf.Workspaces
{
    public class TabWorkspace : TabControl, IComposableWorkspace<UIElement, SmartPartInfo>
    {
        private WorkspaceComposer<UIElement, SmartPartInfo> composer;
        
        public TabWorkspace()
        { 
            composer = new WorkspaceComposer<UIElement, SmartPartInfo>(this);
        }
                
        private void SelectFirstVisibleTab()
        {
            TabItem item = SelectedItem as TabItem;

            if (item != null && (item.Visibility != Visibility.Visible || !item.IsEnabled))
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    TabItem nextItem = (TabItem)Items[i];
                    if (nextItem.Visibility == Visibility.Visible && nextItem.IsEnabled)
                    {
                        this.SelectedIndex = i;
                        break;
                    }
                }
            }
            
        }
        
        #region IComposableWorkspace<UIElement,SmartPartInfo> Members

        public void OnActivate(UIElement smartPart)
        {
            TabItem item = ResolveSmartPartToTabItem(smartPart);
            item.IsSelected = true;

            if (item.Visibility != Visibility.Visible && smartPart.Visibility == Visibility.Visible)
            {
                item.Visibility = Visibility.Visible;
            }
            else if (item.Visibility == Visibility.Visible && smartPart.Visibility != Visibility.Visible)
            {
                item.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is TabItem)
                {
                    if (ActiveSmartPart != ((TabItem)e.AddedItems[0]).Content)
                        Activate(((TabItem)e.AddedItems[0]).Content);
                }
            }

            base.OnSelectionChanged(e);
        }

        public void OnApplySmartPartInfo(UIElement smartPart, SmartPartInfo smartPartInfo)
        {
            TabItem item = ResolveSmartPartToTabItem(smartPart);
            item.Header = smartPartInfo.Title;
            if (!String.IsNullOrEmpty(smartPartInfo.Description))
            {
                item.ToolTip = smartPartInfo.Description;
            }
        }

        public void OnShow(UIElement smartPart, SmartPartInfo smartPartInfo)
        {
            TabItem item = ResolveSmartPartToTabItem(smartPart);

            if (item == null)
            {
                item = new TabItem();
                item.Content = smartPart;

                item.IsVisibleChanged += (s, e) =>
                {
                    SelectFirstVisibleTab();
                };

                item.IsEnabledChanged += (s, e) =>
                {
                    SelectFirstVisibleTab();
                };
                                
                Items.Add(item);
            }

            UpdateTabIndex();

            OnApplySmartPartInfo(smartPart, smartPartInfo);
                        
            Activate(smartPart);
        }

        private void UpdateTabIndex()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                ((TabItem)Items[i]).TabIndex = i;
            }
        }

        public void OnHide(UIElement smartPart)
        {
            TabItem item = ResolveSmartPartToTabItem(smartPart);
            item.Visibility = Visibility.Hidden;
        }

        public void OnClose(UIElement smartPart)
        {
            TabItem item = ResolveSmartPartToTabItem(smartPart);
            Items.Remove(item);
            UpdateTabIndex();
            RaiseSmartPartClosed(new WorkspaceEventArgs(smartPart));
        }

        public void RaiseSmartPartActivated(WorkspaceEventArgs e)
        {
            if (SmartPartActivated != null)
                SmartPartActivated(this, e);
        }

        public void RaiseSmartPartClosing(WorkspaceCancelEventArgs e)
        {
            if (SmartPartClosing != null)
                SmartPartClosing(this, e);
        }

        public void RaiseSmartPartClosed(WorkspaceEventArgs e)
        {
            if (SmartPartClosed != null)
                SmartPartClosed(this, e);
        }

        public SmartPartInfo ConvertFrom(ISmartPartInfo source)
        {
            return SmartPartInfo.ConvertTo<SmartPartInfo>(source);
        }

        private TabItem ResolveSmartPartToTabItem(UIElement smartPart)
        {
            foreach (TabItem item in Items)
            {
                if (item.Content == smartPart)
                    return item;
            }

            return null;
        }

        #endregion

        public event EventHandler<WorkspaceEventArgs> SmartPartClosed;

        #region IWorkspace Members

        public event EventHandler<WorkspaceCancelEventArgs> SmartPartClosing;

        public event EventHandler<WorkspaceEventArgs> SmartPartActivated;

        public ReadOnlyCollection<object> SmartParts
        {
            get
            {
                return composer.SmartParts;
            }
        }

        public object ActiveSmartPart
        {
            get
            {
                return composer.ActiveSmartPart;
            }
        }

        public void Activate(object smartPart)
        {
            composer.Activate(smartPart);
        }

        public void ApplySmartPartInfo(object smartPart, ISmartPartInfo smartPartInfo)
        {
            composer.ApplySmartPartInfo(smartPart, smartPartInfo);
        }

        public void Close(object smartPart)
        {
            composer.Close(smartPart);
        }

        public void Hide(object smartPart)
        {
            composer.Hide(smartPart);
        }

        public void Show(object smartPart, ISmartPartInfo smartPartInfo)
        {
            composer.Show(smartPart, smartPartInfo);
        }

        public void Show(object smartPart)
        {
            composer.Show(smartPart);
        }

        #endregion
    }
}
