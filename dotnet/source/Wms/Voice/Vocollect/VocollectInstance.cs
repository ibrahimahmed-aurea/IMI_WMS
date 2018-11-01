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
using Imi.Wms.Voice.Vocollect.Subscribers;
using Imi.Wms.Voice.Vocollect.Components;
using Imi.Wms.Voice.Vocollect.Configuration;

namespace Imi.Wms.Voice.Vocollect
{
    /// <summary>
    /// Instance 
    /// </summary>
    public class VocollectInstance : InstanceBase
    {
        public override string applicationName
        {
            get { return "IMI iWMS Voice Server"; }
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="VocollectInstance"/> class.</para>
        /// </summary>
        /// <param name="instanceName">
        /// A friendly name of the instance.
        /// </param>
        public VocollectInstance(string instanceName)
            : base(instanceName)
        { 
        
        }

        /// <summary>
        /// Initializes the instance. Use this method to construct pipelines and subscribe to messages.
        /// </summary>
        public override void Initialize()
        {
            VocollectSection config = ConfigurationManager.GetSection(VocollectSection.SectionKey) as VocollectSection;
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
            tcpAdapterConfig.Write("TcpEndPointTimeout", engineConfig.PendingOperationsTimeout);
            TcpAdapter tcpAdapter = new TcpAdapter(tcpAdapterConfig, "tcp");

            tcpAdapter.EndPointCreated += new EventHandler<AdapterEndPointEventArgs>(EndPointCreatedEventHandler);
            tcpAdapter.MessageReceived += new EventHandler<AdapterReceiveEventArgs>(MessageReceivedEventHandler);
                        
            ComponentPipeline tcpReceivePipeline = new ComponentPipeline(PipelineType.Receive);

            PropertyCollection receiveCfg = new PropertyCollection();
            receiveCfg.Write("CodePageName", config.CodePageName);
            tcpReceivePipeline.AddComponent(typeof(StreamToXmlDisassembler), receiveCfg);

            PropertyCollection traceCfg = new PropertyCollection();

            traceCfg.Write("MaxLogSize", config.MaxLogSize);
            traceCfg.Write("LogPath", config.LogPath); 
            tcpReceivePipeline.AddComponent(typeof(TraceContextComponent), traceCfg);
            
            /*
            tcpReceivePipeline.AddComponent(typeof(MessageAcknowledgeComponent), ackCfg);
            */
                                    
            PropertyCollection transmissionControlCfg = new PropertyCollection();
            transmissionControlCfg.Write("PendingOperationsTimeout", engineConfig.PendingOperationsTimeout);
                        
            tcpReceivePipeline.AddComponent(typeof(TransmissionControlComponent), transmissionControlCfg);
            
            PropertyCollection xsltCfg = new PropertyCollection();
            xsltCfg.Write("XsltPath", config.XsltPath);
            
            tcpReceivePipeline.AddComponent(typeof(XslTransformComponent), xsltCfg);
            
            ComponentPipeline tcpSendPipeline = new ComponentPipeline(PipelineType.Send);
            PropertyCollection sendCfg = new PropertyCollection();
            sendCfg.Write("CodePageName", config.CodePageName);
            tcpSendPipeline.AddComponent(typeof(MessageToStreamAssembler), sendCfg);
                        
            PropertyCollection whAdapterConfig = new PropertyCollection();

            //Read connection strings
            foreach (ConnectionStringSettings connection in ConfigurationManager.ConnectionStrings)
            {
                whAdapterConfig.Write(connection.Name, connection.ConnectionString);
            }

            WarehouseAdapter whAdapter = new WarehouseAdapter(whAdapterConfig, "warehouse");

            ComponentPipeline whReceivePipeline = new ComponentPipeline(PipelineType.Receive);
            ComponentPipeline whSendPipeline = new ComponentPipeline(PipelineType.Send);
                        
            //Bind adapters to pipelines
            MessageEngine.Instance.Bind(tcpAdapter, tcpReceivePipeline);
            MessageEngine.Instance.Bind(tcpAdapter, tcpSendPipeline);
            MessageEngine.Instance.Bind(whAdapter, whReceivePipeline);
            MessageEngine.Instance.Bind(whAdapter, whSendPipeline);
                                                
            //Add subscribers 
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.BaseType == typeof(VocollectSubscriber) && t.Namespace == "Imi.Wms.Voice.Vocollect.Subscribers")
                {
                    MessageEngine.Instance.SubscriptionManager.Subscribe("http://www.im.se/wms/voice/vocollect/" + t.Name, (SubscriberBase)t.GetConstructor(new Type[] {}).Invoke(new object[] {}));
                }
            }

            //Add subscriber to reset the trace context after threads have finished
            MessageEngine.Instance.SubscriptionManager.Subscribe(new TraceContextReset());
                        
        }

        private void MessageReceivedEventHandler(object sender, AdapterReceiveEventArgs e)
        {
            ContextTraceListener contextListener = ((ContextTraceListener)MessageEngine.Instance.Tracing.Listeners["ContextTraceListener"]);

            if (contextListener != null)
                contextListener.ResetContext();
        }

        private void EndPointCreatedEventHandler(object sender, AdapterEndPointEventArgs e)
        {
            ContextTraceListener contextListener = ((ContextTraceListener)MessageEngine.Instance.Tracing.Listeners["ContextTraceListener"]);

            if (contextListener != null)
                contextListener.ResetContext();
        }
                
    }
}
