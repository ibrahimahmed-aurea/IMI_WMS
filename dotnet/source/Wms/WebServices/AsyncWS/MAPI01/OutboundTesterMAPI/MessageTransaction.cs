using System;
using Imi.Framework.Job.Data;
using System.Data.Common;
using System.Data;


namespace Imi.Wms.WebServices.OutboundTesterMAPI
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

        private string mapiInId;

        public string MapiInId
        {
            get { return mapiInId; }
            set { mapiInId = value; }
        }

        private int transSeq;

        public int TransSeq
        {
            get { return transSeq; }
            set { transSeq = value; }
        }

        public IDbTransaction Transaction
        {
            get { return db.CurrentTransaction; }
        }


        public MessageTransaction(string ChannelId, string TransactionId, string HAPIObjectName, Database db)
        {
            this.db = db;
            companyId = ChannelId;
            mapiInId = TransactionId;
            transSeq = 1;
        }

        public void Signal()
        {
        }
    }
}
