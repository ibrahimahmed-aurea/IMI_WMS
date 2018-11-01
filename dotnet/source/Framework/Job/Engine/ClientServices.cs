using System;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Diagnostics;
using System.Collections.Generic;
using Imi.Framework.Job.RemoteInterface;
using Imi.Framework.Shared.Diagnostics;

namespace Imi.Framework.Job.Engine
{
    

    /// <summary>
    /// Summary description for ClientServices.
    /// </summary>
    /*
    public class ClientServices
    {
        private JobManager jm;
        private HttpChannel channel;
        private RemoteInterfaceProxy ric;
        private ObjRef oRef;
        private int port;
        private string URI;

        public ClientServices(JobManager jm, int port, string URI)
        {
            this.jm = jm;
            this.port = port;
            this.URI = URI;
        }

        public void Start()
        {
            channel = new HttpChannel(this.port);
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.CustomErrorsEnabled(true);
            ric = new RemoteInterfaceProxy(new RemoteInterface("ClientHandler", jm));

            // Creates the single instance of RemoteInstanceCtrl. 
            oRef = RemotingServices.Marshal(ric, this.URI);

            Trace.WriteLine("ObjRef.URI: " + oRef.URI);
            //Log.Put("ClientServices", "ObjRef.URI: " + oRef.URI);
        }

        public void Dispose()
        {
            if (ric != null)
            {
                RemotingServices.Disconnect(ric);
            }

            if (channel != null)
            {
                ChannelServices.UnregisterChannel(channel);
            }
        }
     
    }*/
}
