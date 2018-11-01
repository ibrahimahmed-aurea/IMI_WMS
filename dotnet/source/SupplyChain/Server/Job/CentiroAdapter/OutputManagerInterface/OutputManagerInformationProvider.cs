using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Imi.SupplyChain.Server.Job.CentiroAdapter.OraclePackage;
using System.ServiceModel;
using System.Diagnostics;

namespace Imi.SupplyChain.Server.Job.CentiroAdapter.OutputManagerInterface
{
    public class OutputManagerInformationProvider
    {
        private TraceSource _tracing;
        private PrintQueue _printQueue;
        private Dictionary<string, OutputManagerConfig> _warehouseOMConfiguration = new Dictionary<string, OutputManagerConfig>();
        private string _endpointName;

        private class Warehouse_OutputManager_Config
        {
            public string WarehouseId;
            public string OutputManagerId;
            public List<string> orderdURLs;
        }

        private class OutputManagerConfig
        {
            public struct PrinterInfo
            {
                public string PrinterID;
                public string PrinterDeviceName;
                public string PrinterType;
            }

            public string WarehouseOMID;

            public Dictionary<string, string> TerminalsWithGroup = new Dictionary<string, string>();
            public Dictionary<string, string> DocumentTypesWithReportGroup = new Dictionary<string, string>();
            public Dictionary<string, PrinterInfo> Printers = new Dictionary<string, PrinterInfo>();
            public Dictionary<string, Dictionary<string, PrinterInfo>> TerGrp_RptGrp_PrtInfo = new Dictionary<string, Dictionary<string, PrinterInfo>>();


            public string GetPrinterType(string terminalID, string documentType, string subDocumentType, out string error)
            {
                string printerType = string.Empty;
                error = string.Empty;

                if (TerminalsWithGroup.ContainsKey(terminalID))
                {
                    string terGrp = TerminalsWithGroup[terminalID];
                    string docTypeSearch = documentType;
                    if (!string.IsNullOrEmpty(subDocumentType))
                    {
                        docTypeSearch += "|" + subDocumentType;
                    }

                    if (DocumentTypesWithReportGroup.ContainsKey(docTypeSearch))
                    {
                        string rptGrp = DocumentTypesWithReportGroup[docTypeSearch];

                        if (TerGrp_RptGrp_PrtInfo.ContainsKey(terGrp))
                        {
                            if (TerGrp_RptGrp_PrtInfo[terGrp].ContainsKey(rptGrp))
                            {
                                printerType = TerGrp_RptGrp_PrtInfo[terGrp][rptGrp].PrinterType;
                            }
                            else
                            {
                                error = "No associated printer found in OutputManager configuration for ReportGroup: [" + rptGrp + "] with TerminalGroup: [" + terGrp + "]"; 
                            }
                        }
                        else
                        {
                            error = "No associated printer found in OutputManager configuration for TerminalGroup: [" + terGrp + "]";
                        }
                    }
                    else
                    {
                        error = "No ReportGroup found in OutputManager configuration for DocumentType: [" + documentType + "] and DocumentSubType: [" + subDocumentType + "].";
                    }
                }
                else
                {
                    error = "Terminal: [" + terminalID + "] not found in OutputManager configuration for OM: [" + WarehouseOMID + "].";
                }

                return printerType;
            }
        }

        public OutputManagerInformationProvider(PrintQueue printQueue, TraceSource tracing, string endpointName)
        {
            _tracing = tracing;
            _printQueue = printQueue;
            _endpointName = endpointName;

            UpdateOutputManagerInformation();
        }


        public string GetPrinterTypeFromOutputManager(string WarehouseID, string TerminalID, string DocumentType, string DocumentSubType, out string Error)
        {
            string printerType = string.Empty;

            lock (_warehouseOMConfiguration)
            {
                if (_warehouseOMConfiguration.ContainsKey(WarehouseID))
                {
                    OutputManagerConfig omConf = _warehouseOMConfiguration[WarehouseID];

                    printerType = omConf.GetPrinterType(TerminalID, DocumentType, DocumentSubType, out Error);
                }
                else
                {
                    Error = "No OutputManager configuration exists for Warehouse: [" + WarehouseID + "]";
                }
            }

            return printerType;
        }


        private void UpdateOutputManagerInformation()
        {
            Dictionary<string, Warehouse_OutputManager_Config> WarehouseOMConfig = GetWarehouseOMConfig();

            Dictionary<string, OutputManagerConfig> outputManagerConfigurationDic = new Dictionary<string, OutputManagerConfig>();
            Dictionary<string, OutputManagerConfig> warehouseOMConfiguration = new Dictionary<string, OutputManagerConfig>();

            foreach (Warehouse_OutputManager_Config WHOMconf in WarehouseOMConfig.Values)
            {
                if (!outputManagerConfigurationDic.ContainsKey(WHOMconf.OutputManagerId))
                {
                    OutputManagerConfig OMconf = GetOutputManagerPrinterConfig(WHOMconf);

                    if (OMconf != null)
                    {
                        outputManagerConfigurationDic.Add(OMconf.WarehouseOMID, OMconf);
                    }
                    else
                    {
                        _tracing.TraceEvent(TraceEventType.Error, 0, "Could not contact OutputManager service(s) for OMID: [" + WHOMconf.OutputManagerId + "]");
                    }
                }

                if (outputManagerConfigurationDic.ContainsKey(WHOMconf.OutputManagerId))
                {
                    if (!warehouseOMConfiguration.ContainsKey(WHOMconf.WarehouseId))
                    {
                        warehouseOMConfiguration.Add(WHOMconf.WarehouseId, outputManagerConfigurationDic[WHOMconf.OutputManagerId]);
                    }
                }
            }

            lock (_warehouseOMConfiguration)
            {
                _warehouseOMConfiguration = warehouseOMConfiguration;
            }
        }

        private Dictionary<string, Warehouse_OutputManager_Config> GetWarehouseOMConfig()
        {
            Dictionary<string, Warehouse_OutputManager_Config> configuration = new Dictionary<string, Warehouse_OutputManager_Config>();

            IDataReader reader;

            _printQueue.Getprintconf(out reader);

            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        Warehouse_OutputManager_Config conf = new Warehouse_OutputManager_Config();
                        conf.orderdURLs = new List<string>();

                        if (reader["WHID"] == DBNull.Value)
                            conf.WarehouseId = "";
                        else
                            conf.WarehouseId = reader["WHID"] as String;

                        if (reader["OMID"] == DBNull.Value)
                            conf.WarehouseId = "";
                        else
                            conf.OutputManagerId = reader["OMID"] as String;

                        if (reader["MAIN_URL"] != DBNull.Value)
                            conf.orderdURLs.Add(reader["MAIN_URL"] as String);

                        if (reader["FALLBACK_URL"] != DBNull.Value)
                            conf.orderdURLs.Add(reader["FALLBACK_URL"] as String);


                        if (!string.IsNullOrEmpty(conf.WarehouseId))
                        {
                            configuration.Add(conf.WarehouseId, conf);
                        }

                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return configuration;
        }

        private OutputManagerConfig GetOutputManagerPrinterConfig(Warehouse_OutputManager_Config omToCall)
        {
            OutputManagerService.OutputHandlerServiceClient client = new OutputManagerService.OutputHandlerServiceClient(_endpointName);

            foreach (string urlToCall in omToCall.orderdURLs)
            {
                bool iOK = true;
                try
                {
                    EndpointAddress currentEndpoint = client.Endpoint.Address;

                    EndpointAddress address = new EndpointAddress(new Uri(urlToCall), currentEndpoint.Identity, currentEndpoint.Headers);

                    client.Endpoint.Address = address;

                    OutputManagerService.FindPrinterInfoResult omResult = client.FindPrinterInfo(null);

                    client.Close();

                    OutputManagerConfig newOMConfig = new OutputManagerConfig();

                    newOMConfig.WarehouseOMID = omToCall.WarehouseId;

                    foreach (OutputManagerService.TerminalGroup terGrp in omResult.TerminalGroups)
                    {
                        foreach (string terminal in terGrp.Terminals)
                        {
                            if (!newOMConfig.TerminalsWithGroup.ContainsKey(terminal))
                            {
                                newOMConfig.TerminalsWithGroup.Add(terminal, terGrp.TerminalGroupID);
                            }
                        }
                    }

                    foreach (OutputManagerService.ReportGroup rptGrp in omResult.ReportGroups)
                    {
                        foreach (string docTypeAndSub in rptGrp.DocumentTypesWithSubDocType)
                        {
                            if (!newOMConfig.DocumentTypesWithReportGroup.ContainsKey(docTypeAndSub))
                            {
                                newOMConfig.DocumentTypesWithReportGroup.Add(docTypeAndSub, rptGrp.ReportGroupID);
                            }
                        }
                    }

                    foreach (OutputManagerService.Printer prt in omResult.Printers)
                    {
                        if (!newOMConfig.Printers.ContainsKey(prt.PrinterID))
                        {
                            newOMConfig.Printers.Add(prt.PrinterID, new OutputManagerConfig.PrinterInfo() { PrinterID = prt.PrinterID, PrinterDeviceName = prt.PrinterDeviceName, PrinterType = prt.PrinterType });
                        }
                    }

                    foreach (OutputManagerService.PrinterAssociation prtAssoc in omResult.PrinterAssociations)
                    {
                        if (newOMConfig.Printers.ContainsKey(prtAssoc.PrinterID))
                        {
                            OutputManagerConfig.PrinterInfo printerInfo = newOMConfig.Printers[prtAssoc.PrinterID];

                            if (!newOMConfig.TerGrp_RptGrp_PrtInfo.ContainsKey(prtAssoc.TerminalGroupID))
                            {
                                newOMConfig.TerGrp_RptGrp_PrtInfo.Add(prtAssoc.TerminalGroupID, new Dictionary<string, OutputManagerConfig.PrinterInfo>());
                            }

                            if (!newOMConfig.TerGrp_RptGrp_PrtInfo[prtAssoc.TerminalGroupID].ContainsKey(prtAssoc.ReportGroupID))
                            {
                                newOMConfig.TerGrp_RptGrp_PrtInfo[prtAssoc.TerminalGroupID].Add(prtAssoc.ReportGroupID, printerInfo); 
                            }
                        }
                    }

                    return newOMConfig;
                }
                catch
                {
                    iOK = false;
                }

                if (iOK) { break; }
            }

            return null;
        }
    }
}
