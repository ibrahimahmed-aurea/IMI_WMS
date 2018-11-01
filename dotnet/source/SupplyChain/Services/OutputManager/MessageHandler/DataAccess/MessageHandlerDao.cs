using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.OutputManager.MessageHandler.BusinessEntities;
using Imi.SupplyChain.OutputManager.MessageHandler.DataAccess.Translators;

namespace Imi.SupplyChain.OutputManager.MessageHandler.DataAccess
{
    public class MessageHandlerDao : DataAccessObject, IMessageHandlerDao
    {
        public MessageHandlerDao(string connectionString)
            : base(connectionString)
        { 
        }

        public GetMessageXMLResult GetErrorWarningXML(GetMessageXMLParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                GetMessageXMLResult result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "UIMessageHandler.GetErrorWarningXML";
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in GetMessageXMLTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                        result = GetMessageXMLTranslator.TranslateResult(command.Parameters);
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public GetInformationXMLResult GetInformationXML(GetInformationXMLParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                GetInformationXMLResult result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "UIMessageHandler.GetInformationXML";
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in GetInformationXMLTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                        result = GetInformationXMLTranslator.TranslateResult(command.Parameters);
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public void Initialize(InitializeParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "UIMessageHandler.Initialize";
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in InitializeTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                    }
                }

                scope.Complete();
            }
        }

        public void Reset()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "UIMessageHandler.Reset";

                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                    }
                }

                scope.Complete();
            }
        }
    }
}
