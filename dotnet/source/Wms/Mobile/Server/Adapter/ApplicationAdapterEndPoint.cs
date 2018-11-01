using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.ComponentModel;
using Imi.Framework.Messaging.Adapter;
using Imi.Wms.Mobile.Server.Adapter;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Configuration;

namespace Imi.Wms.Mobile.Server
{
    public class ApplicationAdapterEndPoint : AdapterEndPoint, IDisposable
    {
        public event EventHandler<AdapterReceiveEventArgs> MessageReceived;
        public event UnhandledExceptionEventHandler UnhandledException;

        private Mutex _mutex;
        private MemoryMappedFile _memoryMappedFile;
        private MemoryMappedViewStream _stream;
        private EventWaitHandle _serverNotifyEvent;
        private EventWaitHandle _clientNotifyEvent;
        private EventWaitHandle _serverReceivedNotifyEvent;
        private EventWaitHandle _clientReceivedNotifyEvent;
        private EventWaitHandle _clientProcessedEvent;
        private Process _process;
        private Thread _receiveThread;
        private bool _isDisposed;
        private string _sessionId;
        private string _applicationName;
        private MessagingSection _config;
        private const int MemoryMappedFileSize = 65536;
        private Exception _exception;
        private bool _abort;
        private EventWaitHandle _abortEvent;
                        
        public ApplicationAdapterEndPoint(AdapterBase adapter, string applicationName, string sessionId)
            : base(adapter, new UriBuilder("app", sessionId).Uri)
        {
            _config = ConfigurationManager.GetSection(MessagingSection.SectionKey) as MessagingSection;
            _sessionId = sessionId;
            _applicationName = applicationName;
            _mutex = new Mutex(false, string.Format("IMI_MOBILE_MUTEX_{0}", _sessionId));
            _memoryMappedFile = MemoryMappedFile.CreateOrOpen(string.Format("IMI_MOBILE_QUEUE_{0}", _sessionId), MemoryMappedFileSize);
            _stream = _memoryMappedFile.CreateViewStream(0, 0, MemoryMappedFileAccess.ReadWrite);
            _serverNotifyEvent = new EventWaitHandle(false, EventResetMode.AutoReset, string.Format("IMI_MOBILE_SERVER_NOTIFY_{0}", _sessionId));
            _serverReceivedNotifyEvent = new EventWaitHandle(false, EventResetMode.AutoReset, string.Format("IMI_MOBILE_SERVER_RECEIVED_NOTIFY_{0}", _sessionId));
            _clientNotifyEvent = new EventWaitHandle(false, EventResetMode.AutoReset, string.Format("IMI_MOBILE_CLIENT_NOTIFY_{0}", _sessionId));
            _clientReceivedNotifyEvent = new EventWaitHandle(false, EventResetMode.AutoReset, string.Format("IMI_MOBILE_CLIENT_RECEIVED_NOTIFY_{0}", _sessionId));
            _clientProcessedEvent = new EventWaitHandle(false, EventResetMode.ManualReset, string.Format("IMI_MOBILE_CLIENT_PROCESSED_NOTIFY_{0}", _sessionId));
            _abortEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        ~ApplicationAdapterEndPoint()
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
                _abort = true;

                if (disposing)
                {
                    _abortEvent.Set();
                    _abortEvent.Dispose();
                    _mutex.Dispose();
                    _memoryMappedFile.Dispose();
                    _stream.Dispose();
                    _serverNotifyEvent.Dispose();
                    _clientNotifyEvent.Dispose();
                    _serverReceivedNotifyEvent.Dispose();
                    _clientReceivedNotifyEvent.Dispose();
                    _clientProcessedEvent.Dispose();

                    if (_process != null)
                    {
                        _process.EnableRaisingEvents = false;
                        _process.Exited -= ApplicationExitedEventHandler;

                        try
                        {
                            _process.Kill();
                        }
                        catch (Win32Exception)
                        {
                        }
                        catch (NotSupportedException)
                        {
                        }
                        catch (InvalidOperationException)
                        {
                        }
                        finally
                        {
                            _process.Dispose();
                        }
                    }
                }
            }
        }

        public void StartApplication(string executablePath, string arguments, string desktopName)
        {
            _process = ProcessHelper.CreateProcess(executablePath, arguments, desktopName);
            _process.Exited += ApplicationExitedEventHandler;
            _process.EnableRaisingEvents = true;
            
            int result = WaitHandle.WaitAny(new WaitHandle[] {_serverNotifyEvent, _abortEvent}, _config.PendingOperationsTimeout);

            if (result == WaitHandle.WaitTimeout)
            {
                throw new TimeoutException("Timed out waiting for application to start.");
            }
            else if (result == 0)
            {
                _receiveThread = new Thread(ReceiveThread);
                _receiveThread.Start();
            }
        }

        private void ApplicationExitedEventHandler(object sender, EventArgs e)
        {
            ApplicationTerminatedException ex = new ApplicationTerminatedException("Application terminated.");
            ex.Data.Add("ErrorCode", _process.ExitCode);

            OnUnhandledException(ex);
        }

        private void OnUnhandledException(Exception ex)
        {
            _exception = ex;

            UnhandledExceptionEventHandler temp = UnhandledException;

            if (temp != null)
            { 
                temp(this, new UnhandledExceptionEventArgs(ex, false));
            }
        }
              
        public int? ProcessId
        {
            get
            {
                if (_process != null)
                {
                    try
                    {
                        return _process.Id;
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    catch (PlatformNotSupportedException)
                    {
                    }
                }

                return null;
            }
        }
               
        public Exception Exception
        {
            get
            {
                return _exception;
            }
        }
        
        public string ApplicationName 
        {
            get
            {
                return _applicationName;
            }
        }

        public void TransmitMessage(MultiPartMessage msg)
        {
            try
            {
                int result = WaitHandle.WaitAny(new WaitHandle[] { _mutex, _abortEvent }, _config.PendingOperationsTimeout);

                if (result == WaitHandle.WaitTimeout)
                {
                    throw new TimeoutException("Timed out waiting to acquire shared memory.");
                }
                else if (result == 0)
                {
                    try
                    {
                        _clientProcessedEvent.Reset();

                        var buffer = new byte[MemoryMappedFileSize];

                        byte[] eof = Encoding.Unicode.GetBytes("\u0000");

                        msg.Data.Seek(0, SeekOrigin.End);
                        msg.Data.Write(eof, 0, 2);
                        msg.Data.Seek(0, SeekOrigin.Begin);

                        while (msg.Data.Position < msg.Data.Length)
                        {
                            int numberOfBytes = msg.Data.Read(buffer, 0, buffer.Length);

                            _stream.Seek(0, SeekOrigin.Begin);
                            _stream.Write(buffer, 0, numberOfBytes);
                            _stream.Flush();

                            _clientNotifyEvent.Set();

                            result = WaitHandle.WaitAny(new WaitHandle[] { _clientReceivedNotifyEvent, _abortEvent }, _config.PendingOperationsTimeout);

                            if (result == WaitHandle.WaitTimeout)
                            {
                                throw new TimeoutException("Timed out waiting for application to acknowledge the message.");
                            }
                            else if (result == 1)
                            {
                                return;
                            }
                        }
                    }
                    finally
                    {
                        _mutex.ReleaseMutex();
                    }
                }

                result = WaitHandle.WaitAny(new WaitHandle[] { _clientProcessedEvent, _abortEvent }, _config.PendingOperationsTimeout);

                if (result == WaitHandle.WaitTimeout)
                {
                    if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Warning) == SourceLevels.Warning)
                    {
                        MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Warning, 0, "The application \"{0}\" did not process the message within the given timeframe. This could be due to a long running operation or the application being unresponsive.", ToString());
                    }
                }
            }
            catch (ObjectDisposedException)
            { 
            }
        }

        private void ReceiveThread(object state)
        {
            while (MessageEngine.Instance.IsRunning && !_abort)
            {
                try
                {
                    int result = WaitHandle.WaitAny(new WaitHandle[] { _serverNotifyEvent, _abortEvent }, _config.PendingOperationsTimeout);

                    if (result == 0)
                    {
                        MemoryStream messageData = new MemoryStream(MemoryMappedFileSize);
                        
                        bool endOfFile = false;

                        while (!endOfFile)
                        {
                            int length = MemoryMappedFileSize;

                            try
                            {
                                _stream.Seek(0, SeekOrigin.Begin);
                                byte[] codepoint = new byte[2];

                                while (_stream.Position < MemoryMappedFileSize - 1)
                                {
                                    _stream.Read(codepoint, 0, 2);

                                    if (codepoint[0] == 0 &&
                                        codepoint[1] == 0)
                                    {
                                        length = (int)_stream.Position - 2;
                                        endOfFile = true;
                                        break;
                                    }
                                }

                                byte[] buffer = new byte[length];
                                _stream.Seek(0, SeekOrigin.Begin);
                                _stream.Read(buffer, 0, length);

                                messageData.Write(buffer, 0, length);
                            }
                            finally
                            {
                                _serverReceivedNotifyEvent.Set();
                            }

                            if (!endOfFile)
                            {
                                result = WaitHandle.WaitAny(new WaitHandle[] { _serverNotifyEvent, _abortEvent }, _config.PendingOperationsTimeout);

                                if (result == WaitHandle.WaitTimeout)
                                {
                                    throw new TimeoutException("Timed out waiting to receive the message.");
                                }
                                else if (result == 1)
                                {
                                    return;
                                }
                            }
                        }

                        using (MultiPartMessage msg = new MultiPartMessage("http://www.im.se/wms/mobile/Application", messageData))
                        {
                            EventHandler<AdapterReceiveEventArgs> temp = MessageReceived;

                            if (temp != null)
                            {
                                temp(this, new AdapterReceiveEventArgs(msg, this));
                            }
                        }
                    }
                }
                catch (ObjectDisposedException)
                { 
                }
                catch (Exception ex)
                {
                    OnUnhandledException(ex);
                }
            }
        }
     }
}
