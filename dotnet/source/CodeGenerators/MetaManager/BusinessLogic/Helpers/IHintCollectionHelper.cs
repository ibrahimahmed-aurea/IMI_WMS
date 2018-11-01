using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public interface IHintCollectionHelper
    {
        void UpdateHints(DataModelChanges detectedChanges, HintCollection hintCollection);
    }
}
