using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media;
using ActiproSoftware.Windows.Controls.Docking;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.Framework.UX.Wpf.Workspaces
{
    public class DockWorkspace : DockSite, IComposableWorkspace<UIElement, SmartPartInfo>
    {
        private WorkspaceComposer<UIElement, SmartPartInfo> _composer;
        private object activeSmartPart;
        private bool _isClosing;
        private bool _isLoading;

        public void BeginLoadLayout()
        {
            _isLoading = true;
        }

        public void EndLoadLayout()
        {
            _isLoading = false;
        }

        public DockWorkspace()
        {
            _composer = new WorkspaceComposer<UIElement, SmartPartInfo>(this);

            Workspace workspace = new Workspace();
            workspace.Background = null;
            
            TabbedMdiHost host = new TabbedMdiHost();
            workspace.Content = host;

            TabbedMdiContainer container = new TabbedMdiContainer();
            host.Content = container;

            this.Content = workspace;
                                    
            this.WindowClosing += (s, e) =>
                {
                    if (!_isLoading)
                    {
                        WorkspaceCancelEventArgs args = new WorkspaceCancelEventArgs(e.Window.Content);
                        RaiseSmartPartClosing(args);
                        e.Cancel = args.Cancel;
                    }
                };

            this.WindowClosed += (s, e) =>
                {
                    if (!_isLoading)
                    {
                        _isClosing = true;
                        Close(e.Window.Content);
                        _isClosing = false;
                    }
                };

            this.WindowActivated += (s, e) =>
                {
                    Activate(e.Window.Content);
                };
        }
                
        #region IComposableWorkspace<UIElement,SmartPartInfo> Members

        public void OnActivate(UIElement smartPart)
        {
            activeSmartPart = smartPart;

            DocumentWindow window = ResolveSmartPartToWindow(smartPart);
            window.Activate(true);

            if (window.Visibility != Visibility.Visible)
                window.Visibility = Visibility.Visible;
        }
                
        public void OnApplySmartPartInfo(UIElement smartPart, SmartPartInfo smartPartInfo)
        {
            DocumentWindow window = ResolveSmartPartToWindow(smartPart);
            window.Title = smartPartInfo.Title;
            
            if (!string.IsNullOrEmpty(smartPartInfo.Description))
            {
                window.ToolTip = smartPartInfo.Description;
            }
        }

        public void OnShow(UIElement smartPart, SmartPartInfo smartPartInfo)
        {
            DocumentWindow window = ResolveSmartPartToWindow(smartPart);
            
            if (window == null)
            {
                window = new DocumentWindow(this, string.Format("Window{0}", this.DocumentWindows.Count), smartPartInfo.Title, null, smartPart);
            }
                        
            OnApplySmartPartInfo(smartPart, smartPartInfo);

            Activate(smartPart);
        }
        
        public void OnHide(UIElement smartPart)
        {
            DocumentWindow window = ResolveSmartPartToWindow(smartPart);
            window.Visibility = Visibility.Collapsed;
        }

        public void OnClose(UIElement smartPart)
        {
            if (activeSmartPart == smartPart)
                activeSmartPart = null;

            if (!_isClosing)
            {
                DocumentWindow window = ResolveSmartPartToWindow(smartPart);

                if (window != null)
                    window.Close();
            }
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

        private DocumentWindow ResolveSmartPartToWindow(UIElement smartPart)
        {
            foreach (DocumentWindow window in this.DocumentWindows.ToArray(false))
            {
                if (window.Content == smartPart)
                    return window;
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
                return _composer.SmartParts;
            }
        }

        public object ActiveSmartPart
        {
            get
            {
                return _composer.ActiveSmartPart;
            }
        }

        public void Activate(object smartPart)
        {
            _composer.Activate(smartPart);
        }

        public void ApplySmartPartInfo(object smartPart, ISmartPartInfo smartPartInfo)
        {
            _composer.ApplySmartPartInfo(smartPart, smartPartInfo);
        }

        public void Close(object smartPart)
        {
            _composer.Close(smartPart);
            RaiseSmartPartClosed(new WorkspaceEventArgs(smartPart));
        }

        public void Hide(object smartPart)
        {
            _composer.Hide(smartPart);
        }

        public void Show(object smartPart, ISmartPartInfo smartPartInfo)
        {
            _composer.Show(smartPart, smartPartInfo);
        }

        public void Show(object smartPart)
        {
            _composer.Show(smartPart);
        }

        #endregion
    }
}
