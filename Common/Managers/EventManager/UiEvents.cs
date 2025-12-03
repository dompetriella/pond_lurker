using Godot;
using System;


public partial class UiEvents : Node
{
    public static UiEvents Instance;

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }
}
