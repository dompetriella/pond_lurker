using Godot;
using System;
using System.Net.NetworkInformation;

public partial class UiState : Node
{
    public static UiState Instance;

    // Test data for the debug screens
    public StatefulData<int> TestCounter = new(0);

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }


}
