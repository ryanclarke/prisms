using System;
using System.Threading.Tasks;

namespace Prisms.Core
{
    public interface IApp {
        Task<Result> ProcessAsync(UserMessage request);
    }

    public class App : IApp
    {
        private readonly IStorage _storage;

        public static App Create()
        {
            throw new NotImplementedException();
        }

        private App(IStorage storage) {
            _storage = storage;
        }

        public static App Create(IStorage storage) {
            return new App(storage);
        }

        public async Task<Result> ProcessAsync(UserMessage request) {
            var userCommands = await _storage.ReadUserCommandsAsync(request.UserId);
            var shard = MessageParser.Parse(userCommands, request);
            await _storage.WriteAsync(shard);
            return new Result.Success();
        }
    }
}
