using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IDataSourceDao
    {
        DataSource FindById(Guid dataSourceId);
        IList<DataSource> FindAll(Guid applicationId);
        DataSource SaveOrUpdate(DataSource dataSource);
        DataSource SaveOrUpdateMerge(DataSource dataSource);
        void Delete(DataSource dataSource);
        DataSource FindByPropertyMapId(Guid propertyMapId);
        IList<DataSource> FindAllByServiceMethodId(Guid serviceMethodId);
    }
}
