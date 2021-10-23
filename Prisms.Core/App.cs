using System;
using System.Threading.Tasks;

namespace Prisms.Core
{
    public interface IApp {
        Task<Result> ProcessAsync(UserMessage request);
    }

    public class App : IApp
    {
        private readonly Storage _storage;

        public static App Create()
        {
            throw new NotImplementedException();
        }

        private App(IDatabase database) {
            _storage = new Storage(database);
        }

        public static App Create(IDatabase storage) {
            return new App(storage);
        }

        public async Task<Result> ProcessAsync(UserMessage request) {
            var userCommands = await _storage.GetUserCommandsAsync(request.UserId);
            var shard = MessageParser.Parse(userCommands, request);
            await _storage.SaveAsync(shard);
            return new Result.Success();
        }
    }
}
