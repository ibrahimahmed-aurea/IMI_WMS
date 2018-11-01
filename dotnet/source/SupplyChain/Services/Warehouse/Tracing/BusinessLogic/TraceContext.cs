using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using System.Threading;

namespace Imi.SupplyChain.Warehouse.Tracing.BusinessLogic
{
    public class TraceContext
    {
        private static Dictionary<string, TraceContext> contextDictionary;
        private static ReaderWriterLock syncLock;

        static TraceContext()
        { 
            contextDictionary = new Dictionary<string, TraceContext>();
            syncLock = new ReaderWriterLock();
        }

        private TraceContext()
        { 
        }

        public static TraceContext CreateContext(string userId, string terminalId, string sessionId)
        {
            string key = string.Format("{0}_{1}", userId, terminalId);

            TraceContext context = new TraceContext();
            context.TerminalId = terminalId;
            context.UserId = userId;
            context.SessionId = sessionId;

            syncLock.AcquireWriterLock(-1);
            
            try
            {
                contextDictionary[key] = context;
            }
            finally
            {
                syncLock.ReleaseWriterLock();
            }

            return context;
        }

        public static TraceContext GetContext(string userId, string terminalId, string sessionId)
        {
            TraceContext context = null;
            string key = string.Format("{0}_{1}", userId, terminalId);

            syncLock.AcquireReaderLock(-1);

            try
            {
                contextDictionary.TryGetValue(key, out context);

                if ((context != null) && (context.SessionId != sessionId))
                {
                    context = null;
                }
            }
            finally
            {
                syncLock.ReleaseReaderLock();
            }

            return context;
        }

        public string UserId { get; private set; }
        public string TerminalId { get; private set; }
        public bool IsTracingEnabled { get; set; }
        public string SessionId { get; private set; }
    }
}
