using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Wpf.Controls;
using Imi.Framework.UX.Services;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Imi.SupplyChain.UX.Infrastructure;
using Microsoft.Practices.CompositeUI;
using System.Windows;

namespace Imi.SupplyChain.UX.Shell
{
    public class ShellDrillDownMenuItem : DrillDownMenuItem, IAuthOperation, ICloneable
    {
        private WeakReference _workItemReference;

        public ShellDrillDownMenuItem()
        {
        }
        
        public bool IsAuthorized
        {
            get { return (bool)GetValue(IsAuthorizedProperty); }
            set { SetValue(IsAuthorizedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAuthorized.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAuthorizedProperty =
            DependencyProperty.Register("IsAuthorized", typeof(bool), typeof(ShellDrillDownMenuItem), new UIPropertyMetadata(false));

        public string Operation { get; set; }

        public string EventTopic { get; set; }

        public string AssemblyFile { get; set; }
        
        public string Parameters { get; set; }

        public WorkItem WorkItem
        {
            get
            {
                if (_workItemReference != null)
                {
                    return _workItemReference.Target as WorkItem;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    _workItemReference = new WeakReference(value);
                }
            }
        }
        
        public override object Clone()
        {
            ShellDrillDownMenuItem clone = new ShellDrillDownMenuItem
            {
                Caption = this.Caption,
                Id = this.Id,
                IsBackItem = this.IsBackItem,
                IsEnabled = this.IsEnabled,
                IsFolder = this.IsFolder,
                IsReadOnly = this.IsReadOnly,
                AssemblyFile = this.AssemblyFile,
                IsAuthorized = this.IsAuthorized,
                Operation = this.Operation,
                Parameters = this.Parameters,
                EventTopic = this.EventTopic,
            };


            BindingOperations.ClearAllBindings(clone);

            ObservableCollection<DrillDownMenuItem> children = new ObservableCollection<DrillDownMenuItem>();

            foreach (DrillDownMenuItem child in this.Children)
            {
                DrillDownMenuItem childClone = child.Clone() as DrillDownMenuItem;
                childClone.Parent = clone;
                children.Add(childClone);
            }

            clone.Children = children;

            return clone;
        }

    }
}
