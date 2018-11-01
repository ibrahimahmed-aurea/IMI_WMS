using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.Framework.UX;
using Microsoft.Practices.CompositeUI.Configuration;
using System.Xml;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX.Views;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.Windows.Media;
using System.Windows.Threading;
using System.Reflection;
using System.Workflow.Runtime;


namespace Imi.SupplyChain.UX
{
    public class ModuleControllerBase : WorkItemController, IShellModule
    {
        protected IExplorerView _helpView;
        
        [ServiceDependency]
        public IModuleLoaderService ModuleLoaderService { get; set; }

        [ServiceDependency]
        public IShellInteractionService ShellInteractionService { get; set; }

        [ServiceDependency]
        public IAuthorizationService AuthorizationService { get; set; }
        
        public override void Run()
        {
            Xceed.Wpf.DataGrid.Licenser.LicenseKey = "DGP60-PF4JB-GTREY-3UXA";

            ShellInteractionService.HyperlinkExecuted += HyperlinkExecutedEventHandler;
            
            ShellInteractionService.SmartPartClosing += (s, e) =>
            {
                if (e.SmartPart == _helpView)
                {
                    WorkItem.SmartParts.Remove(_helpView);
                    _helpView = null;
                }
            };

            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                WorkflowRuntime runtime = WorkItem.Services.AddNew<WorkflowRuntime>();
                runtime.StartRuntime();
            }), DispatcherPriority.ApplicationIdle);
                        
            base.Run();
        }
                
        private void HyperlinkExecutedEventHandler(object sender, HyperlinkExecutedEventArgs e)
        {
            if (e.Hyperlink.Data.ContainsKey("DialogId")) //Support for links with only moduleid
            {
                var node = (from n in GetMenu().SelectNodes("descendant::item").Cast<XmlNode>()
                            where n.Attributes["operation"].Value == e.Hyperlink.Data["DialogId"]
                            select n).LastOrDefault();

                if (node != null)
                {
                    ShellMenuItem temp = new ShellMenuItem();
                    temp.Operation = node.Attributes["operation"].Value;

                    AuthorizationService.CheckAuthorization(Id, new IAuthOperation[] { temp });

                    if (temp.IsAuthorized)
                    {
                        if (!string.IsNullOrEmpty(node.Attributes["assemblyFile"].Value))
                        {
                            ModuleInfo info = new ModuleInfo(node.Attributes["assemblyFile"].Value);
                            ModuleLoaderService.Load(WorkItem, info);
                        }

                        EventTopic itemTopic = WorkItem.RootWorkItem.EventTopics.Get(node.Attributes["parameters"].Value);
                        MenuEventArgs args = new MenuEventArgs("", HyperlinkHelper.BuildQueryString(e.Hyperlink.Data), true, null);
                        itemTopic.Fire(this, args, WorkItem, PublicationScope.Global);
                    }
                    else
                    {
                        ShellInteractionService.ShowMessageBox(StringResources.Authorization_NotAuhtorized, StringResources.Authorization_Message, null, MessageBoxButton.Ok, MessageBoxImage.Warning);
                    }
                }
            }
        }

        protected void HelpRequestedEventHandler(object sender, HelpRequestedEventArgs e)
        {
            if (_helpView == null)
            {
                if (string.IsNullOrEmpty(e.HelpBaseUri))
                {
                    e.Cancel = true;
                }
                else
                {
                    _helpView = WorkItem.SmartParts.AddNew<ExplorerView>();

                    SmartPartInfo info = new SmartPartInfo();
                    info.Title = string.Format(StringResources.Application_Help_Title, Id);
                    Uri helpUri = new Uri(new Uri(e.HelpBaseUri), Id + "/default.htm");

                    _helpView.SetUrl(info.Title, helpUri.AbsoluteUri);

                    ShellInteractionService.Show(_helpView, info);
                }
            }
            else
            {
                ShellInteractionService.Show(_helpView);
            }
        }

        public virtual XmlDocument GetMenu()
        {
            return new XmlDocument();
        }

        public virtual string Id
        {
            get
            {
                return "";
            }
        }

        public virtual ImageSource Icon
        {
            get
            {
                return SupplyChainIcon.ImageSource;
            }
        }

        public virtual string Title
        {
            get
            {
                return "";
            }
        }

        public virtual string Version
        {
            get
            {
                return string.Format("{0}.{1}",
                    Assembly.GetCallingAssembly().GetName().Version.Major,
                    Assembly.GetCallingAssembly().GetName().Version.Minor);
            }
        }

        public virtual int OrderIndex
        {
            get;
            set;
        }
    }


}
