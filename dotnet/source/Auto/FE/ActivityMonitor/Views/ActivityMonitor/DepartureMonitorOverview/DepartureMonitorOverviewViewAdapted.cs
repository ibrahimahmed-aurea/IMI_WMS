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
using Microsoft.Practices.CompositeUI.EventBroker;
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
using System.Windows.Media.Animation;

namespace Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor
{
    public partial class DepartureMonitorOverviewView : UserControl, IDepartureMonitorOverviewView, IActionProvider, ISmartPartInfoProvider, IBuilderAware
    {
        private List<DepartureMonitorOverviewViewResult> _currentData = null;
        private string _valueBinding = "NoCars";
        private Type _valueType = typeof(decimal);
        private double? panFaktor = null;

        public string ValueBinding
        {
            get { return _valueBinding; }
        }

        public void SaveFavoriteSettings(DepartureMonitorControllerSettingsRepository settings)
        {
            settings.TheChart_Zoom = theChart.Zoom;
            settings.TheChart_PanFaktor = theChart.PanOffset.X / theChart.PlotAreaClip.Width;
            settings.typeCombo_SelectedIndex = typeCombo.SelectedIndex;
        }

        public void LoadFavoriteSettings(DepartureMonitorControllerSettingsRepository settings)
        {
            typeCombo.SelectedIndex = settings.typeCombo_SelectedIndex;
            theChart.Zoom = settings.TheChart_Zoom;
            panFaktor = settings.TheChart_PanFaktor;
        }

        private void typeCombo_Initialized(object sender, EventArgs e)
        {
            typeCombo.Items.Add(new typeItem(ResourceManager.str_cars_Caption, "NoCars", typeof(Decimal)));
            typeCombo.Items.Add(new typeItem(ResourceManager.str_paks_Caption, "NoPaks", typeof(Decimal)));
            typeCombo.Items.Add(new typeItem(ResourceManager.str_weight_Caption, "Weight", typeof(Decimal)));
            typeCombo.Items.Add(new typeItem(ResourceManager.str_volume_Caption, "Volume", typeof(Decimal)));
            typeCombo.Items.Add(new typeItem(ResourceManager.str_loadMeter_Caption, "LoadM", typeof(Decimal)));
            typeCombo.Items.Add(new typeItem(ResourceManager.str_carSize_Caption, "CarSize", typeof(Decimal)));

            typeCombo.SelectedIndex = 0;
        }

        private void typeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _valueBinding = ((typeItem)typeCombo.SelectedItem).ValueBinding;
            _valueType = ((typeItem)typeCombo.SelectedItem).ValueType;

            RefreshDataInViews();
        }


        private void LegendExpanderButton_Click(object sender, RoutedEventArgs e)
        {
            bool doAnimation = false;
            DoubleAnimation db = new DoubleAnimation();
            if (LegendStackPanel.Width == 130)
            {
                db.To = 0;
                doAnimation = true;
            }
            else if (LegendStackPanel.Width == 0)
            {
                db.To = 130;
                doAnimation = true;
            }

            if (doAnimation)
            {
                db.Duration = TimeSpan.FromSeconds(0.5);
                db.AutoReverse = false;
                db.RepeatBehavior = new RepeatBehavior(1);
                LegendStackPanel.BeginAnimation(StackPanel.WidthProperty, db);
            }
        }

        //private void UpdateChartZoom()
        //{
        //    if (OnShipLocationSerie.ItemsSource != null)
        //    {

        //        int faktor = Convert.ToInt32(XAxis.Maximum) / 24;
        //        Size newZoom = new Size(faktor, 1);
        //        //int faktor = ((int)theChart.PlotAreaClip.Width / 66);
        //        //Size newZoom = new Size(Math.Round((Convert.ToDouble(XAxis.Maximum) / (XAxis.Maximum > faktor ? faktor : XAxis.Maximum)), 2), 1);
        //        if (newZoom != theChart.Zoom)
        //        {
        //            theChart.PanOffset = new Point(0, 0);
        //            theChart.Zoom = newZoom;
        //        }
        //    }
        //}


        private void RefreshDataInViews()
        {
            if (presenter != null)
            {
                OnShipLocationSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding + "OnShipLoc" };

                PickNotOnShipLocationSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding + "PickNotOnShipLoc" };

                PallNotOnShipLocationSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding + "PallNotOnShipLoc" };

                TransitNotOnShipLocationSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding + "TransNotOnShipLoc" };

                OtherNotOnShipLocationSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding + "OtherNotOnShipLoc" };

                OnShipLocationSerie.ItemsSource = _currentData;
                PickNotOnShipLocationSerie.ItemsSource = _currentData;
                PallNotOnShipLocationSerie.ItemsSource = _currentData;
                TransitNotOnShipLocationSerie.ItemsSource = _currentData;
                OtherNotOnShipLocationSerie.ItemsSource = _currentData;

                presenter.OnViewUpdated(_currentData as IList<DepartureMonitorOverviewViewResult>);
            }
        }

        public void PresentData(object data)
        {
            LastUpdatedText.Text = DateTime.Now.ToString("g");
            _currentData = (List<DepartureMonitorOverviewViewResult>)data;

            if (_currentData != null)
            {
                foreach (DepartureMonitorOverviewViewResult result in _currentData)
                {
                    //Clera values when all are zero
                    CleraZeroValues(result);

                    bool notification = ((result.NumOfNotComposedOrders.GetValueOrDefault() + result.NumOfNotReceivedTransits.GetValueOrDefault()) > 0);

                    result.DepartureLabelInformation = result.DepartureId + "|" + notification.ToString() + "|" + result.PlannedDepartureTime.GetValueOrDefault().ToString("HH:mm") + "|" + result.RouteId;
                }
            }

            RefreshDataInViews();
        }

        private void CleraZeroValues(DepartureMonitorOverviewViewResult result)
        {
            if (result.NoCarsOnShipLoc.GetValueOrDefault() == 0
                        && result.NoCarsPickNotOnShipLoc.GetValueOrDefault() == 0
                        && result.NoCarsPallNotOnShipLoc.GetValueOrDefault() == 0
                        && result.NoCarsTransNotOnShipLoc.GetValueOrDefault() == 0
                        && result.NoCarsOtherNotOnShipLoc.GetValueOrDefault() == 0)
            {
                result.NoCarsOnShipLoc = null;
                result.NoCarsPickNotOnShipLoc = null;
                result.NoCarsPallNotOnShipLoc = null;
                result.NoCarsTransNotOnShipLoc = null;
                result.NoCarsOtherNotOnShipLoc = null;
            }

            if (result.NoPaksOnShipLoc.GetValueOrDefault() == 0
                        && result.NoPaksPickNotOnShipLoc.GetValueOrDefault() == 0
                        && result.NoPaksPallNotOnShipLoc.GetValueOrDefault() == 0
                        && result.NoPaksTransNotOnShipLoc.GetValueOrDefault() == 0
                        && result.NoPaksOtherNotOnShipLoc.GetValueOrDefault() == 0)
            {
                result.NoPaksOnShipLoc = null;
                result.NoPaksPickNotOnShipLoc = null;
                result.NoPaksPallNotOnShipLoc = null;
                result.NoPaksTransNotOnShipLoc = null;
                result.NoPaksOtherNotOnShipLoc = null;
            }

            if (result.CarSizeOnShipLoc.GetValueOrDefault() == 0
                        && result.CarSizePickNotOnShipLoc.GetValueOrDefault() == 0
                        && result.CarSizePallNotOnShipLoc.GetValueOrDefault() == 0
                        && result.CarSizeTransNotOnShipLoc.GetValueOrDefault() == 0
                        && result.CarSizeOtherNotOnShipLoc.GetValueOrDefault() == 0)
            {
                result.CarSizeOnShipLoc = null;
                result.CarSizePickNotOnShipLoc = null;
                result.CarSizePallNotOnShipLoc = null;
                result.CarSizeTransNotOnShipLoc = null;
                result.CarSizeOtherNotOnShipLoc = null;
            }

            if (result.LoadMOnShipLoc.GetValueOrDefault() == 0
                        && result.LoadMPickNotOnShipLoc.GetValueOrDefault() == 0
                        && result.LoadMPallNotOnShipLoc.GetValueOrDefault() == 0
                        && result.LoadMTransNotOnShipLoc.GetValueOrDefault() == 0
                        && result.LoadMOtherNotOnShipLoc.GetValueOrDefault() == 0)
            {
                result.LoadMOnShipLoc = null;
                result.LoadMPickNotOnShipLoc = null;
                result.LoadMPallNotOnShipLoc = null;
                result.LoadMTransNotOnShipLoc = null;
                result.LoadMOtherNotOnShipLoc = null;
            }

            if (result.WeightOnShipLoc.GetValueOrDefault() == 0
                        && result.WeightPickNotOnShipLoc.GetValueOrDefault() == 0
                        && result.WeightPallNotOnShipLoc.GetValueOrDefault() == 0
                        && result.WeightTransNotOnShipLoc.GetValueOrDefault() == 0
                        && result.WeightOtherNotOnShipLoc.GetValueOrDefault() == 0)
            {
                result.WeightOnShipLoc = null;
                result.WeightPickNotOnShipLoc = null;
                result.WeightPallNotOnShipLoc = null;
                result.WeightTransNotOnShipLoc = null;
                result.WeightOtherNotOnShipLoc = null;
            }

            if (result.VolumeOnShipLoc.GetValueOrDefault() == 0
                        && result.VolumePickNotOnShipLoc.GetValueOrDefault() == 0
                        && result.VolumePallNotOnShipLoc.GetValueOrDefault() == 0
                        && result.VolumeTransNotOnShipLoc.GetValueOrDefault() == 0
                        && result.VolumeOtherNotOnShipLoc.GetValueOrDefault() == 0)
            {
                result.VolumeOnShipLoc = null;
                result.VolumePickNotOnShipLoc = null;
                result.VolumePallNotOnShipLoc = null;
                result.VolumeTransNotOnShipLoc = null;
                result.VolumeOtherNotOnShipLoc = null;
            }
        }

        private void DepartureHyperLinkClicked(object sender, RoutedEventArgs e)
        {
            string departureId = (string)((Imi.Framework.Wpf.Controls.HyperLink)(e.Source)).Content;

            DepartureMonitorOverviewViewResult currentDeparture = _currentData.Where(r => r.DepartureId == departureId).FirstOrDefault();

            if (currentDeparture != null)
            {
                CurrentItem = currentDeparture;
                HyperLinkClicked(sender, e);
                presenter.OnViewUpdated(_currentData);
            }
        }

    }

    public class DepartureMonitorControllerSettingsProvider : Imi.Framework.UX.Settings.ISettingsProvider
    {

        #region ISettingsProvider Members

        public string GetKey(object target)
        {
            return ((IDepartureMonitorOverviewView)target).GetType().Name;
        }

        public Type GetSettingsType(object target)
        {
            return typeof(DepartureMonitorControllerSettingsRepository);
        }

        public void LoadSettings(object target, object settings)
        {
            DepartureMonitorControllerSettingsRepository repository = new DepartureMonitorControllerSettingsRepository();

            if (settings != null)
                repository = (DepartureMonitorControllerSettingsRepository)settings;

            ((IDepartureMonitorOverviewView)target).LoadFavoriteSettings(repository);
        }

        public object SaveSettings(object target)
        {
            DepartureMonitorControllerSettingsRepository settings = new DepartureMonitorControllerSettingsRepository();

            ((IDepartureMonitorOverviewView)target).SaveFavoriteSettings(settings);

            return settings;
        }

        #endregion
    }

    [Serializable]
    public class DepartureMonitorControllerSettingsRepository
    {
        public Size TheChart_Zoom { get; set; }
        public double TheChart_PanFaktor { get; set; }

        public int typeCombo_SelectedIndex { get; set; }
    }

    public class DepartureMonitorLabelLinkFormatConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string linkText = string.Empty;
            string departureLabelInfo = (string)value;

            string[] parts = departureLabelInfo.Split('|');

            if (parts.Length > 0)
            {
                linkText = parts[0];
            }

            return linkText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }

        #endregion
    }

    public class DepartureMonitorLabelFormatConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string labelText = string.Empty;
            string departureLabelInfo = (string)value;

            string[] parts = departureLabelInfo.Split('|');

            if (parts.Length > 2)
            {
                for (int i = 2; i < parts.Length; i++)
                {
                    if (!string.IsNullOrEmpty(labelText))
                    {
                        labelText += "\r\n";
                    }

                    labelText += parts[i];
                }
            }

            return labelText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }

        #endregion
    }

    public class DepartureMonitorNotificationFormatConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool notification = false;
            string departureLabelInfo = (string)value;

            string[] parts = departureLabelInfo.Split('|');

            if (parts.Length > 1)
            {
                notification = System.Convert.ToBoolean(parts[1]);
            }

            if (notification)
            {
                return Brushes.Red;
            }
            else
            {
                return Brushes.Black;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
