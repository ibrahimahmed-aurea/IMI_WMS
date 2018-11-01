using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.Framework.UX.Wpf.Workspaces
{
    public class GridWorkspace : Grid, IComposableWorkspace<UIElement, GridSmartPartInfo>
    {
        private WorkspaceComposer<UIElement, GridSmartPartInfo> composer;

        public GridWorkspace()
        { 
             composer = new WorkspaceComposer<UIElement, GridSmartPartInfo>(this);
        }

        #region IComposableWorkspace<UIElement,SmartPartInfo> Members

        public void OnActivate(UIElement smartPart)
        {
            smartPart.Visibility = Visibility.Visible;
        }

        public void OnApplySmartPartInfo(UIElement smartPart, GridSmartPartInfo smartPartInfo)
        {
            Grid.SetColumn(smartPart, smartPartInfo.Column);
            Grid.SetRow(smartPart, smartPartInfo.Row);
        }

        public void OnShow(UIElement smartPart, GridSmartPartInfo smartPartInfo)
        {
            Children.Add(smartPart);
            
            OnApplySmartPartInfo(smartPart, smartPartInfo);

            Activate(smartPart);
        }

        public void OnHide(UIElement smartPart)
        {
            smartPart.Visibility = Visibility.Hidden;
        }

        public void OnClose(UIElement smartPart)
        {
            Children.Remove(smartPart);
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

        public GridSmartPartInfo ConvertFrom(ISmartPartInfo source)
        {
            return SmartPartInfo.ConvertTo<GridSmartPartInfo>(source);
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
