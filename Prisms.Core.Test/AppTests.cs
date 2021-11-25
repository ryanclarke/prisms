namespace Prisms.Core.Test;

public class AppTests
{
    private readonly App _app;
    private readonly InMemoryDatabase _database;
    private static readonly string _userId = "+12223334444";
    private static readonly DateTime _baseTime = new(2021, 6, 5, 4, 3, 2, 1);
    private readonly UserMessage _userMessage = new(_userId, _baseTime, "");

    public AppTests()
    {
        _database = new InMemoryDatabase();
        _app = App.Create(_database);
    }

    [Fact]
    public async Task WritesMessageToStorageAsync()
    {
        (await _app.ProcessAsync(_userMessage with { Message = "message" }))
            .Should().BeOfType<Result.Response>()
            .Which.Content.Should()
                .Be(@"
[2021-06-05 Sat]
4:03 AM: message".Trim());
    }

    [Fact]
    public async Task WritesAndFormatsNoteMessages()
    {
        (await _app.ProcessAsync(_userMessage with { Message = "message 1" }))
            .Should().BeOfType<Result.Response>()
            .Which.Content.Should()
                .Be(@"
[2021-06-05 Sat]
4:03 AM: message 1".Trim());

        (await _app.ProcessAsync(_userMessage with { TimeStamp = _baseTime.AddHours(1), Message = "message 2" }))
            .Should().BeOfType<Result.Response>()
            .Which.Content.Should()
                .Be(@"
[2021-06-05 Sat]
4:03 AM: message 1
5:03 AM: message 2".Trim());

        (await _app.ProcessAsync(_userMessage with { TimeStamp = _baseTime.AddDays(1), Message = "message 3" }))
            .Should().BeOfType<Result.Response>()
            .Which.Content.Should()
                .Be(@"
[2021-06-05 Sat]
4:03 AM: message 1
5:03 AM: message 2

[2021-06-06 Sun]
4:03 AM: message 3".Trim());
    }
}
