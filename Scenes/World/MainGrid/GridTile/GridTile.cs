using Godot;
using System;
using System.Diagnostics.Tracing;
using System.Reflection;

public partial class GridTile : Panel
{
    [Signal]
    public delegate void TileSelectedEventHandler(GridTile gridTile);

    [Export]
    public TextureRect TextureRect;

    [Export]
    public RichTextLabel ValueLabel;

    [Export]
    public Button OverlayButton;

    public int Index;
    public Vector2I TileCoordinates;
    public TileValue Value;
    public StatefulData<bool> isDiscovered = new(false);


    private StyleBoxFlat style;
    public override void _Ready()
    {
        OverlayButton.Pressed += () => EmitSignal(SignalName.TileSelected, this);
    }

    public void DiscoverTile(GridTile[,] totalGrid)
    {

        if (isDiscovered.Value == true) return;

        isDiscovered.Value = true;

        if (Value == TileValue.Mine)
        {
            ValueLabel.Show();
            TextureRect.Show();
            this.OverrideStyleBoxWith(
                (StyleBoxSubtypes.Panel, box => box.With(bgColor: Colors.Red)));
            return;
        }

        if (Value >= TileValue.One)
        {
            ValueLabel.Show();
            this.OverrideStyleBoxWith(
                (StyleBoxSubtypes.Panel, box => box.With(bgColor: Colors.DimGray with { A = 0.25f })));
            return;
        }

        ValueLabel.Text = "";
        ValueLabel.Show();

        this.OverrideStyleBoxWith(
            (StyleBoxSubtypes.Panel, box => box.With(bgColor: Colors.Black)));

        var getAdjacentTiles = totalGrid.GetAdjacent(this.TileCoordinates);

        foreach (var tile in getAdjacentTiles)
        {
            if (tile.isDiscovered.Value == false)
            {
                tile.DiscoverTile(totalGrid: totalGrid);
            }

        }
    }
}
