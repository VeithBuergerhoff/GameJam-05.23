using System;
using System.Collections.Generic;

/// <summary>
/// Based on <see href="https://stackoverflow.com/a/1262619/13314572"/>
/// </summary>
public static class ListShuffleExtension
{
    private readonly static Random rng = new();

    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        for (var n = list.Count - 1; n > 0; n--)
        {
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }

        return list;
    }
}