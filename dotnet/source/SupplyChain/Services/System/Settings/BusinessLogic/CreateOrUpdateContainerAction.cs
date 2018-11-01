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
    public class CreateOrUpdateContainerAction : MarshalByRefObject
    {
        private IContainerDao ContainerDao { get; set; }

        public CreateOrUpdateContainerAction()
        {
            ContainerDao = DatabaseContext.CreateDao<IContainerDao>();
        }

        public CreateOrUpdateContainerResult Execute(CreateOrUpdateContainerParameters parameters)
        {
            CreateOrUpdateContainerResult result = new CreateOrUpdateContainerResult();

            if ((parameters.Container == null) || (string.IsNullOrEmpty(parameters.Container.Name)))
            {
                return result;
            }

            using (ITransactionScope scope = DatabaseContext.CreateTransactionScope())
            {
                var containers = ContainerDao.FindContainer(parameters.Container.Name);

                if ((containers != null) && (containers.Count == 1))
                {
                    var dbContainer = containers[0];

                    if (dbContainer.Name == parameters.Container.Name)
                    {
                        dbContainer.MetaData.Clear();

                        foreach (ContainerMetaData meta in parameters.Container.MetaData)
                        {
                            dbContainer.MetaData.Add(meta);
                        }

                        parameters.Container = dbContainer;
                    }
                }

                parameters.Container.LastModified = DateTime.Now.ToUniversalTime();
                ContainerDao.SaveOrUpdate(parameters.Container);

                scope.Complete();

            }

            return result;
        }
    }
}
