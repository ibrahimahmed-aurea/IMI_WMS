using System;
using System.Linq;
using System.Collections;
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
using System.ComponentModel;
using System.Windows.Threading;
using ActiproSoftware.Windows.Media.Animation;

namespace Imi.Framework.Wpf.Controls
{
    public interface IKeyTipSelector
    {
        void SetKeyTips(IEnumerable menuItems);
    }

    public interface IKeyTipElement
    {
        bool IsKeyTipModeActive { get; set; }
        string KeyTipAccessText { get; set; }
        void Activate(IKeyTipElement child);
    }

    public delegate void MenuItemSelectedEventHandler(object sender, MenuItemSelectedEventArgs e);

    public class MenuItemSelectedEventArgs : RoutedEventArgs
    {
        private object _selectedItem;

        public MenuItemSelectedEventArgs(RoutedEvent routedEvent, object selectedItem)
            : base(routedEvent)
        {
            this._selectedItem = selectedItem;
        }

        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; }
        }
    }

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
    ///     xmlns:MyNamespace="clr-namespace:Imi.Framework.UX.Wpf.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Imi.Framework.UX.Wpf.Controls;assembly=Imi.Framework.UX.Wpf.Controls"
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
    ///     <MyNamespace:DrillDownMenu/>
    ///
    /// </summary>
    public class DrillDownMenu : ListBox, IKeyTipElement
    {
        private TransitionPresenter _presenter;
        private ButtonBase _backButton;
        private ButtonBase _clearButton;
        private ListBox _listBox1;
        private ListBox _listBox2;
        private ListBox _currentListBox;
        private ListBox _oldListBox;
        private ContentControl _headerControl;
        private DrillDownMenuItem _currentFolder;
        private TextBox _searchTextBox;
        private SearchManager _searchManager;
        private string _currentSearchText;

        #region Dependency Properties

        public IKeyTipSelector KeyTipSelector
        {
            get { return (IKeyTipSelector)GetValue(KeyTipSelectorProperty); }
            set { SetValue(KeyTipSelectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeyTipSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyTipSelectorProperty =
            DependencyProperty.Register("KeyTipSelector", typeof(IKeyTipSelector), typeof(DrillDownMenu), new UIPropertyMetadata(null));

        public bool IsKeyTipModeActive
        {
            get { return (bool)GetValue(IsKeyTipModeActiveProperty); }
            set { SetValue(IsKeyTipModeActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsKeyTipModeActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsKeyTipModeActiveProperty =
            DependencyProperty.Register("IsKeyTipModeActive", typeof(bool), typeof(DrillDownMenu), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, IsKeyTipModeActiveChanged));

        public string KeyTipAccessText
        {
            get { return (string)GetValue(KeyTipAccessTextProperty); }
            set { SetValue(KeyTipAccessTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeyTipAccessText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyTipAccessTextProperty =
            DependencyProperty.Register("KeyTipAccessText", typeof(string), typeof(DrillDownMenu), new UIPropertyMetadata(null));

        public DrillDownMenuItem TopMenuItem
        {
            get { return (DrillDownMenuItem)GetValue(TopMenuItemProperty); }
            set { SetValue(TopMenuItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopMenuItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopMenuItemProperty =
            DependencyProperty.Register("TopMenuItem", typeof(DrillDownMenuItem), typeof(DrillDownMenu), new UIPropertyMetadata(null, TopMenuItemChanged));

        // Using a DependencyProperty as the backing store for IsModified.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsModifiedProperty =
            DependencyProperty.Register("IsModified", typeof(bool), typeof(DrillDownMenu), new UIPropertyMetadata(false));


        public string SearchResultText
        {
            get { return (string)GetValue(SearchResultTextProperty); }
            set { SetValue(SearchResultTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchResultText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchResultTextProperty =
            DependencyProperty.Register("SearchResultText", typeof(string), typeof(DrillDownMenu), new UIPropertyMetadata("Results"));

        public string MoreResultsText
        {
            get { return (string)GetValue(MoreResultsTextProperty); }
            set { SetValue(MoreResultsTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoreResultsText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoreResultsTextProperty =
            DependencyProperty.Register("MoreResultsText", typeof(string), typeof(DrillDownMenu), new UIPropertyMetadata("More Results..."));


        private static void TopMenuItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DrillDownMenu)d).CurrentFolder = e.NewValue as DrillDownMenuItem;
        }

        public object Header
        {
            get
            {
                return (object)GetValue(HeaderProperty);
            }
            set
            {
                SetValue(HeaderProperty, value);

                if (_headerControl != null)
                    _headerControl.Content = value;
            }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(DrillDownMenu), new UIPropertyMetadata(""));


        public bool IsAtTop
        {
            get
            {
                return (bool)GetValue(IsAtTopProperty);
            }
            set
            {
                SetValue(IsAtTopProperty, value);
            }
        }

        public static readonly DependencyProperty IsAtTopProperty =
            DependencyProperty.Register("IsAtTop", typeof(bool), typeof(DrillDownMenu), new UIPropertyMetadata(false));




        public string SearchInfoText
        {
            get { return (string)GetValue(SearchInfoTextProperty); }
            set { SetValue(SearchInfoTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchInfoText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchInfoTextProperty =
            DependencyProperty.Register("SearchInfoText", typeof(string), typeof(DrillDownMenu), new UIPropertyMetadata("Find..."));


        public ControlTemplate BackButtonTemplate
        {
            get { return (ControlTemplate)GetValue(BackButtonTemplateProperty); }
            set { SetValue(BackButtonTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackButtonTemplateProperty =
            DependencyProperty.Register("BackButtonTemplate", typeof(ControlTemplate), typeof(DrillDownMenu), new UIPropertyMetadata(null, (d, v) =>
            {
                DrillDownMenu menu = (d as DrillDownMenu);

                if (menu._backButton != null)
                    menu._backButton.Template = v.NewValue as ControlTemplate;
            }
        ));


        public ControlTemplate ItemsContainerTemplate
        {
            get
            {
                return (ControlTemplate)GetValue(ItemsContainerTemplateProperty);
            }
            set
            {
                SetValue(ItemsContainerTemplateProperty, value);
            }
        }

        public static readonly DependencyProperty ItemsContainerTemplateProperty =
            DependencyProperty.Register("ItemsContainerTemplate", typeof(ControlTemplate), typeof(DrillDownMenu), new UIPropertyMetadata(null));


        public ContextMenu ItemsContextMenu
        {
            get { return (ContextMenu)GetValue(ItemsContextMenuProperty); }
            set { SetValue(ItemsContextMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsContextMenuProperty =
            DependencyProperty.Register("ItemsContextMenu", typeof(ContextMenu), typeof(DrillDownMenu), new UIPropertyMetadata(null));


        public bool HighlightSelected
        {
            get { return (bool)GetValue(HighlightSelectedProperty); }
            set { SetValue(HighlightSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HighlightSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightSelectedProperty =
            DependencyProperty.Register("HighlightSelected", typeof(bool), typeof(DrillDownMenu), new UIPropertyMetadata(null));

        #endregion


        private static void IsKeyTipModeActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (d is DrillDownMenu)
                {
                    DrillDownMenu menu = d as DrillDownMenu;

                    menu.RegisterKeyTips();
                }
            }
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            RegisterKeyTips();
        }

        private void RegisterKeyTips()
        {
            if (IsVisible)
            {
                if (KeyTipSelector != null)
                {
                    KeyTipSelector.SetKeyTips(Items);
                }

                if (IsKeyTipModeActive)
                {
                    foreach (IKeyTipElement menuItem in Items)
                    {
                        if (!string.IsNullOrEmpty(menuItem.KeyTipAccessText))
                        {
                            BindingOperations.SetBinding((menuItem as DrillDownMenuItem), DrillDownMenuItem.IsKeyTipModeActiveProperty, new Binding()
                            {
                                Source = this,
                                Path = new PropertyPath("IsKeyTipModeActive")
                            });
                        }
                    }
                }

                KeyTipActivationService.RegisterKeyTips(this, Items.Cast<IKeyTipElement>());
            }
        }

        public DrillDownMenuItem CurrentFolder
        {
            get
            {
                return _currentFolder;
            }
            set
            {
                _currentFolder = value;

                IsAtTop = (_currentFolder == TopMenuItem);

                if (_currentFolder != null)
                {
                    this.ItemsSource = _currentFolder.Children;
                    Header = _currentFolder.Caption;

                    if (_backButton != null)
                    {
                        if (_currentFolder.Parent != null)
                            _backButton.IsEnabled = true;
                        else
                            _backButton.IsEnabled = false;
                    }
                }
                else
                    Header = "";
            }
        }

        public List<DrillDownMenuItem> GetAllFolders
        {
            get
            {
                List<DrillDownMenuItem> returnList = new List<DrillDownMenuItem>();
                FolderSearchRecursion(TopMenuItem, returnList);
                return returnList;
            }
        }

        private void FolderSearchRecursion(DrillDownMenuItem currentMenuItem, List<DrillDownMenuItem> folderList)
        {
            if (currentMenuItem.IsFolder && !currentMenuItem.IsBackItem)
            {
                folderList.Add(currentMenuItem);

                List<DrillDownMenuItem> childFolders = currentMenuItem.Children.Where(m => m.IsFolder && !m.IsBackItem).ToList();

                if (childFolders.Count > 0)
                {
                    foreach (DrillDownMenuItem childFolder in childFolders)
                    {
                        FolderSearchRecursion(childFolder, folderList);
                    }
                }
            }
        }

        public static readonly RoutedEvent MenuItemSelectedEvent = EventManager.RegisterRoutedEvent("MenuItemSelected", RoutingStrategy.Bubble, typeof(MenuItemSelectedEventHandler), typeof(DrillDownMenu));

        public event MenuItemSelectedEventHandler MenuItemSelected
        {
            add { AddHandler(MenuItemSelectedEvent, value); }
            remove { RemoveHandler(MenuItemSelectedEvent, value); }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        static DrillDownMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DrillDownMenu), new FrameworkPropertyMetadata(typeof(DrillDownMenu)));
        }

        public DrillDownMenu()
        {
            _searchManager = new SearchManager();
            _searchManager.SearchCompleted += new QueryCompletedEventHandler(SearchServiceQueryCompleted);
            _searchManager.SearchStarted += new QueryStartedEventHandler(SearchServiceSearchStarted);
            _searchManager.SearchCancelled += new QueryStartedEventHandler(SearchServiceSearchCancelled);

            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(DrillDownMenu_IsVisibleChanged);
        }

        void DrillDownMenu_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RegisterKeyTips();
        }


        System.Threading.EventWaitHandle searchFinishedEvent = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);
        private int ongoingSearches = 0;

        void SearchServiceSearchCancelled()
        {
            ongoingSearches = 0;
            searchFinishedEvent.Set();
        }

        void SearchServiceSearchStarted()
        {
            searchFinishedEvent.Reset();
            ongoingSearches++;
        }

        void SearchServiceQueryCompleted(object result, Exception e)
        {
            if (result != null)
            {
                DrillDownMenuItem root = new DrillDownMenuItem();
                root.Caption = SearchResultText;
                root.IsFolder = true;

                DrillDownMenuItem temp = root;

                IList<DrillDownMenuItem> resultItems = result as IList<DrillDownMenuItem>;

                for (int i = 0; i < resultItems.Count; i++)
                {
                    if ((i % 20 == 0) && (i > 0))
                    {
                        DrillDownMenuItem sub = new DrillDownMenuItem();
                        sub.IsFolder = true;
                        sub.Caption = MoreResultsText;
                        sub.Parent = temp;
                        temp.Children.Add(sub);
                        temp = sub;
                    }

                    temp.Children.Add(resultItems[i]);
                }

                this.CurrentFolder = root;

                if (_currentListBox.Items.Count > 0)
                {
                    _currentListBox.SelectedItem = _currentListBox.Items[0];
                }
            }

            ongoingSearches--;

            if (ongoingSearches <= 0)
            {
                searchFinishedEvent.Set();
            }
        }

        public override void OnApplyTemplate()
        {
            _listBox1 = new ListBox();
            _listBox1.ItemTemplate = this.ItemTemplate;
            _listBox1.ItemTemplateSelector = this.ItemTemplateSelector;
            _listBox1.ItemsPanel = this.ItemsPanel;
            _listBox1.Template = ItemsContainerTemplate;
            _listBox1.ItemContainerStyle = this.ItemContainerStyle;

            _listBox2 = new ListBox();
            _listBox2.ItemTemplate = this.ItemTemplate;
            _listBox2.ItemTemplateSelector = this.ItemTemplateSelector;
            _listBox2.ItemsPanel = this.ItemsPanel;
            _listBox2.Template = ItemsContainerTemplate;
            _listBox2.ItemContainerStyle = this.ItemContainerStyle;
            _listBox2.Visibility = Visibility.Hidden;

            _currentListBox = _listBox1;

            _currentListBox.ItemsSource = this.ItemsSource;

            _presenter = Template.FindName("PART_TransitionPresenter", this) as ActiproSoftware.Windows.Media.Animation.TransitionPresenter;

            _backButton = Template.FindName("BackButton", this) as ButtonBase;

            if (_backButton != null)
            {
                _backButton.Click += new RoutedEventHandler(BackButtonClickedEventHandler);
                _backButton.Template = BackButtonTemplate;
            }

            _headerControl = Template.FindName("Header", this) as ContentControl;

            if (_headerControl != null)
            {
                _headerControl.Content = Header;
            }

            _searchTextBox = Template.FindName("SearchTextBox", this) as TextBox;

            if (_searchTextBox != null)
            {
                _searchTextBox.TextChanged += SearchTextBoxTextChangedEventHandler;
                _searchTextBox.KeyUp += OnSearchTextBoxKeyUp;
                _searchTextBox.GotKeyboardFocus += OnSearchTextBox_GotKeyboardFocus;

                Application.Current.MainWindow.PreviewKeyDown += PreviewKeyDownHandler;
            }

            _clearButton = Template.FindName("ClearButton", this) as ButtonBase;
            if (_clearButton != null)
            {
                _clearButton.Click += new RoutedEventHandler(ClearButtonClickEventHandler);
            }

            CurrentFolder = TopMenuItem;

            _presenter.Content = _currentListBox;

            base.OnApplyTemplate();
        }

        void OnSearchTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _searchTextBox.SelectAll();
        }

        private void PreviewKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.F)
                {
                    this.Dispatcher.BeginInvoke(
                                    new Action(() =>
                                    {
                                        if (_searchTextBox.IsVisible)
                                        {
                                            _searchTextBox.Focus();
                                        }
                                    }),
                                    DispatcherPriority.Background,
                                    null);


                    e.Handled = false;
                }
            }
        }

        private bool pendingNavigateDownRequest = false;

        private void NavigateDownRequestWorker(object state)
        {
            // bool waitForEvent = false;

            System.Threading.Thread.Sleep(20);

            //Dispatcher.Invoke(new Action(() => {
            //    if (_currentListBox.SelectedItem == null)
            //    {
            //        waitForEvent = true;
            //    }
            //}));



            if (ongoingSearches > 0)
            {
                searchFinishedEvent.WaitOne(5000);
            }

            Dispatcher.Invoke(new Action(() =>
            {
                if (_currentListBox.SelectedItem != null)
                {
                    object selectedItem = _currentListBox.SelectedItem;
                    TryNavigateDown(selectedItem);
                }
            }));

            ongoingSearches = 0;
            pendingNavigateDownRequest = false;
        }

        private void OnSearchTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            if ((e.Key == Key.Enter) || (e.Key == Key.Right))
            {
                lock (_searchManager)
                {
                    if (!pendingNavigateDownRequest)
                    {
                        pendingNavigateDownRequest = true;
                        System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(NavigateDownRequestWorker));
                    }
                }

            }
            else if ((e.Key == Key.Left) || (e.Key == Key.Escape))
            {
                if (_backButton != null)
                {
                    NavigateBack();
                }
            }
            else if (e.Key == Key.Up)
            {
                if (_currentListBox.SelectedItem == null)
                {
                    if (_currentListBox.Items.Count > 0)
                    {
                        _currentListBox.SelectedIndex = 0;
                    }
                }
                else
                {
                    if ((_currentListBox.Items.Count > 0) && (_currentListBox.SelectedIndex > 0))
                    {
                        _currentListBox.SelectedIndex--;
                    }
                }
            }
            else if (e.Key == Key.Down)
            {
                if (_currentListBox.SelectedItem == null)
                {
                    if (_currentListBox.Items.Count > 0)
                    {
                        _currentListBox.SelectedIndex = 0;
                    }
                }
                else
                {
                    if ((_currentListBox.Items.Count > 0) && (_currentListBox.SelectedIndex < (_currentListBox.Items.Count - 1)))
                    {
                        _currentListBox.SelectedIndex++;
                    }
                }
            }

            if (_currentListBox.SelectedItem != null)
            {
                _currentListBox.ScrollIntoView(_currentListBox.SelectedItem);
            }
        }

        private void ClearButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            _searchTextBox.Text = "";
            _searchManager.Reset();
        }

        private void SearchTextBoxTextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;

            if (string.IsNullOrEmpty(_searchTextBox.Text))
            {
                _searchManager.CancelSearch();
                CurrentFolder = TopMenuItem;
            }
            else if (_searchTextBox.Text != _currentSearchText)
            {
                _searchManager.TopMenuItem = TopMenuItem;
                _searchManager.Search(_searchTextBox.Text);
            }

            _currentSearchText = _searchTextBox.Text;
        }

        private void BackButtonClickedEventHandler(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            NavigateBack();
        }

        private void NavigateBack()
        {
            if ((CurrentFolder != null) && (CurrentFolder.Parent == null))
                return;

            DrillDownMenuItem selectedMenuItem = CurrentFolder.Parent;

            (_presenter.Transition as SlideTransition).Mode = TransitionMode.Out;

            CurrentFolder = selectedMenuItem;

            if ((_currentListBox.Items.Count > 0) && (_currentListBox.SelectedItem != _currentListBox.Items[0]))
            {
                _currentListBox.SelectedItem = _currentListBox.Items[0];
            }
        }

        public new int SelectedIndex
        {
            get
            {
                return -1;
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (_currentListBox.SelectedItem != null)
            {
                object selectedItem = _currentListBox.SelectedItem;

                if (((selectedItem is DrillDownMenuItem) && ((selectedItem as DrillDownMenuItem).IsReadOnly)) ||
                    (!(selectedItem is DrillDownMenuItem)))
                {
                    IInputElement element = _currentListBox.InputHitTest(e.MouseDevice.GetPosition(_currentListBox));

                    if ((element != null) && !(element is ScrollViewer))
                    {
                        TryNavigateDown(selectedItem);
                    }
                }
            }
        }

        public DrillDownMenuItem GetDropTarget(DragEventArgs e)
        {
            DrillDownMenuItem dropTarget = null;

            for (int i = 0; i < _currentListBox.Items.Count; i++)
            {
                ListBoxItem lbi = _currentListBox.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                if (lbi == null) continue;
                if (IsMouseOverTarget(lbi, e.GetPosition((IInputElement)lbi)))
                {
                    dropTarget = (DrillDownMenuItem)_currentListBox.Items[i];
                    break;
                }
            }

            return dropTarget;
        }

        private static bool IsMouseOverTarget(Visual target, Point point)
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(target);
            return bounds.Contains(point);
        }

        private void TryNavigateDown(object selectedItem)
        {
            MenuItemSelectedEventArgs args;
            bool raiseEvent = true;

            // If we have wrappped the entries
            DrillDownMenuItem selectedMenuItem = selectedItem as DrillDownMenuItem;

            if (selectedMenuItem != null)
            {
                if (selectedMenuItem.IsEnabled)
                {
                    if (selectedMenuItem.IsFolder)
                    {
                        _searchManager.Reset();
                        this.Header = selectedMenuItem.Caption;
                        (_presenter.Transition as SlideTransition).Mode = TransitionMode.In;
                        CurrentFolder = selectedMenuItem;
                    }
                }

                if (selectedMenuItem.IsBackItem)
                {
                    NavigateBack();
                    raiseEvent = false;
                }
            }

            if (raiseEvent)
            {
                // Raise external event
                args = new MenuItemSelectedEventArgs(MenuItemSelectedEvent, selectedItem);
                RaiseEvent(args);
                _currentListBox.SelectedItem = null;

                if ((_currentListBox.Items.Count > 0) && (selectedItem != _currentListBox.Items[0]))
                {
                    _currentListBox.SelectedItem = _currentListBox.Items[0];
                }
            }

            /*
            if (registerKeyTips)
            {
                RegisterKeyTips(this);
            }
            */
        }

        private void MenuItemSelectedEventHandler(DrillDownMenuItem selectedMenuItem)
        {
            if (selectedMenuItem.IsFolder)
            {
                this.Header = selectedMenuItem.Caption;
                CurrentFolder = selectedMenuItem;
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (_currentListBox != null)
            {
                _listBox1.Visibility = Visibility.Visible;
                _listBox2.Visibility = Visibility.Visible;

                if (_currentListBox == _listBox1)
                {
                    _listBox2.ItemsSource = newValue;
                    _oldListBox = _currentListBox;
                    _currentListBox = _listBox2;
                }
                else
                {
                    _listBox1.ItemsSource = newValue;
                    _oldListBox = _currentListBox;
                    _currentListBox = _listBox1;
                }

                _presenter.Content = _currentListBox;
            }
        }

        public void Activate(IKeyTipElement child)
        {
            if (IsVisible)
            {
                TryNavigateDown(child);
            }
        }
    }
}
