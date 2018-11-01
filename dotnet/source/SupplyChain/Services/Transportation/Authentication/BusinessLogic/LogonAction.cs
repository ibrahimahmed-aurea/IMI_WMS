using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Transportation.Authentication.BusinessEntities;
using Imi.Framework.DataAccess;
using System.Configuration;
using Imi.SupplyChain.Transportation.Authentication.DataAccess;
using Imi.SupplyChain.Transportation.Alarm.BusinessLogic;

namespace Imi.SupplyChain.Transportation.Authentication.BusinessLogic
{
    public class LogonAction : MarshalByRefObject
    {
        private const string schemaName = "RMUSER";

        public LogonResult Execute(LogonParameters parameters)
        {
            LogonResult result = null;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            IAuthenticationDao dao = new AuthenticationDao(connectionString);
            
            result = dao.Logon(parameters);

            if (!string.IsNullOrEmpty(result.AlarmId))
                RaiseAlarmAction.Execute(result.AlarmId);

            return result;
        }

    }
}
