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
using System;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor;

namespace Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor
{
    public partial class PickPaceMonitorOverviewView : UserControl, IPickPaceMonitorOverviewView, IActionProvider, ISmartPartInfoProvider, IBuilderAware
    {
        private List<PickPaceMonitorOverviewViewResult> _originalData = new List<PickPaceMonitorOverviewViewResult>();
        private int _duration = 30;
        private string _valueBinding = "PickedQuantity";
        private Type _valueType = typeof(decimal);
        private double? panFaktor = null;


        public void SaveFavoriteSettings(PickPaceMonitorControllerSettingsRepository settings)
        {
            settings.TheChart_Zoom = theChart.Zoom;
            settings.TheChart_PanFaktor = theChart.PanOffset.X / theChart.PlotAreaClip.Width;
            settings.typeCombo_SelectedIndex = typeCombo.SelectedIndex;
            settings.DurationCombo_SelectedIndex = DurationCombo.SelectedIndex;
            //settings.CapacityCheckBox_IsChecked = CapacityCheckBox.IsChecked.Value;
        }

        public void LoadFavoriteSettings(PickPaceMonitorControllerSettingsRepository settings)
        {
            typeCombo.SelectedIndex = settings.typeCombo_SelectedIndex;
            DurationCombo.SelectedIndex = settings.DurationCombo_SelectedIndex;
            //CapacityCheckBox.IsChecked = settings.CapacityCheckBox_IsChecked;
            theChart.Zoom = settings.TheChart_Zoom;
            panFaktor = settings.TheChart_PanFaktor;
        }

        private void DurationCombo_Initialized(object sender, EventArgs e)
        {
            DurationCombo.Items.Add(new durationItem(ResourceManager.str_dur60_Caption, 60));
            DurationCombo.Items.Add(new durationItem(ResourceManager.str_dur30_Caption, 30));
            DurationCombo.Items.Add(new durationItem(ResourceManager.str_dur15_Caption, 15));
            DurationCombo.Items.Add(new durationItem(ResourceManager.str_dur10_Caption, 10));
            DurationCombo.Items.Add(new durationItem(ResourceManager.str_dur5_Caption, 5));

            DurationCombo.SelectedIndex = 0;
        }

        private void DurationCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _duration = ((durationItem)DurationCombo.SelectedItem).Duration;
            RefreshDataInViews();
            if (_originalData.Count > 0)
            {
                UpdateChartZoom();
            }
        }

        private void typeCombo_Initialized(object sender, EventArgs e)
        {
            typeCombo.Items.Add(new typeItem(ResourceManager.str_pickedQuantity_Caption, "PickedQuantity", typeof(decimal)));
            typeCombo.Items.Add(new typeItem(ResourceManager.str_weight_Caption, "Weight", typeof(decimal)));
            typeCombo.Items.Add(new typeItem(ResourceManager.str_volume_Caption, "Volume", typeof(decimal)));

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
            if (pickPaceSerie.ItemsSource != null)
            {
                List<PickPaceMonitorOverviewViewResult> tmpData = ((List<PickPaceMonitorOverviewViewResult>)pickPaceSerie.ItemsSource);

                int faktor = ((int)theChart.PlotAreaClip.Width / 52);
                Size newZoom = new Size(Math.Round((Convert.ToDouble(tmpData.Count) / (tmpData.Count > faktor ? faktor : tmpData.Count)), 2), 1);
                if (newZoom != theChart.Zoom)
                {
                    theChart.PanOffset = new Point(0, 0);
                    theChart.Zoom = newZoom;
                }
            }
        }


        public void PresentData(object data)
        {
            _originalData = (List<PickPaceMonitorOverviewViewResult>)data;

            LastUpdatedText.Text = DateTime.Now.ToString("g");

            if (_originalData != null)
            {
                RemainingStackPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                RemainingStackPanel.Visibility = System.Windows.Visibility.Collapsed;
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


        private void RefreshDataInViews()
        {
            if (presenter != null)
            {
                IList<PickPaceMonitorOverviewViewResult> data;

                data = UpdateChartData();

                if (data is IList<PickPaceMonitorOverviewViewResult>)
                {
                    presenter.OnViewUpdated(data as IList<PickPaceMonitorOverviewViewResult>);
                }
                else
                {
                    presenter.OnViewUpdated(data as PickPaceMonitorOverviewViewResult);
                }
            }
        }

        private IList<PickPaceMonitorOverviewViewResult> UpdateChartData()
        {
            List<PickPaceMonitorOverviewViewResult> tmpData = new List<PickPaceMonitorOverviewViewResult>();


            ChartDataValues chartValues = new ChartDataValues();

            int index = 0;

            if (_originalData != null)
            {
                foreach (PickPaceMonitorOverviewViewResult data in _originalData)
                {
                    if (data.TimeStamp == null)
                    {
                        chartValues.SetRemaningValues(data);
                        continue;
                    }

                    index++;

                    if (index == 1)
                    {
                        chartValues.SetDateTimeForNewTimeSlot(data.TimeStamp.GetValueOrDefault());
                    }


                    chartValues.IncValuesInTimeSlot(data);


                    if (index == (_duration / 5))
                    {
                        chartValues.ReCalculateValuesInTimeSlot();

                        PickPaceMonitorOverviewViewResult newData = new PickPaceMonitorOverviewViewResult();

                        newData.PickedQuantity = (chartValues.pickedQty != null ? (decimal?)Math.Round(chartValues.pickedQty.GetValueOrDefault(), 2) : null);
                        newData.Weight = (chartValues.weight != null ? (decimal?)Math.Round(chartValues.weight.GetValueOrDefault(), 2) : null);
                        newData.Volume = (chartValues.volume != null ? (decimal?)Math.Round(chartValues.volume.GetValueOrDefault(), 2) : null);

                        newData.AveragePickedQuantity = (chartValues.avgPickedQty != null ? (decimal?)Math.Round(chartValues.avgPickedQty.GetValueOrDefault(), 2) : null);
                        newData.AverageWeight = (chartValues.avgWeight != null ? (decimal?)Math.Round(chartValues.avgWeight.GetValueOrDefault(), 2) : null);
                        newData.AverageVolume = (chartValues.avgVolume != null ? (decimal?)Math.Round(chartValues.avgVolume.GetValueOrDefault(), 2) : null);

                        newData.LongTimeAvgPickedQuantity = data.LongTimeAvgPickedQuantity;
                        newData.LongTimeAvgWeight = data.LongTimeAvgWeight;
                        newData.LongTimeAvgVolume = data.LongTimeAvgVolume;

                        newData.TimeStamp = chartValues.date;
                        newData.TimeSpanText = chartValues.date.ToString("HH:mm") + "-" + chartValues.date.AddMinutes((_duration)).ToString("HH:mm") + "\r" + chartValues.date.ToString("dd MMM");

                        tmpData.Add(newData);

                        chartValues.ResetValues();

                        index = 0;
                    }
                }

                chartValues.CalcPreliminaryEndTime(_duration, _valueBinding);
            }

            try
            {

                if (tmpData.Count > 0)
                {

                    pickPaceSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding };

                    avgPickPaceSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = "Average" + _valueBinding };

                    longTimeAvgpickPaceSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = "LongTimeAvg" + _valueBinding };  

                    pickPaceSerie.ItemsSource = tmpData;
                    avgPickPaceSerie.ItemsSource = tmpData;
                    longTimeAvgpickPaceSerie.ItemsSource = tmpData;

                    if (chartValues.lastDate.Date == DateTime.Now.Date) //Show remaining only if last value is today.
                    {
                        RemainingText.Text = Math.Round(chartValues.remaining, 2).ToString();
                        RemainingTimeText.Text = chartValues.estFinishTime.ToString("t");
                    }
                    else
                    {
                        RemainingText.Text = string.Empty;
                        RemainingTimeText.Text = string.Empty;
                    }
                }
                else
                {
                    pickPaceSerie.ItemsSource = null;
                    avgPickPaceSerie.ItemsSource = null;
                    longTimeAvgpickPaceSerie.ItemsSource = null;

                    RemainingText.Text = string.Empty;
                    RemainingTimeText.Text = string.Empty;

                }


            }
            catch (Exception ex)
            {

            }

            return tmpData;
        }


        private class ChartDataValues
        {
            public decimal? pickedQty = null;
            public decimal? weight = null;
            public decimal? volume = null;

            public decimal? avgPickedQty = null;
            public decimal? avgWeight = null;
            public decimal? avgVolume = null;

            public decimal remaningPickedQty = 0;
            public decimal remaningWeight = 0;
            public decimal remaningVolume = 0;

            public decimal lastAvgPickedQty = 0;
            public decimal lastAvgWeight = 0;
            public decimal lastAvgVolume = 0;
            public DateTime lastDate = DateTime.Now;

            public DateTime date = DateTime.Now;

            public DateTime estFinishTime = DateTime.Now;
            public decimal remaining = 0;

            private int numOfSlots = 0;
            private int numOfSlotsAvg = 0;

            public void SetDateTimeForNewTimeSlot(DateTime newDateTime)
            {
                date = newDateTime;
            }

            public void ResetValues()
            {

                pickedQty = null;
                weight = null;
                volume = null;

                avgPickedQty = null;
                avgWeight = null;
                avgVolume = null;

                numOfSlots = 0;
                numOfSlotsAvg = 0;
            }


            public void IncValuesInTimeSlot(PickPaceMonitorOverviewViewResult data)
            {
                if (data.PickedQuantity != null)
                {
                    pickedQty = pickedQty.GetValueOrDefault() + data.PickedQuantity.GetValueOrDefault();
                    numOfSlots++;
                }

                if (data.Weight != null)
                {
                    weight = weight.GetValueOrDefault() + data.Weight.GetValueOrDefault();
                }

                if (data.Volume != null)
                {
                    volume = volume.GetValueOrDefault() + data.Volume.GetValueOrDefault();
                }

                if (data.AveragePickedQuantity != null)
                {
                    avgPickedQty = avgPickedQty.GetValueOrDefault() + data.AveragePickedQuantity.GetValueOrDefault();
                    numOfSlotsAvg++;
                }

                if (data.AverageWeight != null)
                {
                    avgWeight = avgWeight.GetValueOrDefault() + data.AverageWeight.GetValueOrDefault();
                }

                if (data.AverageVolume != null)
                {
                    avgVolume = avgVolume.GetValueOrDefault() + data.AverageVolume.GetValueOrDefault();
                }

                if (pickedQty.GetValueOrDefault() > 0 || weight.GetValueOrDefault() > 0 || volume.GetValueOrDefault() > 0)
                {
                    lastDate = date;
                }
            }

            public void ReCalculateValuesInTimeSlot()
            {
                if (pickedQty != null)
                {
                    pickedQty = pickedQty / numOfSlots;
                }

                if (weight != null)
                {
                    weight = weight / numOfSlots;
                }

                if (volume != null)
                {
                    volume = volume / numOfSlots;
                }

                if (pickedQty != null)
                {
                    if (avgPickedQty != null)
                    {
                        avgPickedQty = avgPickedQty / numOfSlotsAvg;
                    }

                    if (avgWeight != null)
                    {
                        avgWeight = avgWeight / numOfSlotsAvg;
                    }

                    if (avgVolume != null)
                    {
                        avgVolume = avgVolume / numOfSlotsAvg;
                    }
                }
                else
                {
                    avgPickedQty = null;
                    avgWeight = null;
                    avgVolume = null;
                }


                if (pickedQty.GetValueOrDefault() > 0 || weight.GetValueOrDefault() > 0 || volume.GetValueOrDefault() > 0)
                {
                    lastAvgPickedQty = avgPickedQty.GetValueOrDefault();
                    lastAvgWeight = avgWeight.GetValueOrDefault();
                    lastAvgVolume = avgVolume.GetValueOrDefault();
                }
            }

            public void SetRemaningValues(PickPaceMonitorOverviewViewResult data)
            {
                remaningPickedQty = data.PickedQuantity.GetValueOrDefault();
                remaningWeight = data.Weight.GetValueOrDefault();
                remaningVolume = data.Volume.GetValueOrDefault();
            }

            public void CalcPreliminaryEndTime(int duration, string valueBinding)
            {
                switch (valueBinding)
                {
                    case "PickedQuantity":
                        if (lastAvgPickedQty > 0)
                        {
                            estFinishTime = lastDate.AddMinutes(Convert.ToDouble((remaningPickedQty / lastAvgPickedQty) * 60));
                        }
                        remaining = remaningPickedQty;
                        break;
                    case "Weight":
                        if (lastAvgWeight > 0)
                        {
                            estFinishTime = lastDate.AddMinutes(Convert.ToDouble((remaningWeight / lastAvgWeight) * 60));
                        }
                        remaining = remaningWeight;
                        break;
                    case "Volume":
                        if (lastAvgVolume > 0)
                        {
                            estFinishTime = lastDate.AddMinutes(Convert.ToDouble((remaningVolume / lastAvgVolume) * 60));
                        }
                        remaining = remaningVolume;
                        break;
                }
            }
        }
    }


    public class PickPaceMonitorControllerSettingsProvider : Imi.Framework.UX.Settings.ISettingsProvider
    {

        #region ISettingsProvider Members

        public string GetKey(object target)
        {
            return ((IPickPaceMonitorOverviewView)target).GetType().Name;
        }

        public Type GetSettingsType(object target)
        {
            return typeof(PickPaceMonitorControllerSettingsRepository);
        }

        public void LoadSettings(object target, object settings)
        {
            PickPaceMonitorControllerSettingsRepository repository = new PickPaceMonitorControllerSettingsRepository();

            if (settings != null)
                repository = (PickPaceMonitorControllerSettingsRepository)settings;

            ((IPickPaceMonitorOverviewView)target).LoadFavoriteSettings(repository);
        }

        public object SaveSettings(object target)
        {
            PickPaceMonitorControllerSettingsRepository settings = new PickPaceMonitorControllerSettingsRepository();

            ((IPickPaceMonitorOverviewView)target).SaveFavoriteSettings(settings);

            return settings;
        }

        #endregion
    }

    [Serializable]
    public class PickPaceMonitorControllerSettingsRepository
    {
        public Size TheChart_Zoom { get; set; }
        public double TheChart_PanFaktor { get; set; }

        public int typeCombo_SelectedIndex { get; set; }

        public int DurationCombo_SelectedIndex { get; set; }

        //public bool CapacityCheckBox_IsChecked { get; set; }
    }
}
