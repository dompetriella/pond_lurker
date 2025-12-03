using Godot;
using System;
using System.Threading.Tasks;

public partial class Player : Entity
{
    [Export]
    public Label StateLabel;

    [Export]
    public StateMachine StateMachine;

    public override void _Ready()
    {
        base._Ready();

    }


    public override void _Process(double delta)
    {
        base._Process(delta);

        StateLabel.Text = StateMachine.CurrentState.Name;

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        MoveAndSlide();
    }
}