using System;
using System.Collections.Generic;
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
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;


namespace Imi.SupplyChain.UX.Views
{
    /// <summary>
    /// Interaction logic for SearchProgressView.xaml
    /// </summary>
    [SmartPart]
    public partial class SearchProgressView : UserControl, ISearchProgressView, IBuilderAware
    {
        SearchProgressPresenter presenter;
        private CancelSearchDelegate cancelCallback;

        public SearchProgressView()
        {
            InitializeComponent();
            this.IsVisibleChanged += IsVisibleChangedEventHandler;
        }

        [CreateNew]
        public SearchProgressPresenter Presenter
        {
            get { return presenter; }
            set
            {
                presenter = value;
                presenter.View = this;
            }
        }
                
        private void IsVisibleChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                if (presenter != null)
                    presenter.OnViewShow();
            }
        }
                        
        private void CancelButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            CancelButton.IsEnabled = false;
            
            presenter.Cancel(cancelCallback);
        }

        public CancelSearchDelegate CancelCallback
        {
            get
            {
                return cancelCallback;
            }
            set
            {
                this.cancelCallback = value;
            }
        }


        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            
        }

        public void OnTearingDown()
        {
            //Avoid memory leak with progress bar
            grid.Children.Remove(progressBar);

            
        }

        #endregion
    }
}