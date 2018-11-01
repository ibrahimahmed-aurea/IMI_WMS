using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Data;
using System.Security;
using System.Linq;
using Imi.Framework.Messaging.Engine;
using Imi.Wms.Mobile.Server.Interface;

namespace Imi.Wms.Mobile.Server
{
    public class RemoteInterfaceProxy : MarshalByRefObject, IRemoteInterface, IDisposable 
    {
        private DateTime _sessionListTimeStamp = DateTime.MinValue;
        private DataTable _sessionTable;
        private IChannel _channel;
        private ObjRef _remoteRef;
        private bool _isDisposed;
        private SessionManager _sessionManager;
                
        public RemoteInterfaceProxy(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            _sessionTable = new DataTable("Sessions");
            _sessionTable.Columns.Add("Id");
            _sessionTable.Columns.Add("Application");
            _sessionTable.Columns.Add("PID");
            _sessionTable.Columns.Add("LastActivity");
            _sessionTable.Columns.Add("IP");
            _sessionTable.Columns.Add("UserId");
            _sessionTable.Columns.Add("TerminalId");
            _sessionTable.Columns.Add("Version");
            _sessionTable.Columns.Add("Platform");
        }

        ~RemoteInterfaceProxy()
        {
            Dispose(false);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Initialize(int port, string uri)
        {
            _channel = new HttpChannel(port);
            ChannelServices.RegisterChannel(_channel, false);

            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
                        
            _remoteRef = RemotingServices.Marshal(this, uri);

            MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "ObjRef.URI: {0}", _remoteRef.URI);
        }
        
        public string KillSession(string sessionId)
        {
            MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"KillSession {0}\"", sessionId);

            ClientSession session = SessionManager.Instance[sessionId];

            if (session != null)
            {
                lock (session.SyncLock)
                {
                    session.Kill();
                }
            }

            return ("ok");
        }

        public string KillAllSessions()
        {
            MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"KillAllSessions\"");

            foreach (ClientSession session in _sessionManager.GetSessions())
            {
                lock (session.SyncLock)
                {
                    session.Kill();
                }
            }

            return ("ok");
        }
        
        public DataTable GetSessionList()
        {
            MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"GetSessionList\"");

            lock (_sessionTable)
            {
                TimeSpan ts = DateTime.Now.Subtract(_sessionListTimeStamp);

                if (ts.TotalSeconds > 1.0)
                {
                    _sessionTable.Rows.Clear();
                    
                    foreach (ClientSession session in _sessionManager.GetSessions())
                    {
                        lock (session.SyncLock)
                        {
                            if (session.KillTime != null)
                            {
                                continue;
                            }

                            DataRow row = _sessionTable.NewRow();

                            row["Id"] = session.Id;

                            if (session.ApplicationEndPoint != null)
                            {
                                row["Application"] = session.ApplicationEndPoint.ApplicationName;
                            }

                            if (session.ApplicationEndPoint != null)
                            {
                                row["PID"] = session.ApplicationEndPoint.ProcessId;
                            }
                            
                            row["LastActivity"] = session.LastActivityTime;
                            row["IP"] = session.ClientIP;
                            row["UserId"] = session.UserId;
                            row["TerminalId"] = session.TerminalId;
                            row["Version"] = session.ClientVersion;
                            row["Platform"] = session.ClientPlatform;

                            _sessionTable.Rows.Add(row);
                        }
                    }
                                        
                    _sessionListTimeStamp = DateTime.Now;
                }
            }

            return _sessionTable;
        }

        public DateTime Time()
        {
            MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"Time\"");
            return (DateTime.Now);
        }

        public StateResponse GetSessionState(string sessionId)
        {
            ClientSession session = SessionManager.Instance[sessionId];

            if (session != null)
            {
                lock (session.SyncLock)
                {
                    return session.StateResponse;
                }
            }
            else
            {
                return null;
            }
        }

        public string SetParameter(string sessionId, string name, string value)
        {
            MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"SetParameter\" for \"{0} {1}={2}\"", sessionId, name, value);
            
            return ("ok");
        }
        
        public string GetParameter(string sessionId, string name)
        {
            MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"GetParameter\" for \"{0} {1}\"", sessionId, name);
            
            return "ok";
        }

        public string SetTraceLevel(string sessionId, string level)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"SetTraceLevel\" for server level=\"{0}\"", level);
                MessageEngine.Instance.Tracing.Switch.Level = (SourceLevels)Enum.Parse(typeof(SourceLevels), level);
            }
            else
            {
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"SetTraceLevel\" for session \"{0}\" level=\"{1}\"", sessionId, level);
                
                ClientSession session = SessionManager.Instance[sessionId];
                
                if (session != null)
                {
                    session.ClientTraceLevel = (SourceLevels)Enum.Parse(typeof(SourceLevels), level);
                }
            }

            return ("ok");
        }


        public string GetTraceLevel(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"GetTraceLevel\" for server", sessionId);
                return MessageEngine.Instance.Tracing.Switch.Level.ToString();
            }
            else
            {
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"GetTraceLevel\" for session \"{0}\"", sessionId);

                ClientSession session = SessionManager.Instance[sessionId];

                if (session != null)
                {
                    return session.ClientTraceLevel.ToString();
                }
            }

            return ("ok");
        }
        
        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    try
                    {
                        RemotingServices.Disconnect(this);
                    }
                    catch (ArgumentException)
                    {
                    }
                    catch (SecurityException)
                    { 
                    }

                    if (_channel != null)
                    {
                        try
                        {
                        ChannelServices.UnregisterChannel(_channel);
                        }
                        catch (ArgumentException)
                        {
                        }
                        catch (SecurityException)
                        {
                        }
                    }
                }
            }

            _isDisposed = true;
        }

        #endregion
    }
}
