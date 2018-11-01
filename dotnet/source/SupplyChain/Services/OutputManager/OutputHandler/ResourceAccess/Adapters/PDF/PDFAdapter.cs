using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.OM.OutputHandler.ResourceAccess;
using System.IO;
using System.Xml;
using PdfPrintingNet;

namespace SupplyChain.OM.OutputHandler.Adapters
{
    public class PDFAdapter : IAdapter
    {
        private static string _OUTPUTENABLE = "OutputEnable";
        private Dictionary<string, string> _configurationParameters = null;

        #region IAdapter Members

        public string AdapterIdentity
        {
            get { return "pdf"; }
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

        public void UpdateReportFiles(Dictionary<string, byte[]> reportFilesForAdapter, List<string> updatedReports)
        {

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

                    byte[] pdfDoc = new byte[0];
                    int bytesRead = 0;
                    byte[] buf = new byte[65536];

                    while ((bytesRead = reader.ReadElementContentAsBase64(buf, 0, 65536)) > 0)
                    {
                        int oldEndIndex = pdfDoc.Length == 0 ? 0 : (pdfDoc.Length - 1);
                        Array.Resize<byte>(ref pdfDoc, (pdfDoc.Length + bytesRead));
                        Array.Copy(buf, 0, pdfDoc, oldEndIndex, bytesRead);
                    }

                    if (pdfDoc.Length == 0)
                    {
                        throw new Exception("The report has no content.");
                    }


                    if (Convert.ToBoolean(_configurationParameters[_OUTPUTENABLE]))
                    {
                        PdfPrint pdfPrint = new PdfPrint("Aptean AB", "g/4JFMjn6KvRA39zUa4QClRtNo6h5TnW273JkSGKwP0=");
                        pdfPrint.PrinterName = document.PrinterDeviceName;
                        pdfPrint.Copies = Convert.ToInt16(document.MetaParameters[StdMetaParamNames.NumberOfCopiesKey]);

                        PdfPrint.Status status = pdfPrint.Print(pdfDoc);

                        if (status != PdfPrint.Status.OK)
                        {
                            throw new Exception("Error printing PDF report: " + status.ToString());
                        }
                    }

                    if (results.ContainsKey(StdResultParamNames.BinaryResultKey))
                    {
                        results.Remove(StdResultParamNames.BinaryResultKey);
                    }

                    results.Add(StdResultParamNames.BinaryResultKey, pdfDoc);

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error printing PDF report.\r\n" + ex.Message);
            }

            return results;
        }

        #endregion
    }
}
