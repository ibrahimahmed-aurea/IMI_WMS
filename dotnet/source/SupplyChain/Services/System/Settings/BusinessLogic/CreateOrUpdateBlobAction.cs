using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using Imi.SupplyChain.Settings.DataAccess;
using Imi.SupplyChain.Settings.DataAccess.Dao;
using Imi.SupplyChain.Settings.BusinessEntities;

namespace Imi.SupplyChain.Settings.BusinessLogic
{
    public class CreateOrUpdateBlobAction : MarshalByRefObject
    {
        private IBlobDao BlobDao { get; set; }
        private IContainerDao ContainerDao { get; set; }

        public CreateOrUpdateBlobAction()
        {
            BlobDao = DatabaseContext.CreateDao<IBlobDao>();
            ContainerDao = DatabaseContext.CreateDao<IContainerDao>();
        }

        public CreateOrUpdateBlobResult Execute(CreateOrUpdateBlobParameters parameters)
        {
            CreateOrUpdateBlobResult result = new CreateOrUpdateBlobResult();

            if ((parameters.Blobs == null) || (parameters.Blobs.Count == 0))
            {
                return result;
            }

            using (ITransactionScope scope = DatabaseContext.CreateTransactionScope())
            {
                Blob saveBlob;
                Container container;
                bool addedToList = false;

                var dbContainerList = ContainerDao.FindContainer(parameters.ContainerName);

                if ((dbContainerList == null) || (dbContainerList.Count == 0))
                {
                    container = new Container
                    {
                        LastModified = DateTime.Now.ToUniversalTime(),
                        Name = parameters.ContainerName
                    };

                    container = ContainerDao.SaveOrUpdate(container);
                }
                else
                {
                    container = dbContainerList.First();
                }

                foreach (Blob parameterBlob in parameters.Blobs)
                {
                    // Check if blob already exists
                    var dbBlobList = BlobDao.FindContainerBlob(parameters.ContainerName, parameterBlob.Name);

                    if ((dbBlobList != null) && (dbBlobList.Count > 0))
                    {
                        // Update
                        saveBlob = dbBlobList.First();

                        saveBlob.Data = parameterBlob.Data;

                        if (parameterBlob.MetaData != null)
                        {
                            saveBlob.MetaData.Clear();

                            foreach (BlobMetaData meta in parameterBlob.MetaData)
                            {
                                saveBlob.MetaData.Add(meta);
                            }
                        }
                    }
                    else
                    {
                        // Create
                        saveBlob = parameterBlob;
                        
                        if (container.Blobs == null)
                        {
                            container.Blobs = new List<Blob>();
                        }

                        container.Blobs.Add(saveBlob);
                        addedToList = true;
                    }

                    saveBlob.LastModified = DateTime.Now.ToUniversalTime();
                    saveBlob.Container = container;
                    saveBlob = BlobDao.SaveOrUpdate(saveBlob);
                }

                if (addedToList)
                {
                    container.LastModified = DateTime.Now.ToUniversalTime();
                    ContainerDao.SaveOrUpdate(container);
                }

                scope.Complete();
            }


            return result;
        }
    }
}
