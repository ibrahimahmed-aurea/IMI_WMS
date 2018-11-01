using System.Data;
using System.Data.SqlServerCe;
using NHibernate.Driver;
using NHibernate.SqlTypes;

namespace Imi.SupplyChain.Settings.DataAccess.NHibernate
{
    public class SqlServerCeDriver : global::NHibernate.Driver.SqlServerCeDriver
    {
        public override IDbCommand GenerateCommand(CommandType type, global::NHibernate.SqlCommand.SqlString sqlString, SqlType[] parameterTypes)
        {
            return base.GenerateCommand(type, sqlString, parameterTypes);
        }

        protected override void InitializeParameter(IDbDataParameter dbParam, string name, SqlType sqlType)
        {
            base.InitializeParameter(dbParam, name, sqlType);

            if (sqlType is StringClobSqlType)
            {
                var parameter = (SqlCeParameter)dbParam;
                parameter.SqlDbType = SqlDbType.NText;
            }
        }
    } 
}
