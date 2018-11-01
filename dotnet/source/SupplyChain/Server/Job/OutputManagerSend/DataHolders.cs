using System;

namespace Imi.SupplyChain.Server.Job.OutputManagerSend
{
    /// <summary>
    /// Summary description for DatasHolders.
    /// </summary>
    public class Document
    {
        public double? PrintJobId;
        public int SeqNum;
        public string Data;
        public string WarehouseId;
    }

    public class Warehouse_OutputManager_Config
    {
        public string WarehouseId;
        public string OutputManagerId;
        public string Main_Url;
        public string Fallback_Url;
    }
}
