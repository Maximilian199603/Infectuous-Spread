using Godot;
using System;

public class ImmuneCellManager : Node
{
    [Export]
    Vector2 Scalar { get; set; }

    private PackedScene Target = GD.Load<PackedScene>("res://Scenes/ImmuneSystemCell.tscn");

    private int count = 0;

    [Export]
    private int maxCount = 1500;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Target == null)
        {
            throw new ArgumentNullException(nameof(Target));
        }

        if (Scalar.Equals(Vector2.Zero))
        {
            Scalar = new Vector2(0.5f, 0.5f);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
    }

    public void Add()
    {
        if (count >= maxCount)
        {
            return;
        }
        var inst = Target.Instance<ImmuneSystemCell>();
        inst.Position = new Vector2(0, 0);
        inst.Scale = Scalar;
        AddChild(inst);
        count++;
    }

    public void Add(Vector2 pos)
    {
        if (count >= maxCount)
        {
            return;
        }
        var inst = Target.Instance<ImmuneSystemCell>();
        inst.Position = pos;
        inst.Scale = Scalar;
        AddChild(inst);
        count++;
    }
}
