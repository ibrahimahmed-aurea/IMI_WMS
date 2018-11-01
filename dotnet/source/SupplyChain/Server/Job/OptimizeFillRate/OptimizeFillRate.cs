using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Imi.SupplyChain.Server.Job.StandardJob;
using Imi.Framework.Job;
using Imi.Framework.Job.Data;
using LCAInterop;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Description;
using Imi.Framework.Services;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace Imi.SupplyChain.Server.Job.OptimizeFillRate
{
    public class OptimizeFillRate : OracleJob
    {
        #region WorkerProcess
        
        private class WorkerProcessClient : ClientBase<IWorkerProcessService>
        {
            public WorkerProcessClient(NetNamedPipeBinding binding, string workerProcessId)
                : base(new ServiceEndpoint(ContractDescription.GetContract(typeof(IWorkerProcessService)),
                    binding, new EndpointAddress(string.Format("net.pipe://localhost/{0}", workerProcessId))))
            {
            }

            public LCAWrapperResult ProcessGroup(IList<PBROW> rows, IList<AISLE_WAYPOINT> waypoints, LCASettings lcaSettings)
            {
                List<string> parameters = new List<string>();

                XmlSerializer pbrowsSeri = new XmlSerializer(typeof(List<PBROW>));
                XmlSerializer waypointsSeri = new XmlSerializer(typeof(List<AISLE_WAYPOINT>));
                XmlSerializer lcaSettingsSeri = new XmlSerializer(typeof(LCASettings));
                XmlSerializer resultSeri = new XmlSerializer(typeof(LCAWrapperResultWrapper));

                XmlWriterSettings xmlsettings = new XmlWriterSettings();
                xmlsettings.Indent = true;
                xmlsettings.NewLineOnAttributes = true;

                StringBuilder builder = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(builder, xmlsettings);

                pbrowsSeri.Serialize(writer, rows);
                parameters.Add(builder.ToString());

                builder.Clear();
                writer.Close();
                writer = XmlWriter.Create(builder, xmlsettings);

                waypointsSeri.Serialize(writer, waypoints);
                parameters.Add(builder.ToString());

                builder.Clear();
                writer.Close();
                writer = XmlWriter.Create(builder, xmlsettings);

                lcaSettingsSeri.Serialize(writer, lcaSettings);
                parameters.Add(builder.ToString());

                string result = Channel.Process(parameters);

                if (string.IsNullOrEmpty(result))
                {
                    throw new Exception("Error Processing Group, Internal Server Error, SupplyChain.Server.WorkerProcess.OptimizeFillRateWorkerProcessService.Process");
                }

                LCAWrapperResultWrapper resultWrapper = resultSeri.Deserialize(new StringReader(result)) as LCAWrapperResultWrapper;


                LCAWrapperResult realResult = new LCAWrapperResult(resultWrapper.Lines, resultWrapper.GetErrorDictionary());

                return realResult;
            }

            public void Terminate()
            {
                Channel.Terminate();
            }

            public bool IsAlive()
            {
                return Channel.IsAlive();
            }
        }


        private string _workerProcessId;
        private object _syncLock;
        private ReaderWriterLock _processingLock;
        private bool _isRecycling;
        private Process _workerProcess;
        private int _processCount;
        private List<WorkerProcessClient> _clientList;
        private int _recycleThresholdWP = 10;
        private int _processingTimeoutInMinutesWP = 30;

        private const string WorkerProccessName = "Imi.SupplyChain.Server.WorkerProcess";

        private void AbortClients()
        {
            lock (_clientList)
            {
                foreach (WorkerProcessClient client in _clientList)
                {
                    try
                    {
                        client.Abort();
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void CreateWorkerProcess()
        {
            try
            {
                DisposeWorkerProcess();

                _processCount = 0;

                Tracing.TraceEvent(TraceEventType.Verbose, 0, "Starting worker process...");

                _workerProcessId = Guid.NewGuid().ToString();

                using (EventWaitHandle startedEvent = new EventWaitHandle(false, EventResetMode.ManualReset, _workerProcessId))
                {
                    string args = string.Format("\"{0}\" {1}", _workerProcessId, _processingTimeoutInMinutesWP);
                    string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), string.Format("{0}.exe", WorkerProccessName));

                    ProcessStartInfo info = new ProcessStartInfo(path, args);
                    _workerProcess = Process.Start(info);

                    if (!startedEvent.WaitOne(new TimeSpan(0, 1, 0)))
                    {
                        throw new TimeoutException("Timed out waiting for worker process to start.");
                    }
                }

                Tracing.TraceEvent(TraceEventType.Verbose, 0, "Worker process started.");

            }
            catch (Exception ex)
            {
                DisposeWorkerProcess();
            }
        }

        private WorkerProcessClient CreateWorkerProcessClient()
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding();
            binding.TransferMode = TransferMode.StreamedRequest;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.Security.Mode = NetNamedPipeSecurityMode.None;
            binding.ReceiveTimeout = new TimeSpan(0, _processingTimeoutInMinutesWP, 0);
            binding.OpenTimeout = new TimeSpan(0, _processingTimeoutInMinutesWP, 0); 
            binding.SendTimeout = new TimeSpan(0, _processingTimeoutInMinutesWP, 0); 

            XmlDictionaryReaderQuotas readerQuotas = new XmlDictionaryReaderQuotas();
            readerQuotas.MaxDepth = 2147483647;
            readerQuotas.MaxStringContentLength = 2147483647;
            readerQuotas.MaxArrayLength = 2147483647;
            readerQuotas.MaxBytesPerRead = 2147483647;
            readerQuotas.MaxNameTableCharCount = 2147483647;
            binding.ReaderQuotas = readerQuotas;

            return new WorkerProcessClient(binding, _workerProcessId);
        }

        private void DisposeWorkerProcess()
        {
            if (_workerProcess != null)
            {
                try
                {
                    Tracing.TraceEvent(TraceEventType.Verbose, 0, "Disposing worker process...");

                    WorkerProcessClient client = CreateWorkerProcessClient();

                    try
                    {
                        client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 0, 5);
                        client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, 5);
                        client.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 0, 5);
                        client.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 0, 5);

                        client.Open();

                        client.Terminate();

                        _workerProcess.WaitForExit(5000);
                    }
                    finally
                    {
                        try
                        {
                            client.Close();
                        }
                        catch (TimeoutException)
                        {
                            client.Abort();
                        }
                        catch (CommunicationException)
                        {
                            client.Abort();
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    try
                    {
                        if (!_workerProcess.HasExited)
                        {
                            _workerProcess.Kill();
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        _workerProcess.Dispose();
                    }
                }
            }
        }

        private bool IsWokerProccesAlive()
        {
            bool isAlive = false;

            foreach (Process process in Process.GetProcessesByName(WorkerProccessName))
            {
                isAlive = true;
                process.Dispose();
            }

            if (isAlive)
            {
                WorkerProcessClient client = CreateWorkerProcessClient();

                try
                {
                    client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 0, 2);
                    client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, 2);
                    client.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 0, 2);
                    client.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 0, 2);

                    client.Open();

                    return client.IsAlive();
                }
                catch
                {
                    isAlive = false;
                }
            }

            return isAlive;
        }

        private LCAWrapperResult SendGroupToWorkerProcess(IList<PBROW> rows, IList<AISLE_WAYPOINT> waypoints, LCASettings lcaSettings)
        {
            LCAWrapperResult lcaResult = new LCAWrapperResult(new List<LCAWrapperResultLine>(), new Dictionary<int,List<string>>());

            _processingLock.AcquireReaderLock(-1);

                bool recycle = false;
                
                lock (_syncLock)
                {
                    if (!_isRecycling)
                    {
                        if (!IsWokerProccesAlive())
                        {
                            AbortClients();
                            recycle = true;
                        }
                        else if (_processCount == _recycleThresholdWP + 1)
                        {
                            recycle = true;
                        }
                        else
                        {
                            _processCount++;
                        }

                        _isRecycling = recycle;
                    }
                }
                
                if (recycle)
                {
                    LockCookie cookie = _processingLock.UpgradeToWriterLock(-1);
                                        
                    _isRecycling = false;

                    try
                    {
                        Tracing.TraceEvent(TraceEventType.Verbose, 0, "Recycling worker process...");

                        CreateWorkerProcess();

                        Tracing.TraceEvent(TraceEventType.Verbose, 0, "Done recycling worker process.");
                    }
                    finally
                    {
                        _processingLock.DowngradeFromWriterLock(ref cookie);
                    }
                }

           
                WorkerProcessClient client = CreateWorkerProcessClient();

                try
                {
                    lock (_clientList)
                    {
                        _clientList.Add(client);
                    }

                    client.Open();

                    try
                    {
                        lcaResult = client.ProcessGroup(rows, waypoints, lcaSettings);

                    }
                    catch (TimeoutException)
                    {
                        Tracing.TraceEvent(TraceEventType.Warning, 0, "Timed out while processing group");
                        throw;
                    }
                }
                finally
                {
                    lock (_clientList)
                    {
                        _clientList.Remove(client);
                    }

                    try
                    {
                        client.Close();
                    }
                    catch (TimeoutException)
                    {
                        client.Abort();
                    }
                    catch (CommunicationException)
                    {
                        client.Abort();
                    }
                }

                return lcaResult;
        }

        #endregion

        private Pickordercarrierbuild _pickOrderCarrierBuild;
        private Dictionary<string, string> _alarmTextDictionary;
        
        public OptimizeFillRate(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
            _alarmTextDictionary = new Dictionary<string, string>();

            _syncLock = new object();
            _processingLock = new ReaderWriterLock();
            _clientList = new List<WorkerProcessClient>();
        }

        private void LogGroupError(string PBROWGRP_ID_O, string errorMessage)
        {
            int count = 0;
            
            while (count < errorMessage.Length)
            {
                _pickOrderCarrierBuild.AddErrorMessage(PBROWGRP_ID_O, errorMessage.Substring(count, Math.Min(255, errorMessage.Length - count)));
                count += 255;
            }
        }

        Dictionary<string, List<string>> MapErrors(string PBROWGRPID_I, Dictionary<int, List<string>> errors)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var entry in errors)
            {
                string ALMID_I = string.Format("OFR{0}", entry.Key.ToString("0000"));
                string ALMTXT_O = null;
                Regex regex = new Regex("\\\"[^\\\"\\r]*\\\"");
                                
                ALMTXT_O = GetAlarmText(PBROWGRPID_I, ALMID_I);
                
                foreach (string message in entry.Value)
                {
                    if (!result.ContainsKey(ALMID_I))
                    {
                        result.Add(ALMID_I, new List<string>());
                    }

                    if (!string.IsNullOrEmpty(ALMTXT_O))
                    {
                        var tokens = new List<string>();

                        foreach (Match m in regex.Matches(message))
                        {
                            tokens.Add(m.Value.Replace("\"", ""));
                        }

                        try
                        {
                            result[ALMID_I].Add(string.Format(ALMTXT_O, tokens.ToArray()));
                            continue;
                        }
                        catch (FormatException ex)
                        {
                            string parameters = string.Join(",", tokens);
                            Tracing.TraceEvent(TraceEventType.Warning, 0,
                                       string.Format("Unable to format message for ALMID: \"{0}\". Parameters={1}\n{2}.", ALMID_I, parameters, ex.ToString()));
                        }
                    }
                    else
                    {
                        Tracing.TraceEvent(TraceEventType.Warning, 0,
                                        string.Format("No text found for ALMID: \"{0}\".", ALMID_I));
                    }
                    
                    result[ALMID_I].Add(message);
                }
            }

            return result;
        }

        private string GetAlarmText(string PBROWGRPID_I, string ALMID_I)
        {
            if (!_alarmTextDictionary.ContainsKey(ALMID_I))
            { 
                string ALMTXT_O = "";
                _pickOrderCarrierBuild.GetAlarmText(PBROWGRPID_I, ALMID_I, ref ALMTXT_O);
                _alarmTextDictionary.Add(ALMID_I, ALMTXT_O);
            }

            return _alarmTextDictionary[ALMID_I];
            
        }

        private void ConnectPickOrderLines(string PBROWGRP_ID, string CARTYPID, List<LCAWrapperResultLine> lines)
        {
            var PBROWID_I = new string[lines.Count];
            var PBROWGRP_ID_I = new string[lines.Count];
            var ROWSPLIT_ID_I = new string[lines.Count];
            var ORDQTY_I = new double?[lines.Count];
            var PBCARID_VIRTUAL_I = new string[lines.Count];
            var CARTYPID_I = new string[lines.Count];

            for (int i = 0; i < lines.Count; i++)
            {
                PBROWID_I[i] = lines[i].PBRowId;
                PBROWGRP_ID_I[i] = PBROWGRP_ID;
                ROWSPLIT_ID_I[i] = lines[i].PBRowSplitId;
                ORDQTY_I[i] = lines[i].OrdQty;
                PBCARID_VIRTUAL_I[i] = lines[i].PBCarIdVirtual;
                CARTYPID_I[i] = CARTYPID;
            }

            _pickOrderCarrierBuild.ConnectGroupRowToCarrier(PBROWID_I, PBROWGRP_ID_I, ROWSPLIT_ID_I, ORDQTY_I, PBCARID_VIRTUAL_I, CARTYPID_I);
        }
                
        private static IList<AISLE_WAYPOINT> ReadWaypoints(IDataReader reader)
        {
            var waypoints = new List<AISLE_WAYPOINT>();

            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        AISLE_WAYPOINT waypoint = new AISLE_WAYPOINT();

                        if (reader["WSID"] == DBNull.Value)
                            waypoint.WSID = null;
                        else
                            waypoint.WSID = reader["WSID"] as string;

                        if (reader["AISLE"] == DBNull.Value)
                            waypoint.AISLE = null;
                        else
                            waypoint.AISLE = reader["AISLE"] as string;

                        if (reader["WAYPOINT_ID"] == DBNull.Value)
                            waypoint.WAYPOINT_ID = null;
                        else
                            waypoint.WAYPOINT_ID = reader["WAYPOINT_ID"] as string;

                        if (reader["WAYPOINT_XCORD"] == DBNull.Value)
                            waypoint.WAYPOINT_XCORD = 0;
                        else
                            waypoint.WAYPOINT_XCORD = Convert.ToDouble(reader["WAYPOINT_XCORD"]);

                        if (reader["WAYPOINT_YCORD"] == DBNull.Value)
                            waypoint.WAYPOINT_YCORD = 0;
                        else
                            waypoint.WAYPOINT_YCORD = Convert.ToDouble(reader["WAYPOINT_YCORD"]);

                        if (reader["WS_XCORD"] == DBNull.Value)
                            waypoint.WS_XCORD = 0;
                        else
                            waypoint.WS_XCORD = Convert.ToDouble(reader["WS_XCORD"]);

                        if (reader["WS_YCORD"] == DBNull.Value)
                            waypoint.WS_YCORD = 0;
                        else
                            waypoint.WS_YCORD = Convert.ToDouble(reader["WS_YCORD"]);

                        if (reader["AISLE_FROM_XCORD"] == DBNull.Value)
                            waypoint.AISLE_FROM_XCORD = 0;
                        else
                            waypoint.AISLE_FROM_XCORD = Convert.ToDouble(reader["AISLE_FROM_XCORD"]);

                        if (reader["AISLE_FROM_YCORD"] == DBNull.Value)
                            waypoint.AISLE_FROM_YCORD = 0;
                        else
                            waypoint.AISLE_FROM_YCORD = Convert.ToDouble(reader["AISLE_FROM_YCORD"]);

                        if (reader["AISLE_TO_XCORD"] == DBNull.Value)
                            waypoint.AISLE_TO_XCORD = 0;
                        else
                            waypoint.AISLE_TO_XCORD = Convert.ToDouble(reader["AISLE_TO_XCORD"]);

                        if (reader["AISLE_TO_YCORD"] == DBNull.Value)
                            waypoint.AISLE_TO_YCORD = 0;
                        else
                            waypoint.AISLE_TO_YCORD = Convert.ToDouble(reader["AISLE_TO_YCORD"]);

                        if (reader["DIRECTION_PICK"] == DBNull.Value)
                            waypoint.DIRECTION_PICK = null;
                        else
                            waypoint.DIRECTION_PICK = reader["DIRECTION_PICK"] as string;

                        waypoints.Add(waypoint);
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return waypoints;
        }

        private static IList<PBROW> ReadPickOrderLines(IDataReader reader)
        {
            var rows = new List<PBROW>();

            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        PBROW row = new PBROW();

                        if (reader["PBROWID"] == DBNull.Value)
                            row.PBROWID = null;
                        else
                            row.PBROWID = reader["PBROWID"] as string;

                        if (reader["ARTID"] == DBNull.Value)
                            row.ARTID = null;
                        else
                            row.ARTID = reader["ARTID"] as string;

                        if (reader["COMPANY_ID"] == DBNull.Value)
                            row.COMPANY_ID = null;
                        else
                            row.COMPANY_ID = reader["COMPANY_ID"] as string;
                                                
                        if (reader["PICKSEQ"] == DBNull.Value)
                            row.PICKSEQ = 0;
                        else
                            row.PICKSEQ = Convert.ToInt32(reader["PICKSEQ"]);

                        if (reader["ORDQTY"] == DBNull.Value)
                            row.ORDQTY = 0;
                        else
                            row.ORDQTY = Convert.ToDouble(reader["ORDQTY"]);

                        if (reader["VOLUME"] == DBNull.Value)
                            row.VOLUME = 0;
                        else
                            row.VOLUME = Convert.ToDouble(reader["VOLUME"]);

                        if (reader["WEIGHT"] == DBNull.Value)
                            row.WEIGHT = 0;
                        else
                            row.WEIGHT = Convert.ToDouble(reader["WEIGHT"]);

                        if (reader["WSID"] == DBNull.Value)
                            row.WSID = null;
                        else
                            row.WSID = reader["WSID"] as string;

                        if (reader["AISLE"] == DBNull.Value)
                            row.AISLE = null;
                        else
                            row.AISLE = reader["AISLE"] as string;

                        if (reader["ARTGROUP"] == DBNull.Value)
                            row.ARTGROUP = null;
                        else
                            row.ARTGROUP = reader["ARTGROUP"] as string;

                        if (reader["CATGROUP"] == DBNull.Value)
                            row.CATGROUP = null;
                        else
                            row.CATGROUP = reader["CATGROUP"] as string;
                        
                        if (reader["XCORD"] == DBNull.Value)
                            row.XCORD = 0;
                        else
                            row.XCORD = Convert.ToDouble(reader["XCORD"]);

                        if (reader["YCORD"] == DBNull.Value)
                            row.YCORD = 0;
                        else
                            row.YCORD = Convert.ToDouble(reader["YCORD"]);

                        rows.Add(row);
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return rows;
        }
                
        protected override void CreateProcedure(IDbConnectionProvider connectionProvider)
        {
            _pickOrderCarrierBuild = new Pickordercarrierbuild(connectionProvider);
        }

        /// <summary>
        /// ExecuteProcedure is the main activity method, this is the code that is run
        /// when the Job is activated/signalled.
        /// </summary>
        protected override void ExecuteProcedure(Imi.Framework.Job.JobArgumentCollection args)
        {
            if (args.ContainsKey("recycleThresholdWP"))
            {
                _recycleThresholdWP = Convert.ToInt32(args["recycleThresholdWP"]);
            }

            if (args.ContainsKey("processingTimeoutInMinutesWP"))
            {
                _processingTimeoutInMinutesWP = Convert.ToInt32(args["processingTimeoutInMinutesWP"]);
            }

            string PBROWGRP_ID_O = "";
            string PZID_O = "";
            string WHID_O = "";
            string ALLOW_PBROWSPLIT_O = "0";
            string CARTYPID_O = "";
            string SHIPWSID_O = "";
            double? SHIPXCORD_O = 0;
            double? SHIPYCORD_O = 0;
            double? MAXLDVOL_O = 0;
            double? MAXLDWGT_O = 0;
            double? OFR_DISTANCE_FACTOR_O = 0;
            int? MAXPBROWCAR_O = 0;
            int? OFR_MAX_ITER_FILL_O = 0;
            int? OFR_MAX_TME_FILL_O = 0;
            int? OFR_MAX_ITER_STORE_O = 0;
            int? OFR_MAX_TME_STORE_O = 0;
            int? OFR_MAX_ITER_DIST_O = 0;
            int? OFR_MAX_TME_DIST_O = 0;

            IList<PBROW> rows = null;
            IList<AISLE_WAYPOINT> waypoints = null;

            try
            {
                StartTransaction();

                IDataReader rowReader;
                IDataReader waypointReader;

                _pickOrderCarrierBuild.GetGroup(10,
                                        ref PBROWGRP_ID_O,
                                        ref OFR_DISTANCE_FACTOR_O,
                                        ref OFR_MAX_ITER_FILL_O,
                                        ref OFR_MAX_TME_FILL_O,
                                        ref OFR_MAX_ITER_STORE_O,
                                        ref OFR_MAX_TME_STORE_O,
                                        ref OFR_MAX_ITER_DIST_O,
                                        ref OFR_MAX_TME_DIST_O,
                                        ref PZID_O,
                                        ref WHID_O,
                                        ref ALLOW_PBROWSPLIT_O,
                                        ref MAXPBROWCAR_O,
                                        ref CARTYPID_O,
                                        ref MAXLDVOL_O,
                                        ref MAXLDWGT_O,
                                        ref SHIPWSID_O,
                                        ref SHIPXCORD_O,
                                        ref SHIPYCORD_O,
                                        out waypointReader,
                                        out rowReader);

                if (string.IsNullOrEmpty(PBROWGRP_ID_O))
                {
                    Commit();
                    return;
                }

                Tracing.TraceEvent(TraceEventType.Information, 0,
                                        string.Format("Found group {0}", PBROWGRP_ID_O));

                rows = ReadPickOrderLines(rowReader);
                waypoints = ReadWaypoints(waypointReader);

                Commit();

                if (rows.Count < 1)
                {
                    Tracing.TraceEvent(TraceEventType.Warning, 0, "No rows to process, aborting.");

                    return;
                }
                                
                LCASettings lcaSettings = new LCASettings();

                lcaSettings.DistanceFactor = OFR_DISTANCE_FACTOR_O.Value;
                lcaSettings.DoBeautyPhase = OFR_MAX_ITER_STORE_O > 0 && OFR_MAX_TME_STORE_O > 0;
                lcaSettings.DoDistPhase = OFR_MAX_ITER_DIST_O > 0 && OFR_MAX_TME_DIST_O > 0;
                lcaSettings.DoLCPhase = OFR_MAX_ITER_FILL_O > 0 && OFR_MAX_TME_FILL_O > 0;
                lcaSettings.GroupId = PBROWGRP_ID_O;
                lcaSettings.MaxmSecBeauty = OFR_MAX_TME_STORE_O.Value * 1000;
                lcaSettings.MaxmSecDistance = OFR_MAX_TME_DIST_O.Value * 1000;
                lcaSettings.MaxmSecLC = OFR_MAX_TME_FILL_O.Value * 1000;
                lcaSettings.NumberOfIterationsBeauty = OFR_MAX_ITER_STORE_O.Value;
                lcaSettings.NumberOfIterationsDistance = OFR_MAX_ITER_DIST_O.Value;
                lcaSettings.NumberOfIterationsLC = OFR_MAX_ITER_FILL_O.Value;
                lcaSettings.PZId = PZID_O;
                lcaSettings.WHId = WHID_O;
                lcaSettings.StrekArea = SHIPWSID_O;
                lcaSettings.StrekXCoord = SHIPXCORD_O.GetValueOrDefault();
                lcaSettings.StrekYCoord = SHIPYCORD_O.GetValueOrDefault();
                lcaSettings.AllowPBRowSplit = ALLOW_PBROWSPLIT_O == "1";
                lcaSettings.MaxLDWgt = MAXLDWGT_O.GetValueOrDefault(double.MaxValue);
                lcaSettings.MaxLDVol = MAXLDVOL_O.GetValueOrDefault(double.MaxValue);

                lcaSettings.MaxPBRowCar = MAXPBROWCAR_O.GetValueOrDefault(int.MaxValue);
                lcaSettings.OnlyAptean = false;

                Tracing.TraceEvent(TraceEventType.Information, 0,
                       string.Format("Parameters:{0}{1}", Environment.NewLine, lcaSettings.ToString()));
                
                Tracing.TraceEvent(TraceEventType.Information, 0,
                                       string.Format("Processing group {0} ({1} lines, {2} waypoints)...", PBROWGRP_ID_O, rows.Count, waypoints.Count));


                LCAWrapperResult result = null;
                int retry = 1;

                while (retry < 3)
                {
                    // Process Group in Worker Process.
                    try
                    {
                        result = SendGroupToWorkerProcess(rows, waypoints, lcaSettings);  //ProcessGroup(rows, waypoints, lcaSettings);
                        retry = 3;
                    }
                    catch (TimeoutException)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        //Make three tries at processing data in the algorithm if any anknown exception occurs.
                        lock (_syncLock)
                        {
                            AbortClients();
                            retry++;
                        }

                        Thread.Sleep(3000);
                    }
                }
                
                StringBuilder sb = new StringBuilder();

                foreach (var entry in MapErrors(PBROWGRP_ID_O, result.Errors)) //These are actually warnings
                {
                    foreach (string errorMessage in entry.Value)
                    {
                        string formattedMessage = string.Format("{0} [{1}]", errorMessage, entry.Key);
                        Tracing.TraceEvent(TraceEventType.Warning, 0, formattedMessage);
                        sb.AppendLine(string.Format("{0} Warning: {1}", this.Name, formattedMessage));
                    }
                }
                                
                if (sb.Length > 0)
                {
                    LogGroupError(PBROWGRP_ID_O, sb.ToString());
                }

                Tracing.TraceEvent(TraceEventType.Information, 0,
                                       string.Format("Connecting {0} result lines...", result.Lines.Count));

                StartTransaction();

                ConnectPickOrderLines(PBROWGRP_ID_O, CARTYPID_O, result.Lines);

                _pickOrderCarrierBuild.FinishGroup(PBROWGRP_ID_O);

                Tracing.TraceEvent(TraceEventType.Information, 0,
                                       string.Format("Done processing.", PBROWGRP_ID_O));
                Commit();

            }
            catch (Exception ex)
            {
                try
                {
                    Rollback();
                }
                finally
                {
                    try
                    {
                        LogGroupError(PBROWGRP_ID_O, string.Format("Error - {0}", ex.ToString()));
                    }
                    finally
                    {
                        _pickOrderCarrierBuild.CancelGroup(PBROWGRP_ID_O);
                    }
                }

                throw;
            }
        }
       
        protected override void CancelProcedure()
        {
            if (_pickOrderCarrierBuild != null)
            {
                _pickOrderCarrierBuild.Cancel();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                DisposeWorkerProcess();
            }
        }
    }
}
