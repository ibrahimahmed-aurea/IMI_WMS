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
using ActiproSoftware.Windows.Controls.Navigation;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.ObjectModel;
using Imi.Framework.UX.Wpf.Workspaces;
using System.ComponentModel;
using Microsoft.Practices.CompositeUI;
using System.Xml;
using Imi.Framework.UX.Services;
using Imi.Framework.Wpf.Controls;
using Imi.SupplyChain.UX.Shell.Services;
using ActiproSoftware.Windows.Themes;
using Imi.SupplyChain.UX.Infrastructure;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.Framework.UX;
using Imi.SupplyChain.UX.Shell.Settings;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public partial class ShellView : UserControl, IShellView, IBuilderAware
    {
        private ShellPresenter _presenter;
        private Dictionary<IShellModule, IWorkspace> _workspaceDictionary;
        private ProgressView _progressView;
        private bool _isLoaded;
        private IWorkspace _commonWorkspace;
        private IDashboardView _dashboardWorkspace;

        [EventPublication(EventTopicNames.ShowNotification, PublicationScope.Global)]
        public event EventHandler<DataEventArgs<ShellNotification>> ShowNotificationEvent;
              
        [ServiceDependency]
        public WorkItem WorkItem
        {
            get;
            set;
        }
        
        [ServiceDependency]
        public IShellModuleService ShellModuleService { get; set; }
                
        [CreateNew]
        public ShellPresenter Presenter
        {
            get { return _presenter; }
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }
                
        public ShellView()
        {
            InitializeComponent();
                                                
            _workspaceDictionary = new Dictionary<IShellModule, IWorkspace>();
            _progressView = new ProgressView();
            _commonWorkspace = new DockWorkspace();
            ((DockWorkspace)_commonWorkspace).DataContext = null;
            ((DockWorkspace)_commonWorkspace).Background = null;
            
            this.Loaded += (s, e) =>
            {
                if (!_isLoaded)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Presenter.OnViewReady();
                    }));
                }

                _isLoaded = true;
            };
        }
                               
        private IWorkspace ActiveWorkspace
        {
            get
            {
                if (ShellModuleService.ActiveModule != null)
                    return GetWorkspace(ShellModuleService.ActiveModule);
                else
                    return null;
            }
        }

        public void CloseActiveSmartPart()
        {
            if ((ActiveWorkspace != null) && (ActiveWorkspace.ActiveSmartPart != null))
            {
                ActiveWorkspace.Close(ActiveWorkspace.ActiveSmartPart);
            }
        }

        public void CloseActiveWorkspace()
        {
            foreach (var smartPart in ActiveWorkspace.SmartParts)
                ActiveWorkspace.Close(smartPart);
        }
        
        public void CloseAllWorkspaces()
        {
            foreach (var workspace in _workspaceDictionary.Values)
                foreach (var smartPart in workspace.SmartParts)
                    workspace.Close(smartPart);

            foreach (var smartPart in _commonWorkspace.SmartParts)
                _commonWorkspace.Close(smartPart);
        }
                      
        private void UpdateProgressPosition()
        {
            double x = 0;
            double y = 0;

            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                x = Application.Current.MainWindow.Left;
                y = Application.Current.MainWindow.Top;
            }

            _progressView.SetBounds(x + Application.Current.MainWindow.ActualWidth / 2, y + Application.Current.MainWindow.ActualHeight / 2, 45, 45);
        }

        public IWorkspace GetWorkspace(IShellModule module)
        {
            if (module.Id == DashboardModule.ModuleId)
            {
                return _dashboardWorkspace;
            }
            else if (MainWindowSettingsProvider.CurrentSettings.IsCommonWorkspaceEnabled)
            {
                return _commonWorkspace;
            }
            else
            {
                if (!_workspaceDictionary.ContainsKey(module))
                {
                    DockWorkspace workspace = WorkItem.Workspaces.AddNew<DockWorkspace>();
                    workspace.DataContext = null;
                    ((DockWorkspace)workspace).Background = null;
                    _workspaceDictionary.Add(module, workspace);
                }

                return _workspaceDictionary[module];
            }
        }
        
        private void CloseViewCommandExecutedEventHandler(object sender, ExecutedRoutedEventArgs e)
        {
            object smartPart = ((ContentControl)((FrameworkElement)((FrameworkElement)e.OriginalSource).Parent).TemplatedParent).Content;

            IWorkspaceLocatorService locator = WorkItem.Services.Get<IWorkspaceLocatorService>();
            IWorkspace workspace = locator.FindContainingWorkspace(WorkItem, smartPart);

            if (workspace != null)
                workspace.Close(smartPart);
        }

        private void CloseViewCommandCanExecuteEventHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
                               
        public void ShowProgress()
        {
            UpdateProgressPosition();
            _progressView.Show();
            Mouse.SetCursor(Cursors.Wait);
        }

        public void HideProgress()
        {
            Mouse.SetCursor(Cursors.Arrow);
            _progressView.Hide();
        }
              
        public IShellPresentationModel Model
        {
            get
            {
                return this.DataContext as IShellPresentationModel;
            }
            set
            {
                this.DataContext = value;
                startView.DataContext = value;
                workspacePresenter.Content = GetWorkspace(Model.Module);
            }
        }

        public void ShowNotification(string applicationName, ShellNotification notification)
        {
            if (ShowNotificationEvent != null)
            {
                ShowNotificationEvent(applicationName, new DataEventArgs<ShellNotification>(notification));
            }
        }

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            _dashboardWorkspace = WorkItem.SmartParts.AddNew<DashboardView>();
        }

        public void OnTearingDown()
        {
            if (_progressView != null)
                _progressView.Close();
        }

        #endregion
    }
}
