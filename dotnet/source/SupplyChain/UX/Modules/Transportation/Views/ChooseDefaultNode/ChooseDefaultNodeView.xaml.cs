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
using Imi.SupplyChain.Transportation.Services.Authentication.DataContracts;

namespace Imi.SupplyChain.UX.Modules.Transportation.Views.ChooseDefaultNode
{
    /// <summary>
    /// Interaction logic for ChooseDefaultNode.xaml
    /// </summary>
    public partial class ChooseDefaultNodeView : RibbonWindow, IChooseDefaultNodeView
    {
        private ChooseDefaultNodePresenter presenter;
        
        public bool IsDetailView { get; set; }
        
        [CreateNew]
        public ChooseDefaultNodePresenter Presenter
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

        public ChooseDefaultNodeView()
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

        public new void Show()
        {
            this.Owner = System.Windows.Application.Current.MainWindow;
            base.ShowDialog();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            presenter.SelectAndClose(NodesCb.SelectedItem as UserNode);
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

        public void Close(bool result)
        {
            DialogResult = result;
            Close();
        }

        public void PresentData(object data)
        {
            this.DataContext = data;

            if (data != null)
            {
                FindUserDetailsResult userDetails = data as FindUserDetailsResult;
                NodesCb.ItemsSource = userDetails.UserNodes;
            }
            
        }

        public void SelectNode(object userNode)
        {
            NodesCb.SelectedItem = userNode;
        }

        public void Update(object parameters)
        {
        }

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            this.OkButton.IsEnabled = false;
            NodesCb.SelectionChanged += delegate(object sender, SelectionChangedEventArgs e)
                                                            { OkButton.IsEnabled = (NodesCb.SelectedItem != null); };

            presenter.OnViewReady();
        }

        public void OnTearingDown()
        {

        }

        #endregion


    }
}
