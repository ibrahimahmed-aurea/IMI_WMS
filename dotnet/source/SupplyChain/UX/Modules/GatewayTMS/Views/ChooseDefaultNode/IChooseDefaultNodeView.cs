using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.GatewayTMS.Views
{
    public interface IChooseDefaultNodeView : IDataView, IBuilderAware
    {
        bool? ShowDialog();
        void Close();
        void SelectNode(object userNode);
    }
}
