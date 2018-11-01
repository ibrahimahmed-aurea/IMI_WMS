using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public interface IBusinessEntityHelper
    {
        void UpdateBuisinessEntities(DataModelChanges detectedChanges, Application backendApplication);
    }
}
