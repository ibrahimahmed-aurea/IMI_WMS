using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test_WindowsForms;

namespace UnitTestProject
{
    [TestClass]
    public class MathOperationTests
    {
        [TestMethod]
        public void Add_Positive_ReturnSum()
        {
            int reuslt = MathOperations.Add(5, 6);
            Assert.AreEqual(reuslt, 11);
        }

        [TestMethod]
        public void Add_Negative_ReturnSum()
        {
            int reuslt = MathOperations.Add(-5, -6);
            Assert.AreEqual(reuslt, -11);
        }
    }
}
