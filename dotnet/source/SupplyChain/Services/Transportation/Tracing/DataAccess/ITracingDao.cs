using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Transportation.Tracing.BusinessEntities;

namespace Imi.SupplyChain.Transportation.Tracing.DataAccess
{
    public interface ITracingDao
    {
        StartDatabaseTracingResult StartDatabaseTracing(StartDatabaseTracingParameters parameters);
        StopDatabaseTracingResult StopDatabaseTracing(StopDatabaseTracingParameters parameters);

        ModifyInterfaceTracingResult ModifyInterfaceTracing(ModifyInterfaceTracingParameters parameters);
        IList<CheckInterfaceTracingResult> CheckInterfaceTracing(CheckInterfaceTracingParameters parameters);
        IList<GetServerInformationResult> GetServerInformation(GetServerInformationParameters parameters);               
    }
}
