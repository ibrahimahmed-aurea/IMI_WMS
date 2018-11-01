using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Transportation.Alarm.BusinessEntities;

namespace Imi.SupplyChain.Transportation.Alarm.DataAccess
{
    public interface IAlarmDao
    {
        IList<FindAlarmTextResult> FindAlarmText(FindAlarmTextParams parameters);
    }
}
