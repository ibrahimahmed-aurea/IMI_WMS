using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Imi.Wms.Mobile.UI
{
    public class Logger
    {
        private static bool _isEnabled;
        private static string _logFileName;
        private static bool _isInitialized;

        public static string LogFileName
        {
            get
            {
                return _logFileName;
            }
            set
            {
                _logFileName = value;
            }
        }

        public static bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;

                if (_isEnabled && !_isInitialized)
                {
                    Trace.Listeners.Add(new TextWriterTraceListener(_logFileName));
                    Trace.AutoFlush = true;
                    _isInitialized = true;
                }
            }
        }

        private Logger()
        { 
        }

        public static void Write(string message)
        {
            if (IsEnabled)
            {
                Trace.WriteLine(string.Format("{0}: {1}", DateTime.Now, message));
            }
        }
    }
}
