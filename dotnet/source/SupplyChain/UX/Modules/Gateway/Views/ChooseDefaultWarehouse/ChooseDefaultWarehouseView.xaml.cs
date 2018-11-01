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
using ActiproSoftware.Windows.Controls.Ribbon;
using Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts;

namespace Imi.SupplyChain.UX.Modules.Gateway.Views.ChooseDefaultWarehouse
{
    /// <summary>
    /// Interaction logic for ChooseDefaultWarehouseView.xaml
    /// </summary>
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

        public ChooseDefaultWarehouseView()
        {
            InitializeComponent();
            RefreshDataOnShow = true;
            IsDetailView = true;
            IsGlassEnabled = ((RibbonWindow)Application.Current.MainWindow).IsGlassEnabled;
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(IsVisibleChangedEventHandler);
            
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
            presenter.SelectAndClose(WarehouseCb.SelectedItem as UserWarehouse);
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            presenter.Close();
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

        public void Update(object parameters)
        { 
        }
      
        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            this.OkButton.IsEnabled = false;
            WarehouseCb.SelectionChanged += delegate(object sender, SelectionChangedEventArgs e)
            { OkButton.IsEnabled = WarehouseCb.SelectedItem != null; };
                                    
            presenter.OnViewReady();
        }

        public void OnTearingDown()
        {

        }

        #endregion

    }
}
