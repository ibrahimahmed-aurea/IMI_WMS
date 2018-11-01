using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.Transportation.Alarm.BusinessEntities;

namespace Imi.SupplyChain.Transportation.Alarm.DataAccess
{
    public class AlarmDao : DataAccessObject, IAlarmDao
    {
        public AlarmDao(string connectionString)
            : base(connectionString)
        { 
        }

        public IList<FindAlarmTextResult> FindAlarmText(FindAlarmTextParams parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<FindAlarmTextResult> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Transportation.Alarm.DataAccess.Queries.FindAlarmText.sql");
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in FindAlarmTextTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        LogDbCommand(command);

                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindAlarmTextTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }
    }
}
