using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System.Configuration;

namespace Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess
{
    public class ConfigDataHandler
    {
        private static ConfigDataHandler _InternalInstance = null;
        public static ConfigDataHandler GetConfigDataHandlerInstance(string outputManagerID, System.Diagnostics.TraceSource trace)
        {
            if (!string.IsNullOrEmpty(outputManagerID) && trace != null)
            {
                _InternalInstance = new ConfigDataHandler(outputManagerID, trace);
            }

            return _InternalInstance;
        }

        private const string schemaName = "OMUSER";

        private DateTime? _lastUpdateTime = null;
        private IConfigDao _configDataAccess = null;
        private string _outputManagerID = string.Empty;

        private IDictionary<string, Terminal> _terminals;
        private IDictionary<string, IDictionary<string, object>> _documentTypes;
        private IDictionary<string, Report> _reports;
        private IDictionary<string, Printer> _printers;
        private IDictionary<string, IDictionary<string, string>> _terminalGrp_ReportGrp_PrinterID_Dic;
        private IDictionary<string, Adapter> _adapters;

        private ReaderWriterLockSlim _configurationLock = new ReaderWriterLockSlim();

        private ReaderWriterLockSlim _adapterConfigLock = new ReaderWriterLockSlim();
        private List<string> _adapterConfigUpdated = new List<string>();
        private Dictionary<string, List<string>> _adapterReportUpdated = new Dictionary<string, List<string>>();

        private System.Diagnostics.TraceSource _trace;

        public ConfigDataHandler(string outputManagerID, System.Diagnostics.TraceSource trace)
        {
            _trace = trace;

            _outputManagerID = outputManagerID;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            _configDataAccess = PolicyInjection.Create<ConfigDao_Oracle, IConfigDao>(connectionString);

            UpdateConfiguration(true);

            ThreadPool.QueueUserWorkItem(UpdateCheckerThread);
        }

        public void UpdateServiceURL(string serviceURL, bool mainService)
        {
            _configDataAccess.UpdateOM_URL(_outputManagerID, mainService, serviceURL);
        }

        public void LogError(string errorMessage)
        {
            try
            {
                _configDataAccess.LoggError(_outputManagerID, errorMessage);
            }
            catch (Exception ex)
            {
                _trace.TraceEvent(System.Diagnostics.TraceEventType.Error, 0, "\r\n" + ex.Message);
            }
        }

        public bool CheckAdapterConfigurationUpdated(string adapterID)
        {
            _adapterConfigLock.EnterReadLock();
            try
            {
                if (_adapterConfigUpdated.Contains(adapterID))
                {
                    _adapterConfigUpdated.Remove(adapterID);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                _adapterConfigLock.ExitReadLock();
            }
        }

        public Dictionary<string, string> GetAdapterConfigurationParameters(string adapterID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            _adapterConfigLock.EnterReadLock();
            try
            {
                if (_adapters.ContainsKey(adapterID))
                {
                    foreach (KeyValuePair<string, string> parameter in _adapters[adapterID].AdapterParameters)
                    {
                        parameters.Add(parameter.Key, parameter.Value);
                    }
                }

                return parameters;
            }
            finally
            {
                _adapterConfigLock.ExitReadLock();
            }
        }

        public List<string> GetUpdatedReportsForAdapter(string adapterID)
        {
            _configurationLock.EnterReadLock();
            try
            {
                if (_adapterReportUpdated.ContainsKey(adapterID))
                {
                    List<string> reports = _adapterReportUpdated[adapterID];
                    _adapterReportUpdated.Remove(adapterID);
                    return reports;
                }
                else
                {
                    return new List<string>();
                }
            }
            finally
            {
                _configurationLock.ExitReadLock();
            }
        }

        public Dictionary<string, byte[]> GetReports_ReportFiles(string adapterID)
        {
            Dictionary<string, byte[]> result = new Dictionary<string, byte[]>();

            foreach (Report report in _reports.Values)
            {
                if (report.AdapterIDs.Contains(adapterID))
                {
                    if (!result.ContainsKey(report.ReportID))
                    {
                        result.Add(report.ReportID, report.ReportFile);
                    }
                }
            }

            return result;
        }

        public List<string> GetAdapterID_PrinterDeviceName_PrinterID_ReportID_ReportFile(string terminalID, string documentTypeID, string subDocumentTypeID, string printerIDOverride, out string printerID, out string printerDeviceName, out string reportID, out byte[] reportFile, out string errorText)
        {
            List<string> AdapterIDs = new List<string>();
            printerDeviceName = string.Empty;
            printerID = string.Empty;
            reportFile = null;
            reportID = string.Empty;
            errorText = string.Empty;

            _configurationLock.EnterReadLock();
            try
            {
                if (!string.IsNullOrEmpty(terminalID) && !string.IsNullOrEmpty(documentTypeID))
                {
                    if (_documentTypes.ContainsKey(documentTypeID))
                    {
                        if (string.IsNullOrEmpty(subDocumentTypeID))
                        {
                            reportID = (string)_documentTypes[documentTypeID]["REPORTID"];
                        }
                        else
                        {
                            Dictionary<string, string> subDocReportDic = ((Dictionary<string, string>)_documentTypes[documentTypeID]["SUBDOCTYPES"]);

                            if (subDocReportDic.ContainsKey(subDocumentTypeID))
                            {
                                reportID = subDocReportDic[subDocumentTypeID];
                            }
                            else
                            {
                                errorText += "\r\n\r\nThe Sub Document Type: [" + subDocumentTypeID + "] does not exist.";
                            }
                        }

                        if (!string.IsNullOrEmpty(reportID))
                        {
                            if (_reports.ContainsKey(reportID))
                            {
                                AdapterIDs = _reports[reportID].AdapterIDs;
                                reportFile = _reports[reportID].ReportFile;

                                if (!string.IsNullOrEmpty(printerIDOverride)) //Printer override
                                {
                                    if (_printers.ContainsKey(printerIDOverride))
                                    {
                                        printerDeviceName = _printers[printerIDOverride].PrinterDevice;
                                        printerID = printerIDOverride;
                                    }
                                    else
                                    {
                                        errorText += "\r\n\r\nThe Printer: [" + printerIDOverride + "] does not exist.";
                                        printerID = printerIDOverride;
                                    }
                                }
                                else
                                {

                                    string reportGroupID = _reports[reportID].ReportGroupID;

                                    if (!string.IsNullOrEmpty(reportGroupID))
                                    {
                                        string terminalGroupID = string.Empty; //Possible Default Value

                                        if (_terminals.ContainsKey(terminalID))
                                        {
                                            terminalGroupID = _terminals[terminalID].TerminalGroupID;
                                        }
                                        else
                                        {
                                            errorText += "\r\n\r\nThe Terminal: [" + terminalID + "] does not exist.";
                                        }

                                        if (!string.IsNullOrEmpty(terminalGroupID))
                                        {
                                            if (_terminalGrp_ReportGrp_PrinterID_Dic.ContainsKey(terminalGroupID))
                                            {
                                                if (_terminalGrp_ReportGrp_PrinterID_Dic[terminalGroupID].ContainsKey(reportGroupID))
                                                {
                                                    string associatedPrinterID = _terminalGrp_ReportGrp_PrinterID_Dic[terminalGroupID][reportGroupID];

                                                    if (_printers.ContainsKey(associatedPrinterID))
                                                    {
                                                        printerDeviceName = _printers[associatedPrinterID].PrinterDevice;
                                                        printerID = associatedPrinterID;
                                                    }
                                                    else
                                                    {
                                                        errorText += "\r\n\r\nThe Printer: [" + associatedPrinterID + "] does not exist.";
                                                    }
                                                }
                                                else
                                                {
                                                    errorText += "\r\n\r\nCould not find printer device name for TerminalGroup: [" + terminalGroupID + "] ReportGroup: [" + reportGroupID + "] OutputManagerID: [" + _outputManagerID + "]";
                                                }
                                            }
                                            else
                                            {
                                                errorText += "\r\n\r\nCould not find printer device name for TerminalGroup: [" + terminalGroupID + "] ReportGroup: [" + reportGroupID + "] OutputManagerID: [" + _outputManagerID + "]";
                                            }
                                        }
                                        else
                                        {
                                            errorText += "\r\n\r\nThe Terminal: [" + terminalID + "] is not connected to a Terminal Group.";
                                        }
                                    }
                                    else
                                    {
                                        errorText += "\r\n\r\nThe Report: [" + reportID + "] is not connected to a Report Group.";
                                    }
                                }
                            }
                            else
                            {
                                errorText += "\r\n\r\nThe Report: [" + reportID + "] does not exist.";
                            }
                        }
                        else
                        {
                            errorText += "\r\n\r\nNo Report found.";
                        }
                    }
                    else
                    {
                        errorText += "\r\n\r\nThe Document Type: [" + documentTypeID + "] does not exist.";
                    }
                }
            }
            finally
            {
                _configurationLock.ExitReadLock();
            }

            return AdapterIDs;
        }

        private void UpdateCheckerThread(object state)
        {
            int faults = 0;
            while (true)
            {
                try
                {
                    DateTime? upddtm = _configDataAccess.FindConfigUpdateTime(_outputManagerID);

                    if (upddtm != null)
                    {
                        if (_lastUpdateTime == null || upddtm.Value > _lastUpdateTime.Value)
                        {
                            try
                            {
                                UpdateConfiguration();
                                _lastUpdateTime = upddtm;
                            }
                            catch (Exception ex)
                            {
                                LogError(ex.Message);
                            }
                        }
                    }

                    faults = 0;
                }
                catch (Oracle.DataAccess.Client.OracleException ex)
                {
                    LogError(ex.Message);
                    faults++;
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                    faults++;
                }

                if (faults < 5) //Less than 5 faults in a row. All is ok.
                {
                    Thread.Sleep(10000);
                }
                else //More than 5 faults in a row. Something is not right. Maybe DB connection lost. Wait longer before next try.
                {
                    Thread.Sleep(300000);
                }
            }
        }

        private void UpdateConfiguration(bool startUp = false)
        {
            IDictionary<string, Terminal> terminals = null;
            IList<DocumentType> documentTypes = null;
            IDictionary<string, Report> reports = null;
            IDictionary<string, Printer> printers = null;
            IList<PrinterAssociation> printerAssociations = null;
            IDictionary<string, Adapter> adapters = null;

            try
            {
                terminals = _configDataAccess.FindTerminal();
                documentTypes = _configDataAccess.FindDocumentType();
                reports = _configDataAccess.FindReport();
                printers = _configDataAccess.FindPrinter(_outputManagerID);
                printerAssociations = _configDataAccess.FindPrinterAssociation(_outputManagerID);
                adapters = _configDataAccess.FindAdapter(_outputManagerID);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }

            _configurationLock.EnterWriteLock();
            try
            {
                _terminals = terminals;
                _printers = printers;

                if (!startUp)
                {
                    foreach (Report report in reports.Values)
                    {
                        if (_reports.ContainsKey(report.ReportID))
                        {
                            if (report.LatestUpdate > _reports[report.ReportID].LatestUpdate)
                            {
                                foreach (string adapterID in report.AdapterIDs)
                                {
                                    if (!_adapterReportUpdated.ContainsKey(adapterID))
                                    {
                                        _adapterReportUpdated.Add(adapterID, new List<string>());
                                    }

                                    if (!_adapterReportUpdated[adapterID].Contains(report.ReportID))
                                    {
                                        _adapterReportUpdated[adapterID].Add(report.ReportID);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (string adapterID in report.AdapterIDs)
                            {
                                if (!_adapterReportUpdated.ContainsKey(adapterID))
                                {
                                    _adapterReportUpdated.Add(adapterID, new List<string>());
                                }

                                if (!_adapterReportUpdated[adapterID].Contains(report.ReportID))
                                {
                                    _adapterReportUpdated[adapterID].Add(report.ReportID);
                                }
                            }
                        }
                    }

                    //Deleted reports
                    foreach (Report report in _reports.Values)
                    {
                        if (!reports.ContainsKey(report.ReportID))
                        {
                            foreach (string adapterID in report.AdapterIDs)
                            {
                                if (!_adapterReportUpdated.ContainsKey(adapterID))
                                {
                                    _adapterReportUpdated.Add(adapterID, new List<string>());
                                }

                                if (!_adapterReportUpdated[adapterID].Contains(report.ReportID))
                                {
                                    _adapterReportUpdated[adapterID].Add(report.ReportID);
                                }
                            }
                        }
                    }
                }
                _reports = reports;


                _documentTypes = new Dictionary<string, IDictionary<string, object>>();
                foreach (DocumentType docType in documentTypes)
                {
                    if (!_documentTypes.ContainsKey(docType.DocumentTypeID))
                    {
                        _documentTypes.Add(docType.DocumentTypeID, new Dictionary<string, object>());
                        _documentTypes[docType.DocumentTypeID].Add("REPORTID", string.Empty);
                        _documentTypes[docType.DocumentTypeID].Add("SUBDOCTYPES", new Dictionary<string, string>());
                    }

                    if (string.IsNullOrEmpty(docType.SubDocmentTypeID))
                    {
                        _documentTypes[docType.DocumentTypeID]["REPORTID"] = docType.ReportID;
                    }
                    else
                    {
                        Dictionary<string, string> subDocReportDic = ((Dictionary<string, string>)_documentTypes[docType.DocumentTypeID]["SUBDOCTYPES"]);

                        if (!subDocReportDic.ContainsKey(docType.SubDocmentTypeID))
                        {
                            subDocReportDic.Add(docType.SubDocmentTypeID, docType.ReportID);
                        }
                    }

                }

                _terminalGrp_ReportGrp_PrinterID_Dic = new Dictionary<string, IDictionary<string, string>>();
                foreach (PrinterAssociation printAss in printerAssociations)
                {
                    if (!_terminalGrp_ReportGrp_PrinterID_Dic.ContainsKey(printAss.TerminalGroupID))
                    {
                        _terminalGrp_ReportGrp_PrinterID_Dic.Add(printAss.TerminalGroupID, new Dictionary<string, string>());
                    }

                    if (!_terminalGrp_ReportGrp_PrinterID_Dic[printAss.TerminalGroupID].ContainsKey(printAss.ReportGroupID))
                    {
                        _terminalGrp_ReportGrp_PrinterID_Dic[printAss.TerminalGroupID].Add(printAss.ReportGroupID, printAss.PrinterID);
                    }
                }

            }
            finally
            {
                _configurationLock.ExitWriteLock();
            }

            if (startUp)
            {
                _adapters = adapters;
            }
            else
            {
                _adapterConfigLock.EnterWriteLock();
                try
                {
                    foreach (Adapter adapter in adapters.Values)
                    {
                        if (!_adapters.ContainsKey(adapter.AdapterID))
                        {
                            _adapters.Add(adapter.AdapterID, adapter);

                            if (!_adapterConfigUpdated.Contains(adapter.AdapterID))
                            {
                                _adapterConfigUpdated.Add(adapter.AdapterID);
                            }
                        }
                        else
                        {
                            if (adapter.LatestUpdate > _adapters[adapter.AdapterID].LatestUpdate)
                            {
                                _adapters[adapter.AdapterID] = adapter;

                                if (!_adapterConfigUpdated.Contains(adapter.AdapterID))
                                {
                                    _adapterConfigUpdated.Add(adapter.AdapterID);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    _adapterConfigLock.ExitWriteLock();
                }
            }
        }

        public struct PrinterInformation
        {
            public string PrinterID;
            public string PrinterDeviceName;
            public string PrinterType;
        }

        public struct PrinterAssociationInformation
        {
            public string TerminalGroupID;
            public string ReportGroupID;
            public string PrinterID;
        }

        public void GetPrinterInformation(out Dictionary<string, List<string>> terminalGroups, out Dictionary<string, List<string>> reportGroups, out List<PrinterInformation> printers, out List<PrinterAssociationInformation> printerAssociations)
        {
            terminalGroups = new Dictionary<string, List<string>>();
            reportGroups = new Dictionary<string, List<string>>();
            printers = new List<PrinterInformation>();
            printerAssociations = new List<PrinterAssociationInformation>();

            foreach (Terminal ter in _terminals.Values)
            {
                if (!string.IsNullOrEmpty(ter.TerminalGroupID))
                {
                    if (!terminalGroups.ContainsKey(ter.TerminalGroupID))
                    {
                        terminalGroups.Add(ter.TerminalGroupID, new List<string>());
                    }

                    terminalGroups[ter.TerminalGroupID].Add(ter.TerminalID);
                }
            }


            foreach (KeyValuePair<string, IDictionary<string, object>> documentType in _documentTypes)
            {
                string docType = documentType.Key;

                if (!string.IsNullOrEmpty(documentType.Value["REPORTID"].ToString()))
                {
                    string reportGroup = _reports[documentType.Value["REPORTID"].ToString()].ReportGroupID;

                    if (!string.IsNullOrEmpty(reportGroup))
                    {
                        if (!reportGroups.ContainsKey(reportGroup))
                        {
                            reportGroups.Add(reportGroup, new List<string>());
                        }

                        reportGroups[reportGroup].Add(docType);
                    }
                }

                Dictionary<string, string> subDocReportDic = ((Dictionary<string, string>)documentType.Value["SUBDOCTYPES"]);

                foreach (KeyValuePair<string, string> subDocType_Rpt in subDocReportDic)
                {
                    string sugDocType = subDocType_Rpt.Key;
                    string subRptGrp = _reports[subDocType_Rpt.Value].ReportGroupID;

                    if (!string.IsNullOrEmpty(subRptGrp))
                    {
                        if (!reportGroups.ContainsKey(subRptGrp))
                        {
                            reportGroups.Add(subRptGrp, new List<string>());
                        }

                        reportGroups[subRptGrp].Add(string.Format("{0}|{1}", docType, sugDocType));
                    }
                }
            }


            foreach (Printer prt in _printers.Values)
            {
                printers.Add(new PrinterInformation() { PrinterID = prt.PrinterID, PrinterDeviceName = prt.PrinterDevice, PrinterType = prt.PrinterType });
            }

            foreach (KeyValuePair<string, IDictionary<string, string>> terGrp_RptGrpPrtDic in _terminalGrp_ReportGrp_PrinterID_Dic)
            {
                foreach (KeyValuePair<string, string> rptGrp_Prt in terGrp_RptGrpPrtDic.Value)
                {
                    printerAssociations.Add(new PrinterAssociationInformation() { TerminalGroupID = terGrp_RptGrpPrtDic.Key, ReportGroupID = rptGrp_Prt.Key, PrinterID = rptGrp_Prt.Value });
                }
            }

        }

    }
}
