#define ODP_NET
using System;
using System.Collections.Specialized;
using System.IO;
using System.Xml.Serialization;
using Imi.Framework.Shared;
using Imi.CodeGenerators.WebServices.Framework;
using System.Data;
using System.Data.Common;
#if ODP_NET
using Oracle.DataAccess.Client;
#else
using System.Data.OracleClient;
#endif

namespace Imi.CodeGenerators.WebServices.Spec2XML
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    class Class1
    {
        public void MakeProgress(object sender, ProgressEventArgs e)
        {
            Console.Write("\r" + e.Percent() + "% done");
            Console.Out.Flush();
        }


        [STAThread]
        static void Main(string[] args)
        {
            CommandLineParser cl = new CommandLineParser(args);

            String aPackage = cl["$1"];
            String aFileName = cl["$2"];
            String aInDictFileName = cl["$3"];
            String dbConnection = cl["CONN"];
            string optdict = cl["OPTDICT"];
            bool dict_is_optional = false;

            if (optdict != null)
                if (optdict == "1")
                    dict_is_optional = true;

            if (dbConnection == null)
                dbConnection = "user id = owuser; password = owuser; data source = WHTRUNK";

            if ((aPackage == null) || (aFileName == null))
            {
                Console.WriteLine("Reads a PL/SQL spec from the database and converts it to an XML file.");
                Console.WriteLine("");
                Console.WriteLine("Spec2XML package [drive:][path]filename [[drive:][path]dictionary-file-name] [/conn <connection string>] [/OPTDICT 1]");
                return;
            }

            IDbConnection OWDB = null;
            TextWriter writer = null;

            try
            {
                OWDB = new OracleConnection(dbConnection);
                OWDB.Open();

                Console.WriteLine("Reading spec source...");
                PLSQLSpecReader specreader = new PLSQLSpecReader(OWDB, aPackage);

                OWDB.Close();

                if (aFileName != null)
                {
                    Console.WriteLine("Parsing spec source...");
                    PLSQLSpecParser specparser = new PLSQLSpecParser(specreader.GetSpec());

                    Console.WriteLine("Generating XML...");

                    DictionaryDefinition dict = null;
                    if (aInDictFileName != null)
                    {
                        XmlSerializer dictserializer = new XmlSerializer(typeof(DictionaryDefinition));
                        TextReader dictreader = new StreamReader(aInDictFileName);
                        dict = dictserializer.Deserialize(dictreader) as DictionaryDefinition;
                    }
                    XMLGenerator xg = new XMLGenerator(null, null, dict);

                    writer = new StreamWriter(aFileName);
                    xg.GenerateXML(specparser.Package, dict_is_optional);
                    xg.GetOutPut(writer);
                }
            }
            finally
            {
                if (OWDB != null)
                    OWDB.Close();
                if (writer != null)
                    writer.Close();
            }
        }
    }
}
