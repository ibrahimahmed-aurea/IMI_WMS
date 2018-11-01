using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTCoreBreakTypes.
    /// </summary>
    [SessionPolicy(SessionPolicy.None)]
    public class PrTaskLUTCoreBreakTypes : VocollectSubscriber
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
                MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "get_user_languagecode", session);
                whMsg.Properties.Write("EMPID_I", msg.Properties.ReadAsString("Operator"));
                whMsg.Properties.Write("NLANGCOD_O", "");

                CorrelationContext context;

                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                string languageCode = context.ResponseMessages[0].Properties.ReadAsString("NLANGCOD_O");

                VocollectMessagePart part = new VocollectMessagePart();
                part.Properties.Write("BreakType", 1);
                part.Properties.Write("BreakDescription", GetCachedAlarmText("VOICEPICK014", languageCode));
                part.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
                part.Properties.Write("Message", "");

                responseMsg.Parts.Add(part);
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(4);
                part.Properties.Write("ErrorCode", WarehouseAlarm);
                part.Properties.Write("Message", ex.Message);
                responseMsg.Parts.Add(part);

                throw;
            }
            catch (Exception)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(4);
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
