using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.NHibernate.Generic.Support;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernateDeploymentGroupDao : HibernateDaoSupport, IDeploymentGroupDao
    {
        #region IDeploymentGroupDao Members

        [Transaction(ReadOnly = true)]
        public IList<DeploymentGroup> FindAll()
        {
            return HibernateTemplate.LoadAll<DeploymentGroup>();
        }

        [Transaction(ReadOnly = true)]
        public DeploymentGroup FindById(Guid deploymentGroupId)
        {
            return HibernateTemplate.Get<DeploymentGroup>(deploymentGroupId);
        }

        [Transaction(ReadOnly = false)]
        public IList<DeploymentGroup> FindAllWithApplication(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<DeploymentGroup>("DeploymentGroup.FindAllWithApplication", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public DeploymentGroup SaveOrUpdate(DeploymentGroup deploymentGroup)
        {
            HibernateTemplate.SaveOrUpdate(deploymentGroup);
            return deploymentGroup;
        }

        [Transaction(ReadOnly = false)]
        public DeploymentGroup SaveOrUpdateMerge(DeploymentGroup deploymentGroup)
        {
            object mergedObj = Session.Merge(deploymentGroup);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (DeploymentGroup)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(DeploymentGroup deploymentGroup)
        {
            HibernateTemplate.Delete(deploymentGroup);
        }

        #endregion
    }
}
