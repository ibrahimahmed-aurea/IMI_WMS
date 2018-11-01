using System;
using System.IO;
using System.Xml.Serialization;
using Imi.CodeGenerators.WebServices.Framework;
using Imi.Framework.Shared;
using Imi.Framework.Versioning;


namespace Imi.CodeGenerators.WebServices.XML2WSInbound
{
    public enum code_target { hapi, msg, mapi };

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
            String outType = cl["$4"]; // RECEIVER | SENDER

            code_target ct;

            if (cl.IsEnabled("msg"))
            {
                ct = code_target.msg;
            }
            else if (cl.IsEnabled("mapi"))
            {
                ct = code_target.mapi;
            }
            else
            {
                ct = code_target.hapi;
            }

            String aNameSpace = cl["NS"];

            if (aNameSpace == null)
            {
                aNameSpace = "Imi.Wms.WebServices.ExternalInterface";
            }

            bool genReceiver = false;
            bool genSender = false;

            if (outType != null)
            {
                genReceiver = (outType.ToUpper() == "RECEIVER");
                genSender = (outType.ToUpper() == "SENDER");
            }

            if ((aPackFileName == null) || (aStructDescFileName == null) || (aOutFileName == null))
            {
                Console.WriteLine("Creates and writes C# source code for external inbound interface");
                Console.WriteLine("If the switch /msg is used then source code for new message structures are created.");
                Console.WriteLine("Reads");
                Console.WriteLine(" 1) an XML file representing a PL/SQL package specification (auto generated).");
                Console.WriteLine(" 2) an XML file with structure description of the package and its objects (manually created).");
                Console.WriteLine("");
                Console.WriteLine("Writes");
                Console.WriteLine(" 3) a C# file");
                Console.WriteLine(" 4) Interface direction comment");
                Console.WriteLine("XML2WSInbound [drive:][path]filename1 [drive:][path]filename2 [drive:][path]filename3 [ RECEIVER | SENDER ] [/msg] [/ns your.namespace]");
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

            WSInboundGenerator wig = new WSInboundGenerator(msg, p, genReceiver, genSender, ct, aNameSpace);

            using (StreamWriter sw = new StreamWriter(aOutFileName))
            {
                wig.GetOutPut(sw);
            }
            //Console.ReadLine();
        }
    }
}
