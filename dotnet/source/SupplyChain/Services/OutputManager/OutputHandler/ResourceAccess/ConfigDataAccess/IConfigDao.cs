using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess
{
    internal interface IConfigDao
    {
        IDictionary<string,Terminal> FindTerminal();
        IList<DocumentType> FindDocumentType();
        IDictionary<string, Report> FindReport();
        IDictionary<string, Printer> FindPrinter(string outputManagerID);
        IList<PrinterAssociation> FindPrinterAssociation(string outputManagerID);
        IDictionary<string, Adapter> FindAdapter(string outputManagerID);
        
        DateTime? FindConfigUpdateTime(string outputManagerID);

        void LoggError(string outputManagerID, string errorMessage);

        void UpdateOM_URL(string outputManagerID, bool mainService, string URL);
    }

    internal class Terminal
    {
        public string TerminalID { get; set; }
        public string TerminalGroupID { get; set; }
        public DateTime LatestUpdate { get; set; }
        }

    internal class DocumentType
    {
        public string DocumentTypeID { get; set; }
        public string SubDocmentTypeID { get; set; }
        public string ReportID { get; set; }
        public DateTime LatestUpdate { get; set; }
    }

    internal class Report
    {
        public string ReportID { get; set; }
        public string ReportGroupID { get; set; }
        public byte[] ReportFile { get; set; }
        public List<string> AdapterIDs { get; set; }
        public DateTime LatestUpdate { get; set; }
    }

    internal class Printer
    {
        public string PrinterID { get; set; }
        public string PrinterDevice { get; set; }
        public string PrinterType { get; set; }
        public DateTime LatestUpdate { get; set; }
    }

    internal class PrinterAssociation
    {
        public string PrinterID { get; set; }
        public string ReportGroupID { get; set; }
        public string TerminalGroupID { get; set; }
        public DateTime LatestUpdate { get; set; }
    }

    internal class Adapter
    {
        public string AdapterID { get; set; }
        public Dictionary<string, string> AdapterParameters { get; set; }
        public DateTime LatestUpdate { get; set; }
    }
}
