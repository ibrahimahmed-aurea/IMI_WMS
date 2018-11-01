using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public partial class LoadingView : UserControl, ILoadingView
	{
        private LoadingPresenter presenter;

        public LoadingPresenter Presenter
        {
            get { return presenter; }
            set
            {
                presenter = value;
                presenter.View = this;
            }
        }

		public LoadingView()
		{
			this.InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            OnBuiltUp(null);
        }

        public void OnBuiltUp(string id)
        {
            Presenter.OnViewReady();
        }


    }
}