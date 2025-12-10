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

    public override void _Ready()
    {
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

private void OnMouseEntered()
{
    var style = GetThemeStylebox("panel", "PanelContainer");

    if (style is StyleBoxFlat flatStyleBox)
    {
        var newStyle = flatStyleBox.Duplicate() as StyleBoxFlat;
        newStyle.BgColor = Colors.White;
        AddThemeStyleboxOverride("panel", newStyle);
    }
}

private void OnMouseExited()
{
    var style = GetThemeStylebox("panel", "PanelContainer");

    if (style is StyleBoxFlat flatStyleBox)
    {
        var newStyle = flatStyleBox.Duplicate() as StyleBoxFlat;
        newStyle.BgColor = Colors.Transparent;
        AddThemeStyleboxOverride("panel", newStyle);
    }
}
}
