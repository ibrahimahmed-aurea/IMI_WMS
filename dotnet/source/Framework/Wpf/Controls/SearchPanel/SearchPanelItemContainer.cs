using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace Imi.Framework.Wpf.Controls
{
    /// <summary>
    /// ========================================
    /// .NET Framework 3.0 Custom Control
    /// ========================================
    ///
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Imi.Framework.Wpf.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Imi.Framework.Wpf.Controls;assembly=Imi.Framework.Wpf.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file. Note that Intellisense in the
    /// XML editor does not currently work on custom controls and its child elements.
    ///
    ///     <MyNamespace:SearchPanelItem/>
    ///
    /// </summary>
    public class SearchPanelItemContainer : Control
    {
        Selector searchItemSelector;

        #region Dependancy Properties

        public SearchPanelItem CurrentItem
        {
            get { return (SearchPanelItem)GetValue(CurrentItemProperty); }
            set { SetValue(CurrentItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentItemProperty =
            DependencyProperty.Register("CurrentItem", typeof(SearchPanelItem), typeof(SearchPanelItemContainer), new UIPropertyMetadata(null));

        public double CaptionWidth
        {
            get { return (double)GetValue(CaptionWidthProperty); }
            set { SetValue(CaptionWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchItemCaptionWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionWidthProperty =
            DependencyProperty.Register("CaptionWidth", typeof(double), typeof(SearchPanel), new UIPropertyMetadata(120.0));



        public bool IsDraggingBefore
        {
            get { return (bool)GetValue(IsDraggingBeforeProperty); }
            set { SetValue(IsDraggingBeforeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDraggingBefore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDraggingBeforeProperty =
            DependencyProperty.Register("IsDraggingBefore", typeof(bool), typeof(SearchPanelItemContainer), new UIPropertyMetadata(false));


        public bool IsDraggingAfter
        {
            get { return (bool)GetValue(IsDraggingAfterProperty); }
            set { SetValue(IsDraggingAfterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDraggingAfter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDraggingAfterProperty =
            DependencyProperty.Register("IsDraggingAfter", typeof(bool), typeof(SearchPanelItemContainer), new UIPropertyMetadata(false));



        #endregion

        private SearchPanel searchPanel;
        private Point? startDragPoint;

        public SearchPanelItemContainer(SearchPanel searchPanel)
        {
            this.searchPanel = searchPanel;
            this.AllowDrop = true;
        }

        static SearchPanelItemContainer()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchPanelItemContainer), new FrameworkPropertyMetadata(typeof(SearchPanelItemContainer)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            searchItemSelector = Template.FindName("PART_SearchItemSelector", this) as Selector;

            if (searchItemSelector != null)
                searchItemSelector.SelectionChanged += SelectionChanged;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                SearchPanelItem item = e.AddedItems[0] as SearchPanelItem;

                if (item.IsRemoveItem)
                {
                    searchPanel.RemoveItem(CurrentItem);
                }
                else if (!item.IsSeparator)
                {
                    CurrentItem.IsActive = false;
                    item.IsActive = true;
                    searchPanel.ChangeItem(CurrentItem, item);
                }
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            IsDraggingAfter = false;
            IsDraggingBefore = false;

            SearchPanelItemContainer container = e.Data.GetData(typeof(SearchPanelItemContainer)) as SearchPanelItemContainer;

            if (container != null && container != this)
            {
                searchPanel.MoveItem(container.CurrentItem, this.CurrentItem);
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            SearchPanelItemContainer container = e.Data.GetData(typeof(SearchPanelItemContainer)) as SearchPanelItemContainer;

            if (container != null && container != this)
            {
                if (searchPanel.Items.IndexOf(this.CurrentItem) > searchPanel.Items.IndexOf(container.CurrentItem))
                {
                    IsDraggingAfter = true;
                    IsDraggingBefore = false;
                }
                else
                {
                    IsDraggingAfter = false;
                    IsDraggingBefore = true;
                }
            }
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            IsDraggingAfter = false;
            IsDraggingBefore = false;
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point position = e.GetPosition(null);

                if (startDragPoint.HasValue && (Math.Abs(position.X - startDragPoint.Value.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - startDragPoint.Value.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    startDragPoint = null;

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
                    }));
                }

            }

            base.OnMouseMove(e);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (CurrentItem.IsMouseOver)
                return;

            startDragPoint = e.GetPosition(null);
        }
    }
}
