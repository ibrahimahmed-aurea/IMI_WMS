using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Configuration;
using System.ComponentModel;
using System.Security;
using System.Reflection;
using Imi.Framework.Shared;
using Imi.Framework.Messaging.Adapter;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Configuration;
using Imi.Wms.Mobile.Server.Configuration;

namespace Imi.Wms.Mobile.Server.Adapter
{
    public class ApplicationAdapter : AdapterBase
    {
        private object _syncObject;
        private ServerSection _config;
        private DesktopManager _desktopManager;
                        
        public ApplicationAdapter(PropertyCollection configuration, string id)
            : base(configuration, id)
        {
            _syncObject = new object();
            _config = ConfigurationManager.GetSection(ServerSection.SectionKey) as ServerSection;
            _desktopManager = new DesktopManager();
        }

        /// <summary>
        /// Returns the protocol used by this adapter.
        /// </summary>
        public override string ProtocolType
        {
            get
            {
                return "app";
            }
        }

        protected override void Initialize()
        {
            _desktopManager.Initialize(_config.DesktopHeapSizeInKB);
        }

        /// <summary>
        /// Transmits a message over the adapter protocol.
        /// </summary>
        /// <param name="msg">The message to transmit.</param>
        public override void TransmitMessage(MultiPartMessage msg)
        {
            Uri sendUri = (Uri)msg.Metadata.Read("SendUri");

            ApplicationAdapterEndPoint endPoint
                = (ApplicationAdapterEndPoint)MessageEngine.Instance.AdapterProxy.ResolveUriToEndPoint(sendUri);

            if (endPoint == null)
            {
                throw new AdapterException("Failed to transmit message to Uri: \"" + sendUri + "\". The EndPoint does not exist.");
            }

            try
            {
                endPoint.TransmitMessage(msg);
            }
            catch (Exception ex)
            {
                throw new AdapterException("Failed to transmit message to EndPoint: \"" + ToString() + "\".", ex);
            }
        }
                
        public ApplicationAdapterEndPoint StartApplication(string applicationName, string sessionId)
        {
            ApplicationAdapterEndPoint endPoint = null;

            try
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    throw new ArgumentNullException("sessionId");
                }

                ClientSession session = SessionManager.Instance[sessionId];

                if (session == null)
                {
                    throw new SessionNotFoundException(string.Format("Invalid session ID: \"{0}\"", sessionId));
                }

                var applicationElement = (from ApplicationElement e in _config.ApplicationCollection
                                         where e.Name == applicationName
                                         select e).FirstOrDefault();

                if (applicationElement == null)
                {
                    throw new ArgumentException("Invalid application name.", "applicationName");
                }

                endPoint = new ApplicationAdapterEndPoint(this, applicationName, sessionId);

                string arguments = applicationElement.Arguments;

                foreach (PropertyInfo propertyInfo in session.GetType().GetProperties())
                {
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        arguments = arguments.Replace(string.Format("@{0}", propertyInfo.Name), propertyInfo.GetValue(session, null) as string);
                    }
                }

                endPoint.UnhandledException += UnhandledExceptionEventHandler;
                endPoint.MessageReceived += MessageReceivedEventHandler;
                OnEndPointCreated(endPoint);
                endPoint.StartApplication(applicationElement.ExecutablePath, arguments, _desktopManager.DesktopName);
                
                return endPoint;
            }
            catch (Exception ex)
            {
                if (endPoint != null)
                {
                    CloseApplication(endPoint);
                }

                throw new AdapterException("Failed to start application.", ex);
            }
        }

        private void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            CloseApplication((ApplicationAdapterEndPoint)sender);
        }

        public void CloseApplication(ApplicationAdapterEndPoint endPoint)
        {
            endPoint.UnhandledException -= UnhandledExceptionEventHandler;
            endPoint.MessageReceived -= MessageReceivedEventHandler;
            endPoint.Dispose();
            OnEndPointDestroyed(endPoint);
        }
        
        private void MessageReceivedEventHandler(object sender, AdapterReceiveEventArgs e)
        {
            OnMessageReceived(e.Message, e.ReceiveEndPoint);
        }
                       
        /// <summary>
        /// Disposes any unmanaged resources used by this adapter.
        /// </summary>
        /// <param name="disposing">True if called from user code.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                try
                {
                    if (disposing)
                    {
                        _desktopManager.Dispose();

                        foreach (ApplicationAdapterEndPoint endPoint in GetEndPoints())
                        {
                            endPoint.Dispose();
                        }
                    }
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
