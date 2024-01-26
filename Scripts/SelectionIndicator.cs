using Godot;

public class SelectionIndicator : Node2D
{
    private Line2D top;
    private Line2D bottom;
    private Line2D left;
    private Line2D right;

    [Export]
    private Color color { get; set; }
    public new bool Visible { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        top = GetNode<Line2D>("Top");
        bottom = GetNode<Line2D>("Bottom");
        left = GetNode<Line2D>("Left");
        right = GetNode<Line2D>("Right");
    }

    public new void Show()
    {
        Visible = true;
    }

    public new void Hide()
    {
        Visible = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (!top.Modulate.Equals(color))
        {
            top.Modulate = color;
            bottom.Modulate = color;
            left.Modulate = color;
            right.Modulate = color;
        }

        if (!top.Visible.Equals(Visible))
        {
            top.Visible = Visible;
            bottom.Visible = Visible;
            left.Visible = Visible;
            right.Visible = Visible;
        }
    }
}
