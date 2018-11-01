using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Warehouse.Alarm.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.Alarm.DataAccess
{
    public interface IAlarmDao
    {
        IList<FindAlarmTextResult> FindAlarmText(FindAlarmTextParams parameters);
    }
}
