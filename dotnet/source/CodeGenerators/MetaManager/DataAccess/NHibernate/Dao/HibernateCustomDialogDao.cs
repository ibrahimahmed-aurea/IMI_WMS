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
    public class HibernateCustomDialogDao : HibernateDaoSupport, ICustomDialogDao
    {
        #region ICustomDialogDao Members

        [Transaction(ReadOnly = true)]
        public CustomDialog FindById(Guid customDialogId)
        {
            return HibernateTemplate.Get<CustomDialog>(customDialogId);
        }

        [Transaction(ReadOnly = true)]
        public IList<CustomDialog> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<CustomDialog>("CustomDialog.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public CustomDialog SaveOrUpdate(CustomDialog customDialog)
        {
            HibernateTemplate.SaveOrUpdate(customDialog);
            return customDialog;
        }

        [Transaction(ReadOnly = false)]
        public CustomDialog SaveOrUpdateMerge(CustomDialog customDialog)
        {
            object mergedObj = Session.Merge(customDialog);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (CustomDialog)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(CustomDialog customDialog)
        {
            HibernateTemplate.Delete(customDialog);
        }

        #endregion
    }
}
