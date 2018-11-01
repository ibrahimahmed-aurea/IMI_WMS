/*
  File           : 

  Description    : Interface classes for inbound data.
                   This code was generated, do not edit.

*/
using System;
using System.Data;
using System.Xml.Serialization;

namespace Imi.Wms.WebServices.ExternalInterface
{
  public class ExternalInterface
  {
    public string _Debug()
    {
      return "Generated on   : 2017-09-08 11:58:09\r\n" +
             "Generated by   : SWG\\aron@SE0133D\r\n" +
             "Generated in   : C:\\projects\\views\\aron_80M_ss\\dotnet\\source\\CodeGenerators\\wscc\r\n";
    }
  }

  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/wms/webservices/")]
  public class DepartureDoc
  {
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]
    public string             OPCODE;                       /* .                                              */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             DepartureIdentity;            /* MSG_OUT_DEPARTURE.DEPARTUREIDENTITY            */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             DepartureIdentityReference;   /* MSG_OUT_DEPARTURE.DEPARTUREIDENTITYREFERENCE   */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             FromNodeIdentity;             /* MSG_OUT_DEPARTURE.FROMNODEIDENTITY             */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             DeliveryMethod;               /* MSG_OUT_DEPARTURE.DELIVERYMETHOD               */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             RouteIdentity;                /* MSG_OUT_DEPARTURE.ROUTEIDENTITY                */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             RouteDescription;             /* MSG_OUT_DEPARTURE.ROUTEDESCRIPTION             */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> CustomerOrderReleaseDateTime; /* MSG_OUT_DEPARTURE.CUSTOMERORDERRELEASEDATETIME */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> CustomerOrderStopDateTime;    /* MSG_OUT_DEPARTURE.CUSTOMERORDERSTOPDATETIME    */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> TransitStopDateTime;          /* MSG_OUT_DEPARTURE.TRANSITSTOPDATETIME          */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> PlannedDepartureDateTime;     /* MSG_OUT_DEPARTURE.PLANNEDDEPARTUREDATETIME     */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             CheckProductTransportType;    /* MSG_OUT_DEPARTURE.CHECKPRODUCTTRANSPORTTYPE    */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             VehicleIdentity;              /* MSG_OUT_DEPARTURE.VEHICLEIDENTITY              */


  }

  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/wms/webservices/")]
  public class DepartureNodeDoc
  {
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]
    public string             OPCODE;                   /* .                                               */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             DepartureIdentity;        /* MSG_OUT_DEPARTURE_NODE.DEPARTUREIDENTITY        */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<int>      SEQNUM;                   /* MSG_OUT_DEPARTURE_NODE.SEQNUM                   */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             ToNodeIdentity;           /* MSG_OUT_DEPARTURE_NODE.TONODEIDENTITY           */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> EstimatedArrivalDateTime; /* MSG_OUT_DEPARTURE_NODE.ESTIMATEDARRIVALDATETIME */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             ReceiveTransitGoods;      /* MSG_OUT_DEPARTURE_NODE.RECEIVETRANSITGOODS      */


  }

  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/wms/webservices/")]
  public class DepartureTransportTypeDoc
  {
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]
    public string OPCODE;                   /* .                                              */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string DepartureIdentity;        /* MSG_OUT_DEPARTURE_TRP.DEPARTUREIDENTITY        */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string ProductTransportIdentity; /* MSG_OUT_DEPARTURE_TRP.PRODUCTTRANSPORTIDENTITY */


  }

  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/wms/webservices/")]
  public class DepartureLoadDoc
  {
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]
    public string             OPCODE;                       /* .                                            */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             DepartureLoadIdentity;        /* MSG_OUT_DEPLOAD.DEPARTURELOADIDENTITY        */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<double>   CustomerOrderSequence;        /* MSG_OUT_DEPLOAD.CUSTOMERORDERSEQUENCE        */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             DepartureIdentity;            /* MSG_OUT_DEPLOAD.DEPARTUREIDENTITY            */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             RouteIdentity;                /* MSG_OUT_DEPLOAD.ROUTEIDENTITY                */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             RouteDescription;             /* MSG_OUT_DEPLOAD.ROUTEDESCRIPTION             */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> CustomerOrderReleaseDateTime; /* MSG_OUT_DEPLOAD.CUSTOMERORDERRELEASEDATETIME */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> CustomerOrderStopDateTime;    /* MSG_OUT_DEPLOAD.CUSTOMERORDERSTOPDATETIME    */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> PlannedDepartureDateTime;     /* MSG_OUT_DEPLOAD.PLANNEDDEPARTUREDATETIME     */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> EstimatedArrivalDateTime;     /* MSG_OUT_DEPLOAD.ESTIMATEDARRIVALDATETIME     */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             LastChainDepartureIdentity;   /* MSG_OUT_DEPLOAD.LASTCHAINDEPARTUREIDENTITY   */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             WhyNoDepartureMessage;        /* MSG_OUT_DEPLOAD.WHYNODEPARTUREMESSAGE        */


  }

  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/wms/webservices/")]
  public class ModifyDepartureLoadDoc
  {
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]
    public string             OPCODE;                       /* .                                                   */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             DepartureLoadIdentity;        /* MSG_OUT_MODIFY_DEPLOAD.DEPARTURELOADIDENTITY        */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             FromNodeIdentity;             /* MSG_OUT_MODIFY_DEPLOAD.FROMNODEIDENTITY             */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             DepartureIdentity;            /* MSG_OUT_MODIFY_DEPLOAD.DEPARTUREIDENTITY            */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             RouteIdentity;                /* MSG_OUT_MODIFY_DEPLOAD.ROUTEIDENTITY                */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string             RouteDescription;             /* MSG_OUT_MODIFY_DEPLOAD.ROUTEDESCRIPTION             */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> CustomerOrderReleaseDateTime; /* MSG_OUT_MODIFY_DEPLOAD.CUSTOMERORDERRELEASEDATETIME */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> CustomerOrderStopDateTime;    /* MSG_OUT_MODIFY_DEPLOAD.CUSTOMERORDERSTOPDATETIME    */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> PlannedDepartureDateTime;     /* MSG_OUT_MODIFY_DEPLOAD.PLANNEDDEPARTUREDATETIME     */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<DateTime> EstimatedArrivalDateTime;     /* MSG_OUT_MODIFY_DEPLOAD.ESTIMATEDARRIVALDATETIME     */


  }

  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/wms/webservices/")]
  public class RemoveDepartureDoc
  {
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]
    public string OPCODE;            /* .                                    */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string DepartureIdentity; /* MSG_OUT_REMOVE_DEP.DEPARTUREIDENTITY */


  }

  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/wms/webservices/")]
  public class RemoveDepartureNodeDoc
  {
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]
    public string        OPCODE;            /* .                                         */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string        DepartureIdentity; /* MSG_OUT_REMOVE_DEP_NODE.DEPARTUREIDENTITY */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<int> SEQNUM;            /* MSG_OUT_REMOVE_DEP_NODE.SEQNUM            */


  }

  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/wms/webservices/")]
  public class RemoveDepartureTransportTypeDoc
  {
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]
    public string OPCODE;                   /* .                                               */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string DepartureIdentity;        /* MSG_OUT_REMOVE_DEP_TRP.DEPARTUREIDENTITY        */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string ProductTransportIdentity; /* MSG_OUT_REMOVE_DEP_TRP.PRODUCTTRANSPORTIDENTITY */


  }

  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/wms/webservices/")]
  public class ConfirmDoc
  {
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]
    public string OPCODE;            /* .                                  */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string TransactionId;     /* MSG_OUT_CONFIRM.TRANSACTION_ID     */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string TransactionStatus; /* MSG_OUT_CONFIRM.TRANSACTION_STATUS */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string ErrCode;           /* MSG_OUT_CONFIRM.ERRCODE            */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string ErrMsg;            /* MSG_OUT_CONFIRM.ERRMSG             */


  }

}
