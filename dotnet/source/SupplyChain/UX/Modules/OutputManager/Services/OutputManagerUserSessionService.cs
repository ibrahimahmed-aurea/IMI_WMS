using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.UX.Modules.OutputManager.Infrastructure.Services;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using System.Security;
using System.Globalization;

namespace Imi.SupplyChain.UX.Modules.OutputManager.Services
{
    public class OutputManagerUserSessionService : IOutputManagerUserSessionService
    {
        private IUserSessionService userSessionService;

        [InjectionConstructor]
        public OutputManagerUserSessionService([ServiceDependency] IUserSessionService userSessionService)
        {
            this.userSessionService = userSessionService;
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
                return "ENU";
            }
            set
            {
                userSessionService.LanguageCode = value;
            }
        }

        private string userId;

        public string UserId
        {
            get
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return userSessionService.UserId;
                }
                else
                {
                    return userId;
                }
            }
            set
            {
                userId = value;
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

        private string outputManagerId;

        public string OutputManagerId
        {
            get
            {
                return outputManagerId;
            }
            set
            {
                outputManagerId = value;
            }
        }

        

        public CultureInfo UICulture
        {
            get
            {
                return CultureInfo.CreateSpecificCulture("en-US");
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
