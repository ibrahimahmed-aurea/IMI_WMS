using System;
/*using Imi.CodeGenerators.WebServices.Framework;*/
using Imi.Framework.Job.Data;
using System.Data.Common;
using System.Data;


namespace Imi.Wms.WebServices.ExternalInterfaceRMS
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
            Messagercv pkg = new Messagercv(db);
            string msgInId = "";

            // use calling procedure's transaction
            string ALMID_O = "";
            try
            {
                pkg.NewMsgIn(commPartnerId, transactionId, objectName, ref msgInId, ref ALMID_O);
            }
            catch (Exception e)
            {
                Exception ProcException = new Exception("Internal error calling NewMsgIn", e);
                throw (ProcException);
            }

            if (!String.IsNullOrEmpty(ALMID_O))
            {
                Exception e = new Exception(ALMID_O);
                throw (e);
            }
            else
            {
                this.commPartnerId = commPartnerId;
                this.msgInId = msgInId;
            }

            transSeq = 1;
        }
    }
}
