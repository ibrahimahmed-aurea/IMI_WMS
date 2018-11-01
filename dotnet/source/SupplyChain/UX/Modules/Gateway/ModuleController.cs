//===============================================================================
// Microsoft patterns & practices
// Smart Client Software Factory
//===============================================================================
// Copyright  Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by the "Add CAB Module" recipe.
//
// This class contains placeholder methods for the common module initialization 
// tasks, such as adding services, or user-interface element
//
// For more information see: 
// // ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.scsf.2006jun/SCSF/html/03-270-Creating%20a%20Module.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

using System;
using System.Threading;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.SmartParts;
using Imi.Framework.UX;
using Imi.SupplyChain.UX.Views;
using Imi.SupplyChain.UX.Modules.Gateway.Infrastructure.Services;
using Imi.SupplyChain.UX.Modules.Gateway.Views.ChooseDefaultWarehouse;
using Imi.SupplyChain.UX.Modules.Gateway.Services;
using Imi.SupplyChain.UX.Infrastructure.Services;
using System.Xml;
using Imi.SupplyChain.UX.Infrastructure;
using System.Reflection;
using System.IO;
using System.Windows.Media;
using System.Windows.Threading;
using System.Workflow.Runtime;
using System.Collections.Generic;

namespace Imi.SupplyChain.UX.Modules.Gateway
{
	public class ModuleController : ModuleControllerBase
	{
        public const string ModuleId = "Gateway";

        private IGatewayUserSessionService _gatewayUserSessionService;
        private XmlDocument _menu;
        private bool _first = true;
                        
        public override void Run()
		{
            _gatewayUserSessionService = WorkItem.Services.AddNew<GatewayUserSessionService, IGatewayUserSessionService>();

            ShellInteractionService.ModuleActivated += ModuleActivatedEventHandler;

            base.Run();
        }

        private void ModuleActivatedEventHandler(object sender, EventArgs e)
        {
            if (_first)
            {
                _first = false;
                EventTopic userSettingsTopic = WorkItem.EventTopics.Get(EventTopicNames.ShowChooseDefaultWarehouseDialog);
                userSettingsTopic.Fire(this, new EventArgs(), WorkItem, PublicationScope.Global);
            }
        }

        [EventSubscription(EventTopicNames.ShowDialog)]
        public void OnShowDialog(object sender, MenuItemExecutedEventArgs args)
        {
            if (_gatewayUserSessionService.WarehouseId == null)
            {
                OnChooseDefaultWarehouseDialog(this, null);
            }
            else
            {
                EventTopic itemTopic = WorkItem.RootWorkItem.EventTopics.Get(args.MenuItem.Parameters);
                itemTopic.Fire(this, new MenuEventArgs("", "", args.OpenInNewWindow, null), WorkItem, PublicationScope.Global);
            }
        }
                
        [EventSubscription(EventTopicNames.ShowChooseDefaultWarehouseDialog)]
        public void OnChooseDefaultWarehouseDialog(object sender, EventArgs args)
        {
            try
            {
                IChooseDefaultWarehouseView chooseWarehouse = WorkItem.SmartParts.AddNew<ChooseDefaultWarehouseView>();
                chooseWarehouse.ShowDialog();
            }
            finally
            {
                if (_gatewayUserSessionService.WarehouseId == null)
                    ShellInteractionService.ShowMessageBox(LocalResources.ChangeUserSettings_Caption, LocalResources.ChangeUserSettings_Message ,null, MessageBoxButton.Ok, MessageBoxImage.Warning);
            }
        }
                       
        public override string Id
        {
            get
            {
                return ModuleId;
            }
        }

        public override string Title
        {
            get
            {
                return LocalResources.Module_Title;
            }
        }

        public override string Version
        {
            get
            {
                return base.Version;
            }
        }

        public override int OrderIndex
        {
            get { return 3; }
            set { }
        }

        public override XmlDocument GetMenu()
        {
            if (_menu == null)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream xmlStream = assembly.GetManifestResourceStream("Imi.SupplyChain.UX.Modules.Gateway.Menu.xml");
                _menu = new XmlDocument();
                _menu.Load(xmlStream);

                XmlMenuTranslator.Translate(_menu, Resources.ResourceManager);
            }

            return _menu;
        }
	}
}
