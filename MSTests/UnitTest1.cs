using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unit;

namespace MSTests
{
    [TestClass]
    public class Inherit
    {
        [TestMethod]
        public void AisA ()
        {
            var x = new A();
            Assert.IsInstanceOfType(x, typeof(A));
        }
        [TestMethod]
        public void BisA()
        {
            var y = new B();
            Assert.IsInstanceOfType(y, typeof(A));
        }
        [TestMethod]
        public void AisNotB()
        {
            var x = new A();
            Assert.IsNotInstanceOfType(x, typeof(B));
        }
        [TestMethod]
        public void AisIA()
        {
            var x = new A();
            Assert.IsInstanceOfType(x, typeof(IA));
        }
    }
}
