using Godot;
using System;
using System.ComponentModel;

[GlobalClass]
public partial class State : Node
{

    [Signal]
    public delegate void TransitionStateEventHandler(State currentState, string newStateName);

    public virtual void Enter()
    {
        return;
    }

    public virtual void Exit()
    {
        return;
    }

    public virtual void Update(double delta)
    {
        return;
    }

    public virtual void PhysicsUpdate(double delta)
    {

    }
}