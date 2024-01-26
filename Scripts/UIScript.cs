using Godot;

public class UIScript : Label
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        Text = global.AliveViruses.ToString();
    }
}
