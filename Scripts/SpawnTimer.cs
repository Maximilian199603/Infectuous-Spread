using Godot;
using System.Collections.Generic;

public class SpawnTimer : Node2D
{
    [Export]
    private float Duration;

    [Export]
    private bool Repeat;

    private List<Spawner> Spawns;
    private Timer timing;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InitTimer();
    }

    private void InitTimer()
    {
        timing = GetNode<Timer>("Timer");
        timing.WaitTime = Duration;
        timing.OneShot = !Repeat;
        timing.Connect("timeout", this, "_OnTimerElapse");
        Spawns = GetAllChildrenSpawners();
        timing.Start();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    private void _OnTimerElapse()
    {
        GD.Print("Spawning");
        foreach (var spawner in Spawns)
        {
            spawner.Spawn();
        }
    }

    private List<Spawner> GetAllChildrenSpawners()
    {
        List<Spawner> result = new List<Spawner>();
        var list = GetChildren();

        foreach (var item in list)
        {
            if (item.GetType().IsSubclassOf(typeof(Spawner)))
            {
                result.Add(item as Spawner);
            }
        }

        return result;
    }
}
