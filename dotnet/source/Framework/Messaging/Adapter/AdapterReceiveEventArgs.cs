using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;

namespace Imi.Framework.Messaging.Adapter
{    
    /// <summary>
    /// Event data for receiving of messages.
    /// </summary>
    public class AdapterReceiveEventArgs : EventArgs
    {
        private MultiPartMessage msg;
        private AdapterEndPoint receiveEndPoint;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="AdapterReceiveEventArgs"/> class.</para>
        /// </summary>
        /// <param name="msg">
        /// A reference to the received message.
        /// </param>
        /// <param name="receiveEndPoint">
        /// The <see cref="AdapterEndPoint"/> from which the message was received.
        /// </param>
        public AdapterReceiveEventArgs(MultiPartMessage msg, AdapterEndPoint receiveEndPoint) : base()
        {
            this.msg = msg;
            this.receiveEndPoint = receiveEndPoint;
        }

        /// <summary>
        /// The <see cref="AdapterEndPoint"/> from which the message was received.
        /// </summary>
        public AdapterEndPoint ReceiveEndPoint
        {
            get
            {
                return receiveEndPoint;
            }
        }

        /// <summary>
        /// The message received by the adapter.
        /// </summary>
        public MultiPartMessage Message
        {
            get
            {
                return msg;
            }
        }
    }
}
