using System;
using System.IO;
using System.Data;
using System.Xml.Serialization;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.ColumnInfo2XML
{

  public class ColumnInfoGenerator : GenericGenerator
  {
    private static char[] delimiterArr = {'\r','\n'};
    Database db = null;

    public ColumnInfoGenerator( MessageDefinition messageDef, Package packageDef, Database db ) :
      base( messageDef, packageDef )
    {
      this.db = db;
    }

    private static String GetDataTypeDeclaration(String datatype, String length, String precision,  String scale)
    {

      String typeDeclaration = datatype;

      switch(datatype) 
      {
        case "VARCHAR2":
          typeDeclaration = datatype + "(" + length + ")";
          break;
        case "NUMBER":
          if ((scale != "") && (scale != "0")) 
            typeDeclaration =datatype + "(" + precision + "," + scale + ")";
          else
          {
            if (precision != "" )
              typeDeclaration =datatype + "(" + precision + ")";
          }
          break;

      }

      return(typeDeclaration);
    }

    public void AddColumn(ParameterType aParameter)
    {
      ColumnType  col = null;
      IDataReader r   = null;

      try 
      {
        // Don't do anything if no table or column is given
        if ((aParameter.originTable == "") || (aParameter.originColumn == ""))
          return;

        r = db.ExecuteReader( 
          "select U1.COLUMN_NAME, " +
          "       U2.DATA_TYPE, " +
          "       TO_CHAR(decode(U2.DATA_TYPE, 'VARCHAR2', U2.CHAR_LENGTH, 'CHAR', U2.CHAR_LENGTH, U2.DATA_LENGTH))    DATA_LENGTH, " +
          "       TO_CHAR(U2.DATA_PRECISION) DATA_PRECISION, " +
          "       TO_CHAR(U2.DATA_SCALE)     DATA_SCALE, " +
          "       U1.COMMENTS " +
          "  from USER_TAB_COLUMNS  U2, " +
          "       USER_COL_COMMENTS U1 " +
          " where U2.TABLE_NAME  = '" + aParameter.originTable.ToUpper() + "'" +
          "   and U2.COLUMN_NAME = '" + aParameter.originColumn.ToUpper() + "'" +
          "   and U2.TABLE_NAME  = U1.TABLE_NAME " +
          "   and U2.COLUMN_NAME = U1.COLUMN_NAME");

        if(r.Read())
        {
          // preserve mandatory, it has already been read
          if ( aParameter.column != null )
            col = aParameter.column;
          else
            col = new ColumnType();

          String name    = r.GetString(0);
          String comment = "";

          // Read column comments
          if(!r.IsDBNull(5))
            comment = r.GetString(5);
          
          String[] commentSplit = comment.Split(delimiterArr,100);
          
          if(commentSplit.Length > 0) 
            col.comment = (String[])commentSplit.Clone();

          col.caption = commentSplit[0];

          if ((col.caption == null))
            col.caption = "";

          // determine datatype, length etc
          col.dataType  = r.GetString(1);
          col.length    = "";
          col.precision = "";
          col.scale     = "";

          if (!r.IsDBNull(2))
            col.length = r.GetString(2);

          if (!r.IsDBNull(3))
            col.precision =  r.GetString(3);

          if (!r.IsDBNull(4))
            col.scale =  r.GetString(4); 
       
          // Ugly code
          aParameter.column = col;
          aParameter.column.declaration = GetDBDeclaration(aParameter);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine("Error while processing " + aParameter.name + Environment.NewLine +
          e.Message + Environment.NewLine + 
          e.StackTrace);
        throw e;
      }
      finally 
      {
        if (r != null)
          r.Close();
      }
    }

    public void UpdatePackageDef() 
    {
      int curr = 0;
      int total = packageDef.procedures.Length + 1;
      ProgressEventArgs progressArgs = new ProgressEventArgs(curr,total);
      OnProgress(progressArgs);

      foreach(ProcedureType procedure in packageDef.procedures) 
      {
        foreach(ParameterType parameter in procedure.parameters) 
        {
          AddColumn(parameter);
        }
        progressArgs.Update(++curr);
        OnProgress(progressArgs);
      }

      progressArgs.Update(total);
      OnProgress(progressArgs);
    }

    public override void GetOutPut( TextWriter tw )
    {
      UpdatePackageDef();
      XmlSerializer serializer = new XmlSerializer(typeof(Package));
      serializer.Serialize(tw,packageDef);
    }

  }
}
