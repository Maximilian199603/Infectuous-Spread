using Godot;
using System;

public class RNGSUS : Node
{
    public int RandomLowerLimit { get; private set; }

    public int RandomUpperLimit { get; private set; }

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        rng.Seed = (ulong)DateTime.UtcNow.Ticks;

        RandomLowerLimit = 0;
        RandomUpperLimit = 10000;
    }

    public int RandomInt()
    {
        return rng.RandiRange(RandomLowerLimit, RandomUpperLimit);
    }

    public float RandomFloat()
    {
        return rng.RandfRange(RandomLowerLimit, RandomUpperLimit);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
