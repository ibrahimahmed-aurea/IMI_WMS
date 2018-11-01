using System;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Windows.Forms;

namespace Imi.Framework.Shared.IO
{
    public class FileIO
    {
        public static StreamReader GetFileFromResources(string fileName)
        {
            try
            {
                Assembly assembly = Assembly.GetCallingAssembly();
                Stream stream = assembly.GetManifestResourceStream(fileName);
                return (new StreamReader(stream));
            }
            catch (ArgumentNullException ane)
            {
                throw (new ConfigurationErrorsException(
                    String.Format("Failure when loading resource file {0}",fileName),ane));
            }
            catch (ArgumentException ae)
            {
                throw (new ConfigurationErrorsException(
                    String.Format("Failure when loading resource file {0}", fileName), ae));
            }
            catch (FileLoadException fle)
            {
                throw (new ConfigurationErrorsException(
                    String.Format("Failure when loading resource file {0}", fileName), fle));
            }
            catch (FileNotFoundException fnf)
            {
                throw (new ConfigurationErrorsException(
                    String.Format("Failure when loading resource file {0}", fileName), fnf));
            }
            catch (BadImageFormatException bif)
            {
                throw (new ConfigurationErrorsException(
                    String.Format("Failure when loading resource file {0}", fileName), bif));
            }
        }

        public static string FindInitDirectory()
        {
            int pos = Application.ExecutablePath.LastIndexOf(@"\");

            if (pos > -1)
            {
                string dir = Application.ExecutablePath.Substring(0, pos);

                for (int i = 0; i < 3; i++)
                {
                    pos = dir.LastIndexOf(@"\");

                    if (pos > -1)
                    {
                        dir = dir.Substring(0, pos);

                        if (Directory.Exists(dir + @"\init"))
                        {
                            return (dir + @"\init");
                        }
                    }
                    else
                        break;
                }
            }

            return (@"\init");
        }

        public static string FindConfigFile(string baseFileName)
        {
            // Try to locate the file manually
            string initDir = FindInitDirectory();

            string fileName = initDir + @"\" + baseFileName;

            if (File.Exists(fileName))
            {
                return (fileName);
                //throw (new ConfigurationErrorsException(
                //  string.Format("The configuration file is missing, file = \"{0}\"", baseFileName)));
            }

            return (null);
        }

        public static string FindAppConfigFile(string key, string defaultName)
        {
            string fileName = ConfigurationManager.AppSettings[key];

            if (fileName == null)
            {
                fileName = FindConfigFile(defaultName);
            }

            return (fileName);
        }

    }
}
