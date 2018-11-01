using System;
using System.Runtime.InteropServices;

namespace Imi.Framework.Printing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DOCINFO
    {
        [MarshalAs(UnmanagedType.LPWStr)]public string pDocName;
        [MarshalAs(UnmanagedType.LPWStr)]public string pOutputFile;
        [MarshalAs(UnmanagedType.LPWStr)]public string pDataType;
    }

    public sealed class WinSpoolAPI
    {
        [ DllImport( "winspool.drv", EntryPoint="OpenPrinterW", CharSet=CharSet.Unicode, ExactSpelling=true, CallingConvention=CallingConvention.StdCall, SetLastError = true )]
        public static extern long OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);

        [DllImport("winspool.drv", EntryPoint = "StartDocPrinterW", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern long StartDocPrinter(IntPtr hPrinter, int Level, ref DOCINFO pDocInfo);

        [DllImport("winspool.drv", EntryPoint = "StartPagePrinter", CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern long StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", EntryPoint = "WritePrinter", CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern long WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, ref int dwWritten);

        [DllImport("winspool.drv", EntryPoint = "EndPagePrinter", CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern long EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", EntryPoint = "EndDocPrinter", CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern long EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", EntryPoint = "ClosePrinter", CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern long ClosePrinter(IntPtr hPrinter);
    }

}
