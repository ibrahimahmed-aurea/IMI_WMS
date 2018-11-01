using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Imi.Framework.UX.Wpf.Workspaces
{
    public class TransitionWorkspace : ActiproSoftware.Windows.Media.Animation.TransitionPresenter, IWorkspace
    {
        #region IWorkspace Members

        private object activeSmartPart;
        private object smartPartToActivate;
        private List<object> smartParts;
                
        public TransitionWorkspace()
        {
            smartParts = new List<object>();
        }
        
        public void Activate(object smartPart)
        {
            if (smartPart != null)
            {
                if (activeSmartPart != smartPart)
                {
                    activeSmartPart = smartPart;
                    Content = smartPart;
                    RaiseSmartPartActivated(new WorkspaceEventArgs(smartPart));
                }
            }
        }
                          
        public object ActiveSmartPart
        {
            get
            {
                return activeSmartPart;
            }
        }

        public void ApplySmartPartInfo(object smartPart, ISmartPartInfo smartPartInfo)
        {
            
        }

        public void Close(object smartPart)
        {
            if (smartParts.IndexOf(smartPart) >= 0)
            {
                WorkspaceCancelEventArgs args = new WorkspaceCancelEventArgs(smartPart);

                RaiseSmartPartClosing(args);

                if (!args.Cancel)
                {
                    smartParts.Remove(smartPart);
                    
                    RaiseSmartPartClosed(new WorkspaceEventArgs(smartPart));

                    if (smartParts.Count > 0)
                        Activate(smartParts[smartParts.Count - 1]);
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
                        
            if (smartPartToActivate != null)
            {
                Activate(smartPartToActivate);
                smartPartToActivate = null;
            }
        }

        public void Hide(object smartPart)
        {
            if (smartPart is FrameworkElement)
                    (smartPart as FrameworkElement).Visibility = Visibility.Hidden;
        }

        public void Show(object smartPart)
        {
            Show(smartPart, new SmartPartInfo() { Title = "", Description = "" });
        }

        public void Show(object smartPart, ISmartPartInfo smartPartInfo)
        {
            if (!smartParts.Contains(smartPart))
                smartParts.Add(smartPart);

            Activate(smartPart);
        }

        private void SmartPartLoadedEventHandler(object sender, RoutedEventArgs e)
        {
            (sender as FrameworkElement).Loaded -= SmartPartLoadedEventHandler;
            Activate(sender);
        }
        
        public event EventHandler<WorkspaceEventArgs> SmartPartActivated;

        public event EventHandler<WorkspaceCancelEventArgs> SmartPartClosing;

        public event EventHandler<WorkspaceEventArgs> SmartPartClosed;

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

        public ReadOnlyCollection<object> SmartParts
        {
            get
            {
                if (smartParts.Count == 0)
                {
                    return new ReadOnlyCollection<object>(new List<object>());
                }
                else
                {
                    List<object> objects = new List<object>();
                    foreach (object child in smartParts)
                        objects.Add(child);
                    return new ReadOnlyCollection<Object>(objects);
                }
            }
        }
        
        #endregion
    }
}
