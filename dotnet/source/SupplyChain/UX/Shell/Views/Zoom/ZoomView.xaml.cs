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
using System.Windows.Shapes;
using ActiproSoftware.Windows.Controls.Ribbon;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.Wpf.Controls;
using System.Windows.Controls.Primitives;

namespace Imi.SupplyChain.UX.Shell.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class ZoomView : RibbonWindow, IZoomView
    {
        private ZoomPresenter _presenter;

        [CreateNew]
        public ZoomPresenter Presenter
        {
            get { return _presenter; }
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }

        public double? ZoomLevel
        {
            get;
            set;
        }

        public ZoomView()
        {
            InitializeComponent();

            IsGlassEnabled = ((RibbonWindow)Application.Current.MainWindow).IsGlassEnabled;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
                       
            this.IsKeyboardFocusWithinChanged += (s, e) =>
            {
                balloonPopup.IsOpen = false;
            };

            this.PreviewKeyDown += (s, e) =>
            {
                balloonPopup.IsOpen = false;
            };

            this.PreviewMouseDown += (s, e) =>
            {
                balloonPopup.IsOpen = false;
            };

            if (ZoomLevel.HasValue)
            {
                if (ZoomLevel == 2.0)
                {
                    zoom200.IsChecked = true;
                }
                else if (ZoomLevel == 1.0)
                {
                    zoom100.IsChecked = true;
                }
                else if (ZoomLevel == .75)
                {
                    zoom75.IsChecked = true;
                }
                else if (ZoomLevel == .5)
                {
                    zoom50.IsChecked = true;
                }
                else
                {
                    zoomCustom.IsChecked = true;
                    zoomCustomValue.Text = ((int)(ZoomLevel * 100)).ToString();
                }
            }
        }

        private void OKButtonClick(object sender, RoutedEventArgs e)
        {
            bool error = false;
            double percent = 1.0d;
            string errorText = StringResources.About_Between50And200Error;

            try
            {
                if (string.IsNullOrEmpty(zoomCustomValue.Text) || (zoomCustomValue.Text.Length > 3))
                {
                    error = true;
                }
                else
                {
                    double d = Convert.ToDouble(zoomCustomValue.Text);

                    if ((d < 50) || (d > 200))
                        error = true;
                    else
                        percent = d;

                }

            }
            catch (Exception)
            {
                error = true;
            }

            if (!error)
            {
                this.DialogResult = true;
                _presenter.CloseView();
            }
            else
            {
                if (error)
                {
                    (balloonPopup.Child as ContentControl).Content = errorText;

                    balloonPopup.PlacementTarget = zoomCustomValue;
                    balloonPopup.Placement = PlacementMode.RelativePoint;
                    balloonPopup.VerticalOffset = -20;
                    balloonPopup.HorizontalOffset = zoomCustomValue.ActualWidth;
                    balloonPopup.IsOpen = true;
                    zoomCustomValue.Focus();
                    zoomCustomValue.SelectAll();
                }
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.CloseView();
        }


        public double? GetPercentage()
        {
            if (zoom200.IsChecked ?? false)
                return 2.0;
            if (zoom100.IsChecked ?? false)
                return 1.0;
            if (zoom75.IsChecked ?? false)
                return .75;
            if (zoom50.IsChecked ?? false)
                return .5;
            if (zoomCustom.IsChecked ?? false)
            {
                try
                {
                    double d = Convert.ToDouble(zoomCustomValue.Text);
                    d = d / 100.0;
                    return d;
                }
                catch (Exception)
                {
                }
            }

            return null;
            
        }

        private void ZoomCustomValueKeyDown(object sender, KeyEventArgs e)
        {
            balloonPopup.IsOpen = false;
        }


    }
}
