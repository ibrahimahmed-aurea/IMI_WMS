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
    public class HibernatePackageDao : HibernateDaoSupport, IPackageDao
    {
        #region IPackageDao Members

        [Transaction(ReadOnly = true)]
        public Package FindById(Guid packageId)
        {
            return HibernateTemplate.Get<Package>(packageId) as Package;
        }

        [Transaction(ReadOnly = true)]
        public Package FindByStoredProcedureId(Guid storedProcedureId)
        {
            string[] paramNames = { "storedProcedureId" };
            object[] paramValues = { storedProcedureId };

            IList<Package> l = HibernateTemplate.FindByNamedQueryAndNamedParam<Package>(
                                                    "Package.FindByStoredProcedureId",
                                                    paramNames, paramValues);

            if (l.Count == 1)
                return (l[0]);
            else
                return (null);
        }

        [Transaction(ReadOnly = true)]
        public Package FindByNameAndSchemaId(string packageName, Guid schemaId)
        {
            string[] paramNames = { "packageName", "schemaId" };
            object[] paramValues = { packageName, schemaId };

            IList<Package> l = HibernateTemplate.FindByNamedQueryAndNamedParam<Package>(
                                                    "Package.FindByNameAndSchemaId",
                                                    paramNames, paramValues);

            if (l.Count == 1)
                return (l[0]);
            else
                return (null);
        }

        [Transaction(ReadOnly = true)]
        public IList<Package> FindAllBySchemaId(Guid schemaId)
        {
            string[] paramNames = { "schemaId" };
            object[] paramValues = { schemaId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Package>("Package.FindAllBySchemaId",
                                                                            paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<Package> FindAllByApplicationId(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Package>("Package.FindAllByApplicationId",
                                                                            paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public Package Save(Package package)
        {
            if (package.Id == Guid.Empty) { package.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(package);
            return package;
        }

        [Transaction(ReadOnly = false)]
        public Package SaveOrUpdate(Package package)
        {
            HibernateTemplate.SaveOrUpdate(package);
            return package;
        }

        [Transaction(ReadOnly = false)]
        public Package SaveOrUpdateMerge(Package package)
        {
            object mergedObj = Session.Merge(package);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (Package)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(Package package)
        {
            HibernateTemplate.Delete(package);
        }


        #endregion
    }
}
