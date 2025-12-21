using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Reflection;


public partial class GridTile : Panel
{
    [Signal]
    public delegate void TileSelectedEventHandler(GridTile gridTile);

    [Signal]
    public delegate void TileMarkedEventHandler(GridTile gridTile);

    [Export]
    public TextureRect MineTexture;

    [Export]
    public RichTextLabel ValueLabel;

    [Export]
    public TextureRect FlagTexture;

    [Export]
    public Button OverlayButton;

    public int Index;
    public Vector2I TileCoordinates;
    public TileValue Value;


    public enum TileState
    {
        Initial,
        Discovered,
        Marked
    }

    public List<TileState> StateHistory = [];

    public TileState CurrentState;


    private StyleBoxFlat style;
    public override void _Ready()
    {
        OverlayButton.GuiInput += SetInput;

        SetState(TileState.Initial);
    }

    private void SetInput(InputEvent @event)
    {

        switch (@event)
        {
            case InputEventMouseButton mouseEvent when mouseEvent.Pressed:
                switch (mouseEvent.ButtonIndex)
                {
                    case MouseButton.Left:
                        EmitSignal(SignalName.TileSelected, this);
                        GD.Print("Left Click");
                        break;
                    case MouseButton.Right:
                        EmitSignal(SignalName.TileMarked, this);
                        GD.Print("Right Click");
                        break;
                }
                break;

            case InputEventKey keyEvent when keyEvent.Pressed:
                if (keyEvent.IsActionPressed(InputActions.UiAccept))
                {
                    EmitSignal(SignalName.TileSelected, this);
                }
                else if (keyEvent.IsActionPressed(InputActions.UiSelect))
                {
                    EmitSignal(SignalName.TileMarked, this);
                }
                break;
        }
    }

    public void MarkTile(GridTile[,] totalGrid)
    {
        switch (CurrentState)
        {
            case TileState.Initial:
                SetState(TileState.Marked);
                FlagTexture.Show();
                break;

            case TileState.Discovered:
                SetState(TileState.Marked);
                FlagTexture.Show();
                ValueLabel.Hide();
                break;

            case TileState.Marked:
                if (StateHistory.Contains(TileState.Discovered))
                {
                    DiscoverTile(totalGrid);
                }
                else
                {
                    SetState(TileState.Initial);
                    FlagTexture.Hide();
                }
                break;
        }
    }


    public void DiscoverTile(GridTile[,] totalGrid)
    {

        if (CurrentState == TileState.Discovered) return;

        if (CurrentState == TileState.Marked)
        {
            FlagTexture.Hide();
        }

        SetState(TileState.Discovered);


        if (Value == TileValue.Mine)
        {
            ValueLabel.Show();
            MineTexture.Show();
            this.OverrideStyleBoxWith(
                (StyleBoxSubtypes.Panel, box => box.With(bgColor: Colors.Red)));
            return;
        }

        if (Value >= TileValue.One)
        {
            ValueLabel.Show();
            this.OverrideStyleBoxWith(
                (StyleBoxSubtypes.Panel, box => box.With(bgColor: Colors.DimGray with { A = 0.25f })));
            return;
        }

        ValueLabel.Text = "";
        ValueLabel.Show();

        this.OverrideStyleBoxWith(
            (StyleBoxSubtypes.Panel, box => box.With(bgColor: Colors.Black with { A = 0.25f })));

        var getAdjacentTiles = totalGrid.GetAdjacent(this.TileCoordinates);

        foreach (var tile in getAdjacentTiles)
        {
            if (tile.CurrentState == TileState.Initial)
            {
                tile.DiscoverTile(totalGrid: totalGrid);
            }
        }
    }

    private void SetState(TileState state)
    {
        StateHistory.Add(state);
        CurrentState = state;
    }
}
