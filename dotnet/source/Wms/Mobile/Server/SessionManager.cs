using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Imi.Framework.Messaging.Engine;

namespace Imi.Wms.Mobile.Server
{
    public class SessionManager
    {
        private class SessionManagerInstance
        {
            static SessionManagerInstance()
            {
                
            }

            internal static readonly SessionManager instance = new SessionManager();
        }

        private Dictionary<string, ClientSession> _sessionDictionary;
        private ReaderWriterLock syncLock;

        private SessionManager()
        {
            _sessionDictionary = new Dictionary<string, ClientSession>();
            syncLock = new ReaderWriterLock();
        }

        /// <summary>
        /// Returns the session given its session Id.
        /// </summary>
        /// <param name="sessionId">The session Id string.</param>
        /// <returns>The corresponding session object.</returns>
        public ClientSession this[string sessionId]
        {
            get
            {
                syncLock.AcquireReaderLock(Timeout.Infinite);
                
                ClientSession session = null;

                try
                {
                    _sessionDictionary.TryGetValue(sessionId, out session);
                }
                finally
                {
                    syncLock.ReleaseReaderLock();
                }

                return session;

            }
        }

        public IEnumerable<ClientSession> GetSessions()
        {
            return _sessionDictionary.Values.ToArray();
        }

        /// <summary>
        /// Returns the current <see cref="SessionManager"/> instance.
        /// </summary>
        public static SessionManager Instance
        {
            get
            {
                return SessionManagerInstance.instance;
            }
        }

        /// <summary>
        /// Creates a new session with the specified session id.
        /// </summary>
        /// <param name="sessionId">The Id string for the session.</param>
        /// <returns>A reference to the <see cref="ClientSession"/> object.</returns>
        public ClientSession CreateSession(string sessionId)
        {
            syncLock.AcquireWriterLock(Timeout.Infinite);

            try
            {
                if (!_sessionDictionary.ContainsKey(sessionId))
                    _sessionDictionary.Add(sessionId, new ClientSession(sessionId));
            }
            finally
            {
                syncLock.ReleaseWriterLock();
            }
                        
            return _sessionDictionary[sessionId];
        }

        /// <summary>
        /// Removes a session object.
        /// </summary>
        /// <param name="sessionId">The Id string of the session to remove.</param>
        public void DestroySession(string sessionId)
        {
            syncLock.AcquireWriterLock(Timeout.Infinite);
            
            try
            {
                _sessionDictionary.Remove(sessionId);
            }
            finally
            {
                syncLock.ReleaseWriterLock();
            }
        }
    }
}
