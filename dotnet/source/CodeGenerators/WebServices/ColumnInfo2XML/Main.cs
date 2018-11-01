using System;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using Imi.Framework.Shared;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.ColumnInfo2XML
{
  /// <summary>
  /// Summary description for ColumnInfo2XMLMain.
  /// </summary>
  class ColumnInfo2XMLMain
  {

    public void MakeProgress(object sender, ProgressEventArgs e)
    {
      Console.Write("\r" + e.Percent() + "% done");
      Console.Out.Flush();
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      // Get command line parameters
      CommandLineParser cl = new CommandLineParser(args);
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
          "usage: columninfo2xml <filename source> <filename destination> [/conn <connection string>]");
        return;
      }

      try
      {
        try 
        {
          reader = new StreamReader(fileName);
        }
        catch(Exception e) 
        {
          Console.WriteLine("Failed to open input file '" + fileName + "'" + Environment.NewLine + 
            e.Message + Environment.NewLine + 
            e.StackTrace);
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
          return;
        }

        try 
        {
          // Load input document
          XmlSerializer serializer = new XmlSerializer(typeof(Package));
      
          Package package = serializer.Deserialize(reader) as Package;

          ColumnInfoGenerator cig = new ColumnInfoGenerator( null, package, db );

          // Initialize progress report
          ColumnInfo2XMLMain me = new ColumnInfo2XMLMain();
          cig.Progress += new ProgressEventHandler(me.MakeProgress);
          cig.GetOutPut( writer );
        }
        catch(Exception e) 
        {
          Console.WriteLine(e.Message + Environment.NewLine + 
            e.StackTrace);
        }
      }
      finally 
      {
        if(db != null) 
          db.Close();
        if(writer != null) 
          writer.Close();
        if(reader != null) 
          reader.Close();
      }
    }
  }
}