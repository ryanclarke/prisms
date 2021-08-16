using System;

namespace Prisms.Core
{
    public interface IApp {
        Response Process(TextMessage request);
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

        public Response Process(TextMessage request) {
            return Response.Success(request.Content);
        }
    }
}
