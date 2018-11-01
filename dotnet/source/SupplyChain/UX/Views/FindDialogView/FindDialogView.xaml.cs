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
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.Wpf.Controls;

namespace Imi.SupplyChain.UX.Views
{
    public partial class FindDialogView : UserControl, IFindDialogView
    {
        public FindDialogView()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
                {
                    IDataView view = searchWorkspace.ActiveSmartPart as IDataView;

                    if (view != null)
                        view.SetFocus();
                };
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.OriginalSource is System.Windows.Controls.Primitives.Selector || e.OriginalSource is ListBoxItem)
            {
                return;
            }

            if (e.Key == Key.Down)
            {
                IDataView searchView = searchWorkspace.ActiveSmartPart as IDataView;
                if (searchView != null)
                {
                    if (Framework.Wpf.Controls.ControlHelper.IsControlOnView(e.OriginalSource, searchView))
                    {

                        IDataView masterView = masterWorkspace.ActiveSmartPart as IDataView;
                        if (masterView != null)
                        {
                            e.Handled = true;
                            masterView.SetFocus();
                        }

                    }
                }
            }

        }

        public void ShowInSearchWorkspace(object view)
        {
            searchWorkspace.Visibility = Visibility.Visible;
            searchWorkspace.Show(view);
        }

        public void ShowInMasterWorkspace(object view)
        {
            if (view is IDataView)
                (view as IDataView).IsDetailView = true;

            masterWorkspace.Show(view);
        }

        //WORKAROUND TO AVOID GUI FROM HANGING WHEN GRID ARE WIDER THEN WORKSPACE
        public void UpdateWorkspaceLayout()
        {
            masterWorkspace.UpdateLayout();
        }
    }
}
