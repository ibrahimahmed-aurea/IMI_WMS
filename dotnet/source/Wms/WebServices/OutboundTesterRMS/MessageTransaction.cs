using System;
/*using Imi.CodeGenerators.WebServices.Framework;*/
using Imi.Framework.Job.Data;
using System.Data.Common;
using System.Data;


namespace Imi.Wms.WebServices.OutboundTesterRMS
{
    public class MessageTransaction
    {
        private string commPartnerId;
        private Database db;

        public string CommPartnerId
        {
            get { return commPartnerId; }
            set { commPartnerId = value; }
        }

        private string msgInId;

        public string MsgInId
        {
            get { return msgInId; }
            set { msgInId = value; }
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


        public MessageTransaction(string commPartnerId, string transactionId, string objectName, Database db)
        {
            this.db = db;
            msgInId = transactionId;
            this.commPartnerId = commPartnerId;

            transSeq = 1;
        }
    }
}
