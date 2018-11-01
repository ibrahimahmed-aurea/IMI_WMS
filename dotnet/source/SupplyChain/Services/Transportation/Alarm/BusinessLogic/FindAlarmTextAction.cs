using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Imi.SupplyChain.Transportation.Alarm.BusinessEntities;
using Imi.SupplyChain.Transportation.Alarm.DataAccess;

namespace Imi.SupplyChain.Transportation.Alarm.BusinessLogic
{
    public class FindAlarmTextAction : MarshalByRefObject
    {
        private const string schemaName = "RMUSER";
        
        public IList<FindAlarmTextResult> Execute(FindAlarmTextParams parameters)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            IAlarmDao dao = new AlarmDao(connectionString);
            IList<FindAlarmTextResult> result = null;

            result = dao.FindAlarmText(parameters);

            return result;
        }
    }
}
