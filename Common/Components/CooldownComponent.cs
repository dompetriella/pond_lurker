using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
public partial class CooldownComponent : Node
{

    /// <summary>
    /// Signal emitted when the cooldown starts.
    /// </summary>
    [Signal] public delegate void CooldownStartedEventHandler();

    /// <summary>
    /// Signal emitted when the cooldown is stopped
    /// </summary>
    [Signal] public delegate void CooldownStoppedEventHandler();

    /// <summary>
    /// Signal emitted when the cooldown is paused.
    /// </summary>
    [Signal] public delegate void CooldownPausedEventHandler();

    /// <summary>
    /// Signal emitted when the cooldown is resumed from a paused state.
    /// </summary>
    [Signal] public delegate void CooldownResumedEventHandler();

    /// <summary>
    /// Signal emitted when the cooldown finishes.
    /// </summary>
    [Signal] public delegate void CooldownFinishedEventHandler();

    /// <summary>
    /// Total duration of the cooldown in milliseconds.
    /// </summary>
    [Export] public int CooldownTime { get; set; } = 10000;

    /// <summary>
    /// If true, cooldown will automatically restart after finishing.
    /// </summary>
    [Export] public bool AutoRepeat { get; set; } = false;

    /// <summary>
    /// If true, cooldown immediately start running when added to the tree
    /// </summary>
    [Export] public bool AutoStart { get; set; } = false;

    /// <summary>
    /// True if the cooldown is currently running.
    /// </summary>
    public bool IsRunning { get; private set; } = false;

    /// <summary>
    /// Time remaining in milliseconds.
    /// </summary>
    public int TimeRemaining { get; private set; } = 0;

    /// <summary>
    /// Time remaining in milliseconds.
    /// </summary>
    public int TimeElapsed => CooldownTime - TimeRemaining;

    /// <summary>
    /// Percent of the cooldown completed (as double)
    /// </summary>
    public double PercentCompleted => TimeElapsed > 0 ? TimeElapsed / CooldownTime : 0;

    private bool isPaused = false;

    public override void _Ready()
    {
        base._Ready();


        if (AutoStart)
        {
            Start();
        }

    }

    public override void _Process(double delta)
    {
        if (IsRunning && !isPaused)
        {
            TimeRemaining -= (int)(delta * 1000);

            if (TimeRemaining <= 0)
            {
                TimeRemaining = 0;
                IsRunning = false;
                EmitSignal(SignalName.CooldownFinished);

                if (AutoRepeat)
                {
                    Start();
                }
            }
        }
    }

    /// <summary>
    /// <para>Starts the cooldown at the maximum CooldownTime (ms).</para>
    /// <para>Emits the <see cref="CooldownStartedEventHandler"/> signal.</para>
    /// </summary>
    public void Start()
    {
        TimeRemaining = CooldownTime;
        IsRunning = true;
        isPaused = false;

        EmitSignal(SignalName.CooldownStarted);
    }

    /// <summary>
    /// <para>Stops the cooldown and sets the TimeRemaining to 0.</para>
    /// <para>Emits the <see cref="CooldownStoppedEventHandler"/> signal.</para>
    /// </summary>
    public void Stop()
    {
        IsRunning = false;
        TimeRemaining = 0;

        EmitSignal(SignalName.CooldownStopped);
    }

   /// <summary>
    /// <para>Pauses the cooldown. Does not restart the TimeRemaining.</para>
    /// <para>Emits the <see cref="CooldownPausedEventHandler"/> signal.</para>
    /// </summary>
    public void Pause()
    {
        isPaused = true;
        EmitSignal(SignalName.CooldownPaused);
    }


    /// <summary>
    /// <para>Resumes the cooldown from a paused state. Does not restart the TimeRemaining and only works if paused.</para>
    /// <para>Emits the <see cref="CooldownResumedEventHandler"/> signal.</para>
    /// </summary>
    public void Resume()
    {
        if (IsRunning)
            isPaused = false;
            EmitSignal(SignalName.CooldownResumed);
    }
}
