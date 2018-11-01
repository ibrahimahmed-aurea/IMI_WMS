using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTCycleCountingAssignment.
    /// </summary>
    [SessionPolicy(SessionPolicy.None)]
    public class PrTaskLUTCycleCountingAssignment : VocollectSubscriber
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
                VocollectMessagePart part = new VocollectMessagePart();
                part.Properties.Write("LocationId", msg.Properties.ReadAsString("LocationId"));
                part.Properties.Write("PreAisleDirection", "");
                part.Properties.Write("Aisle", ((string)part.Properties.Read("WSID")).PadRight(3, ' '));
                part.Properties.Write("PostAisleDirection", "");
                part.Properties.Write("Slot", "");
                part.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
                part.Properties.Write("Message", "");

                responseMsg.Parts.Add(part);
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(15);
                part.Properties.Write("ErrorCode", WarehouseAlarm);
                part.Properties.Write("Message", ex.Message);
                responseMsg.Parts.Add(part);

                throw;
            }
            catch (Exception)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(15);
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
