using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Imi.SupplyChain.Deployment
{
    public static class RunConsoleApp
    {
        static RunConsoleApp() {}

        public static string Run(string application, string arguments)
        {
            // Create a new process object
            Process process = new Process();

            // StartInfo contains the startup information of the new process
            process.StartInfo.FileName = application;
            process.StartInfo.Arguments = arguments;

            // These two optional flags ensure that no DOS window appears
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // This ensures that you get the output from the DOS application
            process.StartInfo.RedirectStandardOutput = true;

            // Start the process
            process.Start();

            // Wait that the process exits
            process.WaitForExit();

            // Now return the output of the DOS application
            string result = process.StandardOutput.ReadToEnd();

            // Dispose the process
            process.Dispose();

            return result;
        }
    }
}
