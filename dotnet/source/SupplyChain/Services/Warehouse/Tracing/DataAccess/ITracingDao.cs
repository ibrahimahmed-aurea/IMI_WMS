using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Warehouse.Tracing.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.Tracing.DataAccess
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
