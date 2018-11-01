using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using Imi.Framework.Messaging;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter;
using Imi.Framework.Messaging.Adapter.Net.Sockets;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Framework.Messaging.Configuration;
using Imi.Framework.Shared.Configuration;
using Imi.Framework.Shared.Diagnostics;
using Imi.Wms.Mobile.Server.Subscribers;
using Imi.Wms.Mobile.Server.Components;
using Imi.Wms.Mobile.Server.Configuration;
using Imi.Wms.Mobile.Server.Adapter;
using System.Threading;
using System.Security;
using Imi.Framework.Shared;

namespace Imi.Wms.Mobile.Server
{
    /// <summary>
    /// Instance 
    /// </summary>
    public class ServerInstance : InstanceBase
    {
        private RemoteInterfaceProxy _remoteInterface;
        private Thread _timeoutThread;

        public override string applicationName
        {
            get { return "IMI iWMS Thin Client Server"; }
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="ServerInstance"/> class.</para>
        /// </summary>
        /// <param name="instanceName">
        /// A friendly name of the instance.
        /// </param>
        public ServerInstance(string instanceName)
            : base(instanceName)
        { 
        
        }

        /// <summary>
        /// Initializes the instance. Use this method to construct pipelines and subscribe to messages.
        /// </summary>
        public override void Initialize()
        {
            ServerSection config = ConfigurationManager.GetSection(ServerSection.SectionKey) as ServerSection;
            MessagingSection engineConfig = ConfigurationManager.GetSection(MessagingSection.SectionKey) as MessagingSection;

            ContextTraceListener contextListener = ((ContextTraceListener)MessageEngine.Instance.Tracing.Listeners["ContextTraceListener"]);

            if (contextListener == null)
            {
                contextListener = new ContextTraceListener(new RollingFileTraceListener(Path.Combine(config.LogPath, InstanceName + ".log"), config.MaxLogSize));
                contextListener.TraceOutputOptions = TraceOptions.ProcessId | TraceOptions.ThreadId | TraceOptions.DateTime;
                contextListener.Name = "ContextTraceListener";
                MessageEngine.Instance.Tracing.Listeners.Add(contextListener);
            }

            PropertyCollection tcpAdapterConfig = new PropertyCollection();
            tcpAdapterConfig.Write("TcpAdapterPort", config.TcpAdapter.Port);
            tcpAdapterConfig.Write("TcpEndPointTimeout", config.TcpAdapter.SocketIdleTimeout * 1000);

            TcpAdapter tcpAdapter = new TcpAdapter(tcpAdapterConfig, "tcp");
            tcpAdapter.MessageReceived += new EventHandler<AdapterReceiveEventArgs>(MessageReceivedEventHandler);
                        
            ComponentPipeline tcpReceivePipeline = new ComponentPipeline(PipelineType.Receive);

            PropertyCollection receiveCfg = new PropertyCollection();
            tcpReceivePipeline.AddComponent(typeof(StreamToMessageDisassembler), receiveCfg);
            
            PropertyCollection traceCfg = new PropertyCollection();

            traceCfg.Write("MaxLogSize", config.MaxLogSize);
            traceCfg.Write("LogPath", config.LogPath); 
            tcpReceivePipeline.AddComponent(typeof(TraceContextComponent), traceCfg);
                                                
            ComponentPipeline tcpSendPipeline = new ComponentPipeline(PipelineType.Send);
                        
            PropertyCollection appAdapterConfig = new PropertyCollection();
            appAdapterConfig.Write("ApplicationIdleTimeout", 999999999);

            ApplicationAdapter appAdapter = new ApplicationAdapter(appAdapterConfig, "app");
            appAdapter.EndPointDestroyed += EndPointDestroyedEventHandler;

            ComponentPipeline appReceivePipeline = new ComponentPipeline(PipelineType.Receive);
            ComponentPipeline appSendPipeline = new ComponentPipeline(PipelineType.Send);
                        
            //Bind adapters to pipelines
            MessageEngine.Instance.Bind(tcpAdapter, tcpReceivePipeline);
            MessageEngine.Instance.Bind(tcpAdapter, tcpSendPipeline);
            MessageEngine.Instance.Bind(appAdapter, appReceivePipeline);
            MessageEngine.Instance.Bind(appAdapter, appSendPipeline);
            
            MessageEngine.Instance.SubscriptionManager.Subscribe("http://www.im.se/wms/mobile/CreateSessionRequest", new CreateSessionSubscriber());
            MessageEngine.Instance.SubscriptionManager.Subscribe("http://www.im.se/wms/mobile/StateRequest", new StateSubscriber());
            MessageEngine.Instance.SubscriptionManager.Subscribe("http://www.im.se/wms/mobile/EventRequest", new EventSubscriber());
            MessageEngine.Instance.SubscriptionManager.Subscribe("http://www.im.se/wms/mobile/Application", new ApplicationSubscriber());
            MessageEngine.Instance.SubscriptionManager.Subscribe("http://www.im.se/wms/mobile/ConfigurationRequest", new ConfigurationSubscriber());
                                    
            //Add subscriber to reset the trace context after threads have finished
            MessageEngine.Instance.SubscriptionManager.Subscribe(new TraceContextReset());

            _remoteInterface = new RemoteInterfaceProxy(SessionManager.Instance);
            _remoteInterface.Initialize(config.ManagerPort, "IMIServer");
        }

        public override void Start()
        {
            base.Start();

            _timeoutThread = new Thread(TimeoutThread);
            _timeoutThread.Start();
        }
                
        public override void Stop()
        {
            base.Stop();

            try
            {
                foreach (ClientSession session in SessionManager.Instance.GetSessions())
                {
                    session.Dispose();
                }
            }
            finally
            {
                try
                {
                    if (_timeoutThread != null)
                    {
                        _timeoutThread.Abort();
                    }
                }
                catch (SecurityException)
                {
                }
                catch (ThreadStateException)
                {
                }
                finally
                {
                    _remoteInterface.Dispose();
                }
            }
        }

        private void EndPointDestroyedEventHandler(object sender, AdapterEndPointEventArgs e)
        {
            string sessionId = e.EndPoint.Uri.Host;

            ClientSession session = SessionManager.Instance[sessionId];

            if (session != null)
            {
                session.Kill();
            }
        }

        private void MessageReceivedEventHandler(object sender, AdapterReceiveEventArgs e)
        {
            ContextTraceListener contextListener = ((ContextTraceListener)MessageEngine.Instance.Tracing.Listeners["ContextTraceListener"]);

            if (contextListener != null)
                contextListener.ResetContext();
        }

        private void TimeoutThread()
        {
            ServerSection config = ConfigurationManager.GetSection(ServerSection.SectionKey) as ServerSection;
            MessagingSection engineConfig = ConfigurationManager.GetSection(MessagingSection.SectionKey) as MessagingSection;

            while (MessageEngine.Instance.IsRunning)
            {
                foreach (ClientSession session in SessionManager.Instance.GetSessions())
                {
                    bool hasLock = false;

                    try
                    {
                        Monitor.TryEnter(session, ref hasLock);

                        if (!hasLock)
                        {
                            continue;
                        }

                        DateTime lastActivityTime;
                        DateTime? killTime;

                        lock (session.SyncLock)
                        {
                            lastActivityTime = session.LastActivityTime;
                            killTime = session.KillTime;
                        }

                        if (((DateTime.Now - lastActivityTime).TotalSeconds >= config.SessionIdleTimeout) || (killTime != null && DateTime.Now >= killTime))
                        {
                            try
                            {
                                ApplicationAdapterEndPoint endPoint = session.ApplicationEndPoint;

                                if (endPoint != null)
                                {
                                    ApplicationAdapter appAdapter = MessageEngine.Instance.AdapterProxy.ResolveUriToAdapter(endPoint.Uri) as ApplicationAdapter;

                                    if (appAdapter != null)
                                    {
                                        appAdapter.CloseApplication(endPoint);
                                    }
                                }
                            }
                            finally
                            {
                                SessionManager.Instance.DestroySession(session.Id);
                                session.Dispose();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ExceptionHelper.IsCritical(ex))
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        if (hasLock)
                        {
                            Monitor.Exit(session);
                        }
                    }
                }

                Thread.Sleep(5000);
            }
        }
    }
}
