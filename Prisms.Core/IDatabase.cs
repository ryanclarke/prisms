using System.Threading.Tasks;

namespace Prisms.Core
{
    public interface IDatabase {
        Task<Shard[]> GetAllOfDataTypeAsync(string userId, string dataType);
        Task CreateOrUpdateAsync(Shard shard);
    }
}