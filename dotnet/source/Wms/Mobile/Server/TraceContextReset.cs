using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Shared.Diagnostics;

namespace Imi.Wms.Mobile.Server
{
    /// <summary>
    /// Resets the trace context for the current thread. 
    /// <remarks>
    /// This subscriber should always be added last to the subscription list.
    /// </remarks>
    /// </summary>
    public class TraceContextReset : SubscriberBase
    {

        /// <summary>
        /// Invokes the subscriber for processing of the message.
        /// </summary>
        /// <param name="msg">A reference to the subscribed message.</param>
        public override void Invoke(MultiPartMessage msg)
        {
            ContextTraceListener contextListener = ((ContextTraceListener)MessageEngine.Instance.Tracing.Listeners["ContextTraceListener"]);
            
            if (contextListener != null)
                contextListener.ResetContext();
        }
    }
}
