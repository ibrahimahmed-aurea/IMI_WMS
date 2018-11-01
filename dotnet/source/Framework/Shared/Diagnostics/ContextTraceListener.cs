using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Globalization;


namespace Imi.Framework.Shared.Diagnostics
{
    /// <summary>
    /// Context aware trace listener for tracing in a multi-threaded environment.
    /// </summary>
    public class ContextTraceListener : TraceListener
    {
        private ReaderWriterLock listenerSyncLock;
        private Dictionary<string, TraceListener> listenerDictionary;
        private TraceListener defaultListener;
        
        [ThreadStatic]
        private string currentContext;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="ContextTraceListener"/> class.</para>
        /// </summary>
        /// <param name="defaultListener">
        /// The default listener to use when no context is specified.
        /// </param>
        public ContextTraceListener(TraceListener defaultListener)
        {
            if (defaultListener == null)
                throw new ArgumentNullException("defaultListener");
            
            listenerSyncLock = new ReaderWriterLock();
            listenerDictionary = new Dictionary<string, TraceListener>();

            this.defaultListener = defaultListener;
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="ContextTraceListener"/> class.</para>
        /// </summary>
        public ContextTraceListener()
           : this(new ConsoleTraceListener())
        { 
        
        }

        /// <summary>When overridden in a derived class, writes the specified message to the listener you create in the derived class.</summary>
        /// <param name="message">A message to write. </param>
        /// <filterpriority>2</filterpriority>
        public override void Write(string message)
        {
            GetContextListener(currentContext).Write(message);
        }

        /// <summary>When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.</summary>
        /// <param name="message">A message to write. </param>
        /// <filterpriority>2</filterpriority>
        public override void WriteLine(string message)
        {
            GetContextListener(currentContext).WriteLine(message);
        }

        /// <summary>When overridden in a derived class, flushes the output buffer.</summary>
        /// <filterpriority>2</filterpriority>
        public override void Flush()
        {
            GetContextListener(currentContext).Flush();
        }

        /// <summary>When overridden in a derived class, closes the output stream so it no longer receives tracing or debugging output.</summary>
        /// <filterpriority>2</filterpriority>
        public override void Close()
        {
            Dispose(true);
        }

        /// <summary>Gets a value indicating whether the trace listener is thread safe. </summary>
        /// <returns>true if the trace listener is thread safe; otherwise, false. The default is false.</returns>
        public override bool IsThreadSafe
        {
            get 
            {
                return true;
            }
        }

        /// <summary>Gets or sets the indent level.</summary>
        /// <returns>The indent level. The default is zero.</returns>
        /// <filterpriority>2</filterpriority>
        public new int IndentLevel
        {
            set
            {
                GetContextListener(currentContext).IndentLevel = value;
            }
            get
            {
                return GetContextListener(currentContext).IndentLevel;
            }
            
        }

        /// <summary>Gets or sets the number of spaces in an indent.</summary>
        /// <returns>The number of spaces in an indent. The default is four spaces.</returns>
        /// <filterpriority>2</filterpriority>
        public new int IndentSize
        {
            set
            {
                GetContextListener(currentContext).IndentSize = value;
            }
            get
            {
                return GetContextListener(currentContext).IndentSize;
            }
        }   

        /// <summary>
        /// Initializes the context for the current thread.
        /// </summary>
        /// <param name="contextId">The context Id string.</param>
        /// <param name="contextListener">The <see cref="TraceListener"/> for the context.</param>
        public void InitializeContext(string contextId, TraceListener contextListener)
        {
            listenerSyncLock.AcquireWriterLock(Timeout.Infinite);

            try
            {
                listenerDictionary.Add(contextId, contextListener);
            }
            finally
            {
                listenerSyncLock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Checks if the specified context has been initialized.
        /// </summary>
        /// <param name="contextId">The context Id string.</param>
        /// <returns>True if the context has been initialized, otherwise false.</returns>
        public bool IsContextInitialized(string contextId)
        {
            if (GetContextListener(contextId) != defaultListener)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns or sets the context for the current thread.
        /// </summary>
        public string Context
        {
            set
            {
                currentContext = value;
            }
            get
            {
                return currentContext;
            }
        }

        private TraceListener GetContextListener(string contextId)
        {
            if (contextId == null)
                return defaultListener;

            listenerSyncLock.AcquireReaderLock(Timeout.Infinite);

            try
            {
                if (listenerDictionary.ContainsKey(contextId))
                    return listenerDictionary[contextId];
                else
                    return defaultListener;
            }
            finally
            {
                listenerSyncLock.ReleaseReaderLock();
            }
     
        }

        /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Diagnostics.TraceListener"></see> and optionally releases the managed resources.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (string key in listenerDictionary.Keys)
                {
                    listenerDictionary[key].Dispose();
                }
                
                base.Dispose(disposing);
            }
            
        }

        /// <summary>
        /// Returns the default trace listener.
        /// </summary>
        public TraceListener DefaultListener
        {
            get
            {
                return defaultListener;
            }
            set
            {
                defaultListener = value;
            }
        }

        /// <summary>
        /// Resets the context for the current thread.
        /// </summary>
        public void ResetContext()
        {
            currentContext = null;
        }
    }
}
