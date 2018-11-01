using System;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using Imi.CodeGenerators.WebServices.Framework;
using Imi.Framework.Shared;

namespace Imi.CodeGenerators.WebServices.XML2WSOutTest
{
    /// <summary>
    /// Summary description for ColumnInfo2XMLMain.
    /// </summary>
    class XML2WSOutTestMain
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
            String structureFileName = cl["$1"];
            String outFileName = cl["$2"];
            String dictFileName = cl["$3"];
            String dbConnection = cl["CONN"];
            TextReader reader = null;
            TextWriter writer = null;
            TextReader dictReader = null;
            Database db = null;

            if (dbConnection == null)
                dbConnection = "user id = owuser; password = owuser; data source = WHTRUNK";

            if ((structureFileName == null) || (outFileName == null) || (dictFileName == null))
            {
                Console.Error.WriteLine("Parameter missing." + Environment.NewLine +
                  "usage: xml2wsouttest <structure filename> <outfile> <dictionary filename> [/conn <connection string>]");
                return;
            }

            try
            {
                try
                {
                    reader = new StreamReader(structureFileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to open input file '" + structureFileName + "'" + Environment.NewLine +
                      e.Message + Environment.NewLine +
                      e.StackTrace);
                    return;
                }

                try
                {
                    writer = new StreamWriter(outFileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to open output file '" + outFileName + "'" + Environment.NewLine +
                      e.Message + Environment.NewLine +
                      e.StackTrace);
                    return;
                }

                try
                {
                    dictReader = new StreamReader(dictFileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to open dictionary file '" + dictFileName + "'" + Environment.NewLine +
                      e.Message + Environment.NewLine +
                      e.StackTrace);
                    return;
                }

                try
                {
                    db = new Database(dbConnection);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to open database connection using connection string '" + dbConnection + "'" + Environment.NewLine +
                      e.Message + Environment.NewLine +
                      e.StackTrace);
                    return;
                }

                try
                {
                    // Load input document
                    XmlSerializer serializer = new XmlSerializer(typeof(MessageDefinition));
                    MessageDefinition messageDef = serializer.Deserialize(reader) as MessageDefinition;

                    // Load dictionary
                    XmlSerializer dictserializer = new XmlSerializer(typeof(DictionaryDefinition));
                    DictionaryDefinition dictDef = dictserializer.Deserialize(dictReader) as DictionaryDefinition;

                    PackageGenerator pg = new PackageGenerator(messageDef, null, dictDef, db);

                    // Initialize progress report
                    XML2WSOutTestMain me = new XML2WSOutTestMain();
                    pg.Progress += new ProgressEventHandler(me.MakeProgress);
                    pg.GetOutPut(writer);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + Environment.NewLine +
                      e.StackTrace);
                }
            }
            finally
            {
                if (db != null)
                    db.Close();
                if (writer != null)
                    writer.Close();
                if (reader != null)
                    reader.Close();
            }
        }
    }
}