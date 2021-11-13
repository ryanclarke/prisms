namespace Prisms.Core;

public static class Extensions
{
    public static T T<T>(this T input, Action<T> action)
    {
        action(input);
        return input;
    }

    public static T2 λ<T1, T2>(this T1 input, Func<T1, T2> func) => func(input);
}
