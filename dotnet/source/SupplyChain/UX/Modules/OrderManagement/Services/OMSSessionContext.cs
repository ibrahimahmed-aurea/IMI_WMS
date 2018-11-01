using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Services
{
    public class OMSSessionContext : IOMSSessionContext
    {
        private IUserSessionService userSessionService;

        [InjectionConstructor]
        public OMSSessionContext([ServiceDependency] IUserSessionService userSessionService)
        {
            this.userSessionService = userSessionService;
        }
        
        private string autoStart;
        public string AutoStart
        {
            get
            {
                return autoStart;
            }
            set
            {
                autoStart = value;
            }
        }

        private string host;
        public string Host
        {
            get
            {
                return host;
            }
            set
            {
                host = value;
            }
        }

        private decimal port;
        public decimal Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }

        private string serverProgram;
        public string ServerProgram
        {
            get
            {
                return serverProgram;
            }
            set
            {
                serverProgram = value;
            }
        }

        private string parameters;
        public string Parameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters = value;
            }
        }

        private string serverWorkingDirectory;
        public string ServerWorkingDirectory
        {
            get
            {
                return serverWorkingDirectory;
            }
            set
            {
                serverWorkingDirectory = value;
            }
        }

        private string oMSLanguageCode;
        public string OMSLanguageCode
        {
            get
            {
                return oMSLanguageCode;
            }
            set
            {
                oMSLanguageCode = value;
            }
        }

        private string clientProgram;
        public string ClientProgram
        {
            get
            {
                return clientProgram;
            }
            set
            {
                clientProgram = value;
            }
        }

        private string environmentVariables;
        public string EnvironmentVariables
        {
            get
            {
                return environmentVariables;
            }
            set
            {
                environmentVariables = value;
            }
        }

        private string oMSLogicalUserId;
        public string OMSLogicalUserId
        {
            get
            {
                return oMSLogicalUserId;
            }
            set
            {
                oMSLogicalUserId = value;
            }
        }

        private string omsLoginUserId;
        public string OMSLoginUserId
        {
            get
            {
                return omsLoginUserId;
            }
            set
            {
                omsLoginUserId = value;
            }
        }

        private decimal warehouseNumber;
        public decimal WarehouseNumber
        {
            get
            {
                return warehouseNumber;
            }
            set
            {
                warehouseNumber = value;
            }
        }

        private decimal legalEntity;
        public decimal LegalEntity
        {
            get
            {
                return legalEntity;
            }
            set
            {
                legalEntity = value;
            }
        }

        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        private string orgUnit;
        public string OrgUnit
        {
            get
            {
                return orgUnit;
            }
            set
            {
                orgUnit = value;
            }
        }

        private string employNumber;
        public string EmployNumber
        {
            get
            {
                return employNumber;
            }
            set
            {
                employNumber = value;
            }
        }

        private string systemName;
        public string SystemName
        {
            get
            {
                return systemName;
            }
            set
            {
                systemName = value;
            }
        }

        private string helpUrl;
        public string HelpUrl
        {
            get
            {
                return helpUrl;
            }
            set
            {
                helpUrl = value;
            }
        }

        private string decimalKey;
        public string DecimalKey
        {
            get
            {
                return decimalKey;
            }
            set
            {
                decimalKey = value;
            }
        }

        private Dictionary<string, OMSLogicalUserData> oMSUsersList;
        public Dictionary<string, OMSLogicalUserData> OMSUsersList
        {
            get
            {
                return oMSUsersList;
            }
            set
            {
                oMSUsersList = value;
            }
        }

        public string InstanceName
        {
            get
            {
                return userSessionService.InstanceName;
            }
            set
            {
                userSessionService.InstanceName = value;
            }
        }

        public string Domain
        {
            get
            {
                return userSessionService.Domain;
            }
        }

        public string DomainUser
        {
            get
            {
                return userSessionService.DomainUser;
            }
        }

        public string SessionId
        {
            get
            {
                return userSessionService.SessionId;
            }
            set
            {
                userSessionService.SessionId = value;
            }
        }

        public string LanguageCode
        {
            get
            {
                return userSessionService.LanguageCode;
            }
            set
            {
                userSessionService.LanguageCode = value;
            }
        }

        public string UserId
        {
            get
            {
                return userSessionService.UserId;
            }
            set
            {
                userSessionService.UserId = value;
            }
        }

        public SecureString Password
        {
            get
            {
                return userSessionService.Password;
            }
            set
            {
                userSessionService.Password = value;
            }
        }

        public string HostName
        {
            get
            {
                return userSessionService.HostName;
            }
            set
            {
                userSessionService.HostName = value;
            }
        }

        public int HostPort
        {
            get
            {
                return userSessionService.HostPort;
            }
            set
            {
                userSessionService.HostPort = value;
            }
        }

        public string TerminalId
        {
            get
            {
                return userSessionService.TerminalId;
            }
            set
            {
                userSessionService.TerminalId = value;
            }
        }

        public CultureInfo UICulture
        {
            get
            {
                return userSessionService.UICulture;
            }
            set
            {
                userSessionService.UICulture = value;
            }
        }

        public Uri ActivationUri
        {
            get
            {
                return userSessionService.ActivationUri;
            }
            set
            {
                userSessionService.ActivationUri = value;
            }
        }
    }
}
