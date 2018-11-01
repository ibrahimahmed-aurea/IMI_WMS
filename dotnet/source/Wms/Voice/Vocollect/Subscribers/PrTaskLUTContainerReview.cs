using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;


namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTContainerReview
    /// </summary>
    public class PrTaskLUTContainerReview : VocollectSubscriber
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
                MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "get_mt_pbcar_pbrow", session);
                whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                whMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                whMsg.Properties.Write("SEQNUM_I", TrimContainerId(msg.Properties.ReadAsString("ContainerId")));
                whMsg.Properties.Write("PBROW_Cur_O", new object());
                whMsg.Properties.Write("ALMID_O", "");

                CorrelationContext context;

                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                if (context.ResponseMessages[0].Parts.Count > 0)
                {
                    foreach (MessagePart part in context.ResponseMessages[0].Parts)
                    {
                        MessagePart responsePart = new VocollectMessagePart();

                        responsePart.Properties.Write("ContainerId", msg.Properties.Read("ContainerId"));
                        responsePart.Properties.Write("Description", part.Properties.Read("ARTNAME1"));
                        responsePart.Properties.Write("Quantity", part.Properties.Read("PICKQTY"));
                        responsePart.Properties.Write("Location", "");
                        responsePart.Properties.Write("PickTime", "");
                        responsePart.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
                        responsePart.Properties.Write("Message", "");

                        responseMsg.Parts.Add(responsePart);
                    }
                }
                else
                {
                    MessagePart part = CreateEmptyMessagePart(7);
                    responseMsg.Parts.Add(part);
                }
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(7);
                part.Properties.Write("ErrorCode", WarehouseAlarm);
                part.Properties.Write("Message", ex.Message);
                responseMsg.Parts.Add(part);

                throw;
            }
            catch (Exception)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(7);
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
