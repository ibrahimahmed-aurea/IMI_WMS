using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.DataAccess;
using Imi.SupplyChain.OutputManager.Alarm.BusinessEntities;
using Imi.SupplyChain.OutputManager.Alarm.DataAccess;
using System.Configuration;

namespace Imi.SupplyChain.OutputManager.Alarm.BusinessLogic
{
    public class FindAlarmTextAction : MarshalByRefObject
    {
        private const string schemaName = "OMUSER";
        
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
