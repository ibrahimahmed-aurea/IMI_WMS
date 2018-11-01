using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.OM.Services.OutputHandler.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.OM.Services.OutputHandler.DataContracts/2016/11")]
    public class FindPrinterInfoResult
    {
        [DataMember(Order = 0, IsRequired = true)]
        public List<PrinterAssociation> PrinterAssociations { get; set; }
        
        [DataMember(Order = 1, IsRequired = true)]
        public List<Printer> Printers { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public List<TerminalGroup> TerminalGroups { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public List<ReportGroup> ReportGroups { get; set; }
    }

    [DataContract(Namespace = "http://Imi.SupplyChain.OM.Services.OutputHandler.DataContracts/2016/11")]
    public class PrinterAssociation
    {
        [DataMember(Order = 0, IsRequired = true)]
        public string TerminalGroupID { get; set; }

        [DataMember(Order = 1, IsRequired = true)]
        public string ReportGroupID { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public string PrinterID { get; set; }
    }

    [DataContract(Namespace = "http://Imi.SupplyChain.OM.Services.OutputHandler.DataContracts/2016/11")]
    public class TerminalGroup
    {
        [DataMember(Order = 0, IsRequired = true)]
        public string TerminalGroupID { get; set; }

        [DataMember(Order = 1, IsRequired = false)]
        public List<string> Terminals { get; set; }
    }

    [DataContract(Namespace = "http://Imi.SupplyChain.OM.Services.OutputHandler.DataContracts/2016/11")]
    public class ReportGroup
    {
        [DataMember(Order = 0, IsRequired = true)]
        public string ReportGroupID { get; set; }

        [DataMember(Order = 1, IsRequired = false)]
        public List<string> DocumentTypesWithSubDocType { get; set; }
    }

    [DataContract(Namespace = "http://Imi.SupplyChain.OM.Services.OutputHandler.DataContracts/2016/11")]
    public class Printer
    {
        [DataMember(Order = 0, IsRequired = true)]
        public string PrinterID { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public string PrinterType { get; set; }

        [DataMember(Order = 4, IsRequired = false)]
        public string PrinterDeviceName { get; set; }
    }
}