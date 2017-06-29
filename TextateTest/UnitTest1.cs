using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Textate;

namespace TextateTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Scenario("b", "bike:add");
        }

        [TestMethod]
        public void TestMethod2()
        {
            Scenario("b invalid", "bike:help");
        }

        [TestMethod]
        public void TestMethod3()
        {
            Scenario("invalid", "help");
        }

        private void Scenario(string input, string output)
        {
            Assert.AreEqual(output, string.Join(":", CommandParser.Parse(input)));
        }
    }
}
