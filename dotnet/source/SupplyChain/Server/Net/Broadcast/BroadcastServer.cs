using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Imi.SupplyChain.Server.Net.Broadcast
{
  /// <summary>
  /// Summary description for BroadcastServer.
  /// </summary>
  /// <summary>
  /// Responds to broadcasts on the network using UDP multicast groups.
  /// </summary>
  public sealed class BroadcastServer
  {
    public const string serviceIdString = "29D7C70E-6B0B-4066-A53A-52474DD75063";

    private Guid      serviceId;
    private IPAddress groupAddress; 
    private int       serverPort; 
    private int       ttl;

    private UdpClient listener;
    private byte[]    response;

    /// <summary>
    /// Initializes a new instance of the BroadcastServer class, for the specified service id.
    /// </summary>
    /// <param name="serviceId">Unique identifer for the client/server protocol being advertised.</param>
    public BroadcastServer(Guid serviceId,String IPString, int port, int ttl)
    {
      this.serviceId    = serviceId;
      this.groupAddress = IPAddress.Parse(IPString);
      this.serverPort   = port;
      this.ttl          = ttl;
      this.listener     = null;
      this.response     = null;
    }

    /// <summary>
    /// Begin advertising the presence of the service.
    /// </summary>
    /// <param name="message">String containing message to broadcast.</param>
    public void Start(String message)
    {
      // Prepare canned response: serialize serviceId and message into byte[]
      BinaryFormatter bf = new BinaryFormatter();
      MemoryStream    ms = new MemoryStream();

      ms.Write(this.serviceId.ToByteArray(), 0, 16);
      bf.Serialize(ms,message);

      this.response = ms.ToArray();

      // Initalize a new UDP socket for the multicast group
      this.listener = new UdpClient(serverPort);

      // Join the multicast group (sends an IGMP group-membership report to routers)
      this.listener.JoinMulticastGroup(groupAddress, ttl);

      // Punt remainder of implementation to background thread (UdpClient.Receive blocks!)
      Thread listenerThread = new Thread(new ThreadStart(this.Listen));
      listenerThread.Start();
    }

    /// <summary>
    /// Stops advertising the service.
    /// </summary>
    public void Stop()
    {
      if (this.listener != null)
      {
        this.listener.DropMulticastGroup(groupAddress);
        this.listener.Close();
        this.listener = null;
      }
    }


    private void Listen()
    {
      try 
      {
        while (true)
        {
          try
          {
            // Wait for broadcast, will block
            IPEndPoint callerEndpoint = null;
            byte[] request = this.listener.Receive( ref callerEndpoint);

            if (request.Length >= 16)
            {
              byte[] temp = new byte[16];
              request.CopyTo(temp,0);
              Guid requestGuid = new Guid(temp);

              if (requestGuid == this.serviceId)
              {
                // Send response (guid, followed by serialized endpoint info)
                this.listener.Send(this.response, this.response.Length, callerEndpoint);
              }
            }
          }
          catch (System.Net.Sockets.SocketException)
          { } // expected (client got too impatient?)
        }
      }
      catch (System.ObjectDisposedException)
      { } // expected
      catch (System.NullReferenceException)
      { } // expected
    }
  }
}