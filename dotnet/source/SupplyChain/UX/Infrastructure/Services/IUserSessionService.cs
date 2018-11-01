using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Security;
using System.Globalization;

namespace Imi.SupplyChain.UX.Infrastructure.Services
{
    public interface IUserSessionService
    {
        string InstanceName { get; set; }
        string Domain { get; }
        string DomainUser { get; }
        string SessionId { get; set; }
        string LanguageCode { get; set; }
        string UserId { get; set; }
        SecureString Password { get; set; }
        string HostName { get; set; }
        int HostPort { get; set; }
        string TerminalId { get; set; }
        CultureInfo UICulture { get; set; }
        Uri ActivationUri { get; set; }
    }
}
