using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace Imi.Framework.Job.Data
{
    public interface IDbConnectionProvider
    {
        IDbTransaction CurrentTransaction {
            get;
        }

        IDbConnection GetConnection();
    }
}
