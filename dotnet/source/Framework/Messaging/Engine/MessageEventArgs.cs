using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// Class containing event data for the message engine.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        private MultiPartMessage msg;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MessageEventArgs"/> class.</para>
        /// </summary>
        /// <param name="msg">
        /// A reference to the message involved in the event.
        /// </param>
        public MessageEventArgs(MultiPartMessage msg)
        {
            this.msg = msg;
        }

        /// <summary>
        /// A reference to the message involved in the event.
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
