namespace Prisms.Client.Terminal;

public static class Extensions
{
    public static void Write(this object input)
    {
        Console.Write(input);
    }
    public static void WriteLine(this object input)
    {
        Console.WriteLine(input);
    }
}
