using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using Imi.SupplyChain.Server.Job.StandardJob;
using Imi.Framework.Job;
using Imi.Framework.Job.Data;
using Imi.SupplyChain.Server.Job.OutputManagerSend.OutputManagerService;

namespace Imi.SupplyChain.Server.Job.OutputManagerSend
{
    class OutputManagerSend : OracleJob
    {
        private PrintQueue _printQueue;
        private Dictionary<string, Warehouse_OutputManager_Config> _outputManagers = null;
        private string _endpointName;
        private Dictionary<string, int> _jobsPerOMCounter = new Dictionary<string, int>();
        private int _maxNumOfConcurrentJobs = 100;

        private int _totalNumOfConcurrentJobs = 0;

        private class PrintJob
        {
            public double PrintJobId;
            public string WarehouseId;
            public CreateOutputParametersCollection ParametersCollection;
        }

        private class PrintJobArguments
        {
            public double printJobId;
            public CreateOutputParametersCollection documentCollection;
            public string omId;
            public string urlMain;
            public string fallbackUrl;

            public PrintJobArguments(double printJobId, CreateOutputParametersCollection documentCollection, string omId, string urlMain, string fallbackUrl)
            {
                this.printJobId = printJobId;
                this.documentCollection = documentCollection;
                this.omId = omId;
                this.urlMain = urlMain;
                this.fallbackUrl = fallbackUrl;
            }
        }

        public OutputManagerSend(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
            _endpointName = args["endpointName"];
            _maxNumOfConcurrentJobs = Convert.ToInt32(args["maxConcurrentJobs"]);
        }

        protected override void CreateProcedure(IDbConnectionProvider connectionProvider)
        {
            _printQueue = new PrintQueue(connectionProvider);
        }

        protected override void CancelProcedure()
        {
            if (_printQueue != null)
            {
                _printQueue.Cancel();
            }
        }

        /// <summary>
        /// ExecuteProcedure is the main activity method, this is the code that is run
        /// when the Job is activated/signalled.
        /// </summary>
        protected override void ExecuteProcedure(Imi.Framework.Job.JobArgumentCollection args)
        {
            //Check if and reset print queue
            //==================================================
            if (_totalNumOfConcurrentJobs <= 0)
            {
                Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("Resetting print queue..."));
                _printQueue.Reset();
                _totalNumOfConcurrentJobs = 0;
            }
            //==================================================

            IList<Document> documents = null;

            bool hasJobs = true;

            _outputManagers = GetConfig();
            int i = 0;
            while (hasJobs)
            {
                i++;
                StartTransaction();

                try
                { 
                    Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("Fetching print jobs..."));

                    documents = GetPrintJobs();

                    System.Diagnostics.Debug.WriteLine(DateTime.Now + " Fetched " + documents.Count + " documents. Iteration: " + i);

                    hasJobs = documents.Count > 0;

                    Commit();
                }
                catch
                {
                    Rollback();
                    throw;
                }

                if (hasJobs)
                {
                    SendPrintJobs(CreateJobs(documents));
                }
            }
        }

        private void SendPrintJobs(List<PrintJob> jobList)
        {
            while (jobList.Count > 0)
            {
                PrintJob job = jobList[0];

                string callUrl = string.Empty;
                string fallbackUrl = string.Empty;
                string omId = string.Empty;

                if (_outputManagers.ContainsKey(job.WarehouseId))
                {
                    callUrl = _outputManagers[job.WarehouseId].Main_Url;
                    fallbackUrl = _outputManagers[job.WarehouseId].Fallback_Url;

                    omId = _outputManagers[job.WarehouseId].OutputManagerId;
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("No Output Manager configured for Warehouse: {0}  Job: {1}", job.WarehouseId, job.PrintJobId));
                }

                if (!_jobsPerOMCounter.ContainsKey(omId))
                {
                    _jobsPerOMCounter.Add(omId, 0);
                }

                if ((_jobsPerOMCounter[omId] < _maxNumOfConcurrentJobs) || _maxNumOfConcurrentJobs == 0)
                {
                    _jobsPerOMCounter[omId]++;
                    _totalNumOfConcurrentJobs++;

                    PrintJobArguments arguments = new PrintJobArguments(job.PrintJobId, job.ParametersCollection, omId, callUrl, fallbackUrl);

                    System.Threading.ThreadPool.QueueUserWorkItem(SendPrintJobThread, (object)arguments);

                    jobList.RemoveAt(0);
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                }
                //For debugging without threading
                //SendPrintJob(printJobId, documentCollection, omId, callUrl, fallbackUrl);
            }

        }

        private void SendPrintJobThread(object argumentObject)
        {
            try
            {
                PrintJobArguments argument = (PrintJobArguments)argumentObject;
                if (argument != null)
                {

                    double printJobId = argument.printJobId;
                    CreateOutputParametersCollection documentCollection = argument.documentCollection;
                    string omId = argument.omId;
                    string urlMain = argument.urlMain;
                    string fallbackUrl = argument.fallbackUrl;

                    SendPrintJob(printJobId, documentCollection, omId, urlMain, fallbackUrl);

                }
            }
            catch (Exception ex)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error executing print job thread.\r\n\r\n{0}\r\n\r\n", ex.Message));
            }
        }

        private void SendPrintJob(double printJobId, CreateOutputParametersCollection documentCollection, string omId, string urlMain, string fallbackUrl)
        {
            string almId = string.Empty;
            string currentUrl = string.Empty;
            string fallback = "0";
            CreateOutputResultCollection createOutputResultCollection = null;
            OutputManagerService.OutputHandlerServiceClient client = null;

            List<string> urls = new List<string>() { urlMain };
            if (!string.IsNullOrEmpty(fallbackUrl)) { urls.Add(fallbackUrl); }

            try
            {
                int iteration = 0;

                client = new OutputManagerService.OutputHandlerServiceClient(_endpointName);

                while (iteration < urls.Count)
                {
                    almId = string.Empty;

                    if (client.State != CommunicationState.Created)
                    {
                        try
                        {
                            client.Close();
                        }
                        catch
                        {
                            try { client.Abort(); }
                            catch { }
                        }

                        client = new OutputManagerService.OutputHandlerServiceClient(_endpointName);
                    }

                    try
                    {
                        if (iteration == 1) { fallback = "1"; }
                        currentUrl = urls[iteration];

                        Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("Sending print job {0} to {1}...", printJobId, currentUrl));

                        EndpointAddress currentEndpoint = client.Endpoint.Address;

                        EndpointAddress address = new EndpointAddress(new Uri(currentUrl), currentEndpoint.Identity, currentEndpoint.Headers);

                        client.Endpoint.Address = address;

                        createOutputResultCollection = client.CreateOutput(documentCollection);

                        iteration = urls.Count;
                        Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("Dequeuing print job {0} to {1}...", printJobId, currentUrl));
                    }
                    catch (UriFormatException ex)
                    {
                        Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Incorrect Url format: job {0} to {1}\r\n\r\n{2}\r\n\r\n", printJobId, currentUrl, ex.Message));

                        almId = "OMS002";
                        iteration++;
                    }
                    catch (ActionNotSupportedException ex)
                    {
                        Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error sending print job {0} to {1}\r\n\r\n{2}\r\n\r\n", printJobId, currentUrl, ex.Message));

                        almId = "OMS003";
                        iteration++;
                    }
                    catch (ServerTooBusyException ex)
                    {
                        Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error sending print job {0} to {1}\r\n\r\n{2}\r\n\r\n", printJobId, currentUrl, ex.Message));

                        almId = "OMS004";
                        iteration++;
                    }
                    catch (EndpointNotFoundException ex)
                    {
                        Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error sending print job {0} to {1}\r\n\r\n{2}\r\n\r\n", printJobId, currentUrl, ex.Message));

                        almId = "OMS005";
                        iteration++;
                    }
                    catch (CommunicationException ex)
                    {
                        Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error sending print job {0} to {1}\r\n\r\n{2}\r\n\r\n", printJobId, currentUrl, ex.Message));

                        almId = "OMS006";
                        iteration++;
                    }
                    catch (ArgumentException ex)
                    {
                        Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error sending print job {0} to {1}\r\n\r\n{2}\r\n\r\n", printJobId, currentUrl, ex.Message));

                        almId = "OMS007";
                        iteration++;
                    }
                    catch (TimeoutException ex)
                    {
                        Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error sending print job {0} to {1}\r\n\r\n{2}\r\n\r\n", printJobId, currentUrl, ex.Message));

                        almId = "OMS008";
                        iteration = urls.Count; //Do not use fallback for Timeout. To avoid double print outs.
                    }
                    catch (Exception ex)
                    {
                        Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error sending print job {0} to {1}\r\n\r\n{2}\r\n\r\n", printJobId, currentUrl, ex.Message));

                        almId = "OMS009";
                        iteration++;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error sending print job {0} \r\n\r\n{1}\r\n\r\n", printJobId, ex.Message));

                almId = "OMS010";
            }
            finally
            {
                if (_jobsPerOMCounter.ContainsKey(omId))
                {
                    _jobsPerOMCounter[omId]--;
                }

                if (client.State == CommunicationState.Opened || client.State == CommunicationState.Opening)
                {
                    try
                    {
                        client.Close();
                    }
                    catch
                    {
                        try { client.Abort(); }
                        catch { }
                    }
                }

                lock (_printQueue)
                {
                    UpdateDb(omId, printJobId, almId, fallback, createOutputResultCollection);
                }

                _totalNumOfConcurrentJobs--;
            }
        }

        private List<PrintJob> CreateJobs(IList<Document> documents)
        {
            Dictionary<double, PrintJob> internalLookUp = new Dictionary<double, PrintJob>();

            foreach (Document document in documents)
            {
                double printJobId = document.PrintJobId.Value;
                string warehouseId = document.WarehouseId;
                int seqNum = document.SeqNum;

                if (_outputManagers.ContainsKey(warehouseId))
                {

                    if (!internalLookUp.ContainsKey(printJobId))
                    {
                        PrintJob newPrintJob = new PrintJob();
                        newPrintJob.ParametersCollection = new CreateOutputParametersCollection();
                        newPrintJob.PrintJobId = printJobId;
                        newPrintJob.WarehouseId = warehouseId;

                        internalLookUp.Add(printJobId, newPrintJob);
                    }
                    CreateOutputParameters createOutputParameters = new CreateOutputParameters();

                    createOutputParameters.OutputXML = document.Data;
                    createOutputParameters.OutputJobIdentity = printJobId.ToString();
                    createOutputParameters.OutputJobSequenceNumber = seqNum;

                    internalLookUp[printJobId].ParametersCollection.Add(createOutputParameters);
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("No Output Manager configured for Warehouse: {0}  Job: {1}", warehouseId, printJobId));
                }
            }

            return internalLookUp.Select(item => item.Value).ToList(); ;
        }

        private IList<Document> GetPrintJobs()
        {
            IList<Document> documents = new List<Document>();

            IDataReader reader;

            _printQueue.Getprintjobs(5, 10, out reader);

            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        Document document = new Document();

                        if (reader["PRINTJOBID"] == DBNull.Value)
                            document.PrintJobId = null;
                        else
                            document.PrintJobId = Convert.ToDouble(reader["PRINTJOBID"]);

                        if (reader["SEQNUM"] == DBNull.Value)
                            document.SeqNum = 0; //null;

                        else
                            document.SeqNum = Convert.ToInt32(reader["SEQNUM"]);

                        if (reader["DOCUMENT"] == DBNull.Value)
                            document.Data = "";
                        else
                            document.Data = reader["DOCUMENT"] as String;

                        if (reader["WHID"] == DBNull.Value)
                            document.WarehouseId = "";
                        else
                            document.WarehouseId = reader["WHID"] as String;

                        documents.Add(document);
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return documents;
        }

        private Dictionary<string, Warehouse_OutputManager_Config> GetConfig()
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

                        if (reader["WHID"] == DBNull.Value)
                            conf.WarehouseId = "";
                        else
                            conf.WarehouseId = reader["WHID"] as String;

                        if (reader["OMID"] == DBNull.Value)
                            conf.WarehouseId = "";
                        else
                            conf.OutputManagerId = reader["OMID"] as String;

                        if (reader["MAIN_URL"] == DBNull.Value)
                            conf.Main_Url = "";
                        else
                            conf.Main_Url = reader["MAIN_URL"] as String;

                        if (reader["FALLBACK_URL"] == DBNull.Value)
                            conf.Fallback_Url = "";
                        else
                            conf.Fallback_Url = reader["FALLBACK_URL"] as String;


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

        private void UpdateDb(string omId, double printJobId, string almId, string fallback, CreateOutputResultCollection createOutputResultCollection)
        {
            StartTransaction();

            try
            {
                string outputJobIdentity = string.Empty;
                string printerName = string.Empty;
                string printerId = string.Empty;
                bool isError = false;
                bool isErrorOnDocument = false;

                if (!string.IsNullOrEmpty(almId))
                {
                    isError = true;
                }

                if (createOutputResultCollection != null)
                {
                    foreach (CreateOutputResult createOutputResult in createOutputResultCollection)
                    {
                        const string DocumentTypeIDKey = "DOCTYPE";
                        const string SubDocumentTypeIDKey = "SUBDOCTYPE";
                        const string PrinterIDKey = "PRINTERID";
                        const string TerminalIDKey = "TERMINALID";
                        const string PrinterDeviceNameKey = "PRINTERNAME";
                        const string UserIDKey = "USERID";
                        const string NumberOfCopiesKey = "NUMCOPIES";
                        const string ReportIDKey = "REPORTID";
                        const string BinaryResultKey = "BINARY";

                        outputJobIdentity = createOutputResult.OutputJobIdentity;
                        int jobIndentity;
                        if (!int.TryParse(outputJobIdentity, out jobIndentity))
                        {
                            jobIndentity = 0;
                        }
                        int outputJobSequenceNumber = createOutputResult.OutputJobSequenceNumber;

                        byte[] PDF = null;
                        string docTyp = string.Empty;
                        string docSubTyp = string.Empty;
                        string terId = string.Empty;
                        string numberOfCopies = string.Empty;
                        string reportId = string.Empty;
                        string empId = string.Empty;
                        int noCopies = 0;

                        isErrorOnDocument = false;
                        string errorData = createOutputResult.ErrorDescription;

                        if (!string.IsNullOrEmpty(errorData))
                        {
                            almId = "OMS001";
                            isError = true;
                            isErrorOnDocument = true;
                        }

                        foreach (CreateOutputResultProperty createOutputResultProperty in createOutputResult.ResultProperties)
                        {

                            if (createOutputResultProperty.PropertyName.Equals(BinaryResultKey))
                            {
                                PDF = (byte[])createOutputResultProperty.PropertyValue;
                            }
                            if (createOutputResultProperty.PropertyName.Equals(PrinterDeviceNameKey))
                            {
                                printerName = createOutputResultProperty.PropertyValue.ToString();
                            }
                            if (createOutputResultProperty.PropertyName.Equals(PrinterIDKey))
                            {
                                printerId = createOutputResultProperty.PropertyValue.ToString();
                            }
                            if (createOutputResultProperty.PropertyName.Equals(TerminalIDKey))
                            {
                                terId = createOutputResultProperty.PropertyValue.ToString();
                            }
                            if (createOutputResultProperty.PropertyName.Equals(UserIDKey))
                            {
                                empId = createOutputResultProperty.PropertyValue.ToString();
                            }
                            if (createOutputResultProperty.PropertyName.Equals(NumberOfCopiesKey))
                            {
                                numberOfCopies = createOutputResultProperty.PropertyValue.ToString();
                                if (!int.TryParse(numberOfCopies, out noCopies))
                                {
                                    noCopies = 0;
                                }
                            }
                            if (createOutputResultProperty.PropertyName.Equals(DocumentTypeIDKey))
                            {
                                docTyp = createOutputResultProperty.PropertyValue.ToString();
                            }
                            if (createOutputResultProperty.PropertyName.Equals(SubDocumentTypeIDKey))
                            {
                                docSubTyp = createOutputResultProperty.PropertyValue.ToString();
                            }
                            if (createOutputResultProperty.PropertyName.Equals(ReportIDKey))
                            {
                                reportId = createOutputResultProperty.PropertyValue.ToString();
                            }
                        }

                        /* uppdatera PrintQueeDocument */
                        if (isErrorOnDocument)
                        {
                            _printQueue.PrintDocError(printJobId, outputJobSequenceNumber, errorData, PDF, docTyp, docSubTyp, terId, printerId, printerName, empId, noCopies, reportId);
                        }
                        else
                        {
                            _printQueue.PrintDocFinished(printJobId, outputJobSequenceNumber, PDF, docTyp, docSubTyp, terId, printerId, printerName, empId, noCopies, reportId);
                        }
                    }
                }

                if (!isError)
                {
                    _printQueue.PrintFinished(printJobId, omId, fallback);
                }
                else
                {
                    _printQueue.PrintError(printJobId, almId, fallback);
                }

                Commit();
            }
            catch (Exception ex)
            {
                Rollback();

                Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error updating database for Job: {0}\r\n{1}", printJobId, ex.Message));
            }
        }
    }
}
