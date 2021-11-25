namespace Prisms.Core;

public class ResponseFormatter
{
    public static Result Format(Query query, Shard[] shards)
    {
        return query.Command.CommandType switch
        {
            CommandType.List => ListResponse(shards),
            CommandType.Date => DateResponse(shards),
            CommandType.Note => NoteResponse(shards),
            _ => throw new ArgumentException($"{query}", nameof(query))
        } switch
        {
            string { Length: 0 } _ => new Result.Success(),
            string s =>  new Result.Response(s)
        };
    }

    private static string ListResponse(Shard[] shards)
    {
        if (!shards.Any())
        {
            return "";
        }

        return string.Join(Environment.NewLine, shards
            .OrderBy(s => s.TimeStamp)
            .Select((s, i) => $"{i + 1}. {s.Data}"));
    }

    private static string DateResponse(Shard[] shards)
    {
        if (!shards.Any())
        {
            return "";
        }

        return string.Join(Environment.NewLine, shards
            .OrderBy(s => s.TimeStamp)
            .GroupBy(s => $"{s.TimeStamp:yyyy-MM-dd ddd}")
            .Select(g => g.Key));
    }

    private static string NoteResponse(Shard[] shards)
    {
        if (!shards.Any())
        {
            return "";
        }

        return string.Join(Environment.NewLine + Environment.NewLine, shards
            .OrderBy(s => s.TimeStamp)
            .GroupBy(s => $"{s.TimeStamp:yyyy-MM-dd ddd}")
            .Select(g => $"[{g.Key}]{Environment.NewLine}{string.Join(Environment.NewLine, g.Select(s => $"{s.TimeStamp:t}: {s.Data}"))}"));
    }
}