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
using Telerik.Windows.Controls.Chart;
using System.Windows.Media.Animation;


namespace Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor
{
    public partial class LoadingMonitorOverviewView : UserControl, ILoadingMonitorOverviewView, IActionProvider, ISmartPartInfoProvider, IBuilderAware
    {

        private List<LoadingMonitorOverviewViewResult> _originalData = new List<LoadingMonitorOverviewViewResult>();
        private int _duration = 30;
        private string _valueBinding = "NoCars";
        private Type _valueType = typeof(int);
        private Telerik.Windows.Controls.ChartView.BarSeries _capacitySerie;
        //private Telerik.Windows.Controls.ChartView.SplineSeries _burndownSerie;
        private double? panFaktor = null;


        public void SaveFavoriteSettings(LoadingMonitorControllerSettingsRepository settings)
        {
            settings.TheChart_Zoom = theChart.Zoom;
            settings.TheChart_PanFaktor = theChart.PanOffset.X / theChart.PlotAreaClip.Width;
            settings.typeCombo_SelectedIndex = typeCombo.SelectedIndex;
            settings.DurationCombo_SelectedIndex = DurationCombo.SelectedIndex;
            settings.CapacityCheckBox_IsChecked = CapacityCheckBox.IsChecked.Value;
        }

        public void LoadFavoriteSettings(LoadingMonitorControllerSettingsRepository settings)
        {
            typeCombo.SelectedIndex = settings.typeCombo_SelectedIndex;
            DurationCombo.SelectedIndex = settings.DurationCombo_SelectedIndex;
            CapacityCheckBox.IsChecked = settings.CapacityCheckBox_IsChecked;
            theChart.Zoom = settings.TheChart_Zoom;
            panFaktor = settings.TheChart_PanFaktor;
        }

        private void theChart_Initialized(object sender, EventArgs e)
        {
            _capacitySerie = new Telerik.Windows.Controls.ChartView.BarSeries();
            _capacitySerie.DefaultVisualStyle = (Style)FindResource("capacitySeriesStyle");
            _capacitySerie.TrackBallInfoTemplate = (DataTemplate)FindResource("trackBallInfoTemplateCapacity");
            _capacitySerie.CategoryBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = "TimeSpanText" };
            _capacitySerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = "NoCarsCapacityInc" };

            //_burndownSerie = new Telerik.Windows.Controls.ChartView.SplineSeries();
            //_burndownSerie.Stroke = (Brush)FindResource("ChartSerie6Brush");
            //_burndownSerie.TrackBallInfoTemplate = (DataTemplate)FindResource("trackBallInfoTemplateBurnDown");
            //_burndownSerie.CategoryBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = "TimeSpanText" };
            //_burndownSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = "NoCarsCapacityDec" };
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
            typeCombo.Items.Add(new typeItem(ResourceManager.str_cars_Caption, "NoCars", typeof(int)));
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

        private void CapacityCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (CapacityCheckBox.IsChecked.GetValueOrDefault())
            {
                if (theChart.Series.Contains(_capacitySerie))
                {
                    _capacitySerie.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    theChart.Series.Add(_capacitySerie);
                }

                //if (theChart.Series.Contains(_burndownSerie))
                //{
                //    _burndownSerie.Visibility = System.Windows.Visibility.Visible;
                //}
                //else
                //{
                //    theChart.Series.Add(_burndownSerie);
                //}

            }
            else
            {
                theChart.Series.Remove(_capacitySerie);
                //theChart.Series.Remove(_burndownSerie);
            }
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
            if (workloadSerie.ItemsSource != null)
            {
                List<LoadingMonitorOverviewViewResult> tmpData = ((List<LoadingMonitorOverviewViewResult>)workloadSerie.ItemsSource);

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
            _originalData = (List<LoadingMonitorOverviewViewResult>)data;

            LastUpdatedText.Text = DateTime.Now.ToString("g");

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
                IList<LoadingMonitorOverviewViewResult> data;

                data = UpdateChartData();

                if (data is IList<LoadingMonitorOverviewViewResult>)
                {
                    presenter.OnViewUpdated(data as IList<LoadingMonitorOverviewViewResult>);
                }
                else
                {
                    presenter.OnViewUpdated(data as LoadingMonitorOverviewViewResult);
                }
            }
        }



        private IList<LoadingMonitorOverviewViewResult> UpdateChartData()
        {
            List<LoadingMonitorOverviewViewResult> tmpData = new List<LoadingMonitorOverviewViewResult>();

            ChartDataValues chartValues = new ChartDataValues();

            int index = 0;

            if (_originalData != null)
            {
                foreach (LoadingMonitorOverviewViewResult data in _originalData)
                {
                    if (data.TimeStamp == null)
                    {
                        chartValues.SetLaggingValues(data);

                        continue;
                    }

                    index++;

                    if (index == 1)
                    {
                        chartValues.SetDateTimeForNewTimeSlot(data.TimeStamp.GetValueOrDefault());

                        chartValues.LoadSaldoToNewTimeSlot(data);
                    }


                    chartValues.IncValuesInTimeSlot(data);

                    chartValues.RecalcSaldoInTimeSlot(data);

                    if (index == _duration)
                    {
                        //chartValues.CalcBurnDown();

                        LoadingMonitorOverviewViewResult newData = new LoadingMonitorOverviewViewResult();

                        newData.NoCars = Math.Round(chartValues.noCars);
                        newData.Weight = chartValues.weight;
                        newData.Volume = chartValues.volume;

                        newData.NoCarsLate = Math.Round(chartValues.noCarsLate);
                        newData.WeightLate = chartValues.weightLate;
                        newData.VolumeLate = chartValues.volumeLate;

                        newData.NoUsersInc = chartValues.noUsersCapacity;
                        newData.NoCarsCapacity = Math.Round(chartValues.noCarsCapacity);
                        newData.WeightCapacity = chartValues.weightCapacity;
                        newData.VolumeCapacity = chartValues.volumeCapacity;

                        //newData.NoCarsCapacityDec = chartValues.noCarsBurnDown == 0 ? null : (decimal?)chartValues.noCarsBurnDown;
                        //newData.WeightCapacityDec = chartValues.weightBurnDown == 0 ? null : (decimal?)chartValues.weightBurnDown;
                        //newData.VolumeCapacityDec = chartValues.volumeBurnDown == 0 ? null : (decimal?)chartValues.volumeBurnDown;

                        newData.TimeStamp = chartValues.date;
                        newData.TimeSpanText = chartValues.date.ToString("HH:mm") + "-" + chartValues.date.AddMinutes((_duration - 1)).ToString("HH:mm") + "\r" + chartValues.date.ToString("dd MMM");

                        tmpData.Add(newData);

                        chartValues.ResetValues();

                        index = 0;
                    }
                }
            }

            try
            {

                if (tmpData.Count > 0)
                {
                    LoadingMonitorOverviewViewResult laggingWork = new LoadingMonitorOverviewViewResult();

                    laggingWork.TimeStamp = tmpData[0].TimeStamp.GetValueOrDefault().AddMinutes(-_duration);
                    laggingWork.TimeSpanText = "<";

                    LoadingMonitorOverviewViewResult laggingWorkDummy = new LoadingMonitorOverviewViewResult();

                    laggingWorkDummy.TimeStamp = laggingWork.TimeStamp;
                    laggingWorkDummy.TimeSpanText = "<";

                    tmpData.Insert(0, laggingWorkDummy);

                    laggingWork.NoCarsLate = chartValues.LaggingCars;
                    laggingWork.WeightLate = chartValues.Laggingweight;
                    laggingWork.VolumeLate = chartValues.Laggingvolume;

                    laggingSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding + "Late" };

                    laggingSerie.ItemsSource = new LoadingMonitorOverviewViewResult[] { laggingWork };


                    workloadSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding };
                    workloadLateSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding + "Late" };
                    _capacitySerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding  + "Capacity" };
                    //_burndownSerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = _valueBinding.Substring(0, _valueBinding.Length - 3) + "CapacityDec" };

                    //_burndownSerie.ItemsSource = tmpData;
                    _capacitySerie.ItemsSource = tmpData;
                    workloadSerie.ItemsSource = tmpData;
                    workloadLateSerie.ItemsSource = tmpData;

                    tmpData.RemoveAt(0);
                }
                else
                {
                    //_burndownSerie.ItemsSource = null;
                    laggingSerie.ItemsSource = null;
                    _capacitySerie.ItemsSource = null;
                    workloadSerie.ItemsSource = null;
                    workloadLateSerie.ItemsSource = null;
                }


            }
            catch (Exception ex)
            {

            }

            return tmpData;
        }




        private class ChartDataValues
        {
            public decimal noCars = 0;
            public decimal weight = 0;
            public decimal volume = 0;

            public decimal noCarsLate = 0;
            public decimal weightLate = 0;
            public decimal volumeLate = 0;

            public decimal LaggingCars = 0;
            public decimal Laggingweight = 0;
            public decimal Laggingvolume = 0;

            public int noUsersCapacity = 0;
            public decimal noCarsCapacity = 0;
            public decimal weightCapacity = 0;
            public decimal volumeCapacity = 0;


            public int noUsersCapacitySaldo = 0;

            //public decimal noCarsBurnDown = 0;
            //public decimal weightBurnDown = 0;
            //public decimal volumeBurnDown = 0;

            public DateTime date = DateTime.Now;


            public void SetDateTimeForNewTimeSlot(DateTime newDateTime)
            {
                date = newDateTime;
            }

            public void ResetValues()
            {
                noCars = 0;
                weight = 0;
                volume = 0;
                noCarsLate = 0;
                weightLate = 0;
                volumeLate = 0;
                noUsersCapacity = 0;
                noCarsCapacity = 0;
                weightCapacity = 0;
                volumeCapacity = 0;
            }

            public void LoadSaldoToNewTimeSlot(LoadingMonitorOverviewViewResult data)
            {
                noUsersCapacity = noUsersCapacitySaldo;
            }

            public void SetLaggingValues(LoadingMonitorOverviewViewResult data)
            {
                LaggingCars = data.NoCarsLate.GetValueOrDefault();
                Laggingweight = data.WeightLate.GetValueOrDefault();
                Laggingvolume = data.VolumeLate.GetValueOrDefault();
            }

            public void IncValuesInTimeSlot(LoadingMonitorOverviewViewResult data)
            {
                noCars += data.NoCars.GetValueOrDefault();
                weight += data.Weight.GetValueOrDefault();
                volume += data.Volume.GetValueOrDefault();

                noCarsLate += data.NoCarsLate.GetValueOrDefault();
                weightLate += data.WeightLate.GetValueOrDefault();
                volumeLate += data.VolumeLate.GetValueOrDefault();

                noUsersCapacity += data.NoUsersInc.GetValueOrDefault();
                noCarsCapacity += data.NoCarsCapacity.GetValueOrDefault();
                weightCapacity += data.WeightCapacity.GetValueOrDefault();
                volumeCapacity += data.VolumeCapacity.GetValueOrDefault();

                //noCarsBurnDown += data.NoCarsInc.GetValueOrDefault() + data.NoCarsLateInc.GetValueOrDefault();
                //weightBurnDown += data.WeightInc.GetValueOrDefault() + data.WeightLateInc.GetValueOrDefault();
                //volumeBurnDown += data.VolumeInc.GetValueOrDefault() + data.VolumeLateInc.GetValueOrDefault();
            }

            public void RecalcSaldoInTimeSlot(LoadingMonitorOverviewViewResult data)
            {
                noUsersCapacitySaldo += (data.NoUsersInc.GetValueOrDefault() - data.NoUsersDec.GetValueOrDefault());;
            }

            public void CalcBurnDown()
            {
                //noCarsBurnDown -= noCarsCapacity;
                //weightBurnDown -= weightCapacity;
                //volumeBurnDown -= volumeCapacity;
            }
        }
    }

    public class LoadingMonitorControllerSettingsProvider : Framework.UX.Settings.ISettingsProvider
    {

        #region ISettingsProvider Members

        public string GetKey(object target)
        {
            return ((ILoadingMonitorOverviewView)target).GetType().Name;
        }

        public Type GetSettingsType(object target)
        {
            return typeof(LoadingMonitorControllerSettingsRepository);
        }

        public void LoadSettings(object target, object settings)
        {
            LoadingMonitorControllerSettingsRepository repository = new LoadingMonitorControllerSettingsRepository();

            if (settings != null)
                repository = (LoadingMonitorControllerSettingsRepository)settings;

            ((ILoadingMonitorOverviewView)target).LoadFavoriteSettings(repository);
        }

        public object SaveSettings(object target)
        {
            LoadingMonitorControllerSettingsRepository settings = new LoadingMonitorControllerSettingsRepository();

            ((ILoadingMonitorOverviewView)target).SaveFavoriteSettings(settings);

            return settings;
        }

        #endregion
    }

    [Serializable]
    public class LoadingMonitorControllerSettingsRepository
    {
        public Size TheChart_Zoom { get; set; }
        public double TheChart_PanFaktor { get; set; }

        public int typeCombo_SelectedIndex { get; set; }

        public int DurationCombo_SelectedIndex { get; set; }

        public bool CapacityCheckBox_IsChecked { get; set; }
    }
}
