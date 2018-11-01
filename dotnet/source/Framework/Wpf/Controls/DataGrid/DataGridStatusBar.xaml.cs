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

namespace Imi.Framework.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for DataGridStatusBar.xaml
    /// </summary>
    public partial class DataGridStatusBar : UserControl
    {

        public string ServerFeedStatusLabelText
        {
            get { return (string)GetValue(ServerFeedStatusLabelTextProperty); }
            set { SetValue(ServerFeedStatusLabelTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ServerFeedStatusLabelTextProperty =
            DependencyProperty.Register("ServerFeedStatusLabelText", typeof(string), typeof(DataGridStatusBar), new UIPropertyMetadata(null));



        public string ServerFeedStatusTextText
        {
            get { return (string)GetValue(ServerFeedStatusTextTextProperty); }
            set { SetValue(ServerFeedStatusTextTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ServerFeedStatusTextTextProperty =
            DependencyProperty.Register("ServerFeedStatusTextText", typeof(string), typeof(DataGridStatusBar), new UIPropertyMetadata(null));



        public string ExportStatusText
        {
            get { return (string)GetValue(ExportStatusTextProperty); }
            set { SetValue(ExportStatusTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExportStatusTextProperty =
            DependencyProperty.Register("ExportStatusText", typeof(string), typeof(DataGridStatusBar), new UIPropertyMetadata(null));

        public DataGridStatusBar()
        {
            InitializeComponent();

            this.StatusBar.DataContext = this;
        }

        private long _rowsInGrid = 0;
        private long _totalNumberOfRows = -1;

        public void SetRowCountInforamtion(long? rowsInGrid, long? totalRows)
        {
            if (rowsInGrid != null)
            {
                _rowsInGrid = rowsInGrid.GetValueOrDefault();
            }

            if (totalRows != null)
            {
                _totalNumberOfRows = totalRows.GetValueOrDefault();
            }

            RowCountLabel.Text = _rowsInGrid.ToString() + "/" + (_totalNumberOfRows == -1 ? "?" : _totalNumberOfRows.ToString());
        }

        private long _exportedRows = 0;
        private long _totalNumberOfRowsToExport = 0;

        public void SetExportRowCount(long rowCount)
        {
            _totalNumberOfRowsToExport = rowCount;
            ExportStatus.Maximum = _totalNumberOfRowsToExport;
            ExportStatus.IsIndeterminate = (_totalNumberOfRowsToExport == -1);
        }

        public void SetExportInformation(long? exportedRowsInc, bool startExport = false, bool exportFinished = false)
        {
            this.Dispatcher.Invoke((Action)(() =>
                {
                    if (exportFinished)
                    {
                        //ExportStatusBarItem.Visibility = System.Windows.Visibility.Collapsed;
                        _totalNumberOfRowsToExport = 0;
                        _exportedRows = 0;
                        ExportStatus.Minimum = 0;
                        ExportStatus.Maximum = 0;
                        System.Windows.Media.Animation.DoubleAnimation db = new System.Windows.Media.Animation.DoubleAnimation();
                        db.To = 0;
                        db.Duration = TimeSpan.FromSeconds(0.5);
                        db.AutoReverse = false;
                        db.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(1);
                        ExportStatusBarItem.BeginAnimation(System.Windows.Controls.Primitives.StatusBarItem.OpacityProperty, db);

                        return;
                    }

                    if (startExport)
                    {
                        _totalNumberOfRowsToExport = _totalNumberOfRows;
                        _exportedRows = 0;
                        ExportStatus.Minimum = 0;
                        ExportStatus.Maximum = _totalNumberOfRowsToExport;

                        ExportStatus.IsIndeterminate = (_totalNumberOfRowsToExport == -1);
                        
                        System.Windows.Media.Animation.DoubleAnimation db = new System.Windows.Media.Animation.DoubleAnimation();
                        db.To = 1;
                        db.Duration = TimeSpan.FromSeconds(0.5);
                        db.AutoReverse = false;
                        db.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(1);
                        ExportStatusBarItem.BeginAnimation(System.Windows.Controls.Primitives.StatusBarItem.OpacityProperty, db);
                    }

                    if (exportedRowsInc != null)
                    {
                        _exportedRows += exportedRowsInc.GetValueOrDefault();
                    }

                    if (_totalNumberOfRowsToExport > 0)
                    {
                        ExportStatus.Value = _exportedRows;
                    }
                }));
        }
    }
}
