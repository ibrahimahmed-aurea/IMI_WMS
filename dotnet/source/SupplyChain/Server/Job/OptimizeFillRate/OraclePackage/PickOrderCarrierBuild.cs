/*
  File           : Pickordercarrierbuild.cs

  Description    : Wrapper class for Oracle package PICKORDERCARRIERBUILD.
                   This code was generated by the PackageGenerator, do not edit.

  Generated with Commandline: PickOrderCarrierBuild .\PickOrderCarrierBuild.cs Imi.SupplyChain.Server.Job.LCA /conn "data source=WHTRUNK;user id=owuser;password=owuser;pooling=false;enlist=false" 

PICKORDERCARRIERBUILD.GET_CARRIERTYPE_TAB cannot be generated since the following
parameter types are not currently supported:
     PL/SQL RECORD

PICKORDERCARRIERBUILD.GET_OPTIMIZED_CARRIERTYPE cannot be generated since the following
parameter types are not currently supported:
     PL/SQL RECORD

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

namespace Imi.SupplyChain.Server.Job.OptimizeFillRate
{
    public partial class Pickordercarrierbuild
    {
        private IDbConnectionProvider connectionProvider;

        public string _Debug()
        {
            return "Package        : PICKORDERCARRIERBUILD\r\n" +
                    "Generated on   : 2013-12-10 08:45:55\r\n" +
                    "Generated by   : SWG\\thro@SE0131D\r\n" +
                    "Generated in   : C:\\cc_storage\\thro_latest_ss\\dotnet\\source\\CodeGenerators\\PackageGenerator\\bin\\Debug\r\n";
        }

        public Pickordercarrierbuild(IDbConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        private IDbCommand sp_AddErrorMessage;
        private IDbCommand sp_Build;
        private IDbCommand sp_CancelGroup;
        private IDbCommand sp_ConnectGroupRowToCarrier;
        private IDbCommand sp_FinishGroup;
        private IDbCommand sp_GetAlarmText;
        private IDbCommand sp_GetGroup;
        private IDbCommand sp_GetSourceVersionInfo;
        private IDbCommand currentCommand;
        private object syncLock = new object();

        // ----------------------------------------------------------------------------

        private void CreateSP_AddErrorMessage()
        {
            IDbDataParameter p;

            sp_AddErrorMessage = connectionProvider.GetConnection().CreateCommand();
            sp_AddErrorMessage.CommandText = "PICKORDERCARRIERBUILD.ADD_ERROR_MESSAGE";
            sp_AddErrorMessage.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)sp_AddErrorMessage).BindByName = true;
#endif

            p = sp_AddErrorMessage.CreateParameter();
            p.ParameterName = "PBROWGRP_ID_I";
            p.DbType = DbType.String;
            p.Size = 140;
            p.Direction = ParameterDirection.Input;
            sp_AddErrorMessage.Parameters.Add(p);

            p = sp_AddErrorMessage.CreateParameter();
            p.ParameterName = "ERROR_MSG_I";
            p.DbType = DbType.String;
            p.Size = 255;
            p.Direction = ParameterDirection.Input;
            sp_AddErrorMessage.Parameters.Add(p);
        }

        private void CreateSP_Build()
        {
            IDbDataParameter p;

            sp_Build = connectionProvider.GetConnection().CreateCommand();
            sp_Build.CommandText = "PICKORDERCARRIERBUILD.BUILD";
            sp_Build.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)sp_Build).BindByName = true;
#endif

            p = sp_Build.CreateParameter();
            p.ParameterName = "COMPANY_ID_I";
            p.DbType = DbType.String;
            p.Size = 68;
            p.Direction = ParameterDirection.Input;
            sp_Build.Parameters.Add(p);

            p = sp_Build.CreateParameter();
            p.ParameterName = "COID_I";
            p.DbType = DbType.String;
            p.Size = 140;
            p.Direction = ParameterDirection.Input;
            sp_Build.Parameters.Add(p);

            p = sp_Build.CreateParameter();
            p.ParameterName = "COSEQ_I";
            p.DbType = DbType.Decimal;
            p.Precision = 3;
            p.Scale = 0;
            p.Direction = ParameterDirection.Input;
            sp_Build.Parameters.Add(p);

            p = sp_Build.CreateParameter();
            p.ParameterName = "COSUBSEQ_I";
            p.DbType = DbType.Decimal;
            p.Precision = 4;
            p.Scale = 0;
            p.Direction = ParameterDirection.Input;
            sp_Build.Parameters.Add(p);

            p = sp_Build.CreateParameter();
            p.ParameterName = "REBUILD_CASE_I";
            p.DbType = DbType.Boolean;
            p.Direction = ParameterDirection.Input;
            sp_Build.Parameters.Add(p);
        }

        private void CreateSP_CancelGroup()
        {
            IDbDataParameter p;

            sp_CancelGroup = connectionProvider.GetConnection().CreateCommand();
            sp_CancelGroup.CommandText = "PICKORDERCARRIERBUILD.CANCEL_GROUP";
            sp_CancelGroup.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)sp_CancelGroup).BindByName = true;
#endif

            p = sp_CancelGroup.CreateParameter();
            p.ParameterName = "PBROWGRP_ID_I";
            p.DbType = DbType.String;
            p.Size = 140;
            p.Direction = ParameterDirection.Input;
            sp_CancelGroup.Parameters.Add(p);
        }

        private void CreateSP_ConnectGroupRowToCarrier()
        {
            IDbDataParameter p;

            sp_ConnectGroupRowToCarrier = connectionProvider.GetConnection().CreateCommand();
            sp_ConnectGroupRowToCarrier.CommandText = "PICKORDERCARRIERBUILD.CONNECT_GROUP_ROW_TO_CARRIER";
            sp_ConnectGroupRowToCarrier.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)sp_ConnectGroupRowToCarrier).BindByName = true;
#endif

            p = sp_ConnectGroupRowToCarrier.CreateParameter();
            p.ParameterName = "PBROWID_I";
            p.DbType = DbType.String;
            p.Size = 140;
            p.Direction = ParameterDirection.Input;
            sp_ConnectGroupRowToCarrier.Parameters.Add(p);

            p = sp_ConnectGroupRowToCarrier.CreateParameter();
            p.ParameterName = "PBROWGRP_ID_I";
            p.DbType = DbType.String;
            p.Size = 140;
            p.Direction = ParameterDirection.Input;
            sp_ConnectGroupRowToCarrier.Parameters.Add(p);

            p = sp_ConnectGroupRowToCarrier.CreateParameter();
            p.ParameterName = "ROWSPLIT_ID_I";
            p.DbType = DbType.String;
            p.Size = 140;
            p.Direction = ParameterDirection.Input;
            sp_ConnectGroupRowToCarrier.Parameters.Add(p);

            p = sp_ConnectGroupRowToCarrier.CreateParameter();
            p.ParameterName = "ORDQTY_I";
            p.DbType = DbType.Decimal;
            p.Precision = 20;
            p.Scale = 6;
            p.Direction = ParameterDirection.Input;
            sp_ConnectGroupRowToCarrier.Parameters.Add(p);

            p = sp_ConnectGroupRowToCarrier.CreateParameter();
            p.ParameterName = "PBCARID_VIRTUAL_I";
            p.DbType = DbType.String;
            p.Size = 140;
            p.Direction = ParameterDirection.Input;
            sp_ConnectGroupRowToCarrier.Parameters.Add(p);

            p = sp_ConnectGroupRowToCarrier.CreateParameter();
            p.ParameterName = "CARTYPID_I";
            p.DbType = DbType.String;
            p.Size = 12;
            p.Direction = ParameterDirection.Input;
            sp_ConnectGroupRowToCarrier.Parameters.Add(p);
        }

        private void CreateSP_FinishGroup()
        {
            IDbDataParameter p;

            sp_FinishGroup = connectionProvider.GetConnection().CreateCommand();
            sp_FinishGroup.CommandText = "PICKORDERCARRIERBUILD.FINISH_GROUP";
            sp_FinishGroup.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)sp_FinishGroup).BindByName = true;
#endif

            p = sp_FinishGroup.CreateParameter();
            p.ParameterName = "PBROWGRP_ID_I";
            p.DbType = DbType.String;
            p.Size = 140;
            p.Direction = ParameterDirection.Input;
            sp_FinishGroup.Parameters.Add(p);
        }

        private void CreateSP_GetAlarmText()
        {
            IDbDataParameter p;

            sp_GetAlarmText = connectionProvider.GetConnection().CreateCommand();
            sp_GetAlarmText.CommandText = "PICKORDERCARRIERBUILD.GET_ALARM_TEXT";
            sp_GetAlarmText.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)sp_GetAlarmText).BindByName = true;
#endif

            p = sp_GetAlarmText.CreateParameter();
            p.ParameterName = "PBROWGRP_ID_I";
            p.DbType = DbType.String;
            p.Size = 140;
            p.Direction = ParameterDirection.Input;
            sp_GetAlarmText.Parameters.Add(p);

            p = sp_GetAlarmText.CreateParameter();
            p.ParameterName = "ALMID_I";
            p.DbType = DbType.String;
            p.Size = 140;
            p.Direction = ParameterDirection.Input;
            sp_GetAlarmText.Parameters.Add(p);

            p = sp_GetAlarmText.CreateParameter();
            p.ParameterName = "ALMTXT_O";
            p.DbType = DbType.String;
            p.Size = 1200;
            p.Direction = ParameterDirection.Output;
            sp_GetAlarmText.Parameters.Add(p);
        }

        private void CreateSP_GetGroup()
        {
            IDbDataParameter p;
            OracleParameter oP;

            sp_GetGroup = connectionProvider.GetConnection().CreateCommand();
            sp_GetGroup.CommandText = "PICKORDERCARRIERBUILD.GET_GROUP";
            sp_GetGroup.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)sp_GetGroup).BindByName = true;
#endif

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "TIMEOUT_I";
            p.DbType = DbType.Double;
            p.Direction = ParameterDirection.Input;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "PBROWGRP_ID_O";
            p.DbType = DbType.String;
            p.Size = 140;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "OFR_DISTANCE_FACTOR_O";
            p.DbType = DbType.Decimal;
            p.Precision = 6;
            p.Scale = 2;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "OFR_MAX_ITER_FILL_O";
            p.DbType = DbType.Decimal;
            p.Precision = 9;
            p.Scale = 0;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "OFR_MAX_TME_FILL_O";
            p.DbType = DbType.Decimal;
            p.Precision = 9;
            p.Scale = 0;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "OFR_MAX_ITER_STORE_O";
            p.DbType = DbType.Decimal;
            p.Precision = 9;
            p.Scale = 0;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "OFR_MAX_TME_STORE_O";
            p.DbType = DbType.Decimal;
            p.Precision = 9;
            p.Scale = 0;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "OFR_MAX_ITER_DIST_O";
            p.DbType = DbType.Decimal;
            p.Precision = 9;
            p.Scale = 0;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "OFR_MAX_TME_DIST_O";
            p.DbType = DbType.Decimal;
            p.Precision = 9;
            p.Scale = 0;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "PZID_O";
            p.DbType = DbType.String;
            p.Size = 48;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "WHID_O";
            p.DbType = DbType.String;
            p.Size = 16;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "ALLOW_PBROWSPLIT_O";
            p.DbType = DbType.String;
            p.Size = 4;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "MAXPBROWCAR_O";
            p.DbType = DbType.Decimal;
            p.Precision = 6;
            p.Scale = 0;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "CARTYPID_O";
            p.DbType = DbType.String;
            p.Size = 12;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "MAXLDVOL_O";
            p.DbType = DbType.Decimal;
            p.Precision = 19;
            p.Scale = 9;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "MAXLDWGT_O";
            p.DbType = DbType.Decimal;
            p.Precision = 16;
            p.Scale = 6;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "SHIPWSID_O";
            p.DbType = DbType.String;
            p.Size = 12;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "SHIPXCORD_O";
            p.DbType = DbType.Decimal;
            p.Precision = 6;
            p.Scale = 2;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            p = sp_GetGroup.CreateParameter();
            p.ParameterName = "SHIPYCORD_O";
            p.DbType = DbType.Decimal;
            p.Precision = 6;
            p.Scale = 2;
            p.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(p);

            oP = sp_GetGroup.CreateParameter() as OracleParameter;
            oP.ParameterName = "AISLE_WAYPOINT_CUR_O";
#if ODP_NET
            oP.OracleDbType = OracleDbType.RefCursor;
#else
      oP.OracleType = OracleType.Cursor;
#endif
            oP.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(oP);

            oP = sp_GetGroup.CreateParameter() as OracleParameter;
            oP.ParameterName = "PBROW_CUR_O";
#if ODP_NET
            oP.OracleDbType = OracleDbType.RefCursor;
#else
      oP.OracleType = OracleType.Cursor;
#endif
            oP.Direction = ParameterDirection.Output;
            sp_GetGroup.Parameters.Add(oP);
        }

        private void CreateSP_GetSourceVersionInfo()
        {
            IDbDataParameter p;

            sp_GetSourceVersionInfo = connectionProvider.GetConnection().CreateCommand();
            sp_GetSourceVersionInfo.CommandText = "PICKORDERCARRIERBUILD.GET_SOURCE_VERSION_INFO";
            sp_GetSourceVersionInfo.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)sp_GetSourceVersionInfo).BindByName = true;
#endif

            p = sp_GetSourceVersionInfo.CreateParameter();
            p.ParameterName = "";
            p.DbType = DbType.String;
            p.Size = 255;
            p.Direction = ParameterDirection.ReturnValue;
            sp_GetSourceVersionInfo.Parameters.Add(p);
        }

        // ----------------------------------------------------------------------------

        public void AddErrorMessage(string PBROWGRP_ID_I,
                                    string ERROR_MSG_I)
        {
            if (sp_AddErrorMessage == null)
                CreateSP_AddErrorMessage();

            sp_AddErrorMessage.Transaction = connectionProvider.CurrentTransaction;

            // Set In Parameters

            if (String.IsNullOrEmpty(PBROWGRP_ID_I))
                (sp_AddErrorMessage.Parameters["PBROWGRP_ID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_AddErrorMessage.Parameters["PBROWGRP_ID_I"] as IDbDataParameter).Value = PBROWGRP_ID_I;

            if (String.IsNullOrEmpty(ERROR_MSG_I))
                (sp_AddErrorMessage.Parameters["ERROR_MSG_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_AddErrorMessage.Parameters["ERROR_MSG_I"] as IDbDataParameter).Value = ERROR_MSG_I;

            // Execute stored procedure

            sp_AddErrorMessage.Prepare();
            sp_AddErrorMessage.ExecuteNonQuery();

            // Set Out Parameters
        }

        public void Build(string COMPANY_ID_I,
                          string COID_I,
                          Nullable<int> COSEQ_I,
                          Nullable<int> COSUBSEQ_I,
                          Nullable<bool> REBUILD_CASE_I)
        {
            if (sp_Build == null)
                CreateSP_Build();

            sp_Build.Transaction = connectionProvider.CurrentTransaction;

            // Set In Parameters

            if (String.IsNullOrEmpty(COMPANY_ID_I))
                (sp_Build.Parameters["COMPANY_ID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_Build.Parameters["COMPANY_ID_I"] as IDbDataParameter).Value = COMPANY_ID_I;

            if (String.IsNullOrEmpty(COID_I))
                (sp_Build.Parameters["COID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_Build.Parameters["COID_I"] as IDbDataParameter).Value = COID_I;

            if (COSEQ_I == null)
                (sp_Build.Parameters["COSEQ_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_Build.Parameters["COSEQ_I"] as IDbDataParameter).Value = COSEQ_I;

            if (COSUBSEQ_I == null)
                (sp_Build.Parameters["COSUBSEQ_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_Build.Parameters["COSUBSEQ_I"] as IDbDataParameter).Value = COSUBSEQ_I;

            if (REBUILD_CASE_I == null)
                (sp_Build.Parameters["REBUILD_CASE_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_Build.Parameters["REBUILD_CASE_I"] as IDbDataParameter).Value = REBUILD_CASE_I;

            // Execute stored procedure

            sp_Build.Prepare();
            sp_Build.ExecuteNonQuery();

            // Set Out Parameters
        }

        public void CancelGroup(string PBROWGRP_ID_I)
        {
            if (sp_CancelGroup == null)
                CreateSP_CancelGroup();

            sp_CancelGroup.Transaction = connectionProvider.CurrentTransaction;

            // Set In Parameters

            if (String.IsNullOrEmpty(PBROWGRP_ID_I))
                (sp_CancelGroup.Parameters["PBROWGRP_ID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_CancelGroup.Parameters["PBROWGRP_ID_I"] as IDbDataParameter).Value = PBROWGRP_ID_I;

            // Execute stored procedure

            sp_CancelGroup.Prepare();
            sp_CancelGroup.ExecuteNonQuery();

            // Set Out Parameters
        }

        public void ConnectGroupRowToCarrier(string PBROWID_I,
                                             string PBROWGRP_ID_I,
                                             string ROWSPLIT_ID_I,
                                             Nullable<double> ORDQTY_I,
                                             string PBCARID_VIRTUAL_I,
                                             string CARTYPID_I)
        {
            if (sp_ConnectGroupRowToCarrier == null)
                CreateSP_ConnectGroupRowToCarrier();

            sp_ConnectGroupRowToCarrier.Transaction = connectionProvider.CurrentTransaction;

            // Set In Parameters

            if (String.IsNullOrEmpty(PBROWID_I))
                (sp_ConnectGroupRowToCarrier.Parameters["PBROWID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_ConnectGroupRowToCarrier.Parameters["PBROWID_I"] as IDbDataParameter).Value = PBROWID_I;

            if (String.IsNullOrEmpty(PBROWGRP_ID_I))
                (sp_ConnectGroupRowToCarrier.Parameters["PBROWGRP_ID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_ConnectGroupRowToCarrier.Parameters["PBROWGRP_ID_I"] as IDbDataParameter).Value = PBROWGRP_ID_I;

            if (String.IsNullOrEmpty(ROWSPLIT_ID_I))
                (sp_ConnectGroupRowToCarrier.Parameters["ROWSPLIT_ID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_ConnectGroupRowToCarrier.Parameters["ROWSPLIT_ID_I"] as IDbDataParameter).Value = ROWSPLIT_ID_I;

            if (ORDQTY_I == null)
                (sp_ConnectGroupRowToCarrier.Parameters["ORDQTY_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_ConnectGroupRowToCarrier.Parameters["ORDQTY_I"] as IDbDataParameter).Value = ORDQTY_I;

            if (String.IsNullOrEmpty(PBCARID_VIRTUAL_I))
                (sp_ConnectGroupRowToCarrier.Parameters["PBCARID_VIRTUAL_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_ConnectGroupRowToCarrier.Parameters["PBCARID_VIRTUAL_I"] as IDbDataParameter).Value = PBCARID_VIRTUAL_I;

            if (String.IsNullOrEmpty(CARTYPID_I))
                (sp_ConnectGroupRowToCarrier.Parameters["CARTYPID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_ConnectGroupRowToCarrier.Parameters["CARTYPID_I"] as IDbDataParameter).Value = CARTYPID_I;

            // Execute stored procedure

            sp_ConnectGroupRowToCarrier.Prepare();
            sp_ConnectGroupRowToCarrier.ExecuteNonQuery();

            // Set Out Parameters
        }

        public void FinishGroup(string PBROWGRP_ID_I)
        {
            if (sp_FinishGroup == null)
                CreateSP_FinishGroup();

            sp_FinishGroup.Transaction = connectionProvider.CurrentTransaction;

            // Set In Parameters

            if (String.IsNullOrEmpty(PBROWGRP_ID_I))
                (sp_FinishGroup.Parameters["PBROWGRP_ID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_FinishGroup.Parameters["PBROWGRP_ID_I"] as IDbDataParameter).Value = PBROWGRP_ID_I;

            // Execute stored procedure

            sp_FinishGroup.Prepare();
            sp_FinishGroup.ExecuteNonQuery();

            // Set Out Parameters
        }

        public void GetAlarmText(string PBROWGRP_ID_I,
                                 string ALMID_I,
                                 ref string ALMTXT_O)
        {
            if (sp_GetAlarmText == null)
                CreateSP_GetAlarmText();

            sp_GetAlarmText.Transaction = connectionProvider.CurrentTransaction;

            // Set In Parameters

            if (String.IsNullOrEmpty(PBROWGRP_ID_I))
                (sp_GetAlarmText.Parameters["PBROWGRP_ID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_GetAlarmText.Parameters["PBROWGRP_ID_I"] as IDbDataParameter).Value = PBROWGRP_ID_I;

            if (String.IsNullOrEmpty(ALMID_I))
                (sp_GetAlarmText.Parameters["ALMID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_GetAlarmText.Parameters["ALMID_I"] as IDbDataParameter).Value = ALMID_I;

            // Execute stored procedure

            sp_GetAlarmText.Prepare();
            sp_GetAlarmText.ExecuteNonQuery();

            // Set Out Parameters

            if ((sp_GetAlarmText.Parameters["ALMTXT_O"] as IDbDataParameter).Value == DBNull.Value)
                ALMTXT_O = null;
            else
                ALMTXT_O = (sp_GetAlarmText.Parameters["ALMTXT_O"] as IDbDataParameter).Value.ToString();
        }

        public void GetGroup(Nullable<int> TIMEOUT_I,
                             ref string PBROWGRP_ID_O,
                             ref Nullable<double> OFR_DISTANCE_FACTOR_O,
                             ref Nullable<int> OFR_MAX_ITER_FILL_O,
                             ref Nullable<int> OFR_MAX_TME_FILL_O,
                             ref Nullable<int> OFR_MAX_ITER_STORE_O,
                             ref Nullable<int> OFR_MAX_TME_STORE_O,
                             ref Nullable<int> OFR_MAX_ITER_DIST_O,
                             ref Nullable<int> OFR_MAX_TME_DIST_O,
                             ref string PZID_O,
                             ref string WHID_O,
                             ref string ALLOW_PBROWSPLIT_O,
                             ref Nullable<int> MAXPBROWCAR_O,
                             ref string CARTYPID_O,
                             ref Nullable<double> MAXLDVOL_O,
                             ref Nullable<double> MAXLDWGT_O,
                             ref string SHIPWSID_O,
                             ref Nullable<double> SHIPXCORD_O,
                             ref Nullable<double> SHIPYCORD_O,
                             out IDataReader AISLE_WAYPOINT_CUR_O,
                             out IDataReader PBROW_CUR_O)
        {
            if (sp_GetGroup == null)
                CreateSP_GetGroup();

            sp_GetGroup.Transaction = connectionProvider.CurrentTransaction;

            lock (syncLock)
            {
                currentCommand = sp_GetGroup;
            }
            // Set In Parameters

            if (TIMEOUT_I == null)
                (sp_GetGroup.Parameters["TIMEOUT_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_GetGroup.Parameters["TIMEOUT_I"] as IDbDataParameter).Value = TIMEOUT_I;

            // Execute stored procedure

            sp_GetGroup.Prepare();
            sp_GetGroup.ExecuteNonQuery();

            // Set Out Parameters

            if ((sp_GetGroup.Parameters["PBROWGRP_ID_O"] as IDbDataParameter).Value == DBNull.Value)
                PBROWGRP_ID_O = null;
            else
                PBROWGRP_ID_O = (sp_GetGroup.Parameters["PBROWGRP_ID_O"] as IDbDataParameter).Value.ToString();

            if ((sp_GetGroup.Parameters["OFR_DISTANCE_FACTOR_O"] as IDbDataParameter).Value == DBNull.Value)
                OFR_DISTANCE_FACTOR_O = null;
            else
                OFR_DISTANCE_FACTOR_O = Convert.ToDouble((sp_GetGroup.Parameters["OFR_DISTANCE_FACTOR_O"] as IDbDataParameter).Value);

            if ((sp_GetGroup.Parameters["OFR_MAX_ITER_FILL_O"] as IDbDataParameter).Value == DBNull.Value)
                OFR_MAX_ITER_FILL_O = null;
            else
                OFR_MAX_ITER_FILL_O = Convert.ToInt32((sp_GetGroup.Parameters["OFR_MAX_ITER_FILL_O"] as IDbDataParameter).Value);

            if ((sp_GetGroup.Parameters["OFR_MAX_TME_FILL_O"] as IDbDataParameter).Value == DBNull.Value)
                OFR_MAX_TME_FILL_O = null;
            else
                OFR_MAX_TME_FILL_O = Convert.ToInt32((sp_GetGroup.Parameters["OFR_MAX_TME_FILL_O"] as IDbDataParameter).Value);

            if ((sp_GetGroup.Parameters["OFR_MAX_ITER_STORE_O"] as IDbDataParameter).Value == DBNull.Value)
                OFR_MAX_ITER_STORE_O = null;
            else
                OFR_MAX_ITER_STORE_O = Convert.ToInt32((sp_GetGroup.Parameters["OFR_MAX_ITER_STORE_O"] as IDbDataParameter).Value);

            if ((sp_GetGroup.Parameters["OFR_MAX_TME_STORE_O"] as IDbDataParameter).Value == DBNull.Value)
                OFR_MAX_TME_STORE_O = null;
            else
                OFR_MAX_TME_STORE_O = Convert.ToInt32((sp_GetGroup.Parameters["OFR_MAX_TME_STORE_O"] as IDbDataParameter).Value);

            if ((sp_GetGroup.Parameters["OFR_MAX_ITER_DIST_O"] as IDbDataParameter).Value == DBNull.Value)
                OFR_MAX_ITER_DIST_O = null;
            else
                OFR_MAX_ITER_DIST_O = Convert.ToInt32((sp_GetGroup.Parameters["OFR_MAX_ITER_DIST_O"] as IDbDataParameter).Value);

            if ((sp_GetGroup.Parameters["OFR_MAX_TME_DIST_O"] as IDbDataParameter).Value == DBNull.Value)
                OFR_MAX_TME_DIST_O = null;
            else
                OFR_MAX_TME_DIST_O = Convert.ToInt32((sp_GetGroup.Parameters["OFR_MAX_TME_DIST_O"] as IDbDataParameter).Value);

            if ((sp_GetGroup.Parameters["PZID_O"] as IDbDataParameter).Value == DBNull.Value)
                PZID_O = null;
            else
                PZID_O = (sp_GetGroup.Parameters["PZID_O"] as IDbDataParameter).Value.ToString();

            if ((sp_GetGroup.Parameters["WHID_O"] as IDbDataParameter).Value == DBNull.Value)
                WHID_O = null;
            else
                WHID_O = (sp_GetGroup.Parameters["WHID_O"] as IDbDataParameter).Value.ToString();

            if ((sp_GetGroup.Parameters["ALLOW_PBROWSPLIT_O"] as IDbDataParameter).Value == DBNull.Value)
                ALLOW_PBROWSPLIT_O = null;
            else
                ALLOW_PBROWSPLIT_O = (sp_GetGroup.Parameters["ALLOW_PBROWSPLIT_O"] as IDbDataParameter).Value.ToString();

            if ((sp_GetGroup.Parameters["MAXPBROWCAR_O"] as IDbDataParameter).Value == DBNull.Value)
                MAXPBROWCAR_O = null;
            else
                MAXPBROWCAR_O = Convert.ToInt32((sp_GetGroup.Parameters["MAXPBROWCAR_O"] as IDbDataParameter).Value);

            if ((sp_GetGroup.Parameters["CARTYPID_O"] as IDbDataParameter).Value == DBNull.Value)
                CARTYPID_O = null;
            else
                CARTYPID_O = (sp_GetGroup.Parameters["CARTYPID_O"] as IDbDataParameter).Value.ToString();

            if ((sp_GetGroup.Parameters["MAXLDVOL_O"] as IDbDataParameter).Value == DBNull.Value)
                MAXLDVOL_O = null;
            else
                MAXLDVOL_O = Convert.ToDouble((sp_GetGroup.Parameters["MAXLDVOL_O"] as IDbDataParameter).Value);

            if ((sp_GetGroup.Parameters["MAXLDWGT_O"] as IDbDataParameter).Value == DBNull.Value)
                MAXLDWGT_O = null;
            else
                MAXLDWGT_O = Convert.ToDouble((sp_GetGroup.Parameters["MAXLDWGT_O"] as IDbDataParameter).Value);

            if ((sp_GetGroup.Parameters["SHIPWSID_O"] as IDbDataParameter).Value == DBNull.Value)
                SHIPWSID_O = null;
            else
                SHIPWSID_O = (sp_GetGroup.Parameters["SHIPWSID_O"] as IDbDataParameter).Value.ToString();

            if ((sp_GetGroup.Parameters["SHIPXCORD_O"] as IDbDataParameter).Value == DBNull.Value)
                SHIPXCORD_O = null;
            else
                SHIPXCORD_O = Convert.ToDouble((sp_GetGroup.Parameters["SHIPXCORD_O"] as IDbDataParameter).Value);

            if ((sp_GetGroup.Parameters["SHIPYCORD_O"] as IDbDataParameter).Value == DBNull.Value)
                SHIPYCORD_O = null;
            else
                SHIPYCORD_O = Convert.ToDouble((sp_GetGroup.Parameters["SHIPYCORD_O"] as IDbDataParameter).Value);

            if (!((sp_GetGroup.Parameters["AISLE_WAYPOINT_CUR_O"] as IDbDataParameter).Value as OracleRefCursor).IsNull)
            {
#if ODP_NET
                AISLE_WAYPOINT_CUR_O = (IDataReader)((sp_GetGroup.Parameters["AISLE_WAYPOINT_CUR_O"] as IDbDataParameter).Value as OracleRefCursor).GetDataReader();
#else
       AISLE_WAYPOINT_CUR_O = (IDataReader)(sp_GetGroup.Parameters["AISLE_WAYPOINT_CUR_O"] as IDbDataParameter).Value;
#endif
            }
            else
            {
                AISLE_WAYPOINT_CUR_O = null;
            }


            if (!((sp_GetGroup.Parameters["PBROW_CUR_O"] as IDbDataParameter).Value as OracleRefCursor).IsNull)
            {
#if ODP_NET
                PBROW_CUR_O = (IDataReader)((sp_GetGroup.Parameters["PBROW_CUR_O"] as IDbDataParameter).Value as OracleRefCursor).GetDataReader();
#else
       PBROW_CUR_O = (IDataReader)(sp_GetGroup.Parameters["PBROW_CUR_O"] as IDbDataParameter).Value;
#endif
            }
            else
            {
                PBROW_CUR_O = null;
            }

        }

        public string GetSourceVersionInfo()
        {
            if (sp_GetSourceVersionInfo == null)
                CreateSP_GetSourceVersionInfo();

            sp_GetSourceVersionInfo.Transaction = connectionProvider.CurrentTransaction;

            // Set In Parameters

            // Execute stored procedure

            sp_GetSourceVersionInfo.Prepare();
            sp_GetSourceVersionInfo.ExecuteNonQuery();

            // Set Out Parameters

            string ret;

            if ((sp_GetSourceVersionInfo.Parameters[""] as IDbDataParameter).Value == DBNull.Value)
                ret = null;
            else
                ret = (sp_GetSourceVersionInfo.Parameters[""] as IDbDataParameter).Value.ToString();

            return ret;
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
