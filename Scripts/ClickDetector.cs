using Godot;

public class ClickDetector : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (!Input.IsMouseButtonPressed((int)ButtonList.Right))
        {
            return;
        }

        GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        Cell c = global.CurrentTarget as Cell;
        Virus v = global.Selected;
        if (v == null || !IsInstanceValid(v) || c == null || !IsInstanceValid(v))
        {
            return;
        }
        if (IsInstanceValid(c) && c.Incubating)
        {
            ClearSelections(global);
            return;
        }
        _ = v.SetTarget(c);
        ClearSelections(global);
    }

    private void ClearSelections(GlobalValues global)
    {
        global.CurrentTarget = null;
        global.Selected = null;
    }
}
