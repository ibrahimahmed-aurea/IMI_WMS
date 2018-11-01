using System.Windows;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views
{
    public partial class ChooseUserView : Window, IChooseUserView
    {
        private ChooseUserPresenter presenter;
                        
        public bool IsDetailView { get; set; }
        public bool RefreshDataOnShow { get; set; }
        
        [CreateNew]
        public ChooseUserPresenter Presenter
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


        public ChooseUserView()
        {
            InitializeComponent();
            this.Owner = System.Windows.Application.Current.MainWindow;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            presenter.SetSelectedOMSUser(UserCbx.SelectedItem as OMSUser);
        }

        public void UpdateProgress(int progressPercentage)
        {
        }

        public void PresentData(object data)
        {
            if (data != null)
            {
                OMSUsersComboBoxData userDetails = data as OMSUsersComboBoxData;
                UserCbx.ItemsSource = userDetails.OMSUsers;
            }
            presenter.SetDefaultUser(); 
        }

        public string SelectedUserId
        {
            get
            {
                return UserCbx.SelectedValue as string;
            }
            set
            {
                UserCbx.SelectedValue = value;    
            }
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

        public void Update(object parameters)
        { 
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
                    this.Visibility = Visibility.Visible;
                else
                    this.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}
