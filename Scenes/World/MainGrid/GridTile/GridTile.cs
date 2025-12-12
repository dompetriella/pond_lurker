using Godot;
using System;
using System.Reflection;

public partial class GridTile : Panel
{
    [Export]
    public TextureRect TextureRect;

    [Export]
    public RichTextLabel ValueLabel;

    public int Index;
    public Vector2I TileCoordinates;
    public TileValue Value;


    private StyleBoxFlat style;
    public override void _Ready()
    {
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;

    }

    private void OnMouseEntered()
    {
        this.OverrideStyleBoxWith(
            (StyleBoxSubtypes.Panel, box => box.With(bgColor: Colors.White with { A = 0.25f }))
        );
    }

    private void OnMouseExited()
    {
        this.OverrideStyleBoxWith(
             (StyleBoxSubtypes.Panel, box => box.With(bgColor: Colors.Transparent))
         );
    }
}
