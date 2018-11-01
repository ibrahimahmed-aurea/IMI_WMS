using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2CreateTable
{
    /// <summary>
    /// Summary description for CreateViewGenerator.
    /// </summary>
    public class CreateViewGenerator : GenericGenerator
    {
        private code_target code_target;

        public CreateViewGenerator(MessageDefinition messageDef, Package packageDef, code_target ct)
            :
        base(messageDef, packageDef)
        {
            code_target = ct;
        }

        public override void GetOutPut(TextWriter writer)
        {
            StringBuilder s = new StringBuilder();

            int curr = 0;
            int total = messageDef.interfaces.Length + 1;
            ProgressEventArgs progressArgs;
            progressArgs = new ProgressEventArgs(curr, total);
            OnProgress(progressArgs);


            foreach (InterfaceType i in messageDef.interfaces)
            {
                StructureType[] structures = GetUniqueStructureList(i);

                bool first = true;

                switch (code_target)
                {
                    case code_target.hapi:
                        {
                            s.Append("create or replace view HAPI_");
                            s.Append(i.HAPIObjectName);
                            s.Append("_Segment_VW(HAPIRCV_ID,SEQNUM,TABLENAME,SEGMENTKEY) as\r\n");
                            break;
                        }
                    case code_target.msg:
                        {
                            s.Append("create or replace view MSG_IN_");
                            s.Append(i.HAPIObjectName);
                            s.Append("_Seg_VW(MSG_IN_ID,SEQNUM,TABLENAME,SEGMENTKEY) as\r\n");
                            break;
                        }
                    case code_target.mapi:
                        {
                            s.Append("create or replace view MAPI_IN_");
                            s.Append(i.HAPIObjectName);
                            s.Append("_Seg_VW(MAPI_IN_ID,SEQNUM,TABLENAME,SEGMENTKEY) as\r\n");
                            break;
                        }
                    default:
                        break;
                }

                foreach (StructureType structure in structures)
                {
                    if (structure.queueTable == "")
                        continue;

                    if (!first)
                        s.Append("union\r\n");
                    else
                        first = false;

                    switch (code_target)
                    {
                        case code_target.hapi:
                            {
                                s.Append("select HAPIRCV_ID\r\n");
                                break;
                            }
                        case code_target.msg:
                            {
                                s.Append("select MSG_IN_ID\r\n");
                                break;
                            }
                        case code_target.mapi:
                            {
                                s.Append("select MAPI_IN_ID\r\n");
                                break;
                            }
                        default:
                            break;
                    }

                    s.Append("      ,SEQNUM\r\n");
                    s.Append("      ,'");
                    s.Append(structure.queueTable);
                    s.Append("'\r\n");
                    s.Append("      ,ROWID\r\n");
                    s.Append(" from ");
                    s.Append(structure.queueTable);
                    s.Append("\r\n");
                }

                s.Append(";\r\n\r\n");

                progressArgs.Update(++curr);
                OnProgress(progressArgs);
            }

            writer.Write(s.ToString());

            progressArgs.Update(total);
            OnProgress(progressArgs);
        }

    }
}
