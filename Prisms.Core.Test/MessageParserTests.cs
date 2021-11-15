namespace Prisms.Core.Test;

public class MessageParserTests
{
    private static readonly string _userId = "+12223334444";
    private static readonly DateTime _now = DateTime.Now;
    private readonly UserMessage _userMessage = new(_userId, _now, "");
    private readonly Shard _shard = new(_userId, _now, "", "");

    [Fact]
    public void ParsesDefaultMessageWithNoUserCommands()
    {
        var userMessage = _userMessage with { Message = "hello" };
        var expectedShard = _shard with
        {
            DataType = "note",
            Data = "hello"
        };

        MessageParser.Parse(new List<Command>(), userMessage).Should().Be(expectedShard);
    }

    [Theory]
    [InlineData("thing. hello")]
    [InlineData("t. hello")]
    [InlineData("ThinG. hello")]
    [InlineData("T. hello")]
    public void ParsesMessagesWithCommandSentence(string message)
    {
        var userMessage = _userMessage with { Message = message };
        var userCommands = new List<Command> { new Command(CommandType.Note, "thing", "t") };
        var expectedShard = _shard with
        {
            DataType = "thing",
            Data = "hello"
        };

        MessageParser.Parse(userCommands, userMessage).Should().Be(expectedShard, message);
    }

    [Theory]
    [InlineData("movie")]
    [InlineData("movie.")]
    [InlineData("movie. ")]
    [InlineData("m")]
    [InlineData("m.")]
    [InlineData("m. ")]
    [InlineData("MoVie")]
    [InlineData("M")]
    public void ParsesListTypeMessagesWithoutContent(string message)
    {
        var userMessage = _userMessage with { Message = message };
        var userCommands = new List<Command> { new Command(CommandType.List, "movie", "m") };
        var expectedShard = _shard with
        {
            DataType = "movie",
            Data = ""
        };

        MessageParser.Parse(userCommands, userMessage).Should().Be(expectedShard, message);
    }

    [Theory]
    [InlineData("movie.hello")]
    [InlineData("movie. hello")]
    [InlineData("m. hello")]
    [InlineData("MoVie. hello")]
    [InlineData("M. hello")]
    public void ParsesListTypeMessagesWithCotent(string message)
    {
        var userMessage = _userMessage with { Message = message };
        var userCommands = new List<Command> { new Command(CommandType.List, "movie", "m") };
        var expectedShard = _shard with
        {
            DataType = "movie",
            Data = "hello"
        };

        MessageParser.Parse(userCommands, userMessage).Should().Be(expectedShard, message);
    }
}
