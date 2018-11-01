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
    public class DeleteContainerAction : MarshalByRefObject
    {
        private IContainerDao ContainerDao { get; set; }

        public DeleteContainerAction()
        {
            ContainerDao = DatabaseContext.CreateDao<IContainerDao>();
        }

        public DeleteContainerResult Execute(DeleteContainerParameters parameters)
        {
            using (ITransactionScope scope = DatabaseContext.CreateTransactionScope())
            {
                ContainerDao.DeleteContainer(parameters.ContainerName);
                scope.Complete();
            }

            return new DeleteContainerResult();
        }
    }
}
