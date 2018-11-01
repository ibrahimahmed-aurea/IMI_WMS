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
    public class PrTaskLUTVerifyReplenishment : VocollectSubscriber
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
            
            responseMsg.Properties.Write("Replenished", "1");
            responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
            responseMsg.Properties.Write("Message", "");
            
            try
            {
                MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "verify_pp_balance", session);
                whMsg.Properties.Write("TERID_I", msg.Properties.ReadAsString("SerialNumber"));
                whMsg.Properties.Write("PPKEY_I", ExtractPickPlaceKey(msg.Properties.ReadAsString("LocationId")));
                whMsg.Properties.Write("STORQTY_O", 0d);
                whMsg.Properties.Write("ALMID_O", "");

                CorrelationContext context;
                
                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                if ((decimal)context.ResponseMessages[0].Properties.Read("STORQTY_O") <= 0)
                    responseMsg.Properties.Write("Replenished", "0");
                                
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
