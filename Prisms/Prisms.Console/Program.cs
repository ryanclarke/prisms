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
        private readonly App _app;
        private readonly UserMessage _message;

        public Client()
        {
            _app = App.Create(new FlatStorage("./../../../db"));
            _message = new UserMessage(Environment.UserName, DateTime.MinValue, "");
        }

        public void Process(string input)
        {
            var message = _message with
            {
                TimeStamp = DateTime.Now,
                Message = input ?? Console.ReadLine() ?? ""
            };
            var result = _app.Process(message);
            Console.WriteLine(result.ToString());
        }
    }
}
