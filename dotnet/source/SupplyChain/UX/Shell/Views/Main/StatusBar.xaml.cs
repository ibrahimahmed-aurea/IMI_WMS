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
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.Framework.UX;
using System.Windows.Threading;
using System.Timers;

namespace Imi.SupplyChain.UX.Shell.Views
{
    /// <summary>
    /// Interaction logic for StatusBar.xaml
    /// </summary>
    [SmartPart]
    public partial class StatusBar : UserControl
    {
        [EventPublication(EventTopicNames.ShowZoomDialog, PublicationScope.Global)]
        public event EventHandler<EventArgs> ZoomViewOpen;

        private IList<NotificationListBoxItem> _notificationList;
        private DateTime lastNotificationTime = DateTime.MinValue;
        private Timer _notificationTimer;

        public double ZoomLevel
        {
            get 
            { 
                return (double)GetValue(ZoomLevelProperty); 
            }
            set 
            { 
                SetValue(ZoomLevelProperty, value);
                scaleSlider.Value = value;

                if (value == 1)
                {
                    TextOptions.SetTextFormattingMode(Application.Current.MainWindow, TextFormattingMode.Display);
                }
                else
                {
                    TextOptions.SetTextFormattingMode(Application.Current.MainWindow, TextFormattingMode.Ideal);
                }
            }
        }

        // Using a DependencyProperty as the backing store for ZoomValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register("ZoomLevel", typeof(double), typeof(StatusBar), new UIPropertyMetadata(1.0d));
        
        public StatusBar()
        {
            InitializeComponent();
            
            _notificationList = new List<NotificationListBoxItem>();
            _notificationTimer = new Timer(5000);

            notificationPopup.Closed += (s, e) =>
            {
                _notificationTimer.Stop();
            };
                        
            _notificationTimer.Elapsed += (s, e) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    notificationPopup.IsOpen = false;
                }));
            };
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ZoomLevel = Math.Round(e.NewValue, 2);
        }

        private void ZoomButtonClick(object sender, ActiproSoftware.Windows.Controls.Ribbon.Controls.ExecuteRoutedEventArgs e)
        {
            OnZoomViewOpen();
        }

        public virtual void OnZoomViewOpen()
        {
            if (ZoomViewOpen != null)
                ZoomViewOpen(this, new EventArgs());
        }

        private void NotificationItemClick(object sender, RoutedEventArgs e)
        {
            notificationPopup.IsOpen = !notificationPopup.IsOpen;

            if (notificationPopup.IsOpen)
            {
                Application.Current.MainWindow.PreviewKeyDown += MainWindowPreviewKeyDownEventHandler;
                Application.Current.MainWindow.PreviewMouseDown += MainWindowPreviewMouseDownEventHandler;

                notificationListBox.Items.Clear();

                foreach (NotificationListBoxItem item in _notificationList)
                    notificationListBox.Items.Add(item);
            }
        }

        [EventSubscription(EventTopicNames.ShowNotification)]
        public void ShowNotificationEventHandler(object sender, DataEventArgs<ShellNotification> args)
        {
            if (DateTime.Now - lastNotificationTime > new TimeSpan(0, 0, 2))
            {
                notificationListBox.Items.Clear();
            }

            lastNotificationTime = DateTime.Now;
            string applicationName = sender as string;
                        
            NotificationListBoxItem item = new NotificationListBoxItem();
            item.Content = args.Data.Message;
            item.ApplicationName = applicationName;
            item.Tag = args.Data;

            if (_notificationList.Count == 10)
                _notificationList.RemoveAt(0);

            _notificationList.Insert(0, item);

            item.MouseLeftButtonUp += (s, e) =>
            {
                if (args.Data.NotificationCallback != null)
                    args.Data.NotificationCallback(args.Data);
            };

            notificationListBox.Items.Insert(0, item);

            notificationItem.Visibility = Visibility.Visible;

            Application.Current.MainWindow.PreviewKeyDown += MainWindowPreviewKeyDownEventHandler;
            Application.Current.MainWindow.PreviewMouseDown += MainWindowPreviewMouseDownEventHandler;

            notificationPopup.IsOpen = true;
            _notificationTimer.Stop();
            _notificationTimer.Start();
        }

        private void MainWindowPreviewMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (!(notificationPopup.IsMouseOver || notificationItem.IsMouseOver))
            {
                notificationPopup.IsOpen = false;
                Application.Current.MainWindow.PreviewMouseDown -= MainWindowPreviewMouseDownEventHandler;
            }
        }

        private void MainWindowPreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            notificationPopup.IsOpen = false;
            Application.Current.MainWindow.PreviewKeyDown -= MainWindowPreviewKeyDownEventHandler;
        }
        
        private void DeleteItemCommandExecutedEventHandler(object sender, ExecutedRoutedEventArgs e)
        {
            notificationListBox.Items.Remove(e.Source);
            _notificationList.Remove(e.Source as NotificationListBoxItem);

            if (notificationListBox.Items.Count == 0)
            {
                notificationPopup.IsOpen = false;
            }

            if (_notificationList.Count == 0)
            {
                notificationItem.Visibility = Visibility.Collapsed;
            }
        }
    }
}
