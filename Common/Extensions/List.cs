using System;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    /// <summary>
	/// Shuffles a list in place (mutable)
	/// </summary>
	public static void Shuffle<T>(this IList<T> list, Random rng = null)
    {
        rng ??= new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    /// <summary>
	/// Shuffles a list and returns the new randomized list (immutable)
	/// </summary>
    public static IList<T> ShuffleImmutably<T>(this IEnumerable<T> source, Random rng = null)
    {
        var list = source.ToList();
        list.Shuffle(rng);
        return list;
    }

    /// <summary>
	/// Converts a flat list into a 2D array grid (row-major order).  Rows * Columns must == List.Count
	/// </summary>
	public static T[,] ToGridArray<T>(this List<T> list, int rows, int columns)
    {
        if (list.Count != rows * columns)
            throw new ArgumentException("List count does not match grid size.");

        T[,] grid = new T[rows, columns];

        for (int i = 0; i < list.Count; i++)
        {
            int y = i / columns;
            int x = i % columns;
            grid[y, x] = list[i];
        }

        return grid;
    }

    /// <summary>
    /// Tries to get an element from a list safely.
    /// Returns true if the index exists, false otherwise.
    /// </summary>
    public static bool TryGet<T>(this List<T> list, int index, out T value)
    {
        if (index >= 0 && index < list.Count)
        {
            value = list[index];
            return true;
        }

        value = default;
        return false;
    }
}
