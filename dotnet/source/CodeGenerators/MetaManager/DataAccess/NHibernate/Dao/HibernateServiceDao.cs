using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Spring.Data.NHibernate.Generic.Support;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernateServiceDao : HibernateDaoSupport, IServiceDao
    {
        #region IServiceDao Members

        [Transaction(ReadOnly = true)]
        public Service FindById(Guid serviceId)
        {
            return HibernateTemplate.Get<Service>(serviceId) as Service;
        }

        [Transaction(ReadOnly = true)]
        public IList<Service> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Service>("Service.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public Service FindByName(Guid applicationId, string serviceName)
        {
            string[] paramNames = { "applicationId", "serviceName" };
            object[] paramValues = { applicationId, serviceName };

            IList<Service> l = HibernateTemplate.FindByNamedQueryAndNamedParam<Service>("Service.FindByName", paramNames, paramValues);
            
            if (l != null && l.Count > 0)
                return l[0];
            else
                return null;
        }

        [Transaction(ReadOnly = false)]
        public Service Save(Service service)
        {
            if (service.Id == Guid.Empty) { service.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(service);
            return service;
        }

        [Transaction(ReadOnly = false)]
        public Service SaveOrUpdate(Service service)
        {
            HibernateTemplate.SaveOrUpdate(service);
            return service;
        }

        [Transaction(ReadOnly = false)]
        public Service SaveOrUpdateMerge(Service service)
        {
            object mergedObj = Session.Merge(service);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (Service) mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(Service service)
        {
            HibernateTemplate.Delete(service);
        }

        #endregion
    }
}
