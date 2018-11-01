using System;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public interface IServiceMethodHelper
    {
        ServiceMethod SaveAndSynchronize(ServiceMethod serviceMethod);
    }
}
