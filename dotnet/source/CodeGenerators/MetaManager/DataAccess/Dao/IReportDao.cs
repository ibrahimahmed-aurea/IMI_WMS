using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IReportDao
    {
        Report FindById(Guid reportId);
        IList<Report> FindAllReports(Guid applicationId);
        Report SaveOrUpdate(Report report);
        Report SaveOrUpdateMerge(Report report);
        void Delete(Report report);
    }
}
