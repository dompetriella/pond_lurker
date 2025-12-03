using Godot;
using System;

[GlobalClass]
public partial class NavigationRouteComponent : Node
{
    [Export]
    public string SceneUID;

    public PackedScene Scene;

    public override void _Ready() {
        base._Ready();

        Scene = ResourceLoader.Load<PackedScene>(SceneUID);
    }
}
