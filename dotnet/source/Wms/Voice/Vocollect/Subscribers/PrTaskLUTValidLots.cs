using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;


namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTValidLots.
    /// </summary>
    public class PrTaskLUTValidLots : VocollectSubscriber
    {
        private const int VocollectErrorCodeNoValidLots = 101;

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
                MessagePart part = CreateEmptyMessagePart(3);
                part.Properties.Write("ErrorCode", VocollectErrorCodeNoValidLots);
            
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

