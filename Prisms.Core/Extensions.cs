﻿namespace Prisms.Core;

public static class Extensions
{
    public static T T<T>(this T input, Action<T> action)
    {
        action(input);
        return input;
    }

    public static T2 λ<T1, T2>(this T1 input, Func<T1, T2> func) => func(input);

    public static string JoinStrings(this IEnumerable<string> strings, string separator = "") => separator == "" ? string.Concat(strings) : string.Join(separator, strings);
    public static string JoinLines(this IEnumerable<string> strings, int newLines = 1) => strings.JoinStrings(Environment.NewLine.Times(newLines));
    public static string Times(this string s, int i) => Enumerable.Repeat(s, i).JoinStrings();
}
