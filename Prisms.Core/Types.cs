namespace Prisms.Core;

public enum CommandType
{
    Note,
    Date,
    List
}

public record Command(CommandType CommandType, string Name, string ShortName)
{
    public bool Matches(string input) => Name.ToLower() == input.ToLower() || ShortName.ToLower() == input.ToLower();
    public static Command DefaultNote() => new Command(CommandType.Note, "note", "n");
};

public record UserMessage(string UserId, DateTime TimeStamp, string Message);
public record Shard(string UserId, DateTime TimeStamp, string DataType, string Data);
public record Query(string UserId, Command Command, DateTime ReferenceDate);