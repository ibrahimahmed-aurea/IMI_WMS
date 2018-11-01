using System;
using System.IO;
using System.Xml.Serialization;
using Imi.CodeGenerators.WebServices.Framework;
using Imi.Framework.Shared;

namespace Imi.CodeGenerators.WebServices.XML2BizTalkDataTypes
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
      CommandLineParser cl = new CommandLineParser(args);

      String aPackFileName = cl["$1"];
      String aStructDescFileName = cl["$2"];
      String aOutFileName = cl["$3"];

      if ( (aPackFileName == null) || (aStructDescFileName == null) || (aOutFileName == null) )
      {
        Console.WriteLine( "Creates and writes C# source code objects from an oracle package" );
        Console.WriteLine( "Reads" );
        Console.WriteLine( " 1) an XML file representing a PL/SQL package specification (auto generated)." );
        Console.WriteLine( " 2) an XML file with structure description of the package and its objects (manually created)." );
        Console.WriteLine( "" );
        Console.WriteLine( "Writes" );
        Console.WriteLine( " 3) a C# class interface file." );
        Console.WriteLine( "XML2Struct [drive:][path]filename1 [drive:][path]filename2 [drive:][path]filename3." );
        return;
      }

      // Creates an instance of the XmlSerializer class;
      // specifies the type of object to serialize.
      XmlSerializer packserializer = new XmlSerializer( typeof( Package ) );
      TextReader packreader = new StreamReader( aPackFileName );
      Package p;
      p = packserializer.Deserialize( packreader ) as Package;

      XmlSerializer structserializer = new XmlSerializer( typeof( MessageDefinition ) );
      TextReader structreader = new StreamReader( aStructDescFileName );
      MessageDefinition msg;
      msg = structserializer.Deserialize( structreader ) as MessageDefinition;

      BZDataTypeGen sg = new BZDataTypeGen( msg, p );
      
      using ( StreamWriter sw = new StreamWriter( aOutFileName ) ) 
      {
        sg.GetOutPut( sw );
      }
      //Console.ReadLine();
    }
	}
}
