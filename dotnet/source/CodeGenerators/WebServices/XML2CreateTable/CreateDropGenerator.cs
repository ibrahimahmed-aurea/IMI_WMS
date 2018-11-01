using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2CreateTable
{
  /// <summary>
  /// Summary description for CreateTableGenerator.
  /// </summary>
  public class CreateDropGenerator : GenericGenerator
  {
    private String GetCommentString(String[] comment)
    {
      if(comment == null)
        return("'n/a'");

      StringBuilder s = new StringBuilder();
      bool first = true;

      s.Append("'");

      foreach(String l in comment) 
      {
        if(first == false) 
          s.Append("\r\n");
        s.Append(l.Replace("'","''"));
        first = false;
      }

      s.Append("'");

      return(s.ToString());
    }

    public CreateDropGenerator(MessageDefinition messageDef, Package packageDef) : base( messageDef, packageDef )
    {
    }

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

        //
        //  Drop table
        //
        s.Append("drop table ");
        s.Append(tableName);
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
