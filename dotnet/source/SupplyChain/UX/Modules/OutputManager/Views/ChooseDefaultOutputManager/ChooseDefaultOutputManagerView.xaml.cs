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
using System.Windows.Shapes;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using Imi.SupplyChain.OutputManager.Services.Initialization.DataContracts;
using ActiproSoftware.Windows.Controls.Ribbon;

namespace Imi.SupplyChain.UX.Modules.OutputManager.Views.ChooseDefaultOutputManager
{
    /// <summary>
    /// Interaction logic for ChooseDefaultWarehouseView.xaml
    /// </summary>
    [SmartPart]
    public partial class ChooseDefaultOutputManagerView : RibbonWindow, IChooseDefaultOutputManagerView
    {
        private ChooseDefaultOutputManagerPresenter presenter;

        public bool IsDetailView { get; set; }

        [CreateNew]
        public ChooseDefaultOutputManagerPresenter Presenter
        {
            get { return presenter; }
            set
            {
                presenter = value;
                presenter.View = this;
            }
        }

        public void SetFocus()
        {
            Focus();
        }

        public new bool IsVisible
        {
            get
            {
                return base.IsVisible;
            }
            set
            {
                if (value)
                    Visibility = Visibility.Visible;
                else
                    Visibility = Visibility.Collapsed;
            }
        }

        [ServiceDependency]
        public WorkItem WorkItem
        {
            get;
            set;
        }

        public bool RefreshDataOnShow
        {
            get;
            set;
        }

        public void Refresh()
        {
        }


        public ChooseDefaultOutputManagerView()
        {
            InitializeComponent();
            RefreshDataOnShow = true;
            IsDetailView = true;
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(IsVisibleChangedEventHandler);
            IsGlassEnabled = ((RibbonWindow)Application.Current.MainWindow).IsGlassEnabled;
            this.Owner = System.Windows.Application.Current.MainWindow;

        }

        private void IsVisibleChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                if (presenter != null)
                    presenter.OnViewShow();
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            presenter.SelectAndClose(OutputManagerCb.SelectedItem as Imi.SupplyChain.OutputManager.Services.Initialization.DataContracts.OutputManager);
        }

        public void Close(bool result)
        {
            DialogResult = result;
            Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            presenter.Close(false);
        }

        public void UpdateProgress(int progressPercentage)
        {
            if (progressPercentage < 100)
            {
                progressBar.Visibility = Visibility.Visible;
            }
            else
            {
                progressBar.Visibility = Visibility.Hidden;
            }
        }

        public void PresentData(object data)
        {
            this.DataContext = data;

            if (data != null)
            {
                FindOutputManagerResult outputManagerDetails = data as FindOutputManagerResult;
                OutputManagerCb.ItemsSource = outputManagerDetails.OutputManagers;
            }

        }



        public string SelectedOutputManagerId
        {
            get
            {
                return OutputManagerCb.SelectedValue as string;
            }
            set
            {
                OutputManagerCb.SelectedValue = value;
            }
        }



        public void Update(object parameters)
        {
        }

        public void SaveSettings(ChooseDefaultOutputManagerSettingsRepository settings)
        {
            settings.RecentOutputManagerIdentity = SelectedOutputManagerId;
        }

        public void LoadSettings(ChooseDefaultOutputManagerSettingsRepository settings)
        {
            SelectedOutputManagerId = settings.RecentOutputManagerIdentity;
        }

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            this.OkButton.IsEnabled = false;
            OutputManagerCb.SelectionChanged += delegate(object sender, SelectionChangedEventArgs e)
            { OkButton.IsEnabled = ((OutputManagerCb.SelectedItem != null)); };

            this.OkButton.IsEnabled = ((OutputManagerCb.SelectedItem != null));
            presenter.OnViewReady();
        }

        public void OnTearingDown()
        {

        }

        #endregion
    }

    public class ChooseDefaultOutputManagerSettingsProvider : Framework.UX.Settings.ISettingsProvider
    {

        #region ISettingsProvider Members

        public string GetKey(object target)
        {
            return ((IChooseDefaultOutputManagerView)target).GetType().Name;
        }

        public Type GetSettingsType(object target)
        {
            return typeof(ChooseDefaultOutputManagerSettingsRepository);
        }

        public void LoadSettings(object target, object settings)
        {
            ChooseDefaultOutputManagerSettingsRepository repository = new ChooseDefaultOutputManagerSettingsRepository();

            if (settings != null)
                repository = (ChooseDefaultOutputManagerSettingsRepository)settings;

            ((IChooseDefaultOutputManagerView)target).LoadSettings(repository);
        }

        public object SaveSettings(object target)
        {
            ChooseDefaultOutputManagerSettingsRepository settings = new ChooseDefaultOutputManagerSettingsRepository();

            ((IChooseDefaultOutputManagerView)target).SaveSettings(settings);

            return settings;
        }

        #endregion
    }

    [Serializable]
    public class ChooseDefaultOutputManagerSettingsRepository
    {
        public string RecentOutputManagerIdentity { get; set; }
    }
}
