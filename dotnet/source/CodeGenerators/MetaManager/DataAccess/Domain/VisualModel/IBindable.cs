using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    public interface IBindable
    {
        MappedProperty MappedProperty { get; set; }
        DataSource DataSource { get; set; }
    }
}
