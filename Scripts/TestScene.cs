using Godot;

public class TestScene : Node2D
{
    private Camera2D main;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        main = GetNode<Camera2D>("Camera2D");
        main.Current = true;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
