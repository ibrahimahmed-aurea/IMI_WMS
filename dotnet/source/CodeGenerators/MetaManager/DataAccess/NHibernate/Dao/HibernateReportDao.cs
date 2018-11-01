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
    public class HibernateReportDao : HibernateDaoSupport, IReportDao
    {
        #region IReportDao Members

        [Transaction(ReadOnly = true)]
        public Report FindById(Guid reportId)
        {
            return HibernateTemplate.Get<Report>(reportId);
        }

        [Transaction(ReadOnly = true)]
        public IList<Report> FindAllReports(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Report>("Report.FindAllReports", paramNames, paramValues);
        }
                
        [Transaction(ReadOnly = false)]
        public Report SaveOrUpdate(Report report)
        {
            HibernateTemplate.SaveOrUpdate(report);
            return report;
        }

        [Transaction(ReadOnly = false)]
        public Report SaveOrUpdateMerge(Report report)
        {
            Report mergedReport = Session.Merge(report) as Report;
            HibernateTemplate.SaveOrUpdate(mergedReport);
            return mergedReport;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(Report report)
        {
            HibernateTemplate.Delete(report);
        }

        #endregion
    }
}
