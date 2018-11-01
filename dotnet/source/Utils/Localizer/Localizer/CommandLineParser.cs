using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imi.Utils.Localizer
{
    public class CommandLineParser
    {
        private string CommandLine;
        private Dictionary<string, string> namedArgs = new Dictionary<string, string>();
        private Dictionary<string, string> numberedArgs = new Dictionary<string, string>();

        public CommandLineParser(string[] args)
        {
            string aKey = string.Empty;
            int n = 0;
            this.CommandLine = string.Empty;
            foreach (string argItem in args)
            {
                if (argItem.Contains(" "))
                {
                    this.CommandLine = this.CommandLine + "\"" + argItem + "\" ";
                }
                else
                {
                    this.CommandLine = this.CommandLine + argItem + " ";
                }
            }
            this.CommandLine.Trim();
            foreach (string argItem in args)
            {
                if (argItem.StartsWith("/") || argItem.StartsWith("-"))
                {
                    aKey = argItem.ToLower().Substring(1);
                    this.namedArgs.Add(aKey, string.Empty);
                }
                else if (aKey != "")
                {
                    this.namedArgs[aKey] = argItem;
                    aKey = "";
                }
                else
                {
                    n++;
                    this.numberedArgs.Add(string.Format("${0}", n), argItem);
                }
            }
        }

        public string GetCommandLine()
        {
            return this.CommandLine;
        }

        public string GetParameterValue(string parameterName)
        {
            string key = parameterName.ToLower();
            if (this.namedArgs.ContainsKey(key))
            {
                return this.namedArgs[key];
            }
            if (this.numberedArgs.ContainsKey(key))
            {
                return this.numberedArgs[key];
            }
            return null;
        }

        public bool IsEnabled(string switchName)
        {
            return (this.namedArgs.ContainsKey(switchName.ToLower()) || this.numberedArgs.ContainsKey(switchName.ToLower()));
        }

        public string this[string parameterName]
        {
            get
            {
                return this.GetParameterValue(parameterName);
            }
        }
    }
}

