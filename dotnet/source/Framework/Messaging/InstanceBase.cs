using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Configuration;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Shared;
using Imi.Framework.Shared.Configuration;
using Imi.Framework.Shared.Diagnostics;

namespace Imi.Framework.Messaging
{
    /// <summary>
    /// Base class for message engine instances.
    /// </summary>
    public abstract class InstanceBase : ApplicationInstance, IDisposable
    {
        private FileSystemWatcher configWatcher;
        private bool disposed;

        public abstract string applicationName
        {
            get;
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="InstanceBase"/> class.</para>
        /// </summary>
        protected InstanceBase(string instanceName) : base(instanceName)
        {
            Console.WriteLine(string.Format("{0} v{1}", applicationName , System.Reflection.Assembly.GetExecutingAssembly().GetName().Version));
            Console.WriteLine("Copyright (c) Aptean");
            
            if (string.IsNullOrEmpty(instanceName))
                throw new ArgumentException("No instance name specified.");
            
            Console.WriteLine(string.Format("Loading instance: {0}...", instanceName));
            
            AppDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionEventHandler);

            string configFile = AppDomain.SetupInformation.ConfigurationFile;

            Console.WriteLine(string.Format("Configuration file: {0}", configFile));

            if (!string.IsNullOrEmpty(configFile))
            {
                configWatcher = new FileSystemWatcher(Path.GetDirectoryName(configFile), Path.GetFileName(configFile));
                configWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite;
                configWatcher.Changed += new FileSystemEventHandler(ConfigurationChangedEventHandler);
                configWatcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~InstanceBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Called when the configuation file has been updated.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected void ConfigurationChangedEventHandler(object sender, FileSystemEventArgs e)
        {
            List<TraceListener> listenerList = new List<TraceListener>();

            try
            {
                #region Wait for file access

                int count = 10;

                while (count > 0)
                {
                    StreamReader reader = null;

                    try
                    {
                        reader = new StreamReader(e.FullPath);
                        break;
                    }
                    catch (IOException)
                    {
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }

                    Thread.Sleep(200);

                    count--;
                }

                #endregion

                foreach (TraceListener listener in MessageEngine.Instance.Tracing.Listeners)
                    listenerList.Add(listener);

                Trace.Refresh();

                MessageEngine.Instance.Tracing.Listeners.Clear();
                MessageEngine.Instance.Tracing.Listeners.AddRange(listenerList.ToArray());

                if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Information) == SourceLevels.Information)
                    MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Information, 0, "Configuration updated.");
            }
            catch (IOException ex)
            {
                MessageEngine.Instance.Tracing.Switch.Level = SourceLevels.Error;
                MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, ex);
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageEngine.Instance.Tracing.Switch.Level = SourceLevels.Error;
                MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, ex);
            }
            catch (ArgumentException ex)
            {
                MessageEngine.Instance.Tracing.Switch.Level = SourceLevels.Error;
                MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, ex);
            }
        }
                
        protected void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            ContextTraceListener contextListener = ((ContextTraceListener)MessageEngine.Instance.Tracing.Listeners["ContextTraceListener"]);

            if (contextListener != null)
                contextListener.ResetContext();

            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Critical) == SourceLevels.Critical)
                MessageEngine.Instance.Tracing.TraceData(TraceEventType.Critical, 0, e.ExceptionObject);
        }

        /// <summary>
        /// Initializes the instance. Use this method to construct pipelines and subscribe to messages.
        /// </summary>
        public abstract void Initialize();
        
        /// <summary>
        /// Starts the message engine.
        /// </summary>
        public virtual void Start()
        {
            MessageEngine.Instance.Start();
        }

        /// <summary>Obtains a lifetime service object to control the lifetime policy for this instance.</summary>
        /// <returns>An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"></see> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"></see> property.</returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        /// </PermissionSet>
        public override object InitializeLifetimeService()
        {
            //Infinite lease time, since this object will only be marshalled across AppDomains
            return null;
        }

        /// <summary>
        /// Gets or sets the trace level of the message engine. 
        /// </summary>
        public SourceLevels TraceLevel
        {
            set
            {
                MessageEngine.Instance.Tracing.Switch.Level = value;
            }
            get
            {
                return MessageEngine.Instance.Tracing.Switch.Level;
            }
        }
        
        public virtual void Stop()
        {
            try
            {
                MessageEngine.Instance.Stop();
            }
            finally
            {
                configWatcher.Dispose();
            }
        }


        #region IDisposable Members

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">True if called from user code, otherwise false.</param>
        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    MessageEngine.Instance.Dispose();
                    configWatcher.Dispose();
                }
            }

            disposed = true;
        }

        #endregion
    }
}
