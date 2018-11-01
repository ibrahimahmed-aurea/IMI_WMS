using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using NHibernate;
using System.Data;
using NHibernate.Tool.hbm2ddl;
using System.IO;
using Spring.Data.NHibernate;

namespace Cdc.MetaManager.CreateMetaDb
{
    public class DatabaseCreator
    {
        public static void Create(IApplicationContext ctx)
        {
                         
            DDLGenerator generator = ctx["DDLGenerator"] as DDLGenerator;

            ISessionFactory sessionFactory = ctx["SessionFactory"] as ISessionFactory;

            ISession openSession = sessionFactory.OpenSession();

            IDbConnection connection = openSession.Connection;

            SchemaExport e = new SchemaExport(generator.SessionFactory.Configuration);
            e.SetDelimiter(";");
            e.Create(true, false);  // Write DDL to screen
            //  Create database and create DDL file
            e.Execute(false, true, true);
            e.Execute(true, true, false, connection, new StreamWriter("CreateDataBase.ddl.sql"));
        }

        public static void DropAllTables(IApplicationContext ctx) 
        {
            DDLGenerator generator = ctx["DDLGenerator"] as DDLGenerator;

            ISessionFactory sessionFactory = ctx["SessionFactory"] as ISessionFactory;

            ISession openSession = sessionFactory.OpenSession();

            IDbConnection connection = openSession.Connection;

            SchemaExport e = new SchemaExport(generator.SessionFactory.Configuration);
            e.Execute(false, true, true);
        }

    }
}
