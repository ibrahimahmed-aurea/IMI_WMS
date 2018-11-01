using System;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using wscc;
using Warehouse.SchemaStructures;

namespace pkgcc2
{
  /// <summary>
  /// Summary description for Class1.
  /// </summary>
  class Class1
  {
    public static ColumnType createColumn(ParameterType aParameter)
    {
      ColumnType  col = null;
      IDataReader r   = null;
      Database    db  = null;

      // TODO need to handle return values
      try 
      {
        // Don't do anything if no table or column is given
        if ((aParameter.originTable == "") || (aParameter.originColumn == ""))
          return(null);

        db = Database.getInstance();

        r = db.ExecuteReader( 
          "select U1.COLUMN_NAME, " +
          "       U2.DATA_TYPE, " +
          "       TO_CHAR(U2.DATA_LENGTH)    DATA_LENGTH, " +
          "       TO_CHAR(U2.DATA_PRECISION) DATA_PRECISION, " +
          "       TO_CHAR(U2.DATA_SCALE)     DATA_SCALE, " +
          "       U1.COMMENTS " +
          "  from USER_TAB_COLUMNS  U2, " +
          "       USER_COL_COMMENTS U1 " +
          " where U2.TABLE_NAME  = '" + aParameter.originTable  + "'" +
          "   and U2.COLUMN_NAME = '" + aParameter.originColumn + "'" +
          "   and U2.TABLE_NAME  = U1.TABLE_NAME " +
          "   and U2.COLUMN_NAME = U1.COLUMN_NAME");

        if(r.Read())
        {
          col = new ColumnType();

          String name    = r.GetString(0);
          String comment = "";

          // Read column comments
          if(!r.IsDBNull(5))
            comment = r.GetString(5);
          
          String[] commentSplit = TextUtility.splitComment(comment);
          
          if(commentSplit.Length > 0) 
            col.comment = (String[])commentSplit.Clone();

          col.caption = TextUtility.CapitalizeName(commentSplit[0]);

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
        
          col.declaration = TextUtility.GetDataTypeDeclaration(col.dataType, col.length, col.precision, col.scale);

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

      return(col);
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      // Get command line parameters
      CommandLine cl      = new CommandLine(args);
      String fileName     = cl["$1"];
      String outFileName  = cl["$2"];
      String dbConnection = cl["CONN"];
      TextReader reader   = null;
      TextWriter writer   = null;
      Database db         = null;

      if( dbConnection == null )
        dbConnection = "user id = owuser; password = owuser; data source = WHTRUNK";

      if ((fileName == null) || (outFileName == null))
      {
        Console.Error.WriteLine("Parameter missing." + Environment.NewLine +
          "usage: pkgcc2 <filename source> <filename destination> [/conn <connection string>]");
        return;
      }

      try 
      {
        reader = new StreamReader(fileName);
      }
      catch(Exception e) 
      {
        Console.WriteLine("Failed to open input file '" + fileName + "'" + Environment.NewLine + 
          e.Message + Environment.NewLine + 
          e.StackTrace);
        Console.ReadLine();
        return;
      }

      try 
      {
        writer = new StreamWriter(outFileName);
      }
      catch(Exception e) 
      {
        Console.WriteLine("Failed to open output file '" + outFileName + "'" + Environment.NewLine + 
          e.Message + Environment.NewLine + 
          e.StackTrace);
        Console.ReadLine();
        return;
      }

      try 
      {
        db = new Database( dbConnection );
      }
      catch(Exception e) 
      {
        Console.WriteLine("Failed to open database connection using connection string '" + dbConnection + "'" + Environment.NewLine + 
          e.Message + Environment.NewLine + 
          e.StackTrace);
        Console.ReadLine();
        return;
      }

      try 
      {
        // Load input document
        XmlSerializer serializer = new XmlSerializer(typeof(Package));
      
        Package package = serializer.Deserialize(reader) as Package;

        ColumnType col = null;
        Class1 me = new Class1();

        double i = 0;

        foreach(ProcedureType procedure in package.procedures) 
        {
          Console.Write("\r" + Convert.ToString(Math.Ceiling((i++ / (double)package.procedures.Length)*100)) + "% done");
          Console.Out.Flush();
          
          foreach(ParameterType parameter in procedure.parameters) 
          {
            col = createColumn(parameter);
            parameter.column = col;
          }
        }

        Console.WriteLine("\r100% done");

        serializer.Serialize(writer,package);
      }
      catch(Exception e) 
      {
        Console.WriteLine(e.Message + Environment.NewLine + 
          e.StackTrace);
      }
    }
  }
}