using NUnit.Framework;
using Textate;

namespace TextateTest
{
    [TestFixture]
    public class CommandParserTest
    {
        [Test]
        [TestCase("bike add", "bike:add")]
        [TestCase("bike delete", "bike:delete")]
        [TestCase("bike view", "bike:view")]
        [TestCase("bike add 8/3", "bike:add:2017-08-03")]
        [TestCase("bible add", "bible:add")]
        [TestCase("bible delete", "bible:delete")]
        [TestCase("bible view", "bible:view")]
        [TestCase("movie add", "movie:add")]
        [TestCase("movie delete", "movie:delete")]
        [TestCase("movie view", "movie:view")]
        [TestCase("movie add Monsters, Inc.", "movie:add:Monsters, Inc.")]
        public void Test(string input, string output)
        {
            Scenario(input, output);
        }

        private void Scenario(string input, string output)
        {
            Assert.That(CommandParser.Parse(input).Stringify(), Is.EqualTo(output), $"Input: <{input}>");
        }
    }
}
