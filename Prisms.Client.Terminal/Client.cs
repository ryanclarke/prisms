using Prisms.Core;

namespace Prisms.Client.Terminal;

public class Client
{
    private readonly App _app;
    private readonly UserMessage _message;

    public Client()
    {
        _app = App.Create(new FlatFileDatabase("./../../../db"));
        _message = new UserMessage(Environment.UserName, DateTime.MinValue, "");
    }

    public async Task ProcessAsync(string? input)
    {
        var message = _message with
        {
            TimeStamp = DateTime.Now,
            Message = input ?? Console.ReadLine() ?? ""
        };
        var result = await _app.ProcessAsync(message);
        Console.WriteLine(result.ToString());
    }
}
