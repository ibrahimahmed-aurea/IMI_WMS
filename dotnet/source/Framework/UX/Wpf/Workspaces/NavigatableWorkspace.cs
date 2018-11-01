using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.Windows;
using System.Windows.Controls;

namespace Imi.Framework.UX.Wpf.Workspaces
{
    public class NavigationEntry : DependencyObject
    {
        private ISmartPartInfo smartPartInfo;
        
        public bool IsActive
        {
            get
            {
                return (bool)GetValue(IsActiveProperty);
            }
            internal set
            {
                SetValue(IsActiveProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(NavigationEntry), new UIPropertyMetadata(false));
        
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(NavigationEntry), new UIPropertyMetadata(""));


        public ISmartPartInfo SmartPartInfo
        {
            get
            { 
                return smartPartInfo; 
            }
            set
            {
                smartPartInfo = value;
                Title = smartPartInfo.Title;
            }
        }

        private object smartPart;

        public object SmartPart
        {
            get { return smartPart; }
            set { smartPart = value; }
        }
              
    }

    public class NavigatableWorkspace<TWorkspace> : ContentControl, IWorkspace
        where TWorkspace: IWorkspace, new()
    {
        private IWorkspace workspace;
        private List<NavigationEntry> historyList;
        private ReadOnlyObservableCollection<NavigationEntry> readOnlyNavigationEntryCollection;
        private ObservableCollection<NavigationEntry> navigationEntryCollection;
        private int activeEntryIndex;
        
        public bool CanGoBack
        {
            get { return (bool)GetValue(CanGoBackProperty); }
            set { SetValue(CanGoBackProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanGoBack.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanGoBackProperty =
            DependencyProperty.Register("CanGoBack", typeof(bool), typeof(NavigatableWorkspace<TWorkspace>), new UIPropertyMetadata(false));
        
        public bool CanGoForward
        {
            get { return (bool)GetValue(CanGoForwardProperty); }
            set { SetValue(CanGoForwardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanGoForward.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanGoForwardProperty =
            DependencyProperty.Register("CanGoForward", typeof(bool), typeof(NavigatableWorkspace<TWorkspace>), new UIPropertyMetadata(false));
        
        public NavigatableWorkspace()
        {
            workspace = new TWorkspace();
            this.Content = workspace;
            workspace.SmartPartActivated += new EventHandler<WorkspaceEventArgs>(SmartPartActivatedEventHandler);
            workspace.SmartPartClosing += new EventHandler<WorkspaceCancelEventArgs>(SmartPartClosingEventHandler);
            historyList = new List<NavigationEntry>();
            navigationEntryCollection = new ObservableCollection<NavigationEntry>();
            readOnlyNavigationEntryCollection = new ReadOnlyObservableCollection<NavigationEntry>(navigationEntryCollection);
        }

        public IWorkspace Workspace
        {
            get
            {
                return workspace;
            }
        }

        public void GoBack()
        {
            if (CanGoBack)
            {
                Activate(historyList[activeEntryIndex - 1].SmartPart);
            }
        }

        public void GoForward()
        {
            if (CanGoForward)
            {
                Activate(historyList[activeEntryIndex + 1].SmartPart);
            }
        }

        private void SmartPartClosingEventHandler(object sender, WorkspaceCancelEventArgs e)
        {
            NavigationEntry entry = FindEntryForSmartPart(e.SmartPart);
            historyList.Remove(entry);
            navigationEntryCollection.Remove(entry);

            if (historyList.Count > 1)
            {
                Activate(historyList[0].SmartPart);
            }
            else
            {
                if (historyList.Count > 0)
                    ActivateEntry(historyList[0]);
            }
            
            if (SmartPartClosing != null)
                SmartPartClosing(this, e);
        }

        private void SmartPartActivatedEventHandler(object sender, WorkspaceEventArgs e)
        {
            NavigationEntry entry = FindEntryForSmartPart(e.SmartPart);

            if (entry == null)
            {
                entry = new NavigationEntry();
                entry.SmartPart = e.SmartPart;

                historyList.Add(entry);
                navigationEntryCollection.Insert(0, entry);
            }

            ActivateEntry(entry);

            if (SmartPartActivated != null)
                SmartPartActivated(this, e);
        }

        public ReadOnlyObservableCollection<NavigationEntry> NavigationEntries
        {
            get
            {
                return readOnlyNavigationEntryCollection;    
            }
        }

        public void Activate(object smartPart)
        { 
            workspace.Activate(smartPart);
        }

        public void Close(object smartPart)
        {
            workspace.Close(smartPart);
        }

        public void ApplySmartPartInfo(object smartPart, ISmartPartInfo smartPartInfo)
        {
            workspace.ApplySmartPartInfo(smartPart, smartPartInfo);

            NavigationEntry entry = FindEntryForSmartPart(smartPart);
            entry.SmartPartInfo = smartPartInfo;
        }

        private void ActivateEntry(NavigationEntry entry)
        {
            activeEntryIndex = 0;
            
            for (int i = 0; i < historyList.Count; i++)
            {
                if (historyList[i] == entry)
                {
                    historyList[i].IsActive = true;
                    activeEntryIndex = i;
                }
                else
                    historyList[i].IsActive = false;
            }

            if (activeEntryIndex > 0)
                CanGoBack = true;
            else
                CanGoBack = false;

            if (activeEntryIndex < historyList.Count - 1)
                CanGoForward = true;
            else
                CanGoForward = false;
        }

        private NavigationEntry FindEntryForSmartPart(object smartPart)
        {
            foreach (NavigationEntry entry in historyList)
            {
                if (entry.SmartPart == smartPart)
                    return entry;
            }
            
            return null;
        }
                        
        public object ActiveSmartPart
        {
            get
            {
                return workspace.ActiveSmartPart;
            }
        }

        public void Hide(object smartPart)
        {
            workspace.Hide(smartPart);
        }

        public void Show(object smartPart)
        {
            workspace.Show(smartPart);

            if (smartPart is ISmartPartInfoProvider)
            {
                ISmartPartInfo info = (smartPart as ISmartPartInfoProvider).GetSmartPartInfo(typeof(SmartPartInfo));
                ApplySmartPartInfo(smartPart, info);
            }
        }

        public void Show(object smartPart, ISmartPartInfo smartPartInfo)
        {
            workspace.Show(smartPart, smartPartInfo);

            ApplySmartPartInfo(smartPart, smartPartInfo);
        }

        public event EventHandler<WorkspaceEventArgs> SmartPartActivated;

        public event EventHandler<WorkspaceCancelEventArgs> SmartPartClosing;

        public ReadOnlyCollection<object> SmartParts
        {
            get
            {
                return workspace.SmartParts;
            }
        }
    }
}
