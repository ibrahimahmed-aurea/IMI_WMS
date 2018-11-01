using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xceed.Wpf.DataGrid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using Xceed.Wpf.DataGrid.Views;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Specialized;
using System.Collections;

namespace Imi.Framework.Wpf.Controls
{
    public class ItemVirtualizedEventArgs : EventArgs
    {
        private DependencyObject element;
        private object item;

        public ItemVirtualizedEventArgs(DependencyObject element, object item)
        {
            this.element = element;
            this.item = item;
        }

        public DependencyObject Element
        {
            get
            {
                return element;
            }
        }

        public object Item
        {
            get
            {
                return item;
            }
        }
    }

    public class DataExportEventArgs : EventArgs
    {
        public enum ExportTypes
        {
            Excel
        }

        private ExportTypes _exportType;

        public DataExportEventArgs(ExportTypes exportType)
        {
            _exportType = exportType;
        }

        public ExportTypes ExportType { get { return _exportType; } }
    }

    public delegate void InvokeDelegate(Button toggleDetailButton);

    public class DataGrid : DataGridControl
    {
        public static RoutedCommand ToggleDetailCommand = new RoutedCommand("ToggleDetailCommand", typeof(DataGrid));
        public static RoutedCommand DrillDownCommand = new RoutedCommand("DrillDownCommand", typeof(DataGrid));

        public event EventHandler<ItemVirtualizedEventArgs> ItemVirtualized;
        public event EventHandler<DataExportEventArgs> DataExport;

        private ContentPresenter _detailPresenter;
        private object _selectedItem;

        private DataExportContainer _dataExportContainer = null;



        private RelayCommand _excelExportCommand;

        public RelayCommand ExcelExportCommand
        {
            get
            {
                if (_excelExportCommand == null)
                {
                    _excelExportCommand = new RelayCommand((s) =>
                    {
                        if (_dataExportContainer == null)
                        {
                            if (DataExport != null)
                            {
                                DataExport(this, new DataExportEventArgs(DataExportEventArgs.ExportTypes.Excel));
                            }
                        }
                    });
                }

                return _excelExportCommand;
            }
        }

        public void AppendDataForExport(IList<object> data, IDictionary<string, List<string>> propertiesUsedByImport = null)
        {
            if (_dataExportContainer == null)
            {
                _dataExportContainer = new DataExportContainer(this, propertiesUsedByImport);
                IsExportEnabled = false;

            }

            if (_dataExportContainer != null)
            {
                _dataExportContainer.AppendData(data);
            }
        }

        public void DoExport()
        {
            ExcelHelper.CreateExcelSheet(1, 1, _dataExportContainer);
            _dataExportContainer = null;
             this.Dispatcher.Invoke((Action)(() =>
                {
                    IsExportEnabled = true;
                }));
        }

        public bool IsExportEnabled
        {
            get { return (bool)GetValue(IsExportEnabledProperty); }
            set { SetValue(IsExportEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsExportEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExportEnabledProperty =
            DependencyProperty.Register("IsExportEnabled", typeof(bool), typeof(DataGrid), new UIPropertyMetadata(false));



        public ScrollViewer ScrollViewer
        {
            get { return (ScrollViewer)VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this, 0), 0), 0), 0); }
        }

        public bool IsGroupByAreaVisible
        {
            get { return (bool)GetValue(IsGroupByAreaVisibleProperty); }
            set { SetValue(IsGroupByAreaVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsGroupByAreaVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsGroupByAreaVisibleProperty =
            DependencyProperty.Register("IsGroupByAreaVisible", typeof(bool), typeof(DataGrid), new UIPropertyMetadata(false));

        public UIElement InlineElement
        {
            get { return (UIElement)GetValue(InlineElementProperty); }
            set { SetValue(InlineElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InlineElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InlineElementProperty =
            DependencyProperty.Register("InlineElement", typeof(UIElement), typeof(DataGrid), new UIPropertyMetadata(null));

        public bool IsMultipleItemsSelected
        {
            get { return (bool)GetValue(IsMultipleItemsSelectedProperty); }
            set { SetValue(IsMultipleItemsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMultipleItemsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMultipleItemsSelectedProperty =
            DependencyProperty.Register("IsMultipleItemsSelected", typeof(bool), typeof(DataGrid), new UIPropertyMetadata(false));

        public static readonly RoutedEvent ItemSelectedEvent =
                        EventManager.RegisterRoutedEvent("ItemSelected",
                                                         RoutingStrategy.Bubble,
                                                         typeof(SelectionChangedEventHandler),
                                                         typeof(DataGrid));

        public event SelectionChangedEventHandler ItemSelected
        {
            add { AddHandler(ItemSelectedEvent, value); }
            remove { RemoveHandler(ItemSelectedEvent, value); }
        }

        protected override void OnSelectionChanged(DataGridSelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            IsMultipleItemsSelected = (this.SelectedItems.Count > 1);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (SelectedItem != null)
            {
                if ((e.Key == Key.Enter) || (e.Key == Key.Space))
                {
                    e.Handled = true;
                    RaiseItemSelectedEvent();
                }
            }

            if ((e.Key == Key.Space) && (SelectedItem != null))
            {
                e.Handled = true;
                ToggleDetailCommand.Execute(null, this);
                DrillDownCommand.Execute(null, this);
            }

            base.OnKeyUp(e);
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DataGrid), new UIPropertyMetadata(null));

        static DataGrid()
        {
            DataGrid.ItemsSourceProperty.OverrideMetadata(typeof(DataGrid), new FrameworkPropertyMetadata(null, null, (d, baseValue) =>
            {

                IEnumerable collection = baseValue as IEnumerable;

                if (collection != null)
                {
                    DataGridCollectionView view = new DataGridCollectionView(collection);
                    view.AutoFilterMode = AutoFilterMode.And;

                    return view;
                }
                else
                    return baseValue;

            }
               ));

        }

        public DataGrid()
        {
            this.SelectionChanged += (s, e) =>
                {
                    if (e.OriginalSource == this)
                        LoadInlineElement();
                };

            IsExportEnabled = true;

            //Turn off copy to clipboard
            ClipboardExporters.Clear();
        }

        public List<SortDescription> SavedSortDescriptions = new List<SortDescription>();

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            DataGridCollectionView oldCollectionView = oldValue as DataGridCollectionView;

            bool state = IsGroupByAreaVisible;

            base.OnItemsSourceChanged(oldValue, newValue);

            Cursor = null; //Workaround for sticky wait cursor when filtering

            DataGridCollectionView newCollectionView = newValue as DataGridCollectionView;

            ApplyFilters(oldCollectionView, newCollectionView);

            IsGroupByAreaVisible = state;

            if (newValue == null || newCollectionView.Count == 0)
            {
                IsExportEnabled = false;
            }
            else
            {
                if (_dataExportContainer == null)
                {
                    IsExportEnabled = true;
                }
            }

            //Fix sorting to stick to database sorting when a new search is done
            if (SavedSortDescriptions.Count > 0)
            {
                newCollectionView.SortDescriptions.Clear();
                newCollectionView.SortDescriptions.Add(new SortDescription("SortIndex_GUI", ListSortDirection.Ascending));

                foreach (SortDescription sortDesc in SavedSortDescriptions)
                {
                    newCollectionView.SortDescriptions.Add(sortDesc);
                }

                SavedSortDescriptions.Clear();
            }
            else //Copy sort descriptions when a new partition is added
            {
                if (newCollectionView != null)
                {
                    newCollectionView.SortDescriptions.Clear();

                    if (oldCollectionView != null)
                    {
                        foreach (SortDescription sortDesc in oldCollectionView.SortDescriptions)
                        {
                            newCollectionView.SortDescriptions.Add(sortDesc);
                        }
                    }
                }
            }
        }

        private static void ApplyFilters(DataGridCollectionView oldCollectionView, DataGridCollectionView newCollectionView)
        {
            if (oldCollectionView != null && newCollectionView != null)
            {
                foreach (string key in oldCollectionView.AutoFilterValues.Keys)
                {
                    foreach (object item in oldCollectionView.AutoFilterValues[key])
                    {
                        if (newCollectionView.AutoFilterValues.ContainsKey(key))
                        {
                            try
                            {
                                newCollectionView.AutoFilterValues[key].Add(item);
                            }
                            catch (NotSupportedException)
                            {
                            }
                        }
                    }
                }
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            Cursor = null; //Workaround for sticky wait cursor when filtering
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.GroupConfigurationSelector = new ExpanderGroupConfigurationSelector(this);
        }

        private void CloseGroupByItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement)
            {
                FrameworkElement element = e.OriginalSource as FrameworkElement;

                if ((element.TemplatedParent != null) && (element.TemplatedParent is GroupByItem))
                {
                    GroupByItem groupByItem = element.TemplatedParent as GroupByItem;

                    if (groupByItem.Content is GroupLevelDescription)
                    {
                        GroupLevelDescription closedGroupDescription = groupByItem.Content as GroupLevelDescription;

                        if (sender is DataGridControl)
                        {
                            DataGridControl grid = sender as DataGridControl;

                            ICollectionView collectionView = DataGridCollectionViewSource.GetDefaultView(grid.ItemsSource);

                            if (collectionView != null)
                            {
                                IEnumerable<DataGridGroupDescription> descriptions = from DataGridGroupDescription groupDescription in collectionView.GroupDescriptions
                                                                                     where groupDescription.PropertyName == closedGroupDescription.FieldName
                                                                                     select groupDescription;

                                if ((descriptions != null) && (descriptions.Count() > 0))
                                {
                                    collectionView.GroupDescriptions.Remove(descriptions.First());
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (CurrentItem != null && CurrentItem.Equals(item))
            {
                LoadInlineElement();
            }

            Control dataRow = element as Control;

            if (dataRow != null)
            {
                dataRow.MouseLeftButtonUp -= MouseLeftButtonUpEventHandler;
                dataRow.MouseLeftButtonUp += MouseLeftButtonUpEventHandler;

                Button drillDownButton = dataRow.Template.FindName("PART_DrillDownButton", dataRow) as Button;

                if (drillDownButton != null)
                {
                    drillDownButton.Click -= DrillDownButtonClickEventHandler;
                    drillDownButton.Click += DrillDownButtonClickEventHandler;
                }
            }

            if (ItemVirtualized != null)
            {
                ItemVirtualized(this, new ItemVirtualizedEventArgs(element, item));
            }
        }

        private void DrillDownButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            DrillDownCommand.Execute(null, this);
        }

        private void MouseLeftButtonUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (SelectedItem != null)
            {
                RaiseItemSelectedEvent();
            }
        }

        private void RaiseItemSelectedEvent()
        {
            SelectionChangedEventArgs args = new SelectionChangedEventArgs(ItemSelectedEvent, new List<object>(), this.SelectedItems);

            RaiseEvent(args);
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            DependencyObject parent = e.OriginalSource as DependencyObject;

            while (parent != null)
            {
                if (parent is DataRow)
                {
                    this.SelectedItem = (parent as DataRow).DataContext;
                    this.CurrentItem = SelectedItem;
                    break;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            base.OnMouseRightButtonDown(e);
        }

        private void LoadInlineElement()
        {
            if (InlineElement != null)
            {
                if (_detailPresenter != null)
                    _detailPresenter.Content = null;

                _selectedItem = SelectedItem;

                if (_selectedItem != null)
                {
                    Control container = GetContainerFromItem(_selectedItem) as Control;

                    if (container != null)
                    {
                        _detailPresenter = container.Template.FindName("PART_DetailPresenter", container) as ContentPresenter;

                        if ((_detailPresenter != null) && (_detailPresenter.Content == null))
                        {
                            _detailPresenter.Content = InlineElement;
                        }
                    }
                }
            }
        }
    }
}
