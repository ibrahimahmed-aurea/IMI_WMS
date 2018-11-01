using System;
using System.Data;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using Imi.Framework.Job.Data;

namespace Imi.SupplyChain.Server.Job.Gateway.OraclePackage
{
    public class DBGateway
    {
        private IDbConnectionProvider connectionProvider;
        private IDbCommand sp_EndOfInterchange;
        private IDbCommand sp_InvalidMessage;
        private IDbCommand sp_MessageRead;
        private IDbCommand sp_RecieveSegment;
        private IDbCommand sp_SegmentRead;
        private IDbCommand sp_Enable_Server_Log;
        private IDbCommand sp_Disable_Server_Log;
        private IDbCommand currentCommand;
        private object syncLock = new object();

        public DBGateway(IDbConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        // ----------------------------------------------------------------------------

        public static TTarget Convert<TTarget>(object value)
        {
            return (TTarget)global::System.Convert.ChangeType(value, typeof(TTarget));
        }

        public static DbType ConvertToDbType(Type type)
        {
            if (type == typeof(string))
            {
                return DbType.String;
            }
            else if (type == typeof(short))
            {
                return DbType.Decimal;
            }
            else if (type == typeof(int))
            {
                return DbType.Decimal;
            }
            else if (type == typeof(double))
            {
                return DbType.Decimal;
            }
            else if (type == typeof(decimal))
            {
                return DbType.Decimal;
            }
            else if (type == typeof(DateTime))
            {
                return DbType.DateTime;
            }
            else if (type == typeof(bool))
            {
                return DbType.String;
            }
            else
            {
                throw new NotSupportedException("The parameter type is not supported.");
            }
        }

        private void CreateSP_Enable_Server_Log()
        {
            IDbDataParameter p;

            sp_Enable_Server_Log = connectionProvider.GetConnection().CreateCommand();
            ((OracleCommand)sp_Enable_Server_Log).BindByName = true;
            sp_Enable_Server_Log.CommandText = "LOGG_OUTPUT.ENABLE_SERVER_LOG";
            sp_Enable_Server_Log.CommandType = System.Data.CommandType.StoredProcedure;
            ((OracleCommand)sp_Enable_Server_Log).FetchSize = 65536;

            p = new OracleParameter();
            p.ParameterName = "DIRECTORY_I";
            p.DbType = ConvertToDbType(typeof(string));
            p.Size = 255;
            p.Direction = ParameterDirection.Input;
            sp_Enable_Server_Log.Parameters.Add(p);

            p = new OracleParameter();
            p.ParameterName = "FILENAME_I";
            p.DbType = ConvertToDbType(typeof(string));
            p.Size = 255;
            p.Direction = ParameterDirection.Input;
            sp_Enable_Server_Log.Parameters.Add(p);

            p = new OracleParameter();
            p.ParameterName = "LEVEL_I";
            p.DbType = ConvertToDbType(typeof(double));
            p.Direction = ParameterDirection.Input;
            sp_Enable_Server_Log.Parameters.Add(p);

        }

        private void CreateSP_Disable_Server_Log()
        {
            sp_Disable_Server_Log = connectionProvider.GetConnection().CreateCommand();
            ((OracleCommand)sp_Disable_Server_Log).BindByName = true;
            sp_Disable_Server_Log.CommandText = "LOGG_OUTPUT.DISABLE_SERVER_LOG";
            sp_Disable_Server_Log.CommandType = System.Data.CommandType.StoredProcedure;
            ((OracleCommand)sp_Disable_Server_Log).FetchSize = 65536;
        }

        public void EnableServerLog(string directory,
                                    string fileName,
                                    int level)
        {

            if (sp_Enable_Server_Log == null)
                CreateSP_Enable_Server_Log();

            sp_Enable_Server_Log.Transaction = connectionProvider.CurrentTransaction;

            lock (syncLock)
            {
                currentCommand = sp_Enable_Server_Log;
            }

            // Set In Parameters

            if (string.IsNullOrEmpty(directory))
                (sp_Enable_Server_Log.Parameters["DIRECTORY_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_Enable_Server_Log.Parameters["DIRECTORY_I"] as IDbDataParameter).Value = directory;

            if (string.IsNullOrEmpty(fileName))
                (sp_Enable_Server_Log.Parameters["FILENAME_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_Enable_Server_Log.Parameters["FILENAME_I"] as IDbDataParameter).Value = fileName;

            (sp_Enable_Server_Log.Parameters["LEVEL_I"] as IDbDataParameter).Value = (double)level;

            // Execute stored procedure

            sp_Enable_Server_Log.Prepare();
            sp_Enable_Server_Log.ExecuteNonQuery();

            // No Out Parameters

        }

        public void DisableServerLog()
        {

            if (sp_Disable_Server_Log == null)
                CreateSP_Disable_Server_Log();

            sp_Disable_Server_Log.Transaction = connectionProvider.CurrentTransaction;

            lock (syncLock)
            {
                currentCommand = sp_Disable_Server_Log;
            }

            // No In Parameters

            // Execute stored procedure

            sp_Disable_Server_Log.Prepare();
            sp_Disable_Server_Log.ExecuteNonQuery();

            // No Out Parameters

        }

        private void CreateSP_EndOfInterchange()
        {
            IDbDataParameter p;

            sp_EndOfInterchange = connectionProvider.GetConnection().CreateCommand();
            ((OracleCommand)sp_EndOfInterchange).BindByName = true;
            sp_EndOfInterchange.CommandText = "GATEWAY.END_OF_INTERCHANGE";
            sp_EndOfInterchange.CommandType = System.Data.CommandType.StoredProcedure;
            ((OracleCommand)sp_EndOfInterchange).FetchSize = 65536;

            p = new OracleParameter();
            p.ParameterName = "";
            p.DbType = ConvertToDbType(typeof(double));
            p.Direction = ParameterDirection.ReturnValue;

            sp_EndOfInterchange.Parameters.Add(p);
        }

        private void CreateSP_InvalidMessage()
        {
            IDbDataParameter p;

            sp_InvalidMessage = connectionProvider.GetConnection().CreateCommand();
            ((OracleCommand)sp_InvalidMessage).BindByName = true;
            sp_InvalidMessage.CommandText = "GATEWAY.INVALID_MESSAGE";
            sp_InvalidMessage.CommandType = System.Data.CommandType.StoredProcedure;
            ((OracleCommand)sp_InvalidMessage).FetchSize = 65536;

            p = new OracleParameter();
            p.ParameterName = "EDIOUTID_I";
            p.DbType = ConvertToDbType(typeof(double));
            p.Direction = ParameterDirection.Input;
            sp_InvalidMessage.Parameters.Add(p);
        }

        private void CreateSP_MessageRead()
        {
            IDbDataParameter p;

            sp_MessageRead = connectionProvider.GetConnection().CreateCommand();
            ((OracleCommand)sp_MessageRead).BindByName = true;
            sp_MessageRead.CommandText = "GATEWAY.MESSAGE_READ";
            sp_MessageRead.CommandType = System.Data.CommandType.StoredProcedure;
            ((OracleCommand)sp_MessageRead).FetchSize = 65536;

            p = new OracleParameter();
            p.ParameterName = "EDIOUTID_O";
            p.DbType = ConvertToDbType(typeof(double));
            p.Direction = ParameterDirection.Output;
            sp_MessageRead.Parameters.Add(p);

            p = new OracleParameter();
            p.ParameterName = "EDIMSGID_O";
            p.DbType = ConvertToDbType(typeof(string));
            p.Size = 6;
            p.Direction = ParameterDirection.Output;
            sp_MessageRead.Parameters.Add(p);

            p = new OracleParameter();
            p.ParameterName = "PRESTRING_O";
            p.DbType = ConvertToDbType(typeof(string));
            p.Size = 1024;
            p.Direction = ParameterDirection.Output;
            sp_MessageRead.Parameters.Add(p);

            p = new OracleParameter();
            p.ParameterName = "POSTSTRING_O";
            p.DbType = ConvertToDbType(typeof(string));
            p.Size = 1024;
            p.Direction = ParameterDirection.Output;

            sp_MessageRead.Parameters.Add(p);

            p = new OracleParameter();
            p.ParameterName = "SNDDIR_O";
            p.DbType = ConvertToDbType(typeof(string));
            p.Size = 255;
            p.Direction = ParameterDirection.Output;

            sp_MessageRead.Parameters.Add(p);
        }

        private void CreateSP_RecieveSegment()
        {
            IDbDataParameter p;

            sp_RecieveSegment = connectionProvider.GetConnection().CreateCommand();
            ((OracleCommand)sp_RecieveSegment).BindByName = true;
            sp_RecieveSegment.CommandText = "GATEWAY.RECIEVE_SEGMENT";
            sp_RecieveSegment.CommandType = System.Data.CommandType.StoredProcedure;
            ((OracleCommand)sp_RecieveSegment).FetchSize = 65536;

            p = new OracleParameter();
            p.ParameterName = "";
            p.DbType = ConvertToDbType(typeof(double));
            p.Size = 255;
            p.Direction = ParameterDirection.ReturnValue;
            sp_RecieveSegment.Parameters.Add(p);

            p = new OracleParameter();
            p.ParameterName = "FILENAME";
            p.DbType = ConvertToDbType(typeof(string));
            p.Size = 255;
            p.Direction = ParameterDirection.Input;
            sp_RecieveSegment.Parameters.Add(p);

            p = new OracleParameter();
            p.ParameterName = "ROWNO";
            p.DbType = ConvertToDbType(typeof(double));
            p.Direction = ParameterDirection.Input;
            sp_RecieveSegment.Parameters.Add(p);

            p = new OracleParameter();
            p.ParameterName = "ONERECORD";
            p.DbType = ConvertToDbType(typeof(string));
            p.Size = 4096;
            p.Direction = ParameterDirection.Input;
            sp_RecieveSegment.Parameters.Add(p);
        }

        private void CreateSP_SegmentRead()
        {
            IDbDataParameter p;

            sp_SegmentRead = connectionProvider.GetConnection().CreateCommand();
            ((OracleCommand)sp_SegmentRead).BindByName = true;
            sp_SegmentRead.CommandText = "GATEWAY.SEGMENT_READ";
            sp_SegmentRead.CommandType = System.Data.CommandType.StoredProcedure;
            ((OracleCommand)sp_SegmentRead).FetchSize = 65536;

            p = new OracleParameter();
            p.ParameterName = "EDIOUTID_I";
            p.DbType = ConvertToDbType(typeof(double));
            p.Direction = ParameterDirection.Input;
            sp_SegmentRead.Parameters.Add(p);


            p = new OracleParameter();
            p.ParameterName = "LASTREAD_IO";
            p.DbType = ConvertToDbType(typeof(double));
            p.Direction = ParameterDirection.InputOutput;
            sp_SegmentRead.Parameters.Add(p);

            p = new OracleParameter();
            p.ParameterName = "EDIDATA_O";
            p.DbType = ConvertToDbType(typeof(string));
            p.Direction = ParameterDirection.Output;
            p.Size = 4096;
            sp_SegmentRead.Parameters.Add(p);
        }

        // ----------------------------------------------------------------------------

        public int EndOfInterchange()
        {
            if (sp_EndOfInterchange == null)
                CreateSP_EndOfInterchange();

            sp_EndOfInterchange.Transaction = connectionProvider.CurrentTransaction;

            lock (syncLock)
            {
                currentCommand = sp_EndOfInterchange;
            }

            // Set In Parameters

            // Execute stored procedure

            sp_EndOfInterchange.Prepare();
            sp_EndOfInterchange.ExecuteNonQuery();

            // Set Out Parameters

            if ((sp_EndOfInterchange.Parameters[""] as IDbDataParameter).Value == DBNull.Value)
                return -1;
            else
                return Convert<int>((sp_EndOfInterchange.Parameters[""] as IDbDataParameter).Value);
        }

        public void InvalidMessage(double? ediOutId)
        {
            if (sp_InvalidMessage == null)
                CreateSP_InvalidMessage();

            sp_InvalidMessage.Transaction = connectionProvider.CurrentTransaction;

            lock (syncLock)
            {
                currentCommand = sp_InvalidMessage;
            }

            // Set In Parameters

            if (ediOutId == null)
                (sp_InvalidMessage.Parameters["EDIOUTID_I"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_InvalidMessage.Parameters["EDIOUTID_I"] as IDbDataParameter).Value = (Decimal)ediOutId.Value;

            // Execute stored procedure

            sp_InvalidMessage.Prepare();
            sp_InvalidMessage.ExecuteNonQuery();

            // Set Out Parameters
        }

        public EdiMessage ReadNextMessage()
        {
            EdiMessage result = null;

            if (sp_MessageRead == null)
                CreateSP_MessageRead();

            sp_MessageRead.Transaction = connectionProvider.CurrentTransaction;

            lock (syncLock)
            {
                currentCommand = sp_MessageRead;
            }

            // Set In Parameters

            // Execute stored procedure

            sp_MessageRead.Prepare();
            sp_MessageRead.ExecuteNonQuery();

            // Set Out Parameters

            if ((sp_MessageRead.Parameters["EDIOUTID_O"] as IDbDataParameter).Value != DBNull.Value)
            {
                result = new EdiMessage();

                result.EdiOutId = Convert<double>((sp_MessageRead.Parameters["EDIOUTID_O"] as IDbDataParameter).Value);

                if ((sp_MessageRead.Parameters["EDIMSGID_O"] as IDbDataParameter).Value == DBNull.Value)
                    result.EdiMessageId = null;
                else
                    result.EdiMessageId = Convert<string>((sp_MessageRead.Parameters["EDIMSGID_O"] as IDbDataParameter).Value);

                if ((sp_MessageRead.Parameters["PRESTRING_O"] as IDbDataParameter).Value == DBNull.Value)
                    result.PreString = null;
                else
                    result.PreString = Convert<string>((sp_MessageRead.Parameters["PRESTRING_O"] as IDbDataParameter).Value);

                if ((sp_MessageRead.Parameters["POSTSTRING_O"] as IDbDataParameter).Value == DBNull.Value)
                    result.PostString = null;
                else
                    result.PostString = Convert<string>((sp_MessageRead.Parameters["POSTSTRING_O"] as IDbDataParameter).Value);

                if ((sp_MessageRead.Parameters["SNDDIR_O"] as IDbDataParameter).Value == DBNull.Value)
                    result.SendDirectory = null;
                else
                    result.SendDirectory = Convert<string>((sp_MessageRead.Parameters["SNDDIR_O"] as IDbDataParameter).Value);
            }

            return result;
        }

        public int WriteSegment(string fileName, int rowNumber, string segmentData)
        {
            if (sp_RecieveSegment == null)
                CreateSP_RecieveSegment();

            sp_RecieveSegment.Transaction = connectionProvider.CurrentTransaction;

            lock (syncLock)
            {
                currentCommand = sp_RecieveSegment;
            }

            // Set In Parameters

            if (string.IsNullOrEmpty(fileName))
                (sp_RecieveSegment.Parameters["FILENAME"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_RecieveSegment.Parameters["FILENAME"] as IDbDataParameter).Value = fileName;

            (sp_RecieveSegment.Parameters["ROWNO"] as IDbDataParameter).Value = (Decimal)rowNumber;

            if (string.IsNullOrEmpty(segmentData))
                (sp_RecieveSegment.Parameters["ONERECORD"] as IDbDataParameter).Value = DBNull.Value;
            else
                (sp_RecieveSegment.Parameters["ONERECORD"] as IDbDataParameter).Value = segmentData;

            // Execute stored procedure

            sp_RecieveSegment.Prepare();
            sp_RecieveSegment.ExecuteNonQuery();

            // Set Out Parameters

            if ((sp_RecieveSegment.Parameters[""] as IDbDataParameter).Value == DBNull.Value)
                return -1;
            else
            {
                return Convert<int>((sp_RecieveSegment.Parameters[""] as IDbDataParameter).Value);
            }
        }

        public EdiSegment SegmentRead(double ediOutId,
                                      double nextRowNumber)
        {
            EdiSegment result = null;

            if (sp_SegmentRead == null)
                CreateSP_SegmentRead();

            sp_SegmentRead.Transaction = connectionProvider.CurrentTransaction;

            lock (syncLock)
            {
                currentCommand = sp_SegmentRead;
            }

            // Set In Parameters
            (sp_SegmentRead.Parameters["EDIOUTID_I"] as IDbDataParameter).Value = (Decimal)ediOutId;
            (sp_SegmentRead.Parameters["LASTREAD_IO"] as IDbDataParameter).Value = (Decimal)nextRowNumber;

            // Execute stored procedure

            sp_SegmentRead.Prepare();
            sp_SegmentRead.ExecuteNonQuery();

            // Set Out Parameters
            string ediData;

            if ((sp_SegmentRead.Parameters["EDIDATA_O"] as IDbDataParameter).Value == DBNull.Value)
                ediData = null;
            else
                ediData = Convert<string>((sp_SegmentRead.Parameters["EDIDATA_O"] as IDbDataParameter).Value);

            if (!string.IsNullOrEmpty(ediData))
            {
                result = new EdiSegment();

                result.EdiOutId = ediOutId;
                result.EdiData = ediData;

                if ((sp_SegmentRead.Parameters["LASTREAD_IO"] as IDbDataParameter).Value == DBNull.Value)
                    result.NextRowNumber = 0D;
                else
                    result.NextRowNumber = Convert<double>((sp_SegmentRead.Parameters["LASTREAD_IO"] as IDbDataParameter).Value);
            }
            
            return result;
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
