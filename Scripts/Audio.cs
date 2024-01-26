using Godot;

public class Audio : AudioStreamPlayer
{
    AudioStreamSample bgm = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bgm = GD.Load<AudioStreamSample>("res://Assets/Music/BGM.wav");
        Stream = bgm;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    //public override void _Process(float delta)
    //{
    //}
}
