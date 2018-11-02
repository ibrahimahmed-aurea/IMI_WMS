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
using Imi.Framework.Wpf.Controls;
using Imi.Framework.Wpf.Data.Converters;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Views;
using Imi.Framework.UX.Wpf;
using System;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor;

namespace Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor
{
    public partial class PickZoneMonitorOverviewView : UserControl, IPickZoneMonitorOverviewView, IActionProvider, ISmartPartInfoProvider, IBuilderAware
    {
        private IList<PickZoneMonitorOverviewViewResult> _originalData = null;
        private IList<ChartDataHolder> _chartData = null;

        private string _valueBinding = "PickedQuantity";
        private Type _valueType = typeof(decimal);
        private double? panFaktor = null;

        public void SaveFavoriteSettings(PickZoneMonitorControllerSettingsRepository settings)
        {
            settings.TheChart_Zoom = theChart.Zoom;
            settings.TheChart_PanFaktor = theChart.PanOffset.X / theChart.PlotAreaClip.Width;
            settings.typeCombo_SelectedIndex = typeCombo.SelectedIndex;
        }

        public void LoadFavoriteSettings(PickZoneMonitorControllerSettingsRepository settings)
        {
            typeCombo.SelectedIndex = settings.typeCombo_SelectedIndex;
            theChart.Zoom = settings.TheChart_Zoom;
            panFaktor = settings.TheChart_PanFaktor;
        }

        private void typeCombo_Initialized(object sender, EventArgs e)
        {
            typeCombo.Items.Add(new typeItem(ResourceManager.str_pickedQuantity_Caption, "PickedQuantity", typeof(DateTime)));
            typeCombo.Items.Add(new typeItem(ResourceManager.str_weight_Caption, "Weight", typeof(DateTime)));
            typeCombo.Items.Add(new typeItem(ResourceManager.str_volume_Caption, "Volume", typeof(DateTime)));

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

        private void UpdateChartZoom()
        {
            if (finishedSerie.ItemsSource != null)
            {

                int faktor = Convert.ToInt32(XAxis.Maximum) / 24;
                Size newZoom = new Size(faktor, 1);
                //int faktor = ((int)theChart.PlotAreaClip.Width / 66);
                //Size newZoom = new Size(Math.Round((Convert.ToDouble(XAxis.Maximum) / (XAxis.Maximum > faktor ? faktor : XAxis.Maximum)), 2), 1);
                if (newZoom != theChart.Zoom)
                {
                    theChart.PanOffset = new Point(0, 0);
                    theChart.Zoom = newZoom;
                }
            }
        }


        public void PresentData(object data)
        {
            _originalData = (List<PickZoneMonitorOverviewViewResult>)data;

            DateTime updatedDTM = DateTime.Now;
            LastUpdatedText.Text = updatedDTM.ToString("g");
            NowLine.Value = Convert.ToDouble(updatedDTM.Hour + Math.Round((((decimal)updatedDTM.Minute * 1.666666666M) / 100), 2));

            if (_originalData != null)
            {
                _chartData = new List<ChartDataHolder>();
                KeyValuePair<decimal, string> calculatedValues;

                int highestValue = 0;
                foreach (PickZoneMonitorOverviewViewResult row in _originalData.Reverse())
                {


                    ChartDataHolder tmpRecord = new ChartDataHolder();

                    tmpRecord.PickZoneId = row.PickZoneId;

                    tmpRecord.PickedQuantityFinished = row.PickedQuantityFinished;
                    tmpRecord.PickedQuantityPlanned = row.PickQuantityPlanned;
                    tmpRecord.PickedQuantityStarted = row.PickQuantityStarted;

                    tmpRecord.VolumeFinished = row.VolumeFinished;
                    tmpRecord.VolumePlanned = row.VolumePlanned;
                    tmpRecord.VolumeStarted = row.VolumeStarted;

                    tmpRecord.WeightFinished = row.WeightFinished;
                    tmpRecord.WeightPlanned = row.WeightPlanned;
                    tmpRecord.WeightStarted = row.WeightStarted;

                    calculatedValues = calculateTimeValue(null, row.PickQuantityFinished_Start);
                    tmpRecord.PickedQuantityTransparentdtm = calculatedValues.Key;

                    calculatedValues = calculateTimeValue(row.PickQuantityFinished_Start, row.PickQuantityFinished_End);
                    tmpRecord.PickedQuantityFinisheddtm = calculatedValues.Key;
                    tmpRecord.PickedQuantityFinishedInfo = calculatedValues.Value;

                    calculatedValues = calculateTimeValue(row.PickQuantityStarted_Start, row.PickQuantityStarted_End);
                    tmpRecord.PickedQuantityStarteddtm = calculatedValues.Key;
                    tmpRecord.PickedQuantityStartedInfo = calculatedValues.Value;

                    calculatedValues = calculateTimeValue(row.PickQuantityPlanned_Start, row.PickQuantityPlanned_End);
                    tmpRecord.PickedQuantityPlanneddtm = calculatedValues.Key;
                    tmpRecord.PickedQuantityPlannedInfo = calculatedValues.Value;

                    //Find highest value and adjust maximum of axis
                    if (calculatedValues.Key > highestValue)
                    {
                        highestValue = (int)calculatedValues.Key;
                    }

                    calculatedValues = calculateTimeValue(null, row.WeightFinished_Start);
                    tmpRecord.WeightTransparentdtm = calculatedValues.Key;

                    calculatedValues = calculateTimeValue(row.WeightFinished_Start, row.WeightFinished_End);
                    tmpRecord.WeightFinisheddtm = calculatedValues.Key;
                    tmpRecord.WeightFinishedInfo = calculatedValues.Value;

                    calculatedValues = calculateTimeValue(row.WeightStarted_Start, row.WeightStarted_End);
                    tmpRecord.WeightStarteddtm = calculatedValues.Key;
                    tmpRecord.WeightStartedInfo = calculatedValues.Value;

                    calculatedValues = calculateTimeValue(row.WeightPlanned_Start, row.WeightPlanned_End);
                    tmpRecord.WeightPlanneddtm = calculatedValues.Key;
                    tmpRecord.WeightPlannedInfo = calculatedValues.Value;


                    calculatedValues = calculateTimeValue(null, row.VolumeFinished_Start);
                    tmpRecord.VolumeTransparentdtm = calculatedValues.Key;

                    calculatedValues = calculateTimeValue(row.VolumeFinished_Start, row.VolumeFinished_End);
                    tmpRecord.VolumeFinisheddtm = calculatedValues.Key;
                    tmpRecord.VolumeFinishedInfo = calculatedValues.Value;

                    calculatedValues = calculateTimeValue(row.VolumeStarted_Start, row.VolumeStarted_End);
                    tmpRecord.VolumeStarteddtm = calculatedValues.Key;
                    tmpRecord.VolumeStartedInfo = calculatedValues.Value;

                    calculatedValues = calculateTimeValue(row.VolumePlanned_Start, row.VolumePlanned_End);
                    tmpRecord.VolumePlanneddtm = calculatedValues.Key;
                    tmpRecord.VolumePlannedInfo = calculatedValues.Value;

                    _chartData.Add(tmpRecord);
                }

                highestValue = (highestValue / 24) + 1;
                XAxis.Maximum = 24 * highestValue;
                XAxis.MajorStep = highestValue;
            }
            else
            {
                _chartData = null;
            }

            RefreshDataInViews();

            if (panFaktor != null)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    theChart.PanOffset = new Point(theChart.PlotAreaClip.Width * panFaktor.Value, 0);
                    panFaktor = null;
                }));

            }
            else
            {
                if (_originalData != null)
                {
                    if (_originalData.Count > 0)
                    {
                        UpdateChartZoom();
                    }
                }
            }
        }

        private KeyValuePair<decimal, string> calculateTimeValue(DateTime? startdtm, DateTime? enddtm)
        {
            if (startdtm == null) 
            { 
                startdtm = DateTime.Now.Date; 
            }

            if (enddtm == null) {enddtm = DateTime.Now;}

            TimeSpan timeSpan = (enddtm.GetValueOrDefault() - startdtm.GetValueOrDefault());

            decimal timeValue = (timeSpan.Days * 24) + timeSpan.Hours + Math.Round((((decimal)timeSpan.Minutes * 1.666666666M) / 100), 2);

            string info = string.Empty;
            if (timeValue > 0)
            {
                info = startdtm.GetValueOrDefault().ToShortTimeString() + "\r\n" + startdtm.GetValueOrDefault().ToShortDateString() + "|" + enddtm.GetValueOrDefault().ToShortTimeString() + "\r\n" + enddtm.GetValueOrDefault().ToShortDateString();
            }

            KeyValuePair<decimal, string> result = new KeyValuePair<decimal, string>(timeValue, info);

            return result;
        }

        private void RefreshDataInViews()
        {
            if (presenter != null)
            {
                transparentSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding + "Transparentdtm" };

                finishedSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding + "Finisheddtm" };

                startedSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding + "Starteddtm" };

                plannedSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding + "Planneddtm" };

                transparentSerie.ItemsSource = _chartData;
                finishedSerie.ItemsSource = _chartData;
                startedSerie.ItemsSource = _chartData;
                plannedSerie.ItemsSource = _chartData;

                if (_originalData is IList<PickZoneMonitorOverviewViewResult>)
                {
                    presenter.OnViewUpdated(_originalData as IList<PickZoneMonitorOverviewViewResult>);
                }
                else
                {
                    presenter.OnViewUpdated(_originalData as PickZoneMonitorOverviewViewResult);
                }
            }
        }
    }

    public class ChartDataHolder
    {
        private int _valueBinding = 0;

        private decimal _pickedQuantityTransparent = 0;
        private decimal _volumeTransparent = 0;
        private decimal _weightTransparent = 0;


        public string PickZoneId { get; set; }

        public decimal PickedQuantityTransparentdtm { get { _valueBinding = 0; return _pickedQuantityTransparent; } set { _pickedQuantityTransparent = value; } }
        public decimal PickedQuantityFinisheddtm { get; set; }
        public decimal PickedQuantityStarteddtm { get; set; }
        public decimal PickedQuantityPlanneddtm { get; set; }

        public string PickedQuantityFinishedInfo { get; set; }
        public string PickedQuantityStartedInfo { get; set; }
        public string PickedQuantityPlannedInfo { get; set; }

        public decimal? PickedQuantityFinished { get; set; }
        public decimal? PickedQuantityStarted { get; set; }
        public decimal? PickedQuantityPlanned { get; set; }

        public decimal VolumeTransparentdtm { get { _valueBinding = 1; return _volumeTransparent; } set { _volumeTransparent = value; } }
        public decimal VolumeFinisheddtm { get; set; }
        public decimal VolumeStarteddtm { get; set; }
        public decimal VolumePlanneddtm { get; set; }

        public string VolumeFinishedInfo { get; set; }
        public string VolumeStartedInfo { get; set; }
        public string VolumePlannedInfo { get; set; }

        public decimal? VolumeFinished { get; set; }
        public decimal? VolumeStarted { get; set; }
        public decimal? VolumePlanned { get; set; }

        public decimal WeightTransparentdtm { get { _valueBinding = 2; return _weightTransparent; } set { _weightTransparent = value; } }
        public decimal WeightFinisheddtm { get; set; }
        public decimal WeightStarteddtm { get; set; }
        public decimal WeightPlanneddtm { get; set; }

        public string WeightFinishedInfo { get; set; }
        public string WeightStartedInfo { get; set; }
        public string WeightPlannedInfo { get; set; }

        public decimal? WeightFinished { get; set; }
        public decimal? WeightStarted { get; set; }
        public decimal? WeightPlanned { get; set; }

        public string FinishedValue
        {
            get
            {
                switch (_valueBinding)
                {
                    case 0:
                        return (PickedQuantityFinished == null ? string.Empty : PickedQuantityFinished.GetValueOrDefault().ToString());
                    case 1:
                        return (VolumeFinished == null ? string.Empty : VolumeFinished.GetValueOrDefault().ToString());
                    case 2:
                        return (WeightFinished == null ? string.Empty : WeightFinished.GetValueOrDefault().ToString());
                    default:
                        return string.Empty;
                }
            }
        }

        public string StartedValue
        {
            get
            {
                switch (_valueBinding)
                {
                    case 0:
                        return (PickedQuantityStarted == null ? string.Empty : PickedQuantityStarted.GetValueOrDefault().ToString());
                    case 1:
                        return (VolumeStarted == null ? string.Empty : VolumeStarted.GetValueOrDefault().ToString());
                    case 2:
                        return (WeightStarted == null ? string.Empty : WeightStarted.GetValueOrDefault().ToString());
                    default:
                        return string.Empty;
                }
            }
        }

        public string PlannedValue
        {
            get
            {
                switch (_valueBinding)
                {
                    case 0:
                        return (PickedQuantityPlanned == null ? string.Empty : PickedQuantityPlanned.GetValueOrDefault().ToString());
                    case 1:
                        return (VolumePlanned == null ? string.Empty : VolumePlanned.GetValueOrDefault().ToString());
                    case 2:
                        return (WeightPlanned == null ? string.Empty : WeightPlanned.GetValueOrDefault().ToString());
                    default:
                        return string.Empty;
                }
            }
        }

        public string FinishedInfo_Start
        {
            get
            {
                switch (_valueBinding)
                {
                    case 0:
                        return PickedQuantityFinishedInfo.Split('|')[0];
                    case 1:
                        return VolumeFinishedInfo.Split('|')[0];
                    case 2:
                        return WeightFinishedInfo.Split('|')[0];
                    default:
                        return string.Empty;
                }
            }
        }

        public string StartedInfo_Start
        {
            get
            {
                switch (_valueBinding)
                {
                    case 0:
                        return PickedQuantityStartedInfo.Split('|')[0];
                    case 1:
                        return VolumeStartedInfo.Split('|')[0];
                    case 2:
                        return WeightStartedInfo.Split('|')[0];
                    default:
                        return string.Empty;
                }
            }
        }

        public string PlannedInfo_Start
        {
            get
            {
                switch (_valueBinding)
                {
                    case 0:
                        return PickedQuantityPlannedInfo.Split('|')[0];
                    case 1:
                        return VolumePlannedInfo.Split('|')[0];
                    case 2:
                        return WeightPlannedInfo.Split('|')[0];
                    default:
                        return string.Empty;
                }
            }
        }


        public string FinishedInfo_End
        {
            get
            {
                switch (_valueBinding)
                {
                    case 0:
                        if (!PickedQuantityFinishedInfo.Contains('|')) {return string.Empty;}
                        return PickedQuantityFinishedInfo.Split('|')[1];
                    case 1:
                        if (!VolumeFinishedInfo.Contains('|')) {return string.Empty;}
                        return VolumeFinishedInfo.Split('|')[1];
                    case 2:
                        if (!WeightFinishedInfo.Contains('|')) {return string.Empty;}
                        return WeightFinishedInfo.Split('|')[1];
                    default:
                        return string.Empty;
                }
            }
        }

        public string StartedInfo_End
        {
            get
            {
                switch (_valueBinding)
                {
                    case 0:
                        if (!PickedQuantityStartedInfo.Contains('|')) {return string.Empty;}
                        return PickedQuantityStartedInfo.Split('|')[1];
                    case 1:
                        if (!VolumeStartedInfo.Contains('|')) {return string.Empty;}
                        return VolumeStartedInfo.Split('|')[1];
                    case 2:
                        if (!WeightStartedInfo.Contains('|')) {return string.Empty;}
                        return WeightStartedInfo.Split('|')[1];
                    default:
                        return string.Empty;
                }
            }
        }

        public string PlannedInfo_End
        {
            get
            {
                switch (_valueBinding)
                {
                    case 0:
                        if (!PickedQuantityPlannedInfo.Contains('|')) {return string.Empty;}
                        return PickedQuantityPlannedInfo.Split('|')[1];
                    case 1:
                        if (!VolumePlannedInfo.Contains('|')) {return string.Empty;}
                        return VolumePlannedInfo.Split('|')[1];
                    case 2:
                        if (!WeightPlannedInfo.Contains('|')) {return string.Empty;}
                        return WeightPlannedInfo.Split('|')[1];
                    default:
                        return string.Empty;
                }
            }
        }
    }

    public class PickZoneMonitorControllerSettingsProvider : Imi.Framework.UX.Settings.ISettingsProvider
    {

        #region ISettingsProvider Members

        public string GetKey(object target)
        {
            return ((IPickZoneMonitorOverviewView)target).GetType().Name;
        }

        public Type GetSettingsType(object target)
        {
            return typeof(PickZoneMonitorControllerSettingsRepository);
        }

        public void LoadSettings(object target, object settings)
        {
            PickZoneMonitorControllerSettingsRepository repository = new PickZoneMonitorControllerSettingsRepository();

            if (settings != null)
                repository = (PickZoneMonitorControllerSettingsRepository)settings;

            ((IPickZoneMonitorOverviewView)target).LoadFavoriteSettings(repository);
        }

        public object SaveSettings(object target)
        {
            PickZoneMonitorControllerSettingsRepository settings = new PickZoneMonitorControllerSettingsRepository();

            ((IPickZoneMonitorOverviewView)target).SaveFavoriteSettings(settings);

            return settings;
        }

        #endregion
    }

    [Serializable]
    public class PickZoneMonitorControllerSettingsRepository
    {
        public Size TheChart_Zoom { get; set; }
        public double TheChart_PanFaktor { get; set; }

        public int typeCombo_SelectedIndex { get; set; }
    }

    public class ChartXLabelFormatConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal decValue = System.Convert.ToDecimal(value);

            int numOfDays = (int)Math.Floor(decValue / 24);

            decValue = decValue % 24;

            string text = decValue.ToString("00.00").Replace(',', '.');

            string[] hour_min = text.Split('.');

            DateTime date = DateTime.Now.Date;

            date = date.AddHours(System.Convert.ToInt32(hour_min[0]));
            date = date.AddMinutes(Math.Round((System.Convert.ToInt32(hour_min[1]) / 1.666666666), 0));
            date = date.AddDays(numOfDays);

            text = date.ToShortTimeString() + "\r\n" + date.ToShortDateString();

            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }

        #endregion
    }
}
