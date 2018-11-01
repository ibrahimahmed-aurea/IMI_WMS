using System;
using System.Threading;

namespace Imi.Framework.Job.Interfaces
{
    public interface ISubscriber
    {
        string Name
        {
            get;
        }

        AutoResetEvent GetWaitObject();
    }
}
