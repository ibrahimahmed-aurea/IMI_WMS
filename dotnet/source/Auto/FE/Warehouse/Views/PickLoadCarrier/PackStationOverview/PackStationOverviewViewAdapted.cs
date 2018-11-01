using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.SmartParts;
using Validation = Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Xceed.Wpf.DataGrid;
using Imi.Framework.Wpf.Controls;
using Imi.Framework.Wpf.Data.Converters;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Views;
using Imi.Framework.UX.Wpf;

namespace Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier
{
    public partial class PackStationOverviewView : UserControl, IPackStationOverviewView, IActionProvider, ISmartPartInfoProvider, IBuilderAware
    {
        public bool EANFocus
        {
            get;
            private set;
        }

        private void SetFocus(bool force)
        {
            if (EANFocus)
            {
                EAN_code.Focus();
                //EANFocus = false;
            }
            else
            {
                if (!FromLoadCarrier.IsEnabled || !ToLoadCarrier.IsEnabled)
                {
                    EAN_code.Focus();
                }
                else
                {
                    FromLoadCarrier.Focus();
                }
            }
        }

        public void PresentData(object data)
        {
            NoName.SelectionChanged -= DataGridSelectionChangedEventHandler;
            if (this.DataContext is List<PackStationOverviewViewResult>)
            {
                foreach (PackStationOverviewViewResult resultRow in ((List<PackStationOverviewViewResult>)this.DataContext))
                {
                    resultRow.PropertyChanged -= new PropertyChangedEventHandler(DataContext_PropertyChanged);
                }
            }
            this.DataContext = data;
            if (this.DataContext is List<PackStationOverviewViewResult>)
            {
                foreach (PackStationOverviewViewResult resultRow in ((List<PackStationOverviewViewResult>)this.DataContext))
                {
                    resultRow.PropertyChanged += new PropertyChangedEventHandler(DataContext_PropertyChanged);
                }
                if (((List<PackStationOverviewViewResult>)this.DataContext).Count == 0)
                {
                    FromLoadCarrier.Text = string.Empty;
                }
            }
            if (NoName.Items.Count > 0)
                CurrentItem = NoName.Items[0] as PackStationOverviewViewResult;

            if (isMultiEnabled)
                NoName.SelectionMode = SelectionMode.Extended;
            else
                NoName.SelectionMode = SelectionMode.Single;

            GotoBookmark();

            NoName.SelectionChanged += DataGridSelectionChangedEventHandler;

            EnableGenericDrilldown(data);
            presenter.OnViewUpdated(CurrentItem);
        }

        void DataContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Selected")
            {
                if (!((PackStationOverviewViewResult)CurrentItem).Selected.Value)
                {
                    ((PackStationOverviewViewResult)CurrentItem).PropertyChanged -= DataContext_PropertyChanged;
                    ((PackStationOverviewViewResult)CurrentItem).Selected = true;
                    ((PackStationOverviewViewResult)CurrentItem).PropertyChanged += DataContext_PropertyChanged;
                }
                else
                {
                    ((PackStationOverviewViewResult)CurrentItem).PropertyChanged -= DataContext_PropertyChanged;
                    ((PackStationOverviewViewResult)CurrentItem).Selected = presenter.SelectFromLoadCarrier(((PackStationOverviewViewResult)CurrentItem).LoadCarrierId);
                    ((PackStationOverviewViewResult)CurrentItem).PropertyChanged += DataContext_PropertyChanged;
                    presenter.OnViewUpdated(CurrentItem);
                }
            }
        }

        public List<PackStationOverviewViewResult> GetData
        {
            get
            {
                if (this.DataContext != null)
                {
                    return ((List<PackStationOverviewViewResult>)this.DataContext);
                }
                else
                {
                    return null;
                }
            }
        }

        private void FromLoadCarrier_DialogButtonClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Presenter.ShowComboDialog(FromLoadCarrierDropDownContent, true);
        }

        private void ToLoadCarrier_DialogButtonClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Presenter.ShowComboDialog(ToLoadCarrierDropDownContent, false);
        }

        private void FromLoadCarrier_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            FromLoadCarrier.IsDialogOpen = false;
            FromLoadCarrier.Focus();
            FromLoadCarrier.Text = ((FindLoadCarrierForPackStationFindViewResult)e.AddedItems[0]).LoadCarrierId;
            SelectFromLoadCarrier();
        }

        private void ToLoadCarrier_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            ToLoadCarrier.IsDialogOpen = false;
            ToLoadCarrier.Focus();
            ToLoadCarrier.Text = ((FindLoadCarrierForPackStationFindViewResult)e.AddedItems[0]).LoadCarrierId;
            if (!presenter.SelectToLoadCarrier(ToLoadCarrier.Text))
            {
                ToLoadCarrier.Text = string.Empty;
            }
        }

        private void FromLoadCarrier_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                SelectFromLoadCarrier();
            }
        }

        private void ToLoadCarrier_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                if (!presenter.SelectToLoadCarrier(ToLoadCarrier.Text))
                {
                    ToLoadCarrier.Text = string.Empty;
                }
            }
        }

        private void SelectFromLoadCarrier()
        {
            bool foundLoadCarrier = false;
            if (DataContext != null)
            {
                foreach (PackStationOverviewViewResult item in ((IList<PackStationOverviewViewResult>)DataContext))
                {
                    if (item.LoadCarrierId.Trim() == FromLoadCarrier.Text.Trim())
                    {
                        if (item.Selected.Value)
                        {
                            CurrentItem = item;
                            CurrentItem.Selected = true;
                            foundLoadCarrier = true;
                            break;
                        }
                    }
                }
            }
            if (!foundLoadCarrier)
            {
                presenter.SelectFromLoadCarrier(FromLoadCarrier.Text);
            }
            FromLoadCarrier.SelectAll();
            FromLoadCarrier.Focus();
        }

        private void NewLoadCarrierLink_Clicked(object sender, RoutedEventArgs e)
        {
            presenter.ShowCreateLoadCarrierDialog();
        }

        public string ToLoadCarrierId
        {
            get
            {
                return ToLoadCarrier.Text;
            }
            set
            {
                ToLoadCarrier.Text = value;
            }
        }

        private void ToLoadCarrier_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ToLoadCarrier.Text = presenter.ToLoadCarrierId;
        }

        private void EAN_code_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EANFocus = true;
                e.Handled = true;
                presenter.PackWithEAN(EAN_code.Text);
                EAN_code.Text = string.Empty;
            }
        }

        private void EAN_code_LostFocus(object sender, RoutedEventArgs e)
        {
            EANFocus = false;
        }

    }
}
