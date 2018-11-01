using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IApplicationVersionDao
    {
        ApplicationVersion FindById(Guid applicationVersionId);
        IList<ApplicationVersion> FindAll();
        ApplicationVersion Save(ApplicationVersion applicationVersion);
        ApplicationVersion SaveOrUpdate(ApplicationVersion applicationVersion);
        void Delete(ApplicationVersion applicationVersion);
    }
}

