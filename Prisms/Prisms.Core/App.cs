using System;

namespace Prisms.Core
{
    public interface IApp {
        Result Process(UserMessage request);
    }

    public class App : IApp
    {
        private readonly IStorage _storage;

        private App(IStorage storage) {
            _storage = storage;
        }

        public static App Create(IStorage storage) {
            return new App(storage);
        }

        public Result Process(UserMessage request) {
            var shard = MessageParser.Parse(request);
            _storage.Write(shard);
            return new Result.Success();
        }
    }
}
