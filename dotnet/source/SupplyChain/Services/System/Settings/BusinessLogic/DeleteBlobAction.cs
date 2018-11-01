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
    public class DeleteBlobAction : MarshalByRefObject
    {
        private IBlobDao BlobDao { get; set; }

        public DeleteBlobAction()
        {
            BlobDao = DatabaseContext.CreateDao<IBlobDao>();
        }

        public DeleteBlobResult Execute(DeleteBlobParameters parameters)
        {
            using (ITransactionScope scope = DatabaseContext.CreateTransactionScope())
            {
                BlobDao.DeleteBlob(parameters.ContainerName, parameters.BlobName);

                scope.Complete();
            }

            return new DeleteBlobResult();
        }
    }
}
