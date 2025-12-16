using Godot;
using System;
using System.Threading.Tasks;

public partial class SelectScreen : Control
{

    [Export]
    public Button ReturnButton;

    [Export]
    public NavigationRouteComponent NavigationRouteComponent;

    public override void _Ready()
    {
        base._Ready();

        var returnInstance = ResourceLoader.Load<PackedScene>(NavigationRouteComponent.SceneUID);
        ReturnButton.Pressed += () => ScaffoldManager.Instance.ScaffoldNewSceneTree(returnInstance);    

    }
}
