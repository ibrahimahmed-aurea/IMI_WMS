using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Views
{
    public interface IEntityView : IDataView
    {
        void PresentData(object data, IDictionary<string, List<string>> propertiesUsedByImport = null);
    }
}
