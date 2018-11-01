using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface ISchemaDao
    {
        Schema FindById(Guid schemaId);
        Schema FindByApplicationId(Guid applicationId);
        IList<Schema> FindAll();
        Schema Save(Schema schema);
        Schema SaveOrUpdate(Schema schema);
        Schema SaveOrUpdateMerge(Schema schema);
        void Delete(Schema schema);
    }
}

