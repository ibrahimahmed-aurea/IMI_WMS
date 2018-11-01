/*
  File           : Messagercv.cs

  Description    : Wrapper class for Oracle package MESSAGERCV.
                   This code was generated by the PackageGenerator, do not edit.

  Generated with Commandline: MESSAGERCV MessageRcv.cs Imi.Wms.WebServices.ExternalInterfaceRMS /conn "user id = rmuser; password = rmuser; data source = WHTRUNK" 

MESSAGERCV.GET_ALARM_MESSAGE cannot be generated since the following
parameter types are not currently supported:
    ALMTXT_REC_O PL/SQL RECORD

*/
using System;
using System.Data;
using Imi.Framework.Job.Data;

namespace Imi.Wms.WebServices.ExternalInterfaceRMS
{
  public class Messagercv
  {
    private IDbConnectionProvider connectionProvider;

    public string _Debug()
    {
      return  "Package        : MESSAGERCV\r\n" +
              "Generated on   : 2006-09-28 12:54:19\r\n" +
              "Generated by   : IMINT1\\olla@IMIPC1091\r\n" +
              "Generated in   : C:\\project\\views\\olla_dotnet_ss\\dotnet\\source\\CodeGenerators\\wscc\r\n";
    }

    public Messagercv(IDbConnectionProvider connectionProvider)
    {
      this.connectionProvider = connectionProvider;
    }

    private IDbCommand sp_Modify;
    private IDbCommand sp_NewMsgIn;
    private IDbCommand sp_Raiseapplicationerror;
    private IDbCommand sp_TransactionsRemove;
    private IDbCommand sp_TransactionsReset;
    private IDbCommand sp_TransactionsResetAll;

    // ----------------------------------------------------------------------------

    private void CreateSP_Modify()
    {
      IDbDataParameter p;

      sp_Modify = connectionProvider.GetConnection().CreateCommand();
      sp_Modify.CommandText = "MESSAGERCV.MODIFY";
      sp_Modify.CommandType = System.Data.CommandType.StoredProcedure;

      p = sp_Modify.CreateParameter();
      p.ParameterName = "MSG_IN_ID_I";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Input;
      sp_Modify.Parameters.Add( p );

      p = sp_Modify.CreateParameter();
      p.ParameterName = "FIRSTRCVDTM_I";
      p.DbType = DbType.Date;
      p.Direction = ParameterDirection.Input;
      sp_Modify.Parameters.Add( p );

      p = sp_Modify.CreateParameter();
      p.ParameterName = "ALMID_O";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Output;
      sp_Modify.Parameters.Add( p );
    }

    private void CreateSP_NewMsgIn()
    {
      IDbDataParameter p;

      sp_NewMsgIn = connectionProvider.GetConnection().CreateCommand();
      sp_NewMsgIn.CommandText = "MESSAGERCV.NEW_MSG_IN";
      sp_NewMsgIn.CommandType = System.Data.CommandType.StoredProcedure;

      p = sp_NewMsgIn.CreateParameter();
      p.ParameterName = "COMM_PARTNER_ID_I";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Input;
      sp_NewMsgIn.Parameters.Add( p );

      p = sp_NewMsgIn.CreateParameter();
      p.ParameterName = "TRANSACTION_ID_I";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Input;
      sp_NewMsgIn.Parameters.Add( p );

      p = sp_NewMsgIn.CreateParameter();
      p.ParameterName = "MSG_ID_I";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Input;
      sp_NewMsgIn.Parameters.Add( p );

      p = sp_NewMsgIn.CreateParameter();
      p.ParameterName = "MSG_IN_ID_O";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Output;
      sp_NewMsgIn.Parameters.Add( p );

      p = sp_NewMsgIn.CreateParameter();
      p.ParameterName = "ALMID_O";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Output;
      sp_NewMsgIn.Parameters.Add( p );
    }

    private void CreateSP_Raiseapplicationerror()
    {
      IDbDataParameter p;

      sp_Raiseapplicationerror = connectionProvider.GetConnection().CreateCommand();
      sp_Raiseapplicationerror.CommandText = "MESSAGERCV.RAISEAPPLICATIONERROR";
      sp_Raiseapplicationerror.CommandType = System.Data.CommandType.StoredProcedure;

      p = sp_Raiseapplicationerror.CreateParameter();
      p.ParameterName = "ALMID_I";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Input;
      sp_Raiseapplicationerror.Parameters.Add( p );
    }

    private void CreateSP_TransactionsRemove()
    {
      IDbDataParameter p;

      sp_TransactionsRemove = connectionProvider.GetConnection().CreateCommand();
      sp_TransactionsRemove.CommandText = "MESSAGERCV.TRANSACTIONS_REMOVE";
      sp_TransactionsRemove.CommandType = System.Data.CommandType.StoredProcedure;

      p = sp_TransactionsRemove.CreateParameter();
      p.ParameterName = "MSG_IN_ID_I";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Input;
      sp_TransactionsRemove.Parameters.Add( p );

      p = sp_TransactionsRemove.CreateParameter();
      p.ParameterName = "ALMID_O";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Output;
      sp_TransactionsRemove.Parameters.Add( p );
    }

    private void CreateSP_TransactionsReset()
    {
      IDbDataParameter p;

      sp_TransactionsReset = connectionProvider.GetConnection().CreateCommand();
      sp_TransactionsReset.CommandText = "MESSAGERCV.TRANSACTIONS_RESET";
      sp_TransactionsReset.CommandType = System.Data.CommandType.StoredProcedure;

      p = sp_TransactionsReset.CreateParameter();
      p.ParameterName = "MSG_IN_ID_I";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Input;
      sp_TransactionsReset.Parameters.Add( p );

      p = sp_TransactionsReset.CreateParameter();
      p.ParameterName = "ALMID_O";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Output;
      sp_TransactionsReset.Parameters.Add( p );
    }

    private void CreateSP_TransactionsResetAll()
    {
      IDbDataParameter p;

      sp_TransactionsResetAll = connectionProvider.GetConnection().CreateCommand();
      sp_TransactionsResetAll.CommandText = "MESSAGERCV.TRANSACTIONS_RESET_ALL";
      sp_TransactionsResetAll.CommandType = System.Data.CommandType.StoredProcedure;

      p = sp_TransactionsResetAll.CreateParameter();
      p.ParameterName = "ALMID_O";
      p.DbType = DbType.String;
      p.Size = 255;
      p.Direction = ParameterDirection.Output;
      sp_TransactionsResetAll.Parameters.Add( p );
    }

    // ----------------------------------------------------------------------------

    public void Modify(string                  MSG_IN_ID_I, 
                       Nullable<DateTime>      FIRSTRCVDTM_I, 
                       ref string              ALMID_O)
    {
      if ( sp_Modify == null )
        CreateSP_Modify();

      sp_Modify.Transaction = connectionProvider.CurrentTransaction;

      // Set In Parameters

      if ( String.IsNullOrEmpty(MSG_IN_ID_I) )
        (sp_Modify.Parameters["MSG_IN_ID_I"] as IDbDataParameter).Value   = DBNull.Value;
      else
        (sp_Modify.Parameters["MSG_IN_ID_I"] as IDbDataParameter).Value   = MSG_IN_ID_I;

      if ( FIRSTRCVDTM_I == null )
        (sp_Modify.Parameters["FIRSTRCVDTM_I"] as IDbDataParameter).Value = DBNull.Value;
      else
        (sp_Modify.Parameters["FIRSTRCVDTM_I"] as IDbDataParameter).Value = FIRSTRCVDTM_I;

      // Execute stored procedure

      sp_Modify.Prepare();
      sp_Modify.ExecuteNonQuery();

      // Set Out Parameters

      if ( (sp_Modify.Parameters["ALMID_O"] as IDbDataParameter).Value == DBNull.Value )
        ALMID_O = null;
      else
        ALMID_O = (sp_Modify.Parameters["ALMID_O"] as IDbDataParameter).Value.ToString();
    }

    public void NewMsgIn(string                  COMM_PARTNER_ID_I, 
                         string                  TRANSACTION_ID_I, 
                         string                  MSG_ID_I, 
                         ref string              MSG_IN_ID_O, 
                         ref string              ALMID_O)
    {
      if ( sp_NewMsgIn == null )
        CreateSP_NewMsgIn();

      sp_NewMsgIn.Transaction = connectionProvider.CurrentTransaction;

      // Set In Parameters

      if ( String.IsNullOrEmpty(COMM_PARTNER_ID_I) )
        (sp_NewMsgIn.Parameters["COMM_PARTNER_ID_I"] as IDbDataParameter).Value = DBNull.Value;
      else
        (sp_NewMsgIn.Parameters["COMM_PARTNER_ID_I"] as IDbDataParameter).Value = COMM_PARTNER_ID_I;

      if ( String.IsNullOrEmpty(TRANSACTION_ID_I) )
        (sp_NewMsgIn.Parameters["TRANSACTION_ID_I"] as IDbDataParameter).Value  = DBNull.Value;
      else
        (sp_NewMsgIn.Parameters["TRANSACTION_ID_I"] as IDbDataParameter).Value  = TRANSACTION_ID_I;

      if ( String.IsNullOrEmpty(MSG_ID_I) )
        (sp_NewMsgIn.Parameters["MSG_ID_I"] as IDbDataParameter).Value          = DBNull.Value;
      else
        (sp_NewMsgIn.Parameters["MSG_ID_I"] as IDbDataParameter).Value          = MSG_ID_I;

      // Execute stored procedure

      sp_NewMsgIn.Prepare();
      sp_NewMsgIn.ExecuteNonQuery();

      // Set Out Parameters

      if ( (sp_NewMsgIn.Parameters["MSG_IN_ID_O"] as IDbDataParameter).Value == DBNull.Value )
        MSG_IN_ID_O = null;
      else
        MSG_IN_ID_O = (sp_NewMsgIn.Parameters["MSG_IN_ID_O"] as IDbDataParameter).Value.ToString();

      if ( (sp_NewMsgIn.Parameters["ALMID_O"] as IDbDataParameter).Value == DBNull.Value )
        ALMID_O = null;
      else
        ALMID_O = (sp_NewMsgIn.Parameters["ALMID_O"] as IDbDataParameter).Value.ToString();
    }

    public void Raiseapplicationerror(string                  ALMID_I)
    {
      if ( sp_Raiseapplicationerror == null )
        CreateSP_Raiseapplicationerror();

      sp_Raiseapplicationerror.Transaction = connectionProvider.CurrentTransaction;

      // Set In Parameters

      if ( String.IsNullOrEmpty(ALMID_I) )
        (sp_Raiseapplicationerror.Parameters["ALMID_I"] as IDbDataParameter).Value = DBNull.Value;
      else
        (sp_Raiseapplicationerror.Parameters["ALMID_I"] as IDbDataParameter).Value = ALMID_I;

      // Execute stored procedure

      sp_Raiseapplicationerror.Prepare();
      sp_Raiseapplicationerror.ExecuteNonQuery();

      // Set Out Parameters
    }

    public void TransactionsRemove(string                  MSG_IN_ID_I, 
                                   ref string              ALMID_O)
    {
      if ( sp_TransactionsRemove == null )
        CreateSP_TransactionsRemove();

      sp_TransactionsRemove.Transaction = connectionProvider.CurrentTransaction;

      // Set In Parameters

      if ( String.IsNullOrEmpty(MSG_IN_ID_I) )
        (sp_TransactionsRemove.Parameters["MSG_IN_ID_I"] as IDbDataParameter).Value = DBNull.Value;
      else
        (sp_TransactionsRemove.Parameters["MSG_IN_ID_I"] as IDbDataParameter).Value = MSG_IN_ID_I;

      // Execute stored procedure

      sp_TransactionsRemove.Prepare();
      sp_TransactionsRemove.ExecuteNonQuery();

      // Set Out Parameters

      if ( (sp_TransactionsRemove.Parameters["ALMID_O"] as IDbDataParameter).Value == DBNull.Value )
        ALMID_O = null;
      else
        ALMID_O = (sp_TransactionsRemove.Parameters["ALMID_O"] as IDbDataParameter).Value.ToString();
    }

    public void TransactionsReset(string                  MSG_IN_ID_I, 
                                  ref string              ALMID_O)
    {
      if ( sp_TransactionsReset == null )
        CreateSP_TransactionsReset();

      sp_TransactionsReset.Transaction = connectionProvider.CurrentTransaction;

      // Set In Parameters

      if ( String.IsNullOrEmpty(MSG_IN_ID_I) )
        (sp_TransactionsReset.Parameters["MSG_IN_ID_I"] as IDbDataParameter).Value = DBNull.Value;
      else
        (sp_TransactionsReset.Parameters["MSG_IN_ID_I"] as IDbDataParameter).Value = MSG_IN_ID_I;

      // Execute stored procedure

      sp_TransactionsReset.Prepare();
      sp_TransactionsReset.ExecuteNonQuery();

      // Set Out Parameters

      if ( (sp_TransactionsReset.Parameters["ALMID_O"] as IDbDataParameter).Value == DBNull.Value )
        ALMID_O = null;
      else
        ALMID_O = (sp_TransactionsReset.Parameters["ALMID_O"] as IDbDataParameter).Value.ToString();
    }

    public void TransactionsResetAll(ref string              ALMID_O)
    {
      if ( sp_TransactionsResetAll == null )
        CreateSP_TransactionsResetAll();

      sp_TransactionsResetAll.Transaction = connectionProvider.CurrentTransaction;

      // Set In Parameters

      // Execute stored procedure

      sp_TransactionsResetAll.Prepare();
      sp_TransactionsResetAll.ExecuteNonQuery();

      // Set Out Parameters

      if ( (sp_TransactionsResetAll.Parameters["ALMID_O"] as IDbDataParameter).Value == DBNull.Value )
        ALMID_O = null;
      else
        ALMID_O = (sp_TransactionsResetAll.Parameters["ALMID_O"] as IDbDataParameter).Value.ToString();
    }
  }
}
