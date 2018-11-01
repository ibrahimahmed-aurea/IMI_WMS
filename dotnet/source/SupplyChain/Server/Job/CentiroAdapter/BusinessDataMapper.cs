/*
 * Changelog:
 * 090407 318618 Enhanced Centiro Adapter
 */
using System;
using System.Text;
using System.Collections.Generic;
using Imi.SupplyChain.Server.Job.CentiroAdapter.DataEntities;
using System.IO;
using System.Xml.XPath;
using Imi.SupplyChain.Server.Job.CentiroAdapter.CentiroService;

namespace Imi.SupplyChain.Server.Job.CentiroAdapter
{
    public class BusinessDataMapper
    {
        private System.Globalization.NumberFormatInfo _numFormatInfo = new System.Globalization.NumberFormatInfo();

        public BusinessDataMapper()
        {
            _numFormatInfo.NumberDecimalSeparator = ".";
            _numFormatInfo.NumberGroupSeparator = "";
        }

        public PrintShipmentRequestType Map(CentiroPrintShipmentType report, string printerType, string documentType)
        {
            PrintShipmentRequestType request = new PrintShipmentRequestType();

            if (report.createCustomerSpec.Equals("1"))
            {
                request.CreateCustomerSpec = true;
            }
            else
                request.CreateCustomerSpec = false;

            if (report.createLabels.Equals("1"))
            {
                request.CreateLabels = true;
            }
            else
            {
                request.CreateLabels = false;
            }
            request.LabelPrinterType = printerType;

            if (report.createShipmentDoc.Equals("1"))
            {
                request.CreateShipmentDoc = true;
            }
            else
            {
                request.CreateShipmentDoc = false;
            }
            if (report.returnSequenceNumbers.Equals("1"))
            {
                request.ReturnSequenceNumbers = true;
            }
            else
            {
                request.ReturnSequenceNumbers = false;
            }
            if (report.returnTrackingUrls.Equals("1"))
            {
                request.ReturnTrackingUrls = true;
            }
            else
            {
                request.ReturnTrackingUrls = false;
            }
            request.DocumentType = (CentiroService.DocumentType)Enum.Parse(typeof(CentiroService.DocumentType), documentType);
            request.SenderCode = report.senderCode;
            request.ShipmentIdentifier = report.shipmentIdentifier;
            request.MessageId = Convert.ToString(DateTime.Now.Ticks);
            
            return request;
        }

        public PrintDepartureRequestType Map(CentiroPrintDepartureType report,string documentType)
        {
            if (report == null)
            {
                return (null);
            }
            PrintDepartureRequestType request = new PrintDepartureRequestType();
            if (report.createCarrierDoc.Equals("1"))
            {
                request.CreateCarrierDoc = true;
            }
            else
            {
                request.CreateCarrierDoc = false;
            }

            if (report.createCustomerSpec.Equals("1"))
            {
                request.CreateCustomerSpec = true;
            }
            else
            {
                request.CreateCustomerSpec = false;
            }
            request.DocumentType = (CentiroService.DocumentType)Enum.Parse(typeof(CentiroService.DocumentType), documentType); 
            request.RouteNo = report.routeNo;
            request.SenderCode = report.senderCode;
            request.TripNo = report.tripNo;
            request.MessageId = Convert.ToString(DateTime.Now.Ticks);
            return request;
        }

        public PrintParcelsRequestType Map(CentiroPrintParcels report,string printerType)
        {
            PrintParcelsRequestType request = new PrintParcelsRequestType();
            request.NumberOfTags = report.numberOfTags;
            request.LabelPrinterType =  printerType;

            if (report.returnSequenceNumbers.Equals("1"))
            {
                request.ReturnSequenceNumbers = true;
            }
            else
            {
                request.ReturnSequenceNumbers = false;
            }
            if (report.returnTrackingUrls.Equals("1"))
            {
                request.ReturnTrackingUrls = true;
            }
            else
            {
                request.ReturnTrackingUrls = false;
            }
            request.ShipmentIdentifier = report.shipmentIdentifier;
            request.SenderCode = report.senderCode;

            List<string> parcels = new List<string>();

            foreach(CentiroPrintParcelType p in report.Parcels.Parcel)
            {
                parcels.Add(p.parcelIdentifier);
            }
            request.ParcelIdentifierList = parcels;

            request.MessageId = Convert.ToString(DateTime.Now.Ticks);
            return request;
        }

        public UpdateParcelsRequestType Map(CentiroUpdateParcels report)
        {
            if (report == null)
            {
                return (null);
            }
            UpdateParcelsRequestType request = new UpdateParcelsRequestType();
            request.SenderCode = SafeSubstring(report.senderCode,0,50);
            request.ShipmentIdentifier = report.shipmentIdentifier;
            if (report.aggregateParcels.Equals("1"))
            {
                request.AggregateParcels = true;
            }
            else
            {
                request.AggregateParcels = false;
            }
            List<CentiroService.Parcel> l = new List<CentiroService.Parcel>();
            CentiroUpdateParcelsParcels  Parcels = report.Parcels;

            foreach (CentiroUpdateParcelType p in Parcels.Parcel)
            {
                CentiroService.Parcel Parcel = new CentiroService.Parcel();
                Parcel.DeliveryInstruction1 = SafeSubstring(p.deliveryInstructions, 0, 50);
                Parcel.DeliveryInstruction2 = SafeSubstring(p.deliveryInstructions, 50, 50);
                Parcel.DeliveryInstruction3 = SafeSubstring(p.deliveryInstructions, 100, 50);
                Parcel.DeliveryInstruction4 = SafeSubstring(p.deliveryInstructions, 150, 50);
                Parcel.Height = Convert.ToDouble(p.height, _numFormatInfo);
                Parcel.LastModifiedBy = p.lastModifiedBy;
                Parcel.Length = Convert.ToDouble(p.length, _numFormatInfo);
                Parcel.LoadingMeasure = Convert.ToDouble(p.loadingMeasure, _numFormatInfo);
                Parcel.NetWeight = Convert.ToDouble(p.netWeight, _numFormatInfo);
                Parcel.ParcelIdentifier = p.parcelIdentifier;
                Parcel.SequenceNo = p.sequenceNo;
                Parcel.SequenceNoSSCC = p.sequenceNoSSCC;
                Parcel.ShippingLocation = p.shippingLocation;
                Parcel.TransportInstruction1 = SafeSubstring(p.transportInstructions, 0, 50);
                Parcel.TransportInstruction2 = SafeSubstring(p.transportInstructions, 50, 50);
                Parcel.TransportInstruction3 = SafeSubstring(p.transportInstructions, 100, 50);
                Parcel.TypeOfGoods = p.typeOfGoods;
                Parcel.TypeOfPackage = p.typeOfPackage;
                Parcel.Weight = Convert.ToDouble(p.weight, _numFormatInfo);
                Parcel.Width = Convert.ToDouble(p.width, _numFormatInfo);
                Parcel.Volume = Convert.ToDouble(p.volume, _numFormatInfo);
                l.Add(Parcel);
            }

            request.Parcels = l;

            return request;

        }


        public UpdateShipmentRequestType Map(CentiroUpdateShipment report)
        {
            UpdateShipmentRequestType request = new UpdateShipmentRequestType();
            request.Shipment = new CentiroService.Shipment();
            request.Shipment.ModeOfTransport = new ModeOfTransport();
            request.Shipment.Receiver = new CentiroService.Receiver();
            request.Shipment.Addresses = new List<CentiroService.Address>();

            request.Shipment.CODAmount = Convert.ToDouble(report.cODAmount, _numFormatInfo);
            request.Shipment.CODCurrency = report.cODCurrency;
            request.Shipment.CODReference = report.cODReference;
            request.Shipment.DeliveryInstruction1 = SafeSubstring(report.deliveryInstruction, 0, 50);
            request.Shipment.DeliveryInstruction2 = SafeSubstring(report.deliveryInstruction, 50, 50);
            request.Shipment.DeliveryInstruction3 = SafeSubstring(report.deliveryInstruction, 100, 50);
            request.Shipment.DeliveryInstruction4 = SafeSubstring(report.deliveryInstruction, 150, 50);
            request.Shipment.FreightCost = Convert.ToDouble(report.freightCost, _numFormatInfo);
            request.Shipment.FreightCurrency = report.freightCurrency;
            request.Shipment.FreightPrice = Convert.ToDouble(report.freightPrice, _numFormatInfo);
            request.Shipment.InsuranceAmount = Convert.ToDouble(report.insuranceAmount, _numFormatInfo);
            request.Shipment.LastModifiedBy = report.lastModifiedBy;
            request.Shipment.LoadingMeasure = Convert.ToDouble(report.loadingMeasure, _numFormatInfo);
            request.Shipment.MemoField = report.memoField;
            request.Shipment.NumberOfEURPallets = report.numberOfEURPallets;
            request.Shipment.OrderNo = report.orderNo;
            request.Shipment.Reference = report.reference;
            request.Shipment.RouteNo = report.routeNo;
            request.Shipment.SenderCode = report.senderCode;
            request.Shipment.SequenceNo = report.sequenceNo;
            request.Shipment.ShipDate = report.shipDate;
            request.Shipment.ShipmentIdentifier = report.shipmentIdentity;
            request.Shipment.TermsOfDelivery = report.termsOfDelivery;
            switch (report.transportPayer)
            {
                case "1":
                    request.Shipment.TransportPayer = "P";
                    break;
                case "3":
                    request.Shipment.TransportPayer = "C";
                    break;
                case "4":
                    request.Shipment.TransportPayer = "B";
                    break;
                default:
                    request.Shipment.TransportPayer = "P";
                    break;
            }

            request.Shipment.TransportPayerCustNo = report.transportPayerCustNo;
            request.Shipment.TripNo = report.tripNo;
            request.Shipment.Weight = Convert.ToDouble(report.weight, _numFormatInfo);
            request.Shipment.Volume = Convert.ToDouble(report.volume, _numFormatInfo);
            request.Shipment.ModeOfTransport.Carrier = report.carrier;
            request.Shipment.ModeOfTransport.CarrierService = report.methodOfShipment;
            
            List<CentiroService.CarrierServiceAttribute> tsrList = new List<CentiroService.CarrierServiceAttribute>();
            
            if (report.tsrList.tsr != null)
            {
                foreach (tsrType t in report.tsrList.tsr)
                {
                    CentiroService.CarrierServiceAttribute a = new CentiroService.CarrierServiceAttribute();
                    a.Value = t.value;
                    a.Code = t.code;
                    tsrList.Add(a);
                }
            }

            if (tsrList.Count > 0)
            {
                request.Shipment.ModeOfTransport.CarrierServiceAttributes = (tsrList);
            }

            request.Shipment.Receiver.Address1 = report.receiver.address1;
            request.Shipment.Receiver.Address2 = report.receiver.address2;
            request.Shipment.Receiver.Address3 = report.receiver.address3;
            request.Shipment.Receiver.CellPhone = report.receiver.cellphone;
            request.Shipment.Receiver.City = report.receiver.city;
            request.Shipment.Receiver.Contact = report.receiver.contact;
            request.Shipment.Receiver.CustNo = report.receiver.custNo;
            request.Shipment.Receiver.Email = report.receiver.email;
            request.Shipment.Receiver.ISOCountry = report.receiver.iSOCountry;
            request.Shipment.Receiver.Name = report.receiver.name;
            request.Shipment.Receiver.PalletRegNo = report.receiver.palletRegNo;
            request.Shipment.Receiver.Phone = report.receiver.phone;
            request.Shipment.Receiver.State = report.receiver.state;
            request.Shipment.Receiver.VatNo = report.receiver.vatNo;
            request.Shipment.Receiver.ZipCode = report.receiver.zipcode;

            /* Add sender information to address list */
            if (report.senderAddress != null)
            {
                CentiroService.Address address = new CentiroService.Address();
                address.Address1 = report.senderAddress.address1;
                address.Address2 = report.senderAddress.address2;
                address.Address3 = report.senderAddress.address3;
                address.CellPhone = report.senderAddress.cellphone;
                address.City = report.senderAddress.city;
                address.Code = (CentiroService.AddressCode)Enum.Parse(typeof(CentiroService.AddressCode), report.senderAddress.code);
                address.Contact = report.senderAddress.contact;
                address.CustNo = report.senderAddress.custNo;
                address.Email = report.senderAddress.email;
                address.ISOCountry = report.senderAddress.iSOCountry;
                address.Name = report.senderAddress.name;
                address.Phone = report.senderAddress.phone;
                address.State = report.senderAddress.state;
                address.VatNo = report.senderAddress.vatNo;
                address.ZipCode = report.senderAddress.zipcode;

                request.Shipment.Addresses.Add(address);
            }
            request.MessageId = Convert.ToString(DateTime.Now.Ticks);
            

            return request;
        }


        public RemoveParcelsRequestType Map(CentiroRemoveParcels report)
        {
            RemoveParcelsRequestType request = new RemoveParcelsRequestType();
            if (report.aggregateParcels.Equals("1"))
            {
                request.AggregateParcels = true;
            }
            else
            {
                request.AggregateParcels = false;
            } 
            request.SenderCode = report.senderCode;
            request.ParcelIdentifierList = new List<string>();
            request.ShipmentIdentifier = report.shipmentIdentifier;
            foreach (CentiroRemoveParcelType p in report.Parcels.Parcel)
            {

                request.ParcelIdentifierList.Add(p.parcelIdentifier);

            }

            request.MessageId = Convert.ToString(DateTime.Now.Ticks);

            return request;
        }


        public CloseDepartureRequestType Map(CentiroCloseDepartureType report, string documentType)
        {
            CloseDepartureRequestType request = new CloseDepartureRequestType();

            request.SenderCode = report.senderCode;
            request.TripNo = report.tripNo;
            request.RouteNo = report.routeNo;
            
            if (report.createCarrierDoc.Equals("1"))
            {
                request.CreateCarrierDoc = true;
            }
            else
            {
                request.CreateCarrierDoc = false;
            }
            if (report.createCustomerSpec.Equals("1"))
            {
                request.CreateCustomerSpec = true;
            }
            else
            {
                request.CreateCustomerSpec = false;
            }
            if (report.createCustomerSpec.Equals("1"))
            {
                request.CreateCustomerSpec = true;
            }
            else
            {
                request.CreateCustomerSpec = false;
            }

            if (report.returnSequenceNumbers.Equals("1"))
            {
                request.ReturnSequenceNumbers = true;
            }
            else
            {
                request.ReturnSequenceNumbers = false;
            }
            if (report.returnTrackingUrls.Equals("1"))
            {
                request.ReturnTrackingUrls = true;
            }
            else
            {
                request.ReturnTrackingUrls = false;
            }
            request.MessageId = Convert.ToString(DateTime.Now.Ticks);

            request.DocumentType = (CentiroService.DocumentType)Enum.Parse(typeof(CentiroService.DocumentType), documentType);

            return request;
        }

        private string SafeSubstring(string s, int zeroBasedStart, int copyCount)
        {
            try
            {
                if (s != null)
                {
                    // check that any character is within string
                    if (s.Length > zeroBasedStart)
                    {
                        // check if all characters should be copied or only the current length.
                        
                        // remaining chars in string with regards to startchar
                        int remainingChars = s.Length - zeroBasedStart;

                        if (remainingChars >= copyCount)
                            return s.Substring(zeroBasedStart, copyCount);
                        else
                            return s.Substring(zeroBasedStart, remainingChars);
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
