using Godot;
using System.Collections.Generic;

public class CellSpawner : Node2D
{
    private Timer _timer;

    private int Amount = 1;

    [Export]
    private float Radius = 1000f;

    private bool repeating = true;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        Amount = global.SpawnAmount;
        _timer = GetNode<Timer>("Timer");
        _timer.WaitTime = global.CellSpawnInterval;
        _timer.OneShot = !repeating;
        _timer.Connect("timeout", this, "Spawn");
        _timer.Start();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
    }

    public void Spawn()
    {
        CellManager manager = (CellManager)GetNode("/root/CellManager");
        RandomPosition random = (RandomPosition)GetNode("/root/RandomPosition");
        List<Vector2> posis = new List<Vector2>();

        for (int j = 0; j < Amount; j++)
        {
            Vector2 tempPos = random.GetRandomPositionInCircle(Position, Radius + (j * 10));
            if (posis.Contains(tempPos))
            {
                continue;
            }
            posis.Add(tempPos);
            manager.Add(tempPos);
        }
        GD.Print("Spawned Cells");
    }
}
