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
    /// Interaction logic for ExplorerView.xaml
    /// </summary>

    public partial class ExplorerView : System.Windows.Controls.UserControl, IExplorerView, ISmartPartInfoProvider
    {
        ExplorerPresenter presenter;
        private string title;

        [CreateNew]
        public ExplorerPresenter Presenter
        {
            get { return presenter; }
            set
            {
                presenter = value;
                presenter.View = this;
            }
        }

        public ExplorerView()
        {
            InitializeComponent();
        }

        public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
        {
            ISmartPartInfo info = new SmartPartInfo();
            info.Title = this.title;
            return info;
        }
        
        public void SetUrl(string title, string url)
        {
            try
            {
                Browser.Navigate(new Uri(url));
                this.title = title;
            }
            catch (Exception)
            {
            }
        }
    }
}