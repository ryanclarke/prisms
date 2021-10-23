using System;
using System.Collections.Generic;
using System.Linq;

namespace Prisms.Core
{
    public class MessageParser
    {
        private const StringSplitOptions RemoveEmptyAndTrimEntries = StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries;

        public static Shard Parse(IEnumerable<Command> userCommands, UserMessage request)
        {
            var split = request.Message.Split('.', 2, RemoveEmptyAndTrimEntries);
            
            var inputs = split
                .FirstOrDefault()
                .ToLower()
                .Split(' ', RemoveEmptyAndTrimEntries);

            var cmd = userCommands.FirstOrDefault(c => c.Matches(inputs.FirstOrDefault()));

            if (cmd is null || inputs.Length != 1)
            {
                return new Shard(request.UserId, request.TimeStamp, "note", request.Message);
            }

            return new Shard(request.UserId, request.TimeStamp, cmd.Name, split.LastOrDefault().Trim());
        }
    }
}