using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using Imi.Framework.Messaging.Adapter;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Configuration;
using Imi.Framework.Shared;

namespace Imi.Framework.Messaging.Adapter.Net.Sockets
{
    /// <summary>
    /// Asynchronous TCP adapter.
    /// </summary>
    public class TcpAdapter : AdapterBase
    {
        private Socket _serverSocket;
        private AutoResetEvent _acceptWaitEvent;
        private AutoResetEvent _acceptThreadWaitEvent;
        private AutoResetEvent _timeoutWaitEvent;
        private AutoResetEvent _timeoutThreadWaitEvent;
        private DateTime _currentTimeout;
        private Thread _timeoutThread;
        private Thread _acceptThread;
        private object _syncObject;
        private bool _abort;
                                
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="TcpAdapter"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// Configuration properties.
        /// </param>
        /// <param name="id">
        /// The Id string of the adpater.
        /// </param>
        public TcpAdapter(PropertyCollection configuration, string id) : base(configuration, id)
        {
            _acceptWaitEvent = new AutoResetEvent(false);
            _acceptThreadWaitEvent = new AutoResetEvent(false);
            _timeoutWaitEvent = new AutoResetEvent(false);
            _timeoutThreadWaitEvent = new AutoResetEvent(false);
            _syncObject = new object();
        }

        /// <summary>
        /// Returns the protocol used by this adapter.
        /// </summary>
        public override string ProtocolType
        {
            get
            {
                return "tcp";
            }
        }

        /// <summary>
        /// Transmits a message over the adapter protocol.
        /// </summary>
        /// <param name="msg">The message to transmit.</param>
        public override void TransmitMessage(MultiPartMessage msg)
        {
            Uri sendUri = (Uri)msg.Metadata.Read("SendUri");

            TcpAdapterEndPoint endPoint 
                = (TcpAdapterEndPoint)MessageEngine.Instance.AdapterProxy.ResolveUriToEndPoint(sendUri);

            if (endPoint == null)
                throw new AdapterException("Failed to transmit message to Uri: \"" + sendUri + "\". The EndPoint does not exist.");
            
            try
            {
                byte[] buffer = new byte[msg.Data.Length];
                msg.Data.Seek(0, SeekOrigin.Begin);
                msg.Data.Read(buffer, 0, (int)msg.Data.Length);

                endPoint.Socket.Send(buffer, 0, (int)msg.Data.Length, SocketFlags.None);
            }
            catch (NullReferenceException ex)
            {
                throw new AdapterException("Failed to transmit message to EndPoint: \"" + endPoint + "\".", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new AdapterException("Failed to transmit message to EndPoint: \"" + endPoint + "\".", ex);
            }
            catch (SocketException ex)
            {
                Disconnect(endPoint, ex);
                throw new AdapterException("Failed to transmit message to EndPoint: \"" + endPoint + "\".", ex);
            }
            catch (ObjectDisposedException ex)
            {
                //The socket has been closed by another thread
                Disconnect(endPoint, ex);
                throw new AdapterException("Failed to transmit message to EndPoint: \"" + endPoint + "\".", ex);
            }
        }
                
        /// <summary>
        /// Initializes the adapter for sending and receiving of data.
        /// </summary>
        protected internal override void Initialize()
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

            int port = Configuration.ReadAsInt("TcpAdapterPort");

            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            _serverSocket.Listen(100);

            ThreadStart timeoutThreadDelegate = new ThreadStart(TimeoutThreadImpl);
            _timeoutThread = new Thread(timeoutThreadDelegate);
            _timeoutThread.Start();

            _timeoutThreadWaitEvent.WaitOne();

            ThreadStart acceptThreadDelegate = new ThreadStart(AcceptThreadImpl);
            _acceptThread = new Thread(acceptThreadDelegate);
            _acceptThread.Start();

            _acceptThreadWaitEvent.WaitOne();
        }

        private void TimeoutThreadImpl()
        {
            _timeoutThreadWaitEvent.Set();

            DateTime timeoutTime;
            TimeSpan timeSpan;
            TimeSpan temp;
            
            while (MessageEngine.Instance.IsRunning && !_abort)
            {
                timeSpan = TimeSpan.MaxValue;
                timeoutTime = DateTime.MaxValue;

                foreach (TcpAdapterEndPoint endPoint in GetEndPoints())
                {
                    DateTime timeout;

                    lock (endPoint)
                    {
                        timeout = endPoint.Timeout;
                    }

                    //Check if not already timed out
                    if (timeout != DateTime.MaxValue)
                    {
                        if (DateTime.Now >= timeout)
                        {
                            OnEndpointTimeout(endPoint);
                        }
                        else
                        {
                            temp = timeout - DateTime.Now;

                            if (temp > TimeSpan.Zero)
                            {
                                if (temp < timeSpan)
                                {
                                    timeSpan = temp;
                                    timeoutTime = timeout;
                                }
                            }
                            else
                                OnEndpointTimeout(endPoint);
                        }
                    }
                }

                lock (_syncObject)
                {
                    _currentTimeout = timeoutTime;
                }

                if (timeSpan == TimeSpan.MaxValue)
                    timeSpan = new TimeSpan(0, 0, 0, 0, -1);
                                    
                if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                    MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "EndPoint timeout thread is entering sleep mode ({0})...", timeSpan);

                try
                {
                    _timeoutWaitEvent.WaitOne(timeSpan, false);
                }
                catch (ObjectDisposedException)
                { 
                }
            }
        }

        private void OnEndpointTimeout(TcpAdapterEndPoint endPoint)
        {
            //Mark as timed out
            lock (endPoint)
            {
                endPoint.Timeout = DateTime.MaxValue;
            }

            Disconnect(endPoint);

            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Warning) == SourceLevels.Warning)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Warning, 0, "EndPoint: \"{0}\" timed out due to inactivity.", endPoint);
        }

        public void SetEndPointTimeout(TcpAdapterEndPoint endPoint, DateTime timeout)
        {
            lock (endPoint)
            {
                endPoint.Timeout = timeout;
            }

            lock (_syncObject)
            { 
                if (timeout < _currentTimeout)
                    _timeoutWaitEvent.Set();
            }
        }
        
        private void AcceptThreadImpl()
        {
            _acceptThreadWaitEvent.Set();

            while (MessageEngine.Instance.IsRunning && !_abort)
            {
                try
                {
                    _serverSocket.BeginAccept(AcceptCallback, _serverSocket);
                    _acceptWaitEvent.WaitOne();
                }
                catch (ObjectDisposedException)
                {
                    //Server socket was closed, abort
                    break;
                }
            }
        }
       
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket clientSocket = null;

            try
            {
                try
                {
                    clientSocket = _serverSocket.EndAccept(ar);
                }
                finally
                {
                    _acceptWaitEvent.Set();
                }

                CreateEndPoint(clientSocket);
            }
            catch (Exception ex)
            {
                if (clientSocket != null)
                {
                    clientSocket.Dispose();
                }
                
                HandleAsyncException(ex);

                if (ExceptionHelper.IsCritical(ex))
                {
                    throw;
                }
            }
        }

        private TcpAdapterEndPoint CreateEndPoint(Socket socket)
        {
            TcpAdapterEndPoint endPoint = new TcpAdapterEndPoint(this, socket);
            
            try
            {
                Uri uri = new UriBuilder("tcp", ((IPEndPoint)socket.RemoteEndPoint).Address.ToString(),
                        ((IPEndPoint)socket.RemoteEndPoint).Port).Uri;

                TcpAdapterEndPoint existingEndPoint = MessageEngine.Instance.AdapterProxy.ResolveUriToEndPoint(uri) as TcpAdapterEndPoint;

                if (existingEndPoint != null)
                {
                    Disconnect(existingEndPoint);
                }

                OnEndPointCreated(endPoint);

                InitializeEndPoint(endPoint);
            }
            catch (Exception)
            {
                Disconnect(endPoint);
                throw;
            }
            
            return endPoint;
        }

        private void InitializeEndPoint(TcpAdapterEndPoint endPoint)
        {
            int timeout = Configuration.ReadAsInt("TcpEndPointTimeout");

            if (timeout > 0)
                SetEndPointTimeout(endPoint, DateTime.Now.AddMilliseconds(timeout));
            
            endPoint.Socket.BeginReceive(endPoint.buffer, 0, TcpAdapterEndPoint.bufferSize, 0, ReceiveCallback, endPoint);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            TcpAdapterEndPoint endPoint = null;

            try
            {
                endPoint = (TcpAdapterEndPoint)ar.AsyncState;

                Socket clientSocket = endPoint.Socket;
                                
                int byteCount = clientSocket.EndReceive(ar);

                if (byteCount > 0)
                {
                    MultiPartMessage msg = new MultiPartMessage("",
                        new MemoryStream(endPoint.buffer, 0, byteCount));

                    OnMessageReceived(msg, endPoint);

                    InitializeEndPoint(endPoint);
                }
                else
                {
                    Disconnect(endPoint);
                }
            }
            catch (Exception ex)
            {
                Disconnect(endPoint, ex);
            }
        }
                
        public void Disconnect(TcpAdapterEndPoint endPoint)
        {
            Disconnect(endPoint, null);
        }

        private void Disconnect(TcpAdapterEndPoint endPoint, Exception exception)
        {
            if (endPoint != null)
            {
                try
                {
                    endPoint.Socket.Close();
                }
                catch (SocketException)
                {
                }
                catch (ObjectDisposedException)
                {
                }
                finally
                {
                    endPoint.Exception = exception;
                    OnEndPointDestroyed(endPoint);
                }
            }
        }

        private TcpAdapterEndPoint Connect(string host, int port)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            clientSocket.Connect(host, port);
            
            return CreateEndPoint(clientSocket);
        }

        /// <summary>
        /// Disposes any unmanaged resources used by this adapter.
        /// </summary>
        /// <param name="disposing">True if called from user code.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                try
                {
                    if (disposing)
                    {
                        MessagingSection config = ConfigurationManager.GetSection(MessagingSection.SectionKey) as MessagingSection;

                        _abort = true;

                        if (_serverSocket != null)
                        {
                            _serverSocket.Close(); //This also terminates the accept thread
                        }

                        _acceptWaitEvent.Set();
                        _acceptWaitEvent.Close();
                        _acceptThreadWaitEvent.Close();
                        _timeoutWaitEvent.Set();
                        _timeoutWaitEvent.Close();
                        _timeoutThreadWaitEvent.Close();

                        AdapterEndPoint[] endPoints = GetEndPoints();

                        foreach (TcpAdapterEndPoint endPoint in endPoints)
                            Disconnect(endPoint);
                    }
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }

            
        }
               
                        
    }
}
