using System;
using System.Xml.Serialization;
using System.IO;
using Imi.CodeGenerators.WebServices.Framework;
using Imi.Framework.Shared;

// TODO option for creating table in database immediately ??

namespace Imi.CodeGenerators.WebServices.XML2CreateTable
{
    public enum code_target { hapi, msg, mapi };

    /// <summary>
    /// Summary description for XML2CreateTableMain.
    /// </summary>
    class XML2CreateTableMain
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
            String packageFileName = cl["$1"]; // PackageFile
            String structureFileName = cl["$2"]; // StructureFile
            String outFileName = cl["$3"]; // OutfileFile
            String outType = cl["$4"]; // COLUMN | COLUMN_NOTBSP | PK  | COMMENT | TRIGGER | VIEW | DROP | FK | CLEAN

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
            
            TextReader structureReader = null;
            TextReader packageReader = null;
            TextWriter writer = null;

            bool genColumn = false;
            bool genColumnNoTableSpace = false;
            bool genPk = false;
            bool genComment = false;
            bool genTrigger = false;
            bool genView = false;
            bool genDrop = false;
            bool genFk = false;
            bool genClean = false;

            if (outType != null)
            {
                genColumn = (outType.ToUpper() == "COLUMN");
                genColumnNoTableSpace = (outType.ToUpper() == "COLUMN_NOTBSP");
                genPk = (outType.ToUpper() == "PK");
                genComment = (outType.ToUpper() == "COMMENT");
                genTrigger = (outType.ToUpper() == "TRIGGER");
                genView = (outType.ToUpper() == "VIEW");
                genDrop = (outType.ToUpper() == "DROP");
                genFk = (outType.ToUpper() == "FK");
                genClean = (outType.ToUpper() == "CLEAN");
            }

            if ((structureFileName == null) || (packageFileName == null) || (outFileName == null))
            {
                Console.Error.WriteLine("Parameter missing." + Environment.NewLine +
                  "usage: XML2CreateTable <package filename> <structure filename> <filename destination> { COLUMN | COLUMN_NOTBSP | PK  | COMMENT | TRIGGER | VIEW | DROP | FK | CLEAN } [/msg]");
                return;
            }

            try
            {
                structureReader = new StreamReader(structureFileName);
            }
            catch (Exception e)
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
            catch (Exception e)
            {
                Console.WriteLine("Failed to open package file '" + packageFileName + "'" + Environment.NewLine +
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
                // Load structure document
                XmlSerializer structureSerializer = new XmlSerializer(typeof(MessageDefinition));

                MessageDefinition messageDef = structureSerializer.Deserialize(structureReader) as MessageDefinition;

                // Load package document
                XmlSerializer packageSerializer = new XmlSerializer(typeof(Package));
                Package package = packageSerializer.Deserialize(packageReader) as Package;
                XML2CreateTableMain me = new XML2CreateTableMain();

                if (genDrop)
                {
                    CreateDropGenerator cdropg = new CreateDropGenerator(messageDef, package);
                    cdropg.Progress += new ProgressEventHandler(me.MakeProgress);
                    cdropg.GetOutPut(writer);
                    writer.Flush();
                }

                if (genColumn || genColumnNoTableSpace)
                {
                    CreateTableGenerator ctg = new CreateTableGenerator(messageDef, package, genColumn);
                    ctg.Progress += new ProgressEventHandler(me.MakeProgress);
                    ctg.GetOutPut(writer);
                    writer.Flush();
                }

                if (genComment)
                {
                    CreateCommentGenerator ccommentg = new CreateCommentGenerator(messageDef, package);
                    ccommentg.Progress += new ProgressEventHandler(me.MakeProgress);
                    ccommentg.GetOutPut(writer);
                    writer.Flush();
                }

                if (genPk)
                {
                    CreatePKGenerator cpkg = new CreatePKGenerator(messageDef, package, ct);
                    cpkg.Progress += new ProgressEventHandler(me.MakeProgress);
                    cpkg.GetOutPut(writer);
                    writer.Flush();
                }

                if (genTrigger)
                {
                    CreateTriggerGenerator ctrg = new CreateTriggerGenerator(messageDef, package);
                    ctrg.Progress += new ProgressEventHandler(me.MakeProgress);
                    ctrg.GetOutPut(writer);
                    writer.Flush();
                }

                if (genView)
                {
                    CreateViewGenerator cvg = new CreateViewGenerator(messageDef, package, ct);
                    cvg.Progress += new ProgressEventHandler(me.MakeProgress);
                    cvg.GetOutPut(writer);
                    writer.Flush();
                }

                if (genFk)
                {
                    CreateFKGenerator cfkg = new CreateFKGenerator(messageDef, package, ct);
                    cfkg.Progress += new ProgressEventHandler(me.MakeProgress);
                    cfkg.GetOutPut(writer);
                    writer.Flush();
                }

                if (genClean)
                {
                    CreateCleanGenerator ccleang = new CreateCleanGenerator(messageDef, package, ct, mapi_number);
                    ccleang.Progress += new ProgressEventHandler(me.MakeProgress);
                    ccleang.GetOutPut(writer);
                    writer.Flush();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + Environment.NewLine +
                  e.StackTrace);
            }
            finally
            {
                if (structureReader == null)
                    structureReader.Close();

                if (packageReader == null)
                    packageReader.Close();

                if (writer == null)
                    writer.Close();
            }
        }
    }
}