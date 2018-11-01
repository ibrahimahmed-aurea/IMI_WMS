using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Shared
{
    /// <summary>
    /// Class for parsing command line parameters.
    /// </summary>
    public class CommandLineParser
    {
        private Dictionary<string, List<string>> namedArgs;
        private Dictionary<string, string> numberedArgs;
        private string CommandLine;     // Full Commandline

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="CommandLineParser"/> class.</para>
        /// </summary>
        /// <param name="args">
        /// Command line arguments to parse.
        /// </param>
        public CommandLineParser(string[] args)
        {
            namedArgs = new Dictionary<string, List<string>>();
            numberedArgs = new Dictionary<string, string>();

            String aKey = String.Empty;
            int n = 0;
            CommandLine = String.Empty;

            // Handle CommandLine
            foreach (String argItem in args)
            {
                if (argItem.Contains(" "))
                {
                    CommandLine += "\"" + argItem + "\" ";
                }
                else
                {
                    CommandLine += argItem + " ";
                }
            }

            // Trim Commandline
            CommandLine.Trim();

            // Handle all arguments
            foreach (String argItem in args)
            {
                if (argItem.StartsWith("/") || argItem.StartsWith("-"))
                {
                    aKey = argItem.ToLower().Substring(1);
                    if (!namedArgs.ContainsKey(aKey))
                    {
                        namedArgs.Add(aKey, new List<string>());
                    }
                }
                else
                {
                    // Check if last argument was a named one. In that case this next one
                    // should belong to that one.
                    if (aKey != "")
                    {
                        namedArgs[aKey].Add(argItem);
                        aKey = "";
                    }
                    else
                    {
                        // Add as unnamed argument
                        n++;
                        numberedArgs.Add(String.Format("${0}", n), argItem);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a command line switch is enabled.
        /// </summary>
        /// <param name="switchName">The name of the switch.</param>
        /// <returns>True if the switch is enabled, otherwise false.</returns>
        public bool IsEnabled(string switchName)
        {
            return (namedArgs.ContainsKey(switchName.ToLower()) ||
                    numberedArgs.ContainsKey(switchName.ToLower())
                  );
        }

        /// <summary>
        /// Returns the value of the specified command line parameter.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <returns>The parameter value.</returns>
        public string GetParameterValue(string parameterName)
        {
            string key = parameterName.ToLower();

            if (namedArgs.ContainsKey(key))
                return namedArgs[key][0];

            if (numberedArgs.ContainsKey(key))
                return numberedArgs[key];

            return null;
        }

        public List<string> GetNamedParameterValues(string parameterName)
        {
            string key = parameterName.ToLower();

            if (namedArgs.ContainsKey(key))
            {
                return namedArgs[key];
            }

            return null;
        }

        /// <summary>
        /// Returns the value of the specified command line parameter.
        /// </summary>
        /// <param name="parameterName">The name of the command line parameter.</param>
        /// <returns>The parameter value.</returns>
        public string this[string parameterName]
        {
            get
            {
                return GetParameterValue(parameterName);
            }
        }

        /// <summary>
        /// Gets the full CommandLine in one string.
        /// </summary>
        /// <returns>The CommandLine in one string.</returns>
        public string GetCommandLine()
        {
            return CommandLine;
        }

    }
}
