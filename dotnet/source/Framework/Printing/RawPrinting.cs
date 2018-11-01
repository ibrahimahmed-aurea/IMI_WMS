using System;
using System.ComponentModel;
using System.Text;
using System.Runtime.InteropServices;

namespace Imi.Framework.Printing
{
    public class RawPrinting
    {
        /// <summary>
        ///     Prints raw text directly to a printershare with Win32 API.
        ///     If any errors occurs it will throw an exception with the native Win32 exception as the inner exception.
        ///     Check http://support.microsoft.com/kb/322091 for reference.
        /// </summary>
        /// <param name="printerShare">The printershare to write the text to. Example: \\server\printersharename</param>
        /// <param name="documentName">Name of the document to print. This will show up in the printer queue as the name of the document.</param>
        /// <param name="printText">The text to write to the printer.</param>
        /// <returns>Returns the number of bytes written to the printer.</returns>
                
        public static int Print(string printerShare, string documentName, byte[] data)
        {
            // Placeholder for printer handle
            IntPtr lhPrinter = new System.IntPtr();

            int dwWritten = 0;
            long retVal;

            // Create document structure
            DOCINFO di = new DOCINFO();
            di.pDocName = documentName;
            di.pDataType = "RAW";

            // lhPrinter contains the handle for the printer opened
            // If lhPrinter is 0 then an error has occured

            // The OpenPrinter function retrieves a handle to the specified printer 
            retVal = WinSpoolAPI.OpenPrinter(printerShare, ref lhPrinter, 0);

            if (retVal == 0 || lhPrinter.ToInt32() == 0)
            {
                ThrowException(string.Format("WinSpoolAPI: Error opening printer '{0}'.", printerShare), new Win32Exception());
            }

            // The StartDocPrinter function notifies the print spooler that a document is to be spooled for printing.
            retVal = WinSpoolAPI.StartDocPrinter(lhPrinter, 1, ref di);

            if (retVal == 0)
            {
                // Create the win32 exception that fetches the error
                Win32Exception win32Ex = new Win32Exception();

                // Cleanup
                WinSpoolAPI.ClosePrinter(lhPrinter);

                // Throw exception
                ThrowException(string.Format("WinSpoolAPI: Error calling StartDocPrinter (printer '{0}')", printerShare), win32Ex);
            }

            // The StartPagePrinter function notifies the spooler that a page is about to be printed on the specified printer.
            retVal = WinSpoolAPI.StartPagePrinter(lhPrinter);

            if (retVal == 0)
            {
                // Create the win32 exception that fetches the error
                Win32Exception win32Ex = new Win32Exception();

                // Cleanup
                WinSpoolAPI.EndDocPrinter(lhPrinter);
                WinSpoolAPI.ClosePrinter(lhPrinter);

                // Throw exception
                ThrowException(string.Format("WinSpoolAPI: Error calling StartPagePrinter (printer '{0}')", printerShare), win32Ex);
            }
                                                
            IntPtr pBytes = Marshal.AllocCoTaskMem(data.Length);
            
            try
            {
                Marshal.Copy(data, 0, pBytes, data.Length);
                retVal = WinSpoolAPI.WritePrinter(lhPrinter, pBytes, data.Length, ref dwWritten);
            }
            finally
            {
                Marshal.FreeCoTaskMem(pBytes);
            }

            if (retVal == 0)
            {
                // Create the win32 exception that fetches the error
                Win32Exception win32Ex = new Win32Exception();

                // Cleanup
                WinSpoolAPI.EndPagePrinter(lhPrinter);
                WinSpoolAPI.EndDocPrinter(lhPrinter);
                WinSpoolAPI.ClosePrinter(lhPrinter);

                // Throw exception
                ThrowException(string.Format("WinSpoolAPI: Error writing data to the printer with WritePrinter call (printer '{0}')", printerShare), win32Ex);
            }

            // The EndPagePrinter function notifies the print spooler that the application is at the end of a page in a print job.
            retVal = WinSpoolAPI.EndPagePrinter(lhPrinter);

            if (retVal == 0)
            {
                // Create the win32 exception that fetches the error
                Win32Exception win32Ex = new Win32Exception();

                // Cleanup
                WinSpoolAPI.EndDocPrinter(lhPrinter);
                WinSpoolAPI.ClosePrinter(lhPrinter);

                // Throw exception
                ThrowException(string.Format("WinSpoolAPI: Error calling EndPagePrinter (printer '{0}')", printerShare), win32Ex);
            }

            // The EndDocPrinter function ends a print job for the specified printer.
            retVal = WinSpoolAPI.EndDocPrinter(lhPrinter);

            if (retVal == 0)
            {
                // Create the win32 exception that fetches the error
                Win32Exception win32Ex = new Win32Exception();

                // Cleanup
                WinSpoolAPI.ClosePrinter(lhPrinter);

                // Throw exception
                ThrowException(string.Format("WinSpoolAPI: Error calling EndDocPrinter (printer '{0}')", printerShare), win32Ex);
            }

            // The ClosePrinter function closes the specified printer object.
            retVal = WinSpoolAPI.ClosePrinter(lhPrinter);

            if (retVal == 0)
                ThrowException(string.Format("WinSpoolAPI: Error calling ClosePrinter (printer '{0}')", printerShare), new Win32Exception());

            return dwWritten;
        }

        private static void ThrowException(string message, Win32Exception win32Exception)
        {
            throw new Exception(string.Format("{0}\r\nWin32 Error: ({1}) {2}", message, win32Exception.NativeErrorCode.ToString(), win32Exception.Message), win32Exception);
        }
    }
}
