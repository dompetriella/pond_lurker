using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MainGrid : GridContainer
{

    [Export]
    public string GridTileUID;

    [Export]
    public Label MineLabel;

    //
    public List<GridTile> GridTilesList { get; private set; } = [];
    public GridTile[,] GridTilesGrid { get; private set; }

    public GridTile CurrentHoveredTile = null;

    //
    private float MinimumTileSize = 24;
    private float BaseTileDimension = 96;
    private float BaseTileAmount = 16;

    //

    public int TotalTileAmount { get; private set; }
    public int BaseColumns { get; private set; }

    public float AdjustedTileDimension { get; private set; }
    public int MineTotal { get; private set; }

    public override void _Ready()
    {
        base._Ready();


    }

    public void GenerateGrid(int baseColumns, double minesPercentage)
    {

        BaseColumns = baseColumns;
        Columns = baseColumns;
        TotalTileAmount = baseColumns * baseColumns;
        MineTotal = (int)Math.Round(TotalTileAmount * minesPercentage);
        MineLabel.Text = $"{MineTotal}";

        float adjustedSize = BaseTileDimension * MathF.Sqrt(BaseTileAmount / TotalTileAmount);
        GD.Print(adjustedSize);
        AdjustedTileDimension = Math.Clamp(adjustedSize, min: MinimumTileSize, max: BaseTileDimension);


        var resultGridTiles = CreateGridTiles();

        foreach (var tile in resultGridTiles)
        {
            AddChild(tile);
        }

        GridTilesList = [.. resultGridTiles];
        GridTilesGrid = resultGridTiles.ToGridArray(rows: baseColumns, columns: baseColumns);

    }

    private List<GridTile> CreateGridTiles()
    {

        List<GridTile> gridTiles = [];

        var gridScene = ResourceLoader.Load<PackedScene>(GridTileUID);

        // Create tiles and add appropriate mines randomly
        for (int i = 0; i < TotalTileAmount; i++)
        {
            GridTile gridTileInstance = (GridTile)gridScene.Instantiate();
            gridTileInstance.CustomMinimumSize = new Vector2(AdjustedTileDimension, AdjustedTileDimension);
            if (i < MineTotal)
            {
                gridTileInstance.Value = TileValue.Mine;
            }

            gridTiles.Add(gridTileInstance);
        }

        gridTiles.Shuffle();

        // Transform to 2D array and calculate tile value
        var tileGrid = gridTiles.ToGridArray(rows: BaseColumns, columns: BaseColumns);

        for (int y = 0; y < BaseColumns; y++)
        {
            for (int x = 0; x < BaseColumns; x++)
            {

                GridTile tile = tileGrid[y, x];

                // Connect the on clicked signal
                tile.TileSelected += OnTileClicked;
                tile.TileCoordinates = new Vector2I(x, y);

                if (!(tile.Value == TileValue.Mine))
                {

                    tile.TextureRect.Texture = null;

                    var accumulatedValue = 0;
                    var listOfNeighbors = tileGrid.GetAdjacent(coordinate: tile.TileCoordinates);
                    listOfNeighbors.ForEach((tile) =>
                    {
                        if (tile.Value == TileValue.Mine) accumulatedValue++;
                    });


                    tile.Value = (TileValue)accumulatedValue;

                    tile.ValueLabel.Text = accumulatedValue.ToString();
                }

            }
        }

        return gridTiles;
    }

    // Tile Calculations

    private void OnTileClicked(GridTile tile)
    {
       tile.DiscoverTile(totalGrid: GridTilesGrid);
    }
}
