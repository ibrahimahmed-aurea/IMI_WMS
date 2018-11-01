using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Imi.Framework.Wpf.Controls
{
    public class DrillDownMenuItem : DependencyObject, ICloneable, IKeyTipElement
    {
        public bool IsFolder { get; set; }
        public ObservableCollection<DrillDownMenuItem> Children { get; set; }
        public string Id { get; set; }

        private DrillDownMenuItem parent;

        public event EventHandler<EventArgs> TreeChanged;

        public virtual void OnTreeChanged()
        {
            var temp = this.TreeChanged;

            if (temp != null)
            {
                temp(this, new EventArgs());
            }
            else
            {
                // Propagate to parent
                if (this.parent != null)
                {
                    parent.OnTreeChanged();
                }
            }
        }

        public DrillDownMenuItem Parent 
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
                AdjustCaptionBinding();
            }
        }

        private void AdjustCaptionBinding()
        {
            if ((isBackItem) && (parent != null))
            {
                DrillDownMenuItem backParent = parent.Parent != null ? parent.Parent : parent;

                BindingOperations.SetBinding(this, DrillDownMenuItem.CaptionProperty, new Binding()
                {
                    Source = backParent,
                    Path = new PropertyPath("Caption")
                });
            }
        }

        private bool isBackItem;

        public bool IsBackItem 
        {
            get
            {
                return isBackItem;
            }
            set
            {
                isBackItem = value;
                AdjustCaptionBinding();
            }
        }

        public string SearchText { get; set; }

        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(DrillDownMenuItem), new UIPropertyMetadata(false));

        public bool IsKeyTipModeActive
        {
            get { return ((bool)GetValue(IsKeyTipModeActiveProperty)); }
            set { SetValue(IsKeyTipModeActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsKeyTipModeActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsKeyTipModeActiveProperty =
            DependencyProperty.Register("IsKeyTipModeActive", typeof(bool), typeof(DrillDownMenuItem), new UIPropertyMetadata(false));


        public string KeyTipAccessText
        {
            get { return (string)GetValue(KeyTipAccessTextProperty); }
            set { SetValue(KeyTipAccessTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeyTipAccessText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyTipAccessTextProperty =
            DependencyProperty.Register("KeyTipAccessText", typeof(string), typeof(DrillDownMenuItem), new UIPropertyMetadata(null));


        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Caption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(DrillDownMenuItem), new UIPropertyMetadata(null, OnCaptionChanged));


        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DrillDownMenuItem), new UIPropertyMetadata(true));

        public DrillDownMenuItem()
        {
            IsEnabled = true;
            IsFolder = false;
            Children = new ObservableCollection<DrillDownMenuItem>();
            Children.CollectionChanged += ChildrenCollectionChanged;
            IsBackItem = false;
            IsReadOnly = true;
            Id = Guid.NewGuid().ToString();
        }

        private void ChildrenCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnTreeChanged();
        }

        private static void OnCaptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DrillDownMenuItem).SearchText = (string)e.NewValue;

            if (e.NewValue != e.OldValue)
            {
                (d as DrillDownMenuItem).OnTreeChanged();
            }
        }

        public virtual object Clone()
        {
            DrillDownMenuItem clone = new DrillDownMenuItem
                                        {
                                            SearchText = this.SearchText,
                                            Caption = this.Caption,
                                            Id = this.Id,
                                            IsBackItem = this.IsBackItem,
                                            IsEnabled = this.IsEnabled,
                                            IsFolder = this.IsFolder,
                                            IsReadOnly = this.IsReadOnly
                                        };

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

        public void Activate(IKeyTipElement child)
        {
            return;
        }

    }
}
