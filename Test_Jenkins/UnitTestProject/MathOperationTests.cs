using NUnit.Framework;
using Test_WindowsForms;

namespace UnitTestProject
{
    [TestFixture]
    public class MathOperationTests
    {
        [Test]
        public void Add_Positive_ReturnSum()
        {
            int reuslt = MathOperations.Add(5, 6);
            Assert.AreEqual(reuslt, 11);
        }

        [Test]
        public void Add_Negative_ReturnSum()
        {
            int reuslt = MathOperations.Add(-5, -6);
            Assert.AreEqual(reuslt, -11);
        }
    }
}
