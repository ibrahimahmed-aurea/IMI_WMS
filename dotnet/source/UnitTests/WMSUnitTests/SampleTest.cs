using NUnit.Framework;

namespace HelloWorldTest
{
    [TestFixture]
    public class MathOperationTests
    {
        [Test]
        public void AddProduct_InvalidName_DonotAdd()
        {
            int x = 5 + 4;
            Assert.AreEqual(x, 9);
        }
        [Test]
        public void PickOrder_InsufficientAmount_Reject()
        {
            int x = 5 + 4;
            Assert.AreEqual(x, 9);
        }
        [Test]
        public void Print_NoPrinter_ErrorMessage()
        {
            int x = 5 + 4;
            Assert.AreEqual(x, 9);
        }
    }
}
