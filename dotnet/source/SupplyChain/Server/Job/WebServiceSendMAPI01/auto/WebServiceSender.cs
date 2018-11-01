/*
  File           : 

  Description    : Internal classes for calling webservices with outgoing data.
                   This code was generated, do not edit.

*/
using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Services;

namespace Imi.SupplyChain.Server.Job.WebServiceSendMAPI
{
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Web.Services.WebServiceBindingAttribute(Name="InboundInterfaceSoap", Namespace="http://im.se/wms/webservices/")]
  public class SenderHandler : System.Web.Services.Protocols.SoapHttpClientProtocol
  {
    public string _Debug()
    {
      return "Generated on   : 2008-05-12 13:33:56\r\n" +
             "Generated by   : SWG\\olla@IMIPC1091\r\n" +
             "Generated in   : C:\\project\\views\\olla_dotnet_ss\\dotnet\\source\\CodeGenerators\\wscc\r\n";
    }

    public SenderHandler()
    {
      this.Url = "http://localhost/Inbound/auto/ExternalInterface.asmx"; // the Url ALWAYS need to have a value otherwise an exception occurs.
    }


    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://im.se/wms/webservices/MovementIn_01", RequestNamespace="http://im.se/wms/webservices/", ResponseNamespace="http://im.se/wms/webservices/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void MovementIn_01( string MHId, string TransactionId, MovementIn_01Doc aMovementIn_01Doc )
    {
      this.Invoke("MovementIn_01", new object[] {
        MHId,
        TransactionId,
        aMovementIn_01Doc});
      return;
    }

    public System.IAsyncResult BeginMovementIn_01( string MHId, string TransactionId, MovementIn_01Doc aMovementIn_01Doc, System.AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("MovementIn_01", new object[] {
        MHId,
        TransactionId,
        aMovementIn_01Doc}, callback, asyncState);
    }

    public void EndMovementIn_01(System.IAsyncResult asyncResult)
    {
      this.EndInvoke(asyncResult);
    }


    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://im.se/wms/webservices/MovementOut_01", RequestNamespace="http://im.se/wms/webservices/", ResponseNamespace="http://im.se/wms/webservices/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void MovementOut_01( string MHId, string TransactionId, MovementOut_01Doc aMovementOut_01Doc )
    {
      this.Invoke("MovementOut_01", new object[] {
        MHId,
        TransactionId,
        aMovementOut_01Doc});
      return;
    }

    public System.IAsyncResult BeginMovementOut_01( string MHId, string TransactionId, MovementOut_01Doc aMovementOut_01Doc, System.AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("MovementOut_01", new object[] {
        MHId,
        TransactionId,
        aMovementOut_01Doc}, callback, asyncState);
    }

    public void EndMovementOut_01(System.IAsyncResult asyncResult)
    {
      this.EndInvoke(asyncResult);
    }


    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://im.se/wms/webservices/Product_01", RequestNamespace="http://im.se/wms/webservices/", ResponseNamespace="http://im.se/wms/webservices/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void Product_01( string MHId, string TransactionId, Product_01Doc aProduct_01Doc )
    {
      this.Invoke("Product_01", new object[] {
        MHId,
        TransactionId,
        aProduct_01Doc});
      return;
    }

    public System.IAsyncResult BeginProduct_01( string MHId, string TransactionId, Product_01Doc aProduct_01Doc, System.AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("Product_01", new object[] {
        MHId,
        TransactionId,
        aProduct_01Doc}, callback, asyncState);
    }

    public void EndProduct_01(System.IAsyncResult asyncResult)
    {
      this.EndInvoke(asyncResult);
    }


    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://im.se/wms/webservices/StatusUpdate_01", RequestNamespace="http://im.se/wms/webservices/", ResponseNamespace="http://im.se/wms/webservices/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void StatusUpdate_01( string MHId, string TransactionId, StatusUpdate_01Doc aStatusUpdate_01Doc )
    {
      this.Invoke("StatusUpdate_01", new object[] {
        MHId,
        TransactionId,
        aStatusUpdate_01Doc});
      return;
    }

    public System.IAsyncResult BeginStatusUpdate_01( string MHId, string TransactionId, StatusUpdate_01Doc aStatusUpdate_01Doc, System.AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("StatusUpdate_01", new object[] {
        MHId,
        TransactionId,
        aStatusUpdate_01Doc}, callback, asyncState);
    }

    public void EndStatusUpdate_01(System.IAsyncResult asyncResult)
    {
      this.EndInvoke(asyncResult);
    }

  }
}