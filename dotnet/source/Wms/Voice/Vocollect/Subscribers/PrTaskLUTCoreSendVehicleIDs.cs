using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;


namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTCoreSendVehicleIDs.
    /// </summary>
    public class PrTaskLUTCoreSendVehicleIDs : VocollectSubscriber
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
                        
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    CorrelationContext context;

                    if (!string.IsNullOrEmpty(session.ReadAsString("TUID")))
                    {
                        MultiPartMessage tuMsg = CreateRequestMessage("wlvoicepick", "disconnect_ter_from_pt", session);
                        tuMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                        tuMsg.Properties.Write("TUID_I", session.ReadAsString("TUID"));
                        tuMsg.Properties.Write("KEEP_ONLINE_I", "0");
                        tuMsg.Properties.Write("ALMID_O", "");

                        MessageEngine.Instance.TransmitRequestMessage(tuMsg, tuMsg.MessageId, out context);
                    }

                    MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "connect_ter_to_pt", session);
                    whMsg.Properties.Write("TUID_I", msg.Properties.ReadAsString("VehicleId"));
                    whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                    whMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                    whMsg.Properties.Write("TUID_O", "");
                    whMsg.Properties.Write("ALMID_O", "");
                    
                    MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                    MessagePart responsePart = new VocollectMessagePart();

                    responsePart.Properties.Write("SafetyCheck", "");
                    responsePart.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
                    responsePart.Properties.Write("Message", "");

                    responseMsg.Parts.Add(responsePart);

                    session.Write("TUID", context.ResponseMessages[0].Properties.ReadAsString("TUID_O"));
                    session.Write("UPDATE_PBCAR", "1");

                    transactionScope.Complete();
                }
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(3);
                part.Properties.Write("ErrorCode", WarehouseAlarm);
                part.Properties.Write("Message", ex.Message);
                responseMsg.Parts.Add(part);

                throw;
            }
            catch (Exception)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(3);
                part.Properties.Write("ErrorCode", VocollectErrorCodeCritialError);
                part.Properties.Write("Message", GetCriticalErrorMessageText(msg));
                responseMsg.Parts.Add(part);

                throw;
            }
            finally
            {
                TransmitResponseMessage(responseMsg, session);
            }
        }
                                
    }
}
