using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Shared.Diagnostics;
using System.Diagnostics;
using System.Xml;
using System.IO;
using Imi.SupplyChain.Server.Job.CentiroAdapter.CentiroService;
using System.ServiceModel;
using Imi.SupplyChain.Server.Job.CentiroAdapter;


namespace Imi.SupplyChain.Server.Job.CentiroAdapter
{
    public class CentiroServiceAgentException : Exception
    {
        public CentiroServiceAgentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public CentiroServiceAgentException(string message)
            : base(message)
        {
        }

    }

    public class CentiroServiceAgent
    {
        private string _ticket;
        private DateTime _ticketTimeStamp = DateTime.MinValue;
        private string _userName;
        private string _password;
        private string _url;
        private CentiroService.AuthenticateRequestType _authRequest = new CentiroService.AuthenticateRequestType();
        private bool _invalidTicketError;

        private string _logDirectory;

        public string LogDirectory
        {
            get { return _logDirectory; }
            set { _logDirectory = value; }
        }
    
        public string AdapterId
        {
            get;
            set;
        }

        private TraceSource _traceSource;

        private TraceSource Tracing
        {
            get { return _traceSource; }
            set { _traceSource = value; }
        }

        public void NewCentiroCall()
        {
            _invalidTicketError = false;
        }

        public void SetCredentials(string userName, string password, bool verify)
        {
            this._userName = userName;
            this._password = password;
            this._authRequest.UserName = userName;
            this._authRequest.Password = password;


            if (verify)
            {
                RenewTicket(false);
            }
        }

        private void RenewTicket(bool verify)
        {
            // Ticket valid 4 hours
            if ((DateTime.Now.Subtract(_ticketTimeStamp) < TimeSpan.FromHours(3.9)) & !verify)
            {
                return;
            }

            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Aquiring a new authorization ticket.");
            WMSInterfaceClient client = new WMSInterfaceClient("BasicHttpBinding_IWMSInterface", _url);

            try
            {
                CentiroService.AuthenticateResponseType authResponse = client.Authenticate(_authRequest);
                client.Close();

                _ticket = authResponse.Ticket;


                if (string.IsNullOrEmpty(_ticket))
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0, "Failed to aquire a new authorization ticket. Check user and password settings.");
                    throw new CentiroServiceAgentException("Failed to aquire a new authorization ticket. Check user and password settings.");
                }
                else
                {
                    _ticketTimeStamp = DateTime.Now;
                    _invalidTicketError = false;
                    Tracing.TraceEvent(TraceEventType.Verbose, 0, "Successfully authorized.");
                }
            }
            catch (CommunicationException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error while attempting to aquire new authorization ticket.");
                client.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error while attempting to aquire new authorization ticket.");
                client.Abort();
                throw;
            }
        }

        private string LogFileName(string prefix)
        {
            if (!string.IsNullOrEmpty(_logDirectory))
            {
                string fileName = string.Format(@"{0}\{1:yyyy-MM-dd_HH-mm-ss.fff}_{2}_{3}.xml",
                  _logDirectory,
                  DateTime.Now,
                  prefix,
                  AdapterId
                  );
                return fileName;
            }

            return null;
        }

        public string GetErrorString(System.Collections.Generic.List<string> stringList)
        {
            StringBuilder result = new StringBuilder();
            foreach (string str in stringList)
            {
                result.Append(str);
                result.Append("\r\n");
            }
            return result.ToString();
        }

        public RemoveParcelsResponseType RemoveParcels(RemoveParcelsRequestType request, bool dumpXml)
        {
            WMSInterfaceClient client = new WMSInterfaceClient("BasicHttpBinding_IWMSInterface", _url);

            try
            {
                RenewTicket(false);
                request.Ticket = _ticket;

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("RemoveParcels_Request"));
                }

                RemoveParcelsResponseType response = client.RemoveParcels(request);

                client.Close();

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(response), LogFileName("RemoveParcels_Response"));
                }

                if (response.StatusCode == CentiroService.StatusCode.Ok)
                {
                    return response;
                }
                else if (response.StatusCode == CentiroService.StatusCode.InvalidTicket)
                {
                    if (!_invalidTicketError)
                    {
                        RenewTicket(true);
                        _invalidTicketError = true;
                        return RemoveParcels(request, dumpXml);
                    }
                }

                Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error calling RemoveParcels.\nError status = {0}\nError Message = {1}", response.StatusCode, GetErrorString(response.StatusMessages)));
                throw new CentiroServiceAgentException(String.Format("Error calling RemoveParcels.\nError status = {0}\nError Message = {1}",
                                                                     response.StatusCode, GetErrorString(response.StatusMessages)));
            }
            catch (CommunicationException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling RemoveParcels.");
                client.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling RemoveParcels.");
                throw;
            }
        }

        public UpdateShipmentResponseType UpdateShipment(UpdateShipmentRequestType request, bool dumpXml)
        {

            WMSInterfaceClient client = new WMSInterfaceClient("BasicHttpBinding_IWMSInterface", _url);
            try
            {

                RenewTicket(false);
                request.Ticket = _ticket;

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("DataToCentiro_UpdateShipment"));
                }

                UpdateShipmentResponseType response = client.UpdateShipment(request);

                client.Close();

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(response), LogFileName("DataFromCentiro_UpdateShipment"));
                }

                if (response.StatusCode == CentiroService.StatusCode.Ok)
                {
                    return (response);
                }
                else if (response.StatusCode == CentiroService.StatusCode.InvalidTicket)
                {
                    if (!_invalidTicketError)
                    {
                        RenewTicket(true);
                        _invalidTicketError = true;
                        return (UpdateShipment(request, dumpXml));
                    }
                    else
                        return response;
                }
                else
                {
                    string eString;
                    eString = GetErrorString(response.StatusMessages);
                    Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error while updating shipment .\nError status = {0}\nError Message = {1}",
                                                                         response.StatusCode, GetErrorString(response.StatusMessages)));
                    throw new CentiroServiceAgentException(String.Format("Error while updating shipment .\nError status = {0}\nError Message = {1}",
                                                                         response.StatusCode, GetErrorString(response.StatusMessages)));
                }
            }
            catch (CommunicationException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling UpdateShipment.");
                client.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling UpdateShipment.");
                throw;
            }
        }


        public PrintShipmentResponseType PrintShipment(PrintShipmentRequestType request, bool dumpXml)
        {

            WMSInterfaceClient client = new WMSInterfaceClient("BasicHttpBinding_IWMSInterface", _url);
            try
            {

                RenewTicket(false);
                request.Ticket = _ticket;

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("DataToCentiro_PrintShipment"));
                }

                PrintShipmentResponseType response = client.PrintShipment(request);

                client.Close();

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(response), LogFileName("DataFromCentiro_PrintShipment"));
                }

                if (response.StatusCode == CentiroService.StatusCode.Ok)
                {
                    return (response);
                }
                else if (response.StatusCode == CentiroService.StatusCode.InvalidTicket)
                {
                    if (!_invalidTicketError)
                    {
                        RenewTicket(true);
                        _invalidTicketError = true;
                        return (PrintShipment(request, dumpXml));
                    }
                    else
                        return response;
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error for print shipment .\nError status = {0}\nError Message = {1}",
                                                                         response.StatusCode, GetErrorString(response.StatusMessages)));
                    throw new CentiroServiceAgentException(String.Format("Error for Print Shipment .\nError status = {0}\nError Message = {1}",
                                                                         response.StatusCode, GetErrorString(response.StatusMessages)));
                }
            }


            catch (CommunicationException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling PrintShipment.");
                client.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling PrintShipment.");
                throw;
            }
        }



        public PrintParcelsResponseType PrintParcel(PrintParcelsRequestType request, bool dumpXml)
        {

            WMSInterfaceClient client = new WMSInterfaceClient("BasicHttpBinding_IWMSInterface", _url);
            try
            {

                RenewTicket(false);
                request.Ticket = _ticket;

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("DataToCentiro_PrintParcel"));
                }

                PrintParcelsResponseType response = client.PrintParcels(request);
                client.Close();

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(response), LogFileName("DataFromCentiro_PrintParcel"));
                }

                if (response.StatusCode == CentiroService.StatusCode.Ok)
                {
                    return (response);
                }
                else if (response.StatusCode == CentiroService.StatusCode.InvalidTicket)
                {
                    if (!_invalidTicketError)
                    {
                        RenewTicket(true);
                        _invalidTicketError = true;
                        return (PrintParcel(request, dumpXml));
                    }
                    else
                        return response;
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error for print Parcel .\nError status = {0}\nError Message = {1}",
                                                                         response.StatusCode, GetErrorString(response.StatusMessages)));
                    throw new CentiroServiceAgentException(String.Format("Error for Print Parcel .\nError status = {0}\nError Message = {1}",
                                                                         response.StatusCode, GetErrorString(response.StatusMessages)));
                }
            }

            catch (CommunicationException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling PrintParcel.");
                client.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling PrintParcel.");
                throw;
            }
        }




        public UpdateParcelsResponseType UpdateParcels(UpdateParcelsRequestType request, bool dumpXml)
        {

            WMSInterfaceClient client = new WMSInterfaceClient("BasicHttpBinding_IWMSInterface", _url);

            try
            {

                RenewTicket(false);
                request.Ticket = _ticket;

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("DataToCentiro_UpdateParcel"));
                }

                UpdateParcelsResponseType response = client.UpdateParcels(request);
                client.Close();

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(response), LogFileName("DataFromCentiro_UpdateParcels"));
                }

                if (response.StatusCode == CentiroService.StatusCode.Ok)
                {
                    return (response);
                }
                else if (response.StatusCode == CentiroService.StatusCode.InvalidTicket)
                {
                    if (!_invalidTicketError)
                    {
                        RenewTicket(true);
                        _invalidTicketError = true;
                        return (UpdateParcels(request, dumpXml));
                    }
                    else
                        return response;
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error for Update Parcel .\nError status = {0}\nError Message = {1}",
                                                                         response.StatusCode, GetErrorString(response.StatusMessages)));
                    throw new CentiroServiceAgentException(String.Format("Error for Update Parcel .\nError status = {0}\nError Message = {1}",
                                                                         response.StatusCode, GetErrorString(response.StatusMessages)));
                }
            }

            catch (CommunicationException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling UpdateParcel.");
                client.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling UpdateParcel.");
                throw;
            }
        }



        public PrintDepartureResponseType PrintDeparture(PrintDepartureRequestType request, bool dumpXml)
        {

            WMSInterfaceClient client = new WMSInterfaceClient("BasicHttpBinding_IWMSInterface", _url);
            try
            {

                RenewTicket(false);
                request.Ticket = _ticket;

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("DataToCentiro_PrintDeparture"));
                }

                PrintDepartureResponseType response = client.PrintDeparture(request);
                client.Close();

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(response), LogFileName("DataFromCentiro_PrintDeparture"));
                }

                if (response.StatusCode == CentiroService.StatusCode.Ok)
                {
                    return response;
                }
                else if (response.StatusCode == CentiroService.StatusCode.InvalidTicket)
                {
                    if (!_invalidTicketError)
                    {
                        RenewTicket(true);
                        _invalidTicketError = true;
                        return (PrintDeparture(request, dumpXml));
                    }
                    else
                        return (response);
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error for print Departure .\nError status = {0}\nError Message = {1}",
                                                                         response.StatusCode, GetErrorString(response.StatusMessages)));
                    throw new CentiroServiceAgentException(String.Format("Error for Print Departure .\nError status = {0}\nError Message = {1}",
                                                                         response.StatusCode, GetErrorString(response.StatusMessages)));
                }
            }

            catch (CommunicationException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling PrintDeparture.");
                client.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling PrintDeparture.");
                throw;
            }
        }

        public CloseDepartureResponseType CloseDeparture(CloseDepartureRequestType request, bool dumpXml)
        {

            WMSInterfaceClient client = new WMSInterfaceClient("BasicHttpBinding_IWMSInterface", _url);
            try
            {

                RenewTicket(false);
                request.Ticket = _ticket;

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(request), LogFileName("DataToCentiro_CloseDeparture"));
                }

                CloseDepartureResponseType response = client.CloseDeparture(request);
                client.Close();

                if (dumpXml)
                {
                    new XMLHelper().DumpToFile(new XMLHelper().InterfaceClassToXml(response), LogFileName("DataFromCentiro_CloseDeparture"));
                }

                if (response.StatusCode == CentiroService.StatusCode.Ok)
                {
                    return response;
                }
                else if (response.StatusCode == CentiroService.StatusCode.InvalidTicket)
                {
                    if (!_invalidTicketError)
                    {
                        RenewTicket(true);
                        _invalidTicketError = true;
                        return (CloseDeparture(request, dumpXml));
                    }
                    else
                        return response;
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Error for Close Departure .\nError status = {0}\nError Message = {1}",
                                                                         response.StatusCode, GetErrorString(response.StatusMessages)));
                    throw new CentiroServiceAgentException(String.Format("Error for Close Departure .\nError status = {0}\nError Message = {1}",
                                                                         response.StatusCode, GetErrorString(response.StatusMessages)));
                }
            }

            catch (CommunicationException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling CloseDeparture.");
                client.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, "Error calling CloseDeparture.");
                throw;
            }
        }



        public CentiroServiceAgent(string url, TraceSource traceSource)
        {
            this._url = url;
            this._traceSource = traceSource;
        }

    }

}
