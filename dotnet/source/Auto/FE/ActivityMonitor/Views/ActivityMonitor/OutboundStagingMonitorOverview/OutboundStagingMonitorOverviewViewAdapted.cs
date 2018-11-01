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
    public partial class OutboundStagingMonitorOverviewView : UserControl, IOutboundStagingMonitorOverviewView, IActionProvider, ISmartPartInfoProvider, IBuilderAware
    {

        private List<OutboundStagingMonitorOverviewViewResult> _originalData = new List<OutboundStagingMonitorOverviewViewResult>();
        private Telerik.Windows.Controls.ChartView.BarSeries _capacitySerie;
        private double? panFaktor = null;


        public void SaveFavoriteSettings(OutboundStagingMonitorControllerSettingsRepository settings)
        {
            settings.TheChart_Zoom = theChart.Zoom;
            settings.TheChart_PanFaktor = theChart.PanOffset.X / theChart.PlotAreaClip.Width;
        }

        public void LoadFavoriteSettings(OutboundStagingMonitorControllerSettingsRepository settings)
        {
            theChart.Zoom = settings.TheChart_Zoom;
            panFaktor = settings.TheChart_PanFaktor;
        }

        private void theChart_Initialized(object sender, EventArgs e)
        {
            _capacitySerie = new Telerik.Windows.Controls.ChartView.BarSeries();
            _capacitySerie.DefaultVisualStyle = (Style)FindResource("capacitySeriesStyle");
            _capacitySerie.TrackBallInfoTemplate = (DataTemplate)FindResource("trackBallInfoTemplateCapacity");
            _capacitySerie.CategoryBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = "ShippingStageLocation" };
            _capacitySerie.ValueBinding = new Telerik.Windows.Controls.ChartView.PropertyNameDataPointBinding() { PropertyName = "CarSizeCapacity" };
            _capacitySerie.Visibility = System.Windows.Visibility.Visible;
            theChart.Series.Add(_capacitySerie);
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

        public void PresentData(object data)
        {
            _originalData = (List<OutboundStagingMonitorOverviewViewResult>)data;

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
        }


        private void RefreshDataInViews()
        {
            if (presenter != null)
            {
                _capacitySerie.ItemsSource = _originalData;
                workloadStagedSerie.ItemsSource = _originalData;
                workloadInProgressSerie.ItemsSource = _originalData;
                workloadPlannedSerie.ItemsSource = _originalData;

                presenter.OnViewUpdated(_originalData as IList<OutboundStagingMonitorOverviewViewResult>);
            }
        }

    }

    public class OutboundStagingMonitorControllerSettingsProvider : Framework.UX.Settings.ISettingsProvider
    {

        #region ISettingsProvider Members

        public string GetKey(object target)
        {
            return ((IOutboundStagingMonitorOverviewView)target).GetType().Name;
        }

        public Type GetSettingsType(object target)
        {
            return typeof(OutboundStagingMonitorControllerSettingsRepository);
        }

        public void LoadSettings(object target, object settings)
        {
            OutboundStagingMonitorControllerSettingsRepository repository = new OutboundStagingMonitorControllerSettingsRepository();

            if (settings != null)
                repository = (OutboundStagingMonitorControllerSettingsRepository)settings;

            ((IOutboundStagingMonitorOverviewView)target).LoadFavoriteSettings(repository);
        }

        public object SaveSettings(object target)
        {
            OutboundStagingMonitorControllerSettingsRepository settings = new OutboundStagingMonitorControllerSettingsRepository();

            ((IOutboundStagingMonitorOverviewView)target).SaveFavoriteSettings(settings);

            return settings;
        }

        #endregion
    }

    [Serializable]
    public class OutboundStagingMonitorControllerSettingsRepository
    {
        public Size TheChart_Zoom { get; set; }
        public double TheChart_PanFaktor { get; set; }
    }
}
