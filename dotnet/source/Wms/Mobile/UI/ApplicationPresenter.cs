using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using Imi.Wms.Mobile.Server;
using Imi.Wms.Mobile.UI.Configuration;
using Imi.Wms.Mobile.Server.Interface;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Imi.Wms.Mobile.UI.Shared;

namespace Imi.Wms.Mobile.UI
{
    public class ApplicationPresenter
    {
        private ApplicationForm _form;
        private ThinClient _client;
        private UISection _config;
        private INativeDriver _nativeDriver;
        private ConfigurationResponse _serverConfiguration;
        private string _defaultApplication;
        
        public ApplicationPresenter(ApplicationForm form)
        {
            _form = form;
        }

        public ConfigurationResponse ServerConfiguration
        {
            get
            {
                return _serverConfiguration;
            }
            set
            {
                _serverConfiguration = value;
            }
        }

        public UISection Config
        {
            get
            {
                return _config;
            }
            set
            {
                _config = value;
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
            }
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

        public string DefaultApplication
        {
            get
            {
                return _defaultApplication;
            }
            set
            {
                _defaultApplication = value;
            }
        }
                
        public void LoadApplications()
        {
            _form.AddApplications(_serverConfiguration);

            if (!string.IsNullOrEmpty(_defaultApplication))
            {
                if (_serverConfiguration.Applications.Where(a => a.Name == _defaultApplication).Count() == 1)
                {
                    StartApplication(_defaultApplication);
                }
            }
        }

        public void StartApplication(string applicationName)
        {
            Logger.Write(string.Format("Starting application \"{0}\"...", applicationName));

            if (CreateSession(applicationName))
            {
                StartRender();
            }
        }
                
        private bool CreateSession(string applicationName)
        {
            Logger.Write(string.Format("Creating session for application \"{0}\"...", applicationName));
            
            string sessionId = Guid.NewGuid().ToString();

            if (!string.IsNullOrEmpty(_config.LastSessionId))
            {
                string[] parts = _config.LastSessionId.Split('&');
                
                if (parts.Length > 1 && parts[0] == applicationName)
                {
                    sessionId = parts[1];
                }

                Logger.Write(string.Format("Reusing existing session id: \"{0}\"...", sessionId));
            }
            
            Client.SessionId = sessionId;
            
            CreateSessionRequest request = new CreateSessionRequest();
            request.ApplicationName = applicationName;
            request.ClientVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            request.ClientPlatform = DeviceInfo.GetPlatform();
            request.TerminalId = _config.TerminalId;

            if (string.IsNullOrEmpty(request.TerminalId))
            {
                try
                {
                    request.TerminalId = System.Net.Dns.GetHostName();
                }
                catch (Exception)
                {
                    request.TerminalId = "Unknown";
                }
            }

            int callCount = 0;

            while (true)
            {
                DateTime startTime = DateTime.Now;

                try
                {
                    callCount++;
                    Client.Invoke<CreateSessionRequest, CreateSessionResponse>(request);

                    _config.LastSessionId = string.Format("{0}&{1}", applicationName, Client.SessionId);
                    ConfigurationManager.SaveConfiguration(_config);

                    return true;
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

                        string message = string.Format("Unable to contact the server. Do you want to retry?\n\n{0}", ex.Message);

                        Logger.Write(message);
                        Logger.Write(ex.ToString());

                        if (MessageBox.Show(message, _form.Text, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Cancel)
                        {
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

            return false;
        }

        private void HandleServerFaultException(ServerFaultException ex)
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

            MessageBox.Show(message, "IMI iWMS Thin Client", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        }
        
        private void StartRender()
        {
            using (RenderForm renderForm = new RenderForm())
            {
                _form.Visible = false;

                try
                {
                    renderForm.Left = _form.Left;
                    renderForm.Top = _form.Top;
                    renderForm.Width = _form.Width;
                    renderForm.Height = _form.Height;
                    renderForm.Presenter.Client = Client;
                    renderForm.Presenter.NativeDriver = _nativeDriver;
                    renderForm.ShowDialog();
                }
                finally
                {
                    _form.Visible = true;
                }
            }
        }
    }
}
