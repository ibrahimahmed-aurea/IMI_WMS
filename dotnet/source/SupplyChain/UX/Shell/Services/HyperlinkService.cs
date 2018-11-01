using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.Services.Settings.DataContracts;
using Imi.SupplyChain.Services.Settings.ServiceContracts;
using Imi.SupplyChain.UX.Infrastructure.Services;
using System.Collections.Specialized;
using Microsoft.Practices.CompositeUI;
using System.ServiceModel;
using System.Diagnostics;
using System.ServiceModel.Description;
using Imi.SupplyChain.UX.Infrastructure;
using System.Windows;
using Imi.Framework.UX;
using Microsoft.Practices.ObjectBuilder;
using System.Web;
using System.Net;
using Imi.SupplyChain.UX.Shell.Configuration;
using System.Configuration;

namespace Imi.SupplyChain.UX.Shell.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class HyperlinkService : IHyperlinkService, IBuilderAware
    {
        private ServiceHost _serviceHost;
        private IUserSessionService _userSessionService;
        private ShellHyperlink _shellHyperlink;
        private IShellModuleService _shellModuleService;
        private WorkItem _workItem;
        private IShellModule _module;
        private HttpListener _httpListener;

        public HyperlinkService(IUserSessionService userSessionService)
            : this(userSessionService, null, null)
        { 
        }
        
        [InjectionConstructor]
        public HyperlinkService([ServiceDependency] IUserSessionService userSessionService, [ServiceDependency] WorkItem workItem, [ServiceDependency] IShellModuleService shellModuleService)
        {
            _userSessionService = userSessionService;
            _workItem = workItem;
            _shellModuleService = shellModuleService;
            
            if (_shellModuleService != null)
            {
                _shellModuleService.ModuleActivated += ModuleActivatedEventHandler;
            }
        }

        private void ModuleActivatedEventHandler(object sender, DataEventArgs<IShellModule> e)
        {
            if (e.Data == _module && _shellHyperlink != null)
            {
                ExecuteHyperlink(_shellHyperlink);
            }
        }
        
        public string GetInstanceIdentifier()
        {
            return string.Format("{0}/{1}/{2}", _userSessionService.HostName, _userSessionService.HostPort, _userSessionService.InstanceName);
        }

        public bool ExecuteHyperlink(Uri hyperlink)
        {
            if (hyperlink != null)
            {
                ShellHyperlink shellHyperlink = ConvertToShellHyperlink(hyperlink);

                if (!string.IsNullOrEmpty(shellHyperlink.ModuleId))
                {
                    _module = (from m in _shellModuleService.Modules
                               where m.Id == shellHyperlink.Data["ModuleId"]
                               select m).LastOrDefault();

                    if (_module != null)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Application.Current.MainWindow.Activate();

                            if (_shellModuleService.ActiveModule != _module)
                            {
                                _shellHyperlink = shellHyperlink;
                                _shellModuleService.ActiveModule = _module;
                            }
                            else
                            {
                                ExecuteHyperlink(shellHyperlink);
                            }
                        }));

                        return true;
                    }
                }
            }

            return false;
        }

        public static ShellHyperlink ConvertToShellHyperlink(Uri hyperlink)
        {
            var data = new Dictionary<string, string>();

            NameValueCollection args = HttpUtility.ParseQueryString(hyperlink.Query);

            for (int i = 0; i < args.Count; i++)
            {
                data.Add(args.GetKey(i), args[args.GetKey(i)]);
            }
            
            string moduleId = null;

            if (data.ContainsKey("ModuleId"))
            {
                moduleId = data["ModuleId"];
            }

            return new ShellHyperlink(hyperlink.OriginalString, moduleId, data);
        }

        public void ExecuteHyperlink(ShellHyperlink hyperlink)
        {
            IShellModule module = (from m in _shellModuleService.Modules
                                   where m.Id == hyperlink.ModuleId
                                   select m).LastOrDefault();

            if (module != null)
            {
                ShellInteractionService interactionService = _shellModuleService.GetWorkItem(module).Services.Get<IShellInteractionService>() as ShellInteractionService;
                interactionService.OnHyperlinkExecuted(new HyperlinkExecutedEventArgs(hyperlink));
                _shellHyperlink = null;
                //Clean up saved shellhyperlink from activation uri at application startup, See ShellApplication.cs function AddServices
                while (_workItem.RootWorkItem.Items.FindByType<ShellHyperlink>().Count > 0)
                {
                    _workItem.RootWorkItem.Items.Remove(_workItem.RootWorkItem.Items.FindByType<ShellHyperlink>().FirstOrDefault());
                }
            }
        }

        private void HttpListenerCallback(IAsyncResult ar)
        {
            if (_httpListener != null && _httpListener.IsListening)
            {
                try
                {
                    HttpListenerContext context = _httpListener.EndGetContext(ar);

                    ExecuteHyperlink(context.Request.Url);
                }
                finally
                {
                    _httpListener.BeginGetContext(new AsyncCallback(HttpListenerCallback), null);
                }
            }
        }

        public void Start()
        {
            if (_serviceHost != null)
            {
                Stop();
            }
                        
            _serviceHost = new ServiceHost(this, new Uri(string.Format("net.pipe://localhost/{0}", Process.GetCurrentProcess().Id.ToString())));
            _serviceHost.AddServiceEndpoint(typeof(IHyperlinkService), new NetNamedPipeBinding(), typeof(HyperlinkService).Name);
            _serviceHost.Open();


            try
            {
                ShellConfigurationSection config = ConfigurationManager.GetSection(ShellConfigurationSection.SectionKey) as ShellConfigurationSection;

                string uri = string.Format("http://localhost{0}/IMI/SupplyChain/SmartClientKickStart/", string.IsNullOrEmpty(config.HttpListenerPort) ? string.Empty : ":" + config.HttpListenerPort);

                _httpListener = new HttpListener();
                _httpListener.Prefixes.Add(uri);
                _httpListener.Start();
                _httpListener.BeginGetContext(new AsyncCallback(HttpListenerCallback), null);
            }
            catch
            {
                try
                {
                    if (_httpListener != null)
                    {
                        try
                        {
                            _httpListener.Stop();
                            _httpListener.Close();
                        }
                        catch
                        {
                        }
                        finally
                        {
                            _httpListener.Abort();
                        }
                    }
                }
                finally
                {
                    _httpListener = null;
                }
            }
        }

        public void Stop()
        {
            if (_serviceHost != null)
            {
                try
                {
                    _serviceHost.Close();
                }
                catch (InvalidOperationException)
                {
                }
                catch (CommunicationObjectFaultedException)
                {
                }
                catch (TimeoutException)
                {
                }
                finally
                {
                    _serviceHost.Abort();
                }
            }

            if (_httpListener != null)
            {
                try
                {
                    _httpListener.Stop();
                    _httpListener.Close();
                }
                finally
                {
                    _httpListener.Abort();
                }
            }
        }

        public bool RedirectToExistingInstance(Uri hyperlink)
        {
            IEnumerable<Process> processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Where(p => p.Id != Process.GetCurrentProcess().Id);

            if (processes.Count() > 0)
            {
                ChannelFactory<IHyperlinkService> factory = new ChannelFactory<IHyperlinkService>(new NetNamedPipeBinding());

                try
                {
                    foreach (Process proc in processes)
                    {
                        IHyperlinkService client = factory.CreateChannel(new EndpointAddress(string.Format("net.pipe://localhost/{0}/{1}", proc.Id, typeof(HyperlinkService).Name)));

                        try
                        {
                            if (client.GetInstanceIdentifier() == GetInstanceIdentifier())
                            {
                                return client.ExecuteHyperlink(hyperlink);
                            }
                        }
                        catch (CommunicationException)
                        {
                        }
                        catch (InvalidOperationException)
                        {
                        }
                        catch (TimeoutException)
                        {
                        }
                    }
                }
                finally
                {
                    try
                    {
                        factory.Close();
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    catch (CommunicationObjectFaultedException)
                    {
                    }
                    catch (TimeoutException)
                    {
                    }
                    finally
                    {
                        factory.Abort();
                    }
                }
            }
            
            return false;
        }

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
        }

        public void OnTearingDown()
        {
            Stop();
        }

        #endregion
    }
}
