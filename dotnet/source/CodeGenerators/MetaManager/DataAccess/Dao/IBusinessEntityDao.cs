using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IBusinessEntityDao
    {
        BusinessEntity FindById(Guid businessEntityId);
        IList<BusinessEntity> FindAll(Guid applicationId);
        BusinessEntity Save(BusinessEntity businessEntity);
        BusinessEntity SaveOrUpdate(BusinessEntity businessEntity);
        BusinessEntity SaveOrUpdateMerge(BusinessEntity businessEntity);
        void Delete(BusinessEntity businessEntity);
    }
}

