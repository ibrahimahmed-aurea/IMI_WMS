using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Spring.Data.NHibernate.Generic.Support;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Dao;
using DAD = Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernateActionDao : HibernateDaoSupport, IActionDao
    {
        #region IActionDao Members

        [Transaction(ReadOnly = true)]
        public DAD.Action FindById(Guid actionId)
        {
            return HibernateTemplate.Get<DAD.Action>(actionId);
        }

        [Transaction(ReadOnly = true)]
        public DAD.Action FindByQueryId(Guid queryId)
        {
            string[] paramNames = { "queryId" };
            object[] paramValues = { queryId };

            IList<DAD.Action> l = HibernateTemplate.FindByNamedQueryAndNamedParam<DAD.Action>("Action.FindByQueryId", paramNames, paramValues);

            if (l.Count > 0)
                return (l[0]);
            else
                return (null);
        }

        [Transaction(ReadOnly = true)]
        public DAD.Action FindByStoredProcedureId(Guid storedProcedureId)
        {
            string[] paramNames = { "storedProcedureId" };
            object[] paramValues = { storedProcedureId };

            IList<DAD.Action> l = HibernateTemplate.FindByNamedQueryAndNamedParam<DAD.Action>("Action.FindByStoredProcedureId", paramNames, paramValues);

            if (l.Count > 0)
                return (l[0]);
            else
                return (null);
        }

        [Transaction(ReadOnly = true)]
        public DAD.Action FindByServiceMethodId(Guid serviceMethodId)
        {
            string[] paramNames = { "serviceMethodId" };
            object[] paramValues = { serviceMethodId };

            IList<DAD.Action> l = HibernateTemplate.FindByNamedQueryAndNamedParam<DAD.Action>("Action.FindByServiceMethodId", paramNames, paramValues);

            if (l.Count > 0)
                return (l[0]);
            else
                return (null);
        }

        [Transaction(ReadOnly = true)]
        public IList<DAD.Action> FindAll()
        {
            return HibernateTemplate.LoadAll<DAD.Action>();
        }

        [Transaction(ReadOnly = true)]
        public IList<DAD.Action> FindByBusinessEntityId(Guid businessEntityId)
        {
            string[] paramNames = { "businessEntityId" };
            object[] paramValues = { businessEntityId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<DAD.Action>("Action.FindByBusinessEntityId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<DAD.Action> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<DAD.Action>("Action.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public DAD.Action FetchWithMaps(Guid actionId)
        {
            string[] paramNames = { "actionId" };
            object[] paramValues = { actionId };

            IList<DAD.Action> l = HibernateTemplate.FindByNamedQueryAndNamedParam<DAD.Action>("Action.FetchWithMaps", paramNames, paramValues);

            if (l.Count == 1)
                return (l[0]);
            else
                return (null);
        }

        [Transaction(ReadOnly = true)]
        public IList<DAD.Action> FindAllUnassigned(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<DAD.Action>("Action.FindAllUnassigned", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public DAD.Action Save(DAD.Action action)
        {
            HibernateTemplate.SaveOrUpdate(action);
            return action;
        }

        [Transaction(ReadOnly = false)]
        public DAD.Action SaveOrUpdate(DAD.Action action)
        {
            

            HibernateTemplate.SaveOrUpdate(action);
            return action;
        }

        [Transaction(ReadOnly = false)]
        public DAD.Action SaveOrUpdateMerge(DAD.Action action)
        {
            action = (DAD.Action)Session.Merge(action);
            HibernateTemplate.SaveOrUpdate(action);
            return action;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(DAD.Action action)
        {
            HibernateTemplate.Delete(action);
        }

        #endregion

        #region IActionDao Members

        [Transaction(ReadOnly = true)]
        public IList<Cdc.MetaManager.DataAccess.Domain.Action> FindByEntityAndName(string entityName, string actionName, Guid applicationId)
        {
            if (string.IsNullOrEmpty(entityName))
                entityName = "%";

            if (string.IsNullOrEmpty(actionName))
                actionName = "%";

            string[] paramNames = { "entityName", "actionName", "applicationId" };
            object[] paramValues = { entityName, actionName, applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<DAD.Action>("Action.FindByEntityAndName", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public DAD.Action FindByPropertyMapId(Guid propertyMapId)
        {
            string[] paramNames = { "propertyMapId" };
            object[] paramValues = { propertyMapId };

            IList<DAD.Action> tmp = HibernateTemplate.FindByNamedQueryAndNamedParam<DAD.Action>("Action.FindByPropertyMapId", paramNames, paramValues);

            if (tmp.Count > 0)
                return tmp[0];
            else
                return null;
        }


        #endregion
    }
}
