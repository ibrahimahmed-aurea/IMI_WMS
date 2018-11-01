using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public interface IModuleHelper
    {
        void FindAllModulesAndServicesReferancedByModule(Module module, List<Module> dependantModules, List<Service> dependantServices);
    }
}
