using System.Collections.Generic;

public static class GridArrayExtensions
{
    /// <summary>
    /// Flattens a 2D Grid into a List<T>
    /// </summary>
    public static List<T> ToFlatList<T>(this T[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        List<T> list = new(rows * cols);

        for (int y = 0; y < rows; y++)
            for (int x = 0; x < cols; x++)
                list.Add(grid[y, x]);

        return list;
    }

    /// <summary>
    /// Tries to get an element from an x,y coordinate safely.
    /// Returns true if the index exists, false otherwise.
    /// </summary>
    public static bool TryGet<T>(this T[,] grid, int x, int y, out T value)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        if (x >= 0 && x < cols && y >= 0 && y < rows)
        {
            value = grid[y, x];
            return true;
        }

        value = default;
        return false;
    }
}