using System;
using System.IO;
using System.Threading;
using System.Collections;
using Imi.SupplyChain.Server.Job.StandardJob;
using Imi.SupplyChain.Server.Job.Gateway.OraclePackage;
using Imi.Framework.Job;
using Imi.Framework.Job.Data;
using System.Diagnostics;
using System.Text;

namespace Imi.SupplyChain.Server.Job.Gateway
{
    public class GatewaySender : OracleJob
    {
        private DBGateway dbGateway;
        private string tempDirectory;
        private string logFileName;
        private int logLevel = 2;
        private bool logEnabled = false;

        public GatewaySender(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
            // get the temp directory as defined in Windows environment
            tempDirectory = Path.GetTempPath();
        }

        protected override void CreateProcedure(IDbConnectionProvider connection)
        {
            dbGateway = new DBGateway(connection);
        }


        protected override void ExecuteProcedure(Imi.Framework.Job.JobArgumentCollection args)
        {
            if (args.ContainsKey("Log"))
                logEnabled = args["Log"].ToString().ToLower().Equals("true");

            if (args.ContainsKey("LogFile"))
                logFileName = args["LogFile"];

            if (args.ContainsKey("LogLevel"))
                logLevel = Convert.ToInt16(args["LogLevel"]);

            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Start");
            long start = DateTime.Now.Ticks;

            // Process any available EDIOUT messages

            int cc;

            do
            {
                cc = WriteMessageFile();
            } while (cc == 0);

            long stop = DateTime.Now.Ticks;
            TimeSpan ts = new TimeSpan(stop - start);

            Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format("Stop (used {0}s)", ts.TotalSeconds.ToString("0.00")));

        }

        protected override void CancelProcedure()
        {
            if (dbGateway != null)
            {
                dbGateway.Cancel();
            }
        }

        /*
         * MakeFileName
         * create a file name with timestamp, EDIMSGID and EDIOUTID
         * 
         */

        private static string MakeFileName(double ediOutId, string ediMessageId)
        {
            // file name is "owsnd<yyyymmddhhmmss><edimsgid>.<12345>" 
            // where <12345> is MOD(EDIOUTID / 100000) formatted with leading zeros
            return String.Format("owsnd{0:yyyyMMddHHmmss}{1}.{2:00000}", DateTime.Now, ediMessageId, (ediOutId % 100000));
        }

        /*
         * WriteMessageFile
         * obtain message data through procedure calls 
         * create filename and write file to temporary directory
         * Once file is complete, set "Hidden" attribute,
         * move file to EDIOUT directory, unset "Hidden" attribute
         * 
         */

        private int WriteMessageFile()
        {
            try
            {
                StartTransaction();

                if (logEnabled)
                {
                    dbGateway.EnableServerLog(null, logFileName, logLevel);
                }

                EdiMessage message = dbGateway.ReadNextMessage();

                // check if we got data

                if (message == null)
                {
                    return -1; // no more messages
                }
                else
                {
                    // check if SNDDIR exists

                    if (Directory.Exists(message.SendDirectory))
                    {
                        try
                        {
                            string fileName = MakeFileName(message.EdiOutId, message.EdiMessageId);

                            Tracing.TraceEvent(TraceEventType.Verbose, 0, string.Format("Temp directory {0} will be used", tempDirectory));
                            Tracing.TraceEvent(TraceEventType.Verbose, 0, string.Format("Send directory {0} will be used", message.SendDirectory));
                            Tracing.TraceEvent(TraceEventType.Verbose, 0, string.Format("File name {0} will be used", fileName));

                            // start writing the file in the temp directory

                            string workFileName = Path.Combine(tempDirectory, fileName);
                            string sendFileName = Path.Combine(message.SendDirectory, fileName);

                            // make sure the file does not exist already
                            if (File.Exists(workFileName))
                            {
                                File.Delete(workFileName);
                            }

                            Encoding encoding = Encoding.Default;

                            if (args.ContainsKey("SendEncoding"))
                            {
                                string sendEncoding = args["SendEncoding"];

                                if (!string.IsNullOrEmpty(sendEncoding))
                                    encoding = Encoding.GetEncoding(sendEncoding);
                            }

                            using (StreamWriter streamWriter = new StreamWriter(workFileName, false, encoding))
                            {
                                streamWriter.WriteLine(message.Unb);
                                streamWriter.WriteLine(message.Unh);

                                double lastRowNumber = 0;

                                do
                                {
                                    EdiSegment segment = dbGateway.SegmentRead(message.EdiOutId, lastRowNumber);

                                    if (segment != null)
                                    {
                                        streamWriter.WriteLine(segment.EdiData);
                                        lastRowNumber = segment.NextRowNumber;
                                    }
                                    else
                                    {
                                        break;
                                    }

                                } while (true);

                                streamWriter.WriteLine(message.Unt);
                                streamWriter.WriteLine(message.Unz);
                            }

                            // we are done with writing the file, the streamwriter is closed
                            // make the file hidden, move it and then un-hide it

                            File.SetAttributes(workFileName, File.GetAttributes(workFileName) | FileAttributes.Hidden);

                            File.Move(workFileName, sendFileName);

                            File.SetAttributes(sendFileName, FileAttributes.Archive);

                            Commit();
                        }
                        catch (Exception e)
                        {
                            Tracing.TraceEvent(TraceEventType.Error, 0, string.Format("Error while processing message {0}\n{1}", message.EdiOutId,e.Message));
                            dbGateway.InvalidMessage(message.EdiOutId);
                            Commit();
                        }
                    }
                    else
                    {
                        Tracing.TraceEvent(TraceEventType.Error, 0, string.Format("Directory {0} does not exist", message.SendDirectory));
                        dbGateway.InvalidMessage(message.EdiOutId);
                        Commit();
                    }
                }

                return 0;
            }
            finally
            {
                // Anything not commited is rolled back
                Rollback();

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
    }
}

