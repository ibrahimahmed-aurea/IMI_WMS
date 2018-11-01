using System.Collections.Generic;
using System.Xml;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Services
{
    public interface IAOMWebServiceWrapper
    {
        IList<string> GetUsers();
        XmlDocument GetMenu();
        void GetGuiConfiguration();
        string GetAWSOneTimePassword(string logicalUserID);
    }
}
