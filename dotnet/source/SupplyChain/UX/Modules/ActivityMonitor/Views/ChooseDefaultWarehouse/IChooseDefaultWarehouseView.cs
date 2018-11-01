using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.ActivityMonitor.Views.ChooseDefaultWarehouse
{
    public interface IChooseDefaultWarehouseView : IDataView, IBuilderAware
    {
        bool? ShowDialog();
        void Close(bool result);

        string SelectedWarehouseId { get; set; }
        string SelectedClientId { get; set; }

        bool IsClientInterfaceHAPI { get; }
        bool IsClientInterfaceWebServices { get; }
        bool IsClientInterfaceEDI { get; }

        bool hideClientId { get; set; }
    }

}
