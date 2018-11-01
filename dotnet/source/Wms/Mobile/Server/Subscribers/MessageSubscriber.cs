using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Framework.Messaging.Adapter.Net.Sockets;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Shared.Diagnostics;
using Imi.Wms.Mobile.Server.Configuration;
using Imi.Framework.Messaging.Adapter;
using Imi.Framework.Messaging.Configuration;
using Imi.Wms.Mobile.Server.Interface;

namespace Imi.Wms.Mobile.Server.Subscribers
{    
    [SessionPolicy(SessionPolicy.Required)]
    public abstract class MessageSubscriber<TRequest, TResponse> : SubscriberBase
    {
        private readonly SessionPolicy _sessionPolicy;
        protected XmlSerializer _requestSerializer;
        protected XmlSerializer _responseSerializer;
        private XmlSerializer _exceptionSerializer;
        private const int EndOfFileLength = 4;
        private MessagingSection _config;
        
        [ThreadStatic]
        protected static ClientSession Session;

        [ThreadStatic]
        protected static MultiPartMessage SourceMessage;

        public MessageSubscriber()
        {
            SessionPolicyAttribute attribute = (SessionPolicyAttribute)this.GetType().GetCustomAttributes(typeof(SessionPolicyAttribute), true)[0];
            _sessionPolicy = attribute.Policy;
            _config = ConfigurationManager.GetSection(MessagingSection.SectionKey) as MessagingSection;
            _requestSerializer = new XmlSerializer(typeof(TRequest));
            _responseSerializer = new XmlSerializer(typeof(TResponse));
            _exceptionSerializer = new XmlSerializer(typeof(ServerFault));
        }

        protected override void InternalInvoke(MultiPartMessage msg)
        {
            Session = null;
            bool hasLock = false;

            try
            {
                TraceMessageData("Data received: ", msg);

                SourceMessage = msg;
                Session = GetSession(msg);

                if (Session != null)
                {
                    lock (Session.SyncLock)
                    {
                        Session.LastActivityTime = DateTime.Now;
                    }

                    Session.AbortEvent.Set();

                    Monitor.TryEnter(Session, _config.PendingOperationsTimeout, ref hasLock);

                    if (!hasLock)
                    {
                        if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                        {
                            MessageEngine.Instance.Tracing.TraceData(TraceEventType.Verbose, 0, "Application busy, request aborted.");
                        }

                        return;
                    }

                    CheckAndThrowException();

                    if (IsDuplicate(SourceMessage))
                    {
                        if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                        {
                            MessageEngine.Instance.Tracing.TraceData(TraceEventType.Verbose, 0, "Duplicate message detected, retransmitting last response...");
                        }

                        msg.Metadata.Copy("ReceiveUri", Session.LastResponseMessage.Metadata);
                        Session.LastResponseMessage.Metadata.Write("Loopback", true);

                        try
                        {
                            MessageEngine.Instance.TransmitMessage(Session.LastResponseMessage);
                        }
                        catch (AdapterException)
                        {
                        }

                        return;
                    }

                    if (Session.LastRequestMessage != null)
                    {
                        Session.LastRequestMessage.Dispose();
                    }

                    if (Session.LastResponseMessage != null)
                    {
                        Session.LastResponseMessage.Dispose();
                    }

                    Session.LastRequestMessage = msg;
                }

                msg.Data.Seek(0, SeekOrigin.Begin);

                TRequest requestMessage = (TRequest)_requestSerializer.Deserialize(msg.Data);

                Invoke(requestMessage);
            }
            catch (Exception ex)
            {
                ServerFault serverFault = new ServerFault();
                serverFault.Type = ex.GetType().ToString();
                serverFault.Message = ex.Message;
                
                if (ex.Data.Contains("ErrorCode"))
                {
                    serverFault.ErrorCode = ex.Data["ErrorCode"].ToString();
                }

                serverFault.ServerStackTrace = ex.ToString();

                TransmitFaultMessage(serverFault);

                throw;
            }
            finally
            {
                if (hasLock)
                {
                    Monitor.Exit(Session);
                }
            }
        }

        protected static void CheckAndThrowException()
        {
            if (Session != null)
            {
                if (Session.ApplicationEndPoint != null && Session.ApplicationEndPoint.Exception != null)
                {
                    throw Session.ApplicationEndPoint.Exception;
                }
                else if (Session.KillTime != null)
                {
                    throw new SessionNotFoundException("Your session has been terminated.");
                }
            }
        }

        private void TransmitFaultMessage(ServerFault serverFault)
        {
            MultiPartMessage temp = new MultiPartMessage(new MemoryStream(4096));
            _exceptionSerializer.Serialize(temp.Data, serverFault);

            AppendEndOfFile(temp);

            TraceMessageData("Sending data: ", temp);

            SourceMessage.Metadata.Copy("ReceiveUri", temp.Metadata);
            temp.Metadata.Write("Loopback", true);

            if (Session != null)
            {
                Session.LastResponseMessage = temp;
            }

            try
            {
                MessageEngine.Instance.TransmitMessage(temp);
            }
            catch (AdapterException)
            {
            }
        }
                
        public abstract void Invoke(TRequest request);
        
        public override void Invoke(MultiPartMessage msg)
        { 
        }
                    
        protected void TransmitResponseMessage(TResponse response)
        {
            if (!MessageEngine.Instance.IsRunning)
            {
                return;
            }

            CheckAndThrowException();

            MultiPartMessage temp = new MultiPartMessage(new MemoryStream(8192));
            _responseSerializer.Serialize(temp.Data, response);
                                    
            AppendEndOfFile(temp);

            TraceMessageData("Sending data: ", temp);
            
            SourceMessage.Metadata.Copy("ReceiveUri", temp.Metadata);
            temp.Metadata.Write("Loopback", true);

            if (Session != null)
            {
                Session.LastResponseMessage = temp;
            }
                        
            try
            {
                MessageEngine.Instance.TransmitMessage(temp);
            }
            catch (AdapterException)
            { 
            }
        }

        private static void TraceMessageData(string text, MultiPartMessage temp)
        {
            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
            {
                temp.Data.Seek(0, SeekOrigin.Begin);
                string data = Encoding.UTF8.GetString(((MemoryStream)temp.Data).ToArray(), 0, (int)temp.Data.Length).Replace('\0', '0');
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "{0}\"{1}\"", text, data);
            }
        }

        private bool IsDuplicate(MultiPartMessage msg)
        {
            int sequence = msg.Metadata.ReadAsInt("Sequence");

            if ((Session.LastRequestMessage != null)
                && (Session.LastResponseMessage != null)
                && (msg.MessageType == Session.LastRequestMessage.MessageType)
                && (msg.MessageType != "http://www.im.se/wms/mobile/StateRequest")
                && (sequence == Session.LastRequestMessage.Metadata.ReadAsInt("Sequence")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void AppendEndOfFile(MultiPartMessage msg)
        {
            msg.Data.Seek(0, SeekOrigin.End);
            
            for (int i = 0; i < EndOfFileLength; i++)
            {
                msg.Data.WriteByte(0);
            }
        }

        private ClientSession GetSession(MultiPartMessage msg)
        {
            ClientSession session = SessionManager.Instance[msg.Metadata.ReadAsString("SessionId")];

            if (session == null && msg.MessageType == "http://www.im.se/wms/mobile/CreateSessionRequest")
            {
                session = SessionManager.Instance.CreateSession(msg.Metadata.ReadAsString("SessionId"));
            }

            if ((session == null) && (_sessionPolicy == SessionPolicy.Required))
            {
                throw new SessionNotFoundException(string.Format("Session object not found for endpoint: \"{1}\" ({0}).", msg.Metadata.Read("ReceiveUri").ToString(), msg.Metadata.ReadAsString("SessionId")));
            }
                        
            return session;
        }
    }
        
}
