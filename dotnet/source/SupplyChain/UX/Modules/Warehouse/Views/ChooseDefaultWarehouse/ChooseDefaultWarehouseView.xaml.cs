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
using Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts;
using ActiproSoftware.Windows.Controls.Ribbon;

namespace Imi.SupplyChain.UX.Modules.Warehouse.Views.ChooseDefaultWarehouse
{
    /// <summary>
    /// Interaction logic for ChooseDefaultWarehouseView.xaml
    /// </summary>
    [SmartPart]
    public partial class ChooseDefaultWarehouseView : RibbonWindow, IChooseDefaultWarehouseView
    {
        private ChooseDefaultWarehousePresenter presenter;
                
        public bool IsDetailView { get; set; }

        [CreateNew]
        public ChooseDefaultWarehousePresenter Presenter
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

        private bool _hideClientId = false;
        public bool hideClientId 
        {
            get { return _hideClientId; }
            set
            {
                _hideClientId = value;

                if (_hideClientId)
                {
                    ClientCb.Visibility = System.Windows.Visibility.Hidden;
                    ClientLable.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    ClientCb.Visibility = System.Windows.Visibility.Visible;
                    ClientLable.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        public ChooseDefaultWarehouseView()
        {
            InitializeComponent();
            RefreshDataOnShow = true;
            IsDetailView = true;
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(IsVisibleChangedEventHandler);
            IsGlassEnabled = ((RibbonWindow)Application.Current.MainWindow).IsGlassEnabled;
            this.Owner = System.Windows.Application.Current.MainWindow;

            WarehouseCb.SelectionChanged += (sender, e) =>
            {
                SetCompanySource(WarehouseCb.SelectedItem as UserWarehouse);
            };

            hideClientId = _hideClientId;
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
            presenter.SelectAndClose(WarehouseCb.SelectedItem as UserWarehouse, ClientCb.SelectedItem as UserCompany);
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
                FindUserDetailsResult userDetails = data as FindUserDetailsResult;
                WarehouseCb.ItemsSource = userDetails.UserWarehouses;
            }
            
        }

        private void SetCompanySource(UserWarehouse warehouse)
        {
            if (warehouse != null)
            {
                FindUserDetailsResult userDetails = (this.DataContext as FindUserDetailsResult);

                ClientCb.ItemsSource = from UserCompany c in userDetails.UserCompanies
                                       where c.WarehouseIdentity == warehouse.WarehouseIdentity
                                       select c;
            }
        }

        public string SelectedWarehouseId
        {
            get
            {
                return WarehouseCb.SelectedValue as string;
            }
            set
            {
                WarehouseCb.SelectedValue = value;
            }
        }

        public string SelectedClientId
        {
            get
            {
                return ClientCb.SelectedValue as string;
            }
            set
            {
                ClientCb.SelectedValue = value;
            }
        }
        
        public bool IsClientInterfaceHAPI
        {
            get 
            {
                UserCompany company = ClientCb.SelectedItem as UserCompany;

                if (company != null)
                    return company.IsClientInterfaceHAPI == true;
                else
                    return false;
            }
        }

        public bool IsClientInterfaceWebServices
        {
            get
            {
                UserCompany company = ClientCb.SelectedItem as UserCompany;

                if (company != null)
                    return company.IsClientInterfaceWebServices == true;
                else
                    return false;
            }
        }

        public bool IsClientInterfaceEDI
        {
            get
            {
                UserCompany company = ClientCb.SelectedItem as UserCompany;

                if (company != null)
                    return company.IsClientInterfaceEDI == true;
                else
                    return false;
            }
        }

        public void Update(object parameters)
        { 
        }

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            this.OkButton.IsEnabled = false;
            WarehouseCb.SelectionChanged += delegate(object sender, SelectionChangedEventArgs e)
            { OkButton.IsEnabled = ((WarehouseCb.SelectedItem != null) && (ClientCb.SelectedItem != null)); };
            ClientCb.SelectionChanged += delegate(object sender, SelectionChangedEventArgs e)
            { OkButton.IsEnabled = ((WarehouseCb.SelectedItem != null) && (ClientCb.SelectedItem != null)); };
            this.OkButton.IsEnabled = ((WarehouseCb.SelectedItem != null) && (ClientCb.SelectedItem != null));
            presenter.OnViewReady();
        }

        public void OnTearingDown()
        {

        }

        #endregion

    }
}
