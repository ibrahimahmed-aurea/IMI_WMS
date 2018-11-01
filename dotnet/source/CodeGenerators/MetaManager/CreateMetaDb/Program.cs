using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Spring.Context;
using System.IO;
using Spring.Context.Support;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using System.Collections;
using Cdc.MetaManager.BusinessLogic;

namespace Cdc.MetaManager.CreateMetaDb
{
    public class Program
    {
        static void Main(string[] args)
        {
            IApplicationContext ctx;
            ctx = ContextRegistry.GetContext();
            DatabaseCreator.Create(ctx);
        }

        public static void MakeDb()
        {
            IApplicationContext ctx;
            ctx = ContextRegistry.GetContext();
            DatabaseCreator.Create(ctx);
        }

    }
}
