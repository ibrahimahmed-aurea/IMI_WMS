using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.OM.OutputHandler.ResourceAccess;
using DataDynamics.ActiveReports;
using System.Xml;
using System.IO;

namespace Imi.SupplyChain.OM.OutputHandler.Adapters
{
    public class RDFAdapter : IAdapter
    {
        private static string _OUTPUTENABLE = "OutputEnable";
        private Dictionary<string, string> _configurationParameters = null;

        public string AdapterIdentity
        {
            get { return "rdf"; }
        }

        public void InitializeAdapter(Dictionary<string, string> configurationParameters, Dictionary<string, byte[]> reportFilesForAdapter, string uniqueOutputManagerIdentity, out string errorText)
        {
            errorText = string.Empty;
            _configurationParameters = configurationParameters;
        }

        public void UpdateConfiguration(Dictionary<string, string> configurationParameters)
        {
            _configurationParameters = configurationParameters;
        }

        public Dictionary<string, object> Execute(OutputDocument document, Dictionary<string, object> namedPassThroughParameters)
        {
            Dictionary<string, object> results = new Dictionary<string, object>();
            try
            {
                if (document.MetaParameters[StdMetaParamNames.DocumentIDKey] != "GenericReport")
                {
                    throw new Exception("Report must be of type GenericReport.");
                }

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

                        using (ActiveReport report = new ActiveReport())
                        {
                            report.Document.Load(data);

                            //report.Document.Save(exportFilename, DataDynamics.ActiveReports.Document.RdfFormat.ARNet);

                            if (Convert.ToBoolean(_configurationParameters[_OUTPUTENABLE]))
                            {
                                report.Document.Printer.PrinterName = document.PrinterDeviceName;
                                report.Document.Printer.PrinterSettings.Copies = Convert.ToInt16(document.MetaParameters[StdMetaParamNames.NumberOfCopiesKey]);
                                report.Document.Print(false, false, false);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error printing RDF report.\r\n" + ex.Message);
            }

            return results;
        }

        public void UpdateReportFiles(Dictionary<string, byte[]> reportFilesForAdapter, List<string> updatedReports)
        {
            
        }

    }
}
