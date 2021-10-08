using System;

namespace Prisms.Core
{
    public class MessageParser
    {
        public static Shard Parse(UserMessage request)
        {
            return new Shard(request.UserId, request.TimeStamp, "note", request.Message);
        }
    }
}