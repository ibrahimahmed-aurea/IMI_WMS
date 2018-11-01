using System;
using System.Linq;
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
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.ComponentModel;

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
    ///     <MyNamespace:SearchPanel/>
    ///
    /// </summary>
    public class SearchPanel : ItemsControl
    {
        private ComboBox _criteriaCreator;
        private DispatcherTimer _searchTimer;
        private RelayCommand _searchCommand;

        public RelayCommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand((s) =>
                    {
                        RaiseEvent(new SearchExecutedEventArgs(SearchExecutedEvent, SearchAction.Search));
                    });
                }

                return _searchCommand;
            }
        }

        private RelayCommand _clearCommand;

        public RelayCommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new RelayCommand((s) =>
                    {
                        RaiseEvent(new SearchExecutedEventArgs(SearchExecutedEvent, SearchAction.Clear));
                    });
                }

                return _clearCommand;
            }
        }
                
        private RelayCommand _searchRepeatCommand;

        public RelayCommand SearchRepeatCommand
        {
            get
            {
                if (_searchRepeatCommand == null)
                {
                    _searchRepeatCommand = new RelayCommand((p) =>
                    {
                        SetSearchRepeatInterval(Convert.ToInt32(p.ToString()));   
                    });
                }

                return _searchRepeatCommand;
            }
        }
        
        #region Published Events

        public static readonly RoutedEvent SearchExecutedEvent =
                        EventManager.RegisterRoutedEvent("SearchExecuted",
                                                         RoutingStrategy.Bubble,
                                                         typeof(SearchExecutedEventHandler),
                                                         typeof(SearchPanel));

        public event SearchExecutedEventHandler SearchExecuted
        {
            add { AddHandler(SearchExecutedEvent, value); }
            remove { RemoveHandler(SearchExecutedEvent, value); }
        }

        #endregion
                
        private void SetSearchRepeatInterval(int interval)
        {
            if (_searchTimer != null)
            {
                if (interval == 0)
                {
                    Image repeatIcon = Template.FindName("RepetedSearchIcon", this) as Image;
                    repeatIcon.Visibility = System.Windows.Visibility.Collapsed;
                    _searchTimer.Stop();
                }
                else
                {
                    Image repeatIcon = Template.FindName("RepetedSearchIcon", this) as Image;
                    repeatIcon.Visibility = System.Windows.Visibility.Visible;
                    _searchTimer.Stop();
                    _searchTimer.Interval = new TimeSpan(0, interval, 0);
                    _searchTimer.Start();
                }
            }
        }
                        
        static SearchPanel()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchPanel), new FrameworkPropertyMetadata(typeof(SearchPanel)));
        }

        public SearchPanel()
        {
            _searchTimer = new DispatcherTimer();
            _searchTimer.Tick += new EventHandler(SearchTimerTickEventHandler);

            SetValue(AvailableItemsPropertyKey, new ObservableCollection<SearchPanelItem>());
            SetValue(AvailableItemsPlusRemovePropertyKey, new ObservableCollection<SearchPanelItem>());
        }

        private void SearchTimerTickEventHandler(object sender, EventArgs e)
        {
            _searchCommand.Execute(null);
        }

        public double SearchItemCaptionWidth
        {
            get { return (double)GetValue(SearchItemCaptionWidthProperty); }
            set { SetValue(SearchItemCaptionWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchItemCaptionWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchItemCaptionWidthProperty =
            DependencyProperty.Register("SearchItemCaptionWidth", typeof(double), typeof(SearchPanel), new UIPropertyMetadata(null));


        // Using a DependencyProperty as the backing store for AvailableItemsPlusRemove.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey AvailableItemsPlusRemovePropertyKey =
            DependencyProperty.RegisterReadOnly("AvailableItemsPlusRemove", typeof(ObservableCollection<SearchPanelItem>), typeof(SearchPanel), null);

        public static readonly DependencyProperty AvailableItemsPlusRemoveProperty = AvailableItemsPlusRemovePropertyKey.DependencyProperty;

        public ObservableCollection<SearchPanelItem> AvailableItemsPlusRemove
        {
            get { return (ObservableCollection<SearchPanelItem>)GetValue(AvailableItemsPlusRemoveProperty); }
            set { SetValue(AvailableItemsPlusRemoveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvailableItems.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey AvailableItemsPropertyKey =
            DependencyProperty.RegisterReadOnly("AvailableItems", typeof(ObservableCollection<SearchPanelItem>), typeof(SearchPanel), null);

        public static readonly DependencyProperty AvailableItemsProperty = AvailableItemsPropertyKey.DependencyProperty;

        public ObservableCollection<SearchPanelItem> AvailableItems
        {
            get { return (ObservableCollection<SearchPanelItem>)GetValue(AvailableItemsProperty); }
            set { SetValue(AvailableItemsProperty, value); }
        }

        protected override bool ShouldApplyItemContainerStyle(DependencyObject container, object item)
        {
            return true;
        }

        public bool CanAddItems
        {
            get { return (bool)GetValue(CanAddItemsProperty); }
            set { SetValue(CanAddItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanAddItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanAddItemsProperty =
            DependencyProperty.Register("CanAddItems", typeof(bool), typeof(SearchPanel), new UIPropertyMetadata(false));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _criteriaCreator = Template.FindName("PART_CriteriaCreator", this) as ComboBox;
            _criteriaCreator.SelectionChanged += new SelectionChangedEventHandler(CriteriaCreatorSelectionChangedEventHandler);

            UpdateItems();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                
                if (!ControlHelper.IsControlOnView(e.OriginalSource, this))
                {
                    return;
                }

                if (e.OriginalSource is TextBox)
                {
                    UpdateTextBoxBinding(e.OriginalSource as TextBox);
                }

                e.Handled = true;
                _searchCommand.Execute(null);
            }
            base.OnPreviewKeyDown(e);
        }

        private void UpdateTextBoxBinding(TextBox textBox)
        {
            BindingExpression expression = textBox.GetBindingExpression(TextBox.TextProperty);

            if (expression != null)
                expression.UpdateSource();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SearchPanelItemContainer(this);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is SearchPanelItemContainer;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            SearchPanelItemContainer container = element as SearchPanelItemContainer;

            SearchPanelItem searchItem = item as SearchPanelItem;

            container.CurrentItem = searchItem;
            searchItem.Container = container;

            if (searchItem.IsActive)
                container.Visibility = Visibility.Visible;
            else
                container.Visibility = Visibility.Collapsed;
        }

        public void SaveUserSettings(SearchPanelSettingsRepository settings)
        {
            settings.Items = new SearchPanelItemSetting[Items.Count];

            for (int i = 0; i < Items.Count; i++)
            {
                SearchPanelItem item = Items[i] as SearchPanelItem;

                if (item != null)
                {
                    SearchPanelItemSetting setting = new SearchPanelItemSetting();

                    setting.Name = item.Name;
                    setting.IsActive = item.IsActive;
                    setting.Sequence = i;
                    settings.Items[i] = setting;
                }
            }
        }

        public void LoadUserSettings(SearchPanelSettingsRepository settings)
        {
            if (settings.Items != null)
            {
                var internalItems = new List<SearchPanelItemSetting>(settings.Items);

                for (int i = 0; i < Items.Count; i++)
                {
                    SearchPanelItem item = Items[i] as SearchPanelItem;

                    if (item != null)
                    {
                        var n = from x in settings.Items
                                where x.Name == item.Name
                                select x;

                        if (n.Count() == 0)
                        {
                            SearchPanelItemSetting setting = new SearchPanelItemSetting();
                            setting.Name = item.Name;
                            setting.IsActive = item.IsActive;
                            setting.Sequence = i;
                            internalItems.Add(setting);
                        }
                    }
                }

                IList<SearchPanelItem> items = new List<SearchPanelItem>(Items.Cast<SearchPanelItem>());

                Items.Clear();

                foreach (SearchPanelItemSetting setting in internalItems.OrderBy<SearchPanelItemSetting, int>(x => x.Sequence))
                {
                    var n = (from x in items
                             where x.Name == setting.Name
                             select x).FirstOrDefault();

                    if (n != null)
                    {
                        n.IsActive = setting.IsActive;
                        Items.Add(n);
                    }
                }
            }

            UpdateItems();
        }

        private void UpdateItems()
        {
            AvailableItems.Clear();
            AvailableItemsPlusRemove.Clear();

            if (Items != null)
            {
                foreach (object item in Items)
                {
                    SearchPanelItem searchItem = item as SearchPanelItem;

                    if (searchItem.IsActive)
                    {
                        if (searchItem.Container != null)
                            searchItem.Container.Visibility = Visibility.Visible;

                        InitializeElement(searchItem);
                    }
                    else
                    {
                        AvailableItems.Add(searchItem);
                        AvailableItemsPlusRemove.Add(searchItem);

                        if (searchItem.Container != null)
                            searchItem.Container.Visibility = Visibility.Collapsed;
                    }
                }

                SearchPanelItem separatorItem = new SearchPanelItem();
                separatorItem.IsSeparator = true;

                AvailableItemsPlusRemove.Add(separatorItem);

                SearchPanelItem removeItem = new SearchPanelItem();
                removeItem.IsRemoveItem = true;

                AvailableItemsPlusRemove.Add(removeItem);
            }

            CanAddItems = (AvailableItems.Count > 0);
        }

        private void InitializeElement(DependencyObject element)
        {
            if (element != null)
            {
                if (element is NullableComboBox)
                {
                    NullableComboBox comboBox = element as NullableComboBox;

                    if (comboBox != null && !comboBox.IsNullable)
                    {
                        comboBox.SelectFirstItem = true;
                    }
                }
                else if (element is Label)
                    return;

                foreach (object child in LogicalTreeHelper.GetChildren(element))
                {
                    InitializeElement(child as DependencyObject);
                }
            }
        }

        private void CriteriaCreatorSelectionChangedEventHandler(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                SearchPanelItem searchItem = e.AddedItems[0] as SearchPanelItem;

                searchItem.IsActive = true;

                Items.Remove(searchItem);
                Items.Add(searchItem);

                UpdateItems();
            }
        }

        internal void RemoveItem(SearchPanelItem item)
        {
            if (item.Content is DependencyObject)
                NullElement(item.Content as DependencyObject);

            item.IsActive = false;

            UpdateItems();
        }

        public void ClearItems()
        {
            foreach (object item in Items)
            {
                SearchPanelItem searchItem = item as SearchPanelItem;

                if (searchItem != null)
                {
                    if (searchItem.Content is DependencyObject)
                        NullElement(searchItem.Content as DependencyObject);
                }
            }
        }

        private void NullElement(DependencyObject element)
        {
            if (element is Selector)
                (element as Selector).SelectedValue = null;
            else if (element is ToggleButton)
                (element as ToggleButton).IsChecked = null;
            else if (element is TextBox)
            {
                (element as TextBox).Text = null;
                UpdateTextBoxBinding(element as TextBox);
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(element, i);

                NullElement(child);
            }
        }

        internal void MoveItem(SearchPanelItem source, SearchPanelItem target)
        {
            var copy = (from object i in Items
                        select i).ToList();

            int sourceIndex = copy.IndexOf(source);

            copy.Remove(source);

            int targetIndex = copy.IndexOf(target);

            if (targetIndex >= sourceIndex)
                targetIndex++;

            copy.Insert(targetIndex, source);

            Items.Clear();

            foreach (object i in copy)
                Items.Add(i);

            UpdateItems();
        }

        internal void ChangeItem(SearchPanelItem source, SearchPanelItem target)
        {
            var copy = (from object i in Items
                        select i).ToList();

            int targetIndex = copy.IndexOf(target);
            int sourceIndex = copy.IndexOf(source);

            copy[sourceIndex] = target;
            copy[targetIndex] = source;

            Items.Clear();

            foreach (object i in copy)
                Items.Add(i);

            UpdateItems();
        }
    }
}
