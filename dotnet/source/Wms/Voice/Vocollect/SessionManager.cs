using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Imi.Framework.Messaging.Engine;

namespace Imi.Wms.Voice.Vocollect
{
    /// <summary>
    /// Manages voice terminal sessions.
    /// </summary>
    public sealed class SessionManager
    {
        private class SessionManagerInstance
        {
            static SessionManagerInstance()
            {
                
            }

            internal static readonly SessionManager instance = new SessionManager();
        }

        private Dictionary<string, VocollectSession> sessionDictionary;
        private ReaderWriterLock syncLock;

        private SessionManager()
        {
            sessionDictionary = new Dictionary<string, VocollectSession>();
            syncLock = new ReaderWriterLock();
        }

        /// <summary>
        /// Returns the session given its session Id.
        /// </summary>
        /// <param name="sessionId">The session Id string.</param>
        /// <returns>The corresponding session object.</returns>
        public VocollectSession this[string sessionId]
        {
            get
            {
                syncLock.AcquireReaderLock(Timeout.Infinite);
                
                VocollectSession session = null;

                try
                {
                    sessionDictionary.TryGetValue(sessionId, out session);
                }
                finally
                {
                    syncLock.ReleaseReaderLock();
                }

                return session;

            }
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
        /// <returns>A reference to the <see cref="VocollectSession"/> object.</returns>
        public VocollectSession CreateSession(string sessionId)
        {
            syncLock.AcquireWriterLock(Timeout.Infinite);

            try
            {
                if (!sessionDictionary.ContainsKey(sessionId))
                    sessionDictionary.Add(sessionId, new VocollectSession());
            }
            finally
            {
                syncLock.ReleaseWriterLock();
            }
                        
            return sessionDictionary[sessionId];
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
                sessionDictionary.Remove(sessionId);
            }
            finally
            {
                syncLock.ReleaseWriterLock();
            }
        }
    }
}
