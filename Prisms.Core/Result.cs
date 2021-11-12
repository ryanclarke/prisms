namespace Prisms.Core;

public abstract record Result()
{
    public record Success() : Result();
    public record Response(string Content) : Success();
    public record Error(Exception Exception) : Result();

    public override string ToString() => this switch
    {
        Success s => s switch
        {
            Response r => $"Success: {r.Content}",
            _ => "Success"
        },
        Error e => $"Error {e.Exception}",
        _ => throw new NotImplementedException()
    };
}
