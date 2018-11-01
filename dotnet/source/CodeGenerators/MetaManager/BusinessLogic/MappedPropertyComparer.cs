using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic
{
    public class MappedPropertyComparer : IEqualityComparer<MappedProperty>
    {
        #region IEqualityComparer<MappedProperty> Members

        public bool Equals(MappedProperty x, MappedProperty y)
        {
            if (x.Source == y)
                return true;
            else
                return false;
        }

        public int GetHashCode(MappedProperty obj)
        {
            return obj.Id.GetHashCode();
        }

        #endregion
    }
}
