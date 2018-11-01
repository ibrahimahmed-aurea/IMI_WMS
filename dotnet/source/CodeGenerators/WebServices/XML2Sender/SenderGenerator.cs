using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Security.Principal;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2Sender
{
    public class SenderGenerator : GenericGenerator
    {
        private const string NewLine = "\r\n";
        private code_target code_target;
        private string nameSpace;

        public SenderGenerator(MessageDefinition messageDef, Package packageDef, code_target ct, String ns)
            : base(messageDef, packageDef)
        {
            code_target = ct;
            nameSpace = ns;
        }

        private string MakeInterface(InterfaceType i)
        {
            string code_txt = "";
            code_txt += NewLine + "    [System.Web.Services.Protocols.SoapDocumentMethodAttribute(\"http://im.se/wms/webservices/" + i.name + "\", RequestNamespace=\"http://im.se/wms/webservices/\", ResponseNamespace=\"http://im.se/wms/webservices/\", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]";

            switch (code_target)
            {
                case code_target.hapi:
                    code_txt += NewLine + "    public void " + i.name + "( string ChannelId, string ChannelRef, string TransactionId, " + GetExternalTypeName(i.structure) + " a" + GetExternalTypeName(i.structure) + " )";
                    code_txt += NewLine + "    {";
                    code_txt += NewLine + "      this.Invoke(\"" + i.name + "\", new object[] {";
                    code_txt += NewLine + "        ChannelId,";
                    code_txt += NewLine + "        ChannelRef,";
                    code_txt += NewLine + "        TransactionId,";
                    code_txt += NewLine + "        a" + GetExternalTypeName(i.structure) + "});";
                    code_txt += NewLine + "      return;";
                    code_txt += NewLine + "    }";
                    code_txt += NewLine;
                    code_txt += NewLine + "    public System.IAsyncResult Begin" + i.name + "( string ChannelId, string ChannelRef, string TransactionId, " + GetExternalTypeName(i.structure) + " a" + GetExternalTypeName(i.structure) + ", System.AsyncCallback callback, object asyncState)";
                    code_txt += NewLine + "    {";
                    code_txt += NewLine + "      return this.BeginInvoke(\"" + i.name + "\", new object[] {";
                    code_txt += NewLine + "        ChannelId,";
                    code_txt += NewLine + "        ChannelRef,";
                    code_txt += NewLine + "        TransactionId,";
                    code_txt += NewLine + "        a" + GetExternalTypeName(i.structure) + "}, callback, asyncState);";
                    code_txt += NewLine + "    }";
                    break;
                case code_target.msg:
                    code_txt += NewLine + "    public void " + i.name + "( string CommPartnerId, string TransactionId, " + GetExternalTypeName(i.structure) + " a" + GetExternalTypeName(i.structure) + " )";
                    code_txt += NewLine + "    {";
                    code_txt += NewLine + "      this.Invoke(\"" + i.name + "\", new object[] {";
                    code_txt += NewLine + "        CommPartnerId,";
                    code_txt += NewLine + "        TransactionId,";
                    code_txt += NewLine + "        a" + GetExternalTypeName(i.structure) + "});";
                    code_txt += NewLine + "      return;";
                    code_txt += NewLine + "    }";
                    code_txt += NewLine;
                    code_txt += NewLine + "    public System.IAsyncResult Begin" + i.name + "( string CommPartnerId, string TransactionId, " + GetExternalTypeName(i.structure) + " a" + GetExternalTypeName(i.structure) + ", System.AsyncCallback callback, object asyncState)";
                    code_txt += NewLine + "    {";
                    code_txt += NewLine + "      return this.BeginInvoke(\"" + i.name + "\", new object[] {";
                    code_txt += NewLine + "        CommPartnerId,";
                    code_txt += NewLine + "        TransactionId,";
                    code_txt += NewLine + "        a" + GetExternalTypeName(i.structure) + "}, callback, asyncState);";
                    code_txt += NewLine + "    }";
                    break;
                case code_target.mapi:
                    code_txt += NewLine + "    public void " + i.name + "( string MHId, string TransactionId, " + GetExternalTypeName(i.structure) + " a" + GetExternalTypeName(i.structure) + " )";
                    code_txt += NewLine + "    {";
                    code_txt += NewLine + "      this.Invoke(\"" + i.name + "\", new object[] {";
                    code_txt += NewLine + "        MHId,";
                    code_txt += NewLine + "        TransactionId,";
                    code_txt += NewLine + "        a" + GetExternalTypeName(i.structure) + "});";
                    code_txt += NewLine + "      return;";
                    code_txt += NewLine + "    }";
                    code_txt += NewLine;
                    code_txt += NewLine + "    public System.IAsyncResult Begin" + i.name + "( string MHId, string TransactionId, " + GetExternalTypeName(i.structure) + " a" + GetExternalTypeName(i.structure) + ", System.AsyncCallback callback, object asyncState)";
                    code_txt += NewLine + "    {";
                    code_txt += NewLine + "      return this.BeginInvoke(\"" + i.name + "\", new object[] {";
                    code_txt += NewLine + "        MHId,";
                    code_txt += NewLine + "        TransactionId,";
                    code_txt += NewLine + "        a" + GetExternalTypeName(i.structure) + "}, callback, asyncState);";
                    code_txt += NewLine + "    }";
                    break;
                default:
                    break;
            }

            code_txt += NewLine;
            code_txt += NewLine + "    public void End" + i.name + "(System.IAsyncResult asyncResult)";
            code_txt += NewLine + "    {";
            code_txt += NewLine + "      this.EndInvoke(asyncResult);";
            code_txt += NewLine + "    }";

            return code_txt;
        }

        private string MakeInterfaces()
        {
            string code_txt = "";

            foreach (InterfaceType i in messageDef.interfaces)
            {
                code_txt += NewLine + MakeInterface(i);
                code_txt += NewLine;
            }

            return code_txt;
        }

        private string MakeSenderFile()
        {
            string code_txt = "";

            code_txt += "/*";
            code_txt += NewLine + "  File           : ";
            code_txt += NewLine + "";
            code_txt += NewLine + "  Description    : Internal classes for calling webservices with outgoing data.";
            code_txt += NewLine + "                   This code was generated, do not edit.";
            code_txt += NewLine + "";
            code_txt += NewLine + "*/";
            code_txt += NewLine + "using System;";
            code_txt += NewLine + "using System.Diagnostics;";
            code_txt += NewLine + "using System.Xml.Serialization;";
            code_txt += NewLine + "using System.Web.Services.Protocols;";
            code_txt += NewLine + "using System.ComponentModel;";
            code_txt += NewLine + "using System.Web.Services;";
            code_txt += NewLine + "";
            code_txt += NewLine + "namespace " + nameSpace;
            code_txt += NewLine + "{";
            code_txt += NewLine + "  [System.Diagnostics.DebuggerStepThroughAttribute()]";
            code_txt += NewLine + "  [System.ComponentModel.DesignerCategoryAttribute(\"code\")]";
            code_txt += NewLine + "  [System.Web.Services.WebServiceBindingAttribute(Name=\"InboundInterfaceSoap\", Namespace=\"http://im.se/wms/webservices/\")]";
            code_txt += NewLine + "  public class SenderHandler : System.Web.Services.Protocols.SoapHttpClientProtocol";
            code_txt += NewLine + "  {";
            code_txt += NewLine + "    public string _Debug()";
            code_txt += NewLine + "    {";
            code_txt += NewLine + "      return \"Generated on   : " + DateTime.Now.ToShortDateString().Replace("\\", "\\\\") + " " + DateTime.Now.ToLongTimeString().Replace("\\", "\\\\") + "\\r\\n\" +";
            code_txt += NewLine + "             \"Generated by   : " + WindowsIdentity.GetCurrent().Name.Replace("\\", "\\\\") + "@" + SystemInformation.ComputerName + "\\r\\n\" +";
            code_txt += NewLine + "             \"Generated in   : " + Environment.CurrentDirectory.Replace("\\", "\\\\") + "\\r\\n\";";
            code_txt += NewLine + "    }";
            code_txt += NewLine;
            code_txt += NewLine + "    public SenderHandler()";
            code_txt += NewLine + "    {";
            code_txt += NewLine + "      this.Url = \"http://localhost/Inbound/auto/ExternalInterface.asmx\"; // the Url ALWAYS need to have a value otherwise an exception occurs.";
            code_txt += NewLine + "    }";
            code_txt += NewLine;
            code_txt += MakeInterfaces();
            code_txt += NewLine + "  }";
            code_txt += NewLine + "}";
            return code_txt;
        }

        public override void GetOutPut(TextWriter tw)
        {
            tw.WriteLine(MakeSenderFile());
        }
    }
}
