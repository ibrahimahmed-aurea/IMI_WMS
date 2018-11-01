using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Imi.SupplyChain.OM.OutputHandler.ResourceAccess
{
    public class OutputDocument
    {
        public string OutputJobId { get; set; }
        public int OutputJobSequence { get; set; }

        public Dictionary<string, string> MetaParameters { get; set; }
        public Stream Data { get; set; }
        public Dictionary<string, string> Parameters { get; set; }

        public string PrinterDeviceName { get; set; }
        public string PrinterID { get; set; }
        public byte[] ReportFile { get; set; }
        public string ReportID { get; set; }

        public List<string> AdapterIDs { get; set; }

        public string ErrorDescription { get; set; }

        public string DataAsString
        {
            get
            {
                Data.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(Data);
                return reader.ReadToEnd();
            }
        }

    }
}
