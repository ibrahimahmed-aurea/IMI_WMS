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
    public class FindBlobAction : MarshalByRefObject
    {
        private IBlobDao BlobDao { get; set; }

        public FindBlobAction()
        {
            BlobDao = DatabaseContext.CreateDao<IBlobDao>();
        }

        public FindBlobResult Execute(FindBlobParameters parameters)
        {
            using (ITransactionScope scope = DatabaseContext.CreateTransactionScope())
            {
                var result = new FindBlobResult();

                var blobs = BlobDao.FindContainerBlob(parameters.ContainerName, parameters.BlobName);

                if ((blobs != null) && (blobs.Count > 0))
                {
                    result.AddRange(blobs);
                }

                scope.Complete();

                return result;
            }

            
        }
    }
}
