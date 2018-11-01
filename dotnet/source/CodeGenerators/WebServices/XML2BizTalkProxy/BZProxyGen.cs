using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Security.Principal;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2BizTalkProxy
{
  public class BZTProxyGen : GenericGenerator
  {
    private const string NewLine = "\r\n";

    public BZTProxyGen( MessageDefinition messageDef, Package packageDef ) :
      base( messageDef, packageDef ) {}

    private string MakeInterface( InterfaceType i )
    {
      string code_txt = "";

      code_txt += NewLine + "    [WebMethod]";
      code_txt += NewLine + "    public void " + i.name + "( string ChannelId, string ChannelRef, string TransactionId, " + i.structure + "Doc a" + i.structure + "Doc )";
      code_txt += NewLine + "    {";
      code_txt += NewLine + "      // transform external object to packaged object understandable by BizTalk";
      code_txt += NewLine + "      " + i.name + " part = new " + i.name + "();";
      code_txt += NewLine + "      part.ChannelId = ChannelId;";
      code_txt += NewLine + "      part.ChannelRef = ChannelRef;";
      code_txt += NewLine + "      part.TransactionId = TransactionId;";
      code_txt += NewLine + "      part.a" + i.structure + "Doc = a" + i.structure + "Doc;";
      code_txt += NewLine;
      code_txt += NewLine + "      ArrayList inHeaders = null;";
      code_txt += NewLine + "      ArrayList inoutHeaders = null;";
      code_txt += NewLine + "      ArrayList inoutHeaderResponses = null;";
      code_txt += NewLine + "      ArrayList outHeaderResponses = null;";
      code_txt += NewLine + "      SoapUnknownHeader[] unknownHeaderResponses = null;";
      code_txt += NewLine;
      code_txt += NewLine + "      // Parameter information";
      code_txt += NewLine + "      object[] invokeParams = new object[] { part };";
      code_txt += NewLine + "      ParamInfo[] inParamInfos = new ParamInfo[] { new ParamInfo(typeof(" + i.name + "), \"part\") };";
      code_txt += NewLine + "      ParamInfo[] outParamInfos = null;";
      code_txt += NewLine;
      code_txt += NewLine + "      // Define the assembly (port)";
      code_txt += NewLine + "      // NOTE! This line is only a sample, it needs to be modified to match your orchestration";
      code_txt += NewLine + "      // at least regarding your project name and the public access token";
      code_txt += NewLine + "      string bodyTypeAssemblyQualifiedName = \"YOUR_ORCHESTRATION_PROJECT.FromWMSschema+" + i.name + ", YOUR_ORCHESTRATION_PROJECT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=\" +";
      code_txt += NewLine + "        \"92267163d02e63da\";";
      code_txt += NewLine;
      code_txt += NewLine + "      // BizTalk invocation";
      code_txt += NewLine + "      // NOTE! This line is only a sample, it needs to be modified to match your orchestration";
      code_txt += NewLine + "      // regarding the operation to call";
      code_txt += NewLine + "      this.Invoke(\"Operation_1\", invokeParams, inParamInfos, outParamInfos, 0, bodyTypeAssemblyQualifiedName, inHeaders, inoutHeaders, out inoutHeaderResponses, out outHeaderResponses, null, null, null, out unknownHeaderResponses, true, false);";
      code_txt += NewLine + "    }";

      return code_txt;
    }

    private string MakeInterfaces()
    {
      string code_txt = "";

      foreach ( InterfaceType i in messageDef.interfaces )
      {
        code_txt += NewLine + MakeInterface( i );
        code_txt += NewLine;
      }

      return code_txt;
    }

    private string MakeExternalFile()
    {
      string code_txt = "";

      code_txt +=           "/*";
      code_txt += NewLine + "  File           : ";
      code_txt += NewLine + "";
      code_txt += NewLine + "  Description    : Interface classes for BizTalk integration.";
      code_txt += NewLine + "                   This code was generated, do not edit.";
      code_txt += NewLine + "";
      code_txt += NewLine + "*/";
      code_txt += NewLine + "using System;";
      code_txt += NewLine + "using System.Web.Services;";
      code_txt += NewLine + "using System.Web.Services.Protocols;";
      code_txt += NewLine + "using System.Collections;";
      code_txt += NewLine + "using Microsoft.BizTalk.WebServices.ServerProxy;";
      code_txt += NewLine;
      code_txt += NewLine + "using Imi.Wms.WebServices.ExternalInterface.BizTalk.XsdTypes;";
      code_txt += NewLine;
      code_txt += NewLine + "namespace " + GetNameSpace();
      code_txt += NewLine + "{";
      code_txt += NewLine + "  [WebService(Namespace=\"http://im.se/wms/webservices/\")]";
      code_txt += NewLine + "  public sealed class WMSBizTalkProxyPort : ServerProxy";
      code_txt += NewLine + "  {";
      code_txt += NewLine + "    private void InitializeComponent()";
      code_txt += NewLine + "    {";
      code_txt += NewLine + "    }";
      code_txt += NewLine;
      code_txt += MakeInterfaces();
      code_txt += NewLine + "  }";
      code_txt += NewLine + "}";
      return code_txt;
    }

    private string GetNameSpace()
    {
      return "Imi.Wms.WebServices.ExternalInterface.BizTalk.Proxy";
    }
  
    public override void GetOutPut( TextWriter tw )
    {
      tw.WriteLine( MakeExternalFile() );
    }
  }
}
