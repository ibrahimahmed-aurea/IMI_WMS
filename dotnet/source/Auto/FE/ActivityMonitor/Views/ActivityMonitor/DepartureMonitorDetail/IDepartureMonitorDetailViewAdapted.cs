using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor
{
    public interface IDepartureMonitorDetailView : IDepartureMonitorDetailViewBase
    {
        void UpdateRowCount(long? rowsInGrid, long? totalRows, bool isExport = false);
    }
}
