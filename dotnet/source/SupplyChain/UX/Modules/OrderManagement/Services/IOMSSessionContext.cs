using System.Collections.Generic;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Services
{
    public interface IOMSSessionContext : IUserSessionService
    {
        string AutoStart {get; set;}
        string Host { get; set; }
        decimal Port { get; set; }
        string ServerProgram { get; set; }
        string Parameters { get; set; }
        string ServerWorkingDirectory { get; set; }
        string OMSLanguageCode { get; set; }
        string ClientProgram { get; set; }
        string EnvironmentVariables { get; set; }
        string OMSLogicalUserId { get; set; }
        string OMSLoginUserId { get; set; }
        decimal WarehouseNumber { get; set; }
        decimal LegalEntity { get; set; }
        string UserName { get; set; }
        string OrgUnit { get; set; }
        string EmployNumber { get; set; }
        string SystemName { get; set; }
        string HelpUrl { get; set; }
        string DecimalKey { get; set; }
        Dictionary<string, OMSLogicalUserData> OMSUsersList { get; set; }
    }
}
