using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess;
using Spring.Context;
using Spring.Context.Support;

namespace Cdc.MetaManager.BusinessLogic
{
    public class BackendService : IBackendService
    {
        private IApplicationContext ctx = null;

        private void GetContext()
        {
            // Get application service context
            if (ctx == null)
            {
                ctx = ContextRegistry.GetContext();
            }
        }


        
        #region IBackendService Members

        public IList<T> GetAllDomainObjectsByApplicationId<T>(Guid ApplicationId)
        {
            GetContext();
            
            if (typeof(T)  == typeof(DataAccess.Domain.Action))
            {
                return  ((IList<T>) ((object) ((DataAccess.Dao.IActionDao)ctx["ActionDao"]).FindAll(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Application))
            {
                IList<DataAccess.Domain.Application> tmpapplist = new List<DataAccess.Domain.Application>();
                tmpapplist.Add(((DataAccess.Dao.IApplicationDao)ctx["ApplicationDao"]).FindById(ApplicationId));
                return ((IList<T>) ((object) tmpapplist));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.BusinessEntity))
            {
                return ((IList<T>) ((object) ((DataAccess.Dao.IBusinessEntityDao)ctx["BusinessEntityDao"]).FindAll(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Hint))
            {
                return ((IList<T>) ((object) ((DataAccess.Dao.IHintDao)ctx["HintDao"]).FindAll(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.HintCollection))
            {
                return ((IList<T>) ((object) ((DataAccess.Dao.IHintCollectionDao)ctx["HintCollectionDao"]).FindAll(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Issue))
            {
                return ((IList<T>) ((object) ((DataAccess.Dao.IIssueDao)ctx["IssueDao"]).FindAllIssues(ApplicationId)));
            }
            //else if (typeof(T) == typeof(DataAccess.Domain.MappedProperty))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IMappedPropertyDao)ctx["MappedPropertyDao"]).FindAll(ApplicationId)));
            //}
            else if (typeof(T) == typeof(DataAccess.Domain.Package))
            {
                return ((IList<T>) ((object) ((DataAccess.Dao.IPackageDao)ctx["PackageDao"]).FindAllByApplicationId(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Property))
            {
                return ((IList<T>) ((object) ((DataAccess.Dao.IPropertyDao)ctx["PropertyDao"]).FindAll(ApplicationId)));
            }
            //else if (typeof(T) == typeof(DataAccess.Domain.PropertyMap))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IPropertyMapDao)ctx["PropertyMapDao"]).FindAll(ApplicationId)));
            //}
            //else if (typeof(T) == typeof(DataAccess.Domain.PropertyStorageInfo))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IPropertyStorageInfoDao)ctx["PropertyStorageInfoDao"]).FindAll(ApplicationId)));
            //}
            else if (typeof(T) == typeof(DataAccess.Domain.Query))
            {
                return ((IList<T>) ((object) ((DataAccess.Dao.IQueryDao)ctx["QueryDao"]).FindAllByApplicationId(ApplicationId)));
            }
            //else if (typeof(T) == typeof(DataAccess.Domain.QueryProperty))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IQueryPropertyDao)ctx["QueryPropertyDao"]).FindAll(ApplicationId)));
            //}
            else if (typeof(T) == typeof(DataAccess.Domain.Schema))
            {
                return ((IList<T>) ((object) ((DataAccess.Dao.ISchemaDao)ctx["SchemaDao"]).FindByApplicationId(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Service))
            {
                return ((IList<T>) ((object) ((DataAccess.Dao.IServiceDao)ctx["ServiceDao"]).FindAll(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.ServiceMethod))
            {
                return ((IList<T>) ((object) ((DataAccess.Dao.IServiceMethodDao)ctx["ServiceMethodDao"]).FindAllByApplicationId(ApplicationId)));
            }
            //else if (typeof(T) == typeof(DataAccess.Domain.StoredProcedure))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IStoredProcedureDao)ctx["StoredProcedureDao"]).FindAll(ApplicationId)));
            //}
            //else if (typeof(T) == typeof(DataAccess.Domain.StoredProcedureProperty))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IStoredProcedurePropertyDao)ctx["StoredProcedurePropertyDao"]).FindAll(ApplicationId)));
            //}
            return null;
        }

        #endregion
    }
}
