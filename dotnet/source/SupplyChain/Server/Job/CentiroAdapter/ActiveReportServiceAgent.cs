using System;
using System.Collections.Generic;
using System.Text;

using DataDynamics.ActiveReports;
using System.IO;

namespace Imi.Wms.Server.Job.CentiroAdapter
{
    public class ActiveReportServiceAgent
    {

        public static bool PrintDocument(byte[] data, string printername, short copies)
        {

            ActiveReport ar = new ActiveReport();

            Stream mstr = new MemoryStream(data);

            ar.Document.Load(mstr);

            ar.Document.Printer.PrinterName = printername;

            ar.Document.Printer.PrinterSettings.Copies = copies;

            ar.Document.Print(false, false, false);

            mstr.Close();

            return true;


        }

    }
}
