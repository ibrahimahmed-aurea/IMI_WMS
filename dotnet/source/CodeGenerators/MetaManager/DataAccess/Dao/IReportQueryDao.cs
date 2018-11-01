using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IReportQueryDao
    {
        ReportQuery FindById(Guid reportQueryId);
        ReportQuery SaveOrUpdate(ReportQuery reportQuery);
        ReportQuery SaveOrUpdateMerge(ReportQuery reportQuery);
        void Delete(ReportQuery reportQuery);
    }
}
