using System;
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
    /// Returns all adjacent elements (optionally including diagonals) within the given radius.
    /// Automatically bounds-checks and excludes the center element.
    /// </summary>
    public static List<T> GetAdjacent<T>(
        this T[,] grid,
        int x,
        int y,
        bool includeDiagonals = true,
        int radius = 1)
    {
        var neighbors = new List<T>();
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int dy = -radius; dy <= radius; dy++)
        {
            for (int dx = -radius; dx <= radius; dx++)
            {
                // Skip self
                if (dx == 0 && dy == 0)
                    continue;

                // Skip diagonals if not included
                if (!includeDiagonals && Math.Abs(dx) + Math.Abs(dy) > 1)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < cols && ny >= 0 && ny < rows)
                    neighbors.Add(grid[ny, nx]);
            }
        }

        return neighbors;
    }

    /// <summary>
    /// Vector2I overload of GetAdjacent
    /// </summary>
    public static List<T> GetAdjacent<T>(
        this T[,] grid,
        Godot.Vector2I coordinate,
        bool includeDiagonals = true,
        int radius = 1)
        => grid.GetAdjacent(coordinate.X, coordinate.Y, includeDiagonals, radius);

}