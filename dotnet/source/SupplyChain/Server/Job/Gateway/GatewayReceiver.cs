using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using Imi.Framework.Job;
using Imi.Framework.Job.Data;
using Imi.SupplyChain.Server.Job.StandardJob;
using Imi.SupplyChain.Server.Job.Gateway.OraclePackage;
using System.Text;

namespace Imi.SupplyChain.Server.Job.Gateway
{
    public class GatewayReceiver : OracleJob
    {
        private DBGateway dbGateway;
        private DBFindEdiPartner dbFindEdiPartner;
        private string logFileName;
        private int logLevel = 2;
        private bool logEnabled = false;

        public GatewayReceiver(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
        }

        protected override void CreateProcedure(IDbConnectionProvider connection)
        {
            dbGateway = new DBGateway(connection);
            dbFindEdiPartner = new DBFindEdiPartner(connection);
        }

        private Dictionary<string, EdiPartner> partners = new Dictionary<string, EdiPartner>();

        protected override void ExecuteProcedure(JobArgumentCollection args)
        {
            if(args.ContainsKey("Log"))
                logEnabled = args["Log"].ToString().ToLower().Equals("true");

            if (args.ContainsKey("LogFile"))
                logFileName = args["LogFile"];

            if (args.ContainsKey("LogLevel"))
                logLevel = Convert.ToInt16(args["LogLevel"]);

            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Start");

            bool tryAgain;
            long start = DateTime.Now.Ticks;

            do
            {
                tryAgain = false;
                UpdatePartners();

                foreach (EdiPartner partner in partners.Values)
                {
                    if (partner.IsValid)
                    {
                        Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format(" Processing EdiPartner_Id  \"{0}\" ", partner.Id));

                        SortedList files = GetInputFiles(partner.ReceiveDirectory);
                        Tracing.TraceEvent(TraceEventType.Verbose, 0, string.Format("Found {0} files", files.Count));

                        for (int i = 0; i < files.Count; i++)
                        {
                            string currentFile = files.GetByIndex(i) as string;
                            if (!string.IsNullOrEmpty(currentFile))
                            {
                                Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("Processing: {0}", currentFile));
                                ProcessOneFile(partner, currentFile);
                                tryAgain = true;
                            }
                        }

                    }
                    else
                    {
                        Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format(" Skipping EdiPartner_Id  \"{0}\" ", partner.Id));
                    }
                }

            } while (tryAgain); // Do extra lap if a file was found for any partner

            long stop = DateTime.Now.Ticks;
            TimeSpan ts = new TimeSpan(stop - start);

            Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("Stop (used {0}s)", ts.TotalSeconds.ToString("0.00")));
        }

        private void UpdatePartners()
        {
            IList<EdiPartner> partnerList = dbFindEdiPartner.FindEdiPartners();

            foreach (EdiPartner partner in partnerList)
            {
                bool checkPartner = true;
                bool addPartner = true;

                // Check for updates
                if (partners.ContainsKey(partner.Id))
                {
                    if (partner.Equals(partners[partner.Id]))
                    {
                        checkPartner = false;
                        addPartner = false;
                    }
                }

                if (checkPartner)
                {
                    Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format(" Checking EdiPartner_Id  \"{0}\" ", partner.Id));
                    Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("         with RCVDIR    {0}", partner.ReceiveDirectory));
                    Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("         with RCVDIRSAV {0}", partner.ReceiveDirectorySave));
                    Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("         with RCVDIRERR {0}", partner.ReceiveDirectoryError));

                    partner.IsValid = ReceiptDirectoriesOk(partner);

                    if (partner.IsValid)
                    {
                        Tracing.TraceEvent(TraceEventType.Verbose, 0, "Receive directories are ok");
                        if (addPartner)
                        {
                            partners[partner.Id] = partner;
                        }
                    }
                    else
                    {
                        Tracing.TraceEvent(TraceEventType.Error, 0, "Receive directories are NOT ok");
                    }
                }
            }
        }

        protected override void CancelProcedure()
        {
            if (dbGateway != null)
            {
                dbGateway.Cancel();
            }
            if (dbFindEdiPartner != null)
            {
                dbFindEdiPartner.Cancel();
            }
        }

        /*
         * Treat the current file
         * 
         * read the text file and pass each record to the 
         * database handler
         * 
         */
        private bool TreatFile(String filename)
        {
            try
            {
                int segmentCount = 0;
                int cc = 0;

                // first we start a database transaction
                StartTransaction();

                if (logEnabled)
                {
                    dbGateway.EnableServerLog(null, logFileName, logLevel);
                }

                Encoding encoding = Encoding.Default;

                if (args.ContainsKey("ReceiveEncoding"))
                {
                    string receiveEncoding = args["ReceiveEncoding"];
                    
                    if (!string.IsNullOrEmpty(receiveEncoding))
                        encoding = Encoding.GetEncoding(receiveEncoding);
                }
                
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader streamReader = new StreamReader(filename, encoding))
                {
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    string line;

                    do
                    {
                        line = streamReader.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                        {
                            segmentCount++;

                            cc = dbGateway.WriteSegment(filename, segmentCount, line);

                            if (cc != 0)		// an error occured
                                break;			// we leave the "while" loop
                        }

                    } while (line != null);
                }

                if (cc == 0)			// everything ok, we check for completeness
                {
                    cc = dbGateway.EndOfInterchange();
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0, String.Format("Data error in line {0} of file {1}", segmentCount, filename));
                }

                Commit();

                return (cc == 0);
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Tracing.TraceEvent(TraceEventType.Error, 0, string.Format("The file could not be read: {0}", e.Message));
                return false;
            }
            finally
            {
                try
                {
                    if (logEnabled)
                    {
                        dbGateway.DisableServerLog();
                    }
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// Process one file for the current EDI Partner 
        /// move data from file to database
        /// depending on result move it to save- or error-directory
        /// if no directory specified just remove the file 
        /// </summary>
        /// <param name="partner">Partner that sent the file</param>
        /// <param name="filename">Contents of file</param>
        private void ProcessOneFile(EdiPartner partner, String filename)
        {
            // move data from file to database
            string targetDir = partner.ReceiveDirectorySave;

            if (!TreatFile(filename))
            {
                targetDir = partner.ReceiveDirectoryError;
            }

            // if target directory is set, move file otherwise delete it
            if (string.IsNullOrEmpty(targetDir))
            {
                File.Delete(filename);
            }
            else
            {
                string newFile = Path.Combine(targetDir, Path.GetFileName(filename));

                if (File.Exists(newFile))
                {
                    File.Delete(newFile);
                }
                
                File.Move(filename, newFile);
            }
        }

        public static SortedList GetInputFiles(string directory)
        {
            SortedList sortedFileList = new SortedList();

            // check if there are any files to process
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);

            // read directory contents
            FileSystemInfo[] directoryFiles = directoryInfo.GetFiles();

            foreach (FileInfo fileInfo in directoryFiles)
            {
                DateTime key = fileInfo.CreationTime;

                // Make sure key is unique
                while (sortedFileList.ContainsKey(key))
                {
                    key = key.AddMilliseconds(1);
                }

                sortedFileList.Add(key, fileInfo.FullName);
            }

            return sortedFileList;
        }

        /**
         * check if all receive directories are defined and exist
         *		RCVDIR has to be defined and exist
         *		RCVDIRSAV has to exist if it is defined, or not be defined
         *		RCVDIRERR has to exist if it is defined, or not be defined
         */
        public static bool ReceiptDirectoriesOk(EdiPartner partner)
        {
            return (
                    (!string.IsNullOrEmpty(partner.ReceiveDirectory) && Directory.Exists(partner.ReceiveDirectory)) &&
                    (string.IsNullOrEmpty(partner.ReceiveDirectorySave) || Directory.Exists(partner.ReceiveDirectorySave)) &&
                    (string.IsNullOrEmpty(partner.ReceiveDirectoryError) || Directory.Exists(partner.ReceiveDirectoryError)
                    )
                    );
        }
    }
}
