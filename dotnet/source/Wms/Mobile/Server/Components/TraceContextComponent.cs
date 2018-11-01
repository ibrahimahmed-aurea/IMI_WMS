using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Diagnostics;
using System.IO;
using Imi.Framework.Shared.Diagnostics;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter;

namespace Imi.Wms.Mobile.Server.Components
{
    /// <summary>
    /// Sets the trace context for the current thread, directing the trace output to the correct destination.
    /// </summary>
    [Persistence(PersistenceMode.Adapter)]
    public class TraceContextComponent : PipelineComponent
    {
        private readonly ulong maxLogSize = 0;
        private readonly string logPath;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="TraceContextComponent"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// Configuration properties.
        /// </param>
        public TraceContextComponent(PropertyCollection configuration)
            : base(configuration)
        {
            maxLogSize = (ulong)configuration.Read("MaxLogSize");
            logPath = (string)configuration.Read("LogPath");
        }

        /// <summary>
        /// Called by the pipeline to invoke the component.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>A collection of messages produced by this component.</returns>
        public override Collection<MultiPartMessage> Invoke(MultiPartMessage msg)
        {
            Collection<MultiPartMessage> resultCollection = new Collection<MultiPartMessage>();
            resultCollection.Add(msg);
                      
            if (MessageEngine.Instance.Tracing.Switch.Level != SourceLevels.Off)
            {
                ContextTraceListener contextListener = ((ContextTraceListener)MessageEngine.Instance.Tracing.Listeners["ContextTraceListener"]);

                if (contextListener != null)
                {
                    contextListener.Context = null;

                    string sessionId = msg.Metadata.ReadAsString("SessionId");

                    ClientSession session = SessionManager.Instance[sessionId];

                    if (session != null)
                    {
                        string name = null;
                        string ip = null;

                        lock (session.SyncLock)
                        {
                            name = session.TerminalId;
                            ip = session.ClientIP;
                        }

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(ip))
                        {
                            string context = string.Format("{0}_{1}", name, ip.Replace('.', '_'));

                            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                            {
                                MessageEngine.Instance.Tracing.TraceData(TraceEventType.Verbose, 0, string.Format("TraceContextComponent: Switching context to: \"{0}\"", context));
                            }

                            if (!contextListener.IsContextInitialized(context))
                            {
                                contextListener.InitializeContext(context,
                                    new RollingFileTraceListener(Path.Combine(logPath, context + ".log"), maxLogSize));
                            }

                            contextListener.Context = context;
                        }
                    }
                }
            }

            return resultCollection;
        }

        /// <summary>
        /// Checks if the component supports processing of a specified message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>True if the message is supported, othwerwise false.</returns>
        public override bool Supports(MultiPartMessage msg)
        {
            if (msg.MessageType.StartsWith("http://www.im.se/wms/mobile/"))
                return true;
            else
                return false;
        }
    }
}
