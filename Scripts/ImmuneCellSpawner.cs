using Godot;

public class ImmuneCellSpawner : Spawner
{
    private Timer _timer;

    [Export]
    private int Amount = 1;

    [Export]
    private float Radius = 1500f;

    [Export]
    private int Loops = 1;

    private Vector2 lastPos;

    private bool repeating = true;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        _timer = GetNode<Timer>("Timer");
        _timer.WaitTime = global.ImmuneCellSpawnInterval;
        _timer.OneShot = !repeating;
        _timer.Connect("timeout", this, "Spawn");
        _timer.Start();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
    }

    public override void Spawn()
    {
        ImmuneCellManager manager = (ImmuneCellManager)GetNode("/root/ImmuneCellManager");
        RandomPosition random = (RandomPosition)GetNode("/root/RandomPosition");
        for (int j = 0; j < Loops; j++)
        {
            for (int i = 0; i < Amount; i++)
            {
                manager.Add(random.GetRandomPositionInCircle(Position, Radius));
            }
            GD.Print("Spawned Immune Cells");
        }
    }
}
