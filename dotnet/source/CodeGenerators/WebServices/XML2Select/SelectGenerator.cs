using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Security.Principal;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2Select
{
    public class SelectGenerator : GenericGenerator
    {
        private const string NewLine = "\r\n";
        private code_target code_target;
        private string nameSpace;

        public SelectGenerator(MessageDefinition messageDef, Package packageDef, code_target ct, String ns)
            : base(messageDef, packageDef)
        {
            code_target = ct;
            nameSpace = ns;
        }

        private bool IsSelectFromDBField(ParameterType p)
        {
            return ((p.fieldType == ParameterTypeFieldType.Normal) && IsExternalField(p));
        }

        private bool IsInsertField(ParameterType p)
        {
            return ((p.fieldType == ParameterTypeFieldType.SystemId) ||
              (p.fieldType == ParameterTypeFieldType.OpCode) ||
              (p.fieldType == ParameterTypeFieldType.Normal));
        }

        private string MakeSelectConstructor(StructureType st)
        {
            string code_txt = "";
            ParameterType[] ht = GetUniqueParameterList(st);

            code_txt += NewLine + "    public " + st.name + "Select(Database db)";
            code_txt += NewLine + "    {";
            code_txt += NewLine + "      this.db = db;";
            code_txt += NewLine;

            // create the child insert objects
            if (st.children != null)
                foreach (ChildType ct in st.children)
                    code_txt += NewLine + "      a" + ct.name + "Select = new " + ct.name + "Select(this.db);";

            code_txt += NewLine + "    }";
            return code_txt;
        }

        private string MakeSelectProcess(StructureType st)
        {
            string code_txt = "";

            switch (code_target)
            {
                case code_target.hapi:
                    code_txt += NewLine + "    public " + GetExternalTypeName(st.name) + " Process( System.String HapiTransId )";
                    code_txt += NewLine + "    {";
                    code_txt += NewLine + "      ArrayList l = InternalProcess( HapiTransId );";
                    code_txt += NewLine;
                    code_txt += NewLine + "      if ( l.Count == 1 )";
                    code_txt += NewLine + "        return (" + GetExternalTypeName(st.name) + ")l[0];";
                    code_txt += NewLine + "      else";
                    code_txt += NewLine + "        return null;";
                    code_txt += NewLine + "    }";
                    code_txt += NewLine;
                    code_txt += NewLine + "    public ArrayList InternalProcess( System.String HapiTransId )";
                    code_txt += NewLine + "    {";
                    code_txt += NewLine + "      ArrayList a" + GetExternalTypeName(st.name) + "List = new ArrayList();";
                    code_txt += NewLine;
                    code_txt += NewLine + "      IDataReader r = db.ExecuteReader( ";
                    code_txt += NewLine + "        \"select\" +";
                    break;
                case code_target.msg:
                    code_txt += NewLine + "    public " + GetExternalTypeName(st.name) + " Process( System.String MsgOutId )";
                    code_txt += NewLine + "    {";
                    code_txt += NewLine + "      ArrayList l = InternalProcess( MsgOutId );";
                    code_txt += NewLine;
                    code_txt += NewLine + "      if ( l.Count == 1 )";
                    code_txt += NewLine + "        return (" + GetExternalTypeName(st.name) + ")l[0];";
                    code_txt += NewLine + "      else";
                    code_txt += NewLine + "        return null;";
                    code_txt += NewLine + "    }";
                    code_txt += NewLine;
                    code_txt += NewLine + "    public ArrayList InternalProcess( System.String MsgOutId )";
                    code_txt += NewLine + "    {";
                    code_txt += NewLine + "      ArrayList a" + GetExternalTypeName(st.name) + "List = new ArrayList();";
                    code_txt += NewLine;
                    code_txt += NewLine + "      IDataReader r = db.ExecuteReader( ";
                    code_txt += NewLine + "        \"select\" +";
                    break;
                case code_target.mapi:
                    code_txt += NewLine + "    public " + GetExternalTypeName(st.name) + " Process( System.String MapiOutId )";
                    code_txt += NewLine + "    {";
                    code_txt += NewLine + "      ArrayList l = InternalProcess( MapiOutId );";
                    code_txt += NewLine;
                    code_txt += NewLine + "      if ( l.Count == 1 )";
                    code_txt += NewLine + "        return (" + GetExternalTypeName(st.name) + ")l[0];";
                    code_txt += NewLine + "      else";
                    code_txt += NewLine + "        return null;";
                    code_txt += NewLine + "    }";
                    code_txt += NewLine;
                    code_txt += NewLine + "    public ArrayList InternalProcess( System.String MapiOutId )";
                    code_txt += NewLine + "    {";
                    code_txt += NewLine + "      ArrayList a" + GetExternalTypeName(st.name) + "List = new ArrayList();";
                    code_txt += NewLine;
                    code_txt += NewLine + "      IDataReader r = db.ExecuteReader( ";
                    code_txt += NewLine + "        \"select\" +";
                    break;
                default:
                    break;
            }

            ParameterType[] pt = GetUniqueParameterList(st);

            string First = " ";
            foreach (ParameterType p in pt)
            {
                if (IsSelectFromDBField(p))
                {
                    code_txt += NewLine + "               \"" + First + p.originTable + "." + p.originColumn + "\" +";
                    First = ",";
                }
            }

            switch (code_target)
            {
                case code_target.hapi:
                    code_txt += NewLine + "        \" from   " + st.baseTable + "\" +";
                    code_txt += NewLine + "        \" where  " + st.baseTable + ".HAPITRANS_ID = '\" + HapiTransId + \"'\" );";
                    code_txt += NewLine + "";
                    code_txt += NewLine + "      while ( r.Read() )";
                    code_txt += NewLine + "      {";
                    code_txt += NewLine + "        " + GetExternalTypeName(st.name) + " a" + GetExternalTypeName(st.name) + " = new " + GetExternalTypeName(st.name) + "();";
                    code_txt += NewLine;
                    break;
                case code_target.msg:
                    code_txt += NewLine + "        \" from   " + st.baseTable + "\" +";
                    code_txt += NewLine + "        \" where  " + st.baseTable + ".MSG_OUT_ID = '\" + MsgOutId + \"'\" );";
                    code_txt += NewLine + "";
                    code_txt += NewLine + "      while ( r.Read() )";
                    code_txt += NewLine + "      {";
                    code_txt += NewLine + "        " + GetExternalTypeName(st.name) + " a" + GetExternalTypeName(st.name) + " = new " + GetExternalTypeName(st.name) + "();";
                    code_txt += NewLine;
                    break;
                case code_target.mapi:
                    code_txt += NewLine + "        \" from   " + st.baseTable + "\" +";
                    code_txt += NewLine + "        \" where  " + st.baseTable + ".MAPI_OUT_ID = '\" + MapiOutId + \"'\" );";
                    code_txt += NewLine + "";
                    code_txt += NewLine + "      while ( r.Read() )";
                    code_txt += NewLine + "      {";
                    code_txt += NewLine + "        " + GetExternalTypeName(st.name) + " a" + GetExternalTypeName(st.name) + " = new " + GetExternalTypeName(st.name) + "();";
                    code_txt += NewLine;
                    break;
                default:
                    break;
            }

            int MaxLenName = 0;
            int MaxLenColumn = 0;
            foreach (ParameterType p in pt)
            {
                if (IsExternalField(p))
                {
                    MaxLenName = Math.Max(MaxLenName, GetExternalName(p, st).Length);
                    MaxLenColumn = Math.Max(MaxLenColumn, (p.originColumn).Length);
                }
            }

            int ix = 0;
            foreach (ParameterType p in pt)
            {
                if (IsExternalField(p))
                {
                    if (IsSelectFromDBField(p))
                    {
                        code_txt += NewLine;
                        code_txt += NewLine + "        if ( r.IsDBNull( " + Convert.ToString(ix) + " ) )";
                        code_txt += NewLine + "          a" + GetExternalTypeName(st.name) + "." + GetExternalName(p, st) + " = null;";
                        code_txt += NewLine + "        else";
                        code_txt += NewLine + "        {";
                        switch (GetDataType(p).dataType)
                        {
                            case DataType.StringType:
                                code_txt += NewLine + "          a" + GetExternalTypeName(st.name) + "." + GetExternalName(p, st) + " = r.";
                                code_txt += "GetString";
                                code_txt += "( " + Convert.ToString(ix) + " );";
                                break;
                            case DataType.IntegerType:
                                code_txt += NewLine + "          a" + GetExternalTypeName(st.name) + "." + GetExternalName(p, st) + " = r.";
                                code_txt += "GetInt32";
                                code_txt += "( " + Convert.ToString(ix) + " );";
                                break;
                            case DataType.DoubleType:
                                code_txt += NewLine + "          a" + GetExternalTypeName(st.name) + "." + GetExternalName(p, st) + " = Convert.ToDouble( r.";
                                code_txt += "GetDecimal";
                                code_txt += "( " + Convert.ToString(ix) + " ) );";
                                break;
                            case DataType.DateTimeType:
                                code_txt += NewLine + "          a" + GetExternalTypeName(st.name) + "." + GetExternalName(p, st) + " = DateTime.SpecifyKind(r.";
                                code_txt += "GetDateTime";
                                code_txt += "( " + Convert.ToString(ix) + " ), DateTimeKind.Local);";
                                break;
                        }
                        code_txt += NewLine + "        }";
                        ix++;
                    }
                    else
                    {
                        if (p.fieldType == ParameterTypeFieldType.OpCode)
                        {
                            code_txt += NewLine + "        a" + GetExternalTypeName(st.name) + "." + GetExternalName(p, st) + " = \"1\";"; // opcode == new
                        }
                    }
                }
            }
            code_txt += NewLine;
            code_txt += NewLine + "        a" + GetExternalTypeName(st.name) + "List.Add( a" + GetExternalTypeName(st.name) + " );";
            code_txt += NewLine + "      }";
            code_txt += NewLine;
            code_txt += NewLine + "      r.Close();";
            code_txt += NewLine;
            // call the child insert objects
            if (st.children != null)
            {
                foreach (ChildType ct in st.children)
                {
                    if (ct.maxOccurrs == "1")
                        code_txt += NewLine + "      maxOccurrs == 1 - Internal error not supported";
                    else
                    {
                        code_txt += NewLine + "      ArrayList a" + GetExternalTypeName(ct.name) + "List = ";

                        switch (code_target)
                        {
                            case code_target.hapi:
                                code_txt += "a" + ct.name + "Select.InternalProcess( HapiTransId );";
                                break;
                            case code_target.msg:
                                code_txt += "a" + ct.name + "Select.InternalProcess( MsgOutId );";
                                break;
                            case code_target.mapi:
                                code_txt += "a" + ct.name + "Select.InternalProcess( MapiOutId );";
                                break;
                            default:
                                break;
                        }
                    }
                }

                code_txt += NewLine;
                code_txt += NewLine + "      foreach ( " + GetExternalTypeName(st.name) + " a" + GetExternalTypeName(st.name) + " in a" + GetExternalTypeName(st.name) + "List )";
                code_txt += NewLine + "      {";
                code_txt += NewLine + "        ArrayList ConnectedChildren = null;";
                foreach (ChildType ct in st.children)
                {
                    if (ct.maxOccurrs == "1")
                        code_txt += NewLine + "      maxOccurrs == 1 - Internal error not supported";
                    else
                    {
                        code_txt += NewLine + "        ConnectedChildren = new ArrayList();";
                        code_txt += NewLine;
                        code_txt += NewLine + "        foreach ( " + GetExternalTypeName(ct.name) + " a" + GetExternalTypeName(ct.name) + " in a" + GetExternalTypeName(ct.name) + "List )";
                        code_txt += NewLine + "        {";
                        code_txt += NewLine + "          if ( GeneratedComparer.Equal" + GetExternalTypeName(st.name) + GetExternalTypeName(ct.name) + "(";
                        code_txt += " a" + GetExternalTypeName(st.name) + ", a" + GetExternalTypeName(ct.name) + " ) )";
                        code_txt += NewLine + "            ConnectedChildren.Add( a" + GetExternalTypeName(ct.name) + " );";
                        code_txt += NewLine + "        }";
                        code_txt += NewLine + "        a" + GetExternalTypeName(st.name) + ".a" + GetExternalTypeName(ct.name) + "s = ConnectedChildren.ToArray(typeof(" + GetExternalTypeName(ct.name) + ")) as " + GetExternalTypeName(ct.name) + "[];";
                    }
                }
                code_txt += NewLine + "      }";
            }

            code_txt += NewLine;
            code_txt += NewLine + "      return a" + GetExternalTypeName(st.name) + "List;";
            code_txt += NewLine + "    }";
            return code_txt;
        }

        private string MakeSelectFindChildren(StructureType st)
        {
            string code_txt = "";
            code_txt += NewLine + "    public " + GetExternalTypeName(st.name) + "[] FindChildren( " + GetExternalTypeName(st.name) + " parent, ArrayList AllChildren )";
            code_txt += NewLine + "    {";
            code_txt += NewLine + "      /* parent is only carrier of primary keys! */";
            code_txt += NewLine + "      ArrayList ConnectedChildren = new ArrayList();";
            code_txt += NewLine;
            code_txt += NewLine + "      foreach ( " + GetExternalTypeName(st.name) + " child in AllChildren )";
            code_txt += NewLine + "      {";
            ParameterType[] pl = GetUniqueParameterList(st);
            String First = "if ( ";
            foreach (ParameterType p in pl)
            {
                if (IsExternalField(p))
                {
                    if (p.column != null)
                    {
                        if (p.column.primaryKey)
                        {
                            code_txt += NewLine + "        " + First + " ( child." + GetExternalName(p, st) + " == parent." + GetExternalName(p, st) + " )";
                            First = "  &&";
                        }
                    }
                }
            }
            if (First != "if ( ")
            {
                code_txt += " )";
                code_txt += NewLine + "        {";
                code_txt += NewLine + "          ConnectedChildren.Add( child );";
                code_txt += NewLine + "        }";
            }
            code_txt += NewLine + "      }";
            code_txt += NewLine;
            code_txt += NewLine + "      return ConnectedChildren.ToArray(typeof(" + GetExternalTypeName(st.name) + ")) as " + GetExternalTypeName(st.name) + "[];";
            code_txt += NewLine + "    }";
            return code_txt;
        }

        private string MakeSelectClass(string structure)
        {
            string code_txt = "";

            foreach (StructureType st in messageDef.structures)
            {
                if (st.name == structure)
                {
                    code_txt = "  public class " + structure + "Select";
                    code_txt += NewLine + "  {";
                    code_txt += NewLine + "    private Database db;";
                    // declare the child insert objects
                    if (st.children != null)
                        foreach (ChildType ct in st.children)
                            code_txt += NewLine + "    private " + ct.name + "Select a" + ct.name + "Select;";

                    code_txt += NewLine;
                    code_txt += MakeSelectConstructor(st);
                    code_txt += NewLine;
                    code_txt += MakeSelectProcess(st);
                    //          code_txt += NewLine;
                    //          code_txt += MakeSelectFindChildren( st );
                    code_txt += NewLine + "  }";
                }
            }
            return code_txt;
        }

        private string MakeSelectClasses()
        {
            string code_txt = "";

            foreach (StructureType st in messageDef.structures)
            {
                code_txt += NewLine + MakeSelectClass(st.name);
                code_txt += NewLine;
            }

            return code_txt;
        }

        private string MakeCompararer(StructureType st1, StructureType st2)
        {
            string code_txt = "";
            code_txt += NewLine + "    static public bool Equal" + GetExternalTypeName(st1.name) + GetExternalTypeName(st2.name) + "( " + GetExternalTypeName(st1.name) + " a" + GetExternalTypeName(st1.name) + ", " + GetExternalTypeName(st2.name) + " a" + GetExternalTypeName(st2.name) + " )";
            code_txt += NewLine + "    {";
            ParameterType[] pl1 = GetUniqueParameterList(st1);
            ParameterType[] pl2 = GetUniqueParameterList(st2);
            String First = "return ( ";
            bool Found = false;
            foreach (ParameterType p1 in pl1)
            {
                foreach (ParameterType p2 in pl2)
                {
                    if (IsExternalField(p1) && IsExternalField(p2))
                    {
                        if ((p1.column != null) && (p2.column != null))
                        {
                            if ((p1.column.primaryKey) && (p2.column.primaryKey))
                            {
                                if (GetExternalName(p1, st1).ToUpper() == GetExternalName(p2, st2).ToUpper())
                                {
                                    switch (GetDataType(p1).dataType) // assume both parameters are of the same datatype, otherwise an error will occur
                                    {
                                        case DataType.StringType:
                                            code_txt += NewLine + "      " + First + " ( Convert.ToString( a" + GetExternalTypeName(st1.name) + "." + GetExternalName(p1, st1) + " ) == Convert.ToString( a" + GetExternalTypeName(st2.name) + "." + GetExternalName(p2, st2) + " ) )";
                                            break;
                                        case DataType.IntegerType:
                                            code_txt += NewLine + "      " + First + " ( a" + GetExternalTypeName(st1.name) + "." + GetExternalName(p1, st1) + " == a" + GetExternalTypeName(st2.name) + "." + GetExternalName(p2, st2) + " )";
                                            break;
                                        case DataType.DoubleType:
                                            code_txt += NewLine + "      " + First + " ( a" + GetExternalTypeName(st1.name) + "." + GetExternalName(p1, st1) + " == a" + GetExternalTypeName(st2.name) + "." + GetExternalName(p2, st2) + " )";
                                            break;
                                        case DataType.DateTimeType:
                                            code_txt += NewLine + "      " + First + " ( a" + GetExternalTypeName(st1.name) + "." + GetExternalName(p1, st1) + " == a" + GetExternalTypeName(st2.name) + "." + GetExternalName(p2, st2) + " )";
                                            break;
                                    }

                                    First = "        &&";
                                    Found = true;
                                }
                            }
                        }
                    }
                }
            }
            if (Found)
            {
                code_txt += " );";
            }
            else
            {
                code_txt += NewLine + "      return false;";
            }
            code_txt += NewLine + "    }";
            return code_txt;
        }

        private string MakeCompareClass()
        {
            string code_txt = "";

            code_txt = "  public class GeneratedComparer";
            code_txt += NewLine + "  {";

            foreach (StructureType st1 in messageDef.structures)
            {
                // declare the child insert objects
                if (st1.children != null)
                    foreach (ChildType ct in st1.children)
                    {
                        StructureType st2 = GetStructureFromName(ct.name);
                        code_txt += MakeCompararer(st1, st2);
                        code_txt += NewLine;
                    }
            }
            code_txt += NewLine + "  }";
            return code_txt;
        }

        private string MakeSelectFile()
        {
            string code_txt = "";

            code_txt += "/*";
            code_txt += NewLine + "  File           : ";
            code_txt += NewLine + "";
            code_txt += NewLine + "  Description    : Internal classes for getting outbound data from queue tables.";
            code_txt += NewLine + "                   This code was generated, do not edit.";
            code_txt += NewLine + "";
            code_txt += NewLine + "*/";
            code_txt += NewLine + "using System;";
            code_txt += NewLine + "using System.Text;";
            code_txt += NewLine + "using System.Data;";
            code_txt += NewLine + "using System.Collections;";
            code_txt += NewLine + "using Imi.Framework.Job.Data;";
            code_txt += NewLine + "";
            code_txt += NewLine + "namespace " + nameSpace;
            code_txt += NewLine + "{";
            code_txt += NewLine + "  public class SelectHandler";
            code_txt += NewLine + "  {";
            code_txt += NewLine + "    public string _Debug()";
            code_txt += NewLine + "    {";
            code_txt += NewLine + "      return \"Generated on   : " + DateTime.Now.ToShortDateString().Replace("\\", "\\\\") + " " + DateTime.Now.ToLongTimeString().Replace("\\", "\\\\") + "\\r\\n\" +";
            code_txt += NewLine + "             \"Generated by   : " + WindowsIdentity.GetCurrent().Name.Replace("\\", "\\\\") + "@" + SystemInformation.ComputerName + "\\r\\n\" +";
            code_txt += NewLine + "             \"Generated in   : " + Environment.CurrentDirectory.Replace("\\", "\\\\") + "\\r\\n\";";
            code_txt += NewLine + "    }";
            code_txt += NewLine + "  }";
            code_txt += NewLine;
            code_txt += MakeSelectClasses();
            code_txt += NewLine;
            code_txt += MakeCompareClass();
            code_txt += NewLine + "}";
            return code_txt;
        }

        public override void GetOutPut(TextWriter tw)
        {
            tw.WriteLine(MakeSelectFile());
        }

        /*public DeliveryReceiptLineDoc[] FindChildren( DeliveryReceiptHeadDoc parent, ArrayList AllChildren )
        {
          ArrayList ConnectedChildren = new ArrayList();

          foreach ( DeliveryReceiptLineDoc child in AllChildren )
          {
            if ( ( child.pk1 == parent.pk1 )
              && ( child.pk2 == parent.pk2 ) )
            {
              ConnectedChildren.Add( l );
            }

          }

          return ConnectedChildren.ToArray(typeof(DeliveryReceiptLineDoc)) as DeliveryReceiptLineDoc[];
        }*/
    }
}
