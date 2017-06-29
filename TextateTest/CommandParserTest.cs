using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Textate;

namespace TextateTest
{
    [TestClass]
    public class CommandParserTest
    {
        [TestMethod]
        public void InvalidCommandShouldReturnHelp()
        {
            Scenario("invalid", "help");
        }

        [TestMethod]
        public void CustomUserCommandShortcutShouldMatch()
        {
            Scenario("b", "bike:add");
        }

        [TestMethod]
        public void CustomUserCommandNameShouldMatch()
        {
            Scenario("bike", "bike:add");
        }

        [TestMethod]
        public void CustomUserCommandSubcommandShortcutShouldMatch()
        {
            Scenario("bike x", "bike:remove");
        }

        [TestMethod]
        public void CustomUserCommandSubcommandNameShouldMatch()
        {
            Scenario("bike add", "bike:add");
        }

        [TestMethod]
        public void CustomUserCommandWithInvalidSubcommandShouldReturnHelp()
        {
            Scenario("b invalid", "bike:help");
        }

        private void Scenario(string input, string output)
        {
            Assert.AreEqual(output, string.Join(":", CommandParser.Parse(input)), $"Input: <{input}>");
        }
    }
}
