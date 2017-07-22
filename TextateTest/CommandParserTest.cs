using NUnit.Framework;
using Textate;

namespace TextateTest
{
    [TestFixture]
    public class CommandParserTest
    {
        [Test]
        public void InvalidCommandShouldReturnHelp()
        {
            Scenario("invalid", "help");
        }

        [Test]
        public void CustomUserCommandShortcutShouldMatch()
        {
            Scenario("b", "bike:add");
        }

        [Test]
        public void CustomUserCommandNameShouldMatch()
        {
            Scenario("bike", "bike:add");
        }

        [Test]
        public void CustomUserCommandSubcommandShortcutShouldMatch()
        {
            Scenario("bike x", "bike:remove");
        }

        [Test]
        public void CustomUserCommandShortcutAndSubcommandShortcutShouldMatch()
        {
            Scenario("b x", "bike:remove");
        }

        [Test]
        public void CustomUserCommandSubcommandNameShouldMatch()
        {
            Scenario("bike add", "bike:add");
        }

        [Test]
        public void CustomUserShortcutAndSubcommandNameShouldMatch()
        {
            Scenario("b add", "bike:add");
        }

        [Test]
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
