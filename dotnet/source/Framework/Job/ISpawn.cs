using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Job.Configuration;

namespace Imi.Framework.Job
{
    public interface ISpawn
    {
        ManagedJob[] SpawnJobs();
    }
}
