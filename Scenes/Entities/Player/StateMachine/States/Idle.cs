using Godot;
using System;

public partial class Idle : State
{
    [Export] public Entity Entity;
    [Export] public SpeedComponent SpeedComponent;

    public override void Update(double delta)
    {
        base.Update(delta);

        if (Entity.Velocity != Vector2.Zero)
        {
            EmitSignal(State.SignalName.TransitionState, this, PlayerStateMachine.States.Moving);
        }

    }

    public override void PhysicsUpdate(double delta)
    {
        base.PhysicsUpdate(delta);

        var inputVector = PlayerStateMachine.GetInputVector();
        Entity.Velocity = inputVector * SpeedComponent.Speed;
    }
}