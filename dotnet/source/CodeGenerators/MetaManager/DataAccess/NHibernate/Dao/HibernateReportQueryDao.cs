using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.NHibernate.Generic.Support;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;
using NHibernate.SqlCommand;

namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernateReportQueryDao : HibernateDaoSupport, IReportQueryDao
    {
        #region IReportQueryDao Members

        [Transaction(ReadOnly = true)]
        public ReportQuery FindById(Guid reportQueryId)
        {
            return HibernateTemplate.Get<ReportQuery>(reportQueryId);
        }

        [Transaction(ReadOnly = false)]
        public ReportQuery SaveOrUpdate(ReportQuery reportQuery)
        {
            HibernateTemplate.SaveOrUpdate(reportQuery);
            return reportQuery;
        }

        [Transaction(ReadOnly = false)]
        public ReportQuery SaveOrUpdateMerge(ReportQuery reportQuery)
        {
            object mergedObj = Session.Merge(reportQuery);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (ReportQuery) mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(ReportQuery reportQuery)
        {
            HibernateTemplate.Delete(reportQuery);
        }

        #endregion
    }
}
