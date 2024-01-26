using Godot;

public class DirectionIndicator : Node2D
{
    private CellAncestor parent;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        parent = GetParent<CellAncestor>();
        Visible = true;
        GD.Print(parent);
    }

    public override void _Draw()
    {
        GD.Print("Drawing Line To Dest");
        DrawLineTo(new Color(0, 0, 0, 1), 3.0f);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
    }

    private void DrawLineTo(Color col, float thickness)
    {
        DrawLine(Position, parent.currTargetPosition, col, thickness);
    }
}
