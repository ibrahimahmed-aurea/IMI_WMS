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
  public class CreateTableGenerator : GenericGenerator
  {
    private bool GenerateTableSpace;

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

    public CreateTableGenerator(MessageDefinition messageDef, Package packageDef, bool GenerateTableSpace ) : base( messageDef, packageDef )
    {
      this.GenerateTableSpace = GenerateTableSpace;
    }

    private bool IsTableField( ParameterType p )
    {
      return ( ( p.fieldType == ParameterTypeFieldType.SystemId ) ||
        ( p.fieldType == ParameterTypeFieldType.OpCode ) ||
        ( p.fieldType == ParameterTypeFieldType.Normal ) ||
        ( p.fieldType == ParameterTypeFieldType.SystemAdmin ) );
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
        //  Create table section
        //
        s.Append("create table ");
        s.Append(tableName);
        s.Append("(\r\n");

        ParameterType[] parameterList = GetUniqueParameterList(structure);

        int MaxLenFieldName = 0;
        int MaxLenDataType = 0;
        int MaxLenOrigin = 0;

        foreach ( ParameterType p in parameterList )
        {
          if ( IsTableField( p ) )
          {
            MaxLenFieldName = Math.Max( MaxLenFieldName, p.name.Length );
            MaxLenDataType  = Math.Max( MaxLenDataType,  GetDBDeclaration( p ).Length );
            MaxLenOrigin    = Math.Max( MaxLenOrigin,    (p.originTable + p.originColumn).Length + 1 );
          }
        }

        string First = " ";
        foreach(ParameterType p in parameterList) 
        {
          if ( IsTableField( p ) )
          {
            s.Append(" ");
            s.Append(First);
            First = ",";
            s.Append( p.name.PadRight(MaxLenFieldName) );
            s.Append(" ");
            s.Append( GetDBDeclaration( p ).ToLower().PadRight(MaxLenDataType) );
            s.Append(" /* ");
            s.Append( ( p.originTable + "." + p.originColumn).PadRight(MaxLenOrigin ) );

            if (p.column == null) 
              s.Append(" is not defined properly");

            s.Append(" */\r\n");
          }
        }

        s.Append(")");
        if ( GenerateTableSpace )
          s.Append(" tablespace &&TBLSPC2");
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
