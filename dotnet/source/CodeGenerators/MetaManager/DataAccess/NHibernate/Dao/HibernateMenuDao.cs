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
    public class HibernateMenuDao : HibernateDaoSupport, IMenuDao
    {
        #region IMenuDao Members

        [Transaction(ReadOnly = true)]
        public Menu FindById(Guid menuId)
        {
            return HibernateTemplate.Get<Menu>(menuId) as Menu;
        }

        [Transaction(ReadOnly = true)]
        public Menu FindByApplicationId(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            IList<Menu> l = HibernateTemplate.FindByNamedQueryAndNamedParam<Menu>(
                                                    "Menu.FindByApplicationId",
                                                    paramNames, paramValues);

            if (l.Count == 1)
                return (l[0]);
            else
                return (null);
        }

        [Transaction(ReadOnly = true)]
        public IList<Menu> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Menu>(
                                                    "Menu.FindByApplicationId",
                                                    paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public Menu Save(Menu menu)
        {
            if (menu.Id == Guid.Empty) { menu.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(menu);
            return menu;
        }

        [Transaction(ReadOnly = false)]
        public Menu SaveOrUpdate(Menu menu)
        {
            HibernateTemplate.SaveOrUpdate(menu);
            return menu;
        }

        [Transaction(ReadOnly = false)]
        public Menu SaveOrUpdateMerge(Menu menu)
        {
            object mergedObj = Session.Merge(menu);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (Menu)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(Menu menu)
        {
            HibernateTemplate.Delete(menu);
        }


        #endregion
    }
}
