using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.OutputManager.RowTracker.BusinessEntities;

namespace Imi.SupplyChain.OutputManager.RowTracker.DataAccess
{
    public interface IRowTrackerDao
    {
        FindRowIdentityResult FindRowIdentity(FindRowIdentityParameters parameters);
        void EnableTracking();
        void SetIsLastMultiSelectRow(bool isLastMultiSelectRow);
    }
}
