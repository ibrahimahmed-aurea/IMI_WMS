using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;


namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrDialogueLUTReplenStatus
    /// </summary>
    public class PrTaskLUTPrint : VocollectSubscriber
    {
        private const int VocollectErrorCodeCollectContainers = 2;
        private const string HOLD_TYPE_Definitely = "D";
        
        /// <summary>
        /// Invokes the subscriber.
        /// </summary>
        /// <param name="msg">A reference to the subscribed message.</param>
        /// <param name="session">The current <see cref="VocollectSession"/> object.</param>
        /// <exception cref="WarehouseAdapterException">
        /// </exception>
        /// <exception cref="MessageEngineException">
        /// </exception>
        public override void Invoke(MultiPartMessage msg, VocollectSession session)
        {
            MultiPartMessage responseMsg = CreateResponseMessage(msg);
            
            responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
            responseMsg.Properties.Write("Message", "");

            try
            {
                /* Do not direct operator to collect containers when continuing a temporarily stopped assignment
                 * or when print request is sent at the end of an assignment */
                if ((string.IsNullOrEmpty(session.ReadAsString("HOLD_TYPE"))
                    || (session.ReadAsString("HOLD_TYPE") == HOLD_TYPE_Definitely))
                    && string.IsNullOrEmpty(session.ReadAsString("PBROWID")))
                    responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeCollectContainers);
                                
                if ((msg.Properties.ReadAsInt("PrinterNumber") > 0) && (msg.Properties.ReadAsInt("Operation") < 2))
                {
                    if ((session.ReadAsString("PRINTED") == "1") &&
                        (string.IsNullOrEmpty(TrimContainerId(msg.Properties.ReadAsString("SystemContainerId")))) && 
                        (msg.Properties.ReadAsInt("Operation") == 1))
                    {
                        Print(msg.Properties.ReadAsString("PrinterNumber"), TrimContainerId(msg.Properties.ReadAsString("SystemContainerId")), session, true);
                    }
                    else
                    {
                        Print(msg.Properties.ReadAsString("PrinterNumber"), TrimContainerId(msg.Properties.ReadAsString("SystemContainerId")), session, false);
                    }
                }

                session.Write("PRINTED", "1");
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Properties.Write("ErrorCode", WarehouseAlarm);
                responseMsg.Properties.Write("Message", ex.Message);

                throw;
            }
            catch (Exception)
            {
                responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeCritialError);
                responseMsg.Properties.Write("Message", GetCriticalErrorMessageText(msg));

                throw;
            }
            finally
            {
                TransmitResponseMessage(responseMsg, session);
            }
        }

        public static void Print(string printerNumber, string containerId, VocollectSession session)
        {
            Print(printerNumber, containerId, session, false);
        }

        public static void Print(string printerNumber, string containerId, VocollectSession session, bool isReprint)
        {
            if ((string.IsNullOrEmpty(printerNumber)) || (printerNumber == "1"))
                printerNumber = "";

            CorrelationContext context;

            /* Save printer number in session for use when finish pick to print PS */
            session.Write("PRINTERNUMBER", printerNumber);

            if (string.IsNullOrEmpty(containerId) && isReprint)
            {
                MultiPartMessage reprintMsg = CreateRequestMessage("wlvoicepick", "print_allloadcarriers", session);
                reprintMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                reprintMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                reprintMsg.Properties.Write("EMPID_I", session.ReadAsString("EMPID"));
                reprintMsg.Properties.Write("PRTID_I", printerNumber);

                reprintMsg.Properties.Write("ALMID_O", "");

                MessageEngine.Instance.TransmitRequestMessage(reprintMsg, reprintMsg.MessageId, out context);
            }
            else if (string.IsNullOrEmpty(containerId))
            {
                MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "print", session);
                whMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                whMsg.Properties.Write("EMPID_I", session.ReadAsString("EMPID"));
                whMsg.Properties.Write("PRTID_I", printerNumber);

                if (string.IsNullOrEmpty(session.ReadAsString("PBROWID")) && (session.ReadAsInt("PBROW_COUNT") > 0))
                    whMsg.Properties.Write("PRINTCOD_I", "BP");
                else
                    whMsg.Properties.Write("PRINTCOD_I", "AP");

                whMsg.Properties.Write("ALMID_O", "");

                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);
            }
            else
            {
                MultiPartMessage reprintMsg = CreateRequestMessage("wlvoicepick", "print_singleloadcarrier", session);
                reprintMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                reprintMsg.Properties.Write("SEQNUM_I", TrimContainerId(containerId));
                reprintMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                reprintMsg.Properties.Write("EMPID_I", session.ReadAsString("EMPID"));
                reprintMsg.Properties.Write("PRTID_I", printerNumber);

                reprintMsg.Properties.Write("ALMID_O", "");

                MessageEngine.Instance.TransmitRequestMessage(reprintMsg, reprintMsg.MessageId, out context);
            }
        }
    }
}
