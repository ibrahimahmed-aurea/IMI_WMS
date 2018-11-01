#define ODP_NET
using System;
using System.Collections.Specialized;
using System.IO;
using Imi.Framework.Shared;
using System.Data;
using System.Data.Common;
#if ODP_NET
using Oracle.DataAccess.Client;
#else
using System.Data.OracleClient;
#endif

namespace Imi.CodeGenerators.PackageGenerator
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

            String aPackage = cl["$1"];         // Oracle Packagename
            String aOutFileName = cl["$2"];     // Outfile to write to.
            String aNamespace = cl["$3"];       // Namespace that the generated code is placed in.
            String dbConnection = cl["CONN"];   // Optional connection string. Defaults to owuser/owuser in WHTRUNK.
            
            if (dbConnection == null)
                dbConnection = "user id = owuser; password = owuser; data source = WHTRUNK";

            if ((aPackage == null) || (aOutFileName == null) || (aNamespace == null) ) 
            {
                Console.WriteLine("Creates and writes C# source code for calling an oracle package");
                Console.WriteLine("Reads");
                Console.WriteLine(" 1) an oracle package from the database.");
                Console.WriteLine("");
                Console.WriteLine("Writes");
                Console.WriteLine(" 2) a C# source file.");
                Console.WriteLine("PackageGenerator <PackageName> <[drive:][path]Filename> <Namespace> [/conn <Connectionstring>]");

                // Example Commandline: MessageSend DBWebServiceSendRMS.cs Imi.Wms.Server.Job.WebServiceSendRMS.PLSQLInterface /conn "user id = rmuser; password = rmuser; data source = WHTRUNK"

                return;
            }

            aPackage = aPackage.ToUpper();

            IDbConnection OWDB;

            OWDB = new OracleConnection(dbConnection);
            OWDB.Open();

            TPackageReader pkg = new TPackageReader(OWDB, aPackage);

            OWDB.Close();

            using (StreamWriter sw = new StreamWriter(aOutFileName))
            {
                sw.WriteLine(pkg.GetFullPackage(cl.GetCommandLine(), aNamespace));
            }

        }
    }
}
