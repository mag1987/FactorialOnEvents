using Microsoft.VisualStudio.TestTools.UnitTesting;
using Factorial;
using System;
using System.Reflection;

namespace MSTestFactorial
{
    [TestClass]
    public class FactorialTest
    {
        [TestMethod]
        public void InitializedEventEmission()
        {
            Factorial.Factorial f = new Factorial.Factorial(3);
            f.Initialized += (fact) => { throw new EventRaisedExeption(); };
            Assert.ThrowsException<EventRaisedExeption>(() => { f.Start(); });
        }
        [TestMethod]
        public void InitializedEventSignature()
        {
            Factorial.Factorial f = new Factorial.Factorial(3);
            Assert.AreEqual(
                typeof(FactorialStateHandler).Name,
                f.GetType().GetEvent("Initialized").EventHandlerType.Name
                );
        }
    }
    public class EventRaisedExeption : Exception
    {
        public EventRaisedExeption() : base() { }
    }
}
