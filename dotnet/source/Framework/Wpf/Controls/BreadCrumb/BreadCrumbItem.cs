using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imi.Framework.Wpf.Controls
{
    public class BreadCrumbItem : HeaderedContentControl
    {
        private FrameworkElement _animator;
        private MarginLeftAnimation _addAnimation { get; set; }
        private MarginLeftAnimation _closeAnimation { get; set; }
                
        private RelayCommand _breadCrumbItemClickCommand;

        public RelayCommand BreadCrumbItemClickCommand
        {
            get
            {
                if (_breadCrumbItemClickCommand == null)
                {
                    _breadCrumbItemClickCommand = new RelayCommand((s) =>
                    {
                        BreadCrumbEventArgs args = new BreadCrumbEventArgs(BreadCrumbItemClickedEvent, this);
                        RaiseEvent(args);
                    });
                }
                
                return _breadCrumbItemClickCommand;
            }
        }

        #region Routed Events

        public static readonly RoutedEvent BreadCrumbItemClickedEvent = EventManager.RegisterRoutedEvent("BreadCrumbItemClicked", RoutingStrategy.Bubble, typeof(BreadCrumbEventHandler), typeof(BreadCrumbItem));

        public event BreadCrumbEventHandler Clicked
        {
            add { AddHandler(BreadCrumbItemClickedEvent, value); }
            remove { RemoveHandler(BreadCrumbItemClickedEvent, value); }
        }

        public static readonly RoutedEvent BreadCrumbItemClosedEvent = EventManager.RegisterRoutedEvent("BreadCrumbItemClosed", RoutingStrategy.Bubble, typeof(BreadCrumbEventHandler), typeof(BreadCrumbItem));

        public event BreadCrumbEventHandler Closed
        {
            add { AddHandler(BreadCrumbItemClosedEvent, value); }
            remove { RemoveHandler(BreadCrumbItemClosedEvent, value); }
        }

        #endregion

        #region Dependency Properties

        public bool IsCurrentItem
        {
            get { return (bool)GetValue(IsCurrentItemProperty); }
            set { SetValue(IsCurrentItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCurrentItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCurrentItemProperty =
            DependencyProperty.Register("IsCurrentItem", typeof(bool), typeof(BreadCrumbItem), new UIPropertyMetadata(false));


        public bool IsClosing
        {
            get { return (bool)GetValue(IsClosingProperty); }
            set { SetValue(IsClosingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsClosing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsClosingProperty =
            DependencyProperty.Register("IsClosing", typeof(bool), typeof(BreadCrumbItem), new UIPropertyMetadata(false));


        #endregion

        static BreadCrumbItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BreadCrumbItem), new FrameworkPropertyMetadata(typeof(BreadCrumbItem)));
        }

        /// <summary>
        /// Hook up animations
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _animator = this.Template.FindName("PART_Animator", this) as FrameworkElement;
            _addAnimation = this.Template.Resources["PART_AddAnimation"] as MarginLeftAnimation;
            _closeAnimation = this.Template.Resources["PART_CloseAnimation"] as MarginLeftAnimation;
            
            this.Loaded += BreadCrumbItemLoaded;
        }

        private void BreadCrumbItemLoaded(object sender, RoutedEventArgs e)
        {
            if (AnimatesAdd)
            {
                MarginLeftAnimation addAnimation = _addAnimation.Clone() as MarginLeftAnimation;
                addAnimation.TranslationDistance = _animator.ActualWidth;
                _animator.BeginAnimation(StackPanel.MarginProperty, addAnimation);
            }
        }

        public bool AnimatesClose
        {
            get
            {
                return ((_animator != null) && (_closeAnimation != null));
            }
        }

        public bool AnimatesAdd
        {
            get
            {
                return ((_animator != null) && (_addAnimation != null));
            }
        }

        public void Close()
        {
            if (AnimatesClose)
            {
                IsClosing = true;
                MarginLeftAnimation closeAnimation = _closeAnimation.Clone() as MarginLeftAnimation;
                closeAnimation.TranslationDistance = _animator.ActualWidth;
                closeAnimation.Completed += new EventHandler(CloseCompleted);
                _animator.BeginAnimation(StackPanel.MarginProperty, closeAnimation);
            }
            else
            {
                CloseCompleted(this, null);
            }
        }

        private void CloseCompleted(object sender, EventArgs e)
        {
            IsClosing = false;
            BreadCrumbEventArgs args = new BreadCrumbEventArgs(BreadCrumbItemClosedEvent, this);
            RaiseEvent(args);
        }
    }
}
