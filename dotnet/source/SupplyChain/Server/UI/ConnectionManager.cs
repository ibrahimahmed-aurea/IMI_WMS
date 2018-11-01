using System;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using System.Runtime.Remoting;
using Imi.SupplyChain.Server.UI.Configuration;
using Imi.Framework.Job.RemoteInterface;
using System.Collections.Generic;

namespace Imi.SupplyChain.Server.UI
{
    public delegate void ConnectStateEventDelegate(ManagedConnection sender, bool connected);

    public class ManagedConnection
    {
        private ServerType config;
        private bool connected;
        private bool oldConnectedState;
        private IRemoteInterface remoteInterface;
        private TimeSpan timeDiff = new TimeSpan(0);

        public static ConnectStateEventDelegate ConnectStateChanged;

        private static void OnConnectStateChanged(ManagedConnection mc, bool oldConnectedState, bool newConnectedState)
        {
            if (ConnectStateChanged != null)
            {
                if (oldConnectedState != newConnectedState)
                {
                    ConnectStateChanged(mc, newConnectedState);
                    oldConnectedState = newConnectedState;
                }
            }
        }

        private void OnConnectStateChanged()
        {
            if (oldConnectedState != connected)
            {
                OnConnectStateChanged(this, oldConnectedState, connected);
                 oldConnectedState = connected;
             }
        }

        public ManagedConnection(ServerType c)
        {
            connected = false;
            config = c;
        }

        public static ManagedConnection CreateDefaultConnection()
        {
            ServerType s = new ServerType();
            s.DisplayName = "<No name>";
            s.HostName = "<Unknown>";
            s.Port = 9000;
            return (new ManagedConnection(s));
        }
        public IRemoteInterface Instance
        {
            get
            {
                return (remoteInterface);
            }
        }

        public ServerType Config
        {
            get
            {
                return (config);
            }
        }

        public bool Connected
        {
            get
            {
                return (connected);
            }
        }

        public void Disconnect()
        {
            connected = false;
            remoteInterface = null;
            OnConnectStateChanged();
        }

        public bool Connect()
        {
            connected = false;

            try
            {
                remoteInterface = (IRemoteInterface)Activator.GetObject(typeof(IRemoteInterface), new UriBuilder("http", config.HostName, config.Port, "/IMIServer").ToString());

                DateTime serverTime = remoteInterface.Time();
                timeDiff = serverTime - DateTime.Now;
                connected = true;
                OnConnectStateChanged();
            }
            catch (Exception)
            {
                remoteInterface = null;
                throw;
            }

            return (connected);
        }

        public String Ps()
        {
            return (remoteInterface.Ps());
        }

        public TimeSpan GetInstanceTimeDiff()
        {
            return (timeDiff);
        }
    }

    /// <summary>
    /// Summary description for ConnectionManager.
    /// </summary>
    //public class ConnectionManager
    //{
    //    private List<ManagedConnection> managedConnections;
    //    private static ManagedConnection current;

    //    public void Refresh()
    //    {
    //        if (managedConnections != null)
    //        {
    //            foreach (ManagedConnection mc in managedConnections)
    //            {
    //                mc.Disconnect();
    //            }
    //        }

    //        UIConfigFileHandler gc = UIConfigFileHandler.Instance();

    //        try
    //        {
    //            gc.Load();
    //        }
    //        catch (Exception)
    //        {
    //        }

    //        managedConnections = new List<ManagedConnection>();

    //        foreach (ServerType sct in gc.Connections)
    //        {
    //            ManagedConnection mc = new ManagedConnection(sct);
    //            managedConnections.Add(mc);
    //        }
    //    }

    //    public ConnectionManager()
    //    {
    //        Refresh();
    //    }

    //    public static ManagedConnection CurrentConnection
    //    {
    //        get
    //        {
    //            return (current);
    //        }
    //        set
    //        {
    //            current = value;
    //        }
    //    }

    //    public ManagedConnection[] Connections
    //    {
    //        get
    //        {
    //            return (managedConnections.ToArray());
    //        }
    //    }



    //    public void AddConnection(ManagedConnection nu)
    //    {
    //        if (!managedConnections.Contains(nu))
    //        {
    //            managedConnections.Add(nu);
    //        }
    //    }

    //    public void RemoveConnection(ManagedConnection c)
    //    {
    //        managedConnections.Remove(c);
    //    }
    //}
}
