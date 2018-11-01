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
    public class HibernateModuleDao : HibernateDaoSupport, IModuleDao
    {
        #region IModuleDao Members

        [Transaction(ReadOnly = true)]
        Module IModuleDao.FindById(Guid moduleId)
        {
            return HibernateTemplate.Get<Module>(moduleId);
        }

        [Transaction(ReadOnly = true)]
        IList<Module> IModuleDao.FindAll()
        {
            return HibernateTemplate.LoadAll<Module>();
        }
        
        [Transaction(ReadOnly = true)]
        public IList<Module> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Module>("Module.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public Module Save(Module module)
        {
            if (module.Id == Guid.Empty) { module.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(module);
            return module;
        }

        [Transaction(ReadOnly = false)]
        public Module SaveOrUpdate(Module module)
        {
            HibernateTemplate.SaveOrUpdate(module);
            return module;
        }

        [Transaction(ReadOnly = false)]
        public Module SaveOrUpdateMerge(Module module)
        {
            object mergedObj = Session.Merge(module);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (Module)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(Module module)
        {
            HibernateTemplate.Delete(module);
        }

        #endregion
    }
}
