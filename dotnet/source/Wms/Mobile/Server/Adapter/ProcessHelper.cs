using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace Imi.Wms.Mobile.Server.Adapter
{
    public class ProcessHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct StartupInformation
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        private struct ProcessInformation
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [DllImport("kernel32.dll", SetLastError=true)]
        static extern bool CreateProcess(string lpApplicationName,
           string lpCommandLine, IntPtr lpProcessAttributes,
           IntPtr lpThreadAttributes, bool bInheritHandles,
           uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory,
           ref StartupInformation lpStartupInfo,
           out ProcessInformation lpProcessInformation);

        private const uint NormalPriorityClass = 0x0020;

        private ProcessHelper()
        { 
        }

        public static Process CreateProcess(string executablePath, string commandline, string desktopName)
        {
            executablePath = Path.GetFullPath(executablePath);
            commandline = string.Format("{0} {1}", executablePath, commandline);

            if (!File.Exists(executablePath))
            { 
                throw new FileNotFoundException("The applicaiton file was not found.", executablePath);
            }
                                    
            var startInfo = new StartupInformation();
            startInfo.cb = Marshal.SizeOf(typeof(StartupInformation));
            startInfo.lpDesktop = desktopName;
            
            ProcessInformation processInfo;

            var result = CreateProcess(executablePath, commandline, IntPtr.Zero, IntPtr.Zero, true, NormalPriorityClass, IntPtr.Zero, Path.GetDirectoryName(executablePath), ref startInfo, out processInfo);
            
            if (!result)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return Process.GetProcessById(processInfo.dwProcessId);
        }
    }
}
