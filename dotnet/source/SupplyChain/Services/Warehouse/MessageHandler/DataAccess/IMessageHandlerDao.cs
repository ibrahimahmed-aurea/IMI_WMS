using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Warehouse.MessageHandler.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.MessageHandler.DataAccess
{
    public interface IMessageHandlerDao
    {
        GetMessageXMLResult GetErrorWarningXML(GetMessageXMLParameters parameters);
        GetInformationXMLResult GetInformationXML(GetInformationXMLParameters parameters);
        void Initialize(InitializeParameters parameters);
        void Reset();
    }
}
