using System;
/*using Imi.CodeGenerators.WebServices.Framework;*/
using Imi.Framework.Job.Data;
using System.Data.Common;
using System.Data;


namespace Imi.Wms.WebServices.MAPIIn
{
    public class MessageTransaction
    {
        private string mapiInId;
        private Database db;

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

        public MessageTransaction(string mhId, string transactionId, string objectName, Database db)
        {
            this.db = db;
            Mapiin pkg = new Mapiin(this.db);
            mapiInId = "";

            // use calling procedure's transaction
            string ALMID_O = "";
            try
            {
                pkg.NewMapiIn(mhId, transactionId, objectName, ref mapiInId, ref ALMID_O);
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

            transSeq = 1;
        }

        public void Signal()
        {
            Mapiin pkg = new Mapiin(this.db);

            string ALMID_O = "";
            try
            {
                db.StartTransaction();
                pkg.SignalProcess("MAPIIn_01");
                db.Commit();
            }
            catch (Exception e)
            {
                Exception ProcException = new Exception("Internal error calling SignalProcess", e);
                throw (ProcException);
            }
        }
    }
}
