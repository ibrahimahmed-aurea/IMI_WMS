using System;
using System.IO;
using System.Xml.Serialization;
using Imi.CodeGenerators.WebServices.Framework;
using Imi.Framework.Shared;

namespace Imi.CodeGenerators.WebServices.XML2IntoProc
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
            String aType = cl["$4"];

            code_target ct;
            string mapi_number = "";

            if (cl.IsEnabled("msg"))
            {
                ct = code_target.msg;
            }
            else if (cl.IsEnabled("mapi"))
            {
                ct = code_target.mapi;
                mapi_number = cl.GetParameterValue("mapi");
            }
            else
            {
                ct = code_target.hapi;
            }

            bool spec;
            bool paramvalid;
            if (aType == null)
            {
                spec = false;
                paramvalid = true;
            }
            else
            {
                spec = (aType.ToUpper() == "SPEC");
                paramvalid = spec;
            }

            if ((aPackFileName == null) || (aStructDescFileName == null) || (aOutFileName == null) || (!paramvalid))
            {
                Console.WriteLine("Creates and writes PL/SQL source code inserts to queue tables");
                Console.WriteLine("If the switch /msg is used then source code for new message structures are created.");
                Console.WriteLine("Reads");
                Console.WriteLine(" 1) an XML file representing a PL/SQL package specification (auto generated).");
                Console.WriteLine(" 2) an XML file with structure description of the package and its objects (manually created).");
                Console.WriteLine("");
                Console.WriteLine("Writes");
                Console.WriteLine(" 3) a PL/SQL source file (spec or body depending on parameter SPEC present).");
                Console.WriteLine("XML2IntoProc [drive:][path]filename1 [drive:][path]filename2 [drive:][path]filename3 [SPEC] [/msg]");
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

            PLSQLGenerator psg = new PLSQLGenerator(msg, p, ct, mapi_number);
            psg.GetOutPut(Console.Out);

            using (StreamWriter sw = new StreamWriter(aOutFileName))
            {
                psg.GetOutPut(sw, !spec);
            }
            //Console.ReadLine();
        }
    }
}
