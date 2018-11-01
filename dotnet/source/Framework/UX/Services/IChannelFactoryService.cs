using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Framework.UX.Services
{
    public interface IChannelFactoryService : IDisposable
    {
        object CreateChannel(Type channelType);
    }
}
