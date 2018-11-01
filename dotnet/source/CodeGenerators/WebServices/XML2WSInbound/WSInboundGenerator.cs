using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Security.Principal;
using Imi.CodeGenerators.WebServices.Framework;
using Imi.Framework.Versioning;

namespace Imi.CodeGenerators.WebServices.XML2WSInbound
{
    public class WSInboundGenerator : GenericGenerator
    {
        private const string NewLine = "\r\n";
        private bool GenerateOneClassPerInterface = false;
        private bool genReceiver = false;
        private bool genSender = false;
        private code_target code_target;
        private string nameSpace;

        public WSInboundGenerator(MessageDefinition messageDef, Package packageDef, bool genReceiver, bool genSender, code_target ct, String ns)
            : base(messageDef, packageDef)
        {
            this.genReceiver = genReceiver;
            this.genSender = genSender;
            this.code_target = ct;
            nameSpace = ns;
        }

        private string MakeInterface(InterfaceType i)
        {
            string code_txt = "";

            if (GenerateOneClassPerInterface)
            {
                code_txt += NewLine + "  }";
                code_txt += NewLine;
                code_txt += NewLine + "  [WebService(Namespace=\"http://im.se/wms/webservices/\")]";
                code_txt += NewLine + "  public class " + i.name + " : WSBase";
                code_txt += NewLine + "  {";
                code_txt += NewLine + "    [WebMethod]";
                code_txt += NewLine + "    public void Process";
            }
            else
            {
                code_txt += NewLine + "    [WebMethod]";
                code_txt += NewLine + "    public void " + i.name;
            }

            switch (code_target)
            {
                case code_target.hapi:
                    code_txt += "( string ChannelId, string ChannelRef, string TransactionId, " + GetExternalTypeName(i.structure) + " a" + GetExternalTypeName(i.structure) + " )";
                    break;
                case code_target.msg:
                    code_txt += "( string CommPartnerId, string TransactionId, " + GetExternalTypeName(i.structure) + " a" + GetExternalTypeName(i.structure) + " )";
                    break;
                case code_target.mapi:
                    code_txt += "( string MHId, string TransactionId, " + GetExternalTypeName(i.structure) + " a" + GetExternalTypeName(i.structure) + " )";
                    break;
                default:
                    break;
            }

            code_txt += NewLine + "    {";
            code_txt += NewLine + "      EnterProc();";
            code_txt += NewLine;
            code_txt += NewLine + "      " + i.structure + "Insert a" + i.structure + "Handler;";
            code_txt += NewLine;
            code_txt += NewLine + "      try";
            code_txt += NewLine + "      {";

            switch (code_target)
            {
                case code_target.hapi:
                    code_txt += NewLine + "        MessageTransaction mt = BeginWebmethod( ChannelId, ChannelRef, TransactionId, \"" + i.HAPIObjectName + "\" );";
                    break;
                case code_target.msg:
                    code_txt += NewLine + "        MessageTransaction mt = BeginWebmethod( CommPartnerId, TransactionId, \"" + i.HAPIObjectName + "\" );";
                    break;
                case code_target.mapi:
                    code_txt += NewLine + "        MessageTransaction mt = BeginWebmethod( MHId, TransactionId, \"" + i.HAPIObjectName + "\" );";
                    break;
                default:
                    break;
            }

            code_txt += NewLine;
            code_txt += NewLine + "        try";
            code_txt += NewLine + "        {";
            code_txt += NewLine + "          a" + i.structure + "Handler = new " + i.structure + "Insert( this );";
            code_txt += NewLine + "        }";
            code_txt += NewLine + "        catch (Exception e)";
            code_txt += NewLine + "        {";
            code_txt += NewLine + "          Exception InternalError = new Exception( \"InternalError: Building insert handler\", e );";
            code_txt += NewLine + "          throw (InternalError);";
            code_txt += NewLine + "        }";
            code_txt += NewLine;
            code_txt += NewLine + "        try";
            code_txt += NewLine + "        {";
            code_txt += NewLine + "          if ( a" + GetExternalTypeName(i.structure) + " == null )";
            code_txt += NewLine + "          {";
            code_txt += NewLine + "            Exception InternalError = new Exception( \"DataError: Root object cannot be null\" );";
            code_txt += NewLine + "            throw (InternalError);";
            code_txt += NewLine + "          }";
            code_txt += NewLine + "          a" + i.structure + "Handler.Process( ref mt, null, a" + GetExternalTypeName(i.structure) + " );";
            code_txt += NewLine + "          GetDataBase().Commit();";
            switch (code_target)
            {
                case code_target.hapi:
                    break;
                case code_target.msg:
                    break;
                case code_target.mapi:
                    code_txt += NewLine + "          mt.Signal();";
                    break;
                default:
                    break;
            }
            code_txt += NewLine + "        }";
            code_txt += NewLine + "        catch (Exception e)";
            code_txt += NewLine + "        {";
            code_txt += NewLine + "          try";
            code_txt += NewLine + "          {";
            code_txt += NewLine + "            GetDataBase().Rollback();";
            code_txt += NewLine + "          }";
            code_txt += NewLine + "          catch (Exception)";
            code_txt += NewLine + "          {}";
            code_txt += NewLine + "          Exception InternalError = new Exception( \"DataError: Error processing data\", e );";
            code_txt += NewLine + "          throw (InternalError);";
            code_txt += NewLine + "        }";
            code_txt += NewLine + "      }";
            code_txt += NewLine;
            code_txt += NewLine + "      finally";
            code_txt += NewLine + "      {";
            code_txt += NewLine + "        EndWebmethod();";
            code_txt += NewLine + "      }";
            code_txt += NewLine;
            code_txt += NewLine + "      ExitProc();";
            code_txt += NewLine;
            code_txt += NewLine + "      return;";
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

        private string MakeInterfacesFile()
        {
            string code_txt = "";

            code_txt += "/*";
            code_txt += NewLine + "  File           : ";
            code_txt += NewLine + "";
            code_txt += NewLine + "  Description    : Public interface class for WebService interface for inbound data.";
            code_txt += NewLine + "                   This code was generated, do not edit.";
            code_txt += NewLine + "";
            code_txt += NewLine + "*/";

            code_txt += NewLine + "using System;";
            code_txt += NewLine + "using System.Web;";
            code_txt += NewLine + "using System.Web.Services;";
            code_txt += NewLine + "using System.Web.Services.Protocols;";
            code_txt += NewLine + "using System.Collections;";
            code_txt += NewLine + "using System.ComponentModel;";
            code_txt += NewLine + "using System.Data;";
            code_txt += NewLine + "using System.Diagnostics;";
            code_txt += NewLine + "using System.IO;";
            code_txt += NewLine + "using Imi.Framework.Versioning;";
            code_txt += NewLine + "";

            code_txt += NewLine + "namespace " + nameSpace;
            code_txt += NewLine + "{";

            switch (code_target)
            {
                case code_target.hapi:
                    code_txt += NewLine + "  [WebService(Namespace=\"http://im.se/wms/webservices/\", Description=\"WMS ";
                    break;
                case code_target.msg:
                    code_txt += NewLine + "  [WebService(Namespace=\"http://im.se/wms/webservices/\", Description=\"Message ";
                    break;
                case code_target.mapi:
                    code_txt += NewLine + "  [WebService(Namespace=\"http://im.se/wms/webservices/\", Description=\"MAPI ";
                    break;
                default:
                    break;
            }

            if (genReceiver)
            {
                code_txt += "Inbound (Receiver)";
            }
            else if (genSender)
            {
                code_txt += "Outbound (Sender)";
            }
            else
            {
            }


            code_txt += " interface ";
            code_txt += CurrentVersion.VersionName;
            code_txt += " generated on ";
            code_txt += DateTime.Now.ToShortDateString().Replace("\\", "\\\\");
            code_txt += " ";
            code_txt += DateTime.Now.ToLongTimeString().Replace("\\", "\\\\");
            code_txt += "\")]";
            code_txt += NewLine + "  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]";
            code_txt += NewLine + "  public class InboundInterface : WSBase";
            code_txt += NewLine + "  {";
            code_txt += NewLine + "    public string _Debug()";
            code_txt += NewLine + "    {";
            code_txt += NewLine + "      return \"Generated on   : " + DateTime.Now.ToShortDateString().Replace("\\", "\\\\") + " " + DateTime.Now.ToLongTimeString().Replace("\\", "\\\\") + "\\r\\n\" +";
            code_txt += NewLine + "             \"Generated by   : " + WindowsIdentity.GetCurrent().Name.Replace("\\", "\\\\") + "@" + SystemInformation.ComputerName + "\\r\\n\" +";
            code_txt += NewLine + "             \"Generated in   : " + Environment.CurrentDirectory.Replace("\\", "\\\\") + "\\r\\n\";";
            code_txt += NewLine + "    }";
            code_txt += NewLine;
            code_txt += NewLine + "    private void Log( bool Enter )";
            code_txt += NewLine + "    {";
            code_txt += NewLine;
            code_txt += NewLine + "      string path = @\"C:\\log\\Inbound.log\";";
            code_txt += NewLine;
            code_txt += NewLine + "      using (StreamWriter w = File.AppendText(path))";
            code_txt += NewLine + "      {";
            code_txt += NewLine + "        w.Write( \"{0} {1}.{2}\", System.DateTime.Now.ToShortDateString(), ";
            code_txt += NewLine + "          System.DateTime.Now.ToLongTimeString(), System.Convert.ToString( System.DateTime.Now.Millisecond ) );";
            code_txt += NewLine;
            code_txt += NewLine + "        StackTrace st = new StackTrace(1, true);";
            code_txt += NewLine;
            code_txt += NewLine + "        if ( st.FrameCount > 1 )";
            code_txt += NewLine + "        {";
            code_txt += NewLine + "          StackFrame sf = st.GetFrame(1);";
            code_txt += NewLine + "        ";
            code_txt += NewLine + "          w.Write(\"{0}{1}{2}\", '\\t', sf.GetMethod(), '\\t' );";
            code_txt += NewLine + "        }";
            code_txt += NewLine + "        ";
            code_txt += NewLine + "        if (Enter)";
            code_txt += NewLine + "          w.WriteLine( \"Enter\" );";
            code_txt += NewLine + "        else";
            code_txt += NewLine + "          w.WriteLine( \"Leave\" );";
            code_txt += NewLine + "      }";
            code_txt += NewLine + "    }";
            code_txt += NewLine;
            code_txt += NewLine + "    private void EnterProc()";
            code_txt += NewLine + "    {";
            code_txt += NewLine + "      try";
            code_txt += NewLine + "      {";
            code_txt += NewLine + "        // Log( true );";
            code_txt += NewLine + "      }";
            code_txt += NewLine + "      catch";
            code_txt += NewLine + "      {";
            code_txt += NewLine + "      }";
            code_txt += NewLine + "    }";
            code_txt += NewLine;
            code_txt += NewLine + "    private void ExitProc()";
            code_txt += NewLine + "    {";
            code_txt += NewLine + "      try";
            code_txt += NewLine + "      {";
            code_txt += NewLine + "        // Log( false );";
            code_txt += NewLine + "      }";
            code_txt += NewLine + "      catch";
            code_txt += NewLine + "      {";
            code_txt += NewLine + "      }";
            code_txt += NewLine + "    }";
            code_txt += NewLine;
            code_txt += NewLine + "    [WebMethod]";
            code_txt += NewLine + "    public string WhoAmI()";
            code_txt += NewLine + "    {";
            code_txt += NewLine + "      EnterProc();";
            code_txt += NewLine;
            code_txt += NewLine + "      string s = CurrentVersion.VersionName;";
            code_txt += NewLine;
            code_txt += NewLine + "      ExitProc();";
            code_txt += NewLine;
            code_txt += NewLine + "      return s;";
            code_txt += NewLine + "    }";
            code_txt += NewLine;
            code_txt += MakeInterfaces();
            code_txt += NewLine + "  }";
            code_txt += NewLine + "}";
            return code_txt;
        }

        public override void GetOutPut(TextWriter tw)
        {
            tw.WriteLine(MakeInterfacesFile());
        }
    }
}
