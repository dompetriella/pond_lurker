using Godot;
using System;
using System.Threading.Tasks;

public partial class TitleScreen : Control
{

    [Export]
    public Button NextPageButton;


    [Export]
    public NavigationRouteComponent NavigationRouteComponent;

    public override void _Ready()
    {
        base._Ready();

        NextPageButton.Pressed += async () =>
        {
            var scaffoldManager = ScaffoldManager.Instance;
            scaffoldManager.ScaffoldNewSceneTree(newSceneTree: NavigationRouteComponent.Scene.Instantiate());};
    }
}
