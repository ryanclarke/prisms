namespace Prisms.Core;

public class Storage
{
    public static class Constants
    {
        public static class Config
        {
            private static readonly string _name = "_config";
            public static readonly string Command = $"{_name}.command";
        }
    }

    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public Storage(IDatabase database)
    {
        _database = database;
        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public async Task SaveAsync(Shard shard) => await _database.CreateOrUpdateAsync(shard);

    public async Task<IEnumerable<Command>> GetUserCommandsAsync(string userId) =>
        (await _database.GetAllOfDataTypeAsync(userId, Constants.Config.Command))
            .Select(c => Deserialize<Command>(c.Data))
            .OfType<Command>();

    public async Task<Shard[]> FetchAsync(Query query)
    {
        return await _database.GetAllOfDataTypeAsync(query.UserId, query.Command.Name);
    }

    public T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);

    public string Serialize<T>(T obj) => JsonSerializer.Serialize(obj, _jsonSerializerOptions);
}
