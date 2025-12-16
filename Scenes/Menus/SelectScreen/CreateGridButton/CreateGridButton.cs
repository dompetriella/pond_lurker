using Godot;
using System;
using System.Threading.Tasks;

public partial class CreateGridButton : Button
{
    [Export]
    public int Columns;

    [Export]
    public double MinesPercentage;

    public override void _Ready()
    {
        base._Ready();

        this.Pressed += OnButtonPressed;
    }

    private string GameScreenUID = "uid://b2qrpntbnv82g";

    private void OnButtonPressed()
    {
        var gameScreen = ResourceLoader.Load<PackedScene>(GameScreenUID);
        GameScreen gameScreenInstance = gameScreen.Instantiate<GameScreen>();

        Func<Task> dataLoadingFunction = async () =>
        {
            gameScreenInstance.MainGrid.GenerateGrid(baseColumns: Columns, minesPercentage: MinesPercentage);
            await Task.CompletedTask;
        };

        ScaffoldManager.Instance.ScaffoldNewSceneTree(gameScreenInstance, dataLoadingFunction: dataLoadingFunction);
    }


}
