using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.OutputManager.Alarm.BusinessEntities;

namespace Imi.SupplyChain.OutputManager.Alarm.DataAccess
{
    public interface IAlarmDao
    {
        IList<FindAlarmTextResult> FindAlarmText(FindAlarmTextParams parameters);
    }
}
