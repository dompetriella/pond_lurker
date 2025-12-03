using Godot;
using System;
using System.Threading.Tasks;

public partial class TestPage : Control
{
    [Export]
    public Button ReturnButton;

    [Export]
    public NavigationRouteComponent NavigationRouteComponent;

    [Export]
    public Button CounterButton;

    [Export]
    public ProgressBar CounterProgress;

    [Export]
    public Label CounterLabel;

    public override void _Ready()
    {
        base._Ready();


        ReturnButton.Pressed += () =>
        {
            ScaffoldManager.Instance.ScaffoldNewSceneTree(NavigationRouteComponent.Scene);
        };


        // Null checking for testing purposes
        // Example of using the StatefulData 
        if (CounterButton != null && CounterLabel != null && CounterProgress != null)
        {

           var counterState = UiState.Instance.TestCounter;

            var initialValue = counterState.Value;

            CounterLabel.Text = initialValue.ToString();
            CounterProgress.MaxValue = 100;
            CounterProgress.Value = initialValue;

 
            CounterButton.Pressed += () =>
            {
                var currentValue = counterState.Value;
                counterState.SetValue(currentValue + 5);
            };

            counterState.ValueChanged(this, async (previousValue, newValue) =>
            {
                CounterLabel.Text = newValue.ToString();
                var progressValue = Math.Clamp(value: newValue, min: 0, max: CounterProgress.MaxValue);
                CreateTween().TweenProperty(CounterProgress, property: ProgressBarProperties.Value, finalVal: progressValue, duration: 0.25);

                if (newValue >= CounterProgress.MaxValue)
                {
                    await NotificationManager.Instance.ShowNotification(messageText: "[color=red]MAX[/color] Value Reached", showDurationInMs: 2000);
                }
            });
        }

    }
}
