using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.ServiceModel;
using System.Threading;
using System.Globalization;

namespace Imi.Framework.Services
{
    
    public class ApplicationContext
    {
        public const string DefaultLanguageCode = "ENU";
        
        [ThreadStatic]
        private static ApplicationContext currentContext;
                
        private CultureInfo uiCulture;
        private string terminalId;
        private string userId;
        private string sessionId;
                               
        public ApplicationContext()
        {
        }
                
        public static ApplicationContext Current
        {
            get
            {
                return currentContext;
            }
        }

        internal static void SetContext(ApplicationContext context)
        {
            currentContext = context;
        }

        public string LanguageCode
        {
            get
            {
                if (uiCulture != null)
                    return uiCulture.ThreeLetterWindowsLanguageName.ToUpper();
                else
                    return DefaultLanguageCode;
            }
        }

        public CultureInfo UICulture
        {
            get 
            {
                if (uiCulture == null)
                    return Thread.CurrentThread.CurrentCulture;
                else
                    return uiCulture;
            }
            set 
            {
                uiCulture = value; 
            }
        }
                
        public string TerminalId
        {
            get
            {
                return terminalId;
            }
            set
            {
                terminalId = value;
            }
        }

        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
            }
        }

        public string SessionId
        {
            get
            {
                return sessionId;
            }
            set
            {
                sessionId = value;
            }
        }
    }
}
