using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace Imi.Framework.Wpf.Controls
{
    public delegate void BreadCrumbEventHandler(object sender, BreadCrumbEventArgs e);

    public class BreadCrumbEventArgs : RoutedEventArgs
    {
        public BreadCrumbEventArgs(RoutedEvent routedEvent, BreadCrumbItem path)
            : base(routedEvent)
        {
            this.BreadCrumbItem = path;
        }

        public BreadCrumbItem BreadCrumbItem { get; set; }
    }

    public delegate void BreadCrumbDirectoryEventHandler(object sender, BreadCrumbDirectoryEventArgs e);

    public class BreadCrumbDirectoryEventArgs : RoutedEventArgs
    {
        public BreadCrumbDirectoryEventArgs(RoutedEvent routedEvent, object newPath)
            : base(routedEvent)
        {
            this.Directory = newPath;
        }

        public object Directory { get; set; }
    }

    public class BreadCrumb : ItemsControl
    {
        /// <summary>
        /// Use to prevent triggering multiple animation chains while
        /// an animation is already running,
        /// </summary>
        private bool _isClosing;
                
        private RelayCommand _breadCrumbHomeCommand;

        public RelayCommand BreadCrumbHomeCommand
        {
            get 
            {
                if (_breadCrumbHomeCommand == null)
                {
                    _breadCrumbHomeCommand = new RelayCommand((s) =>
                        {
                            RaisePreviewDirectoryClosing(Items[0] as BreadCrumbItem);
                        });
                }
                
                return _breadCrumbHomeCommand;
            }
        }

        private RelayCommand _breadCrumbCloseCommand;

        public RelayCommand BreadCrumbCloseCommand
        {
            get 
            {
                if (_breadCrumbCloseCommand == null)
                {
                    _breadCrumbCloseCommand = new RelayCommand((s) =>
                        {
                            if (Items.Count > 1)
                            {
                                RaisePreviewDirectoryClosing(Items[Items.Count - 2] as BreadCrumbItem);
                            }
                        });
                }
                
                return _breadCrumbCloseCommand;
            }
        }
        
        #region Routed Events

        public static readonly RoutedEvent DirectoryChangedEvent = EventManager.RegisterRoutedEvent("DirectoryChangedEvent", RoutingStrategy.Bubble, typeof(BreadCrumbDirectoryEventHandler), typeof(BreadCrumb));

        public event BreadCrumbDirectoryEventHandler DirectoryChanged
        {
            add { AddHandler(DirectoryChangedEvent, value); }
            remove { RemoveHandler(DirectoryChangedEvent, value); }
        }

        public static readonly RoutedEvent PreviewDirectoryCloseToEvent = EventManager.RegisterRoutedEvent("PreviewDirectoryCloseToEvent", RoutingStrategy.Tunnel, typeof(BreadCrumbDirectoryEventHandler), typeof(BreadCrumb));

        public event BreadCrumbDirectoryEventHandler PreviewDirectoryCloseTo
        {
            add { AddHandler(PreviewDirectoryCloseToEvent, value); }
            remove { RemoveHandler(PreviewDirectoryCloseToEvent, value); }
        }

        public static readonly RoutedEvent DirectoryClosedToEvent = EventManager.RegisterRoutedEvent("DirectoryClosedTo", RoutingStrategy.Bubble, typeof(BreadCrumbDirectoryEventHandler), typeof(BreadCrumb));

        public event BreadCrumbDirectoryEventHandler DirectoryClosedTo
        {
            add { AddHandler(DirectoryClosedToEvent, value); }
            remove { RemoveHandler(DirectoryClosedToEvent, value); }
        }


        #endregion

        #region Dependency Properties

        public BreadCrumbItem CurrentDirectory
        {
            get { return (BreadCrumbItem)GetValue(CurrentDirectoryProperty); }
            set { SetValue(CurrentDirectoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentDirectoryProperty =
            DependencyProperty.Register("CurrentDirectory", typeof(BreadCrumbItem), typeof(BreadCrumb), new UIPropertyMetadata(null, CurrentDirectoryChanged));

        public bool IsAtHomeDirectory
        {
            get { return (bool)GetValue(IsAtHomeDirectoryProperty); }
            set { SetValue(IsAtHomeDirectoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAtHomeDirectoryProperty =
            DependencyProperty.Register("IsAtHomeDirectory", typeof(bool), typeof(BreadCrumb), new UIPropertyMetadata(false));

        public BreadCrumbItem TargetDirectory
        {
            get { return (BreadCrumbItem)GetValue(TargetDirectoryProperty); }
            set { SetValue(TargetDirectoryProperty, value); }
        }

        public static readonly DependencyProperty TargetDirectoryProperty =
            DependencyProperty.Register("TargetDirectory", typeof(BreadCrumbItem), typeof(BreadCrumb), new UIPropertyMetadata(null));

        #endregion

        static BreadCrumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BreadCrumb), new FrameworkPropertyMetadata(typeof(BreadCrumb)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.PreviewDirectoryCloseTo += new BreadCrumbDirectoryEventHandler(PreviewDirectoryClose);
        }

        /// <summary>
        /// Tack the changes in CurrentPath so we correctly can update the
        /// IsCurrentItem property on the child Items
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void CurrentDirectoryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BreadCrumb breadCrumb = d as BreadCrumb;

            if (e.NewValue != null)
            {
                BreadCrumbItem path = e.NewValue as BreadCrumbItem;

                foreach (object item in breadCrumb.Items)
                {
                    (item as BreadCrumbItem).IsCurrentItem = false;
                }

                path.IsCurrentItem = true;

                if (breadCrumb.Items.Count > 0)
                    breadCrumb.IsAtHomeDirectory = (path == breadCrumb.Items[0]);
                else
                    breadCrumb.IsAtHomeDirectory = true;
                
                // Don't trigger directory changed event while closing 
                if (!breadCrumb._isClosing)
                    breadCrumb.RaiseDirectoryChanged(path);
            }
        }

        Dictionary<object, BreadCrumbItem> crossRef = new Dictionary<object, BreadCrumbItem>();

        public void AddDirectory(string caption, object directory)
        {
            if (!crossRef.Keys.Contains(directory))
            {
                BreadCrumbItem breadCrumbItem = new BreadCrumbItem() { Header = caption, Tag = directory };

                breadCrumbItem.Clicked += new BreadCrumbEventHandler(BreadCrumbItemClicked);
                CurrentDirectory = breadCrumbItem;
                Items.Add(breadCrumbItem);
                crossRef.Add(directory, breadCrumbItem);
            }
            else
            {
                CloseToDirectory(directory);
            }
        }

        public void CloseToDirectory(object directory)
        {
            BreadCrumbItem breadCrumbItem = null;

            if (crossRef.Keys.Contains(directory))
                breadCrumbItem = crossRef[directory];

            if (breadCrumbItem != null)
            {
                CloseToDirectory(breadCrumbItem);
            }
        }

        /// <summary>
        /// Used for switching directories or for adding a new directory to the list of children
        /// </summary>
        /// <param name="path"></param>
        private void CloseToDirectory(BreadCrumbItem breadCrumbItem)
        {
            if (Items.Contains(breadCrumbItem))
            {
                // This is what we aim for
                TargetDirectory = breadCrumbItem;
                CloseCurrentDirectory();
            }
        }

        private void CloseCurrentDirectory()
        {
            // Trigger animation chain
            if (CurrentDirectory != TargetDirectory)
            {
                _isClosing = true;
                CurrentDirectory.Closed += new BreadCrumbEventHandler(DirectoryCloseComplete);
                CurrentDirectory.Close();
            }
            else
            {
                RaiseDirectoryClosed(TargetDirectory);
            }
        }

        /// <summary>
        /// Triggered when the close animation for a BreadCrumbItem is completed.
        /// The logic triggers the next animation in the chain until TargetPath is
        /// reached.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirectoryCloseComplete(object sender, BreadCrumbEventArgs e)
        {
            e.BreadCrumbItem.Closed -= DirectoryCloseComplete;

            crossRef.Remove(e.BreadCrumbItem.Tag);
            Items.Remove(e.BreadCrumbItem);

            BreadCrumbItem nextInChain = Items[Items.Count - 1] as BreadCrumbItem;

            if (TargetDirectory == nextInChain)
                _isClosing = false;

            CurrentDirectory = nextInChain;
                        
            CloseCurrentDirectory();
        }

        protected virtual void PreviewDirectoryClose(object sender, BreadCrumbDirectoryEventArgs e)
        {
            CloseToDirectory(e.Directory);
            e.Handled = true;
        }
                
        /// <summary>
        /// Triggered when a BreadCrumbItem is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BreadCrumbItemClicked(object sender, BreadCrumbEventArgs e)
        {
            RaisePreviewDirectoryClosing(e.BreadCrumbItem);
        }

        private void RaisePreviewDirectoryClosing(BreadCrumbItem breadCrumbItem)
        {
            BreadCrumbDirectoryEventArgs args = new BreadCrumbDirectoryEventArgs(PreviewDirectoryCloseToEvent, breadCrumbItem.Tag);
            this.RaiseEvent(args);
        }

        private void RaiseDirectoryClosed(BreadCrumbItem breadCrumbItem)
        {
            BreadCrumbDirectoryEventArgs args = new BreadCrumbDirectoryEventArgs(DirectoryClosedToEvent, breadCrumbItem.Tag);
            this.RaiseEvent(args);
        }

        private void RaiseDirectoryChanged(BreadCrumbItem breadCrumbItem)
        {
            BreadCrumbDirectoryEventArgs args = new BreadCrumbDirectoryEventArgs(DirectoryChangedEvent, breadCrumbItem.Tag);
            this.RaiseEvent(args);
        }

    }

}
