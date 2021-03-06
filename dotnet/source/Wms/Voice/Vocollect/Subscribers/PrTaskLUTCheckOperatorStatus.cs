using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTCheckOperatorStatus.
    /// </summary>
    public class PrTaskLUTCheckOperatorStatus : VocollectSubscriber
    {
        /// <summary>
        /// Invokes the subscriber.
        /// </summary>
        /// <param name="msg">A reference to the subscribed message.</param>
        /// <param name="session">The current <see cref="VocollectSession"/> object.</param>
        /// <exception cref="MessageEngineException">
        /// </exception>
        public override void Invoke(MultiPartMessage msg, VocollectSession session)
        {
            MultiPartMessage responseMsg = CreateResponseMessage(msg);

            responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
            responseMsg.Properties.Write("Message", "");
            
            try
            {
                if (session.Contains("CatchMeasureException"))
                {
                    Exception ex = (Exception)session["CatchMeasureException"];
                    responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeCritialError);
                    responseMsg.Properties.Write("Message", ex.Message);
                }
            }
            catch (Exception)
            {
                responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeCritialError);
                responseMsg.Properties.Write("Message", GetCriticalErrorMessageText(msg));

                throw;
            }
            finally
            {
                try
                {
                    session.Remove("CatchMeasureException");
                }
                finally
                {
                    TransmitResponseMessage(responseMsg, session);
                }
            }
        }
    }
}
