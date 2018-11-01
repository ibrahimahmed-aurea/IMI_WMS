using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;


namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrDialogueLUTGetPackagingType.
    /// </summary>
    public class PrTaskLUTGetPackagingType : VocollectSubscriber
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
                MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "get_mt_pm", session);
                whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                whMsg.Properties.Write("PM_Cur_O", new object());
                
                CorrelationContext context;
                
                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                foreach (MessagePart part in context.ResponseMessages[0].Parts)
                {
                    int temp;

                    /* Packaging Type must be numeric in order to be valid */
                    if (!int.TryParse(part.Properties.ReadAsString("PMTYPID"), out temp))
                        continue;

                    VocollectMessagePart responsePart = new VocollectMessagePart();

                    responsePart.Properties.Write("PackNumber", part.Properties.Read("PMTYPID"));
                    responsePart.Properties.Write("PackDescription", part.Properties.Read("PMNAME"));

                    responsePart.Properties.Write("ErrorCode", (int)VocollectErrorCodeNoError);
                    responsePart.Properties.Write("Message", "");

                    responseMsg.Parts.Add(responsePart);
                }
            
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(2);
                part.Properties.Write("ErrorCode", WarehouseAlarm);
                part.Properties.Write("Message", ex.Message);
                responseMsg.Parts.Add(part);
                
                throw;
            }
            catch (Exception)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(2);
                part.Properties.Write("ErrorCode", (int)VocollectErrorCodeCritialError);
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

