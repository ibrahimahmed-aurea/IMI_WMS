/*
  File           : WebServiceSend.cs

  Description    : Wrapper class for Oracle package WebServiceSend.
                   This code was generated, do not edit.

*/
#define ODP_NET
using System;
using System.Data;
#if ODP_NET
using Oracle.DataAccess.Client;
#else
using System.Data.OracleClient;
#endif
using Imi.Framework.Job.Data;

namespace Imi.SupplyChain.Server.Job.WebServiceSendiRen.PLSQLInterface
{
    public class DbWebServiceSend
    {
        private IDbConnectionProvider connectionProvider;

        public string _Debug()
        {
            return "Package        : WebServiceSend\r\n" +
              "Generated on   : 2005-09-02 10:56:22\r\n" +
              "Generated by   : IMINT1\\olla@IMIPC1091\r\n" +
              "Generated in   : C:\\TEMP\\wscc\r\n";
        }

        public DbWebServiceSend(IDbConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        private IDbCommand sp_ModifyError;
        private IDbCommand sp_ModifySend;
        private IDbCommand sp_GetHAPITRANSCur;
        private IDbCommand sp_GetWEBSPROFILECur;
        private IDbCommand currentCommand;
        private object syncLock = new object();
        // ----------------------------------------------------------------------------

        // ----------------------------------------------------------------------------

        private void CreateSP_ModifyError()
        {
            IDbDataParameter p;

            sp_ModifyError = connectionProvider.GetConnection().CreateCommand();
            sp_ModifyError.CommandText = "HAPITRANS_CONFIG.MODIFY_ERROR";
            sp_ModifyError.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)sp_ModifyError).BindByName = true;
#endif


            p = sp_ModifyError.CreateParameter();
            p.ParameterName = "HAPITRANS_ID_I";
            p.DbType = DbType.String;
            p.Size = 255;
            p.Direction = ParameterDirection.Input;
            sp_ModifyError.Parameters.Add(p);

            p = sp_ModifyError.CreateParameter();
            p.ParameterName = "FIRSTSNDDTM_I";
            p.DbType = DbType.DateTime;
            p.Direction = ParameterDirection.Input;
            sp_ModifyError.Parameters.Add(p);

            p = sp_ModifyError.CreateParameter();
            p.ParameterName = "HAPIERRCOD_I";
            p.DbType = DbType.String;
            p.Size = 255;
            p.Direction = ParameterDirection.Input;
            sp_ModifyError.Parameters.Add(p);

            p = sp_ModifyError.CreateParameter();
            p.ParameterName = "HAPIERRMSG_I";
            p.DbType = DbType.String;
            p.Size = 255;
            p.Direction = ParameterDirection.Input;
            sp_ModifyError.Parameters.Add(p);

            p = sp_ModifyError.CreateParameter();
            p.ParameterName = "ALMID_O";
            p.DbType = DbType.String;
            p.Size = 255;
            p.Direction = ParameterDirection.Output;
            sp_ModifyError.Parameters.Add(p);
        }

        private void CreateSP_ModifySend()
        {
            IDbDataParameter p;

            sp_ModifySend = connectionProvider.GetConnection().CreateCommand();
            sp_ModifySend.CommandText = "HAPITRANS_CONFIG.MODIFY_SEND";
            sp_ModifySend.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)sp_ModifySend).BindByName = true;
#endif


            p = sp_ModifySend.CreateParameter();
            p.ParameterName = "HAPITRANS_ID_I";
            p.DbType = DbType.String;
            p.Size = 14;
            p.Direction = ParameterDirection.Input;
            sp_ModifySend.Parameters.Add(p);

            p = sp_ModifySend.CreateParameter();
            p.ParameterName = "FIRSTSNDDTM_I";
            p.DbType = DbType.DateTime;
            p.Direction = ParameterDirection.Input;
            sp_ModifySend.Parameters.Add(p);

            p = sp_ModifySend.CreateParameter();
            p.ParameterName = "ALMID_O";
            p.DbType = DbType.String;
            p.Size = 35;
            p.Direction = ParameterDirection.Output;
            sp_ModifySend.Parameters.Add(p);
        }

        private void CreateSP_GetWEBSPROFILECur()
        {
            sp_GetWEBSPROFILECur = connectionProvider.GetConnection().CreateCommand();
            sp_GetWEBSPROFILECur.CommandText = "WebServiceSend.GET_WEBSPROFILE_CUR";
            sp_GetWEBSPROFILECur.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)sp_GetWEBSPROFILECur).BindByName = true;
#endif


            OracleParameter oP = sp_GetWEBSPROFILECur.CreateParameter() as OracleParameter;
            oP.ParameterName = "WEBSPROFILE_Cur_O";
#if ODP_NET
            oP.OracleDbType = OracleDbType.RefCursor;
#else
            oP.OracleType = OracleType.Cursor;
#endif
            oP.Direction = ParameterDirection.Output;
            sp_GetWEBSPROFILECur.Parameters.Add(oP);
        }

        private void CreateSP_GetHAPITRANSCur()
        {
            sp_GetHAPITRANSCur = connectionProvider.GetConnection().CreateCommand();
            sp_GetHAPITRANSCur.CommandText = "WebServiceSend.GET_HAPITRANS_CUR";
            sp_GetHAPITRANSCur.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)sp_GetHAPITRANSCur).BindByName = true;
#endif


            OracleParameter oP = sp_GetHAPITRANSCur.CreateParameter() as OracleParameter;
            oP.ParameterName = "HAPITRANS_Cur_O";
#if ODP_NET
            oP.OracleDbType = OracleDbType.RefCursor;
#else
            oP.OracleType = OracleType.Cursor;
#endif
            oP.Direction = ParameterDirection.Output;
            sp_GetHAPITRANSCur.Parameters.Add(oP);
        }

        // ----------------------------------------------------------------------------

        public void ModifyError(string HAPITRANS_ID_I,
                                Nullable<DateTime> FIRSTSNDDTM_I,
                                string HAPIERRCOD_I,
                                string HAPIERRMSG_I,
                                ref string ALMID_O)
        {
            if (sp_ModifyError == null)
                CreateSP_ModifyError();

            sp_ModifyError.Transaction = connectionProvider.CurrentTransaction;

            lock (syncLock)
            {
                currentCommand = sp_ModifyError;
            }

            // Set In Parameters
            
            if (String.IsNullOrEmpty(HAPITRANS_ID_I))
                (sp_ModifyError.Parameters["HAPITRANS_ID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_ModifyError.Parameters["HAPITRANS_ID_I"] as IDbDataParameter).Value = HAPITRANS_ID_I;

            if (FIRSTSNDDTM_I == null)
                (sp_ModifyError.Parameters["FIRSTSNDDTM_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_ModifyError.Parameters["FIRSTSNDDTM_I"] as IDbDataParameter).Value = FIRSTSNDDTM_I;

            if (String.IsNullOrEmpty(HAPIERRCOD_I))
                (sp_ModifyError.Parameters["HAPIERRCOD_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_ModifyError.Parameters["HAPIERRCOD_I"] as IDbDataParameter).Value = HAPIERRCOD_I;

            if (String.IsNullOrEmpty(HAPIERRMSG_I))
                (sp_ModifyError.Parameters["HAPIERRMSG_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_ModifyError.Parameters["HAPIERRMSG_I"] as IDbDataParameter).Value = HAPIERRMSG_I;

            // Execute stored procedure

            sp_ModifyError.Prepare();
            sp_ModifyError.ExecuteNonQuery();

            // Set Out Parameters

            if ((sp_ModifyError.Parameters["ALMID_O"] as IDbDataParameter).Value == DBNull.Value)
                ALMID_O = "";
            else
                ALMID_O = (sp_ModifyError.Parameters["ALMID_O"] as IDbDataParameter).Value.ToString();
        }

        public void ModifySend(string HAPITRANS_ID_I,
                               Nullable<DateTime> FIRSTSNDDTM_I,
                               ref string ALMID_O)
        {
            if (sp_ModifySend == null)
                CreateSP_ModifySend();

            sp_ModifySend.Transaction = connectionProvider.CurrentTransaction;

            lock (syncLock)
            {
                currentCommand = sp_ModifySend;
            }

            // Set In Parameters

            if (String.IsNullOrEmpty(HAPITRANS_ID_I))
                (sp_ModifySend.Parameters["HAPITRANS_ID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_ModifySend.Parameters["HAPITRANS_ID_I"] as IDbDataParameter).Value = HAPITRANS_ID_I;

            if (FIRSTSNDDTM_I == null)
                (sp_ModifySend.Parameters["FIRSTSNDDTM_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_ModifySend.Parameters["FIRSTSNDDTM_I"] as IDbDataParameter).Value = FIRSTSNDDTM_I;

            // Execute stored procedure

            sp_ModifySend.Prepare();
            sp_ModifySend.ExecuteNonQuery();

            // Set Out Parameters

            if ((sp_ModifySend.Parameters["ALMID_O"] as IDbDataParameter).Value == DBNull.Value)
                ALMID_O = "";
            else
                ALMID_O = (sp_ModifySend.Parameters["ALMID_O"] as IDbDataParameter).Value.ToString();
        }

        public IDataReader GetWEBSPROFILECur()
        {
            if (sp_GetWEBSPROFILECur == null)
                CreateSP_GetWEBSPROFILECur();

            sp_GetWEBSPROFILECur.Transaction = connectionProvider.CurrentTransaction; 

            lock (syncLock)
            {
                currentCommand = sp_GetWEBSPROFILECur;
            }

            // Set In Parameters

            // Execute stored procedure

            sp_GetWEBSPROFILECur.Prepare();
            IDataReader r = sp_GetWEBSPROFILECur.ExecuteReader();

            return r;
        }

        public IDataReader GetHAPITRANSCur()
        {
            if (sp_GetHAPITRANSCur == null)
                CreateSP_GetHAPITRANSCur();

            sp_GetHAPITRANSCur.Transaction = connectionProvider.CurrentTransaction; 

            lock (syncLock)
            {
                currentCommand = sp_GetHAPITRANSCur;
            }

            // Set In Parameters

            // Execute stored procedure

            sp_GetHAPITRANSCur.Prepare();
            IDataReader r = sp_GetHAPITRANSCur.ExecuteReader();

            return r;
        }

        public void Cancel()
        {
            lock (syncLock)
            {
                if (currentCommand != null)
                {
                    if (currentCommand.Connection != null)
                    {
                        if (currentCommand.Connection.State != ConnectionState.Closed)
                        {
                            currentCommand.Cancel();
                        }
                    }
                }
            }
        }

    }
}