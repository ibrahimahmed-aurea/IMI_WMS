using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LCAInterop;

namespace Imi.SupplyChain.Server.Job.OptimizeFillRate.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TestFixture
    {
        public TestFixture()
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
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
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

        private LCASettings GetBasicSettings()
        {
            LCASettings settings = new LCASettings();
            settings.AllowPBRowSplit = false;
            settings.DistanceFactor = 1.5;
            settings.DoBeautyPhase = true;
            settings.DoDistPhase = true;
            settings.DoLCPhase = true;
            settings.GroupId = "TEST";
            settings.MaxLDWgt = 10;
            settings.MaxLDVol = 10;
            settings.MaxmSecBeauty = 60 * 1000;
            settings.MaxmSecDistance = 60 * 1000;
            settings.MaxmSecLC = 60 * 1000;
            settings.MaxPBRowCar = 1000;
            settings.NumberOfIterationsBeauty = 100000;
            settings.NumberOfIterationsDistance = 100000;
            settings.NumberOfIterationsLC = 100000;
            settings.OnlyAptean = false;
            settings.WHId = "TEST_WH";
            settings.PZId = "TEST_PZ";
            settings.StrekArea = "TEST_AREA";
            settings.StrekXCoord = 1;
            settings.StrekYCoord = 1;

            return settings;
        }

        [TestMethod]
        public void LCAInvalidParamtersShouldThrow()
        {
            LCASettings settings = GetBasicSettings();
            settings.MaxLDWgt = -1;
            LCAWrapper lca = new LCAWrapper(settings);

            lca.AddArea("TEST_AREA", 1, 1);

            lca.AddAisle("TEST_AREA", "TEST_AISLE", 1, 1, 1, 1, "+");

            lca.AddPickBatchLine("TEST_PBROW", 0, "TEST_PRODUCT", "TEST_COMPANY", 10, 1, 1, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "", 1, 1, "TEST_WPADR");

            try
            {
                LCAWrapperResult result = lca.Process();
            }
            catch (LCAWrapperException ex)
            {
                if (ex.ErrorCode == 1401) //Parameter out of bounds
                {
                    return;
                }
            }

            Assert.Fail("Parameter out of bounds error expected.");
        }

        [TestMethod]
        public void LCABasicTest()
        {
            LCASettings settings = GetBasicSettings();
                                   
            LCAWrapper lca = new LCAWrapper(settings);
            
            lca.AddArea("TEST_AREA", 1, 1);
            
            lca.AddAisle("TEST_AREA", "TEST_AISLE", 1, 1, 1, 1, "+");

            lca.AddPickBatchLine("TEST_PBROW", 0, "TEST_PRODUCT", "TEST_COMPANY", 10, 1, 1, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            
            LCAWrapperResult result = lca.Process();

            Assert.AreEqual(0, result.Errors.Count);
            Assert.AreEqual(1, result.Lines.Count);
            Assert.AreEqual("TEST_PBROW", result.Lines[0].PBRowId);
            Assert.IsNotNull(result.Lines[0].PBCarIdVirtual);
        }

        [TestMethod]
        public void LCATheoreticalMinimumVolumeTest()
        {
            LCASettings settings = GetBasicSettings();
            settings.DoDistPhase = false;
            settings.DoBeautyPhase = false;

            LCAWrapper lca = new LCAWrapper(settings);

            lca.AddArea("TEST_AREA", 1, 1);

            lca.AddAisle("TEST_AREA", "TEST_AISLE", 1, 1, 1, 1, "+");

            lca.AddPickBatchLine("TEST_PBROW_1", 1, "TEST_PRODUCT", "TEST_COMPANY", 1, 5, 1, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_2", 2, "TEST_PRODUCT", "TEST_COMPANY", 1, 5, 1, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_3", 3, "TEST_PRODUCT", "TEST_COMPANY", 1, 5, 1, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_4", 4, "TEST_PRODUCT", "TEST_COMPANY", 1, 5, 1, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_5", 5, "TEST_PRODUCT", "TEST_COMPANY", 1, 5, 1, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_6", 6, "TEST_PRODUCT", "TEST_COMPANY", 1, 5, 1, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_7", 7, "TEST_PRODUCT", "TEST_COMPANY", 1, 5, 1, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_8", 8, "TEST_PRODUCT", "TEST_COMPANY", 1, 5, 1, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_9", 9, "TEST_PRODUCT", "TEST_COMPANY", 1, 5, 1, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_10", 10, "TEST_PRODUCT", "TEST_COMPANY", 1, 5, 1, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");

            LCAWrapperResult result = lca.Process();

            Assert.AreEqual(0, result.Errors.Count);
            Assert.AreEqual(10, result.Lines.Count);
            var loadCarrierCount = (from x in result.Lines
                                    select x.PBCarIdVirtual).Distinct().Count();
            Assert.AreEqual(5, loadCarrierCount);
        }

        [TestMethod]
        public void LCATheoreticalMinimumWeightTest()
        {
            LCASettings settings = GetBasicSettings();
            settings.DoDistPhase = false;
            settings.DoBeautyPhase = false;

            LCAWrapper lca = new LCAWrapper(settings);

            lca.AddArea("TEST_AREA", 1, 1);

            lca.AddAisle("TEST_AREA", "TEST_AISLE", 1, 1, 1, 1, "+");

            lca.AddPickBatchLine("TEST_PBROW_1", 1, "TEST_PRODUCT", "TEST_COMPANY", 1, 1, 5, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_2", 2, "TEST_PRODUCT", "TEST_COMPANY", 1, 1, 5, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_3", 3, "TEST_PRODUCT", "TEST_COMPANY", 1, 1, 5, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_4", 4, "TEST_PRODUCT", "TEST_COMPANY", 1, 1, 5, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_5", 5, "TEST_PRODUCT", "TEST_COMPANY", 1, 1, 5, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_6", 6, "TEST_PRODUCT", "TEST_COMPANY", 1, 1, 5, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_7", 7, "TEST_PRODUCT", "TEST_COMPANY", 1, 1, 5, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_8", 8, "TEST_PRODUCT", "TEST_COMPANY", 1, 1, 5, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_9", 9, "TEST_PRODUCT", "TEST_COMPANY", 1, 1, 5, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");
            lca.AddPickBatchLine("TEST_PBROW_10", 10, "TEST_PRODUCT", "TEST_COMPANY", 1, 1, 5, "TEST_AREA", "TEST_AISLE", "TEST_PRODUCT_GROUP", "TEST_CAT_GROUP", 1, 1, "TEST_WPADR");

            LCAWrapperResult result = lca.Process();

            Assert.AreEqual(0, result.Errors.Count);
            Assert.AreEqual(10, result.Lines.Count);
            var loadCarrierCount = (from x in result.Lines
                                    select x.PBCarIdVirtual).Distinct().Count();
            Assert.AreEqual(5, loadCarrierCount);
        }


        [TestMethod]
        public void StringMarshallingTest()
        {
            LCAWrapper lca = new LCAWrapper(GetBasicSettings());
            string str = "test";
            string result = lca.StringMarshallingTest(str);
            Assert.AreEqual(str, result);
        }
    }
}
