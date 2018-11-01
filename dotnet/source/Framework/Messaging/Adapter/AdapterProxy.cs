using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Imi.Framework.Messaging.Engine;

namespace Imi.Framework.Messaging.Adapter
{
    /// <summary>
    /// Proxy class which manages adapters and endpoints.
    /// </summary>
    public class AdapterProxy
    {
        private Dictionary<Uri, AdapterEndPoint> endPointDictionary;
        private Dictionary<string, List<AdapterBase>> protocolDictionary;
        private Dictionary<string, AdapterBase> adapterDictionary;
        private ReaderWriterLock endPointSyncLock;

        internal AdapterProxy()
        {
            endPointSyncLock = new ReaderWriterLock();
            endPointDictionary = new Dictionary<Uri, AdapterEndPoint>();
            protocolDictionary = new Dictionary<string, List<AdapterBase>>();
            adapterDictionary = new Dictionary<string, AdapterBase>();
        }

        /// <summary>
        /// Registers an adapter to be managed by the proxy.
        /// </summary>
        /// <param name="adapter">The adapter to register.</param>
        public void RegisterAdapter(AdapterBase adapter)
        {
            if (!protocolDictionary.ContainsKey(adapter.ProtocolType))
            {
                protocolDictionary.Add(adapter.ProtocolType, new List<AdapterBase>());
            }
            else
                throw new InvalidOperationException("Adapter already registered.");

            List<AdapterBase> adapterCollection = protocolDictionary[adapter.ProtocolType];

            adapterCollection.Add(adapter);
            adapter.EndPointCreated += new EventHandler<AdapterEndPointEventArgs>(EndPointCreatedEventHandler);
            adapter.EndPointDestroyed += new EventHandler<AdapterEndPointEventArgs>(EndPointDestroyedEventHandler);

            adapterDictionary.Add(adapter.AdapterId, adapter);
        }

        private void EndPointDestroyedEventHandler(object sender, AdapterEndPointEventArgs e)
        {
            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "EndPoint destroyed: \"{0}\".", e.EndPoint.Uri);

            endPointSyncLock.AcquireWriterLock(MessageEngine.Instance.LockMillisecondsTimeout);
            
            try
            {
                endPointDictionary.Remove(e.EndPoint.Uri);
            }
            finally
            {
                endPointSyncLock.ReleaseWriterLock();
            }
       }

        private void EndPointCreatedEventHandler(object sender, AdapterEndPointEventArgs e)
        {
            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "EndPoint created: \"{0}\".", e.EndPoint.Uri);

            endPointSyncLock.AcquireWriterLock(MessageEngine.Instance.LockMillisecondsTimeout);

            try
            {
                endPointDictionary.Add(e.EndPoint.Uri, e.EndPoint);
            }
            finally
            {
                endPointSyncLock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Returns an adapter given its id.
        /// </summary>
        /// <param name="adapterId">The id string of the adapter.</param>
        /// <returns></returns>
        public AdapterBase this[string adapterId]
        {
            get
            {
                return GetAdapterById(adapterId);
            }
        }
        
        /// <summary>
        /// Returns an adapter given its id.
        /// </summary>
        /// <param name="adapterId">The id string of the adapter.</param>
        /// <returns></returns>
        public AdapterBase GetAdapterById(string adapterId)
        {
            return adapterDictionary[adapterId];
        }

        /// <summary>
        /// Resolves the specified URI to an adapter endpoint reference.
        /// </summary>
        /// <param name="uri">The URI to resolve.</param>
        /// <returns>An endpoint corresponding to the URI, otherwise null.</returns>
        public AdapterEndPoint ResolveUriToEndPoint(Uri uri)
        {
            AdapterEndPoint endPoint;

            endPointSyncLock.AcquireReaderLock(MessageEngine.Instance.LockMillisecondsTimeout);

            try
            {
                endPointDictionary.TryGetValue(uri, out endPoint);
            }
            finally
            {
                endPointSyncLock.ReleaseReaderLock();
            }

            return endPoint;
        }

        /// <summary>
        /// Resolves the specified URI to an adapter reference.
        /// </summary>
        /// <param name="uri">The URI to resolve.</param>
        /// <returns>An adapter compatible with the URI's scheme name, otherwise null.</returns>
        public AdapterBase ResolveUriToAdapter(Uri uri)
        {
            AdapterBase adapter;
            
            adapterDictionary.TryGetValue(uri.Scheme, out adapter);

            return adapter;
        }
    }
}
