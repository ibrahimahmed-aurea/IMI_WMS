using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views
{
    public interface IChooseUserView : IDataView, IBuilderAware
    {
        bool? ShowDialog();
        void Close();
        string SelectedUserId { get; set; }
    }

}
