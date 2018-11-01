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
    public partial class MovementMonitorOverviewView : UserControl, IMovementMonitorOverviewView, IActionProvider, ISmartPartInfoProvider, IBuilderAware
    {

        private List<MovementMonitorOverviewViewResult> _originalData = new List<MovementMonitorOverviewViewResult>();
        private int _duration = 30;
        //private Telerik.Windows.Controls.ChartView.LineSeries _capacitySerie;
        private double? panFaktor = null;


        public void SaveFavoriteSettings(MovementMonitorControllerSettingsRepository settings)
        {
            settings.TheChart_Zoom = theChart.Zoom;
            settings.TheChart_PanFaktor = theChart.PanOffset.X / theChart.PlotAreaClip.Width;
            settings.DurationCombo_SelectedIndex = DurationCombo.SelectedIndex;
        }

        public void LoadFavoriteSettings(MovementMonitorControllerSettingsRepository settings)
        {
            DurationCombo.SelectedIndex = settings.DurationCombo_SelectedIndex;
            theChart.Zoom = settings.TheChart_Zoom;
            panFaktor = settings.TheChart_PanFaktor;
        }

        private void theChart_Initialized(object sender, EventArgs e)
        {
            //_capacitySerie = new Telerik.Windows.Controls.ChartView.LineSeries();
            //_capacitySerie.DefaultVisualStyle = (Style)FindResource("capacitySeriesStyle");
            //_capacitySerie.TrackBallInfoTemplate = (DataTemplate)FindResource("trackBallInfoTemplateCapacity");
            //_capacitySerie.CategoryBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = "TimeStamp" };
            //_capacitySerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = "NoLiftTruksCapacityInc" };
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
                List<MovementMonitorOverviewViewResult> tmpData = ((List<MovementMonitorOverviewViewResult>)workloadSerie.ItemsSource);

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
            _originalData = (List<MovementMonitorOverviewViewResult>)data;

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
                IList<MovementMonitorOverviewViewResult> data;

                data = UpdateChartData();

                if (data is IList<MovementMonitorOverviewViewResult>)
                {
                    presenter.OnViewUpdated(data as IList<MovementMonitorOverviewViewResult>);
                }
                else
                {
                    presenter.OnViewUpdated(data as MovementMonitorOverviewViewResult);
                }
            }
        }



        private IList<MovementMonitorOverviewViewResult> UpdateChartData()
        {
            List<MovementMonitorOverviewViewResult> tmpData = new List<MovementMonitorOverviewViewResult>();

            ChartDataValues chartValues = new ChartDataValues();
            
            int index = 0;

            if (_originalData != null)
            {
                foreach (MovementMonitorOverviewViewResult data in _originalData)
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
                        MovementMonitorOverviewViewResult newData = new MovementMonitorOverviewViewResult();

                        newData.NoTrpAssInc = chartValues.noTrpAss;

                        newData.NoTrpAssLateInc = chartValues.noTrpAssLate;

                        newData.NoLiftTruksCapacityInc = chartValues.noLiftTruksCapacity;


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
                    MovementMonitorOverviewViewResult laggingWork = new MovementMonitorOverviewViewResult();

                    laggingWork.TimeStamp = tmpData[0].TimeStamp.GetValueOrDefault().AddMinutes(-_duration);
                    laggingWork.TimeSpanText = "<";

                    MovementMonitorOverviewViewResult laggingWorkDummy = new MovementMonitorOverviewViewResult();

                    laggingWorkDummy.TimeStamp = laggingWork.TimeStamp;
                    laggingWorkDummy.TimeSpanText = "<";

                    tmpData.Insert(0, laggingWorkDummy);

                    laggingWork.NoTrpAssLateInc = chartValues.LaggingTrpAss;

                    laggingSerie.ItemsSource = new MovementMonitorOverviewViewResult[] { laggingWork };

                    _capacitySerie.ItemsSource = tmpData;
                    workloadSerie.ItemsSource = tmpData;
                    workloadLateSerie.ItemsSource = tmpData;

                    tmpData.RemoveAt(0);
                }
                else
                {
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
            public int noTrpAss = 0;

            public int noTrpAssSaldo = 0;

            public int noTrpAssLate = 0;

            public int noTrpAssLateSaldo = 0;

            public int LaggingTrpAss = 0;

            public int noLiftTruksCapacity = 0;

            public int noLiftTruksCapacitySaldo = 0;

            public DateTime date = DateTime.Now;


            public void SetDateTimeForNewTimeSlot(DateTime newDateTime)
            {
                date = newDateTime;
            }

            public void ResetValues()
            {
                noTrpAss = 0;
                noTrpAssLate = 0;
                noLiftTruksCapacity = 0;
            }

            public void LoadSaldoToNewTimeSlot(MovementMonitorOverviewViewResult data)
            {
                noTrpAss = noTrpAssSaldo;

                noTrpAssLate = noTrpAssLateSaldo;

                noLiftTruksCapacity = noLiftTruksCapacitySaldo;
            }

            public void SetLaggingValues(MovementMonitorOverviewViewResult data)
            {
                LaggingTrpAss = data.NoTrpAssLateInc.GetValueOrDefault();
            }

            public void IncValuesInTimeSlot(MovementMonitorOverviewViewResult data)
            {
                noTrpAss += data.NoTrpAssInc.GetValueOrDefault();

                noTrpAssLate += data.NoTrpAssLateInc.GetValueOrDefault();

                noLiftTruksCapacity += data.NoLiftTruksCapacityInc.GetValueOrDefault();
            }

            public void RecalcSaldoInTimeSlot(MovementMonitorOverviewViewResult data)
            {
                noTrpAssSaldo += (data.NoTrpAssInc.GetValueOrDefault() - data.NoTrpAssDec.GetValueOrDefault());

                noTrpAssLateSaldo += (data.NoTrpAssLateInc.GetValueOrDefault() - data.NoTrpAssLateDec.GetValueOrDefault());

                noLiftTruksCapacitySaldo += (data.NoLiftTruksCapacityInc.GetValueOrDefault() - data.NoLiftTruksCapacityDec.GetValueOrDefault());
            }
        }
    }

    public class MovementMonitorControllerSettingsProvider : Framework.UX.Settings.ISettingsProvider
    {

        #region ISettingsProvider Members

        public string GetKey(object target)
        {
            return ((IMovementMonitorOverviewView)target).GetType().Name;
        }

        public Type GetSettingsType(object target)
        {
            return typeof(MovementMonitorControllerSettingsRepository);
        }

        public void LoadSettings(object target, object settings)
        {
            MovementMonitorControllerSettingsRepository repository = new MovementMonitorControllerSettingsRepository();

            if (settings != null)
                repository = (MovementMonitorControllerSettingsRepository)settings;

            ((IMovementMonitorOverviewView)target).LoadFavoriteSettings(repository);
        }

        public object SaveSettings(object target)
        {
            MovementMonitorControllerSettingsRepository settings = new MovementMonitorControllerSettingsRepository();

            ((IMovementMonitorOverviewView)target).SaveFavoriteSettings(settings);

            return settings;
        }

        #endregion
    }

    [Serializable]
    public class MovementMonitorControllerSettingsRepository
    {
        public Size TheChart_Zoom { get; set; }
        public double TheChart_PanFaktor { get; set; }

        public int DurationCombo_SelectedIndex { get; set; }
    }
}
