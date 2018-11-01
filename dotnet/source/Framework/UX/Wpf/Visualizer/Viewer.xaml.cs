using Microsoft.Practices.CompositeUI;
using System.Windows.Controls;
using Utility = Microsoft.Practices.CompositeUI.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Imi.Framework.UX.Wpf.Visualizer
{
    public partial class Viewer : UserControl
    {
        private double scale = 1.0;
        private double panMargin = 50;
        public EventHandler<Utility.DataEventArgs<object>> OnSelectedItem;

        public Viewer(WorkItem workItem)
        {
            InitializeComponent();

            this.Zoom = 1;

            bool loaded = false;

            Loaded += delegate
            {
                if (!loaded)
                {
                    loaded = true;
                    RunWorkItemHelper(workItem);
                }
            };
        }

        private void RunWorkItemHelper(WorkItem workItem)
        {
            Style itemStyle = FindResource("DefaultTreeViewItemStyle") as Style;
            Style activeStyle = FindResource("ActiveTreeViewItemStyle") as Style;
            Style workItemStyle = FindResource("WorkItemTreeViewItemStyle") as Style;

            WorkItemHelper wiHelper = new WorkItemHelper(TreeView, activeStyle, itemStyle, workItemStyle);
            wiHelper.OnSelectedItem += delegate(object sender, Utility.DataEventArgs<object> e)
            {
                if (OnSelectedItem != null)
                    OnSelectedItem(this, new Utility.DataEventArgs<object>(e.Data));
            };

            wiHelper.Run(workItem);
        }

        protected override void OnInitialized(EventArgs e)
        {
           this.SizeChanged += new SizeChangedEventHandler(UserControl_SizeChanged);
           this.Loaded += new RoutedEventHandler(Viewer_Loaded);
            
           base.OnInitialized(e);
        }

        private void Viewer_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateScrollSize();
        }

        public double Scale
        {
            get { return this.scale; }
            set
            {
                if (this.scale != value)
                {
                    scale = value;
                    this.TreeView.LayoutTransform = new ScaleTransform(scale, scale);
                }
            }
        }

        public double Zoom
        {
            get { return ZoomSlider.Value; }
            set
            {
                if (value >= ZoomSlider.Minimum && value <= ZoomSlider.Maximum)
                {
                    this.Scale = value;
                    ZoomSlider.Value = value;
                    UpdateScrollSize();
                }
            }
        }

        private void UpdateScrollSize()
        {
            if (this.ActualWidth == 0 || this.ActualHeight == 0)
                return;

            Size diagramSize = new Size(
                TreeView.ActualWidth * this.Zoom,
                TreeView.ActualHeight * this.Zoom);

            Grid.Width = (this.ActualWidth * 2) + TreeView.Width - panMargin;
            Grid.Height = (this.ActualHeight * 2) + TreeView.Height - panMargin;
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Zoom = e.NewValue;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentWidthChange != 0 &&
                e.ExtentWidthChange != e.ExtentWidth)
            {
                double percent = e.ExtentWidthChange / (e.ExtentWidth - e.ExtentWidthChange);
                double middle = e.HorizontalOffset + (e.ViewportWidth / 2);
                ScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset + (middle * percent));
            }

            if (e.ExtentHeightChange != 0 &&
                e.ExtentHeightChange != e.ExtentHeight)
            {
                double percent = e.ExtentHeightChange / (e.ExtentHeight - e.ExtentHeightChange);
                double middle = e.VerticalOffset + (e.ViewportHeight / 2);
                ScrollViewer.ScrollToVerticalOffset(e.VerticalOffset + (middle * percent));
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateScrollSize();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateScrollSize();
        }
    }
}