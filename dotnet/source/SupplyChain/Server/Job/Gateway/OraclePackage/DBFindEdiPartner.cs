using System;
using System.Data;
using System.IO;
using System.Collections;
using Imi.Framework.Job.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Imi.SupplyChain.Server.Job.Gateway.OraclePackage
{
    public class DBFindEdiPartner
    {
        private IDbConnectionProvider connectionProvider;
        private IDbCommand findCommand;
        private IDbCommand currentCommand;
        private object syncLock = new object();

        private const String FindEdiPartnerQry =
            "select" +
            " EDIPARTNER_ID, EDIPARTNER_NAME, ICHID, ICHVER, ICHFRM, ICHTO," +
            " ICHPW, ICHTST, RCVDIR, RCVDIRSAV, RCVDIRERR, SNDDIR" +
            " FROM   EDIPARTNER";

        public DBFindEdiPartner(IDbConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public IList<EdiPartner> FindEdiPartners()
        {
            IList<EdiPartner> partners = new List<EdiPartner>();

            findCommand = connectionProvider.GetConnection().CreateCommand();
            findCommand.CommandText = FindEdiPartnerQry;
            findCommand.Transaction = connectionProvider.CurrentTransaction;
            currentCommand = findCommand;

            IDataReader reader = findCommand.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    EdiPartner partner = new EdiPartner();

                    partner.Id = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                    partner.Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                    partner.MessageId = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                    partner.MessageVersion = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                    partner.MessageFrom = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                    partner.MessageTo = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                    partner.MessagePassword = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                    partner.MessageTest = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                    partner.ReceiveDirectory = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                    partner.ReceiveDirectorySave = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                    partner.ReceiveDirectoryError = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                    partner.SendDirectory = reader.IsDBNull(11) ? string.Empty : reader.GetString(11);

                    partners.Add(partner);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return partners;
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
