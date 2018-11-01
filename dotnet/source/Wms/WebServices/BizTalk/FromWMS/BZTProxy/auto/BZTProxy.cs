/*
  File           : 

  Description    : Interface classes for BizTalk integration.
                   This code was generated, do not edit.

*/
using System;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using Microsoft.BizTalk.WebServices.ServerProxy;

using Imi.Wms.WebServices.ExternalInterface.BizTalk.XsdTypes;

namespace Imi.Wms.WebServices.ExternalInterface.BizTalk.Proxy
{
  [WebService(Namespace="http://im.se/wms/webservices/")]
  public sealed class WMSBizTalkProxyPort : ServerProxy
  {
    private void InitializeComponent()
    {
    }


    [WebMethod]
    public void DeliveryReceipt( string ChannelId, string ChannelRef, string TransactionId, DeliveryReceiptHeadDoc aDeliveryReceiptHeadDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      DeliveryReceipt part = new DeliveryReceipt();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aDeliveryReceiptHeadDoc = aDeliveryReceiptHeadDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(DeliveryReceipt), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+DeliveryReceipt, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void PickReceipt( string ChannelId, string ChannelRef, string TransactionId, PickReceiptHeadDoc aPickReceiptHeadDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      PickReceipt part = new PickReceipt();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aPickReceiptHeadDoc = aPickReceiptHeadDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(PickReceipt), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+PickReceipt, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void VendorReturnReceipt( string ChannelId, string ChannelRef, string TransactionId, ReturnReceiptHeadDoc aReturnReceiptHeadDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      VendorReturnReceipt part = new VendorReturnReceipt();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aReturnReceiptHeadDoc = aReturnReceiptHeadDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(VendorReturnReceipt), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+VendorReturnReceipt, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void InspectionReceipt( string ChannelId, string ChannelRef, string TransactionId, InspectionReceiptHeadDoc aInspectionReceiptHeadDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      InspectionReceipt part = new InspectionReceipt();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aInspectionReceiptHeadDoc = aInspectionReceiptHeadDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(InspectionReceipt), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+InspectionReceipt, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void InventoryChange( string ChannelId, string ChannelRef, string TransactionId, InventoryChangeLineDoc aInventoryChangeLineDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      InventoryChange part = new InventoryChange();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aInventoryChangeLineDoc = aInventoryChangeLineDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(InventoryChange), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+InventoryChange, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void BalanceAnswer( string ChannelId, string ChannelRef, string TransactionId, BalanceAnswerLineDoc aBalanceAnswerLineDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      BalanceAnswer part = new BalanceAnswer();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aBalanceAnswerLineDoc = aBalanceAnswerLineDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(BalanceAnswer), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+BalanceAnswer, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void ReturnedPackingMaterial( string ChannelId, string ChannelRef, string TransactionId, ReturnedPackingMaterialHeadDoc aReturnedPackingMaterialHeadDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      ReturnedPackingMaterial part = new ReturnedPackingMaterial();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aReturnedPackingMaterialHeadDoc = aReturnedPackingMaterialHeadDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(ReturnedPackingMaterial), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+ReturnedPackingMaterial, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void ASN( string ChannelId, string ChannelRef, string TransactionId, ASNHeadDoc aASNHeadDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      ASN part = new ASN();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aASNHeadDoc = aASNHeadDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(ASN), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+ASN, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void ConfirmationOfReceipt( string ChannelId, string ChannelRef, string TransactionId, ConfirmationOfReceiptHeadDoc aConfirmationOfReceiptHeadDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      ConfirmationOfReceipt part = new ConfirmationOfReceipt();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aConfirmationOfReceiptHeadDoc = aConfirmationOfReceiptHeadDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(ConfirmationOfReceipt), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+ConfirmationOfReceipt, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void InboundOrderCompleted( string ChannelId, string ChannelRef, string TransactionId, InboundOrderCompletedDoc aInboundOrderCompletedDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      InboundOrderCompleted part = new InboundOrderCompleted();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aInboundOrderCompletedDoc = aInboundOrderCompletedDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(InboundOrderCompleted), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+InboundOrderCompleted, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void TransportInstruction( string ChannelId, string ChannelRef, string TransactionId, TransportInstructionDoc aTransportInstructionDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      TransportInstruction part = new TransportInstruction();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aTransportInstructionDoc = aTransportInstructionDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(TransportInstruction), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+TransportInstruction, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void TransportPlan( string ChannelId, string ChannelRef, string TransactionId, TransportPlanHeadDoc aTransportPlanHeadDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      TransportPlan part = new TransportPlan();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aTransportPlanHeadDoc = aTransportPlanHeadDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(TransportPlan), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+TransportPlan, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }

  }
}
