using Godot;
using System;

public partial class HealthComponent : Node
{
    [Export] public int TotalHealth = 10;
    [Export] public int CurrentHealth;

    [Signal] public delegate void HealthChangedEventHandler(int initialHealth, int amountChanged, int finalHealth, int adjustedHealth);
    [Signal] public delegate void HealthDepletedEventHandler();

    public override void _Ready()
    {
        base._Ready();
        CurrentHealth = TotalHealth;
    }

    public void TakeDamage(int amount) => ApplyHealthChange(-Math.Abs(amount));
    public void HealDamage(int amount) => ApplyHealthChange(Math.Abs(amount));

    private void ApplyHealthChange(int amount)
    {
        var initialHealth = CurrentHealth;

        CurrentHealth = Math.Clamp(CurrentHealth + amount, 0, TotalHealth);

        var actualChange = CurrentHealth - initialHealth;

        EmitSignal(SignalName.HealthChanged, initialHealth, actualChange, initialHealth + amount, CurrentHealth);

        if (CurrentHealth <= 0 && amount < 0)
        {
            EmitSignal(SignalName.HealthDepleted);
        }
    }
}
