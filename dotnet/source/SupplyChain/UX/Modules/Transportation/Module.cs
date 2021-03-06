using System;
using System.Xml;
using System.ServiceModel;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.SupplyChain.UX;
using Imi.Framework.UX;
using Imi.SupplyChain.UX.Views;
using System.Reflection;
using System.IO;
using System.Configuration;
using Imi.SupplyChain.UX.Modules.Transportation.Infrastructure.Services;
using System.Windows.Media;
using Microsoft.Practices.CompositeUI.SmartParts;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Modules.Transportation
{
    public class Module : ModuleInit
	{
		private WorkItem rootWorkItem;
                        
		[InjectionConstructor]
		public Module([ServiceDependency] WorkItem rootWorkItem)
		{
            this.rootWorkItem = rootWorkItem;
		}
       
        public override void Load()
        {
            base.Load();
                        
            ControlledWorkItem<ModuleController> workItem = 
                rootWorkItem.WorkItems.AddNew<ControlledWorkItem<ModuleController>>(ModuleController.ModuleId);
            workItem.Controller.Run();
        }
    }
}
