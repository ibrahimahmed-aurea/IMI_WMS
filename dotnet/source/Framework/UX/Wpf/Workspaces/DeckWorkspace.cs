using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imi.Framework.UX.Wpf.Workspaces
{
    public class DeckWorkspace : ContentControl, IWorkspace
    {
        private List<UIElement> children;

        //
        // Summary:
        //     Gets a System.Windows.Controls.UIElementCollection of child elements of this
        //     System.Windows.Controls.Panel.
        //
        // Returns:
        //     A System.Windows.Controls.UIElementCollection. The default is an empty System.Windows.Controls.UIElementCollection.
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IList<UIElement> Children
        {
            get
            {
                return children;
            }

        }

        public DeckWorkspace()
            : base()
        {
            children = new List<UIElement>(); // new UIElementCollection(this, this);
            this.Loaded += new RoutedEventHandler(DeckWorkspaceLoaded);
            IsTabStop = false;
        }
                
        void DeckWorkspaceLoaded(object sender, RoutedEventArgs e)
        {
            if (children.Count > 0)
                this.Content = children[children.Count - 1];
            else
            {
                if (this.Content != null)
                {
                    if (this.Content is UIElement)
                        children.Add(this.Content as UIElement);
                    else
                        this.Content = null;
                }
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

        private ISmartPartInfo activeSmartPartInfo;

        public ISmartPartInfo ActiveSmartPartInfo
        {
            get { return activeSmartPartInfo; }
        }

        #region IWorkspace Members

        public void Activate(object smartPart)
        {
            if (children.Contains(smartPart as UIElement))
            {
                this.Content = smartPart;
                RaiseSmartPartActivated(new WorkspaceEventArgs(smartPart));
            }
        }

        public object ActiveSmartPart
        {
            get
            {
                return this.Content;
            }
        }

        public void ApplySmartPartInfo(object smartPart, ISmartPartInfo smartPartInfo)
        {

        }

        public void Close(object smartPart)
        {
            UIElement element = smartPart as UIElement;

            if ((element != null) && (children.Contains(element)))
            {
                WorkspaceCancelEventArgs eventArgs = new WorkspaceCancelEventArgs(element);

                RaiseSmartPartClosing(eventArgs);

                if (!eventArgs.Cancel)
                {
                    int index = children.IndexOf(element);

                    if (index > -1)
                    {
                        while (children.Count > index)
                        {
                            UIElement child = children[children.Count - 1];
                            children.Remove(child);

                            RaiseSmartPartClosed(new WorkspaceEventArgs(child));
                        }

                        if (children.Count > 0)
                        {
                            Activate(children[children.Count - 1]);
                        }
                        else
                        {
                            this.Content = null;
                        }
                    }
                }
            }
        }

        public void Hide(object smartPart)
        {
            if (this.Content == smartPart)
            {
                if (smartPart is FrameworkElement)
                    (smartPart as FrameworkElement).Visibility = Visibility.Hidden;
            }

            int idx = children.IndexOf(smartPart as UIElement);
            if (idx > 0)
            {
                this.Content = children[idx - 1];
            }
        }

        public void Show(object smartPart)
        {
            Show(smartPart, new SmartPartInfo("", ""));
        }

        public void Show(object smartPart, ISmartPartInfo smartPartInfo)
        {

            if (this.Content == smartPart)
            {
                if (smartPart is FrameworkElement)
                    (smartPart as FrameworkElement).Visibility = Visibility.Visible;

                if (this.Content != smartPart)
                {
                    children.Add(smartPart as UIElement);
                    activeSmartPartInfo = smartPartInfo;
                    Activate(smartPart);
                }
            }
            else
            {
                children.Add(smartPart as UIElement);
                activeSmartPartInfo = smartPartInfo;
                Activate(smartPart);
            }
        }

        public event EventHandler<WorkspaceEventArgs> SmartPartActivated;

        public event EventHandler<WorkspaceCancelEventArgs> SmartPartClosing;

        public event EventHandler<WorkspaceEventArgs> SmartPartClosed;

        public ReadOnlyCollection<object> SmartParts
        {
            get
            {
                if (children.Count == 0)
                {
                    return new ReadOnlyCollection<object>(new List<object>());
                }
                else
                {
  //                  return new ReadOnlyCollection<object>(new List<object>() { this.Content });
                    List<object> objects = new List<object>();
                    foreach (UIElement child in children)
                        objects.Add(child);
                    return new ReadOnlyCollection<Object>(objects);
                }
            }
        }

        #endregion
    }
}

