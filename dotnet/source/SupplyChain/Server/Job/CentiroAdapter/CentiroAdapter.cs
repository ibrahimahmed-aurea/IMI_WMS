using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.ServiceModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Imi.Framework.Job;
using Imi.Framework.Job.Data;
using Imi.Framework.Job.Configuration;
using Imi.SupplyChain.Server.Job.StandardJob;
using Imi.SupplyChain.Server.Job.CentiroAdapter.OraclePackage;
using Imi.SupplyChain.Server.Job.CentiroAdapter;
using Imi.SupplyChain.Server.Job.CentiroAdapter.DataEntities;
using Imi.SupplyChain.Server.Job.CentiroAdapter.CentiroService;
using Imi.SupplyChain.Server.Job.CentiroAdapter.OutputManagerInterface;

namespace Imi.SupplyChain.Server.Job.CentiroAdapter
{
    public class CentiroAdapterException : Exception
    {
        public CentiroAdapterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public CentiroAdapterException(string message)
            : base(message)
        {
        }

    }

    public class CENTIROTRANS
    {
        public double REPORTJOBID;
        public string REPORT;
        public string REPORTTYPE;
        public string WHID;
    }

    public class CentiroAdapter : OracleJob
    {
        private CarrierCompliantQueue _carrierCompliantQueue;
        private Carriercompliant _carrierCompliant;
        private PrintQueue _printQueue;
        private Dictionary<string, CentiroServiceAgent> _adapters;
        private BusinessDataMapper _businessDataMapper;
        private OutputManagerInformationProvider _outputManagerInfoProvider;
        private string _outputManagerEndpointName;
        private string _userName;
        private string _password;
        private string _logDirectory;
        private bool _isXmlDumpEnabled;
        private string _printerType;
        private string _documentType;
        private bool _fetchEachPrinterType;
        private bool _getMessages;
        private bool _useLogTable;
        private string _adapterId;
        private bool _stopJob;

        private XmlSerializer _updateShipmentSerializer;
        private XmlSerializer _updateParcelSerializer;
        private XmlSerializer _closeDepartureSerializer;
        private XmlSerializer _printDepartureSerializer;
        private XmlSerializer _removeParcelRequestSerializer;
        private XmlSerializer _printParcelSerializer;
        private XmlSerializer _printShipmentSerializer;
        private XmlSerializer _genericReportSerializer;
        private List<GenericReport> _receivedDocuments;

        public string MessageId;

        public CentiroAdapter(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
            _fetchEachPrinterType = true;
            _getMessages = true;

            _adapters = new Dictionary<string, CentiroServiceAgent>();
            _businessDataMapper = new BusinessDataMapper();
            _receivedDocuments = new List<GenericReport>();

            _updateShipmentSerializer = new XmlSerializer(typeof(CentiroUpdateShipment));
            _updateParcelSerializer = new XmlSerializer(typeof(CentiroUpdateParcels));
            _closeDepartureSerializer = new XmlSerializer(typeof(CentiroCloseDepartureType));
            _printDepartureSerializer = new XmlSerializer(typeof(CentiroPrintDepartureType));
            _removeParcelRequestSerializer = new XmlSerializer(typeof(CentiroRemoveParcels));
            _printParcelSerializer = new XmlSerializer(typeof(CentiroPrintParcels));
            _printShipmentSerializer = new XmlSerializer(typeof(CentiroPrintShipmentType));
            _genericReportSerializer = new XmlSerializer(typeof(GenericReport));

            _adapterId = args["adapterId"];

            _outputManagerEndpointName = args["outputManagerEndpointName"];

            if (string.IsNullOrEmpty(_adapterId))
            {
                _adapterId = "0";
            }

        }

        protected override void CreateProcedure(IDbConnectionProvider connection)
        {
            _carrierCompliantQueue = new CarrierCompliantQueue(connection);
            _carrierCompliant = new Carriercompliant(connection);
            _printQueue = new PrintQueue(connection);
            _outputManagerInfoProvider = new OutputManagerInformationProvider(_printQueue, Tracing, _outputManagerEndpointName);

            //Reseting all jobs for the adapter at startup.
            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Reseting all jobs in progress for adapter: " + _adapterId);
            StartTransaction();
            _carrierCompliantQueue.AdapterReset(_adapterId);
            Commit();
        }

        private CentiroServiceAgent GetCachedAdapter(string url)
        {
            CentiroServiceAgent csAdapter;

            // fallback to overcome bugs in server code - if only one url is present in the cache and
            // url from the server is null, then use the already cached url
            if (string.IsNullOrEmpty(url))
            {
                Dictionary<string, CentiroServiceAgent>.KeyCollection kc = _adapters.Keys;

                if (kc != null)
                {
                    if (kc.Count == 1)
                    {
                        foreach (string k in kc)
                        {
                            url = k;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(url))
            {
                string exceptionMsg = "Empty url received from queue without already present cached url or more than one url in cache. Unable to determine host addess, aborting send.";
                Tracing.TraceEvent(TraceEventType.Error, 0, exceptionMsg);
                throw new CentiroAdapterException(exceptionMsg);
            }

            if (_adapters.ContainsKey(url))
            {
                csAdapter = _adapters[url];
            }
            else
            {
                try
                {
                    csAdapter = new CentiroServiceAgent(url, Tracing);

                    if (csAdapter != null)
                    {
                        csAdapter.SetCredentials(_userName, _password, true);
                        csAdapter.LogDirectory = _logDirectory;
                        csAdapter.AdapterId = _adapterId;

                        _adapters.Add(url, csAdapter);
                    }
                }
                catch (CentiroServiceAgentException ex)
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0,
                        string.Format("Exception while creating adapter for url = {0}", url));
                    Tracing.TraceData(TraceEventType.Error, 0, ex);
                    throw;
                }
            }

            csAdapter.NewCentiroCall();

            return csAdapter;
        }

        private void SaveParcel(CentiroService.ParcelDetails parcel)
        {
            if (!string.IsNullOrEmpty(parcel.SequenceNo)
                || !string.IsNullOrEmpty(parcel.SequenceNo2)
                || !string.IsNullOrEmpty(parcel.Barcode)
                || !string.IsNullOrEmpty(parcel.SSCC)
              )
            {
                _carrierCompliant.ModifyParcel(parcel.ParcelIdentifier, parcel.SequenceNo, parcel.SequenceNo2, parcel.SSCC, parcel.Barcode);
            }

            if (parcel.TrackingUrl != null)
            {
                _carrierCompliant.ModifyTrackingUrl(parcel.ParcelIdentifier, parcel.TrackingUrl);
            }
        }

        private void SaveShipment(CentiroService.ShipmentDetails shipment)
        {
            _carrierCompliant.ModifyConsignment(shipment.ShipmentIdentifier, shipment.SequenceNo, shipment.SequenceNo2);

            if (shipment.ParcelDetails != null)
            {
                foreach (ParcelDetails parcel in shipment.ParcelDetails)
                {
                    SaveParcel(parcel);
                }
            }
        }


        private CENTIROTRANS FindMessageToSend()
        {
            CENTIROTRANS transaction = null;

            double? tmpCCJOBID = null;
            string tmpCCTYPE = null;
            string tmpCCMSG = null;
            string tmpWHID = null;

            _carrierCompliantQueue.GetNextMessage(_adapterId, ref tmpCCJOBID, ref tmpCCTYPE, ref tmpCCMSG, ref tmpWHID);


            if (tmpCCJOBID != null)
            {
                transaction = new CENTIROTRANS();

                transaction.REPORTJOBID = tmpCCJOBID.Value;

                if (string.IsNullOrEmpty(tmpCCTYPE))
                    transaction.REPORTTYPE = "";
                else
                    transaction.REPORTTYPE = tmpCCTYPE;

                if (string.IsNullOrEmpty(tmpCCMSG))
                    transaction.REPORT = "";
                else
                    transaction.REPORT = tmpCCMSG;

                if (string.IsNullOrEmpty(tmpWHID))
                    transaction.WHID = "";
                else
                    transaction.WHID = tmpWHID;

                Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                   string.Format("Found job {0} - {1}", transaction.REPORTJOBID, transaction.REPORTTYPE));
            }


            return transaction;
        }

        private string GetPrinterType(string terminalId, string documentType, string warehouseId)
        {
            string error;
            string printerType = _outputManagerInfoProvider.GetPrinterTypeFromOutputManager(warehouseId, terminalId, documentType, string.Empty, out error);

            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            return printerType;
        }

        protected override void ExecuteProcedure(JobArgumentCollection args)
        {
            // Check parameters
            _userName = args["userName"];
            _password = args["password"];
            _useLogTable = Convert.ToBoolean(args["useLogTable"]);

            _logDirectory = args["logDirectory"];
            _printerType = args["printerType"];

            if (!string.IsNullOrEmpty(_printerType))
            {
                _fetchEachPrinterType = false;
            }

            string dumpXmlValue = args["dumpXml"];

            if (!string.IsNullOrEmpty(dumpXmlValue))
            {
                _isXmlDumpEnabled = (dumpXmlValue == "true");
            }

            _documentType = args["documentType"];

            if (string.IsNullOrEmpty(_documentType))
            {
                _documentType = "Rdf";
            }

            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Start");

            _getMessages = true;
            _stopJob = false;

            while (_getMessages && !_stopJob)
            {
                DateTime startDT = DateTime.Now;

                long start = DateTime.Now.Ticks;
                _getMessages = false;

                MessageId = Convert.ToString(DateTime.Now.Ticks);

                DateTime startDBDT = DateTime.Now;

                StartTransaction();

                CENTIROTRANS transaction = FindMessageToSend();

                Commit();

                DateTime doneDBDT = DateTime.Now;


                if (transaction != null)
                {
                    string error = "";

                    try
                    {
                        //Write log table
                        NewLog("Start message loop", startDT, 0, transaction);

                        //Write log table
                        NewLog("Start fetching message from database", startDBDT, 10, transaction);

                        //Write log table
                        NewLog("Done fetching message from database", doneDBDT, 20, transaction);

                        _getMessages = true;

                        if (_isXmlDumpEnabled)
                        {
                            //Write log table
                            NewLog("Start XML dump to file", DateTime.Now, 30, transaction);

                            new XMLHelper().DumpToFile(transaction.REPORT, LogFileName("FromPLQueue_type_" + transaction.REPORTTYPE));

                            //Write log table
                            NewLog("Done XML dump to file", DateTime.Now, 40, transaction);
                        }

                        //Write log table
                        NewLog("Start logging message", DateTime.Now, 50, transaction);

                        Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                           string.Format("Found REPORTJOBID {0} REPORTTYPE {1}", transaction.REPORTJOBID, transaction.REPORTTYPE));

                        //Write log table
                        NewLog("Done logging message", DateTime.Now, 60, transaction);

                        StartTransaction();

                        //Write log table
                        NewLog("Start Centiro send and receive process", DateTime.Now, 70, transaction);

                        switch (transaction.REPORTTYPE)
                        {
                            case "UP":  /* Update Parcel */
                                UpdateParcel(transaction.REPORT);
                                break;
                            case "RP":  /* Remove Parcel */
                                RemoveParcel(transaction.REPORT);
                                break;
                            case "PP":  /* Print Parcel */
                                PrintParcel(transaction.REPORT, transaction.WHID);
                                break;
                            case "US":  /* Update Shipment */
                                UpdateShipment(transaction.REPORT);
                                break;
                            case "PS":  /* Print Shipment */
                                PrintShipment(transaction.REPORT, transaction.WHID);
                                break;
                            case "PD":  /* Print Departure */
                                PrintDeparture(transaction.REPORT);
                                break;
                            case "CD":  /* Close Departure */
                                CloseDeparture(transaction.REPORT);
                                break;
                            default:
                                Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                                   string.Format("REPORTTYPE {0} not supported", transaction.REPORTTYPE));
                                break;
                        }

                        //Write log table
                        NewLog("Done Centiro send and receive process", DateTime.Now, 80, transaction);

                        //Write log table
                        NewLog("Start enqueueing documents in print queue", DateTime.Now, 90, transaction);

                        EnqueueDocuments(transaction.WHID);

                        //Write log table
                        NewLog("Done enqueueing documents in print queue", DateTime.Now, 100, transaction);
                    }
                    catch (Exception ex)
                    {
                        Rollback();

                        //Write log table
                        NewLog("Start exception handler", DateTime.Now, 110, transaction);

                        Tracing.TraceEvent(TraceEventType.Error, 0, string.Format("Exception while printing REPORTJOBID {0} REPORTTYPE {1}", transaction.REPORTJOBID, transaction.REPORTTYPE));
                        Tracing.TraceData(TraceEventType.Error, 0, ex);

                        error = ex.Message;

                        StartTransaction();

                        //Write log table
                        NewLog("Done exception handler", DateTime.Now, 120, transaction);
                    }

                    string almid = "";

                    if (string.IsNullOrEmpty(error))
                    {
                        //Write log table
                        NewLog("Start updating status processed and logging status change", DateTime.Now, 130, transaction);

                        Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("No error reported for REPORTJOBID {0} REPORTTYPE {1}", transaction.REPORTJOBID, transaction.REPORTTYPE));
                        _carrierCompliantQueue.ModifyProcessed(transaction.REPORTJOBID, MessageId, ref almid);

                        //Write log table
                        NewLog("Done updating status processed and logging status change", DateTime.Now, 140, transaction);
                    }
                    else
                    {
                        //Write log table
                        NewLog("Start updating status error and logging status change", DateTime.Now, 150, transaction);

                        Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error reported for REPORTJOBID {0} REPORTTYPE {1}", transaction.REPORTJOBID, transaction.REPORTTYPE));

                        _carrierCompliantQueue.ModifyError(transaction.REPORTJOBID, error, MessageId, null, ref almid);

                        //Write log table
                        NewLog("Done updating status error and logging status change", DateTime.Now, 160, transaction);
                    }

                    if (!string.IsNullOrEmpty(almid))
                    {
                        //Write log table
                        NewLog("Start logging almid", DateTime.Now, 170, transaction);

                        Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error while updating transaction status. Almid={0}. REPORTJOBID {1} REPORTTYPE {2}", almid, transaction.REPORTJOBID, transaction.REPORTTYPE));

                        //Write log table
                        NewLog("Done logging almid", DateTime.Now, 180, transaction);
                    }

                    Commit();

                    //Write log table
                    NewLog("Done processing message", DateTime.Now, 190, transaction);
                }

                long stop = DateTime.Now.Ticks;
                TimeSpan ts = new TimeSpan(stop - start);

                Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("Stop (used {0}s)", ts.TotalSeconds.ToString("0.00")));


            };
        }

        private void NewLog(string text, DateTime timeStamp, int index, CENTIROTRANS transaction)
        {
            if (_useLogTable)
            {
                _carrierCompliantQueue.NewLog(transaction != null ? (double?)transaction.REPORTJOBID : null , transaction != null ? transaction.REPORTTYPE : null, text, timeStamp, index, transaction != null ? (double?)transaction.REPORT.Length : null, _adapterId);
            }
        }

        private void UpdateParcel(string requestClob)
        {
            CentiroUpdateParcels report = _updateParcelSerializer.Deserialize(new StringReader(requestClob)) as CentiroUpdateParcels;
            UpdateParcelsRequestType request = _businessDataMapper.Map(report);

            if (request == null)
            {
                throw new CentiroAdapterException(
                                string.Format("Failed to deserialize UpdateParcel clob. Data = {0}", requestClob));
            }
            else
            {
                MessageId = Convert.ToString(DateTime.Now.Ticks);
                request.MessageId = MessageId;

                if (_isXmlDumpEnabled)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("UpdateParcel_Request"));
                }

                CentiroServiceAgent csAdapter = GetCachedAdapter(report.url);
                UpdateParcelsResponseType ur = csAdapter.UpdateParcels(request, _isXmlDumpEnabled);
            }
        }

        private void RemoveParcel(string removeClob)
        {
            CentiroRemoveParcels report = _removeParcelRequestSerializer.Deserialize(new StringReader(removeClob)) as CentiroRemoveParcels;
            RemoveParcelsRequestType removeRequest = _businessDataMapper.Map(report);

            if (removeRequest == null)
            {
                throw new CentiroAdapterException(
                                string.Format("Failed to deserialize RemoveParcel clob. Data = {0}", removeClob));
            }
            else
            {
                MessageId = Convert.ToString(DateTime.Now.Ticks);
                removeRequest.MessageId = MessageId;

                if (_isXmlDumpEnabled)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(removeRequest), LogFileName("RemoveParcel_Request"));
                }

                CentiroServiceAgent csAdapter = GetCachedAdapter(report.url);
                csAdapter.RemoveParcels(removeRequest, _isXmlDumpEnabled);
            }
        }

        private void UpdateShipment(string requestClob)
        {
            CentiroUpdateShipment report = _updateShipmentSerializer.Deserialize(new StringReader(requestClob)) as CentiroUpdateShipment;
            UpdateShipmentRequestType request = _businessDataMapper.Map(report);

            if (request == null)
            {
                throw new CentiroAdapterException(
                                string.Format("Failed to deserialize UpdateShipment clob. Data = {0}", requestClob));
            }
            else
            {
                MessageId = Convert.ToString(DateTime.Now.Ticks);
                request.MessageId = MessageId;

                if (_isXmlDumpEnabled)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("UpdateShipment_Request"));
                }

                CentiroServiceAgent csAdapter = GetCachedAdapter(report.url);
                csAdapter.UpdateShipment(request, _isXmlDumpEnabled);
            }
        }

        private void PrintParcel(string requestClob, string whId)
        {
            CentiroPrintParcels report = _printParcelSerializer.Deserialize(new StringReader(requestClob)) as CentiroPrintParcels;

            
            if (_fetchEachPrinterType)
            {
                _printerType = GetPrinterType(report.terminal, "C*CPL", whId);
            }

            PrintParcelsRequestType request = _businessDataMapper.Map(report, _printerType);

            if (request == null)
            {
                throw new CentiroAdapterException(
                                string.Format("Failed to deserialize PrintParcel clob. Data = {0}", requestClob));
            }
            else
            {
                MessageId = Convert.ToString(DateTime.Now.Ticks);
                request.MessageId = MessageId;

                if (_isXmlDumpEnabled)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("PrintParcel_Request"));
                }

                CentiroServiceAgent csAdapter = GetCachedAdapter(report.url);
                PrintParcelsResponseType ur = csAdapter.PrintParcel(request, _isXmlDumpEnabled);

                if (ur == null)
                {
                    throw new CentiroAdapterException(
                              string.Format("No data in PrintParcel response. MessageId = {0}", request.MessageId));
                }
                else
                {
                    if (ur.LabelCode != null)
                    {
                        GenericReport document = new GenericReport();
                        document.MetaData = new MetaDataType();
                        document.MetaData.applicationIdentity = "Warehouse";
                        document.MetaData.documentType = "C*CPL";
                        document.MetaData.documentSubType = "";
                        document.MetaData.terminalIdentity = report.terminal;
                        document.MetaData.userIdentity = report.user;

                        if (report.printerIdentity == null)
                        {
                            document.MetaData.printerIdentity = "";
                        }
                        else
                        {
                            document.MetaData.printerIdentity = report.printerIdentity;
                        }

                        document.MetaData.numberOfCopies = 1;
                        document.Data = ur.LabelCode;

                        _receivedDocuments.Add(document);
                    }

                    if (ur.ParcelDetails != null)
                    {
                        foreach (ParcelDetails p in ur.ParcelDetails)
                        {
                            SaveParcel(p);
                        }
                    }
                }
            }
        }

        private void PrintShipment(string requestClob, string whId)
        {
            CentiroPrintShipmentType report = _printShipmentSerializer.Deserialize(new StringReader(requestClob)) as CentiroPrintShipmentType;

            if (_fetchEachPrinterType)
            {
                bool _check = (string.Equals( report.createCustomerSpec,  "1") || string.Equals( report.createLabels, "1") || string.Equals(report.createShipmentDoc, "1"));
                
                if (report.terminal != null && _check )
                {
                    _printerType = GetPrinterType(report.terminal, "C*CPL", whId);
                }
                
            }

            PrintShipmentRequestType request = _businessDataMapper.Map(report, _printerType, _documentType);

            if (request == null)
            {
                throw new CentiroAdapterException(
                                string.Format("Failed to deserialize PrintShipment clob. Data = {0}", requestClob));
            }
            else
            {
                MessageId = Convert.ToString(DateTime.Now.Ticks);
                request.MessageId = MessageId;

                if (_isXmlDumpEnabled)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("PrintShipment_Request"));
                }

                CentiroServiceAgent csAdapter = GetCachedAdapter(report.url);
                PrintShipmentResponseType ur = csAdapter.PrintShipment(request, _isXmlDumpEnabled);

                if (ur == null)
                {
                    throw new CentiroAdapterException(
                              string.Format("No data in PrintShipment response. MessageId = {0}", request.MessageId));
                }
                else
                {
                    if (ur.LabelCode != null)
                    {
                        GenericReport document = new GenericReport();
                        document.MetaData = new MetaDataType();
                        document.MetaData.applicationIdentity = "Warehouse";
                        document.MetaData.documentType = "C*CPL";
                        document.MetaData.documentSubType = "";
                        document.MetaData.terminalIdentity = report.terminal;
                        document.MetaData.userIdentity = report.user;
                        document.MetaData.printerIdentity = "";
                        document.MetaData.numberOfCopies = 1;
                        document.Data = ur.LabelCode;

                        _receivedDocuments.Add(document);
                    }

                    if (ur.ShipmentDocCode != null)
                    {
                        foreach (string d in ur.ShipmentDocCode)
                        {
                            GenericReport document = new GenericReport();
                            document.MetaData = new MetaDataType();
                            document.MetaData.applicationIdentity = "Warehouse";
                            document.MetaData.documentType = "C*BOL";
                            document.MetaData.documentSubType = "";
                            document.MetaData.terminalIdentity = report.terminal;
                            document.MetaData.userIdentity = report.user;
                            document.MetaData.printerIdentity = "";
                            document.MetaData.numberOfCopies = (sbyte)report.numberOfCopies;
                            document.Data = d;

                            _receivedDocuments.Add(document);
                        }
                    }

                    if (ur.CustomerSpecCode != null)
                    {
                        foreach (string d in ur.CustomerSpecCode)
                        {
                            GenericReport document = new GenericReport();
                            document.MetaData = new MetaDataType();
                            document.MetaData.applicationIdentity = "Warehouse";
                            document.MetaData.documentType = "C*BOL";
                            document.MetaData.documentSubType = "";
                            document.MetaData.terminalIdentity = report.terminal;
                            document.MetaData.userIdentity = report.user;
                            document.MetaData.printerIdentity = "";
                            document.MetaData.numberOfCopies = (sbyte)report.numberOfCopies;
                            document.Data = d;

                            _receivedDocuments.Add(document);
                        }
                    }

                    if (ur.ShipmentDetails != null)
                    {
                        SaveShipment(ur.ShipmentDetails);
                    }
                }
            }
        }

        private void PrintDeparture(string requestClob)
        {
            CentiroPrintDepartureType report = _printDepartureSerializer.Deserialize(new StringReader(requestClob)) as CentiroPrintDepartureType;
            PrintDepartureRequestType request = _businessDataMapper.Map(report, _documentType);

            if (request == null)
            {
                throw new CentiroAdapterException(
                                string.Format("Failed to deserialize PrintDeparture clob. Data = {0}", requestClob));
            }
            else
            {
                MessageId = Convert.ToString(DateTime.Now.Ticks);
                request.MessageId = MessageId;

                if (_isXmlDumpEnabled)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("PrintDeparture_Request"));
                }

                CentiroServiceAgent csAdapter = GetCachedAdapter(report.url);
                PrintDepartureResponseType ur = csAdapter.PrintDeparture(request, _isXmlDumpEnabled);

                if (ur == null)
                {
                    throw new CentiroAdapterException(
                              string.Format("No data in PrintDeparture response. MessageId = {0}", request.MessageId));
                }
                else
                {
                    if (ur.CarrierDocCode != null)
                    {
                        foreach (string d in ur.CarrierDocCode)
                        {
                            GenericReport document = new GenericReport();
                            document.MetaData = new MetaDataType();
                            document.MetaData.applicationIdentity = "Warehouse";
                            document.MetaData.documentType = "C*CPS";
                            document.MetaData.documentSubType = "";
                            document.MetaData.terminalIdentity = report.terminal;
                            document.MetaData.userIdentity = report.user;
                            document.MetaData.printerIdentity = "";
                            document.MetaData.numberOfCopies = (sbyte)report.numberOfCopies;
                            document.Data = d;

                            _receivedDocuments.Add(document);
                        }
                    }

                    if (ur.CustomerSpecCode != null)
                    {
                        foreach (string d in ur.CustomerSpecCode)
                        {
                            GenericReport document = new GenericReport();
                            document.MetaData = new MetaDataType();
                            document.MetaData.applicationIdentity = "Warehouse";
                            document.MetaData.documentType = "C*CPS";
                            document.MetaData.documentSubType = "";
                            document.MetaData.terminalIdentity = report.terminal;
                            document.MetaData.userIdentity = report.user;
                            document.MetaData.printerIdentity = "";
                            document.MetaData.numberOfCopies = (sbyte)report.numberOfCopies;
                            document.Data = d;

                            _receivedDocuments.Add(document);
                        }
                    }
                }
            }
        }

        private void CloseDeparture(string requestClob)
        {
            CentiroCloseDepartureType report = _closeDepartureSerializer.Deserialize(new StringReader(requestClob)) as CentiroCloseDepartureType;
            CloseDepartureRequestType request = _businessDataMapper.Map(report, _documentType);

            if (request == null)
            {
                throw new CentiroAdapterException(
                                string.Format("Failed to deserialize CloseDeparture clob. Data = {0}", requestClob));
            }
            else
            {
                MessageId = Convert.ToString(DateTime.Now.Ticks);
                request.MessageId = MessageId;

                if (_isXmlDumpEnabled)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("CloseDeparture_Request"));
                }

                CentiroServiceAgent csAdapter = GetCachedAdapter(report.url);
                CloseDepartureResponseType ur = csAdapter.CloseDeparture(request, _isXmlDumpEnabled);

                if (ur == null)
                {
                    throw new CentiroAdapterException(
                              string.Format("No data in CloseDeparture response. MessageId = {0}", request.MessageId));
                }
                else
                {
                    if (ur.CarrierDocCode != null)
                    {
                        foreach (string d in ur.CarrierDocCode)
                        {
                            GenericReport document = new GenericReport();
                            document.MetaData = new MetaDataType();
                            document.MetaData.applicationIdentity = "Warehouse";
                            document.MetaData.documentType = "C*CPS";
                            document.MetaData.documentSubType = "";
                            document.MetaData.terminalIdentity = report.terminal;
                            document.MetaData.userIdentity = report.user;
                            document.MetaData.printerIdentity = "";
                            document.MetaData.numberOfCopies = 1;
                            document.Data = d;

                            _receivedDocuments.Add(document);
                        }
                    }

                    if (ur.CustomerSpecCode != null)
                    {
                        foreach (string d in ur.CustomerSpecCode)
                        {
                            GenericReport document = new GenericReport();
                            document.MetaData = new MetaDataType();
                            document.MetaData.applicationIdentity = "Warehouse";
                            document.MetaData.documentType = "C*CPS";
                            document.MetaData.documentSubType = "";
                            document.MetaData.terminalIdentity = report.terminal;
                            document.MetaData.userIdentity = report.user;
                            document.MetaData.printerIdentity = "";
                            document.MetaData.numberOfCopies = 1;
                            document.Data = d;

                            _receivedDocuments.Add(document);
                        }
                    }

                    if (ur.ShipmentDetails != null)
                    {
                        foreach (ShipmentDetails shipment in ur.ShipmentDetails)
                        {
                            SaveShipment(shipment);
                        }
                    }
                }
            }
        }

        private void EnqueueDocuments(string warehouseID)
        {
            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Creating print job in print queue...");
            if (_receivedDocuments.Count > 0)
            {
                _printQueue.Createprintjob(warehouseID, null);
            }

            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Adding documents to print queue...");

            try
            {
                foreach (GenericReport d in _receivedDocuments)
                {
                    if (string.IsNullOrEmpty(d.Data))
                    {
                        Tracing.TraceEvent(TraceEventType.Verbose, 0, string.Format("Empty document data - document type: {0} Nothing to add.", d.MetaData.documentType));
                    }
                    else
                    {
                        Tracing.TraceEvent(TraceEventType.Verbose, 0, string.Format("Document type = {0}.", d.MetaData.documentType));

                        UTF8Encoding utf8 = new UTF8Encoding();
                        StringWriterWithEncoding sw = new StringWriterWithEncoding(utf8);

                        _genericReportSerializer.Serialize(sw, d);

                        string almid = "";

                        try
                        {
                            _printQueue.Enqueuedocument(sw.ToString(), ref almid, d.MetaData.documentType, d.MetaData.documentSubType, d.MetaData.terminalIdentity, d.MetaData.printerIdentity, d.MetaData.userIdentity, d.MetaData.numberOfCopies);
                        }
                        catch (Exception ex)
                        {
                            Tracing.TraceData(TraceEventType.Error, 0, ex);
                            throw;
                        }
                    }
                }

                Tracing.TraceEvent(TraceEventType.Verbose, 0, "Done adding documents.");

                Tracing.TraceEvent(TraceEventType.Verbose, 0, "Compleating print job in print queue...");

                _printQueue.Completeprintjob();
            }
            finally
            {
                _receivedDocuments.Clear();
            } 
        }

        protected override void CancelProcedure()
        {
            _stopJob = true;
        }

        private string LogFileName(string prefix)
        {
            if (_logDirectory != "")
            {
                string fileName = string.Format(@"{0}\{1:yyyy-MM-dd_HH-mm-ss.fff}_{2}_{3}.xml",
                  _logDirectory,
                  DateTime.Now,
                  prefix,
                  _adapterId
                  );
                return fileName;
            }
            return null;
        }
    }


}