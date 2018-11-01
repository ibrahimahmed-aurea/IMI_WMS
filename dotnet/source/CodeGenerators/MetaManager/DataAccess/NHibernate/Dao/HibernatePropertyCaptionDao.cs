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
    public class HibernatePropertyCaptionDao : HibernateDaoSupport, IPropertyCaptionDao
    {
        [Transaction(ReadOnly = true)]
        public PropertyCaption FindById(Guid propertyCaptionId)
        {
            return HibernateTemplate.Get<PropertyCaption>(propertyCaptionId);
        }
    }
}
