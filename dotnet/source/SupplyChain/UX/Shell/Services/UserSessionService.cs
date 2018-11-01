using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Threading;
using System.Reflection;
using System.Security;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Shell.Services
{
    [Serializable]
    public class UserSessionService : IUserSessionService
    {
        private string _sessionId;
        private string _languageCode;
        private string _userId;
        private SecureString _password;
        private string _hostName;
        private int _hostPort;
        private string _instanceName;
        private string _domain;
        private string _domainUser;
        private string _terminalId;
        private CultureInfo _cultureInfo;
        private Version _currentVersion;
        private string _configFilename;
        private Uri _activationUri;
        
        public UserSessionService()
        {
            RandomNumberGenerator rng = RNGCryptoServiceProvider.Create();

            byte[] data = new byte[16];

            rng.GetBytes(data);

            SHA256 sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(data);

            _sessionId = Convert.ToBase64String(hash);

            _cultureInfo = Thread.CurrentThread.CurrentCulture;
        }

        public string Domain
        {
            get
            {
                return _domain;
            }
        }

        public string DomainUser
        {
            get
            {
                return _domainUser;
            }
        }

        public string SessionId
        {
            get
            {
                return _sessionId;
            }
            set
            {
                _sessionId = value;
            }
        }

        public string LanguageCode
        {
            get
            {
                return _languageCode;
            }
            set
            {
                _languageCode = value;
            }
        }

        public string UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                if (UserId != value)
                {
                    if ((value.Contains('\\') || value.Contains('/')))
                    {
                        string[] userIdParts = value.Split(new char[] { '\\', '/' });
                        _domain = userIdParts[0];
                        _userId = userIdParts[1];
                        _domainUser = value;
                    }
                    else
                    {
                        _userId = value;
                        _domainUser = value;
                    }
                }
            }
        }

        public SecureString Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
                
        public string HostName
        {
            get
            {
                return _hostName;
            }
            set
            {
                _hostName = value;
            }
        }

        public int HostPort
        {
            get
            {
                return _hostPort;
            }
            set
            {
                _hostPort = value;
            }
        }

        public string InstanceName
        {
            get
            {
                return _instanceName;
            }
            set
            {
                _instanceName = value;
            }
        }

        public string TerminalId
        {
            get
            {
                return _terminalId;
            }
            set
            {
                _terminalId = value;
            }
        }

        public CultureInfo UICulture
        {
            get
            {
                return _cultureInfo;
            }
            set
            {
                _cultureInfo = value;
                LanguageCode = _cultureInfo.ThreeLetterWindowsLanguageName.ToUpper();
            }
        }

        public Version CurrentVersion
        {
            get
            {
                return _currentVersion;
            }
            set
            {
                _currentVersion = value;
            }
        }

        public string ConfigFilename
        {
            get
            {
                return _configFilename;
            }
            set
            {
                _configFilename = value;
            }
        }

        public Uri ActivationUri
        {
            get
            {
                return _activationUri;
            }
            set
            {
                _activationUri = value;
            }
        }
    }
}
