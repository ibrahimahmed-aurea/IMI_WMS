using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Imi.Wms.Mobile.UI.Configuration;
using Imi.Wms.Mobile.UI.Shared;
using Imi.Wms.Mobile.Server.Interface;
using Imi.Wms.Mobile.UI;

namespace Imi.Wms.Mobile.UI
{
    public class RenderPresenter
    {
        private RenderForm _form;
        private Thread _stateRequestThread;
        private EventWaitHandle _stateRequestEvent;
        private string _hashCode;
        private bool _abort;
        private ThinClient _client;
        private UISection _config;
        private INativeDriver _nativeDriver;

        public RenderPresenter(RenderForm form)
        {
            _form = form;
            _config = ConfigurationManager.LoadConfiguration();
            _form.StateChanged += ClientStateChangedEventHandler;
#if PocketPC
            _form.Closed += (s, e) =>
            {
                Abort();
            };
#else
            _form.FormClosed += (s, e) =>
            {
                Abort();
            };
#endif
        }

        public UISection Config
        {
            get { return _config; }
        }

        private void Abort()
        {
            _abort = true;
            Client.Abort();
            _stateRequestEvent.Set();
        }

        public void StartRender()
        {
            _stateRequestThread = new Thread(new ThreadStart(StateRequestThread));
            _stateRequestEvent = new EventWaitHandle(true, EventResetMode.ManualReset);
            _stateRequestThread.Start();
        }

        public ThinClient Client 
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
            }
        }

        public INativeDriver NativeDriver 
        {
            get
            {
                return _nativeDriver;
            }
            set
            {
                _nativeDriver = value;

                if (_nativeDriver != null)
                {
                    _nativeDriver.Form = _form;
                }
            }
        }
                
        private void ClientStateChangedEventHandler(object sender, StateChangedEventArgs e)
        {
            EventRequest request = new EventRequest();
            request.Events = e.Events.ToArray();
            request.HashCode = _hashCode;

            EscapeControlChars(request);

            _stateRequestEvent.Reset();
            Client.Abort();
            
            int callCount = 0;

            while (true)
            {
                DateTime startTime = DateTime.Now;

                try
                {
                    callCount++;
                    Client.Invoke<EventRequest, EventResponse>(request);
                    _stateRequestEvent.Set();
                    break;
                }
                catch (ServerFaultException ex)
                {
                    HandleServerFaultException(ex);
                    
                    break;
                }
                catch (Exception ex)
                {
                    if (callCount >= _config.RetryCount + 1)
                    {
                        callCount = 0;

                        if (MessageBox.Show(string.Format("Unable to contact the server. Do you want to retry?\n\n{0}", ex.Message), "IMI iWMS Thin Client", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Cancel)
                        {
                            _form.Close();
                            break;
                        }
                    }
                    else
                    {
                        TimeSpan processingTime = DateTime.Now - startTime;

                        if (processingTime.TotalMilliseconds < Client.ConnectTimeout)
                        {
                            int sleepTime = Client.ConnectTimeout - (int)processingTime.TotalMilliseconds;
                            Thread.Sleep(sleepTime);
                        }
                    }
                }
            }
        }

        private static void EscapeControlChars(EventRequest request)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Event e in request.Events)
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    sb.Remove(0, sb.Length);

                    for (int i = 0; i < e.Data.Length; i++)
                    {
                        if (e.Data[i] < 32)
                        {
                            sb.Append(string.Format(@"\x{0:00}", (int)e.Data[i]));
                        }
                        else if (e.Data[i] == '\\')
                        {
                            sb.Append(@"\\");
                        }
                        else
                        {
                            sb.Append(e.Data[i]);
                        }
                    }

                    e.Data = sb.ToString();
                }
            }
        }

        private void HandleServerFaultException(ServerFaultException ex)
        {
            try
            {
                if (ex.Type == "Imi.Wms.Mobile.Server.Adapter.ApplicationTerminatedException" && ex.ErrorCode == "0")
                {
                    _form.Invoke(new Action(() =>
                    {
                        _form.Close();
                    }));
                }
                else
                {
                    string message = null;

                    if (!string.IsNullOrEmpty(ex.ErrorCode))
                    {
                        message = string.Format("{0}\nError Code = {1}", ex.Message, ex.ErrorCode);
                    }
                    else
                    {
                        message = ex.Message;
                    }

                    _form.Invoke(new Action(() =>
                    {
                        MessageBox.Show(message, "IMI iWMS Thin Client", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        _form.Close();
                    }));
                }
            }
            finally
            {
                _config.LastSessionId = null;
                ConfigurationManager.SaveConfiguration(_config);
            }
        }

        private void StateRequestThread()
        {
            while (!_abort)
            {
                _stateRequestEvent.WaitOne();

                if (_abort)
                {
                    return;
                }

                DateTime startTime = DateTime.Now;

                try
                {
                    StateRequest stateRequest = new StateRequest();
                    stateRequest.HashCode = _hashCode;

                    StateResponse stateResponse = Client.Invoke<StateRequest, StateResponse>(stateRequest);

                    if (!string.IsNullOrEmpty(stateResponse.HashCode))
                    {
                        _hashCode = stateResponse.HashCode;
                        Logger.IsEnabled = stateResponse.TraceLevel.ToLower() != "off";

                        _form.Invoke(new Action(() =>
                        {
                            _form.RenderState(stateResponse);
                        }));
                    }
                }
                catch (ServerFaultException ex)
                {
                    HandleServerFaultException(ex);

                    _abort = true;
                }
                catch (ObjectDisposedException)
                {
                }
                catch (Exception)
                {
                    TimeSpan processingTime = DateTime.Now - startTime;

                    if (processingTime.TotalMilliseconds < Client.ConnectTimeout)
                    {
                        int sleepTime = Client.ConnectTimeout - (int)processingTime.TotalMilliseconds;
                        Thread.Sleep(sleepTime);
                    }
                }
            }
        }
    }
}
