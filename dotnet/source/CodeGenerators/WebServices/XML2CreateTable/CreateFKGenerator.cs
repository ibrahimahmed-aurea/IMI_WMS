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
    public class CreateFKGenerator : GenericGenerator
    {
        private code_target code_target;

        public CreateFKGenerator(MessageDefinition messageDef, Package packageDef, code_target ct)
            :
        base(messageDef, packageDef)
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

                // bad solution but it might work
                String fkTableName;
                if (tableName.Length > 19)
                    fkTableName = tableName.Substring(0, 9) + "_" + tableName.Substring(tableName.Length - 9, 9);
                else
                    fkTableName = tableName;

                String fkName;

                switch (code_target)
                {
                    case code_target.hapi:
                        {
                            fkName = "FK_HAPIRCV_" + fkTableName.Substring(0, Math.Min(19, fkTableName.Length));
                            break;
                        }
                    case code_target.msg:
                        {
                            fkName = "FK_MSG_IN_" + fkTableName.Substring(0, Math.Min(19, fkTableName.Length));
                            break;
                        }
                    case code_target.mapi:
                        {
                            fkName = "FK_MAPI_IN_" + fkTableName.Substring(0, Math.Min(19, fkTableName.Length));
                            break;
                        }
                    default:
                        fkName = "Internal error";
                        break;
                }

                //
                //  Create constraint FK
                //

                // alter table ALMTXT
                // add ( 
                // constraint FK_ALM_ALMTXT foreign key(ALMID) references ALM(ALMID)
                // );

                s.Append("alter table ");
                s.Append(tableName);
                s.Append("\r\n");
                s.Append("add (\r\n");
                s.Append("constraint ");
                s.Append(fkName);
                s.Append(" foreign key\r\n");

                switch (code_target)
                {
                    case code_target.hapi:
                        {
                            s.Append("  (HAPIRCV_ID)\r\n");
                            s.Append("  references HAPIRCV(HAPIRCV_ID) on delete cascade\r\n");
                            break;
                        }
                    case code_target.msg:
                        {
                            s.Append("  (MSG_IN_ID)\r\n");
                            s.Append("  references MSG_IN(MSG_IN_ID) on delete cascade\r\n");
                            break;
                        }
                    case code_target.mapi:
                        {
                            s.Append("  (MAPI_IN_ID)\r\n");
                            s.Append("  references MAPI_IN(MAPI_IN_ID) on delete cascade\r\n");
                            break;
                        }
                    default:
                        break;
                }

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
