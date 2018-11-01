using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Security.Principal;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2IntoProc
{
    public class PLSQLGenerator : GenericGenerator
    {
        private const string NewLine = "\r\n";

        private code_target code_target;
        private string tableName;
        private string packageName;

        public PLSQLGenerator(MessageDefinition messageDef, Package packageDef, code_target ct, string mapi_number)
            : base(messageDef, packageDef)
        {
            code_target = ct;

            switch (code_target)
            {
                case code_target.hapi:
                    {
                        tableName = "HAPIRCV";
                        packageName = "HAPI_Rcv_Object";
                        break;
                    }
                case code_target.msg:
                    {
                        tableName = "MSG_IN";
                        packageName = "MessageRcv_Object";
                        break;
                    }
                case code_target.mapi:
                    {
                        tableName = "MAPI_IN";
                        packageName = "MAPIIn_" + mapi_number + "Object";
                        break;
                    }
                default:
                    break;
            }
        }

        private string GetRec(StructureType st)
        {
            return GetTableName(st.queueTable).Substring(5) + "_Rec_I";
        }

        private string GetProc(StructureType st)
        {
            return (st.name);
        }

        private string MakeHapiCall(StructureType st, bool bodyfile)
        {
            string code_txt = "";
            string First;
            ParameterType[] pl;

            if (bodyfile)
            {
                code_txt += NewLine + "/*page";
                code_txt += NewLine + "********************************************************************************";
                code_txt += NewLine + "**";
                code_txt += NewLine + "** Procedure     : " + GetProc(st) + ".";
                code_txt += NewLine + "**";
                code_txt += NewLine + "** Description   : This code is generated. Do not edit.";
                code_txt += NewLine + "**";
                code_txt += NewLine + "** Input         : ";
                code_txt += NewLine + "**";
                code_txt += NewLine + "** Output        : ALMID        Alarm id  ";
                code_txt += NewLine + "**";
                code_txt += NewLine + "** Global output : Global.ALMID when error. ";
                code_txt += NewLine + "**";
                code_txt += NewLine + "********************************************************************************";
                code_txt += NewLine + "*/";
            }
            string s = "procedure " + GetProc(st) + "(";
            code_txt += NewLine + s + " " + GetRec(st) + "     " + GetTableName(st.queueTable) + "%rowtype";
            code_txt += NewLine + new String(' ', s.Length) + "," + "ALMID_O".PadRight(GetRec(st).Length) + " out ALM.ALMID%type )";
            if (!bodyfile)
            {
                code_txt += ";" + NewLine;
                return code_txt;
            }
            code_txt += NewLine + "is";
            code_txt += NewLine + "begin";
            code_txt += NewLine + "  Global.Stack_Name('" + packageName + "." + GetProc(st) + "');";
            code_txt += NewLine + "  logg_output.put_line('>>> " + packageName + "." + GetProc(st) + "');";
            code_txt += NewLine;
            code_txt += NewLine + "  Global.ALMID := null;";
            code_txt += NewLine;
            code_txt += NewLine + "    if " + GetRec(st) + ".OPCODE = Def.OPCODE_New then";
            if (st.insertSP != "")
            {
                code_txt += NewLine + "      " + st.insertSP + "(";

                First = " ";
                pl = GetParameters(st.insertSP);
                foreach (ParameterType p in pl)
                {
                    if (!IsExcluded(p.name, st))
                        code_txt += NewLine + "         " + First + GetRec(st) + "." + p.name;
                    else
                        code_txt += NewLine + "         " + First + "null /* " + p.name + " */";

                    First = ",";
                }
                code_txt += " );";
            }
            else
            {
                code_txt += NewLine;
                code_txt += NewLine + "         null; /* feature not supported */";
            }
            code_txt += NewLine;
            code_txt += NewLine + "    elsif " + GetRec(st) + ".OPCODE = Def.OPCODE_Change then";
            if (st.updateSP != "")
            {
                code_txt += NewLine + "      " + st.updateSP + "(";

                First = " ";
                pl = GetParameters(st.updateSP);
                foreach (ParameterType p in pl)
                {
                    if (!IsExcluded(p.name, st))
                        code_txt += NewLine + "         " + First + GetRec(st) + "." + p.name;
                    else
                        code_txt += NewLine + "         " + First + "null /* " + p.name + " */";

                    First = ",";
                }

                code_txt += " );";
            }
            else
            {
                code_txt += NewLine;
                code_txt += NewLine + "         null; /* feature not supported */";
            }
            code_txt += NewLine;
            code_txt += NewLine + "    elsif " + GetRec(st) + ".OPCODE = Def.OPCODE_Remove then";
            if (st.deleteSP != "")
            {
                code_txt += NewLine + "      " + st.deleteSP + "(";

                First = " ";
                pl = GetParameters(st.deleteSP);
                foreach (ParameterType p in pl)
                {
                    if (!IsExcluded(p.name, st))
                        code_txt += NewLine + "         " + First + GetRec(st) + "." + p.name;
                    else
                        code_txt += NewLine + "         " + First + "null /* " + p.name + " */";

                    First = ",";
                }

                code_txt += " );";
            }
            else
            {
                code_txt += NewLine;
                code_txt += NewLine + "         null; /* feature not supported */";
            }
            code_txt += NewLine + "    end if;";
            code_txt += NewLine;
            code_txt += NewLine + "  ALMID_O := Global.ALMID;";
            code_txt += NewLine;
            code_txt += NewLine + "  logg_output.put_line('<<< " + packageName + "." + GetProc(st) + "  ALMID:' || nvl( Global.ALMID, '<null>' ));";
            code_txt += NewLine + "  Global.Unstack_Name;";
            code_txt += NewLine;
            code_txt += NewLine + "end " + GetProc(st) + ";";

            return code_txt;
        }

        private string MakeHapiCalls(bool bodyfile)
        {
            string code_txt = "";

            foreach (StructureType st in messageDef.structures)
            {
                if (st.queueTable != "")
                {
                    code_txt += NewLine + MakeHapiCall(st, bodyfile);
                    code_txt += NewLine;
                }
            }

            return code_txt;
        }

        private string GetInterface(InterfaceType i)
        {
            return (i.name);
        }

        private string MakeInterface(InterfaceType i, bool bodyfile)
        {
            string code_txt = "";

            StructureType[] sl = GetUniqueStructureList(i);

            int NumStructures = 0;

            foreach (StructureType st in sl)
            {
                if (st.queueTable != "")
                    NumStructures++;
            }

            if (NumStructures == 0)
                return "";

            if (bodyfile)
            {
                code_txt += NewLine + "/*page";
                code_txt += NewLine + "********************************************************************************";
                code_txt += NewLine + "**";
                code_txt += NewLine + "** Procedure     : " + GetInterface(i) + ".";
                code_txt += NewLine + "**";
                code_txt += NewLine + "** Description   : This code is generated. Do not edit.";
                code_txt += NewLine + "**";

                switch (code_target)
                {
                    case code_target.hapi:
                        {
                            code_txt += NewLine + "** Input         : " + tableName + "_ID   - HAPI receive identity";
                            break;
                        }
                    case code_target.msg:
                        {
                            code_txt += NewLine + "** Input         : " + tableName + "_ID   - Incoming Message Identity";
                            break;
                        }
                    case code_target.mapi:
                        {
                            code_txt += NewLine + "** Input         : " + tableName + "_ID   - Incoming Message Identity";
                            break;
                        }
                    default:
                        break;
                }

                code_txt += NewLine + "**";
                code_txt += NewLine + "** Output        : -";
                code_txt += NewLine + "**";
                code_txt += NewLine + "** Global output : -";
                code_txt += NewLine + "**";
                code_txt += NewLine + "********************************************************************************";
                code_txt += NewLine + "*/";
            }
            code_txt += NewLine + "procedure " + GetInterface(i) + "( " + tableName + "_ID_I      " + tableName + "." + tableName + "_ID%type";
            code_txt += NewLine + "          " + "".PadRight(GetInterface(i).Length) + " ,ALMID_O       out ALM.ALMID%type )";

            if (!bodyfile)
            {
                code_txt += ";" + NewLine;
                return code_txt;
            }
            code_txt += NewLine + "";
            code_txt += NewLine + "";
            code_txt += NewLine + "is";
            foreach (StructureType st in sl)
            {
                if (st.queueTable == "")
                    continue;

                code_txt += NewLine + "  /* ----------------------------------------";
                code_txt += NewLine + "  **  Get " + st.name;
                code_txt += NewLine + "  ** ---------------------------------------- */";
                code_txt += NewLine + "";
                code_txt += NewLine + "  cursor " + GetTableName(st.queueTable).Substring(5) + "_Cur( " + tableName + "_ID_I     " + tableName + "." + tableName + "_ID%type )";
                code_txt += NewLine + "  is";
                code_txt += NewLine + "    select     " + GetTableName(st.queueTable) + ".*";
                code_txt += NewLine + "      from     " + GetTableName(st.queueTable) + "";
                code_txt += NewLine + "      where    " + GetTableName(st.queueTable) + "." + tableName + "_ID = " + tableName + "_ID_I";
                code_txt += NewLine + "      order by " + GetTableName(st.queueTable) + "." + tableName + "_ID";
                code_txt += NewLine + "              ," + GetTableName(st.queueTable) + ".SEQNUM;";
                code_txt += NewLine;
            }
            code_txt += NewLine + "begin";
            code_txt += NewLine + "";
            code_txt += NewLine + "  Global.Stack_Name('" + packageName + "." + GetInterface(i) + "');";
            code_txt += NewLine + "  logg_output.put_line('>>> " + packageName + "." + GetInterface(i) + "');";
            code_txt += NewLine + "";
            code_txt += NewLine + "  ALMID_O := null;";
            code_txt += NewLine + "";
            foreach (StructureType st in sl)
            {
                if (st.queueTable == "")
                    continue;

                code_txt += NewLine + "  /* ----------------------------------------";
                code_txt += NewLine + "  **  Get " + st.name;
                code_txt += NewLine + "  ** ---------------------------------------- */";
                code_txt += NewLine + "  ";
                code_txt += NewLine + "  for " + GetTableName(st.queueTable).Substring(5) + "_C_R in " + GetTableName(st.queueTable).Substring(5) + "_Cur( " + tableName + "_ID_I )";
                code_txt += NewLine + "  loop";
                code_txt += NewLine + "";
                code_txt += NewLine + "    if ALMID_O is null then";
                code_txt += NewLine + "";
                code_txt += NewLine + "      " + GetProc(st) + "( " + GetTableName(st.queueTable).Substring(5) + "_C_R, ALMID_O );";
                code_txt += NewLine + "";
                code_txt += NewLine + "    end if;";
                code_txt += NewLine + "";
                code_txt += NewLine + "  end loop;   /* " + GetTableName(st.queueTable).Substring(5) + "_Cur */";
                code_txt += NewLine;
            }
            code_txt += NewLine;
            code_txt += NewLine + "  logg_output.put_line('<<< " + packageName + "." + GetInterface(i) + "');";
            code_txt += NewLine + "  Global.Unstack_Name;";
            code_txt += NewLine;
            code_txt += NewLine + "end " + GetInterface(i) + ";";

            return code_txt;
        }

        private string MakeInterfaces(bool bodyfile)
        {
            string code_txt = "";

            foreach (InterfaceType i in messageDef.interfaces)
            {
                code_txt += NewLine + MakeInterface(i, bodyfile);
                code_txt += NewLine;
            }

            return code_txt;
        }

        private string MakeInsertFile(bool bodyfile)
        {
            string code_txt = "";

            code_txt += NewLine + "/*";
            code_txt += NewLine + "** File         : " + packageName + ".";
            if (bodyfile)
                code_txt += "body";
            else
                code_txt += "spec";
            code_txt += NewLine + "**";
            code_txt += NewLine + "** Description  : This code is generated. Do not edit.";
            code_txt += NewLine + "**";
            code_txt += NewLine + "** Author       : Original solution: Ulf Andersson, Industri-Matematik AB.";
            code_txt += NewLine + "** Date         : 05-01-13";
            code_txt += NewLine + "** ";
            code_txt += NewLine + "** Rev.      Sign.  Date    Note";
            code_txt += NewLine + "** --------  -----  ------  ----------------------------------------------------";
            code_txt += NewLine + "** 5.0       ULAN			Original version.";
            code_txt += NewLine + "**";
            code_txt += NewLine + "********************************************************************************";
            code_txt += NewLine + "*/";
            code_txt += NewLine + "";
            code_txt += NewLine + "create or replace package ";
            if (bodyfile)
                code_txt += "body ";
            code_txt += "" + packageName + " as ";
            code_txt += NewLine + "";
            code_txt += NewLine + "";
            if (bodyfile)
            {
                code_txt += NewLine + "/* ------------ Local definitions ------------------------------------------- */";
                code_txt += NewLine + "";
                code_txt += NewLine + "/* ------------ Local variables --------------------------------------------- */";
                code_txt += NewLine + "";
                code_txt += NewLine + "/* ------------ Specification of functions/procedures ----------------------- */";
                code_txt += NewLine + "";
                code_txt += NewLine;
            }
            code_txt += MakeHapiCalls(bodyfile);
            code_txt += NewLine;
            code_txt += MakeInterfaces(bodyfile);
            code_txt += NewLine;
            code_txt += NewLine + "end " + packageName + ";";
            code_txt += NewLine + "/";
            return code_txt;
        }

        public override void GetOutPut(TextWriter tw)
        {
            // not used
        }

        public void GetOutPut(TextWriter tw, bool bodyfile)
        {
            tw.WriteLine(MakeInsertFile(bodyfile));
        }
    }
}
