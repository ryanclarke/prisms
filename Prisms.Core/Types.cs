using System;
using System.Collections.Generic;

namespace Prisms.Core
{
    public enum CommandType
    {
        Note,
        Habit,
        List
    }

    public record Command(CommandType CommandType, string Name, string ShortName)
    {
        public bool Matches(string input) => Name.ToLower() == input.ToLower() || ShortName.ToLower() == input.ToLower();
    };

    public record UserMessage(string UserId, DateTime TimeStamp, string Message);
    public record Shard(string UserId, DateTime TimeStamp, string DataType, string Data);
}