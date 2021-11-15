namespace Prisms.Client.Terminal;

public class FlatFileDatabase : IDatabase
{
    private readonly string _rootPath;

    public FlatFileDatabase(string rootPath)
    {
        _rootPath = rootPath;
    }

    public Task<Shard[]> GetAllOfDataTypeAsync(string userId, string dataType)
    {
        var path = DataTypePath(userId, dataType);
        if (Directory.Exists(path))
        {
            var filePaths = Directory.GetFiles(path);
            var shards = filePaths.Select(async path => new Shard(userId, path.λ(Path.GetFileName).λ(DateTime.Parse), dataType, await File.ReadAllTextAsync(path)));
            return Task.WhenAll(shards);
        }
        return Task.FromResult(Array.Empty<Shard>());
    }

    public async Task CreateOrUpdateAsync(Shard shard)
    {
        var path = FilePath(shard);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        await File.WriteAllTextAsync(path, shard.Data);
    }

    private string UserPath(string userId) => Path.Combine(_rootPath, userId);
    private string DataTypePath(string userId, string dataType) => Path.Combine(UserPath(userId), dataType);
    private string FilePath(Shard shard) => Path.Combine(DataTypePath(shard.UserId, shard.DataType), shard.TimeStamp.ToString("o").Replace(':', ';').Replace('.', ','));
}
