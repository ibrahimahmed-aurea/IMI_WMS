using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.OutputManager.Initialization.BusinessEntities;

namespace Imi.SupplyChain.OutputManager.Initialization.DataAccess
{
    public class InitializationDao : DataAccessObject, IInitializationDao
    {
        public InitializationDao(string connectionString)
            : base(connectionString)
        { 
        }

        public IList<FindOutputManagerResult> FindAllOutputManagers()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<FindOutputManagerResult> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.OutputManager.Initialization.DataAccess.Queries.FindAllOutputManagers.sql");
                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindOutputManagersTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }             

    }
}
