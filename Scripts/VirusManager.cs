using Godot;
using System;
using System.Collections.Generic;

public class VirusManager : Node2D
{
    [Export]
    Vector2 Scalar { get; set; }

    private PackedScene Target = GD.Load<PackedScene>("res://Scenes/Virus.tscn");

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
        var inst = Target.Instance<Virus>();
        inst.Position = new Vector2(0, 0);
        inst.Scale = Scalar;
        AddChild(inst);
    }

    public void Add(Vector2 pos)
    {
        var inst = Target.Instance<Virus>();
        inst.Position = pos;
        inst.Scale = Scalar;
        AddChild(inst);
    }

    public Virus GetRandom()
    {
        List<Virus> list = GetAllViruses();
        Random rand = new Random();
        int index = rand.Next(list.Count);
        return list[index];
    }

    private List<Virus> GetAllViruses()
    {
        List<Virus> result = new List<Virus>();
        var list = GetChildren();

        foreach (var item in list)
        {
            if (item.GetType().Equals(typeof(Virus)))
            {
                result.Add(item as Virus);
            }
        }
        return result;
    }
}
