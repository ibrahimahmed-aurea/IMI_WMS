using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Warehouse.RowTracker.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.RowTracker.DataAccess
{
    public interface IRowTrackerDao
    {
        FindRowIdentityResult FindRowIdentity(FindRowIdentityParameters parameters);
        void EnableTracking();
        void SetIsLastMultiSelectRow(bool isLastMultiSelectRow);
    }
}
