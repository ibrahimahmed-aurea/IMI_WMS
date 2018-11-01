/*
   File         :

   Description  :

   Author       : Olof Laurin, Industri-Matematik International

   Date         :

   Ancestor     :

   Revision         Sign. Date     Note
  ---------------- ----- -------  ---------------------------------------------
   5.0.2            olla  041217   Original version.

*/

using System;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace PLSQLInterface
{
  class OWDouble
  {
    public double Value;
    public bool IsNull;
    public OWDouble( double d )
    {
      Value = d;
      IsNull = false;
    }
    public OWDouble()
    {
      IsNull = true;
    }
  }

  class OWDateTime
  {
    public DateTime Value;
    public bool IsNull;
    public OWDateTime( DateTime dt )
    {
      Value = dt;
      IsNull = false;
    }
    public OWDateTime()
    {
      IsNull = true;
    }
  }

  class OWDB
  {
    private Oracle.DataAccess.Client.OracleConnection OracleConnection1;
    private OracleTransaction OracleTransaction1;
  
    public OWDB( string DataBaseName, string LoginUser, string LoginPassword )
    {
      OracleConnection1 = new Oracle.DataAccess.Client.OracleConnection();

      // Sample Connectionstring 'User Id=owuser;Password=owuser;Data source=WHTRUNK'

      OracleConnection1.ConnectionString =
        "User Id=" +
        LoginUser +
        ";Password=" +
        LoginPassword +
        ";Data source=" +
        DataBaseName +
        "";
      OracleConnection1.Open();
      OracleTransaction1 = OracleConnection1.BeginTransaction();
    }

    public Oracle.DataAccess.Client.OracleConnection DataBase()
    {
      return OracleConnection1;
    }

    public void Close()
    {
      OracleTransaction1 = null;
      OracleConnection1.Close();
      OracleConnection1 = null;
    }

    public void Commit()
    {
      OracleTransaction1.Commit();
    }

    public void Rollback()
    {
      OracleTransaction1.Rollback();
    }
  }

  abstract class OWStoredProc
  {
    OWDB FConnection;

    public abstract string _Debug();

    public OWStoredProc(OWDB conn)
    {
      FConnection = conn;
    }

    protected OWDB GetConnection()
    {
      return FConnection;
    }
  }
}
