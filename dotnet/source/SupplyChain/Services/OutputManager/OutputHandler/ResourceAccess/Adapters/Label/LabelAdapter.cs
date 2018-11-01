using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.OM.OutputHandler.ResourceAccess;
using System.Xml;
using System.IO;
using Imi.Framework.Printing.LabelParsing;
using Imi.Framework.Printing;
using System.Threading;


namespace SupplyChain.OM.OutputHandler.Adapters
{
    public class LabelAdapter : IAdapter
    {
        private static string _OUTPUTENABLE = "OutputEnable";
        private Dictionary<string, string> _configurationParameters = null;
        private Dictionary<string, ParseLabel> _cachedLabelDictionary;
        private ReaderWriterLockSlim _cachedLabelsLock = new ReaderWriterLockSlim();

        public string AdapterIdentity
        {
            get { return "label"; }
        }

        public void InitializeAdapter(Dictionary<string, string> configurationParameters, Dictionary<string, byte[]> reportFilesForAdapter, string uniqueOutputManagerIdentity, out string errorText)
        {
            errorText = string.Empty;
            _configurationParameters = configurationParameters;
            _cachedLabelDictionary = new Dictionary<string, ParseLabel>();
            errorText = PrepareCache(reportFilesForAdapter, new List<string>());
        }

        public void UpdateReportFiles(Dictionary<string, byte[]> reportFilesForAdapter, List<string> updatedReports)
        {
            PrepareCache(reportFilesForAdapter, updatedReports);
        }

        public void UpdateConfiguration(Dictionary<string, string> configurationParameters)
        {
            _configurationParameters = configurationParameters;
        }

        public Dictionary<string, object> Execute(OutputDocument document, Dictionary<string, object> namedPassThroughParameters)
        {
            Dictionary<string, object> results = new Dictionary<string, object>();
            string labelText = null;
            byte[] labelData = null;

            string reportID = document.ReportID;
            string outputJobSequence = document.OutputJobSequence.ToString();
            string documentType = document.MetaParameters[StdMetaParamNames.DocumentTypeIDKey].ToString();
            string outputJobId = document.OutputJobId;

            string documentName = string.Format("{0}_{1}_{2}_{3}",
              outputJobId,
              outputJobSequence,
              documentType,
              reportID
              );

            try
            {

                if (document.MetaParameters[StdMetaParamNames.DocumentIDKey] != "GenericReport")
                {
                    XmlDocument doc = new XmlDocument();

                    document.Data.Seek(0, SeekOrigin.Begin);

                    using (StreamReader reader = new StreamReader(document.Data))
                    {
                        doc.Load(reader);
                    }

                    ParseLabel label = GetCachedLabel(document.ReportID);

                    string errorVariables;

                    lock (label)
                    {
                        labelText = label.Execute(doc, out errorVariables);
                    }

                    if (!string.IsNullOrEmpty(errorVariables))
                    {
                        throw new Exception(string.Format("Error processing label, unable to merge label template with label data.\r\n{0}", errorVariables));
                    }

                    labelData = Encoding.Default.GetBytes(labelText);
                }
                else
                {
                    document.Data.Seek(0, SeekOrigin.Begin);

                    using (XmlReader reader = XmlReader.Create(document.Data))
                    {
                        reader.ReadToFollowing("Data");

                        using (MemoryStream data = new MemoryStream())
                        {
                            int bytes = 0;
                            byte[] buf = new byte[65536];

                            while ((bytes = reader.ReadElementContentAsBase64(buf, 0, 65536)) > 0)
                            {
                                data.Write(buf, 0, bytes);
                            }

                            if (data.Length == 0)
                            {
                                throw new Exception("The report has no content.");
                            }

                            data.Seek(0, SeekOrigin.Begin);

                            labelData = data.ToArray();
                        }
                    }
                }


                if (Convert.ToBoolean(_configurationParameters[_OUTPUTENABLE]))
                {
                    PrintLabel(documentName, document.PrinterDeviceName, labelData);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error printing Label .\r\n" + ex.Message);
            }

            return results;
        }


        private static void PrintLabel(string documentName, string printerDevice, byte[] labelData)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                documentName = documentName.Replace(c, '_');
            }

            RawPrinting.Print(printerDevice, documentName, labelData);
        }



        private string PrepareCache(Dictionary<string, byte[]> reportFilesForAdapter, List<string> updatedReports)
        {
            string errorText = string.Empty;
            if (updatedReports.Count == 0)
            {
                _cachedLabelDictionary.Clear();
                updatedReports = reportFilesForAdapter.Keys.ToList();
            }

            StringBuilder errorLabels = new StringBuilder();

            foreach (string reportIdentity in updatedReports)
            {
                try
                {
                    if (reportFilesForAdapter[reportIdentity] != null)
                    {
                        string reportFile = System.Text.Encoding.Default.GetString(reportFilesForAdapter[reportIdentity]);
                        AddCachedLabel(reportIdentity, reportFile);
                    }
                    else
                    {
                        errorText += "\r\n\r\nReport: " + reportIdentity + " has no label template.";
                    }
                }
                catch (Exception ex)
                {
                    errorLabels.Append(string.Format("\r\nReportID: [{0}] - ERROR: {1}\r\n{2}", reportIdentity, ex.Message, "=============================================================================="));
                }
            }

            if (errorLabels.Length > 0)
            {
                throw new Exception(errorLabels.ToString());
            }

            return errorText;
        }

        private void AddCachedLabel(string reportIdentity, string labelContent)
        {
            ParseLabel label = null;

            _cachedLabelsLock.EnterWriteLock();

            try
            {
                label = new ParseLabel();

                string compileErrors;

                if (!label.LoadTemplate(labelContent, out compileErrors))
                {
                    _cachedLabelDictionary.Remove(reportIdentity.ToUpper());

                    throw new Exception(string.Format("Error compiling the label template.\r\n{1}\r\n{0}", compileErrors, "-------------------------------------------------------------------------------------------------------------------------------"));
                }

                if (_cachedLabelDictionary.ContainsKey(reportIdentity.ToUpper()))
                {
                    _cachedLabelDictionary[reportIdentity.ToUpper()] = label;
                }
                else
                {
                    _cachedLabelDictionary.Add(reportIdentity.ToUpper(), label);
                }
            }
            finally
            {
                _cachedLabelsLock.ExitWriteLock();
            }

        }

        private ParseLabel GetCachedLabel(string reportIdentity)
        {
            ParseLabel label = null;

            _cachedLabelsLock.EnterReadLock();

            try
            {
                if (_cachedLabelDictionary.ContainsKey(reportIdentity.ToUpper()))
                {
                    label = _cachedLabelDictionary[reportIdentity.ToUpper()];
                }
                else
                {
                    throw new Exception(string.Format("Error: No Label Template found for ReportID: [{0}]", reportIdentity));
                }
            }
            finally
            {
                _cachedLabelsLock.ExitReadLock();
            }

            return label;
        }
    }


}
