using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prisms.Core
{
    public interface IStorage {
        Task<List<Command>> ReadUserCommandsAsync(string userId);
        Task WriteAsync(Shard shard);
    }
}