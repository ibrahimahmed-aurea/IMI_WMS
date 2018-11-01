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

namespace Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor
{

    public partial class DepartureMonitorDetailView : UserControl, IDepartureMonitorDetailView, IActionProvider, ISmartPartInfoProvider, IBuilderAware
    {
        public void UpdateRowCount(long? rowsInGrid, long? totalRows, bool isExport = false)
        {
            if (!isExport)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    NoNameStatusBar.SetRowCountInforamtion(rowsInGrid, totalRows);
                }));
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    NoNameStatusBar.SetExportRowCount(totalRows.Value);
                }));
            }
        }
    }
}
