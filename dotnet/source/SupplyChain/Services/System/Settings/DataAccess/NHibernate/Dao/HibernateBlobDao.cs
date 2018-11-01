using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using Spring.Data.NHibernate.Generic;
using Spring.Data.NHibernate.Generic.Support;
using Imi.SupplyChain.Settings.DataAccess.Dao;
using Imi.SupplyChain.Settings.BusinessEntities;

namespace Imi.SupplyChain.Settings.DataAccess.NHibernate.Dao
{
    public class HibernateBlobDao : HibernateDaoSupport, IBlobDao
    {
        [Transaction(ReadOnly = false)]
        public Blob SaveOrUpdate(Blob blob)
        {
            HibernateTemplate.SaveOrUpdate(blob);
            return blob;
        }


        [Transaction(ReadOnly = false)]
        public IList<Blob> FindContainerBlob(string containerName, string blobName)
        {
            string[] paramNames = { "containerName" , "blobName"};
            object[] paramValues = { containerName , (blobName ?? "%") };

            IList<Blob> result = HibernateTemplate.FindByNamedQueryAndNamedParam<Blob>("Blob.FindContainerBlob", paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = false)]
        public void DeleteBlob(string containerName, string blobName)
        {
            IList<Blob> result = FindContainerBlob(containerName, blobName);

            if (result != null)
            {
                foreach (Blob blob in result)
                {
                    HibernateTemplate.Delete(blob);
                }
            }
        }

    }
}
