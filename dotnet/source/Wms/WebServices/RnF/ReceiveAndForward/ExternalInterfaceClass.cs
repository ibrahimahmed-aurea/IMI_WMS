using System;
using System.Collections;
using Imi.Wms.WebServices.ExternalInterface;

namespace Wms.WebServices.OutboundMapper.ReceiveAndForward
{
    [System.Xml.Serialization.XmlRootAttribute("DeliveryReceipt", Namespace = "http://im.se/wms/webservices/", IsNullable = false)]
    public class DeliveryReceipt : IInterfaceClass
    {
        public string ChannelId;
        public string ChannelRef;
        public string TransactionId;
        public DeliveryReceiptHeadDoc aDeliveryReceiptHeadDoc;
    }

    [System.Xml.Serialization.XmlRootAttribute("PickReceipt", Namespace = "http://im.se/wms/webservices/", IsNullable = false)]
    public class PickReceipt : IInterfaceClass
    {
        public string ChannelId;
        public string ChannelRef;
        public string TransactionId;
        public PickReceiptHeadDoc aPickReceiptHeadDoc;
    }

    [System.Xml.Serialization.XmlRootAttribute("VendorReturnReceipt", Namespace = "http://im.se/wms/webservices/", IsNullable = false)]
    public class VendorReturnReceipt : IInterfaceClass
    {
        public string ChannelId;
        public string ChannelRef;
        public string TransactionId;
        public ReturnReceiptHeadDoc aReturnReceiptHeadDoc;
    }

    [System.Xml.Serialization.XmlRootAttribute("InspectionReceipt", Namespace = "http://im.se/wms/webservices/", IsNullable = false)]
    public class InspectionReceipt : IInterfaceClass
    {
        public string ChannelId;
        public string ChannelRef;
        public string TransactionId;
        public InspectionReceiptHeadDoc aInspectionReceiptHeadDoc;
    }

    [System.Xml.Serialization.XmlRootAttribute("InventoryChange", Namespace = "http://im.se/wms/webservices/", IsNullable = false)]
    public class InventoryChange : IInterfaceClass
    {
        public string ChannelId;
        public string ChannelRef;
        public string TransactionId;
        public InventoryChangeLineDoc aInventoryChangeLineDoc;
    }

    [System.Xml.Serialization.XmlRootAttribute("BalanceAnswer", Namespace = "http://im.se/wms/webservices/", IsNullable = false)]
    public class BalanceAnswer : IInterfaceClass
    {
        public string ChannelId;
        public string ChannelRef;
        public string TransactionId;
        public BalanceAnswerLineDoc aBalanceAnswerLineDoc;
    }

    [System.Xml.Serialization.XmlRootAttribute("ReturnedPackingMaterial", Namespace = "http://im.se/wms/webservices/", IsNullable = false)]
    public class ReturnedPackingMaterial : IInterfaceClass
    {
        public string ChannelId;
        public string ChannelRef;
        public string TransactionId;
        public ReturnedPackingMaterialHeadDoc aReturnedPackingMaterialHeadDoc;
    }

    [System.Xml.Serialization.XmlRootAttribute("ASN", Namespace = "http://im.se/wms/webservices/", IsNullable = false)]
    public class ASN : IInterfaceClass
    {
        public string ChannelId;
        public string ChannelRef;
        public string TransactionId;
        public ASNHeadDoc aASNHeadDoc;
    }

    [System.Xml.Serialization.XmlRootAttribute("ConfirmationOfReceipt", Namespace = "http://im.se/wms/webservices/", IsNullable = false)]
    public class ConfirmationOfReceipt : IInterfaceClass
    {
        public string ChannelId;
        public string ChannelRef;
        public string TransactionId;
        public ConfirmationOfReceiptHeadDoc aConfirmationOfReceiptHeadDoc;
    }

    [System.Xml.Serialization.XmlRootAttribute("InboundOrderCompleted", Namespace = "http://im.se/wms/webservices/", IsNullable = false)]
    public class InboundOrderCompleted : IInterfaceClass
    {
        public string ChannelId;
        public string ChannelRef;
        public string TransactionId;
        public InboundOrderCompletedDoc aInboundOrderCompletedDoc;
    }

    [System.Xml.Serialization.XmlRootAttribute("TransportInstruction", Namespace = "http://im.se/wms/webservices/", IsNullable = false)]
    public class TransportInstruction : IInterfaceClass
    {
        public string ChannelId;
        public string ChannelRef;
        public string TransactionId;
        public TransportInstructionDoc aTransportInstructionDoc;
    }
}
