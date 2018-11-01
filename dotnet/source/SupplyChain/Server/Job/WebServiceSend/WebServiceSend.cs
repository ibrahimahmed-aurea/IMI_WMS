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
using Imi.SupplyChain.Server.Job.WebServiceSend.PLSQLInterface;
using Imi.Framework.Job;
using Imi.Framework.Job.Data;

namespace Imi.SupplyChain.Server.Job.WebServiceSend
{
    class WebServiceSend : OracleJob
    {
        private List<WEBS_PROFILE>    websProfiles;
        private DateTime              websProfileTimeStamp = DateTime.Now;
        private int                   profileRefreshTime;
        private SenderHandler         sender;
        private StringBuilder         currentProfileReport;


        private DbWebServiceSend dbWebServiceSend;

        public WebServiceSend(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
        }

        private void LoadWebsProfiles()
        {
            /* Only refresh every now and then */
            if (websProfileTimeStamp > DateTime.Now)
                return;

            websProfileTimeStamp = DateTime.Now.AddSeconds(profileRefreshTime);

            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Load Webs Profiles");
            StringBuilder report = new StringBuilder();

            websProfiles = new List<WEBS_PROFILE>();

            IDataReader r = dbWebServiceSend.GetWEBSPROFILECur();

            try
            {
                while (r.Read())
                {
                    WEBS_PROFILE p = new WEBS_PROFILE();

                    if (r["WEBS_PROFILE_ID"] == DBNull.Value)
                        p.WEBS_PROFILE_ID = "";
                    else
                        p.WEBS_PROFILE_ID = r["WEBS_PROFILE_ID"] as String;

                    if (r["WEBS_PROFILE_NAME"] == DBNull.Value)
                        p.WEBS_PROFILE_NAME = "";
                    else
                        p.WEBS_PROFILE_NAME = r["WEBS_PROFILE_NAME"] as String;

                    if (r["HAPIOBJECTNAME"] == DBNull.Value)
                        p.HAPIOBJECTNAME = "";
                    else
                        p.HAPIOBJECTNAME = r["HAPIOBJECTNAME"] as String;

                    if (r["DESTINATION_URL"] == DBNull.Value)
                        p.DESTINATION_URL = "";
                    else
                        p.DESTINATION_URL = r["DESTINATION_URL"] as String;

                    websProfiles.Add(p);

                    report.Append(String.Format(" - Profile {0} : ObjectType {1} Url {2}", p.WEBS_PROFILE_ID, p.HAPIOBJECTNAME, p.DESTINATION_URL));

                }
            }
            finally
            {
                if (r != null)
                    r.Close();

                report.Append("Loading done.\n");

                if ((currentProfileReport == null) || (report.ToString() != currentProfileReport.ToString()))
                {
                    Tracing.TraceEvent(TraceEventType.Verbose, 0, report.ToString());
                    currentProfileReport = report;
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Verbose, 0, "Loading done, no difference(s) in profile(s).");
                }

            }
        }

        private List<HAPITRANS> FindMessagesToSend()
        {
            List<HAPITRANS> transList = new List<HAPITRANS>();

            IDataReader r = dbWebServiceSend.GetHAPITRANSCur();

            try
            {
                while (r.Read())
                {
                    HAPITRANS t = new HAPITRANS();

                    // Build HAPITRANS object

                    if (r["HAPITRANS_ID"] == DBNull.Value)
                        t.HAPITRANS_ID = "";
                    else
                        t.HAPITRANS_ID = r["HAPITRANS_ID"] as String;

                    if (r["HAPIOBJECTNAME"] == DBNull.Value)
                        t.HAPIOBJECTNAME = "";
                    else
                        t.HAPIOBJECTNAME = r["HAPIOBJECTNAME"] as String;

                    if (r["HAPISTAT"] == DBNull.Value)
                        t.HAPISTAT = "";
                    else
                        t.HAPISTAT = r["HAPISTAT"] as String;

                    if (r["HAPIERRCOD"] == DBNull.Value)
                        t.HAPIERRCOD = "";
                    else
                        t.HAPIERRCOD = r["HAPIERRCOD"] as String;

                    if (r["HAPIERRMSG"] == DBNull.Value)
                        t.HAPIERRMSG = "";
                    else
                        t.HAPIERRMSG = r["HAPIERRMSG"] as String;

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

                    if (r["COMPANY_ID"] == DBNull.Value)
                        t.COMPANY_ID = "";
                    else
                        t.COMPANY_ID = r["COMPANY_ID"] as String;

                    if (r["WEBS_PROFILE_ID"] == DBNull.Value)
                        t.WEBS_PROFILE_ID = "";
                    else
                        t.WEBS_PROFILE_ID = r["WEBS_PROFILE_ID"] as String;

                    if (r["CHANNEL_ID"] == DBNull.Value)
                        t.CHANNEL_ID = "";
                    else
                        t.CHANNEL_ID = r["CHANNEL_ID"] as String;

                    transList.Add(t);

                    Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                       String.Format("{0} - {1}", t.HAPITRANS_ID, t.HAPIOBJECTNAME));
                }
            }
            finally
            {
                if (r != null)
                    r.Close();
            }

            return (transList);
        }

        private string FindUrlToUse(string WEBS_PROFILE_ID, string HAPIOBJECTNAME)
        {
            foreach (WEBS_PROFILE wp in websProfiles)
            {
                if ((wp.WEBS_PROFILE_ID == WEBS_PROFILE_ID) && (wp.HAPIOBJECTNAME == HAPIOBJECTNAME))
                    return (wp.DESTINATION_URL);
            }
            return "";
        }

        private string HandleMessageType(HAPITRANS transfer)
        {
            string error = "";
            string Url = "";

            try
            {
                Url = FindUrlToUse(transfer.WEBS_PROFILE_ID, transfer.HAPIOBJECTNAME);

                Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format(" use Url = {0}", Url));

                if (String.IsNullOrEmpty(Url))
                    return("unknown url for " + transfer.WEBS_PROFILE_ID + " - " + transfer.HAPIOBJECTNAME);

                sender.Url = Url;

                switch (transfer.HAPIOBJECTNAME)
                {
                    case "DR": // -- HAPIOBJECTNAME_Delivery_Receipt
                        {
                            DeliveryReceiptHeadSelect handler = new DeliveryReceiptHeadSelect(this.db);
                            DeliveryReceiptHeadDoc message = handler.Process(transfer.HAPITRANS_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.HAPIOBJECTNAME);
                            else
                                sender.DeliveryReceipt(transfer.CHANNEL_ID, transfer.COMPANY_ID, transfer.HAPITRANS_ID, message);

                            break;
                        }

                    case "BA":  // -- HAPIOBJECTNAME_Balance_Answer
                        {
                            BalanceAnswerLineSelect handler = new BalanceAnswerLineSelect(this.db);
                            BalanceAnswerLineDoc message = handler.Process(transfer.HAPITRANS_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.HAPIOBJECTNAME);
                            else
                                sender.BalanceAnswer(transfer.CHANNEL_ID, transfer.COMPANY_ID, transfer.HAPITRANS_ID, message);

                            break;
                        }

                    case "PR":  // -- HAPIOBJECTNAME_Pick_Receipt
                        {
                            PickReceiptHeadSelect handler = new PickReceiptHeadSelect(this.db);
                            PickReceiptHeadDoc message = handler.Process(transfer.HAPITRANS_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.HAPIOBJECTNAME);
                            else
                                sender.PickReceipt(transfer.CHANNEL_ID, transfer.COMPANY_ID, transfer.HAPITRANS_ID, message);

                            break;
                        }

                    case "IC":  // -- HAPIOBJECTNAME_Item_Change
                        {
                            InventoryChangeLineSelect handler = new InventoryChangeLineSelect(this.db);
                            InventoryChangeLineDoc message = handler.Process(transfer.HAPITRANS_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.HAPIOBJECTNAME);
                            else
                                sender.InventoryChange(transfer.CHANNEL_ID, transfer.COMPANY_ID, transfer.HAPITRANS_ID, message);

                            break;
                        }

                    case "PM":  // -- HAPIOBJECTNAME_Packing_Material  
                        {
                            ReturnedPackingMaterialHeadSelect handler = new ReturnedPackingMaterialHeadSelect(this.db);
                            ReturnedPackingMaterialHeadDoc message = handler.Process(transfer.HAPITRANS_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.HAPIOBJECTNAME);
                            else
                                sender.ReturnedPackingMaterial(transfer.CHANNEL_ID, transfer.COMPANY_ID, transfer.HAPITRANS_ID, message);

                            break;
                        }

                    case "RR":  // -- HAPIOBJECTNAME_Return_Receipt  
                        {
                            ReturnReceiptHeadSelect handler = new ReturnReceiptHeadSelect(this.db);
                            ReturnReceiptHeadDoc message = handler.Process(transfer.HAPITRANS_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.HAPIOBJECTNAME);
                            else
                                sender.VendorReturnReceipt(transfer.CHANNEL_ID, transfer.COMPANY_ID, transfer.HAPITRANS_ID, message);

                            break;
                        }

                    case "IR":  // -- HAPIOBJECTNAME_Inspection_Receipt
                        {
                            InspectionReceiptHeadSelect handler = new InspectionReceiptHeadSelect(this.db);
                            InspectionReceiptHeadDoc message = handler.Process(transfer.HAPITRANS_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.HAPIOBJECTNAME);
                            else
                                sender.InspectionReceipt(transfer.CHANNEL_ID, transfer.COMPANY_ID, transfer.HAPITRANS_ID, message);

                            break;
                        }

                    case "SR":  // -- HAPIOBJECTNAME_Shipment_Report  
                        {
                            ASNHeadSelect handler = new ASNHeadSelect(this.db);
                            ASNHeadDoc message = handler.Process(transfer.HAPITRANS_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.HAPIOBJECTNAME);
                            else
                                sender.ASN(transfer.CHANNEL_ID, transfer.COMPANY_ID, transfer.HAPITRANS_ID, message);

                            break;
                        }

                    case "COR": // -- HAPIOBJECTNAME_Confirmation_Of_Receipt
                        {

                            ConfirmationOfReceiptHeadSelect handler = new ConfirmationOfReceiptHeadSelect(this.db);
                            ConfirmationOfReceiptHeadDoc message = handler.Process(transfer.HAPITRANS_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.HAPIOBJECTNAME);
                            else
                                sender.ConfirmationOfReceipt(transfer.CHANNEL_ID, transfer.COMPANY_ID, transfer.HAPITRANS_ID, message);

                            break;
                        }

                    case "IOC": // -- HAPIOBJECTNAME_Inbound_Order_Completed
                        {

                            InboundOrderCompletedSelect handler = new InboundOrderCompletedSelect(this.db);
                            InboundOrderCompletedDoc message = handler.Process(transfer.HAPITRANS_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.HAPIOBJECTNAME);
                            else
                                sender.InboundOrderCompleted(transfer.CHANNEL_ID, transfer.COMPANY_ID, transfer.HAPITRANS_ID, message);

                            break;
                        }

                    case "TI": // -- HAPIOBJECTNAME_Transport_Instruction
                        {

                            TransportInstructionSelect handler = new TransportInstructionSelect(this.db);
                            TransportInstructionDoc message = handler.Process(transfer.HAPITRANS_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.HAPIOBJECTNAME);
                            else
                                sender.TransportInstruction(transfer.CHANNEL_ID, transfer.COMPANY_ID, transfer.HAPITRANS_ID, message);

                            break;
                        }

                    case "TP": // -- HAPIOBJECTNAME_Trp_Plan
                        {

                            TransportPlanHeadSelect handler = new TransportPlanHeadSelect(this.db);
                            TransportPlanHeadDoc message = handler.Process(transfer.HAPITRANS_ID);

                            if (message == null)
                                error = string.Format("{0} obj missing.", transfer.HAPIOBJECTNAME);
                            else
                                sender.TransportPlan(transfer.CHANNEL_ID, transfer.COMPANY_ID, transfer.HAPITRANS_ID, message);

                            break;
                        }

                    default:

                        error = string.Format("transfer type {0} not found", transfer.HAPIOBJECTNAME);
                        break;

                }  // switch (transfer.HAPIOBJECTNAME)

            }
            catch (UriFormatException e)
            {
                error = "The format of the URI is invalid. URL: " + Url + ", HAPITRANS_ID: " + transfer.HAPITRANS_ID + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace;
            }
            catch (InvalidOperationException e) 
            {
                error = "Failed to reach " + sender.Url + " for " + transfer.HAPIOBJECTNAME + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace;
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                error = "Interface mismatch. URL: " + sender.Url + ", HAPIOBJECTNAME: " + transfer.HAPIOBJECTNAME + ", HAPITRANS_ID: " + transfer.HAPITRANS_ID + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace;
            }
            catch (Exception e)
            {
                error = "Unexpected error. URL: " + sender.Url + ", HAPIOBJECTNAME: " + transfer.HAPIOBJECTNAME + ", HAPITRANS_ID: " + transfer.HAPITRANS_ID + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace;
            }

            return (error);

        }

        private bool CheckParameters()
        {
            // Check config
            if (args["profileRefreshTime"] != null)
            {
                try
                {
                    int newTime = Convert.ToInt32(args["profileRefreshTime"]);
                    if (newTime != profileRefreshTime)
                    {
                        profileRefreshTime = newTime;
                        Tracing.TraceEvent(TraceEventType.Information, 0,
                                           String.Format("Update profileRefreshTime to {0}", newTime));
                    }
                }
                catch (Exception)
                {
                    Tracing.TraceEvent(TraceEventType.Critical, 0,
                                       String.Format("The profileRefreshTime parameter has an illegal value '{0}', no update is possible. Old value '{1}' is still used.", args["profileRefreshTime"], profileRefreshTime));
                }

                return false;
            }

            return true;
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
            CheckParameters(); // ignore result

            LoadWebsProfiles();

            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Start");

            long start = DateTime.Now.Ticks;

            // Find msg
            List<HAPITRANS> transList = FindMessagesToSend();

            foreach (HAPITRANS t in transList)
            {
                Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                   String.Format(" found TYPE {0} HAPITRANS_ID {1} for profile {2}", t.HAPIOBJECTNAME, t.HAPITRANS_ID, t.WEBS_PROFILE_ID));

                string error = HandleMessageType(t);

                // -------------------------------------------
                // --- Update HAPITRANS record
                // -------------------------------------------
                // This will never be null, see assignment in cursor above
                Nullable<DateTime> first = t.FIRSTSNDDTM;
                string ALMID_W = "";

                StartTransaction();

                if (error == "")
                    dbWebServiceSend.ModifySend(t.HAPITRANS_ID, first, ref ALMID_W);
                else
                    dbWebServiceSend.ModifyError(t.HAPITRANS_ID, first, "WS", error, ref ALMID_W);

                Commit();

                #region TracingOfResult
                if (error == "")
                {
                    Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                        String.Format("HAPITRANS_ID {0} was sent successfully.", t.HAPITRANS_ID));
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0,
                                       String.Format("HAPITRANS_ID {0} error while sending.\n{1}.", t.HAPITRANS_ID, error));
                }

                if (ALMID_W != "")
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0,
                                       String.Format("Error {0} when updating HAPITRANS record status.", ALMID_W));
                }
                #endregion

            }  // foreach ( HAPITRANS t in transList )

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
