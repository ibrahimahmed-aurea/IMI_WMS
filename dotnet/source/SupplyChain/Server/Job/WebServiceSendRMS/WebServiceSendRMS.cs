using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Generic;
using Imi.SupplyChain.Server.Job.StandardJob;
using Imi.Wms.WebServices.ExternalInterface;
using Imi.SupplyChain.Server.Job.WebServiceSendRMS.PLSQLInterface;     
using Imi.Framework.Job;
using Imi.Framework.Job.Data;

namespace Imi.SupplyChain.Server.Job.WebServiceSendRMS
{
    class WebServiceSendRMS : OracleJob
    {
        private DateTime websProfileTimeStamp = DateTime.Now;
        private SenderHandler sender;

        private Messagesend dbWebServiceSendRMS;

        public WebServiceSendRMS(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
        }


        private List<MSG_SEND> FindMessagesToSend(string CHANNEL_TYPE_I)
        {
            IDataReader r;
            List<MSG_SEND> Messages_List = new List<MSG_SEND>();

            dbWebServiceSendRMS.GetMessagesCur(CHANNEL_TYPE_I, out r);

            try
            {
                while (r.Read())
                {
                    MSG_SEND t = new MSG_SEND();

                    // Build MSG_SEND object

                    if (r["MSG_OUT_ID"] == DBNull.Value)
                        t.MSG_OUT_ID = "";
                    else
                        t.MSG_OUT_ID = r["MSG_OUT_ID"] as String;

                    if (r["MSG_SEND_ID"] == DBNull.Value)
                        t.MSG_SEND_ID = "";
                    else
                        t.MSG_SEND_ID = r["MSG_SEND_ID"] as String;

                    if (r["MSG_ID"] == DBNull.Value)
                        t.MSG_ID = "";
                    else
                        t.MSG_ID = r["MSG_ID"] as String;

                    if (r["URL"] == DBNull.Value)
                        t.URL = "";
                    else
                        t.URL = r["URL"] as String;

                    if (r["APILOGIN"] == DBNull.Value)
                        t.APILOGIN = "";
                    else
                        t.APILOGIN = r["APILOGIN"] as String;


                    if (r["FIRSTSNDDTM"] == DBNull.Value)
                        t.FIRSTSNDDTM = System.DateTime.Now;
                    else
                        t.FIRSTSNDDTM = (DateTime)r["FIRSTSNDDTM"];

                    if (r["COMM_PARTNER_ID"] == DBNull.Value)
                        t.COMM_PARTNER_ID = "";
                    else
                        t.COMM_PARTNER_ID = r["COMM_PARTNER_ID"] as String;

                    if (r["CHANNEL_ID"] == DBNull.Value)
                        t.CHANNEL_ID = "";
                    else
                        t.CHANNEL_ID = r["CHANNEL_ID"] as String;


                    Messages_List.Add(t);

                    Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                       String.Format("{0} - {1}", t.MSG_SEND_ID, t.MSG_ID));
                }
            }
            finally
            {
                if (r != null)
                    r.Close();
            }

            return (Messages_List);
        }



        private string HandleMessageType(MSG_SEND message_send)
        {
            string error = "";
            string Url = message_send.URL;

            try
            {
                Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format(" use Url = {0}", Url));

                if (String.IsNullOrEmpty(Url))
                    return ("unknown url for communication partner/channel " + message_send.COMM_PARTNER_ID + " / " + message_send.CHANNEL_ID + " - " + message_send.MSG_ID);

                sender.Url = Url;

                switch (message_send.MSG_ID)
                {
                    case "DEP": // -- Departure Message
                        {
                            DepartureSelect handler = new DepartureSelect(this.db);
                            DepartureDoc message = handler.Process(message_send.MSG_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", message_send.MSG_ID);
                            else
                                sender.Departure(message_send.COMM_PARTNER_ID, message_send.MSG_SEND_ID, message);

                            break;
                        }

                    case "DEP_NODE":  // -- Departure Node Message
                        {
                            DepartureNodeSelect handler = new DepartureNodeSelect(this.db);
                            DepartureNodeDoc message = handler.Process(message_send.MSG_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", message_send.MSG_ID);
                            else
                                sender.DepartureNode(message_send.COMM_PARTNER_ID, message_send.MSG_SEND_ID, message);

                            break;
                        }

                    case "DEP_TRP":  // -- Departure Transport Type
                        {
                            DepartureTransportTypeSelect handler = new DepartureTransportTypeSelect(this.db);
                            DepartureTransportTypeDoc message = handler.Process(message_send.MSG_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", message_send.MSG_ID);
                            else
                                sender.DepartureTransportType(message_send.COMM_PARTNER_ID, message_send.MSG_SEND_ID, message);

                            break;
                        }

                    case "DEP_LOAD":  // -- Departure Load
                        {
                            DepartureLoadSelect handler = new DepartureLoadSelect(this.db);
                            DepartureLoadDoc message = handler.Process(message_send.MSG_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", message_send.MSG_ID);
                            else
                                sender.DepartureLoad(message_send.COMM_PARTNER_ID, message_send.MSG_SEND_ID, message);

                            break;
                        }

                    case "MOD_DEP_LOAD":  // -- Modify Departure Load  
                        {
                            ModifyDepartureLoadSelect handler = new ModifyDepartureLoadSelect(this.db);
                            ModifyDepartureLoadDoc message = handler.Process(message_send.MSG_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", message_send.MSG_ID);
                            else
                                sender.ModifyDepartureLoad(message_send.COMM_PARTNER_ID, message_send.MSG_SEND_ID, message);

                            break;
                        }

                    case "REM_DEP":  // -- Remove Departure  
                        {
                            RemoveDepartureSelect handler = new RemoveDepartureSelect(this.db);
                            RemoveDepartureDoc message = handler.Process(message_send.MSG_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", message_send.MSG_ID);
                            else
                                sender.RemoveDeparture(message_send.COMM_PARTNER_ID, message_send.MSG_SEND_ID, message);

                            break;
                        }

                    case "REM_DEP_NODE":  // -- Remove Departure Node
                        {
                            RemoveDepartureNodeSelect handler = new RemoveDepartureNodeSelect(this.db);
                            RemoveDepartureNodeDoc message = handler.Process(message_send.MSG_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", message_send.MSG_ID);
                            else
                                sender.RemoveDepartureNode(message_send.COMM_PARTNER_ID, message_send.MSG_SEND_ID, message);

                            break;
                        }

                    case "REM_DEP_TRP":  // -- Remove Departure Transport Type  
                        {
                            RemoveDepartureTransportTypeSelect handler = new RemoveDepartureTransportTypeSelect(this.db);
                            RemoveDepartureTransportTypeDoc message = handler.Process(message_send.MSG_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", message_send.MSG_ID);
                            else
                                sender.RemoveDepartureTransportType(message_send.COMM_PARTNER_ID, message_send.MSG_SEND_ID, message);

                            break;
                        }

                    case "CONFIRM":  // -- Confirmation Message  
                        {
                            ConfirmSelect handler = new ConfirmSelect(this.db);
                            ConfirmDoc message = handler.Process(message_send.MSG_OUT_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", message_send.MSG_ID);
                            else
                                sender.Confirm(message_send.COMM_PARTNER_ID, message_send.MSG_SEND_ID, message);

                            break;
                        }


                    default:

                        error = string.Format("message type {0} not found", message_send.MSG_ID);
                        break;

                }  // switch (message_send.MSG_ID)

            }
            catch (UriFormatException e)
            {
                error = "The format of the URI is invalid. URL: " + Url + ", MSG_SEND_ID: " + message_send.MSG_SEND_ID + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace;
            }
            catch (InvalidOperationException e)
            {
                error = "Failed to reach " + sender.Url + " for " + message_send.MSG_ID + ". MSG_SEND_ID: " + message_send.MSG_SEND_ID + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace;
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                error = "Interface mismatch. URL: " + sender.Url + ", MSG_ID: " + message_send.MSG_ID + ", MSG_SEND_ID: " + message_send.MSG_SEND_ID + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace;
            }
            catch (Exception e)
            {
                error = "Unexpected error. URL: " + sender.Url + ", MSG_ID: " + message_send.MSG_ID + ", MSG_SEND_ID: " + message_send.MSG_SEND_ID + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace;
            }

            return (error);

        }

        private bool CheckParameters()
        {
            // Check config

            return true;
        }

        protected override void CreateProcedure(IDbConnectionProvider connectionProvider)
        {

            dbWebServiceSendRMS = new Messagesend(connectionProvider);
            sender = new SenderHandler();

        }

        /// <summary>
        /// ExecuteProcedure is the main activity method, this is the code that is run
        /// when the Job is activated/signalled.
        /// </summary>
        protected override void ExecuteProcedure(Imi.Framework.Job.JobArgumentCollection args)
        {
            CheckParameters(); // ignore result

            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Start");

            long start = DateTime.Now.Ticks;

            // Find messages of type WebServices (W)
            List<MSG_SEND> Message_List = FindMessagesToSend("W");

            foreach (MSG_SEND t in Message_List)
            {
                Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                   String.Format(" found MSG_ID {0} MSG_SEND_ID {1} for CHANNEL_ID {2}", t.MSG_ID, t.MSG_SEND_ID, t.CHANNEL_ID));

                string error = HandleMessageType(t);

                // -------------------------------------------
                // --- Update MSGSEND record
                // -------------------------------------------
                // This will never be null, see assignment in cursor above
                Nullable<DateTime> first = t.FIRSTSNDDTM;
                string ALMID_W = "";

                StartTransaction();

                if (error == "")
                    dbWebServiceSendRMS.ModifySend(t.MSG_SEND_ID, first, ref ALMID_W);
                else
                    dbWebServiceSendRMS.ModifyError(t.MSG_SEND_ID, first, "WS", error, ref ALMID_W);

                Commit();

                #region TracingOfResult
                if (error == "")
                {
                    Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                        String.Format("MSG_ID {0} was sent successfully.", t.MSG_ID));
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0,
                                       String.Format("MSG_ID {0} error while sending.\n{1}.", t.MSG_ID, error));
                }

                if (!String.IsNullOrEmpty(ALMID_W))
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0,
                                       String.Format("Error when updating MSG_SEND record status. ALMID: {0}", ALMID_W));
                }
                #endregion

            }  // foreach ( MSG_SEND t in Message_List )

            long stop = DateTime.Now.Ticks;
            TimeSpan ts = new TimeSpan(stop - start);

            Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("Stop (used {0}s)", ts.TotalSeconds.ToString("0.00")));
        }

        protected override void CancelProcedure()
        {
            if (dbWebServiceSendRMS != null)
            {
                dbWebServiceSendRMS.Cancel();
            }
        }
    }
}
