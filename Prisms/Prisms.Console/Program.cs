using System;
using System.Linq;
using Prisms.Core;

namespace Prisms.Client.Terminal
{
    class Program
    {

        static void Main(string[] args)
        {
            new Client().Process(args.FirstOrDefault());
        }
    }

    public class Client
    {
        private readonly IStorage _storage;
        private readonly App _app;

        public Client()
        {
            _storage = new FlatStorage();
            _app = App.Create(_storage);
        }

        public void Process(string arg)
        {
            do
            {
                var content = arg ?? Console.ReadLine();
                if (content is not null)
                {
                    var message = new TextMessage
                    {
                        PhoneNumber = "+12345678901",
                        Content = content
                    };
                    var response = _app.Process(message);
                    Console.WriteLine(response.ToString());
                }
            } while (arg is null);
        }
    }
}
