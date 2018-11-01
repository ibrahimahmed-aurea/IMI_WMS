using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using Imi.Framework.Messaging.Engine;
using Imi.Wms.Mobile.Server.Interface;
using System.Diagnostics;

namespace Imi.Wms.Mobile.Server
{
    /// <summary>
    /// Represents a voice terminal session.
    /// </summary>
    public class ClientSession : IDisposable
    {
        private MultiPartMessage _lastRequestMessage;
        private MultiPartMessage _lastResponseMessage;
        private EventWaitHandle _stateChangedEvent;
        private EventWaitHandle _abortEvent;
        private StateResponse _stateResponse;
        private object _syncLock;
        private ApplicationAdapterEndPoint _applicationEndPoint;
        private string _id;
        private string _clientIP;
        private DateTime _lastActivityTime;
        private SourceLevels _clientTraceLevel;
        private bool _isDisposed;
                                       	                              	
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="ClientSession"/> class.</para>
        /// </summary>
        public ClientSession(string id)
        {
            _stateChangedEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
            _abortEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
            _syncLock = new object();
            _id = id;
        }

        ~ClientSession()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _abortEvent.Set();
                    _stateChangedEvent.Dispose();
                    _abortEvent.Dispose();

                    if (_lastRequestMessage != null)
                    {
                        _lastRequestMessage.Dispose();
                    }

                    if (_lastResponseMessage != null)
                    {
                        _lastResponseMessage.Dispose();
                    }

                    if (_applicationEndPoint != null)
                    {
                        _applicationEndPoint.Dispose();
                    }
                }       
            }
        }

        public DateTime? KillTime { get; private set; }

        public void Kill()
        {
            AbortEvent.Set();

            if (KillTime == null)
            {
                KillTime = DateTime.Now.AddSeconds(20);
            }
        }

        public SourceLevels ClientTraceLevel
        {
            get
            {
                return _clientTraceLevel;
            }
            set
            {
                _clientTraceLevel = value;
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public string ClientIP
        {
            get
            {
                return _clientIP;
            }
            set
            {
                _clientIP = value;
            }
        }

        public string UserId
        {
            get
            {
                if (StateResponse != null)
                {
                    return StateResponse.UserId;
                }
                else
                {
                    return null;
                }
            }
        }

        public object SyncLock
        {
            get
            {
                return _syncLock;
            }
        }

        public StateResponse StateResponse
        {
            get
            {
                return _stateResponse;
            }
            set
            {
                _stateResponse = value;
            }
        }

        public DateTime LastActivityTime
        {
            get
            {
                return _lastActivityTime;
            }
            set
            {
                _lastActivityTime = value;
            }
        }
                
        public MultiPartMessage LastRequestMessage
        {
            get
            {
                return _lastRequestMessage;
            }
            set
            {
                _lastRequestMessage = value;
            }
        }

        public EventWaitHandle StateChangedEvent
        {
            get
            {
                return _stateChangedEvent;
            }
        }

        public EventWaitHandle AbortEvent
        {
            get
            {
                return _abortEvent;
            }
        }

        public MultiPartMessage LastResponseMessage
        {
            get
            {
                return _lastResponseMessage;
            }
            set
            {
                _lastResponseMessage = value;
            }
        }

        public ApplicationAdapterEndPoint ApplicationEndPoint 
        {
            get
            {
                return _applicationEndPoint;
            }
            set
            {
                _applicationEndPoint = value;
            }
        }

        public string TerminalId { get; set; }

        public string ClientVersion { get; set; }

        public string ClientPlatform { get; set; }
    }
}
