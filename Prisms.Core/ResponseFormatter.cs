namespace Prisms.Core;

public class ResponseFormatter
{
    private static readonly string[] iosNumbersWhite = new string[] { "\U00100529", "\U0010052a", "\U0010052b", "\U0010052c", "\U0010052d", "\U0010052e", "\U0010052f", "\U00100530", "\U00100531", "\U00100532", "\U00100533", "\U00100534", "\U00100535", "\U00100536", "\U00100537", "\U00100538", "\U00100539", "\U0010053a", "\U0010053b", "\U0010053c", "\U0010053d", "\U0010053e", "\U0010053f", "\U00100540", "\U00100541", "\U00100542", "\U00100543", "\U00100544", "\U00100545", "\U00100546", "\U00100547", "\U00100622" };
    private static readonly string[] iosNumbersBlack = new string[] { "\U00100548", "\U00100549", "\U0010054a", "\U0010054b", "\U0010054c", "\U0010054d", "\U0010054e", "\U0010054f", "\U00100550", "\U00100551", "\U00100552", "\U00100553", "\U00100554", "\U00100555", "\U00100556", "\U00100557", "\U00100558", "\U00100559", "\U0010055a", "\U0010055b", "\U0010055c", "\U0010055d", "\U0010055e", "\U0010055f", "\U00100560", "\U00100561", "\U00100562", "\U00100563", "\U00100564", "\U00100565", "\U00100566", "\U00100623" };
    private static readonly string SMTWTFS = "\U001000b8\U001000ac\U001000ba\U001000c0\U001000ba\U0010009e\U001000b8";

    public static Result Format(Query query, Shard[] shards) =>
        query.Command.CommandType switch
        {
            CommandType.List => ListResponse(shards),
            CommandType.Date => DateResponse(query.ReferenceDate, shards),
            CommandType.Note => NoteResponse(shards),
            _ => throw new ArgumentException($"{query}", nameof(query))
        } switch
        {
            string { Length: 0 } _ => new Result.Success(),
            string s => new Result.Response(s)
        };

    private static string ListResponse(Shard[] shards) => shards
            .OrderBy(s => s.TimeStamp)
            .Select((s, i) => $"{i + 1}. {s.Data}")
            .JoinLines();

    private static string DateResponse(DateTime referenceDate, Shard[] shards)
    {
        var futureDays = Enumerable.Repeat("\U001004d4", 6);

        var dates = shards.Select(s => s.TimeStamp.Date).Distinct();

        return Enumerable.Range(0, 29 + (int)referenceDate.DayOfWeek)
            .Reverse()
            .Select(i => referenceDate.Date.AddDays(-i))
            .Select(d => dates.Contains(d)
                            ? iosNumbersBlack[d.Day]
                            : iosNumbersWhite[d.Day])
            .Chunk(7)
            .Select(ss => ss.Concat(futureDays).Take(7).JoinStrings())
            .Prepend(SMTWTFS)
            .JoinLines();
    }

    private static string NoteResponse(Shard[] shards) => shards
            .OrderBy(s => s.TimeStamp)
            .GroupBy(s => $"{s.TimeStamp:yyyy-MM-dd ddd}")
            .Select(g => g.Select(s => $"{s.TimeStamp:t}: {s.Data}").Prepend($"[{g.Key}]").JoinLines())
            .JoinLines(2);
}