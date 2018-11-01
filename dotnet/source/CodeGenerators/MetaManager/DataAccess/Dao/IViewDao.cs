using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public enum FindViewTypes { All, Custom, Drilldowns };

    public interface IViewDao
    {
        View FindById(Guid viewId);
        IList<View> FindAll();
        View Save(View view);
        View SaveOrUpdate(View view);
        View SaveOrUpdateMerge(View view);
        void Delete(View view);
        IList<View> FindByEntityAndName(string entityName, string viewName, Guid applicationId);
        IList<string> FindAllUniqueCustomDLLNames(Guid applicationId);
        IList<View> FindViews(string entityName, string viewName, string title, FindViewTypes findViewTypes, Guid applicationId);
        View FindByPropertyMapId(Guid propertyMapId);
        IList<View> FindAllByServiceMethodId(Guid serviceMethodId);
        IList<View> FindByBusinessEntityId(Guid businessEntityId);
        IList<View> FindByNameAndApplicationId(string viewName, Guid applicationId);
        IList<View> FindAll(Guid applicationId);
    }
}
