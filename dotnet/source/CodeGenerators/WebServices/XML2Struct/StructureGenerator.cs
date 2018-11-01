using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Security.Principal;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2Struct
{
    public class StructureGenerator : GenericGenerator
    {
        private const string NewLine = "\r\n";
        private code_target code_target;
        private string nameSpace;

        public StructureGenerator(MessageDefinition messageDef, Package packageDef, code_target ct, String ns)
            : base(messageDef, packageDef)
        {
            code_target = ct;
            nameSpace = ns;
        }

        // we had painted us into a corner...
        protected override bool IsClientIdentity(ParameterType p)
        {
            if (code_target == code_target.hapi)
            {
                return (p.name.ToUpper() == ("ClientIdentity").ToUpper());
            }
            else
            {
                return false;
            }
        }

        private bool IsNullable(ParameterType p)
        {
            return (p.fieldType != ParameterTypeFieldType.OpCode);
        }

        private string MakeExternalClass(string structure)
        {
            string code_txt = "";

            foreach (StructureType st in messageDef.structures)
            {
                if (st.name == structure)
                {
                    ParameterType[] ht = GetUniqueParameterList(st);
                    code_txt = "  [System.Xml.Serialization.XmlTypeAttribute(Namespace=\"http://im.se/wms/webservices/\")]";
                    code_txt += NewLine + "  public class " + GetExternalTypeName(structure);
                    code_txt += NewLine + "  {";

                    // identify the basic parameters
                    int MaxLenDataType = 0;
                    int MaxLenName = 0;
                    int MaxLenDBField = 0;

                    foreach (ParameterType p in ht)
                    {
                        if (IsExternalField(p))
                        {
                            MaxLenDataType = Math.Max(MaxLenDataType, GetExternalDataType(p).Length);
                            MaxLenName = Math.Max(MaxLenName, GetExternalName(p, st).Length);
                            MaxLenDBField = Math.Max(MaxLenDBField, (p.originTable + "." + p.originColumn).Length);
                        }
                    }

                    foreach (ParameterType p in ht)
                    {
                        if (IsExternalField(p))
                        {
                            if (IsExcluded(p.name, st))
                            {
                                code_txt += NewLine + "    [System.Xml.Serialization.XmlIgnoreAttribute]";                                
                            }
                            else
                            {
                              if (IsNullable(p))
                                  code_txt += NewLine + "    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]";
                              else
                                  code_txt += NewLine + "    [System.Xml.Serialization.XmlElementAttribute(IsNullable=false)]";
                            }
                            
                            code_txt += NewLine + "    public " +
                              GetExternalDataType(p).PadRight(MaxLenDataType) + " " +
                              (GetExternalName(p, st) + ";").PadRight(MaxLenName + 1) + " /" + "* " +
                              (p.originTable + "." + p.originColumn).PadRight(MaxLenDBField) + " *" + "/";
                            code_txt += NewLine;
                        }
                    }

                    code_txt += NewLine;

                    // identify child objects
                    if (st.children != null)
                        foreach (ChildType ct in st.children)
                        {
                            if (ct.maxOccurrs == "1")
                                code_txt += NewLine + "    public " + GetExternalTypeName(ct.name) + " a" + GetExternalTypeName(ct.name) + ";";
                            else
                                code_txt += NewLine + "    public " + GetExternalTypeName(ct.name) + "[] a" + GetExternalTypeName(ct.name) + "s;";
                        }

                    code_txt += NewLine + "  }";
                }
            }
            return code_txt;
        }

        private string MakeExternalClasses()
        {
            string code_txt = "";

            foreach (StructureType st in messageDef.structures)
            {
                code_txt += NewLine + MakeExternalClass(st.name);
                code_txt += NewLine;
            }

            return code_txt;
        }

        private string MakeExternalFile()
        {
            string code_txt = "";

            code_txt += "/*";
            code_txt += NewLine + "  File           : ";
            code_txt += NewLine + "";
            code_txt += NewLine + "  Description    : Interface classes for inbound data.";
            code_txt += NewLine + "                   This code was generated, do not edit.";
            code_txt += NewLine + "";
            code_txt += NewLine + "*/";
            code_txt += NewLine + "using System;";
            code_txt += NewLine + "using System.Data;";
            code_txt += NewLine + "using System.Xml.Serialization;";
            code_txt += NewLine + "";
            code_txt += NewLine + "namespace " + nameSpace;
            code_txt += NewLine + "{";
            code_txt += NewLine + "  public class ExternalInterface";
            code_txt += NewLine + "  {";
            code_txt += NewLine + "    public string _Debug()";
            code_txt += NewLine + "    {";
            code_txt += NewLine + "      return \"Generated on   : " + DateTime.Now.ToShortDateString().Replace("\\", "\\\\") + " " + DateTime.Now.ToLongTimeString().Replace("\\", "\\\\") + "\\r\\n\" +";
            code_txt += NewLine + "             \"Generated by   : " + WindowsIdentity.GetCurrent().Name.Replace("\\", "\\\\") + "@" + SystemInformation.ComputerName + "\\r\\n\" +";
            code_txt += NewLine + "             \"Generated in   : " + Environment.CurrentDirectory.Replace("\\", "\\\\") + "\\r\\n\";";
            code_txt += NewLine + "    }";
            code_txt += NewLine + "  }";
            code_txt += NewLine;
            code_txt += MakeExternalClasses();
            code_txt += NewLine + "}";
            return code_txt;
        }

        public override void GetOutPut(TextWriter tw)
        {
            tw.WriteLine(MakeExternalFile());
        }
    }
}
