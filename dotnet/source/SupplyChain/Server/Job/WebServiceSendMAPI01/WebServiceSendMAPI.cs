/*
 * Changelog:
 * 090317 Only handle MSG_ID that belong to MAPI01
 */
using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Generic; 
using Imi.SupplyChain.Server.Job.StandardJob;
using Imi.SupplyChain.Server.Job.WebServiceSendMAPI.PLSQLInterface;
using Imi.Framework.Job;
using Imi.Framework.Job.Data;

namespace Imi.SupplyChain.Server.Job.WebServiceSendMAPI
{
    class WebServiceSendMAPI : OracleJob
    {
        private SenderHandler         sender;

        private DbWebServiceSend dbWebServiceSend;

        public WebServiceSendMAPI(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
        }

        private List<MAPI_OUT> FindMessagesToSend()
        {
            List<MAPI_OUT> transList = new List<MAPI_OUT>();

            IDataReader r = dbWebServiceSend.GetMAPIOUTCur();

            try
            {
                while (r.Read())
                {
                    MAPI_OUT t = new MAPI_OUT();

                    if (r["MAPI_OUT_ID"] == DBNull.Value)
                        t.MAPI_OUT_ID = "";
                    else
                        t.MAPI_OUT_ID = r["MAPI_OUT_ID"] as String;

                    if (r["MHID"] == DBNull.Value)
                        t.MHID = "";
                    else
                        t.MHID = r["MHID"] as String;

                    if (r["MSG_ID"] == DBNull.Value)
                        t.MSG_ID = "";
                    else
                        t.MSG_ID = r["MSG_ID"] as String;

                    if (r["MAPI_OUT_STAT"] == DBNull.Value)
                        t.MAPI_OUT_STAT = "";
                    else
                        t.MAPI_OUT_STAT = r["MAPI_OUT_STAT"] as String;

                    if (r["SNDERRCODE"] == DBNull.Value)
                        t.SNDERRCODE = "";
                    else
                        t.SNDERRCODE = r["SNDERRCODE"] as String;

                    if (r["SNDERRMSG"] == DBNull.Value)
                        t.SNDERRMSG = "";
                    else
                        t.SNDERRMSG = r["SNDERRMSG"] as String;

                    if (r["CREATEDTM"] == DBNull.Value)
                        t.CREATEDTM = null;
                    else
                        t.CREATEDTM = (DateTime)r["CREATEDTM"];

                    if (r["FIRSTSNDDTM"] == DBNull.Value)
                        t.FIRSTSNDDTM = System.DateTime.Now;
                    else
                        t.FIRSTSNDDTM = (DateTime) r["FIRSTSNDDTM"];

                    if (r["LASTSNDDTM"] == DBNull.Value)
                        t.LASTSNDDTM = null;
                    else
                        t.LASTSNDDTM = (DateTime)r["LASTSNDDTM"];

                    if (r["NOSNDS"] == DBNull.Value)
                        t.NOSNDS = 0;
                    else
                        t.NOSNDS = Int32.Parse(r["NOSNDS"].ToString());

                    // skip orderby column here

                    if (r["URL"] == DBNull.Value)
                        t.URL = "";
                    else
                        t.URL = r["URL"] as String;

                    if ((t.MSG_ID == "MOVM_IN_01")
                        || (t.MSG_ID == "MOVM_OUT_01")
                        || (t.MSG_ID == "PRODUCT_01")
                        || (t.MSG_ID == "STAT_UPD_01"))
                    {
                        transList.Add(t);

                        Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                           String.Format("{0} - {1}", t.MAPI_OUT_ID, t.MSG_ID));
                    }
                }
            }
            finally
            {
                if (r != null)
                    r.Close();
            }

            return (transList);
        }

        private string HandleMessageType(MAPI_OUT transfer)
        {
            string error = "";

            try
            {
                Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format(" use Url = {0}", transfer.URL));

                if (String.IsNullOrEmpty(transfer.URL))
                    return("unknown url for " + transfer.MHID + " - " + transfer.MSG_ID);

                sender.Url = transfer.URL;

                switch (transfer.MSG_ID)
                {
                    case "MOVM_IN_01":
                        {
                            MovementIn_01Select handler = new MovementIn_01Select(this.db);
                            MovementIn_01Doc message = handler.Process(transfer.MAPI_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.MSG_ID);
                            else
                                sender.MovementIn_01(transfer.MHID, transfer.MAPI_OUT_ID, message);

                            break;
                        }

                    case "MOVM_OUT_01":
                        {
                            MovementOut_01Select handler = new MovementOut_01Select(this.db);
                            MovementOut_01Doc message = handler.Process(transfer.MAPI_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.MSG_ID);
                            else
                                sender.MovementOut_01(transfer.MHID, transfer.MAPI_OUT_ID, message);

                            break;
                        }

                    case "PRODUCT_01":
                        {
                            Product_01Select handler = new Product_01Select(this.db);
                            Product_01Doc message = handler.Process(transfer.MAPI_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.MSG_ID);
                            else
                                sender.Product_01(transfer.MHID, transfer.MAPI_OUT_ID, message);

                            break;
                        }

                    case "STAT_UPD_01":
                        {
                            StatusUpdate_01Select handler = new StatusUpdate_01Select(this.db);
                            StatusUpdate_01Doc message = handler.Process(transfer.MAPI_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.MSG_ID);
                            else
                                sender.StatusUpdate_01(transfer.MHID, transfer.MAPI_OUT_ID, message);

                            break;
                        }

                    default:

                        error = string.Format("transfer type {0} not found", transfer.MSG_ID);
                        break;

                }  // switch (transfer.MSG_ID)

            }
            catch (UriFormatException e)
            {
                error = "The format of the URI is invalid. URL: " + transfer.URL + ", MAPI_OUT_ID: " + transfer.MAPI_OUT_ID + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace;
            }
            catch (InvalidOperationException e) 
            {
                error = "Failed to reach " + sender.Url + " for " + transfer.MSG_ID + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace;
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                error = "Interface mismatch. URL: " + sender.Url + ", MSG_ID: " + transfer.MSG_ID + ", MAPI_OUT_ID: " + transfer.MAPI_OUT_ID + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace;
            }
            catch (Exception e)
            {
                error = "Unexpected error. URL: " + sender.Url + ", MSG_ID: " + transfer.MSG_ID + ", MAPI_OUT_ID: " + transfer.MAPI_OUT_ID + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace;
            }

            return (error);

        }

        protected override void CreateProcedure(IDbConnectionProvider connectionProvider)
        {
            dbWebServiceSend = new DbWebServiceSend(connectionProvider);
            sender = new SenderHandler();
        }

        /// <summary>
        /// ExecuteProcedure is the main activity method, this is the code that is run
        /// when the Job is activated/signalled.
        /// </summary>
        protected override void ExecuteProcedure(Imi.Framework.Job.JobArgumentCollection args)
        {
            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Start");

            long start = DateTime.Now.Ticks;

            // Find msg
            List<MAPI_OUT> transList = FindMessagesToSend();

            foreach (MAPI_OUT t in transList)
            {
                Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                   String.Format(" found TYPE {0} MAPI_OUT_ID {1} for profile {2}", t.MSG_ID, t.MAPI_OUT_ID, t.URL));

                string error = HandleMessageType(t);

                // -------------------------------------------
                // --- Update MAPI_OUT record
                // -------------------------------------------
                // This will never be null, see assignment in cursor above
                Nullable<DateTime> first = t.FIRSTSNDDTM;
                string ALMID_W = "";

                StartTransaction();

                if (error == "")
                    dbWebServiceSend.Modify(t.MAPI_OUT_ID, first, ref ALMID_W);
                else
                    dbWebServiceSend.ModifyError(t.MAPI_OUT_ID, first, "WS", error, ref ALMID_W);

                Commit();

                #region TracingOfResult
                if (error == "")
                {
                    Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                        String.Format("MAPI_OUT_ID {0} was sent successfully.", t.MAPI_OUT_ID));
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0,
                                       String.Format("MAPI_OUT_ID {0} error while sending.\n{1}.", t.MAPI_OUT_ID, error));
                }

                if (ALMID_W != "")
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0,
                                       String.Format("Error {0} when updating MAPI_OUT record status.", ALMID_W));
                }
                #endregion

            }  // foreach ( MAPI_OUT t in transList )

            long stop = DateTime.Now.Ticks;
            TimeSpan ts = new TimeSpan(stop - start);

            Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("Stop (used {0}s)", ts.TotalSeconds.ToString("0.00")));
        }

        protected override void CancelProcedure()
        {
            if (dbWebServiceSend != null)
            {
                dbWebServiceSend.Cancel();
            }
        }
    }
}
