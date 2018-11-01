using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spring.Context;
using Spring.Data.Support;
using Spring.Context.Support;
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.CreateMetaDb;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess;
using System.Collections;

namespace Cdc.MetaManager.TestSchema
{
    public class DoDa : IValueConverter
    {
    }
    public class DoDa2 : IValueCalculator
    {
    }

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class BasicTests
    {
        public BasicTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class

        private static IApplicationContext ctx;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            ctx = ContextRegistry.GetContext();

            //IPlatformTransactionManager transactionManager = ctx["HibernateTransactionManager"] as IPlatformTransactionManager;
            //tt = new TransactionTemplate(transactionManager);
        }

        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreateDatabase()
        {
            try
            {
                DatabaseCreator.Create(ctx);
            }
            catch (Exception e)
            {
                Assert.Fail(String.Format("CreateMetaDb fails with exception: {0}", e.Message));
            }
        }

        [TestMethod]
        public void DropDatabase()
        {
            try
            {
                DatabaseCreator.DropAllTables(ctx);
            }
            catch (Exception e)
            {
                Assert.Fail(String.Format("DropDatabase fails with exception: {0}", e.Message));
            }
        }

        [TestMethod]
        public void CreateUXApplicationWithService()
        {
            IApplicationService appService = ctx["ApplicationService"] as IApplicationService;
            Guid id = appService.CreateUXApplication();
            UXApplication app = appService.ReadUXApplication(id);
        }


        [TestMethod]
        public void ReadDialogWithTreeView()
        {
            IDialogService dialogService = ctx["DialogService"] as IDialogService;
            Dialog dialog = dialogService.GetDialogWithViewTree(new Guid("F2E585CE-1B5B-456D-866C-690DF46F0BA7"));
        }


        [TestMethod]
        public void CreateApplication()
        {

            try
            {
                IApplicationDao applicationDao = ctx["ApplicationDao"] as IApplicationDao;
                Assert.IsNotNull(applicationDao);

                try
                {
                    // Create the application
                    Application application = new Application();
                    application.Name = "Phoenix Backend";
                    application.Namespace = "Cdc.SupplyChain.Transportation";
                    Application applicationNew = applicationDao.Save(application);
                    Assert.IsNotNull(application.Id);
                    Assert.AreEqual(application.Name, applicationNew.Name);
                    Assert.AreEqual(application.Namespace, applicationNew.Namespace);
                }
                catch (Exception e)
                {
                    Assert.Fail(String.Format("Failed to create Application fails with exception: {0}", e.Message));
                }

            }
            catch (Exception e)
            {
                Assert.Fail(String.Format("Failed to aquire ApplicationDao fails with exception: {0}", e.Message));
            }
        }

        [TestMethod]
        public void CreateApplicationVersion()
        {

            try
            {
                IApplicationVersionDao applicationVersionDao = ctx["ApplicationVersionDao"] as IApplicationVersionDao;
                Assert.IsNotNull(applicationVersionDao);
                IApplicationDao applicationDao = ctx["ApplicationDao"] as IApplicationDao;
                Assert.IsNotNull(applicationDao);

                try
                {
                    // Create the Application Version
                    ApplicationVersion applicationVersion = new ApplicationVersion();

                    Application applicationFind = null;
                    try
                    {
                        // Read first application from database
                        applicationFind = applicationDao.FindById(new Guid("8BAAC282-E064-E011-8703-782BCB8968B9"));
                        Assert.IsNotNull(applicationFind);
                    }
                    catch (Exception e)
                    {
                        Assert.Fail(String.Format("Failed to find Application with Id = 1, failed with exception: {0}", e.Message));
                    }

                    applicationVersion.Application = applicationFind;
                                        
                    applicationVersion.Name = "Beta v0.1.0";
                    applicationVersion.Major = 0;
                    applicationVersion.Minor = 1;
                    applicationVersion.Build = 0;

                    ApplicationVersion applicationVersionNew = applicationVersionDao.Save(applicationVersion);
                    Assert.IsNotNull(applicationVersion.Id);
                    Assert.AreEqual(applicationVersion.Name, applicationVersionNew.Name);
                    Assert.AreEqual(applicationVersion.Major, applicationVersionNew.Major);
                    Assert.AreEqual(applicationVersion.Minor, applicationVersionNew.Minor);
                    Assert.AreEqual(applicationVersion.Build, applicationVersionNew.Build);
                }
                catch (Exception e)
                {
                    Assert.Fail(String.Format("Failed to create ApplicationVersion fails with exception: {0}", e.Message));
                }

            }
            catch (Exception e)
            {
                Assert.Fail(String.Format("Failed to aquire ApplicationVersionDao or ApplicationDao, fails with exception: {0}", e.Message));
            }
        }

        [TestMethod]
        public void CreateSchema()
        {

            try
            {
                ISchemaDao schemaDao = ctx["SchemaDao"] as ISchemaDao;
                Assert.IsNotNull(schemaDao);
                IApplicationVersionDao applicationVersionDao = ctx["ApplicationVersionDao"] as IApplicationVersionDao;
                Assert.IsNotNull(applicationVersionDao);

                try
                {
                    // Create the Schema
                    Schema schema = new Schema();

                    ApplicationVersion applicationVersionFind = null;
                    try
                    {
                        // Read first application from database
                        applicationVersionFind = applicationVersionDao.FindById(new Guid("8DAAC282-E064-E011-8703-782BCB8968B9"));
                        Assert.IsNotNull(applicationVersionFind);
                    }
                    catch (Exception e)
                    {
                        Assert.Fail(String.Format("Failed to find ApplicationVersion with Id = 1, failed with exception: {0}", e.Message));
                    }

                    schema.ApplicationVersion = applicationVersionFind;
                    schema.Name = "RMUSER";
                    // Connectionstring to fetch all ORACLE stuff from
                    schema.ConnectionString = "data source=WH51M;user id=rmuser;password=rmuser;pooling=true;enlist=false";


                    Schema schemaNew = schemaDao.Save(schema);
                    Assert.IsNotNull(schema.Id);
                    Assert.AreEqual(schema.Name, schemaNew.Name);
                    Assert.AreEqual(schema.ConnectionString, schemaNew.ConnectionString);
                }
                catch (Exception e)
                {
                    Assert.Fail(String.Format("Failed to create Schema fails with exception: {0}", e.Message));
                }

            }
            catch (Exception e)
            {
                Assert.Fail(String.Format("Failed to aquire SchemaDao or ApplicationVersionDao, fails with exception: {0}", e.Message));
            }
        }

        [TestMethod]
        public void ModifyApplication()
        {

            using (new SessionScope())
            {
                try
                {
                    IApplicationDao applicationDao = ctx["ApplicationDao"] as IApplicationDao;
                    Assert.IsNotNull(applicationDao);

                    try
                    {
                        // Create application
                        Application application = new Application();
                        application.Name = "Second Application";
                        application.Namespace = "Cdc.Strange.Namespace";
                        Application applicationNew = applicationDao.Save(application);

                        Assert.IsNotNull(application.Id);
                        Assert.AreEqual(application.Name, applicationNew.Name);
                        Assert.AreEqual(application.Namespace, applicationNew.Namespace);

                        // Read application from database
                        Application applicationFind = applicationDao.FindById(applicationNew.Id);

                        Assert.IsNotNull(applicationFind);
                        Assert.AreEqual(applicationFind.Id, applicationNew.Id);
                        Assert.AreEqual(applicationFind.Name, applicationNew.Name);

                        // Change and save
                        applicationFind.Name = "New Filename";
                        applicationDao.Save(applicationFind);

                        // Read and compare
                        Application applicationFind2 = applicationDao.FindById(applicationNew.Id);
                        Assert.AreEqual(applicationFind2.Name, "New Filename");
                    }
                    catch (Exception e)
                    {
                        Assert.Fail(String.Format("Failed to create Application fails with exception: {0}", e.Message));
                    }

                }
                catch (AssertFailedException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    Assert.Fail(String.Format("Failed to aquire ApplicationDao fails with exception: {0}", e.Message));
                }
            }

        }

        [TestMethod]
        public void CreateApplicationWithService()
        {
            IApplicationService appService = ctx["ApplicationService"] as IApplicationService;
            appService.CreateApplication();
        }
        
        [TestMethod]
        public void ListApplicationsWithService()
        {
            IApplicationService appService = ctx["ApplicationService"] as IApplicationService;
            appService.ListApplications();
        }

        [TestMethod]
        public void AddOneMoreStoredProcedureWithService()
        {
            IApplicationService appService = ctx["ApplicationService"] as IApplicationService;
            appService.AddOneMoreStoredProcedure(new Guid("7348D284-E064-E011-8703-782BCB8968B9"));
        }



        [TestMethod]
        public void TestMappedProperty()
        {
            MappedProperty property = new MappedProperty();
            property.ValueConverter = new DoDa();

            Assert.IsNotNull(property.ValueConverterTypeName);

            property.ValueConverter = null;
            Assert.IsNull(property.ValueConverterTypeName);

            property.ValueConverterTypeName = new DoDa().GetType().AssemblyQualifiedName;
            Assert.IsNotNull(property.ValueConverter);

            property.ValueCalculator = new DoDa2();

            Assert.IsNotNull(property.ValueCalculatorTypeName);

            property.ValueCalculator = null;
            Assert.IsNull(property.ValueCalculatorTypeName);

            property.ValueCalculatorTypeName = new DoDa2().GetType().AssemblyQualifiedName;
            Assert.IsNotNull(property.ValueCalculator);

        }

    }
}
