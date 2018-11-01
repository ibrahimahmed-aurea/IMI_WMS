using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Messaging.Adapter
{
    /// <summary>
    /// Event data for endpoint events.
    /// </summary>
    public class AdapterEndPointEventArgs : EventArgs
    {
        private AdapterEndPoint endPoint;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="AdapterEndPointEventArgs"/> class.</para>
        /// </summary>
        /// <param name="endPoint">
        /// The <see cref="AdapterEndPoint"/> participating in the event.
        /// </param>
        public AdapterEndPointEventArgs(AdapterEndPoint endPoint) : base()
        {
            this.endPoint = endPoint;
        }

        /// <summary>
        /// Returns the <see cref="AdapterEndPoint"/>.
        /// </summary>
        public AdapterEndPoint EndPoint
        {
            get
            {
                return endPoint;
            }
        }
       
    }
}
