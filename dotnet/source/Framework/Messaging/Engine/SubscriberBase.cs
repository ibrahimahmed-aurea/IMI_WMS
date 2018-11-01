using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// Base class for message subscribers.
    /// </summary>
    public abstract class SubscriberBase
    {
        /// <summary>
        /// Internal invokation method.
        /// </summary>
        /// <param name="msg">A reference to the subscribed message.</param>
        protected internal virtual void InternalInvoke(MultiPartMessage msg)
        {
            Invoke(msg);
        }

        /// <summary>
        /// Invokes the subscriber for processing of the message.
        /// </summary>
        /// <param name="msg">A reference to the subscribed message.</param>
        public abstract void Invoke(MultiPartMessage msg);
        
    }
}
