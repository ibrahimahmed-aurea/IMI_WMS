using System;
using System.Xml;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Windows.Media;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.SupplyChain.UX;
using Imi.Framework.UX;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class DashboardModule : ModuleInit, IShellModule
	{
        public const string ModuleId = "Dashboard";

        private WorkItem _rootWorkItem;
        private XmlDocument _menu;
        private IDashboardView _dashboardView;

		[InjectionConstructor]
        public DashboardModule([ServiceDependency] WorkItem rootWorkItem)
		{
            _rootWorkItem = rootWorkItem;
		}

        [ServiceDependency]
        public IActionCatalogService ActionCatalogService
        {
            get;
            set;
        }

        [ServiceDependency]
        public IShellInteractionService ShellInteractionService
        {
            get;
            set;
        }

        public override void AddServices()
        {
            base.AddServices();
        }
                
        public override void Load()
        {
            base.Load();
                         
            ShellAction refreshAction = new ShellAction(this._rootWorkItem);
            refreshAction.Caption = StringResources.Dashboard_Refresh;
            refreshAction.IsEnabled = true;
            refreshAction.IsAuthorized = true;
            refreshAction.Id = "action://Imi.SupplyChain.UX.Shell/DashboardRefresh";
                        
            ActionCatalogService.RegisterActionImplementation(refreshAction.Id, OnDashboardRefresh);

            ShellInteractionService.Actions.Add(refreshAction);

            ShellAction arrangeAction = new ShellAction(this._rootWorkItem);
            arrangeAction.Caption = StringResources.Dashboard_Arrange;
            arrangeAction.IsEnabled = true;
            arrangeAction.IsAuthorized = true;
            arrangeAction.Id = "action://Imi.SupplyChain.UX.Shell/DashboardArrange";

            ActionCatalogService.RegisterActionImplementation(arrangeAction.Id, OnDashboardArrange);

            ShellInteractionService.Actions.Add(arrangeAction);

            ControlledWorkItem<ShellController> shellWorkItem = _rootWorkItem.WorkItems.FindByType<ControlledWorkItem<ShellController>>().Last();
                        
            _dashboardView = shellWorkItem.SmartParts.FindByType<IDashboardView>().Last();
        }

        [EventSubscription(EventTopicNames.DashboardShowDialog)]
        public void OnShowDialog(object sender, MenuItemExecutedEventArgs args)
        {
            IAddWebPageView addWebPageView = _rootWorkItem.SmartParts.AddNew<AddWebPageView>();

            try
            {
                if (addWebPageView.ShowDialog().GetValueOrDefault())
                {
                    IWebView webView = _rootWorkItem.SmartParts.AddNew<WebView>();
                    webView.SetUrl(addWebPageView.Title, addWebPageView.Address);
                    _dashboardView.Show(webView);
                }
            }
            finally
            {
                _rootWorkItem.SmartParts.Remove(addWebPageView);
            }
        }

        public void OnDashboardArrange(WorkItem context, object caller, object target)
        {
            _dashboardView.Arrange();
        }
        
        public void OnDashboardRefresh(WorkItem context, object caller, object target)
        {
            _dashboardView.Refresh();
        }
                                
        public string Id
        {
            get
            {
                return ModuleId;
            }
        }

        public ImageSource Icon
        {
            get
            {
                return DashboardIcon.ImageSource;
            }
        }


        public string Title
        {
            get
            {
                return StringResources.Dashboard_Title;
            }
        }

        public string Version
        {
            get
            {
                return string.Format("{0}.{1}",
                    Assembly.GetExecutingAssembly().GetName().Version.Major,
                    Assembly.GetExecutingAssembly().GetName().Version.Minor);
            }
        }

        public int OrderIndex
        {
            get { return 0; }
            set {}
        }

        public XmlDocument GetMenu()
        {
            if (_menu == null)
                LoadMenu();

            return _menu;
        }

        private void LoadMenu()
        {
            _menu = new XmlDocument();
            XmlElement folderElement = _menu.CreateElement("folder");
            _menu.AppendChild(folderElement);
            folderElement.SetAttribute("caption", StringResources.Dashboard_Title);
            XmlElement itemElement = _menu.CreateElement("item");
            itemElement.SetAttribute("caption", StringResources.Dashboard_WebPage);
            itemElement.SetAttribute("topicIdentity", EventTopicNames.DashboardShowDialog);
            folderElement.AppendChild(itemElement);
        }
    }
}
