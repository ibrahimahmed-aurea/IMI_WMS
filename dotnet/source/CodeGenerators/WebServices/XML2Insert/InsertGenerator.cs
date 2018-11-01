using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Security.Principal;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2Insert
{
    public class InsertGenerator : GenericGenerator
    {
        private const string NewLine = "\r\n";
        private code_target code_target;
        private string nameSpace;

        public InsertGenerator(MessageDefinition messageDef, Package packageDef, code_target ct, String ns)
            : base(messageDef, packageDef)
        {
            code_target = ct;
            nameSpace = ns;
        }

        private bool IsInsertField(ParameterType p)
        {
            return ((p.fieldType == ParameterTypeFieldType.SystemId) ||
              (p.fieldType == ParameterTypeFieldType.OpCode) ||
              (p.fieldType == ParameterTypeFieldType.Normal));
        }

        private string Converter(ParameterType p)
        {
            string s = "";
            switch (GetDataType(p).dataType)
            {
                case DataType.StringType:
                    s = "StringValue";
                    break;
                case DataType.DoubleType:
                case DataType.IntegerType:
                    s = "NumberValue";
                    break;
                case DataType.DateTimeType:
                    s = "DateValue  ";
                    break;
            }
            return s;
        }

        private string MakeInsertConstructor(StructureType st)
        {
            string code_txt = "";
            ParameterType[] ht = GetUniqueParameterList(st);

            code_txt += NewLine + "    public " + st.name + "Insert(WSBase owner)";
            code_txt += NewLine + "        : base(owner)";
            code_txt += NewLine + "    {";

            // if no queue table no inserts can be done
            if (st.queueTable != "")
            {
                code_txt += NewLine + "        StringBuilder s = new StringBuilder(\"insert into " + GetTableName(st.queueTable) + " ( \");";

                int MaxLenName = 0;
                foreach (ParameterType p in ht)
                {
                    if (IsInsertField(p))
                    {
                        MaxLenName = Math.Max(MaxLenName, (p.name).Length);
                    }
                }

                string First = " ";
                string s;
                foreach (ParameterType p in ht)
                {
                    if (IsInsertField(p))
                    {
                        s = "\"" + First + p.name + "\"";
                        code_txt += NewLine + "        s.Append(" + s + ");";

                        First = ",";
                    }
                }

                code_txt += NewLine;
                code_txt += NewLine + "        s.Append(\" ) values ( \");";
                code_txt += NewLine;

                First = " ";
                foreach (ParameterType p in ht)
                {
                    if (IsInsertField(p))
                    {
                        s = "\"" + First + ":" + p.name + "\"";
                        code_txt += NewLine + "        s.Append(" + s + ");";

                        First = ",";
                    }
                }

                code_txt += NewLine;
                code_txt += NewLine + "        s.Append(\" )\");";
                code_txt += NewLine;
                code_txt += NewLine + "        fStmt.CommandText = s.ToString();";
                code_txt += NewLine;

                foreach (ParameterType p in ht)
                {
                    if (IsInsertField(p))
                    {
                        code_txt += NewLine + "        fStmt.Parameters.Add(";
                        s = "\"" + p.name + "\"";
                        DataType dt = GetDataType(p);
                        switch (dt.dataType)
                        {
                            case DataType.StringType:
                                s = (s + ",") + " ";
                                code_txt += "StringParam(" + s + Convert.ToString(dt.length) + ")";
                                break;
                            case DataType.DoubleType:
                            case DataType.IntegerType:
                                s = (s + ",") + " ";
                                code_txt += "NumberParam(" + s + Convert.ToString(dt.precision) + ", " + Convert.ToString(dt.scale) + ")";
                                break;
                            case DataType.DateTimeType:
                                code_txt += "DateParam(" + s + ")";
                                break;
                        }

                        code_txt += ");";
                    }
                }

                code_txt += NewLine;
                code_txt += NewLine + "        fStmt.Prepare();";
                code_txt += NewLine;
            }

            // create the child insert objects
            if (st.children != null)
                foreach (ChildType ct in st.children)
                    code_txt += NewLine + "        a" + ct.name + "Insert = new " + ct.name + "Insert(owner);";

            code_txt += NewLine + "    }";
            return code_txt;
        }

        private string MakeInsertProcess(StructureType st)
        {
            string code_txt = "";
            ParameterType[] ht = GetUniqueParameterList(st);
            code_txt += NewLine + "    public void Process(ref MessageTransaction trans, SegmentImpl parent, ";
            code_txt += GetExternalTypeName(st.name) + " p)";
            code_txt += NewLine + "    {";
            code_txt += NewLine + "        StringBuilder error = new StringBuilder();";
            code_txt += NewLine;
            code_txt += NewLine + "        if (p == null)";
            code_txt += NewLine + "        {";
            code_txt += NewLine + "            // No data is available - abort";
            code_txt += NewLine + "            throw new NullReferenceException(\"Failed to process message \" + p.GetType() + \". Message structure is empty (null).\");";
            code_txt += NewLine + "        }";

            code_txt += NewLine;

            code_txt += NewLine + "        fStmt.Transaction = trans.Transaction;";
            code_txt += NewLine;


            // if no queue table no inserts can be done
            if (st.queueTable != "")
            {
                // if no procedure exists the assume it is a holder-structure instead of a doer
                if ((st.insertSP == "") && (st.updateSP == "") && (st.deleteSP == ""))
                {
                    // let any value pass as valid for opcode, it will be checked in the children
                }
                else
                {
                    code_txt += NewLine + "        if (p.OPCODE == null)";
                    code_txt += NewLine + "        {";
                    code_txt += NewLine + "            error.AppendLine(\"Invalid Opcode (null) in \" + p.GetType());";
                    code_txt += NewLine + "        }";
                    code_txt += NewLine + "        else";
                    code_txt += NewLine + "        {";
                    code_txt += NewLine + "            bool OpcodeValid = false;";
                    code_txt += NewLine + "            OpcodeValid |= (p.OPCODE == \"0\");";
                    if (st.insertSP != "")
                        code_txt += NewLine + "            OpcodeValid |= (p.OPCODE == \"1\");";
                    if (st.updateSP != "")
                        code_txt += NewLine + "            OpcodeValid |= (p.OPCODE == \"2\");";
                    if (st.deleteSP != "")
                        code_txt += NewLine + "            OpcodeValid |= (p.OPCODE == \"3\");";
                    code_txt += NewLine;
                    code_txt += NewLine + "            if (!OpcodeValid)";
                    code_txt += NewLine + "            {";
                    code_txt += NewLine + "                error.AppendLine(\"Opcode not supported/invalid (\" + p.OPCODE + \") in \" + p.GetType());";
                    code_txt += NewLine + "            }";
                    code_txt += NewLine + "        }";
                    code_txt += NewLine;
                }


                int MaxLenName = 0;
                int MaxLenExternalName = 0;
                foreach (ParameterType p in ht)
                {
                    if (IsInsertField(p))
                    {
                        MaxLenName = Math.Max(MaxLenName, p.name.Length);
                        MaxLenExternalName = Math.Max(MaxLenExternalName, GetExternalName(p, st).Length);
                    }
                }

                string s;
                foreach (ParameterType p in ht)
                {
                    if (IsInsertField(p))
                    {
                        s = "\"" + p.name + "\"";
                        string Assignment = "(fStmt.Parameters[" + s + "]";
                        // handle special fields
                        if (p.name.ToUpper() == "HAPIRCV_ID")
                        {
                            code_txt += NewLine + "        " + Assignment + " as IDbDataParameter).Value = ";
                            code_txt += Converter(p);
                            code_txt += "(trans.HapiTransId);";
                        }
                        else if (p.name.ToUpper() == "SEQNUM")
                        {
                            code_txt += NewLine + "        " + Assignment + " as IDbDataParameter).Value = ";
                            code_txt += Converter(p);

                            switch (code_target)
                            {
                                case code_target.hapi:
                                    code_txt += "(trans.HapiTransSeq);";
                                    break;
                                case code_target.msg:
                                    code_txt += "(trans.TransSeq);";
                                    break;
                                case code_target.mapi:
                                    code_txt += "(trans.TransSeq);";
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if ((p.name.ToUpper() == "CLIENTIDENTITY") && (code_target == code_target.hapi))
                        {
                            code_txt += NewLine + "        " + Assignment + " as IDbDataParameter).Value = ";
                            code_txt += Converter(p);
                            code_txt += "(trans.CompanyId);";
                        }
                        else if (p.name.ToUpper() == "COMMPARTNERIDENTITY")
                        {
                            code_txt += NewLine + "        " + Assignment + " as IDbDataParameter).Value = ";
                            code_txt += Converter(p);
                            code_txt += "(trans.CommPartnerId);";
                        }
                        else if (p.name.ToUpper() == "MSG_IN_ID")
                        {
                            code_txt += NewLine + "        " + Assignment + " as IDbDataParameter).Value = ";
                            code_txt += Converter(p);
                            code_txt += "(trans.MsgInId);";
                        }
                        else if ((p.name.ToUpper() == "MAPI_IN_ID") || (p.name.ToUpper() == "MAPI_OUT_ID"))
                        {
                            code_txt += NewLine + "        " + Assignment + " as IDbDataParameter).Value = ";
                            code_txt += Converter(p);
                            code_txt += "(trans.MapiInId);";
                        }
                        else
                        {
                            code_txt += NewLine + "        if (p." + GetExternalName(p, st) + " != null)";
                            code_txt += NewLine + "        {";

                            if (GetDataType(p).dataType == DataType.StringType)
                            {
                                code_txt += NewLine + "            if (p." + GetExternalName(p, st) + ".Length > " + Convert.ToString(GetDataType(p).length) + ")";
                                code_txt += NewLine + "                error.AppendLine(\"Value for " + GetExternalTypeName(st.name);
                                code_txt += "." + GetExternalName(p, st) + " too long, max ";
                                code_txt += Convert.ToString(GetDataType(p).length) + " chars\");";
                                code_txt += NewLine;

                                if (GetMandatory(p))
                                {
                                    code_txt += NewLine + "            if (p." + GetExternalName(p, st) + ".Length == 0)";
                                    code_txt += NewLine + "                error.AppendLine(\"Zero length for mandatory parameter " + GetExternalTypeName(st.name);
                                    code_txt += "." + GetExternalName(p, st) + " not allowed\");";
                                    code_txt += NewLine;
                                }
                            }

                            code_txt += NewLine + "            " + Assignment + " as IDbDataParameter).Value = ";
                            code_txt += "p." + GetExternalName(p, st);

                            code_txt += ";";
                            code_txt += NewLine + "        }";
                            code_txt += NewLine + "        else";
                            if (!GetMandatory(p))
                            {
                                code_txt += NewLine + "            " + Assignment + " as IDbDataParameter).Value = DBNull.Value;";
                            }
                            else
                            {
                                code_txt += NewLine + "            error.AppendLine(\"Null value for mandatory parameter " + GetExternalTypeName(st.name);
                                code_txt += "." + GetExternalName(p, st) + " not allowed\");";
                                code_txt += NewLine;
                            }

                        }
                        code_txt += NewLine;
                    }
                }

                code_txt += NewLine + "        if (error.Length > 0)";
                code_txt += NewLine + "        {";
                code_txt += NewLine + "            throw (new Exception(error.ToString()));";
                code_txt += NewLine + "        }";
                code_txt += NewLine;

                switch (code_target)
                {
                    case code_target.hapi:
                        code_txt += NewLine + "        trans.HapiTransSeq++;";
                        break;
                    case code_target.msg:
                        code_txt += NewLine + "        trans.TransSeq++;";
                        break;
                    case code_target.mapi:
                        code_txt += NewLine + "        trans.TransSeq++;";
                        break;
                    default:
                        break;
                }

                code_txt += NewLine;
                code_txt += NewLine + "        fStmt.ExecuteNonQuery();";
                code_txt += NewLine;
            }

            // call the child insert objects
            if (st.children != null)
            {
                char child = 'c';
                foreach (ChildType ct in st.children)
                {
                    if (ct.maxOccurrs == "1")
                    {
                        code_txt += NewLine + "        if (p.a" + GetExternalTypeName(ct.name) + " != null)";
                        code_txt += NewLine + "            a" + ct.name + "Insert.Process(ref trans, this, p.a" + GetExternalTypeName(ct.name) + ");";
                    }
                    else
                    {
                        code_txt += NewLine + "        if (p.a" + GetExternalTypeName(ct.name) + "s != null)";
                        code_txt += NewLine + "            foreach (" + GetExternalTypeName(ct.name) + " " + child + " in p.a" + GetExternalTypeName(ct.name) + "s)";
                        code_txt += NewLine + "                a" + ct.name + "Insert.Process(ref trans, this, " + child + ");";
                        child++;
                    }
                }
            }

            code_txt += NewLine + "      }";

            return code_txt;
        }

        private string MakeInsertClass(string structure)
        {
            string code_txt = "";

            foreach (StructureType st in messageDef.structures)
            {
                if (st.name == structure)
                {
                    code_txt = "  public class " + structure + "Insert : SegmentImpl";
                    code_txt += NewLine + "  {";

                    // declare the child insert objects
                    if (st.children != null)
                        foreach (ChildType ct in st.children)
                            code_txt += NewLine + "    private " + ct.name + "Insert a" + ct.name + "Insert;";

                    code_txt += NewLine;
                    code_txt += MakeInsertConstructor(st);
                    code_txt += NewLine;
                    code_txt += MakeInsertProcess(st);
                    code_txt += NewLine + "  }";
                }
            }
            return code_txt;
        }

        private string MakeInsertClasses()
        {
            string code_txt = "";

            foreach (StructureType st in messageDef.structures)
            {
                code_txt += NewLine + MakeInsertClass(st.name);
                code_txt += NewLine;
            }

            return code_txt;
        }

        private string MakeInsertFile()
        {
            string code_txt = "";

            code_txt += "/*";
            code_txt += NewLine + "  File           : ";
            code_txt += NewLine + "";
            code_txt += NewLine + "  Description    : Internal classes for inserting inbound data into queue tables.";
            code_txt += NewLine + "                   This code was generated, do not edit.";
            code_txt += NewLine + "";
            code_txt += NewLine + "*/";
            code_txt += NewLine + "using System;";
            code_txt += NewLine + "using System.Text;";
            code_txt += NewLine + "using System.Data;";
            code_txt += NewLine + "using System.Data.Common;";
            code_txt += NewLine + "";
            code_txt += NewLine + "namespace " + nameSpace;
            code_txt += NewLine + "{";
            code_txt += NewLine + "  public class InsertHandler";
            code_txt += NewLine + "  {";
            code_txt += NewLine + "      public string _Debug()";
            code_txt += NewLine + "      {";
            code_txt += NewLine + "          return \"Generated on   : " + DateTime.Now.ToShortDateString().Replace("\\", "\\\\") + " " + DateTime.Now.ToLongTimeString().Replace("\\", "\\\\") + "\\r\\n\" +";
            code_txt += NewLine + "                 \"Generated by   : " + WindowsIdentity.GetCurrent().Name.Replace("\\", "\\\\") + "@" + SystemInformation.ComputerName + "\\r\\n\" +";
            code_txt += NewLine + "                 \"Generated in   : " + Environment.CurrentDirectory.Replace("\\", "\\\\") + "\\r\\n\";";
            code_txt += NewLine + "      }";
            code_txt += NewLine + "  }";
            code_txt += NewLine;
            code_txt += MakeInsertClasses();
            code_txt += NewLine + "}";
            return code_txt;
        }

        public override void GetOutPut(TextWriter tw)
        {
            tw.WriteLine(MakeInsertFile());
        }
    }
}
