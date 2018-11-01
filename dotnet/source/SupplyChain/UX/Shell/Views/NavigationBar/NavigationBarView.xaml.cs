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
using ActiproSoftware.Windows.Themes;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using ActiproSoftware.Windows.Controls.Navigation;
using Imi.Framework.Wpf.Controls;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Shell.Services;
using Microsoft.Practices.CompositeUI;
using ActiproSoftware.Windows.Controls.Ribbon;
using Imi.Framework.UX;

namespace Imi.SupplyChain.UX.Shell.Views
{
    /// <summary>
    /// Interaction logic for NavigationBarView.xaml
    /// </summary>
    [SmartPart]
    public partial class NavigationBarView : UserControl, INavigationBarView
    {
        private NavigationBarPresenter _presenter;
        
        public NavigationBarView()
        {
            InitializeComponent();
                        
            this.Loaded += (s, e) =>
                {
                    if (Presenter != null)
                        Presenter.OnViewReady();
                };

            Application.Current.MainWindow.PreviewKeyDown += new KeyEventHandler(MainWindow_PreviewKeyDown);
        }

        private bool _minimizeAfterSelect = false;

        void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.F)
                {
                    _minimizeAfterSelect = navigationBar.IsMinimized;
                    SetNavigationBarMinimized(false);

                    e.Handled = false;
                }
                else if (e.Key == Key.M)
                {
                    navigationBar.IsMinimized = !navigationBar.IsMinimized;
                }
            }
        }

        [ServiceDependency]
        public WorkItem WorkItem { get; set; }
                
        [CreateNew]
        public NavigationBarPresenter Presenter
        {
            get { return _presenter; }
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }
                
        private void NavigationBarSelectionChangedEventHandler(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == navigationBar && e.AddedItems.Count > 0 && WorkItem.RootWorkItem.Items.FindByType<ShellHyperlink>().Count == 0) //Looks at shellhyperlink in workitem to prevent dialog specified in activation uri from opening before the module is fully loaded at application startup.
            {
                NavigationPane pane = e.AddedItems[0] as NavigationPane;

                if (pane != null && pane.IsVisible)
                    Presenter.ActivateModule(pane.Tag as IShellModule);
            }
        }

        private void StartMenuItemExecutedEventHandler(object sender, StartMenuItemExecutedEventArgs e)
        {
            ShellDrillDownMenuItem item = e.MenuItem;

            if ((item != null) && (!item.IsFolder))
            {
                IInputElement element = FocusManager.GetFocusedElement(Application.Current.MainWindow);
                Application.Current.MainWindow.Focus();

                if (element != null)
                    element.Focus();
                
                Presenter.ExecuteMenuItem(e);

                SetNavigationBarMinimized(_minimizeAfterSelect);
            }
        }

        private void ActionMenuItemSelectedEventHandler(object sender, MenuItemSelectedEventArgs e)
        {
            ShellDrillDownMenuItem action = e.SelectedItem as ShellDrillDownMenuItem;

            if ((action != null))
            {
                IInputElement element = FocusManager.GetFocusedElement(Application.Current.MainWindow);
                ((UIElement)e.Source).Focus();
                
                if (element != null)
                    element.Focus();

                Presenter.ExecuteAction(action);
            }
        }

        public void AddModel(IShellPresentationModel model)
        {
            ApplicationBar applicationBar = new ApplicationBar();
            applicationBar.DataContext = model;
            applicationBar.ActionMenuItemSelected += ActionMenuItemSelectedEventHandler;
            applicationBar.StartMenuItemExecuted += StartMenuItemExecutedEventHandler;
                        
            NavigationPane pane = new NavigationPane();
            pane.Title = model.Module.Title;
            pane.Content = applicationBar;
            pane.Name = model.Module.Id;
            pane.Tag = model.Module;
                                                
            if (model.Module.Icon != null)
            {
                Rect rect = new Rect(0, 0, 24, 24);
                DrawingVisual drawingVisual = new DrawingVisual();

                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    drawingContext.DrawImage(model.Module.Icon, rect);
                }

                RenderTargetBitmap bitmap = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default);
                bitmap.Render(drawingVisual);

                pane.ImageSourceLarge = bitmap;
                pane.ImageSourceSmall = model.Module.Icon;
            }

            navigationBar.Items.Add(pane);
        }

        private void SetNavigationBarMinimized(bool value)
        {
            navigationBar.Minimized -= NavigationBarisMinimizedChangedEventHandler;
            navigationBar.Expanded -= NavigationBarisMinimizedChangedEventHandler;
            navigationBar.IsMinimized = value;
            navigationBar.Minimized += NavigationBarisMinimizedChangedEventHandler;
            navigationBar.Expanded += NavigationBarisMinimizedChangedEventHandler;
        }

        private void NavigationBarisMinimizedChangedEventHandler(object sender, RoutedEventArgs e)
        {
            _minimizeAfterSelect = navigationBar.IsMinimized;
        }

        #region INavigationBarView Members


        public IShellModule SelectedModule
        {
            get
            {
                NavigationPane pane = navigationBar.SelectedItem as NavigationPane;

                if (pane != null)
                {
                    return pane.Tag as IShellModule;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                var pane = (from NavigationPane p in navigationBar.Items
                           where p.Tag == value
                           select p).LastOrDefault();

                navigationBar.SelectedItem = pane;
            }
        }

        public void AddToFavorites(IShellModule module, DrillDownMenuItem drillDownMenuItem)
        {
            var pane = (from NavigationPane p in navigationBar.Items
                        where p.Tag == module
                        select p).LastOrDefault();

            ApplicationBar bar = pane.Content as ApplicationBar;
            bar.AddToFavorites(drillDownMenuItem, null);
        }

        #endregion

    }
}
