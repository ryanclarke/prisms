namespace Prisms.Core;

public interface IApp
{
    Task<Result> ProcessAsync(UserMessage request);
}

public class App : IApp
{
    private readonly Storage _storage;

    public static App Create()
    {
        throw new NotImplementedException();
    }

    private App(IDatabase database)
    {
        _storage = new Storage(database);
    }

    public static App Create(IDatabase storage)
    {
        return new App(storage);
    }

    public async Task<Result> ProcessAsync(UserMessage request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return new Result.Error(new ArgumentException("Cannot be empty or whitespace", nameof(request.Message)));
        }
        
        var userCommands = await _storage.GetUserCommandsAsync(request.UserId);
        var (query, shard) = MessageParser.Parse(userCommands, request);
        await _storage.SaveAsync(shard);
        return (await _storage.FetchAsync(query))?.LastOrDefault()?.ToString() switch
        {
            string s => new Result.Response(s),
            null => new Result.Success()
        };
    }
}
