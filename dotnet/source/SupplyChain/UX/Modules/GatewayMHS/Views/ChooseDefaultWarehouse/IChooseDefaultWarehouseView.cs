using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.GatewayMHS.Views.ChooseDefaultWarehouse
{
    public interface IChooseDefaultWarehouseView : IDataView, IBuilderAware
    {
        bool? ShowDialog();
        void Close();
        string SelectedWarehouseId { get; set; }
    }
}
