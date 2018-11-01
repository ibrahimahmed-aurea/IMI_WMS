using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Spring.Context;
using Spring.Context.Support;
using Cdc.SupplyChain.Settings.DataAccess;
using Cdc.SupplyChain.Settings.DataAccess.Dao;
using Cdc.SupplyChain.Settings.BusinessEntities;

namespace TestIt
{
    public class Program
    {
        static void Main(string[] args)
        {
            IApplicationContext ctx = ContextRegistry.GetContext();

            IDatabaseCreator dc = ctx["IDatabaseCreator"] as IDatabaseCreator;
            dc.CreateDatabase(false);

//            IBlobDao blobDao = DatabaseContext.CreateDao<IBlobDao>();
//            IContainerDao containerDao = DatabaseContext.CreateDao<IContainerDao>();

//            Container cont = new Container()
//            {
////                ContainerIdentity = Guid.NewGuid().ToString(),
//                LastModified = DateTime.Now.ToUniversalTime(),
//                Name = @"/Shell/Settings",
//                Version = "1.0.0.0"
//            };

//            cont = containerDao.SaveOrUpdate(cont);
        }
    }
}
