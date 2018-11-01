using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTStopAssignment.
    /// </summary>
    public class PrTaskLUTStopAssignment : VocollectSubscriber
    {
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
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    //Drop last load carrier from PrTaskLUTGetDeliveryLocation
                    if (!string.IsNullOrEmpty(session.ReadAsString("DROPSEQNUM")))
                    {
                        PrTaskLUTAltDeliveryLocations.DropLoadCarrier(session.ReadAsString("DROPSEQNUM"), session.ReadAsString("DROPWSID"), session.ReadAsString("DROPWPADR"), session);
                    }

                    MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "finish_pick", session);
                    whMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                    whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                    
                    string CPL_PRINTCOD = session.ReadAsString("CPL_PRINTCOD");
                    string PLCP_PRINTCOD = session.ReadAsString("PLCP_PRINTCOD");

                    if (session.Contains("PRINTERNUMBER"))
                        whMsg.Properties.Write("PRTID_I", session.ReadAsString("PRINTERNUMBER"));
                    else
                        whMsg.Properties.Write("PRTID_I", "");
                    
                    whMsg.Properties.Write("ALMID_O", "");

                    CorrelationContext context;

                    MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);
                                        
                    transactionScope.Complete();

                    session.Write("PBHEADID", "");
                    session.CurrentAssignmentData = null;
                }
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

    }
}
