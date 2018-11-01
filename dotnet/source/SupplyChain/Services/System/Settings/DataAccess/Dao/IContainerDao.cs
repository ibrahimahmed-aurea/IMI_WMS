using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Settings.BusinessEntities;

namespace Imi.SupplyChain.Settings.DataAccess.Dao
{
    public interface IContainerDao
    {
        IList<Container> FindContainer(string containerName);
        void DeleteContainer(string containerName);
        Container SaveOrUpdate(Container container);
    }
}
