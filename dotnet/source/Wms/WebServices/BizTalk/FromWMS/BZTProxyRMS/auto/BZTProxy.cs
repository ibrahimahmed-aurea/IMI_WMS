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
    public void Departure( string ChannelId, string ChannelRef, string TransactionId, DepartureDoc aDepartureDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      Departure part = new Departure();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aDepartureDoc = aDepartureDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(Departure), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+Departure, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void DepartureNode( string ChannelId, string ChannelRef, string TransactionId, DepartureNodeDoc aDepartureNodeDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      DepartureNode part = new DepartureNode();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aDepartureNodeDoc = aDepartureNodeDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(DepartureNode), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+DepartureNode, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void DepartureTransportType( string ChannelId, string ChannelRef, string TransactionId, DepartureTransportTypeDoc aDepartureTransportTypeDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      DepartureTransportType part = new DepartureTransportType();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aDepartureTransportTypeDoc = aDepartureTransportTypeDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(DepartureTransportType), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+DepartureTransportType, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void DepartureLoad( string ChannelId, string ChannelRef, string TransactionId, DepartureLoadDoc aDepartureLoadDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      DepartureLoad part = new DepartureLoad();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aDepartureLoadDoc = aDepartureLoadDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(DepartureLoad), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+DepartureLoad, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void ModifyDepartureLoad( string ChannelId, string ChannelRef, string TransactionId, ModifyDepartureLoadDoc aModifyDepartureLoadDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      ModifyDepartureLoad part = new ModifyDepartureLoad();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aModifyDepartureLoadDoc = aModifyDepartureLoadDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(ModifyDepartureLoad), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+ModifyDepartureLoad, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void RemoveDeparture( string ChannelId, string ChannelRef, string TransactionId, RemoveDepartureDoc aRemoveDepartureDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      RemoveDeparture part = new RemoveDeparture();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aRemoveDepartureDoc = aRemoveDepartureDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(RemoveDeparture), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+RemoveDeparture, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void RemoveDepartureNode( string ChannelId, string ChannelRef, string TransactionId, RemoveDepartureNodeDoc aRemoveDepartureNodeDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      RemoveDepartureNode part = new RemoveDepartureNode();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aRemoveDepartureNodeDoc = aRemoveDepartureNodeDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(RemoveDepartureNode), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+RemoveDepartureNode, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void RemoveDepartureTransportType( string ChannelId, string ChannelRef, string TransactionId, RemoveDepartureTransportTypeDoc aRemoveDepartureTransportTypeDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      RemoveDepartureTransportType part = new RemoveDepartureTransportType();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aRemoveDepartureTransportTypeDoc = aRemoveDepartureTransportTypeDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(RemoveDepartureTransportType), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+RemoveDepartureTransportType, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }


    [WebMethod]
    public void Confirm( string ChannelId, string ChannelRef, string TransactionId, ConfirmDoc aConfirmDoc )
    {
      // transform external object to packaged object understandable by BizTalk
      Confirm part = new Confirm();
      part.ChannelId = ChannelId;
      part.ChannelRef = ChannelRef;
      part.TransactionId = TransactionId;
      part.aConfirmDoc = aConfirmDoc;

      ArrayList inHeaders = null;
      ArrayList inoutHeaders = null;
      ArrayList inoutHeaderResponses = null;
      ArrayList outHeaderResponses = null;
      SoapUnknownHeader[] unknownHeaderResponses = null;

      // Parameter information
      object[] invokeParams = new object[] { part };
      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(Confirm), "part") };
      ParamInfo[] outParamInfos = null;

      // Define the assembly (port)
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // at least regarding your project name and the public access token
      string bodyTypeAssemblyQualifiedName = "YOUR_ORCHESTRATION_PROJECT.FromWMSschema+Confirm, YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=" +
        "92267163d02e63da";

      // BizTalk invocation
      // NOTE! This line is only a sample, it needs to be modified to match your orchestration
      // regarding the operation to call
      this.Invoke("Operation_1", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);
    }

  }
}
