using System;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public interface IReportQueryHelper
    {
        void SaveAndSynchronize(ReportQuery reportQuery);
    }
}
