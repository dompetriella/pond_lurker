using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

[GlobalClass]
public partial class StateMachine : State
{

    [Signal]
    public delegate void StateHasChangedEventHandler();

    [Export] public State DefaultState;

    public State CurrentState;

    public List<State> StateHistory { get; private set; } = [];

    public Dictionary<string, State> StatesDictionary = [];


    public override void _Ready()
    {
        base._Ready();

        foreach (var child in GetChildren())
        {
            if (child is State state)
            {
                StatesDictionary[child.Name] = state;
                state.TransitionState += OnTransitionState;
            }

        }

        if (DefaultState != null)
        {
            DefaultState.Enter();
            CurrentState = DefaultState;
            StateHistory.Add(DefaultState);
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        CurrentState?.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        CurrentState?.PhysicsUpdate(delta);
    }

    private void OnTransitionState(State oldState, string newStateName)
    {
        if (!StatesDictionary.TryGetValue(newStateName, out State newState))
        {
            GD.PushWarning($"Tried to transition state but {newStateName} doesn't exist");
            return;
        }

        if (newState == CurrentState)
        {
            StateHistory.Add(newState);
            return;
        }


        CurrentState?.Exit();
        newState.Enter();

        CurrentState = newState;
        StateHistory.Add(newState);

        EmitSignal(SignalName.StateHasChanged, CurrentState);
    }
}