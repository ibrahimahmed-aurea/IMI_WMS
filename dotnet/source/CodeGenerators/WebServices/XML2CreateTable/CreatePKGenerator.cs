using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2CreateTable
{
    /// <summary>
    /// Summary description for CreateTriggerGenerator.
    /// </summary>
    public class CreatePKGenerator : GenericGenerator
    {
        private code_target code_target;

        public CreatePKGenerator(MessageDefinition messageDef, Package packageDef, code_target ct)
            : base(messageDef, packageDef)
        {
            code_target = ct;
        }

        public override void GetOutPut(TextWriter writer)
        {
            StringBuilder s = new StringBuilder();

            int curr = 0;
            int total = messageDef.structures.Length + 1;
            ProgressEventArgs progressArgs;
            progressArgs = new ProgressEventArgs(curr, total);
            OnProgress(progressArgs);

            foreach (StructureType structure in messageDef.structures)
            {
                if (structure.queueTable == "")
                    continue;

                // Table name is the short name from
                // the structure document
                String tableName = GetTableName(structure.queueTable);

                // Bad solution but it works
                String pkName = "PK_" + tableName.Substring(0, Math.Min(27, tableName.Length));

                //
                //  Create constraint PK
                //

                // alter table HAPI_BALANCE_QUERY
                // add (	
                // constraint PK_HAPI_BALANCE_QUERY primary key(HAPIRCV_ID)
                // );

                s.Append("alter table ");
                s.Append(tableName);
                s.Append("\r\n");
                s.Append("add (\r\n");
                s.Append("constraint ");
                s.Append(pkName);
                s.Append(" primary key\r\n");

                switch (code_target)
                {
                    case code_target.hapi:
                        {
                            s.Append("  (HAPIRCV_ID, SEQNUM)\r\n");
                            break;
                        }
                    case code_target.msg:
                        {
                            s.Append("  (MSG_IN_ID, SEQNUM)\r\n");
                            break;
                        }
                    case code_target.mapi:
                        {
                            s.Append("  (MAPI_IN_ID, SEQNUM)\r\n");
                            break;
                        }
                    default:
                        break;
                }

                s.Append("using index tablespace &&TBLSPC2_IX\r\n");
                s.Append(");\r\n");
                s.Append("\r\n");

                progressArgs.Update(++curr);
                OnProgress(progressArgs);
            }

            writer.Write(s.ToString());

            progressArgs.Update(total);
            OnProgress(progressArgs);
        }
    }
}
