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
  public class CreateTriggerGenerator : GenericGenerator
  {
    public CreateTriggerGenerator(MessageDefinition messageDef, Package packageDef) : 
      base( messageDef, packageDef )  {}

    public override void GetOutPut(TextWriter writer) 
    {
      StringBuilder s = new StringBuilder();

      int curr = 0;
      int total = messageDef.structures.Length + 1;
      ProgressEventArgs progressArgs;
      progressArgs = new ProgressEventArgs(curr,total);
      OnProgress(progressArgs);

      foreach(StructureType structure in messageDef.structures) 
      {
        if ( structure.queueTable == "" )
          continue;

        // Table name is the short name from
        // the structure document
        String tableName = GetTableName(structure.queueTable);

        // Bad solution but it works
        String triggerName = tableName.Substring(0,Math.Min(26,tableName.Length)) + "_Trc";
        //
        //  Create trigger
        //
        s.Append("/************************************************************************************/\r\n");
        s.Append("create or replace trigger ");
        s.Append(triggerName);
        s.Append("\r\n");
        s.Append("before insert or update on ");
        s.Append(tableName);
        s.Append("\r\n");
        s.Append("for each row\r\n");
        s.Append("declare\r\n");
        s.Append("begin\r\n");
        s.Append("\r\n");
        s.Append("  /* Set debugging columns */\r\n");
        s.Append("\r\n");
        s.Append("  if true then\r\n");
        s.Append("    Process.ID_Read(:new.PROID);\r\n");
        s.Append("    :new.UPDDTM := sysdate;\r\n");
        s.Append("  end if;\r\n");
        s.Append("\r\n");
        s.Append("end;\r\n");
        s.Append("/\r\n");
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
