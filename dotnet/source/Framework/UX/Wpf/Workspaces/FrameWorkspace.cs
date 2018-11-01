using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.Windows.Input;

namespace Imi.Framework.UX.Wpf.Workspaces
{
    public class FrameWorkspace : Frame, IComposableWorkspace<UIElement, SmartPartInfo>
    {
        private WorkspaceComposer<UIElement, SmartPartInfo> composer;
        private ReadOnlyObservableCollection<NavigationEntry> workspaceDataCollection;
        private ObservableCollection<NavigationEntry> internalWorkspaceDataCollection;
                
        public FrameWorkspace()
        {
            composer = new WorkspaceComposer<UIElement, SmartPartInfo>(this);
            internalWorkspaceDataCollection = new ObservableCollection<NavigationEntry>();
            workspaceDataCollection = new ReadOnlyObservableCollection<NavigationEntry>(internalWorkspaceDataCollection);
            base.Navigated += new System.Windows.Navigation.NavigatedEventHandler(NavigatedEventHandler);
        }
        
        public ReadOnlyObservableCollection<NavigationEntry> WorkspaceData
        {
            get
            {   
                return workspaceDataCollection;
            }
        }

        private void NavigatedEventHandler(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Content is UIElement)
            {
                foreach (NavigationEntry data in internalWorkspaceDataCollection)
                {
                    if (data.SmartPart == e.Content)
                        data.IsActive = true;
                    else
                        data.IsActive = false;
                }
            }
            
            Activate(e.Content);
        }
        
        #region IComposableWorkspace<UIElement,SmartPartInfo> Members
        
        public void OnActivate(UIElement smartPart)
        {
            if (this.Content != smartPart)
                Navigate(smartPart);
        }
                
        public void OnApplySmartPartInfo(UIElement smartPart, SmartPartInfo smartPartInfo)
        {
            foreach (NavigationEntry data in internalWorkspaceDataCollection)
            {
                if (data.SmartPart == smartPart)
                {
                    data.SmartPartInfo = smartPartInfo;
                    break;
                }
            }
        }

        public void OnShow(UIElement smartPart, SmartPartInfo smartPartInfo)
        {
            NavigationEntry data = new NavigationEntry();
            data.SmartPart = smartPart;
            data.SmartPartInfo = smartPartInfo;
            internalWorkspaceDataCollection.Add(data);
            
            Activate(smartPart);
        }

        public void OnHide(UIElement smartPart)
        {
            if (composer.ActiveSmartPart == smartPart)
                this.Navigate(null);
        }

        public void OnClose(UIElement smartPart)
        {
            OnHide(smartPart);
            
            NavigationEntry dataToRemove = null;

            foreach (NavigationEntry data in internalWorkspaceDataCollection)
            {
                if (data.SmartPart == smartPart)
                {
                    dataToRemove = data;
                    break;
                }
            }
            
            if (dataToRemove == null)
                internalWorkspaceDataCollection.Remove(dataToRemove);
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

        public SmartPartInfo ConvertFrom(ISmartPartInfo source)
        {
            return SmartPartInfo.ConvertTo<SmartPartInfo>(source);
        }

        #endregion

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
