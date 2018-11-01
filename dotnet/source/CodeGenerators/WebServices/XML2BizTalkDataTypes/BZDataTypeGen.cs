using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Security.Principal;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2BizTalkDataTypes
{
  public class BZDataTypeGen : GenericGenerator
  {
    private const string NewLine = "\r\n";

    public BZDataTypeGen( MessageDefinition messageDef, Package packageDef ) :
      base( messageDef, packageDef ) {}

    private bool IsNullable( ParameterType p )
    {
      return ( p.fieldType != ParameterTypeFieldType.OpCode );
    }

    private string MakeExternalClass( string structure )
    {
      string code_txt = "";

      foreach ( StructureType st in messageDef.structures )
      {
        if ( st.name == structure )
        {
          ParameterType[] ht = GetUniqueParameterList( st );
          code_txt = GetClassAttribs();
          code_txt += NewLine + "  public class " + GetExternalTypeName( structure );
          code_txt += NewLine + "  {";

          // identify the basic parameters
          int MaxLenDataType = 0;
          int MaxLenName = 0;
          int MaxLenDBField = 0;

          foreach ( ParameterType p in ht )
          {
            if ( IsExternalField( p ) )
            {
              MaxLenDataType = Math.Max( MaxLenDataType, GetExternalDataType( p ).Length               );
              MaxLenName =     Math.Max( MaxLenName,     GetExternalName( p, st ).Length               );
              MaxLenDBField =  Math.Max( MaxLenDBField,  (p.originTable + "." + p.originColumn).Length );
            }
          }

          foreach ( ParameterType p in ht )
          {
            if ( IsExternalField( p ) )
            {
              if ( IsNullable( p ) )
                code_txt += NewLine + "    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]";
              else
                code_txt += NewLine + "    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]";

              code_txt += NewLine + "    public " + 
                GetExternalDataType( p ).PadRight(MaxLenDataType ) + " " +
                (GetExternalName( p, st ) + ";").PadRight(MaxLenName + 1 ) + " /" + "* " +
                ( p.originTable + "." + p.originColumn).PadRight(MaxLenDBField ) + " *" + "/";
              code_txt += NewLine;
            }
          }

          code_txt += NewLine;

          // identify child objects
          if ( st.children != null )
            foreach ( ChildType ct in st.children )
            {
              if ( ct.maxOccurrs == "1" )
                code_txt += NewLine + "    public " + GetExternalTypeName( ct.name ) + " a" + GetExternalTypeName( ct.name ) + ";";
              else
                code_txt += NewLine + "    public " + GetExternalTypeName( ct.name ) + "[] a" + GetExternalTypeName( ct.name ) + "s;";
            }

          code_txt += NewLine + "  }";
        }
      }
      return code_txt;
    }

    private string MakeExternalClasses()
    {
      string code_txt = "";

      foreach ( StructureType st in messageDef.structures )
      {
        code_txt += NewLine + MakeExternalClass( st.name );
        code_txt += NewLine;
      }

      return code_txt;
    }

    private string MakeInterfaceClass( InterfaceType i )
    {
      string code_txt = "";

      // Query
      code_txt += GetClassAttribs();
      code_txt += NewLine + "  public class " + i.name;
      code_txt += NewLine + "  {";
      code_txt += NewLine + "    public string ChannelId;";
      code_txt += NewLine + "    public string ChannelRef;";
      code_txt += NewLine + "    public string TransactionId;";
      code_txt += NewLine + "    public " + i.structure + "Doc a" + i.structure + "Doc;";
      code_txt += NewLine + "  }";
      code_txt += NewLine;

      // Resonse, always empty
      code_txt += GetClassAttribs();
      code_txt += NewLine + "  public class " + i.name + "Response";
      code_txt += NewLine + "  {";
      code_txt += NewLine + "  }";

      return code_txt;
    }

    private string MakeInterfaceClasses()
    {
      string code_txt = "";

      foreach ( InterfaceType i in messageDef.interfaces )
      {
        code_txt += NewLine + MakeInterfaceClass( i );
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
      code_txt += NewLine + "using System.Data;";
      code_txt += NewLine + "using System.Xml.Serialization;";
      code_txt += NewLine + "";
      code_txt += NewLine + "namespace " + GetNameSpace();
      code_txt += NewLine + "{";
      code_txt += NewLine + "  // Generated on   : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
      code_txt += NewLine + "  // Generated by   : " + WindowsIdentity.GetCurrent().Name + "@" + SystemInformation.ComputerName;
      code_txt += NewLine + "  // Generated in   : " + Environment.CurrentDirectory;
      code_txt += NewLine;
      code_txt += NewLine + "  [System.Xml.Serialization.XmlTypeAttribute(Namespace=\"http://im.se/wms/webservices/\")]";
      code_txt += NewLine + "  public class @int";
      code_txt += NewLine + "  {";
      code_txt += NewLine + "      [System.Xml.Serialization.XmlTextAttribute()]";
      code_txt += NewLine + "      public int Value;";
      code_txt += NewLine + "  }";
      code_txt += NewLine;
      code_txt += NewLine + "  [System.Xml.Serialization.XmlTypeAttribute(Namespace=\"http://im.se/wms/webservices/\")]";
      code_txt += NewLine + "  public class dateTime";
      code_txt += NewLine + "  {";
      code_txt += NewLine + "      [System.Xml.Serialization.XmlTextAttribute()]";
      code_txt += NewLine + "      public System.DateTime Value;";
      code_txt += NewLine + "  }";
      code_txt += NewLine;
      code_txt += NewLine + "  [System.Xml.Serialization.XmlTypeAttribute(Namespace=\"http://im.se/wms/webservices/\")]";
      code_txt += NewLine + "  public class @double";
      code_txt += NewLine + "  {";
      code_txt += NewLine + "      [System.Xml.Serialization.XmlTextAttribute()]";
      code_txt += NewLine + "      public System.Double Value;";
      code_txt += NewLine + "  }";
      code_txt += NewLine;
      code_txt += GetClassAttribs();
      code_txt += NewLine + "  public class WhoAmI";
      code_txt += NewLine + "  {";
      code_txt += NewLine + "  }";
      code_txt += NewLine;
      code_txt += GetClassAttribs();
      code_txt += NewLine + "  public class WhoAmIResponse";
      code_txt += NewLine + "  {";
      code_txt += NewLine + "    public string WhoAmIResult;";
      code_txt += NewLine + "  }";
      code_txt += NewLine;
      code_txt += MakeExternalClasses();
      code_txt += NewLine;
      code_txt += MakeInterfaceClasses();
      code_txt += NewLine + "}";
      return code_txt;
    }

    private string GetClassAttribs()
    {
      string code_txt = "";
      code_txt += NewLine + "  [System.Xml.Serialization.XmlTypeAttribute(Namespace=\"http://im.se/wms/webservices/\")]";
      code_txt += NewLine + "  [System.Xml.Serialization.XmlRootAttribute(Namespace=\"http://im.se/wms/webservices/\", IsNullable=false)]";
      return code_txt;
    }

    private string GetNameSpace()
    {
      return "Imi.Wms.WebServices.ExternalInterface.BizTalk.XsdTypes";
    }
  
    public override void GetOutPut( TextWriter tw )
    {
      tw.WriteLine( MakeExternalFile() );
    }
  }
}
