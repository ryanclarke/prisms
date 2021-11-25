namespace Prisms.Core.Test;

public class ResponseFormatterTests
{
    private static readonly string _userId = "+12223334444";
    private static readonly DateTime _baseTime = new(2021, 6, 5, 4, 3, 2, 1);

    [Fact]
    public void WritesAndFormatsNoteMessages()
    {
        ResponseFormatter.Format(
            new Query(_userId, new Command(CommandType.Note, "note", "n")), 
            new[]
            {
                new Shard(_userId, _baseTime.AddHours(1), "note", "message 2"),
                new Shard(_userId, _baseTime.AddDays(1),  "note", "message 3"),
                new Shard(_userId, _baseTime,             "note", "message 1")
            })
            .Should().BeOfType<Result.Response>()
            .Which.Content.Should()
                .Be(@"
[2021-06-05 Sat]
4:03 AM: message 1
5:03 AM: message 2

[2021-06-06 Sun]
4:03 AM: message 3".Trim());
    }

    [Fact]
    public void WritesAndFormatsListMessages()
    {
        ResponseFormatter.Format(
            new Query(_userId, new Command(CommandType.List, "movie", "m")),
            new[]
            {
                new Shard(_userId, _baseTime.AddHours(1), "movie", "Movie 1"),
                new Shard(_userId, _baseTime.AddDays(1),  "movie", "Movie 2"),
                new Shard(_userId, _baseTime,             "movie", "Movie 3")
            })
            .Should().BeOfType<Result.Response>()
            .Which.Content.Should()
                .Be(@"
1. Movie 3
2. Movie 1
3. Movie 2".Trim());
    }

    [Fact]
    public void WritesAndFormatsDateMessages()
    {
        ResponseFormatter.Format(
            new Query(_userId, new Command(CommandType.Date, "bike", "b")),
            new[]
            {
                new Shard(_userId, _baseTime.AddHours(1), "bike", ""),
                new Shard(_userId, _baseTime.AddDays(1),  "bike", "A comment"),
                new Shard(_userId, _baseTime,             "bike", ""),
                new Shard(_userId, _baseTime.AddDays(8),  "bike", "")
            })
            .Should().BeOfType<Result.Response>()
            .Which.Content.Should()
                .Be(@"
2021-06-05 Sat
2021-06-06 Sun
2021-06-13 Sun".Trim());
    }
}
