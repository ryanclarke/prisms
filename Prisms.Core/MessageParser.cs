namespace Prisms.Core;

public class MessageParser
{
    private const StringSplitOptions RemoveEmptyAndTrimEntries = StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries;

    public static Shard Parse(IEnumerable<Command> userCommands, UserMessage request)
    {
        var split = request.Message.Split('.', 2, RemoveEmptyAndTrimEntries);

        var directive = split.Length >= 1 ? split[0].Trim() : "";
        var rest = split.Length >= 2 ? split[1].Trim() : "";

        var cmd = userCommands.FirstOrDefault(c => c.Matches(directive));

        if (cmd is null || string.IsNullOrWhiteSpace(directive))
        {
            return new Shard(request.UserId, request.TimeStamp, "note", request.Message);
        }

        return cmd.CommandType switch
        {
            CommandType.Note => new Shard(request.UserId, request.TimeStamp, cmd.Name, rest),
            CommandType.List => new Shard(request.UserId, request.TimeStamp, cmd.Name, rest),
            CommandType.Date => new Shard(request.UserId, request.TimeStamp.Date, cmd.Name, rest),
            _ => throw new NotImplementedException()
        };
    }
}
