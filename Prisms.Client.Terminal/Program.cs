using Prisms.Client.Terminal;

if (args.Length > 0)
{
    await new Client().ProcessAsync(string.Join(' ', args));
}
await new Client().RunAsync();
