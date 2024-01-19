using Godot;
using System;
using System.Collections.Generic;

public class PlayerTest : Node
{
    [Export]
    private PackedScene virusScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //virusScene = GD.Load<PackedScene>("res://Virus.tscn");

        //for (int i = 0; i < 3; i++)
        //{
        //    Node virusInstance = virusScene.Instance();

        //    Node2D cast = (Node2D)virusInstance;
        //    Virus cast2 = (Virus)cast;

        //    AddChild(cast2);
        //    GD.Print("Spawned Child Virus Id:" + cast2.Id);

        //    cast.Position = new Vector2 (i * 100, i * 100);
        //}
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
