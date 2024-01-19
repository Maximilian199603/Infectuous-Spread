using Godot;
using System;

public class ClickDetector : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        // Check if the left mouse button is pressed
        if (Input.IsMouseButtonPressed((int)ButtonList.Right))
        {
            GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
            if (global.Selected != null && global.CurrentTarget != null)
            {
                bool result = global.Selected.SetTarget(global.CurrentTarget);
                if (result)
                {
                    global.CurrentTarget = null;
                    global.Selected = null;
                }
            }
        }
    }
}
