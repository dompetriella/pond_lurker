using Godot;
using System;

public partial class Notification : MarginContainer
{
    public string Message;

    [Export]
    public RichTextLabel MessageLabel;

    [Export]
    public TextureRect TextureRectImage;

    [Export]
    public Texture2D MessageImage;

    [Export]
    public Control MessageSpacing;

    public override void _Ready() {
        base._Ready();

        MessageLabel.Text = Message;
        TextureRectImage.Texture = MessageImage;

        if (MessageImage == null)
        {
            MessageSpacing.QueueFree();
        }
    }

}
