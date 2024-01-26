using Godot;

public class Spawner : Node2D
{
    private PackedScene SpawnTarget;

    private Timer timing;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        timing = GetNode<Timer>("Timer");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
    }

    public virtual void Spawn()
    {
    }
}
