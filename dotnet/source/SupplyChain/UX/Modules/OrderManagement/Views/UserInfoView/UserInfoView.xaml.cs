using System.Windows;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX.Modules.OrderManagement.Services;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views
{
    public partial class UserInfoView : Window, IUserInfoView
    {
        private UserInfoPresenter presenter;
                       
        public bool IsDetailView { get; set; }
        
        [CreateNew]
        public UserInfoPresenter Presenter
        {
            get { return presenter; }
            set
            {
                presenter = value;
                presenter.View = this;
            }
        }

        [ServiceDependency]
        public WorkItem WorkItem
        {
            get;
            set;
        }

        public void SetFocus()
        {
            Focus();
        }

        public bool RefreshDataOnShow
        {
            get;
            set;
        }

        public UserInfoView()
        {
            InitializeComponent();
            RefreshDataOnShow = true;
            IsDetailView = true;
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(IsVisibleChangedEventHandler);
            //this.UpdateProgress += UpdateProgressInternal;

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
            this.DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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
                OMSSessionContext userDetails = data as OMSSessionContext;
                
                userId.Text = userDetails.OMSLogicalUserId;
                userName.Text = userDetails.UserName;
                empNum.Text = userDetails.EmployNumber;
                langCode.Text = userDetails.OMSLanguageCode;
                legalEntity.Text = userDetails.LegalEntity.ToString();
                whsNum.Text = userDetails.WarehouseNumber.ToString();
                orgUnit.Text = userDetails.OrgUnit;
                systemName.Text = userDetails.SystemName;
                loginId.Text = userDetails.OMSLoginUserId;
                host.Text = userDetails.Host;
                port.Text = userDetails.Port.ToString();
            }

        }

        public void Update(object parameters)
        { 
        }

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            presenter.OnViewReady();
        }

        public void OnTearingDown()
        {

        }

        #endregion


        #region IDataView Members


        public new bool IsVisible
        {
            get
            {
                return base.IsVisible;
            }
            set
            {
                if (value)
                    this.Visibility = Visibility.Visible;
                else
                    this.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}
