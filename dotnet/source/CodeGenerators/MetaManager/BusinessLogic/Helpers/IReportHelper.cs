using System;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public interface IReportHelper
    {
        Report GetInitializedReport(Guid reportId);
    }
}
