namespace Prisms.Core;

public abstract record Result()
{
    public record Success() : Result()
    {
        public override string ToString() => nameof(Success);
    }    
    public record Response(string Content) : Success();
    public record Error(Exception Exception) : Result();
}
