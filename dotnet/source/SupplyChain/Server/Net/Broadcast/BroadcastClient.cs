using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Collections;

namespace Imi.SupplyChain.Server.Net.Broadcast
{
    /// <summary>
    /// Summary description for BroadcastClient.
    /// </summary>
    /// <summary>
    /// Searches for BroadcastServers on the network using UDP multicast groups.
    /// </summary>
    public sealed class BroadcastClient
    {
        private BroadcastClient()
        { }

        /// <summary>
        /// Locates BroadcastServers on the network identified by the specified Guid
        /// </summary>
        /// <param name="serviceId">Unique identifier for the requested server.</param>
        /// <param name="groupAddress">Group address to broadcast on.</param>
        /// <param name="serverPort">Port to broadcast on.</param>
        /// <param name="millisecondTimeout">Time (in milliseconds) to wait for responses from remote server.</param>
        /// <returns>An array of ServerResponse structures.</returns>
        public static ServerResponse[] FindServer(Guid serviceId, IPAddress groupAddress, int serverPort, int millisecondTimeout)
        {
            UdpClient sender = new UdpClient();

            // Construct datagram
            byte[] request = serviceId.ToByteArray();
            IPEndPoint groupEP = new IPEndPoint(groupAddress, serverPort);

            // Send the message
            sender.Send(request, request.Length, groupEP);

            // Accumulate responses in thread
            ResponseHandler h = new ResponseHandler(ResponseHandlerImpl);
            IAsyncResult ar = h.BeginInvoke(sender, serviceId, null, null);

            // Wait for mullisecondTimeout milliseconds then close socket
            Thread.Sleep(millisecondTimeout);
            sender.Close();

            // Return the results
            ServerResponse[] r = h.EndInvoke(ar);
            return r;
        }

        public class ServerResponse
        {
            private readonly IPAddress address;
            private readonly String message;

            internal ServerResponse(IPAddress address, String message)
            {
                this.address = address;
                this.message = message;
            }

            /// <summary>
            /// Gets the IPAddress of the responding host.
            /// </summary>
            public IPAddress IPAddress
            {
                get { return this.address; }
            }

            /// <summary>
            /// Gets a protocol-specific message from the responding host.
            /// </summary>
            public String Message
            {
                get { return this.message; }
            }
        }

        //
        // Implementation

        internal delegate ServerResponse[] ResponseHandler(UdpClient udpClient, Guid serviceId);

        internal static ServerResponse[] ResponseHandlerImpl(UdpClient udpClient, Guid serviceId)
        {
            ArrayList responses = new ArrayList();

            // Loop forever (until underlying socket is closed)
            try
            {
                while (true)
                {
                    // Grab a response datagram
                    IPEndPoint remoteEndpoint = null;
                    byte[] response = udpClient.Receive(ref remoteEndpoint);

                    // Unmarshal the response
                    MemoryStream responseStream = new MemoryStream(response, false);

                    // Format is a 16-byte guid, followed by message
                    if (response.Length >= 16)
                    {
                        byte[] temp = new byte[16];
                        responseStream.Read(temp, 0, 16);
                        Guid guid = new Guid(temp);

                        if (guid == serviceId)
                        {
                            BinaryFormatter bf = new BinaryFormatter();

                            object message = bf.Deserialize(responseStream);
                            responses.Add(new ServerResponse(remoteEndpoint.Address, (String)message));
                        }
                    }
                }
            }
            catch (System.ObjectDisposedException)
            { }
            catch (System.Net.Sockets.SocketException)
            { }

            return responses.ToArray(typeof(ServerResponse)) as ServerResponse[];
        }
    }
}