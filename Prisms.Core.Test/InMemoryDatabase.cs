namespace Prisms.Core.Test;

public class InMemoryDatabase : IDatabase
{
    private Dictionary<string, List<Shard>> _db;

    public InMemoryDatabase()
    {
        _db = new Dictionary<string, List<Shard>>();
    }
    
    public void Clear()
    {
        _db = new Dictionary<string, List<Shard>>();
    }

    public Task<Shard[]> GetAllOfDataTypeAsync(string userId, string dataType)
    {
        var key = $"{userId}/{dataType}";

        if (!_db.ContainsKey(key))
        {
            _db.Add(key, new List<Shard>());
        }
        
        return Task.FromResult(_db[key].ToArray());
    }

    public Task CreateOrUpdateAsync(Shard shard)
    {
        var key = $"{shard.UserId}/{shard.DataType}";

        if (!_db.ContainsKey(key))
        {
            _db.Add(key, new List<Shard>());
        }
        _db[key].Add(shard);

        return Task.CompletedTask;
    }
}
