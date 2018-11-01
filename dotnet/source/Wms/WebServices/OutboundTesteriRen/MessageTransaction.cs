using System;
using Imi.Wms.PLSQLInterface;
using Imi.Framework.Job.Data;
using System.Data.Common;
using System.Data;


namespace Imi.Wms.WebServices.ExternalInterface
{
    public class MessageTransaction
    {
        private string companyId;
        private Database db;

        public string CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        private string hapiTransId;

        public string HapiTransId
        {
            get { return hapiTransId; }
            set { hapiTransId = value; }
        }

        private int hapiTransSeq;

        public int HapiTransSeq
        {
            get { return hapiTransSeq; }
            set { hapiTransSeq = value; }
        }

        public IDbTransaction Transaction
        {
            get { return db.CurrentTransaction; }
        }


        public MessageTransaction(string ChannelId, string TransactionId, string HAPIObjectName, Database db)
        {
            this.db = db;
            companyId = ChannelId;
            hapiTransId = TransactionId;
            hapiTransSeq = 1;
        }
    }
}
