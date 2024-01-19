using Godot;
using System;

public class GlobalValues : Node
{
    public Vector2 Value { get; set; }
    public Virus Selected { get; set; }
    public Node2D CurrentTarget { get; set; }
    public Vector2 MiddleOfViewPort { get; private set; }
    public int LastId { get; set; } = 0;

    public int RandomLowerLimit { get; private set; }

    public int RandomUpperLimit { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Vector2 dim = GetViewport().Size;
        MiddleOfViewPort = new Vector2(dim.x / 2, -dim.y / 2);

        RandomLowerLimit = 0;
        RandomUpperLimit = 10000;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
