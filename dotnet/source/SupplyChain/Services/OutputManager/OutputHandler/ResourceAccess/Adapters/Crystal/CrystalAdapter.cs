using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.OM.OutputHandler.ResourceAccess;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using CrystalDecisions.Shared;
using System.Runtime.InteropServices;
using System.IO;
using CrystalDecisions.ReportAppServer.Controllers;
using CrystalDecisions.ReportAppServer.ClientDoc;
using System.Threading;
using System.Collections.Concurrent;

namespace Imi.SupplyChain.OM.OutputHandler.Adapters
{
    public class CrystalAdapter : IAdapter
    {
        private static string CONFPARAM_OUTPUTENABLE = "OutputEnable";
        private static string CONFPARAM_DBNAME = "DatabaseName";
        private static string CONFPARAM_DBUSER = "DatabaseUser";
        private static string CONFPARAM_DBPASS = "DatabasePassword";

        private static object _exportLockObj = new object();

        private Dictionary<string, string> _configurationParameters = null;

        private Dictionary<string, string> _reportFilesDic = new Dictionary<string, string>();

        private string _uniqueOutputManagerIdentity;

        private int _NumberOfConcurrentJobs = 0;
        private object _NumOfConcurrentJobsLockObj = new object();

        private class PrintFailureException : Exception
        {
            public PrintFailureException(){}
            public PrintFailureException(string message) : base(message){}
            public PrintFailureException(string message, Exception inner) : base(message, inner){}
        }

        //private Dictionary<string, ReportCache> _reportCache = new Dictionary<string, ReportCache>();
        //private object _crystalLock = new object();
        //private class ReportCache
        //{
        //    public enum CacheState
        //    {
        //        NotStarted,
        //        Running,
        //        CleanCache,
        //        Clean,
        //        Disposed
        //    }

        //    public ReportCache()
        //    {
        //        State = CacheState.NotStarted;
        //        Cache = new ConcurrentStack<ReportDocument>();
        //    }

        //    public CacheState State { get; set; }

        //    public ConcurrentStack<ReportDocument> Cache { get; set; }
        //}

        public string AdapterIdentity
        {
            get { return "crystal"; }
        }

        public void InitializeAdapter(Dictionary<string, string> configurationParameters, Dictionary<string, byte[]> reportFilesForAdapter, string uniqueOutputManagerIdentity, out string errorText)
        {
            errorText = string.Empty;

            if (!Crystal.RuntimePolicyHelper.LegacyV2RuntimeEnabledSuccessfully)
            {
                throw new Exception("Unable to activate Legacy V2 Runtime.");
            }

            _uniqueOutputManagerIdentity = uniqueOutputManagerIdentity;
            _configurationParameters = configurationParameters;

            //lock (_reportCache)
            //{
            PrepareReports(reportFilesForAdapter, new List<string>(), out errorText);
            //}
        }

        public void UpdateConfiguration(Dictionary<string, string> configurationParameters)
        {
            _configurationParameters = configurationParameters;
        }

        public void UpdateReportFiles(Dictionary<string, byte[]> reportFilesForAdapter, List<string> updatedReports)
        {
            string errorText = string.Empty;
            //lock (_reportCache)
            //{
            PrepareReports(reportFilesForAdapter, updatedReports, out errorText);
            //}

            if (!string.IsNullOrEmpty(errorText))
            {
                throw new Exception(errorText);
            }
        }


        public Dictionary<string, object> Execute(OutputDocument document, Dictionary<string, object> namedPassThroughParameters)
        {
            Dictionary<string, object> results = new Dictionary<string, object>();
            bool goOn = true;
            int retry = 0;
            string errorText = string.Empty;

            while (goOn) //Loop to be able to make retries of printouts in case of exceptions.
            {
                //There can not be more than 75 reports open at the same time.
                bool exitWaitLoop = false;

                while (!exitWaitLoop)
                {
                    lock (_NumOfConcurrentJobsLockObj)
                    {
                        if (_NumberOfConcurrentJobs < 75)
                        {
                            _NumberOfConcurrentJobs++;
                            exitWaitLoop = true;
                        }
                    }

                    if (!exitWaitLoop) { Thread.Sleep(25); }
                }

                try
                {
                    errorText = string.Empty;
                    using (ReportDocument report = GetReportDocument(document.ReportID, document.ReportFile, out errorText))
                    {
                        if (report == null || !string.IsNullOrEmpty(errorText))
                        {
                            throw new Exception("Error loading report\r\n\r\n" + errorText);
                        }

                        DataSet dataSet = null;

                        try
                        {

                            errorText = string.Empty;
                            if (document.MetaParameters[StdMetaParamNames.DocumentIDKey] == "GenericReport")
                            {
                                ConnectionInfo info = new ConnectionInfo();
                                info.ServerName = _configurationParameters[CONFPARAM_DBNAME];
                                info.UserID = _configurationParameters[CONFPARAM_DBUSER];
                                info.Password = _configurationParameters[CONFPARAM_DBPASS];

                                SetReportConnectionInfo(info, report);

                                ApplyReportParameters(document.Parameters, report, out errorText);
                            }
                            else
                            {
                                dataSet = new DataSet();
                                document.Data.Seek(0, SeekOrigin.Begin);
                                dataSet.ReadXml(document.Data);

                                //DateTime before = DateTime.Now;
                                report.SetDataSource(dataSet);
                                //DateTime after = DateTime.Now;

                                //System.Diagnostics.Debug.WriteLine("SetDataSource: " + (after - before).ToString());
                            }

                            if (!string.IsNullOrEmpty(errorText))
                            {
                                throw new Exception(errorText);
                            }

                            if (Convert.ToBoolean(_configurationParameters[CONFPARAM_OUTPUTENABLE]))
                            {
                                PrintReportOptions clientReportOptions = new PrintReportOptions();
                                clientReportOptions.PrinterName = document.PrinterDeviceName;
                                clientReportOptions.NumberOfCopies = int.Parse(document.MetaParameters[StdMetaParamNames.NumberOfCopiesKey]);
                                clientReportOptions.Collated = true;
                                clientReportOptions.JobTitle = document.OutputJobId + "_" + document.OutputJobSequence.ToString();

                                ISCDReportClientDocument clientDoc = report.ReportClientDocument;

                                try
                                {
                                    clientDoc.PrintOutputController.PrintReport(clientReportOptions);
                                }
                                catch (Exception ex)
                                {
                                    throw new PrintFailureException("Print out failed", ex);
                                }
                            }

                            try
                            {
                                byte[] exportData = null;
                                Stream exportStream = null;

                                lock (_exportLockObj) //Lock this part to avoid unhandled exceptions
                                {
                                    exportStream = report.ExportToStream(ExportFormatType.PortableDocFormat);
                                }

                                exportData = new byte[exportStream.Length];
                                exportStream.Read(exportData, 0, Convert.ToInt32(exportStream.Length));
                                exportStream.Close();

                                if (results.ContainsKey(StdResultParamNames.BinaryResultKey))
                                {
                                    results.Remove(StdResultParamNames.BinaryResultKey);
                                }

                                results.Add(StdResultParamNames.BinaryResultKey, exportData);
                            }
                            catch{} //Ignore exceptions from PDF export. 

                        }
                        finally
                        {
                            if (dataSet != null)
                            {
                                dataSet.Dispose();
                            }

                            try { report.Close(); }
                            catch { }
                            finally { report.Dispose(); }

                            lock (_NumOfConcurrentJobsLockObj)
                            {
                                _NumberOfConcurrentJobs--;
                            }

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            GC.Collect();
                        }
                    }
                    goOn = false;
                }
                catch (PrintFailureException ex)
                {
                    //Make three tries to print. 
                    if (retry >= 3)
                    {
                        string txt = "Error printing Crystal Report\r\n\r\n" + ex.Message;
                        if (ex.InnerException != null)
                        {
                            txt += "\r\n" + ex.InnerException.Message;
                            if (ex.InnerException.InnerException != null)
                            {
                                txt += "\r\n" + ex.InnerException.InnerException.Message;
                            }
                        }
                        throw new Exception(txt);
                    }
                    else
                    {
                        retry++;
                        goOn = true;
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    goOn = false;
                    string txt = "Error printing Crystal Report\r\n\r\n" + ex.Message;
                    if (ex.InnerException != null)
                    {
                        txt += "\r\n" + ex.InnerException.Message;
                        if (ex.InnerException.InnerException != null)
                        {
                            txt += "\r\n" + ex.InnerException.InnerException.Message;
                        }
                    }
                    throw new Exception(txt);
                }
            }

            return results;
        }

        private void SetReportConnectionInfo(ConnectionInfo connectionInfo, ReportDocument reportDocument)
        {
            foreach (Table table in reportDocument.Database.Tables)
            {
                table.LogOnInfo.ConnectionInfo.ServerName = connectionInfo.ServerName;
                table.LogOnInfo.ConnectionInfo.UserID = connectionInfo.UserID;
                table.LogOnInfo.ConnectionInfo.Password = connectionInfo.Password;
                table.ApplyLogOnInfo(table.LogOnInfo);
            }
        }

        private void ApplyReportParameters(Dictionary<string, string> parameters, ReportDocument report, out string errorText)
        {
            errorText = string.Empty;

            foreach (KeyValuePair<string, string> entry in parameters)
            {
                ParameterFieldDefinition reportParameter = null;

                try
                {
                    reportParameter = report.DataDefinition.ParameterFields[entry.Key];
                }
                catch (COMException ex)
                {
                    if (ex.ErrorCode == -2147352565)
                    {
                        errorText += string.Format("Parameter: \"{0}\" doesn't exist in the report file: \"{1}\"", entry.Key, report.FileName);

                        continue;
                    }
                    else
                    {
                        throw;
                    }
                }

                if (reportParameter != null)
                {
                    reportParameter.CurrentValues.Clear();

                    ParameterDiscreteValue parameterValue = new ParameterDiscreteValue();
                    parameterValue.Value = entry.Value;
                    reportParameter.CurrentValues.Add(parameterValue);
                    reportParameter.ApplyCurrentValues(reportParameter.CurrentValues);
                }
            }
        }

        private void PrepareReports(Dictionary<string, byte[]> reportFilesForAdapter, List<string> updatedReports, out string errorText)
        {
            errorText = string.Empty;

            if (updatedReports.Count == 0)
            {
                updatedReports = reportFilesForAdapter.Keys.ToList();

                //Clean up of old report files
                foreach (string oldReport in Directory.GetFiles(Path.GetTempPath(), "Imi_" + _uniqueOutputManagerIdentity + "_*.rpt"))
                {
                    File.Delete(oldReport);
                }


            }


            foreach (string reportID in updatedReports)
            {
                if (reportFilesForAdapter.ContainsKey(reportID))
                {
                    if (reportFilesForAdapter[reportID] != null)
                    {
                        try
                        {
                            string tempFileName = Path.Combine(Path.GetTempPath(), "Imi_" + _uniqueOutputManagerIdentity + "_" + Guid.NewGuid().ToString() + ".rpt");

                            byte[] content = reportFilesForAdapter[reportID];

                            File.WriteAllBytes(tempFileName, content);

                            lock (_reportFilesDic)
                            {
                                if (!_reportFilesDic.ContainsKey(reportID))
                                {
                                    _reportFilesDic.Add(reportID, tempFileName);
                                }
                                else
                                {
                                    _reportFilesDic[reportID] = tempFileName;
                                }
                            }

                            //if (_reportCache.ContainsKey(reportID))
                            //{
                            //    if (_reportCache[reportID].State == ReportCache.CacheState.Running)
                            //    {
                            //        _reportCache[reportID].State = ReportCache.CacheState.CleanCache;

                            //        //Time out?
                            //        while (_reportCache[reportID].State != ReportCache.CacheState.Clean)
                            //        {
                            //            Thread.Sleep(100);
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    _reportCache.Add(reportID, new ReportCache());
                            //}

                            //string[] args = new string[2];

                            //args[0] = reportID;
                            //args[1] = tempFileName;

                            //ThreadPool.QueueUserWorkItem(FillReportCache, args);
                        }
                        catch (Exception ex)
                        {
                            errorText += "\r\n\r\nError while loading report file for Report: " + reportID + ".\r\n\r\n" + ex.Message;
                        }
                    }
                    else
                    {
                        errorText += "\r\n\r\nReport: " + reportID + " has no report file.";
                    }
                }
            }
        }

        //private int _reportCounter = 0;

        //private void FillReportCache(object state)
        //{
        //    string reportID = ((string[])state)[0];
        //    string filepath = ((string[])state)[1];

        //    try
        //    {
        //        _reportCache[reportID].State = ReportCache.CacheState.Running;

        //        while (_reportCache[reportID].State == ReportCache.CacheState.Running)
        //        {
        //            if (_reportCache[reportID].Cache.Count < 2)
        //            {
        //                ReportDocument[] newReportCache = new ReportDocument[10];

        //                for (int i = 0; i < 3; i++)
        //                {
        //                    lock (_crystalLock)
        //                    {
        //                        ReportDocument preparedReport = new ReportDocument();
        //                        preparedReport.Load(filepath);

        //                        _reportCounter++;
        //                        newReportCache[i] = preparedReport;
        //                    }
        //                }

        //                _reportCache[reportID].Cache.PushRange(newReportCache);
        //            }

        //            Thread.Sleep(35);
        //        }

        //        if (_reportCache[reportID].State == ReportCache.CacheState.CleanCache)
        //        {
        //            while (_reportCache[reportID].Cache.Count > 0)
        //            {
        //                ReportDocument report;
        //                if (_reportCache[reportID].Cache.TryPop(out report))
        //                {
        //                    try { report.Close(); }
        //                    catch { }
        //                    finally { report.Dispose(); }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        _reportCache[reportID].State = ReportCache.CacheState.Disposed;
        //    }
        //}


        private ReportDocument GetReportDocument(string reportID, byte[] reportFileContent, out string errorText)
        {
            errorText = string.Empty;
            ReportDocument report = null;

            if (!_reportFilesDic.ContainsKey(reportID))
            {
                PrepareReports(new Dictionary<string, byte[]>() { { reportID, reportFileContent } }, new List<string>() { reportID }, out errorText);

                if (!string.IsNullOrEmpty(errorText))
                {
                    return null;
                }
            }

            report = new ReportDocument();
            report.Load(_reportFilesDic[reportID]);


            //if (!_reportCache.ContainsKey(reportID))
            //{
            //    lock (_reportCache)
            //    {
            //        if (!_reportCache.ContainsKey(reportID))
            //        {
            //            PrepareReports(new Dictionary<string, byte[]>() { { reportID, reportFileContent } }, new List<string>() { reportID }, out errorText);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(errorText))
            //    {
            //        return null;
            //    }

            //    Thread.Sleep(2000);
            //}

            //int faults = 0;
            //while (!_reportCache[reportID].Cache.TryPop(out report) && _reportCache[reportID].State != ReportCache.CacheState.Disposed)
            //{
            //    faults++;
            //    if (faults > 10)
            //    {
            //        return null;
            //    }
            //    Thread.Sleep(500);
            //}

            return report;
        }
    }
}
