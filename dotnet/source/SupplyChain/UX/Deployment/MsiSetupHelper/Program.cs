using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace MsiSetupHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "/install")
            {
                string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (File.Exists(Path.Combine(exepath, "Unzip.exe")) && File.Exists(Path.Combine(exepath, "SmartClient_msi.zip")))
                {
                    //Update creationtime an all installed files before unzip. To be able to do uninstall operation later.
                    DateTime timestamp = DateTime.Now;
                    foreach (string file in Directory.GetFiles(exepath))
                    {
                        try
                        {
                            File.SetCreationTime(Path.Combine(exepath, file), timestamp);
                        }
                        catch
                        {
                        }
                    }


                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = Path.Combine(exepath, "Unzip.exe");
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                    startInfo.RedirectStandardOutput = false;
                    startInfo.RedirectStandardError = false;
                    startInfo.UseShellExecute = true;
                    startInfo.CreateNoWindow = true;
                    startInfo.WorkingDirectory = exepath;
                    startInfo.Arguments = "\"" + Path.Combine(exepath, "SmartClient_msi.zip") + "\"";
                    try
                    {
                        // Start the process with the info we specified.
                        // Call WaitForExit and then the using statement will close.
                        using (Process exeProcess = Process.Start(startInfo))
                        {
                            exeProcess.WaitForExit();
                        }
                    }
                    catch
                    {
                        // Log error.
                    }

                }

            }
            else if (args[0] == "/uninstall")
            {
                DateTime createtime = File.GetCreationTime(Assembly.GetExecutingAssembly().Location);

                string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                foreach (string file in Directory.GetFiles(exepath))
                {
                    string filePath = Path.Combine(exepath, file);
                    if (createtime != File.GetCreationTime(filePath) && filePath != Assembly.GetExecutingAssembly().Location)
                    {
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch
                        {
                        }
                    }
                }

                foreach (string dir in Directory.GetDirectories(exepath))
                {
                    try
                    {
                        Directory.Delete(Path.Combine(exepath, dir), true);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
