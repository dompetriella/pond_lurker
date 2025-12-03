using Godot;
using System;

public partial class GameEvents : Node
{
    public static GameEvents Instance;

    public override void _Ready() {
        base._Ready();

        Instance = this;
    }
}
