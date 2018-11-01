using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using NHibernate;
using Spring.Data.NHibernate;
using System.Data;
using System.Data.SqlClient;
using NHibernate.Tool.hbm2ddl;
using System.IO;
using System.Data.SqlServerCe;

//
// This code is SQL Server CE specific
//

namespace Imi.SupplyChain.Settings.DataAccess.NHibernate
{
    public class DatabaseCreator : IDatabaseCreator
    {
        public bool CreateDatabase(bool replaceExisting)
        {
            IApplicationContext ctx = ContextRegistry.GetContext();

            ISessionFactory sessionFactory = ctx["SessionFactory"] as ISessionFactory;

            ISession openSession = sessionFactory.OpenSession();

            bool databaseExists = false;

            try
            {
                IDbConnection connection = openSession.Connection;
                databaseExists = true;
            }
            catch (ADOException)
            {
            }
            catch (SqlException)
            {
            }

            LocalSessionFactoryObject sessionFactoryObject = ctx["&SessionFactory"] as LocalSessionFactoryObject;

            if (!databaseExists)
            {
                string connectionString = sessionFactoryObject.Configuration.Properties["connection.connection_string"];

                using (SqlCeEngine en = new SqlCeEngine(connectionString))
                {
                    en.CreateDatabase();
                }

            }

            if ((replaceExisting) || (!databaseExists))
            {
                SchemaExport exporter = new SchemaExport(sessionFactoryObject.Configuration);
                ////  Create database 
                exporter.Create(false, true);
                //    exporter.Execute(true, true, false, true, connection, new StreamWriter("CreateDataBase.ddl.sql"));
            }

            return false;
        }


        public bool DatabaseExists()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();

            ISessionFactory sessionFactory = ctx["SessionFactory"] as ISessionFactory;

            ISession openSession = sessionFactory.OpenSession();

            try
            {
                IDbConnection connection = openSession.Connection;
                return true;
            }
            catch (ADOException)
            {
            }
            catch (SqlException)
            {
            }

            return false;
        }

    }
}
