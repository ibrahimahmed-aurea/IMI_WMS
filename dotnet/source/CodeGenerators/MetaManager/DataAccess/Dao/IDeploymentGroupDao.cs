using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IDeploymentGroupDao
    {
        IList<DeploymentGroup> FindAll();
        DeploymentGroup FindById(Guid deploymentGroupId);
        IList<DeploymentGroup> FindAllWithApplication(Guid applicationId);
        DeploymentGroup SaveOrUpdate(DeploymentGroup deploymentGroup);
        DeploymentGroup SaveOrUpdateMerge(DeploymentGroup deploymentGroup);
        void Delete(DeploymentGroup deploymentGroup);
    }
}
