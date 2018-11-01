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
    public class HibernateMenuItemDao : HibernateDaoSupport, IMenuItemDao
    {
        #region IMenuItemDao Members

        [Transaction(ReadOnly = true)]
        public MenuItem FindById(Guid menuItemId)
        {
            return HibernateTemplate.Get<MenuItem>(menuItemId) as MenuItem;
        }

        [Transaction(ReadOnly = true)]
        public IList<MenuItem> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<MenuItem>("MenuItem.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<MenuItem> FindAllByActionId(Guid actionId)
        {
            string[] paramNames = { "actionId" };
            object[] paramValues = { actionId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<MenuItem>("MenuItem.FindAllByActionId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public MenuItem Save(MenuItem menuItem)
        {
            if (menuItem.Id == Guid.Empty) { menuItem.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(menuItem);
            return menuItem;
        }

        [Transaction(ReadOnly = false)]
        public MenuItem SaveOrUpdate(MenuItem menuItem)
        {
            HibernateTemplate.SaveOrUpdate(menuItem);
            return menuItem;
        }

        [Transaction(ReadOnly = false)]
        public MenuItem SaveOrUpdateMerge(MenuItem menuItem)
        {
            object mergedObj = Session.Merge(menuItem);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (MenuItem)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(MenuItem menuItem)
        {
            HibernateTemplate.Delete(menuItem);
        }


        #endregion
    }
}
