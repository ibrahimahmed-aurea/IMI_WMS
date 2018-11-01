using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Imi.Framework.Messaging.Adapter.Net.Sockets
{
    /// <summary>
    /// Represents a Tcp endpoint.
    /// </summary>
    public class TcpAdapterEndPoint : AdapterEndPoint
    {
        internal const int bufferSize = 4096;
        internal byte[] buffer = new byte[bufferSize];
        private Socket socket;
        private Exception exception;
        private DateTime timeout;
        	
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="TcpAdapterEndPoint"/> class.</para>
        /// </summary>
        /// <param name="adapter">
        /// The adapter which owns the endpoint.
        /// </param>
        /// <param name="socket">
        /// The underlying <see cref="Socket"/> object.
        /// </param>
        public TcpAdapterEndPoint(AdapterBase adapter, Socket socket) 
            : base(adapter, new UriBuilder("tcp", ((IPEndPoint)socket.RemoteEndPoint).Address.ToString(),
                    ((IPEndPoint)socket.RemoteEndPoint).Port).Uri)
        {
            this.socket = socket;
            this.timeout = DateTime.MaxValue;
        }
        
        /// <summary>
        /// Returns a reference to the underlying <see cref="Socket"/> object.
        /// </summary>
        public Socket Socket
        {
            get
            {
                return socket;
            }
        }

        internal DateTime Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        /// <summary>
        /// Returns any exception thrown during communication.
        /// </summary>
        public Exception Exception
        {
            get
            {
                return exception;
            }
            set
            {
                exception = value;
            }
        }
                        
    }
}
