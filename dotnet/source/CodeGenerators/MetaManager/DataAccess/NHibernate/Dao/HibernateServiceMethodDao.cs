using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Spring.Data.NHibernate.Generic.Support;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;

namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernateServiceMethodDao : HibernateDaoSupport, IServiceMethodDao
    {
        #region IServiceMethodDao Members

        [Transaction(ReadOnly = true)]
        public ServiceMethod FindById(Guid serviceMethodId)
        {
            return HibernateTemplate.Get<ServiceMethod>(serviceMethodId) as ServiceMethod;
        }

        [Transaction(ReadOnly = true)]
        public IList<ServiceMethod> FindAll()
        {
            return HibernateTemplate.LoadAll<ServiceMethod>();
        }

        [Transaction(ReadOnly = true)]
        public IList<ServiceMethod> FindAllQueriesByApplicationId(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ServiceMethod>("ServiceMethod.FindAllQueriesByApplicationId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<ServiceMethod> FindAllRefCursorProcsByApplicationId(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ServiceMethod>("ServiceMethod.FindAllRefCursorProcsByApplicationId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<ServiceMethod> FindAllByApplicationId(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ServiceMethod>("ServiceMethod.FindAllByApplicationId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<ServiceMethod> FindAllByService(Guid serviceId)
        {
            string[] paramNames = {"serviceId" };
            object[] paramValues = {serviceId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ServiceMethod>("ServiceMethod.FindAllByService", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<ServiceMethod> FindByApplicationIdNameAndService(Guid applicationId, string name, string serviceName)
        {
            string[] paramNames = { "applicationId", "name", "serviceName" };
            object[] paramValues = { applicationId, name, serviceName };

            IList<ServiceMethod> serviceMethods = HibernateTemplate.FindByNamedQueryAndNamedParam<ServiceMethod>("ServiceMethod.FindByApplicationIdNameAndService", paramNames, paramValues);

            foreach (ServiceMethod serviceMethod in serviceMethods)
            {
                NHibernateUtil.Initialize(serviceMethod.Service);
                NHibernateUtil.Initialize(serviceMethod.MappedToAction.Query);
                NHibernateUtil.Initialize(serviceMethod.MappedToAction.StoredProcedure);

                if (serviceMethod.MappedToAction.StoredProcedure != null)
                    NHibernateUtil.Initialize(serviceMethod.MappedToAction.StoredProcedure.Package);
            }

            return serviceMethods;
        }

        [Transaction(ReadOnly = false)]
        public ServiceMethod Save(ServiceMethod serviceMethod)
        {
            if (serviceMethod.Id == Guid.Empty) { serviceMethod.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(serviceMethod);
            return serviceMethod;
        }

        [Transaction(ReadOnly = false)]
        public ServiceMethod SaveOrUpdate(ServiceMethod serviceMethod)
        {
            HibernateTemplate.SaveOrUpdate(serviceMethod);
            return serviceMethod;
        }

        [Transaction(ReadOnly = false)]
        public ServiceMethod SaveOrUpdateMerge(ServiceMethod serviceMethod)
        {
            object mergedObj = Session.Merge(serviceMethod);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (ServiceMethod) mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(ServiceMethod serviceMethod)
        {
            HibernateTemplate.Delete(serviceMethod);
        }

        [Transaction(ReadOnly = true)]
        public ServiceMethod FindByPropertyMapId(Guid propertyMapId)
        {
            string[] paramNames = { "propertyMapId" };
            object[] paramValues = { propertyMapId };

            IList<ServiceMethod> tmp = HibernateTemplate.FindByNamedQueryAndNamedParam<ServiceMethod>("ServiceMethod.FindByPropertyMapId", paramNames, paramValues);

            if (tmp.Count > 0)
                return tmp[0];
            else
                return null;
        }


        #endregion
    }
}
