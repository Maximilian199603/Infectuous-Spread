using Godot;

public class VirusSpawner : Spawner
{
    [Export]
    private PackedScene SpawnTarget;

    private float time = 10f;
    private bool done = false;

    public override void _Ready()
    {
        if (SpawnTarget == null)
        {
            SpawnTarget = GD.Load<PackedScene>("res://Scenes/Virus.tscn");
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (time < 0 && !done)
        {
            Spawn();
            done = true;
        }
        time -= delta;
    }

    public override void Spawn()
    {
        var inst = SpawnTarget.Instance<Virus>();
        AddChild(inst);
    }
}
