using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Settings.BusinessEntities;

namespace Imi.SupplyChain.Settings.DataAccess.Dao
{
    public interface IBlobDao
    {
        IList<Blob> FindContainerBlob(string containerName, string blobName);
        Blob SaveOrUpdate(Blob blob);
        void DeleteBlob(string containerName, string blobName);
    }
}
