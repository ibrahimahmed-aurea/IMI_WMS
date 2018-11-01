using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using Spring.Data.NHibernate.Generic;
using Spring.Data.NHibernate.Generic.Support;
using Imi.SupplyChain.Settings.DataAccess.Dao;
using Imi.SupplyChain.Settings.BusinessEntities;

namespace Imi.SupplyChain.Settings.DataAccess.NHibernate.Dao
{
    public class HibernateContainerDao : HibernateDaoSupport,IContainerDao
    {
        [Transaction(ReadOnly = false)]
        public Container SaveOrUpdate(Container container)
        {
            container.LastModified = DateTime.Now;
            HibernateTemplate.SaveOrUpdate(container);
            return container;
        }

        [Transaction(ReadOnly = false)]
        public void DeleteContainer(string containerName)
        {
            IList<Container> result = FindContainer(containerName);

            foreach (Container container in result)
            {
                HibernateTemplate.Delete(container);
            }
        }

        [Transaction(ReadOnly = false)]
        public IList<Container> FindContainer(string containerName)
        {
            string[] paramNames = { "containerName" };
            object[] paramValues = { (containerName ?? "%") };

            IList<Container> result = HibernateTemplate.FindByNamedQueryAndNamedParam<Container>("Container.FindContainerByContainerName", paramNames, paramValues);

            return result;
        }

    }
}
