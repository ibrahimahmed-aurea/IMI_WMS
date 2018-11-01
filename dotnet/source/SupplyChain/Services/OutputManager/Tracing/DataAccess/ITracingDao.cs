using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.OutputManager.Tracing.BusinessEntities;

namespace Imi.SupplyChain.OutputManager.Tracing.DataAccess
{
    public interface ITracingDao
    {
        StartDatabaseTracingResult StartDatabaseTracing(StartDatabaseTracingParameters parameters);
        StopDatabaseTracingResult StopDatabaseTracing(StopDatabaseTracingParameters parameters);        
        IList<GetServerInformationResult> GetServerInformation(GetServerInformationParameters parameters);               
    }
}
