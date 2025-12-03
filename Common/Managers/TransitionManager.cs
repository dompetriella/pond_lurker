using Godot;
using System;

public partial class TransitionManager : CanvasLayer
{

    public static TransitionManager Instance { get; private set; }

    [Signal]
    public delegate void TransitionStartedEventHandler(string animationName);

    [Signal]
    public delegate void TransitionEndedEventHandler(string animationName);


    [Export]
    private ColorRect ColorRectNode;

    [Export]
    private AnimationPlayer AnimationPlayerNode;


    public static class TransitionType
    {
        public const string FadeIn = "fade_in";
        public const string FadeOut = "fade_out";
    }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;

        AnimationPlayerNode.AnimationStarted += OnAnimationStartedInternal;
        AnimationPlayerNode.AnimationFinished += OnAnimationFinishedInternal;
    }

    public void PlayTransition(string transitionType, double transitionDuration = 1.0,
        bool shouldDisableInputOnStart = true, bool shouldEnableInputOnEnd = true)
    {
        var animation = AnimationPlayerNode.GetAnimation(transitionType);
        if (animation == null)
        {
            GD.PushWarning($"Animation {transitionType} not found");
            return;
        }

        if (animation.Length <= 0f)
        {
            GD.PushWarning($"Animation {transitionType} has no length");
            return;
        }

        float customSpeed = (float)(animation.Length / transitionDuration);

        _pendingDisableInput = shouldDisableInputOnStart;
        _pendingEnableInput = shouldEnableInputOnEnd;

        AnimationPlayerNode.Play(name: transitionType, customSpeed: customSpeed);
    }

    private bool _pendingDisableInput;
    private bool _pendingEnableInput;

    private void OnAnimationStartedInternal(StringName animationName)
    {
        if (_pendingDisableInput)
        {
            ColorRectNode.MouseFilter = Control.MouseFilterEnum.Stop;
        }

        EmitSignal(SignalName.TransitionStarted, (string)animationName);
    }

    private void OnAnimationFinishedInternal(StringName animationName)
    {
        if (_pendingEnableInput)
            ColorRectNode.MouseFilter = Control.MouseFilterEnum.Pass;

        EmitSignal(SignalName.TransitionEnded, (string)animationName);
    }
}
