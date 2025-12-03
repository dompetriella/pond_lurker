using Godot;
using System;
using System.Threading.Tasks;

public partial class NotificationManager : Node
{

    public static NotificationManager Instance;

    [Signal]
    public delegate void NotificationShownEventHandler(string messageText);

    [Signal]
    public delegate void NotificationRemovedEventHandler(string messageText);


    [Export]
    public VBoxContainer NotificationColumn;

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }

    /// <summary>
    /// <para>Shows a notification with provided image and text.  If no image is given, notification
    /// will only show text.</para>
    /// <para>Notification stays on screen for <see cref="showDurationInMs"/> milliseconds.</para>
    /// <para>Appearance and layout of notification can be edited in the Notification.tscn scene.</para>
    /// </summary>
    public async Task ShowNotification(string messageText, Texture2D image = null, double showDurationInMs = 5000, double fadeInDurationInMs = 500, double fadeOutDurationInMs = 500)
    {
        var notificationInstance = CreateNotificationInstance(messageText, image);

        EmitSignal(SignalName.NotificationShown);

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        await FadeInNotification(notificationInstance, fadeInDurationInMs);

        var timer = GetTree().CreateTimer(showDurationInMs / 1000);
        await ToSignal(timer, Timer.SignalName.Timeout);

        await FadeOutNotification(notificationInstance, fadeOutDurationInMs);

        notificationInstance.QueueFree();

        EmitSignal(SignalName.NotificationRemoved);
    }

    private Notification CreateNotificationInstance(string messageText, Texture2D image)
    {
        var notificationInstance = ResourceLoader
            .Load<PackedScene>("uid://ciudsaf8ox1g4")
            .Instantiate<Notification>();

        notificationInstance.Message = messageText;
        notificationInstance.MessageImage = image;
        // Make notification invisible to fade in
        notificationInstance.Modulate = new Color(1, 1, 1, 0);

        NotificationColumn.AddChild(notificationInstance);

        return notificationInstance;
    }

    private async Task FadeInNotification(Control notification, double durationMs)
    {
        var tween = CreateTween()
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.In);

        tween.TweenProperty(notification, ControlProperties.Modulate.Alpha, 1.0, duration: durationMs / 1000);
        await ToSignal(tween, Tween.SignalName.Finished);
    }

    private async Task FadeOutNotification(Control notification, double durationMs)
    {
        var tween = CreateTween()
            .SetEase(Tween.EaseType.InOut);

        tween.TweenProperty(notification, ControlProperties.Modulate.Alpha, 0.0, duration: durationMs / 1000);
        await ToSignal(tween, Tween.SignalName.Finished);
    }

}
