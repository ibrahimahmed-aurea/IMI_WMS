using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using System.Net;
using Imi.Wms.Mobile.Server;
using Imi.Wms.Mobile.Server.Interface;

namespace Imi.Wms.Mobile.UI
{
    public class ThinClient : IDisposable
    {
        #region State Object

        private class SocketState : IDisposable
        {
            private EventWaitHandle _syncEvent;
            private byte[] _buffer;
            private MemoryStream _messageData;
            private bool _isConnected;
            private Socket _socket;
            private Exception _exception;
            private bool _isDisposed;
            
            public SocketState()
            {
                _syncEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
                _buffer = new byte[4096];
            }

            ~SocketState()
            {
                Dispose(false);
            }

            public Socket Socket 
            {
                get
                {
                    return _socket;
                }
                set
                {
                    _socket = value;
                }
            }

            public Exception Exception
            {
                get
                {
                    return _exception;
                }
                set
                {
                    _exception = value;
                }
            }
            
            public bool IsConnected
            {
                get
                {
                    try
                    {
                        if (Socket != null && Socket.Connected && _isConnected)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        return false;
                    }
                    catch (SocketException)
                    {
                        return false;
                    }
                }
                set
                {
                    _isConnected = value;
                }
            }
                        
            public byte[] Buffer
            {
                get
                {
                    return _buffer;
                }
            }

            public MemoryStream MessageData
            {
                get
                {
                    return _messageData;
                }
                set
                {
                    _messageData = value;
                }
            }
                        
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            public void Dispose(bool disposing)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _syncEvent.Close();

                    if (_messageData != null)
                    {
                        _messageData.Close();
                    }

                    if (_socket != null)
                    {
                        try
                        {
                            _socket.Close();
                        }
                        catch (ObjectDisposedException)
                        {
                        }
                        catch (SocketException)
                        {
                        }
                    }
                }
            }

            public bool Wait(int millisecondsTimeout)
            {
                return _syncEvent.WaitOne(millisecondsTimeout, false);
            }

            public void Complete()
            {
                if (!_isDisposed)
                {
                    try
                    {
                        _syncEvent.Set();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }
            }
        }
        
        #endregion

        private string _host;
        private int _port;
        private bool _isDisposed;
        
        private const int EndOfFileLength = 4;
        private Dictionary<Type, XmlSerializer> _serializerCache;
        private string _sessionId;
        private int _sequence;
        private object _syncLock;
        private int _connectTimeout;
        private int _receiveTimeout;
        private int _sendTimeout;
        private Type _lastRequestType;
        private SocketState _socketState;

        public ThinClient(string host, int port)
        {
            _serializerCache = new Dictionary<Type, XmlSerializer>();
            _host = host;
            _port = port;
            _syncLock = new object();
            _connectTimeout = 5000;
            _sendTimeout = 5000;
            _receiveTimeout = 10000;
            _sessionId = Guid.NewGuid().ToString();
        }

        public string SessionId
        {
            get
            {
                return _sessionId;
            }
            set
            {
                if (_sessionId != value)
                {
                    _sessionId = value;
                    _sequence = 0;
                }
                
            }
        }
        
        public int ConnectTimeout
        {
            get
            {
                return _connectTimeout;
            }
            set
            {
                _connectTimeout = value;
            }
        }

        public int SendTimeout
        {
            get
            {
                return _sendTimeout;
            }
            set
            {
                _sendTimeout = value;
            }
        }

        public int ReceiveTimeout
        {
            get
            {
                return _receiveTimeout;
            }
            set
            {
                _receiveTimeout = value;
            }
        }


        ~ThinClient()
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
            _isDisposed = true;

            if (disposing)
            {
                if (_socketState != null)
                {
                    _socketState.Dispose();
                }
            }
        }
                
        public void Abort()
        {
            Logger.Write("Aborting...");

            if (_isDisposed)
            {
                throw new ObjectDisposedException(typeof(ThinClient).FullName);
            }
                                    
            if (_socketState != null)
            {
                _socketState.IsConnected = false;

                if (_socketState.Socket != null)
                {
                    try
                    {
                        _socketState.Socket.Close();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                    catch (SocketException)
                    {
                    }
                }
            }
        }
                        
        public TResponse Invoke<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            Logger.Write(string.Format("Invoking request \"{0}\"...", request.GetType().Name));

            if (_isDisposed)
            {
                throw new ObjectDisposedException(typeof(ThinClient).FullName);
            }

            TResponse response = null;

            lock (_syncLock)
            {
                try
                {
                    if (_lastRequestType != typeof(TRequest))
                    {
                        _sequence++;
                    }

                    request.GetType().GetProperty("SessionId").SetValue(request, _sessionId, null);
                    request.GetType().GetProperty("Sequence").SetValue(request, _sequence, null);
                    
                    _lastRequestType = typeof(TRequest);

                    Connect();

                    XmlSerializer serializer = GetSerializer(typeof(TRequest));

                    var stream = new MemoryStream(4096);

                    serializer.Serialize(stream, request);

                    for (int i = 0; i < EndOfFileLength; i++)
                    {
                        stream.WriteByte(0);
                    }

                    if (Logger.IsEnabled)
                    {
                        string data = Encoding.UTF8.GetString(stream.ToArray(), 0, (int)stream.Length).Replace('\0', '0');
                        Logger.Write(string.Format("Sending data \"{0}\"...", data));
                    }

                    _socketState.Socket.BeginSend(stream.ToArray(), 0, (int)stream.Length, 0, SendCallback, _socketState);

                    if (!_socketState.Wait(_sendTimeout))
                    {
                        throw new TimeoutException("Send timed out.");
                    }

                    if (_socketState.Exception != null)
                    {
                        throw _socketState.Exception;
                    }

                    Logger.Write("Receiving response...");

                    response = ReceiveResponse<TResponse>();

                    _sequence++;

                    Logger.Write("Request completed successfully.");

                    return response;
                }
                catch (Exception ex)
                {
                    Logger.Write(ex.ToString());
                                        
                    if (_socketState != null)
                    {
                        _socketState.Dispose();
                    }
                                        
                    throw;
                }
            }
        }

        private TResponse ReceiveResponse<TResponse>()
        {
            _socketState.MessageData = new MemoryStream(4096);

            try
            {
                _socketState.Socket.BeginReceive(_socketState.Buffer, 0, _socketState.Buffer.Length, 0, ReceiveCallback, _socketState);

                if (!_socketState.Wait(_receiveTimeout))
                {
                    _socketState.Exception = new TimeoutException("Receive timed out.");
                }

                if (Logger.IsEnabled)
                {
                    string data = Encoding.UTF8.GetString(_socketState.MessageData.ToArray(), 0, (int)_socketState.MessageData.Length).Replace('\0', '0');
                    Logger.Write(string.Format("Data received \"{0}\"", data));
                }

                if (_socketState.Exception != null)
                {
                    throw _socketState.Exception;
                }

                CheckAndThrowException(_socketState.MessageData);

                _socketState.MessageData.Seek(0, SeekOrigin.Begin);
                XmlSerializer serializer = GetSerializer(typeof(TResponse));

                return (TResponse)serializer.Deserialize(_socketState.MessageData);
            }
            finally
            {
                _socketState.MessageData.Close();
            }
        }

        private void CheckAndThrowException(MemoryStream messageData)
        {
            byte[] temp = new byte[64];

            messageData.Seek(0, SeekOrigin.Begin);
            messageData.Read(temp, 0, 64);

            if (Encoding.UTF8.GetString(temp, 0, 64).Contains("<ServerFault"))
            {
                messageData.Seek(0, SeekOrigin.Begin);

                XmlSerializer serializer = GetSerializer(typeof(ServerFault));
                ServerFault serverFault = (ServerFault)serializer.Deserialize(messageData);
                
                throw new ServerFaultException(serverFault.Message, serverFault.Type, serverFault.ErrorCode, serverFault.ServerStackTrace);
            }
        }
                
        private void Connect()
        {
            if (_socketState != null)
            {
                if (_socketState.IsConnected)
                {
                    return;
                }

                _socketState.Dispose();
            }
                                                       
            Logger.Write("Connecting to server...");

            Logger.Write(string.Format("Resolving host \"{0}\"...", _host));

            IPAddress address = null;

            try
            {
                address = IPAddress.Parse(_host);
            }
            catch (ArgumentException)
            {
            }
            catch (FormatException)
            { 
            }

            if (address == null)
            {
                IPHostEntry dnsEntry = Dns.GetHostEntry(_host);
                 
                foreach (IPAddress ia in dnsEntry.AddressList)
                {
                    if (ia.AddressFamily == AddressFamily.InterNetwork)
                    {
                        address = ia;
                        break;
                    }
                }
            }

            IPEndPoint endPoint = new IPEndPoint(address, _port);

            Logger.Write("Creating socket...");
            
            _socketState = new SocketState();
            _socketState.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            
            Logger.Write(string.Format("Connecting to \"{0}:{1}\"...", address.ToString(), _port));

            _socketState.Socket.BeginConnect(endPoint, ConnectCallback, _socketState);

            if (!_socketState.Wait(_connectTimeout))
            {
                throw new TimeoutException("Connection timed out.");
            }

            if (_socketState.Exception != null)
            {
                throw _socketState.Exception;
            }

            _socketState.IsConnected = true;
                        
            Logger.Write("Connection successful.");
        }

        private void SendCallback(IAsyncResult ar)
        {
            SocketState state = null;

            try
            {
                state = (SocketState)ar.AsyncState;
                state.Socket.EndSend(ar);
            }
            catch (Exception ex)
            {
                if (state != null)
                {
                    state.Exception = ex;
                }
            }
            finally
            {
                if (state != null)
                {
                    state.Complete();
                }
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            SocketState state = null;

            try
            {
                state = (SocketState)ar.AsyncState;
                state.Socket.EndConnect(ar);
            }
            catch (Exception ex)
            {
                if (state != null)
                {
                    state.Exception = ex;
                }
            }
            finally
            {
                if (state != null)
                {
                    state.Complete();
                }
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            SocketState state = null;

            try
            {
                state = (SocketState)ar.AsyncState;

                int byteCount = state.Socket.EndReceive(ar);

                if (byteCount > 0)
                {
                    state.MessageData.Seek(0, SeekOrigin.End);
                    state.MessageData.Write(state.Buffer, 0, byteCount);
                    state.MessageData.Seek(0, SeekOrigin.Begin);

                    int b = 0;
                    int count = 0;

                    while ((b = state.MessageData.ReadByte()) > -1)
                    {
                        if (b == 0)
                        {
                            count++;
                        }
                        else
                        {
                            count = 0;
                        }

                        if (count == EndOfFileLength)
                        {
                            state.Complete();
                            return;
                        }
                    }

                    state.Socket.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, ReceiveCallback, state);
                }
            }
            catch (Exception ex)
            {
                if (state != null)
                {
                    state.Exception = ex;
                    state.Complete();
                }
            }
        }

        private XmlSerializer GetSerializer(Type type)
        {
            if (!_serializerCache.ContainsKey(type))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                _serializerCache.Add(type, serializer);
            }
                            
            return _serializerCache[type];
        }
    }
}
