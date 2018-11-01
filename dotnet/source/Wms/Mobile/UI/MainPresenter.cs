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
    public class MainPresenter
    {
        private MainForm _form;
        private ThinClient _client;
        private UISection _config;
        private INativeDriver _nativeDriver;
        
        public MainPresenter(MainForm form)
        {
            _form = form;
        }

        public void Initialize()
        {
            try
            {
                _config = ConfigurationManager.LoadConfiguration();
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error loading configuration.\n\n{0}", ex.Message);
                MessageBox.Show(errorMessage, _form.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

                return;
            }

            try
            {
                Logger.LogFileName = string.Format("{0}\\client.log.txt", new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)).AbsolutePath.Replace("%20", " "));
                Logger.IsEnabled = _config.LogEnabled;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error initializing logging.\n\n{0}", ex.Message);
                MessageBox.Show(errorMessage, _form.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

                return;
            }

            try
            {
                if (!string.IsNullOrEmpty(_config.NativeDriver))
                {
                    string nativeDriverTypeName = string.Format("Imi.Wms.Mobile.UI.Native.{0}Driver", _config.NativeDriver);

                    Type nativeDriverType = Assembly.GetExecutingAssembly().GetType(nativeDriverTypeName);

                    if (nativeDriverType == null)
                    {
                        throw new ArgumentException(string.Format("Unable to load native driver type: \"{0}\".", nativeDriverTypeName));
                    }

                    _nativeDriver = (INativeDriver)Activator.CreateInstance(nativeDriverType);
                }
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    ex = ex.InnerException;
                }
                                
                string errorMessage = string.Format("Error loading native driver.\n\n{0}", ex.Message);

                Logger.Write(errorMessage);
                Logger.Write(ex.ToString());

                MessageBox.Show(errorMessage, _form.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

                return;
            }
        }

        public void LoadServers(bool autoConnect)
        {
            _form.AddServers(_config.ServerCollection.OrderBy((a) => (a.Name)));

            var defaultServer = (from a in _config.ServerCollection
                                      where a.Default
                                      select a).FirstOrDefault();

            if (defaultServer != null && autoConnect)
            {
                Connect(defaultServer.Name, defaultServer.DefaultApplication);
            }
        }

        public void Connect(string serverName, string defaultApplication)
        {
            var server = (from ServerElement element in _config.ServerCollection
                          where element.Name == serverName
                       select element).LastOrDefault();

            _client = new ThinClient(server.HostName, server.Port);
            _client.ConnectTimeout = _config.ConnectTimeout * 1000;
            _client.ReceiveTimeout = _config.ReceiveTimeout * 1000;
            _client.SendTimeout = _config.SendTimeout * 1000;

            ConfigurationRequest request = new ConfigurationRequest();
            ConfigurationResponse response = null;

            int callCount = 0;

            while (true)
            {
                DateTime startTime = DateTime.Now;

                try
                {
                    callCount++;
                    response = _client.Invoke<ConfigurationRequest, ConfigurationResponse>(request);
                    break;
                }
                catch (ServerFaultException ex)
                {
                    HandleServerFaultException(ex);

                    return;
                }
                catch (Exception ex)
                {
                    if (callCount >= _config.RetryCount + 1)
                    {
                        callCount = 0;

                        if (MessageBox.Show(string.Format("Unable to contact the server. Do you want to retry?\n\n{0}", ex.Message), _form.Text, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                    else
                    {
                        TimeSpan processingTime = DateTime.Now - startTime;

                        if (processingTime.TotalMilliseconds < _client.ConnectTimeout)
                        {
                            int sleepTime = _client.ConnectTimeout - (int)processingTime.TotalMilliseconds;
                            Thread.Sleep(sleepTime);
                        }
                    }
                }
            }

            using (ApplicationForm applicationForm = new ApplicationForm())
            {
                _form.Visible = false;

                try
                {
                    applicationForm.Presenter.Client = _client;
                    applicationForm.Presenter.NativeDriver = _nativeDriver;
                    applicationForm.Presenter.Config = _config;
                    applicationForm.Presenter.ServerConfiguration = response;
                    applicationForm.Presenter.DefaultApplication = defaultApplication;
                    applicationForm.Left = _form.Left;
                    applicationForm.Top = _form.Top;
                    applicationForm.Width = _form.Width;
                    applicationForm.Height = _form.Height;
                    applicationForm.ShowDialog();
                }
                finally
                {
                    _form.Visible = true;
                }
            }
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

        public void AddServer()
        {
            using (ModifyServerForm form = new ModifyServerForm())
            {
                form.ShowDialog();
            }

            LoadServers(false);
        }

        public void ModifyServer(string serverName)
        {
            var server = (from ServerElement element in _config.ServerCollection
                       where element.Name == serverName
                       select element).LastOrDefault();

            using (ModifyServerForm form = new ModifyServerForm())
            {
                form.ServerElement = server;
                form.ShowDialog();
            }

            LoadServers(false);
        }

        public void DeleteServer(string serverName)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected server?", "IMI iWMS Thin Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {

                var server = (from ServerElement element in _config.ServerCollection
                           where element.Name == serverName
                           select element).LastOrDefault();

                _config.ServerCollection.Remove(server);

                ConfigurationManager.SaveConfiguration(_config);

                LoadServers(false);
            }
        }

        public void ModifyOptions()
        {
            using (OptionsForm form = new OptionsForm())
            {
                form.Config = _config;
                form.ShowDialog();
            }
        }

        public void DisposeNativeDriver()
        {
            if (_nativeDriver != null)
            {
                _nativeDriver.Dispose();
            }
        }
    }
}
