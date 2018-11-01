using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.OM.Services.OutputHandler.DataContracts;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System.IO;
using System.Reflection;
using Imi.SupplyChain.OM.OutputHandler.ResourceAccess;
using System.Threading;
using System.Xml;
using System.Configuration;

namespace Imi.SupplyChain.OM.OutputHandler.BusinessLogic
{
    public class CreateOutputAction : MarshalByRefObject
    {
        private string _outputManagerID;
        private bool _mainService;
        private ConfigDataAccess.ConfigDataHandler _configDataHandler;
        private Dictionary<string, IAdapter> _adapters;
        private object _checkAdapterConfigLock = new object();
        private Dictionary<string, ReaderWriterLockSlim> _adapterLockDictionary;
        private Dictionary<string, ReaderWriterLockSlim> _printerLockDictionary;

        public CreateOutputAction()
        {
            System.Diagnostics.TraceSource trace = new System.Diagnostics.TraceSource(typeof(CreateOutputAction).Name);
            trace.Switch.Level = System.Diagnostics.SourceLevels.Error;
            string loggPath = Path.GetDirectoryName(Imi.Framework.Job.Configuration.InstanceConfig.CurrentInstance.Log.FileName);
            string loggFile = Path.Combine(loggPath, typeof(CreateOutputAction).FullName + ".log");
            Imi.Framework.Shared.Diagnostics.RollingFileTraceListener listener = new Imi.Framework.Shared.Diagnostics.RollingFileTraceListener(loggFile, 512000);
            trace.Listeners.Add(listener);

            try
            {
                _adapterLockDictionary = new Dictionary<string, ReaderWriterLockSlim>();
                _printerLockDictionary = new Dictionary<string, ReaderWriterLockSlim>();

                _outputManagerID = ConfigurationManager.AppSettings["OutputManagerID"];
                _mainService = Convert.ToBoolean(ConfigurationManager.AppSettings["MainService"]);

                if (string.IsNullOrEmpty(_outputManagerID))
                {
                    trace.TraceEvent(System.Diagnostics.TraceEventType.Error, 0, "\r\n Could not find AppSettings value for OutputManagerID");
                }

                _configDataHandler = ConfigDataAccess.ConfigDataHandler.GetConfigDataHandlerInstance(_outputManagerID, trace);
            }
            catch (Exception ex)
            {
                trace.TraceEvent(System.Diagnostics.TraceEventType.Error, 0, "\r\n" + ex.Message);
            }

            _adapters = new Dictionary<string, IAdapter>();

            CreateAdapters();

            ThreadPool.QueueUserWorkItem(CheckAndUpdateAdaptersThread);
        }

        public void UpdateURL(string serviceURL)
        {
            _configDataHandler.UpdateServiceURL(serviceURL, _mainService);
        }

        private void CreateAdapters()
        {
            try
            {
                string uniqueOutputManagerIdentity = _outputManagerID + (_mainService ? "_M" : "_F");
                //Create Adapters
                Uri uri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));


                foreach (string filepath in Directory.GetFiles(uri.LocalPath, "Imi.SupplyChain.OM.OutputHandler.Adapters.*.dll"))
                {
                    try
                    {
                        FileInfo f = new FileInfo(filepath);

                        Assembly assemb = Assembly.LoadFile(f.FullName);

                        Type assemblyType = null;

                        foreach (Type typeinAssemb in assemb.GetTypes())
                        {
                            if (typeof(IAdapter).IsAssignableFrom(typeinAssemb))
                            {
                                assemblyType = typeinAssemb;
                                break;
                            }
                        }

                        if (assemblyType != null)
                        {
                            IAdapter adapter = (IAdapter)assemb.CreateInstance(assemblyType.FullName);

                            if (!_adapters.ContainsKey(adapter.AdapterIdentity))
                            {
                                string errorText = string.Empty;
                                Dictionary<string, string> configParams = _configDataHandler.GetAdapterConfigurationParameters(adapter.AdapterIdentity);
                                Dictionary<string, byte[]> reportFiles = _configDataHandler.GetReports_ReportFiles(adapter.AdapterIdentity);
                                adapter.InitializeAdapter(configParams, reportFiles, uniqueOutputManagerIdentity, out errorText);
                                _adapters.Add(adapter.AdapterIdentity, adapter);

                                if (!string.IsNullOrEmpty(errorText))
                                {
                                    _configDataHandler.LogError("Error loading Adapter [" + adapter.AdapterIdentity + "]: " + errorText);
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        _configDataHandler.LogError("Error loading Adapter:\r\n" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _configDataHandler.LogError(ex.Message);
            }
        }

        public CreateOutputResultCollection Execute(CreateOutputParametersCollection parameters)
        {
            List<OutputDocument> documentsToExecute = new List<OutputDocument>();
            List<OutputDocument> documentsNotExecuted = new List<OutputDocument>();

            foreach (CreateOutputParameters parameter in parameters)
            {
                OutputDocument newDoc = new OutputDocument();

                newDoc.OutputJobId = parameter.OutputJobIdentity;
                newDoc.OutputJobSequence = parameter.OutputJobSequenceNumber;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(parameter.OutputXML);

                newDoc.MetaParameters = new Dictionary<string, string>();
                newDoc.Parameters = new Dictionary<string, string>();

                newDoc.Data = new MemoryStream();
                StreamWriter writer = new StreamWriter(newDoc.Data);
                writer.Write(parameter.OutputXML);
                writer.Flush();

                for (int i = 0; i < doc.ChildNodes.Count; i++)
                {
                    if (doc.ChildNodes[i].NodeType == XmlNodeType.Element)
                    {

                        newDoc.MetaParameters.Add(StdMetaParamNames.DocumentIDKey, doc.ChildNodes[i].Name);

                        foreach (XmlNode node in doc.ChildNodes[i].ChildNodes)
                        {
                            switch (node.Name)
                            {
                                case "MetaData":
                                    foreach (XmlAttribute attribute in node.Attributes)
                                    {
                                        newDoc.MetaParameters.Add(attribute.Name.ToUpper(), attribute.Value.Trim());
                                    }
                                    break;
                                case "Data":
                                    break;
                                case "Parameters":
                                    foreach (XmlNode paramNode in node.ChildNodes)
                                    {
                                        if (paramNode.Name == "Parameter")
                                        {
                                            newDoc.Parameters.Add(paramNode.Attributes["name"].Value, paramNode.Attributes["value"].Value);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }

                //Get configuration
                string configErrorText;

                if (newDoc.MetaParameters.ContainsKey(StdMetaParamNames.DocumentTypeIDKey) && newDoc.MetaParameters.ContainsKey(StdMetaParamNames.SubDocumentTypeIDKey) && newDoc.MetaParameters.ContainsKey(StdMetaParamNames.TerminalIDKey) && newDoc.MetaParameters.ContainsKey(StdMetaParamNames.PrinterIDKey))
                {
                    string reportID;
                    byte[] reportFile;
                    string printerDeviceName;
                    string printerID;
                    List<string> adapterIDs;

                    adapterIDs = _configDataHandler.GetAdapterID_PrinterDeviceName_PrinterID_ReportID_ReportFile(newDoc.MetaParameters[StdMetaParamNames.TerminalIDKey]
                                                                                            , newDoc.MetaParameters[StdMetaParamNames.DocumentTypeIDKey]
                                                                                            , newDoc.MetaParameters[StdMetaParamNames.SubDocumentTypeIDKey]
                                                                                            , newDoc.MetaParameters[StdMetaParamNames.PrinterIDKey]
                                                                                            , out printerID, out printerDeviceName, out reportID, out reportFile, out configErrorText);

                    newDoc.AdapterIDs = adapterIDs;
                    newDoc.PrinterID = printerID;
                    newDoc.PrinterDeviceName = printerDeviceName;
                    newDoc.ReportID = reportID;
                    newDoc.ReportFile = reportFile;
                }
                else
                {
                    configErrorText = "\r\n\r\nDocument Type, Sub Document Type, TerminalID or PrinterID are missing from the document meta data.";
                }

                if (string.IsNullOrEmpty(configErrorText))
                {
                    string nonExistingAdapters = string.Empty;
                    bool allAdaptersExists = true;
                    foreach (string adapterId in newDoc.AdapterIDs)
                    {
                        if (!_adapters.ContainsKey(adapterId))
                        {
                            allAdaptersExists = false;
                            nonExistingAdapters += " [" + adapterId + "] ";
                        }
                    }

                    if (allAdaptersExists)
                    {
                        documentsToExecute.Add(newDoc);
                    }
                    else
                    {
                        newDoc.ErrorDescription = "Adapter(s):" + nonExistingAdapters + "do not exsist.";
                        documentsNotExecuted.Add(newDoc);
                    }
                }
                else
                {
                    newDoc.ErrorDescription += configErrorText;
                    documentsNotExecuted.Add(newDoc);
                }
            }

            Dictionary<string, int> numOfDocsPerPrinter = new Dictionary<string, int>();
            List<string> usedAdapters = new List<string>();

            foreach (OutputDocument doc in documentsToExecute)
            {
                //Collect the printers used.
                if (numOfDocsPerPrinter.ContainsKey(doc.PrinterDeviceName))
                {
                    numOfDocsPerPrinter[doc.PrinterDeviceName]++;
                }
                else
                {
                    numOfDocsPerPrinter.Add(doc.PrinterDeviceName, 1);
                }

                //Get all adapters used in job
                foreach (string adapterId in doc.AdapterIDs)
                {
                    if (!usedAdapters.Contains(adapterId))
                    {
                        usedAdapters.Add(adapterId);
                    }
                }
            }


            //Set sufficient adapter locks
            foreach (string adapterId in usedAdapters)
            {
                AcquireLock(adapterId, false, _adapterLockDictionary);
            }

            try
            {
                //Set sufficient printer locks
                foreach (string printerDeviceName in numOfDocsPerPrinter.Keys)
                {
                    bool exclusive = (numOfDocsPerPrinter[printerDeviceName] > 1);
                    AcquireLock(printerDeviceName, exclusive, _printerLockDictionary);
                }

                try
                {
                    CreateOutputResultCollection resultCollection = new CreateOutputResultCollection();

                    //Process each document
                    foreach (OutputDocument doc in documentsToExecute)
                    {
                        Dictionary<string, object> results = new Dictionary<string, object>();
                        try
                        {
                            //Adding overall results
                            results.Add(StdResultParamNames.PrinterDeviceNameKey, doc.PrinterDeviceName);
                            results.Add(StdResultParamNames.ReportIDKey, doc.ReportID);
                            results.Add(StdResultParamNames.PrinterIDKey, doc.PrinterID);
                            if (doc.MetaParameters.ContainsKey(StdMetaParamNames.UserIdentityKey))
                            {
                                results.Add(StdResultParamNames.UserIDKey, doc.MetaParameters[StdMetaParamNames.UserIdentityKey]);
                            }
                            results.Add(StdResultParamNames.TerminalIDKey, doc.MetaParameters[StdMetaParamNames.TerminalIDKey]);
                            if (doc.MetaParameters.ContainsKey(StdMetaParamNames.NumberOfCopiesKey))
                            {
                                results.Add(StdResultParamNames.NumberOfCopiesKey, doc.MetaParameters[StdMetaParamNames.NumberOfCopiesKey]);
                            }
                            results.Add(StdResultParamNames.SubDocumentTypeIDKey, doc.MetaParameters[StdMetaParamNames.SubDocumentTypeIDKey]);
                            results.Add(StdResultParamNames.DocumentTypeIDKey, doc.MetaParameters[StdMetaParamNames.DocumentTypeIDKey]);

                            foreach (string adapterId in doc.AdapterIDs)
                            {
                                Dictionary<string, object> adapterResults = _adapters[adapterId].Execute(doc, results);

                                if (results.Count == 0)
                                {
                                    results = adapterResults;
                                }
                                else
                                {
                                    foreach (string key in adapterResults.Keys)
                                    {
                                        if (results.ContainsKey(key))
                                        {
                                            results[key] = adapterResults[key];
                                        }
                                        else
                                        {
                                            results.Add(key, adapterResults[key]);
                                        }
                                    }
                                }
                            }

                            doc.Data.Close();
                        }
                        catch (Exception ex)
                        {
                            doc.ErrorDescription += "\r\nError while executing adapters.\r\n" + ex.Message;
                        }

                        CreateOutputResult documentResult = new CreateOutputResult();
                        documentResult.OutputJobIdentity = doc.OutputJobId;
                        documentResult.OutputJobSequenceNumber = doc.OutputJobSequence;
                        documentResult.ErrorDescription = doc.ErrorDescription;
                        documentResult.ResultProperties = new List<CreateOutputResultProperty>();

                        foreach (KeyValuePair<string, object> result in results)
                        {
                            CreateOutputResultProperty resultProperty = new CreateOutputResultProperty(result.Key, result.Value);

                            documentResult.ResultProperties.Add(resultProperty);
                        }

                        resultCollection.Add(documentResult);

                    }

                    //Return error for non processed documents
                    foreach (OutputDocument doc in documentsNotExecuted)
                    {
                        CreateOutputResult documentResult = new CreateOutputResult();
                        documentResult.OutputJobIdentity = doc.OutputJobId;
                        documentResult.OutputJobSequenceNumber = doc.OutputJobSequence;
                        documentResult.ErrorDescription = doc.ErrorDescription;
                        documentResult.ResultProperties = new List<CreateOutputResultProperty>();


                        //Adding overall results
                        documentResult.ResultProperties.Add(new CreateOutputResultProperty(StdResultParamNames.PrinterIDKey, doc.PrinterID));
                        documentResult.ResultProperties.Add(new CreateOutputResultProperty(StdResultParamNames.PrinterDeviceNameKey, doc.PrinterDeviceName));
                        documentResult.ResultProperties.Add(new CreateOutputResultProperty(StdResultParamNames.ReportIDKey, doc.ReportID));

                        if (doc.MetaParameters.ContainsKey(StdMetaParamNames.UserIdentityKey))
                        {
                            documentResult.ResultProperties.Add(new CreateOutputResultProperty(StdResultParamNames.UserIDKey, doc.MetaParameters[StdMetaParamNames.UserIdentityKey]));
                        }
                        if (doc.MetaParameters.ContainsKey(StdMetaParamNames.TerminalIDKey))
                        {
                            documentResult.ResultProperties.Add(new CreateOutputResultProperty(StdResultParamNames.TerminalIDKey, doc.MetaParameters[StdMetaParamNames.TerminalIDKey]));
                        }
                        if (doc.MetaParameters.ContainsKey(StdMetaParamNames.NumberOfCopiesKey))
                        {
                            documentResult.ResultProperties.Add(new CreateOutputResultProperty(StdResultParamNames.NumberOfCopiesKey, doc.MetaParameters[StdMetaParamNames.NumberOfCopiesKey]));
                        }
                        if (doc.MetaParameters.ContainsKey(StdMetaParamNames.SubDocumentTypeIDKey))
                        {
                            documentResult.ResultProperties.Add(new CreateOutputResultProperty(StdResultParamNames.SubDocumentTypeIDKey, doc.MetaParameters[StdMetaParamNames.SubDocumentTypeIDKey]));
                        }
                        if (doc.MetaParameters.ContainsKey(StdMetaParamNames.DocumentTypeIDKey))
                        {
                            documentResult.ResultProperties.Add(new CreateOutputResultProperty(StdResultParamNames.DocumentTypeIDKey, doc.MetaParameters[StdMetaParamNames.DocumentTypeIDKey]));
                        }


                        resultCollection.Add(documentResult);
                    }

                    return resultCollection;
                }
                finally
                {
                    //Release printer locks
                    foreach (string printerDeviceName in numOfDocsPerPrinter.Keys)
                    {
                        bool exclusive = (numOfDocsPerPrinter[printerDeviceName] > 1);
                        ReleaseLock(printerDeviceName, exclusive, _printerLockDictionary);
                    }
                }

            }
            finally
            {
                foreach (string adapterId in usedAdapters)
                {
                    ReleaseLock(adapterId, false, _adapterLockDictionary);
                }
            }
        }


        private void AcquireLock(string lockName, bool exclusive, Dictionary<string, ReaderWriterLockSlim> lockDictionary)
        {
            ReaderWriterLockSlim theLock = null;

            lock (lockDictionary)
            {
                if (!lockDictionary.ContainsKey(lockName))
                {
                    lockDictionary.Add(lockName, new ReaderWriterLockSlim());
                }

                theLock = lockDictionary[lockName];
            }

            if (exclusive)
            {
                theLock.EnterWriteLock();
            }
            else
            {
                theLock.EnterReadLock();
            }
        }

        private void ReleaseLock(string lockName, bool exclusive, Dictionary<string, ReaderWriterLockSlim> lockDictionary)
        {
            if (lockDictionary.ContainsKey(lockName))
            {
                ReaderWriterLockSlim theLock = lockDictionary[lockName];

                if (exclusive)
                {
                    theLock.ExitWriteLock();
                }
                else
                {
                    theLock.ExitReadLock();
                }
            }
        }

        private class UpdatedAdapterObjects
        {
            public bool configUpdated = false;
            public List<string> updatedReports = new List<string>();
        }

        private void CheckAndUpdateAdaptersThread(object state)
        {
            Dictionary<string, UpdatedAdapterObjects> updates = new Dictionary<string, UpdatedAdapterObjects>();

            while (true)
            {
                try
                {
                    updates.Clear();
                    //Check if adapter configuration parameters changed
                    foreach (string adapterId in _adapters.Keys)
                    {
                        if (_configDataHandler.CheckAdapterConfigurationUpdated(adapterId))
                        {
                            if (!updates.ContainsKey(adapterId))
                            {
                                updates.Add(adapterId, new UpdatedAdapterObjects());
                            }

                            updates[adapterId].configUpdated = true;
                        }


                        List<string> updatedReports = _configDataHandler.GetUpdatedReportsForAdapter(adapterId);

                        if (updatedReports.Count > 0)
                        {
                            if (!updates.ContainsKey(adapterId))
                            {
                                updates.Add(adapterId, new UpdatedAdapterObjects());
                            }

                            updates[adapterId].updatedReports = updatedReports;
                        }
                    }

                    if (updates.Count > 0)
                    {
                        foreach (string adapterId in updates.Keys)
                        {
                            AcquireLock(adapterId, true, _adapterLockDictionary);

                            try
                            {
                                UpdatedAdapterObjects updObj = updates[adapterId];

                                if (updObj.configUpdated)
                                {
                                    _adapters[adapterId].UpdateConfiguration(_configDataHandler.GetAdapterConfigurationParameters(adapterId));
                                }

                                if (updObj.updatedReports.Count > 0)
                                {
                                    _adapters[adapterId].UpdateReportFiles(_configDataHandler.GetReports_ReportFiles(adapterId), updObj.updatedReports);
                                }
                            }
                            finally
                            {
                                ReleaseLock(adapterId, true, _adapterLockDictionary);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    _configDataHandler.LogError(ex.Message);
                }

                Thread.Sleep(30000);
            }
        }
    }
}
