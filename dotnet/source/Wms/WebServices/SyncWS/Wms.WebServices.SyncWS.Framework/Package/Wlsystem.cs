/*
  File           : Wlsystem.cs

  Description    : Wrapper class for Oracle package WLSYSTEM.
                   This code was generated by the PackageGenerator, do not edit.

  Generated with Commandline: WLSYSTEM Wlsystem.cs Imi.Wms.WebServices.SyncWS.Framework 

*/
#define ODP_NET
using System;
using System.Data;
#if ODP_NET
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
#else
using System.Data.OracleClient;
#endif
using Imi.Framework.Job.Data;

namespace Imi.Wms.WebServices.SyncWS.Framework
{
  public partial class Wlsystem
  {
    private IDbConnectionProvider connectionProvider;

    public Wlsystem(IDbConnectionProvider connectionProvider)
    {
      this.connectionProvider = connectionProvider;
    }

    private IDbCommand sp_Getalmtxt;
    private IDbCommand sp_StartLog;

    // ----------------------------------------------------------------------------

    private void CreateSP_Getalmtxt()
    {
      IDbDataParameter p;

      sp_Getalmtxt = connectionProvider.GetConnection().CreateCommand();
      sp_Getalmtxt.CommandText = "WLSYSTEM.GETALMTXT";
      sp_Getalmtxt.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
      ((OracleCommand)sp_Getalmtxt).BindByName = true;
#endif

      p = sp_Getalmtxt.CreateParameter();
      p.ParameterName = "ALMID_I";
      p.DbType = DbType.String;
      p.Size = 35;
      p.Direction = ParameterDirection.Input;
      sp_Getalmtxt.Parameters.Add( p );

      p = sp_Getalmtxt.CreateParameter();
      p.ParameterName = "NLANGCOD_I";
      p.DbType = DbType.String;
      p.Size = 3;
      p.Direction = ParameterDirection.Input;
      sp_Getalmtxt.Parameters.Add( p );

      p = sp_Getalmtxt.CreateParameter();
      p.ParameterName = "ALMTXT_O";
      p.DbType = DbType.String;
      p.Size = 300;
      p.Direction = ParameterDirection.Output;
      sp_Getalmtxt.Parameters.Add( p );
    }

    private void CreateSP_StartLog()
    {
      IDbDataParameter p;

      sp_StartLog = connectionProvider.GetConnection().CreateCommand();
      sp_StartLog.CommandText = "WLSYSTEM.START_LOG";
      sp_StartLog.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
      ((OracleCommand)sp_StartLog).BindByName = true;
#endif

      p = sp_StartLog.CreateParameter();
      p.ParameterName = "EMPID_I";
      p.DbType = DbType.String;
      p.Size = 8;
      p.Direction = ParameterDirection.Input;
      sp_StartLog.Parameters.Add( p );

      p = sp_StartLog.CreateParameter();
      p.ParameterName = "TERID_I";
      p.DbType = DbType.String;
      p.Size = 35;
      p.Direction = ParameterDirection.Input;
      sp_StartLog.Parameters.Add( p );

      p = sp_StartLog.CreateParameter();
      p.ParameterName = "THREAD_I";
      p.DbType = DbType.String;
      p.Size = 1;
      p.Direction = ParameterDirection.Input;
      sp_StartLog.Parameters.Add( p );
    }

    // ----------------------------------------------------------------------------

    public void Getalmtxt(string                  ALMID_I, 
                          string                  NLANGCOD_I, 
                          ref string              ALMTXT_O)
    {
      if ( sp_Getalmtxt == null )
        CreateSP_Getalmtxt();

      sp_Getalmtxt.Transaction = connectionProvider.CurrentTransaction;

      // Set In Parameters

      if ( String.IsNullOrEmpty(ALMID_I) )
        (sp_Getalmtxt.Parameters["ALMID_I"] as IDbDataParameter).Value    = DBNull.Value;
      else
        (sp_Getalmtxt.Parameters["ALMID_I"] as IDbDataParameter).Value    = ALMID_I;

      if ( String.IsNullOrEmpty(NLANGCOD_I) )
        (sp_Getalmtxt.Parameters["NLANGCOD_I"] as IDbDataParameter).Value = DBNull.Value;
      else
        (sp_Getalmtxt.Parameters["NLANGCOD_I"] as IDbDataParameter).Value = NLANGCOD_I;

      // Execute stored procedure

      sp_Getalmtxt.Prepare();
      sp_Getalmtxt.ExecuteNonQuery();

      // Set Out Parameters

      if ( (sp_Getalmtxt.Parameters["ALMTXT_O"] as IDbDataParameter).Value == DBNull.Value )
        ALMTXT_O = null;
      else
        ALMTXT_O = (sp_Getalmtxt.Parameters["ALMTXT_O"] as IDbDataParameter).Value.ToString();
    }

    public void StartLog(string                  EMPID_I, 
                         string                  TERID_I, 
                         string                  THREAD_I)
    {
      if ( sp_StartLog == null )
        CreateSP_StartLog();

      sp_StartLog.Transaction = connectionProvider.CurrentTransaction;

      // Set In Parameters

      if ( String.IsNullOrEmpty(EMPID_I) )
        (sp_StartLog.Parameters["EMPID_I"] as IDbDataParameter).Value  = DBNull.Value;
      else
        (sp_StartLog.Parameters["EMPID_I"] as IDbDataParameter).Value  = EMPID_I;

      if ( String.IsNullOrEmpty(TERID_I) )
        (sp_StartLog.Parameters["TERID_I"] as IDbDataParameter).Value  = DBNull.Value;
      else
        (sp_StartLog.Parameters["TERID_I"] as IDbDataParameter).Value  = TERID_I;

      if ( String.IsNullOrEmpty(THREAD_I) )
        (sp_StartLog.Parameters["THREAD_I"] as IDbDataParameter).Value = DBNull.Value;
      else
        (sp_StartLog.Parameters["THREAD_I"] as IDbDataParameter).Value = THREAD_I;

      // Execute stored procedure

      sp_StartLog.Prepare();
      sp_StartLog.ExecuteNonQuery();

      // Set Out Parameters
    }
  }
}
