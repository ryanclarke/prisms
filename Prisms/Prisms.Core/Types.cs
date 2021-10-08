using System;

namespace Prisms.Core
{
    public record UserMessage(string UserId, DateTime TimeStamp, string Message);
    public record Shard(string UserId, DateTime TimeStamp, string DataType, string Data);
}