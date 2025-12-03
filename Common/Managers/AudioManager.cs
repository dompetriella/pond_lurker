using Godot;
using System;
using System.Threading.Tasks;

public partial class AudioManager : Node
{
    public static AudioManager Instance { get; private set; }

    [Signal]
    public delegate void MusicStartedEventHandler(AudioStream audioStream);

    [Signal]
    public delegate void MusicStoppedEventHandler(AudioStream audioStream);

    [Signal]
    public delegate void MusicPausedEventHandler(AudioStream audioStream);

    [Signal]
    public delegate void MusicResumedEventHandler(AudioStream audioStream);

    [Signal]
    public delegate void SoundEffectPlayedEventHandler(AudioStream audioStream);

    [Signal]
    public delegate void SoundEffectEndedEventHandler(AudioStream audioStream);

    [Export]
    private AudioStreamPlayer MusicNode;

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }

    public async void StartMusicTrack(AudioStream audioStream, float fadeInTimeInMs = 0.0f, bool shouldRepeat = true)
    {
        if (MusicNode.Playing)
        {
            MusicNode.Stop();
            MusicNode.VolumeDb = 0;
        }

        MusicNode.Stream = PrepareStream(audioStream: audioStream, shouldRepeat: shouldRepeat);

        if (fadeInTimeInMs > 0.0f)
        {
            float fadeInTime = fadeInTimeInMs / 1000f;
            MusicNode.VolumeDb = -80;
            MusicNode.Play();
            Tween musicTween = GetTree().CreateTween();
            musicTween.TweenProperty(MusicNode, AudioStreamPlayerProperties.VolumeDb, 0, fadeInTime);
            await ToSignal(musicTween, Tween.SignalName.Finished);
        }

        MusicNode.Play();
        EmitSignal(SignalName.MusicStarted);
    }

    public async void StopMusicTrack(float fadeOutTimeInMs = 0.0f)
    {
        if (MusicNode.Playing)
        {
            if (fadeOutTimeInMs > 0)
            {
                float fadeOutTime = fadeOutTimeInMs / 1000f;
                Tween musicTween = GetTree().CreateTween();
                musicTween.TweenProperty(MusicNode, AudioStreamPlayerProperties.VolumeDb, -80, fadeOutTime);
                await ToSignal(musicTween, Tween.SignalName.Finished);
            }

            MusicNode.Stop();
            EmitSignal(SignalName.MusicStopped);
        }
    }

    public async void ResumeMusic(float fadeInTimeInMs = 0.0f)
    {
        if (!MusicNode.Playing)
        {
            if (fadeInTimeInMs > 0)
            {
                float fadeInTime = fadeInTimeInMs / 1000f;
                MusicNode.VolumeDb = -80;
                MusicNode.Play();
                Tween musicTween = GetTree().CreateTween();
                musicTween.TweenProperty(MusicNode, AudioStreamPlayerProperties.VolumeDb, 0, fadeInTime);
                await ToSignal(musicTween, Tween.SignalName.Finished);
            }
            MusicNode.Play();
            EmitSignal(SignalName.MusicResumed);
        }
    }

    public async void PauseMusic(float fadeOutTimeInMs = 0.0f)
    {
        if (!MusicNode.Playing)
        {
            GD.PushWarning("Cannot pause music - no music playing");
            return;
        }

        if (fadeOutTimeInMs > 0)
        {
            float fadeOutTime = fadeOutTimeInMs / 1000f;
            Tween musicTween = GetTree().CreateTween();
            musicTween.TweenProperty(MusicNode, AudioStreamPlayerProperties.VolumeDb, -80, fadeOutTime);
            await ToSignal(musicTween, Tween.SignalName.Finished);
        }

        MusicNode.StreamPaused = true;
        EmitSignal(SignalName.MusicResumed);
    }

    public void PlayMusic(AudioStream audioStream, float fadeInTimeInMs = 0.0f, float previousTrackFadeOutTimeInMs = 0.0f, bool shouldRepeat = true)
    {
        StopMusicTrack(fadeOutTimeInMs: previousTrackFadeOutTimeInMs);
        StartMusicTrack(audioStream: audioStream, fadeInTimeInMs: fadeInTimeInMs);
    }

    public async Task PlaySoundEffect(AudioStream soundEffect)
    {
        AudioStreamPlayer audioStreamPlayer = new AudioStreamPlayer();
        AddChild(audioStreamPlayer);

        audioStreamPlayer.Stream = soundEffect;
        audioStreamPlayer.Play();
        EmitSignal(SignalName.SoundEffectPlayed);

        await ToSignal(audioStreamPlayer, AudioStreamPlayer.SignalName.Finished);

        audioStreamPlayer.QueueFree();
        EmitSignal(SignalName.SoundEffectEnded);
    }

    private static AudioStream PrepareStream(AudioStream audioStream, bool shouldRepeat)
    {
        if (audioStream == null)
        {
            GD.PushWarning("Audio stream cannot be played - is null");
            return null;
        }

        if (audioStream.Duplicate() is not AudioStream streamCopy)
        {
            GD.PushWarning("Audio stream parameter is not an usable AudioStream");
            return null;
        }

        if (shouldRepeat)
        {
            switch (streamCopy)
            {
                case AudioStreamOggVorbis ogg:
                    ogg.Loop = shouldRepeat;
                    break;

                case AudioStreamMP3 mp3:
                    mp3.Loop = shouldRepeat;
                    break;

                case AudioStreamWav wav:
                    wav.LoopMode = shouldRepeat ? AudioStreamWav.LoopModeEnum.Forward : AudioStreamWav.LoopModeEnum.Disabled;
                    break;
            }
        }

        return streamCopy;
    }
}
