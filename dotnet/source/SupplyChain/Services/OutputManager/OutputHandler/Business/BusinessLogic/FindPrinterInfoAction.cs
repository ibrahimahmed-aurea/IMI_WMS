using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.OM.Services.OutputHandler.DataContracts;

namespace Imi.SupplyChain.OM.OutputHandler.BusinessLogic
{
    public class FindPrinterInfoAction : MarshalByRefObject
    {
        public FindPrinterInfoResult Execute(FindPrinterInfoParameters parameters)
        {
            FindPrinterInfoResult result = new FindPrinterInfoResult();
            result.PrinterAssociations = new List<PrinterAssociation>();
            result.Printers = new List<Printer>();
            result.ReportGroups = new List<ReportGroup>();
            result.TerminalGroups = new List<TerminalGroup>();

            ConfigDataAccess.ConfigDataHandler configDataHandler = ConfigDataAccess.ConfigDataHandler.GetConfigDataHandlerInstance(string.Empty, null);
            
            List<Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess.ConfigDataHandler.PrinterInformation> printers;
            List<Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess.ConfigDataHandler.PrinterAssociationInformation> printerAssociations;
            Dictionary<string, List<string>> terminalGroups;
            Dictionary<string, List<string>> ReportGroups;

            configDataHandler.GetPrinterInformation(out terminalGroups, out ReportGroups, out printers, out printerAssociations);

            foreach (KeyValuePair<string, List<string>> terGrp in terminalGroups)
            {
                result.TerminalGroups.Add(new TerminalGroup() { TerminalGroupID = terGrp.Key, Terminals = terGrp.Value });
            }

            foreach (KeyValuePair<string, List<string>> rptGrp in ReportGroups)
            {
                result.ReportGroups.Add(new ReportGroup() { ReportGroupID = rptGrp.Key, DocumentTypesWithSubDocType = rptGrp.Value });
            }

            foreach (Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess.ConfigDataHandler.PrinterInformation prt in printers)
            {
                result.Printers.Add(new Printer() { PrinterID = prt.PrinterID, PrinterDeviceName = prt.PrinterDeviceName, PrinterType = prt.PrinterType });
            }

            foreach (Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess.ConfigDataHandler.PrinterAssociationInformation prtAssoc in printerAssociations)
            {
                result.PrinterAssociations.Add(new PrinterAssociation() { TerminalGroupID = prtAssoc.TerminalGroupID, ReportGroupID = prtAssoc.ReportGroupID, PrinterID = prtAssoc.PrinterID });
            }

            return result;
        }
    }
}
