using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Settings.DataAccess;
using Imi.SupplyChain.Settings.DataAccess.Dao;
using Imi.SupplyChain.Settings.BusinessEntities;
using Imi.Framework.DataAccess;

namespace Imi.SupplyChain.Settings.BusinessLogic
{
    public class FindContainerAction : MarshalByRefObject
    {
        private IContainerDao ContainerDao { get; set; }

        public FindContainerAction()
        {
            ContainerDao = DatabaseContext.CreateDao<IContainerDao>();
        }

        public FindContainerResult Execute(FindContainerParameters parameters)
        {
            using (ITransactionScope scope = DatabaseContext.CreateTransactionScope())
            {
                var result = new FindContainerResult();
                var containers = ContainerDao.FindContainer(parameters.ContainerName);

                if ((containers != null) && (containers.Count > 0))
                {
                    result.AddRange(containers);
                }

                scope.Complete();

                return result;
            }
        }
    }

}



