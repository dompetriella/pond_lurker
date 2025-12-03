using Godot;
using System;
using System.Threading.Tasks;

public partial class ScaffoldManager : Node
{
    public static ScaffoldManager Instance { get; private set; }

    [Signal]
    public delegate void ScaffoldingStartedEventHandler();

    [Signal]
    public delegate void ScaffoldingEndedEventHandler();

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }

    public async void ScaffoldNewSceneTree(Node newSceneTree, Func<Task> dataLoadingFunction = null, String transitionTypeOnExitScene = TransitionManager.TransitionType.FadeOut, String transitionTypeOnEnterNewScene = TransitionManager.TransitionType.FadeIn, float transitionDuration = 0.5f)
    {
        await BuildSceneTree(newSceneTree: newSceneTree, dataLoadingFunction: dataLoadingFunction, transitionTypeOnEnterNewScene: transitionTypeOnEnterNewScene, transitionTypeOnExitScene: transitionTypeOnExitScene, transitionDuration: transitionDuration);
    }

    public async void ScaffoldNewSceneTree(PackedScene newSceneTree, Func<Task> dataLoadingFunction = null, String transitionTypeOnExitScene = TransitionManager.TransitionType.FadeOut, String transitionTypeOnEnterNewScene = TransitionManager.TransitionType.FadeIn, float transitionDuration = 0.5f)
    {

        var newSceneTreeNode = newSceneTree.Instantiate();

        await BuildSceneTree(newSceneTree: newSceneTreeNode, dataLoadingFunction: dataLoadingFunction, transitionTypeOnEnterNewScene: transitionTypeOnEnterNewScene, transitionTypeOnExitScene: transitionTypeOnExitScene, transitionDuration: transitionDuration);
    }

    private async Task BuildSceneTree(Node newSceneTree, Func<Task> dataLoadingFunction = null, String transitionTypeOnExitScene = TransitionManager.TransitionType.FadeOut, String transitionTypeOnEnterNewScene = TransitionManager.TransitionType.FadeIn, float transitionDuration = 0.5f)
    {
        EmitSignal(SignalName.ScaffoldingStarted);

        TransitionManager.Instance.PlayTransition(transitionType: transitionTypeOnExitScene, transitionDuration, shouldEnableInputOnEnd: false);

        await ToSignal(TransitionManager.Instance, TransitionManager.SignalName.TransitionEnded);

        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        AddChild(newSceneTree);

        if (dataLoadingFunction != null)
        {
            await dataLoadingFunction();
        }

        TransitionManager.Instance.PlayTransition(transitionType: transitionTypeOnEnterNewScene, transitionDuration);

        await ToSignal(TransitionManager.Instance, TransitionManager.SignalName.TransitionEnded);

        EmitSignal(SignalName.ScaffoldingEnded);
    }
}