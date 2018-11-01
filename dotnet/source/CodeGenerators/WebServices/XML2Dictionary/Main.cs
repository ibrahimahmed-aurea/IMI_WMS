using System;
using System.IO;
using System.Xml.Serialization;
using Imi.CodeGenerators.WebServices.Framework;
using Imi.Framework.Shared;

namespace Imi.CodeGenerators.WebServices.XML2Dictionary
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
            String aInDictFileName = cl["$4"];

            if ((aPackFileName == null) || (aStructDescFileName == null) || (aOutFileName == null))
            {
                Console.WriteLine("Extends/creates an xml-dictionary for getting proper case of interface fields");
                Console.WriteLine("XML2Dictionary [drive:][path]<xml-package-file> [drive:][path]<xml-master-file> [drive:][path]<created-dictionary-file> [[drive:][path]<master-dictionary-file>]");
                return;
            }

            // Creates an instance of the XmlSerializer class;
            // specifies the type of object to serialize.
            XmlSerializer packserializer = new XmlSerializer(typeof(Package));
            TextReader packreader = new StreamReader(aPackFileName);
            Package p;
            p = packserializer.Deserialize(packreader) as Package;

            XmlSerializer structserializer = new XmlSerializer(typeof(MessageDefinition));
            TextReader structreader = new StreamReader(aStructDescFileName);
            MessageDefinition msg;
            msg = structserializer.Deserialize(structreader) as MessageDefinition;

            DictionaryDefinition dict = null;
            if (aInDictFileName != null)
            {
                XmlSerializer dictserializer = new XmlSerializer(typeof(DictionaryDefinition));
                TextReader dictreader = new StreamReader(aInDictFileName);
                dict = dictserializer.Deserialize(dictreader) as DictionaryDefinition;
            }

            DictionaryGenerator sg = new DictionaryGenerator(msg, p, dict);

            using (StreamWriter sw = new StreamWriter(aOutFileName))
            {
                sg.GetOutPut(sw);
            }
            //Console.ReadLine();
        }
    }
}
