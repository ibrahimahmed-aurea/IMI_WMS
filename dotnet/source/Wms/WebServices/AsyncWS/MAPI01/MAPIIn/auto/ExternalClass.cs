/*
  File           : 

  Description    : Interface classes for inbound data.
                   This code was generated, do not edit.

*/
using System;
using System.Data;
using System.Xml.Serialization;

namespace Imi.Wms.WebServices.MAPIIn
{
  public class ExternalInterface
  {
    public string _Debug()
    {
      return "Generated on   : 2010-05-07 16:22:28\r\n" +
             "Generated by   : SWG\\olla@IMIPC1091\r\n" +
             "Generated in   : C:\\project\\views\\olla_5.2E.2_ss\\dotnet\\source\\CodeGenerators\\wscc\r\n";
    }
  }

  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/wms/webservices/")]
  public class MovementPickUp_01Doc
  {
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]
    public string        OPCODE;                   /* .               */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string        MaterialHandlingSystemId; /* MAPI_IN.MHID    */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<int> MovementOrder;            /* TRPASS.TRPORDID */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<int> SequenceNumber;           /* TRPASS.SEQNUM   */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string        MovementTaskStatus;       /* .               */


  }

  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/wms/webservices/")]
  public class MovementDrop_01Doc
  {
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]
    public string        OPCODE;                   /* .               */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string        MaterialHandlingSystemId; /* MAPI_IN.MHID    */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<int> MovementOrder;            /* TRPASS.TRPORDID */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public Nullable<int> SequenceNumber;           /* TRPASS.SEQNUM   */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string        MovementTaskStatus;       /* .               */


  }

  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/wms/webservices/")]
  public class HandlingUnitStatus_01Doc
  {
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]
    public string OPCODE;                   /* .                 */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string MaterialHandlingSystemId; /* MAPI_IN.MHID      */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string HandlingUnitId;           /* HU.HUID           */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string HandlingUnitInStatus;     /* HU.OPSTAT_IN_SUB  */

    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string HandlingUnitOutStatus;    /* HU.OPSTAT_OUT_SUB */


  }

}