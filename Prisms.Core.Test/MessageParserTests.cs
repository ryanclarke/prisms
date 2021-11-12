using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

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
}
