using Imi.Framework.UX;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.OrderManagement
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
                        
            ControlledWorkItem<ModuleController> workItem = rootWorkItem.WorkItems.AddNew<ControlledWorkItem<ModuleController>>(ModuleController.ModuleId);
            workItem.Controller.Run();
        }
    }
}
