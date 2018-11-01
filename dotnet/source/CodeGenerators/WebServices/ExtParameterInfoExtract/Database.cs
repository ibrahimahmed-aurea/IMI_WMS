using System;
using System.Data;
using Oracle.DataAccess.Client;

namespace pkgcc2
{
  /// <summary>
  /// Summary description for Database.
  /// </summary>
  public class Database
  {
    private OracleConnection OWDB = null;
    private static Database me = null;

    public Database(String connectionString)
    {
      OWDB = new OracleConnection();
      OWDB.ConnectionString = connectionString;
      OWDB.Open();
      me = this;
    }

    public static Database getInstance()
    {
      return(me); 
    }

    public IDataReader ExecuteReader(String sqlText) 
    {
      OracleCommand aQuery  = new OracleCommand();
      aQuery.Connection     = OWDB;
      aQuery.FetchSize      = (Int64)65536;

      aQuery.CommandText = sqlText;

      aQuery.Prepare();

      return( aQuery.ExecuteReader() );
    }
  }


}
