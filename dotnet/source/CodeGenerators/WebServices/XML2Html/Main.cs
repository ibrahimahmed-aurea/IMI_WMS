using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using Imi.CodeGenerators.WebServices.Framework;
using Imi.Framework.Shared;

namespace Imi.CodeGenerators.WebServices.XML2Html
{
  /// <summary>
  /// Summary description for XML2HtmlMain.
  /// </summary>
  class XML2HtmlMain
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
      String packageFileName     = cl["$1"]; // PackageFile
      String structureFileName   = cl["$2"]; // StructureFile
      String outDirectory        = cl["$3"]; // OutDirectory
      TextReader structureReader = null;
      TextReader packageReader   = null;

      if ((structureFileName == null) || (packageFileName == null) || (outDirectory == null))
      {
        Console.Error.WriteLine("Parameter missing." + Environment.NewLine +
          "usage: xml2html <package filename> <structure filename> <html directory>");
        return;
      }

      try 
      {
        structureReader = new StreamReader(structureFileName);
      }
      catch(Exception e) 
      {
        Console.WriteLine("Failed to open structure file '" + structureFileName + "'" + Environment.NewLine + 
          e.Message + Environment.NewLine + 
          e.StackTrace);
        return;
      }

      try 
      {
        packageReader = new StreamReader(packageFileName);
      }
      catch(Exception e) 
      {
        Console.WriteLine("Failed to open package file '" + packageFileName + "'" + Environment.NewLine + 
          e.Message + Environment.NewLine + 
          e.StackTrace);
        return;
      }


      try 
      {
        // Load structure document
        XmlSerializer structureSerializer = new XmlSerializer(typeof(MessageDefinition));
      
        MessageDefinition messageDef = structureSerializer.Deserialize(structureReader) as MessageDefinition;

        // Load package document
        XmlSerializer packageSerializer = new XmlSerializer(typeof(Package));
      
        Package package = packageSerializer.Deserialize(packageReader) as Package;

        XML2HtmlMain me = new XML2HtmlMain();

        // Create Structure pages
        HtmlGenerator hg = new HtmlGenerator(messageDef, package);
        hg.Progress += new ProgressEventHandler(me.MakeProgress);
        hg.Save(outDirectory);

        // Create Index page
        HtmlIndexGenerator hig = new HtmlIndexGenerator(messageDef, package);
        hig.Progress += new ProgressEventHandler(me.MakeProgress);
        hig.Save(outDirectory);

      }
      catch(Exception e) 
      {
        Console.WriteLine(e.Message + Environment.NewLine + 
          e.StackTrace);
      }
      finally 
      {
        if (structureReader != null)
          structureReader.Close();

        if (packageReader != null)
          packageReader.Close();
        
      }
    }
  }
}
