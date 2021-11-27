namespace Prisms.Core.Test;

public class ResponseFormatterTests
{
    private static readonly string _userId = "+12223334444";
    private static readonly DateTime _baseTime = new(2021, 6, 14, 4, 3, 2, 1);

    [Fact]
    public void FormatsEmptyNoteMessage()
    {
        ResponseFormatter.Format(
            new Query(_userId, new Command(CommandType.Note, "note", "n"), _baseTime),
            Array.Empty<Shard>())
            .Should().BeOfType<Result.Success>();
    }

    [Fact]
    public void FormatsNoteMessages()
    {
        ResponseFormatter.Format(
            new Query(_userId, new Command(CommandType.Note, "note", "n"), _baseTime), 
            new[]
            {
                new Shard(_userId, _baseTime.AddHours(-1), "note", "message 2"),
                new Shard(_userId, _baseTime.AddDays(-1),  "note", "message 3"),
                new Shard(_userId, _baseTime,             "note", "message 1")
            })
            .Should().BeOfType<Result.Response>()
            .Which.Content.Should()
                .Be(@"
[2021-06-13 Sun]
4:03 AM: message 3

[2021-06-14 Mon]
3:03 AM: message 2
4:03 AM: message 1".Trim());
    }

    [Fact]
    public void FormatsEmptyListMessage()
    {
        ResponseFormatter.Format(
            new Query(_userId, new Command(CommandType.List, "movie", "m"), _baseTime),
            Array.Empty<Shard>())
            .Should().BeOfType<Result.Success>();
    }

    [Fact]
    public void FormatsListMessages()
    {
        ResponseFormatter.Format(
            new Query(_userId, new Command(CommandType.List, "movie", "m"), _baseTime),
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
    public void FormatsEmptyDateMessage()
    {
        ResponseFormatter.Format(
            new Query(_userId, new Command(CommandType.Date, "bike", "b"), _baseTime),
            Array.Empty<Shard>())
            .Should().BeOfType<Result.Response>()
            .Which.Content.Should()
                .Be(@"
􀂸􀂬􀂺􀃀􀂺􀂞􀂸
􀔹􀔺􀔻􀔼􀔽􀔾􀔿
􀕀􀕁􀕂􀕃􀕄􀕅􀕆
􀕇􀘢􀔪􀔫􀔬􀔭􀔮
􀔯􀔰􀔱􀔲􀔳􀔴􀔵
􀔶􀔷􀓔􀓔􀓔􀓔􀓔".Trim());
    }

    [Fact]
    public void FormatsDateMessages()
    {
        ResponseFormatter.Format(
            new Query(_userId, new Command(CommandType.Date, "bike", "b"), _baseTime),
            new[]
            {
                new Shard(_userId, _baseTime.AddDays(-8), "bike", "A comment"),
                new Shard(_userId, _baseTime.AddDays(-9), "bike", ""),
                new Shard(_userId, _baseTime.AddDays(-1), "bike", "")
            })
            .Should().BeOfType<Result.Response>()
            .Which.Content.Should()
                .Be(@"
􀂸􀂬􀂺􀃀􀂺􀂞􀂸
􀔹􀔺􀔻􀔼􀔽􀔾􀔿
􀕀􀕁􀕂􀕃􀕄􀕅􀕆
􀕇􀘢􀔪􀔫􀔬􀔭􀕍
􀕎􀔰􀔱􀔲􀔳􀔴􀔵
􀕕􀔷􀓔􀓔􀓔􀓔􀓔".Trim());
    }
}
