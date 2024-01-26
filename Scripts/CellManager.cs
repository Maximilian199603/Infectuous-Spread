using Godot;
using System;
using System.Collections.Generic;

public class CellManager : Node
{
    [Export]
    Vector2 Scalar { get; set; }

    private PackedScene Target = GD.Load<PackedScene>("res://Scenes/Cell.tscn");

    private int count = 0;

    [Export]
    private int maxCount = 2000;

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
        var inst = Target.Instance<Cell>();
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
        var inst = Target.Instance<Cell>();
        inst.Position = pos;
        inst.Scale = Scalar;
        AddChild(inst);
        count++;
    }

    public bool IsAnyoneIncubating()
    {
        List<Cell> cells = GetChildCells();

        foreach (Cell c in cells)
        {
            if (c.Incubating)
            {
                return true;
            }
        }
        return false;
    }

    private List<Cell> GetChildCells()
    {
        List<Cell> cells = new List<Cell>();
        var list = GetChildren();
        foreach (var child in list)
        {
            if (child.GetType().Equals(typeof(Cell)))
            {
                cells.Add(child as Cell);
            }
        }
        return cells;
    }

    public bool IsAnynoneIncubatingFast()
    {
        var list = GetChildren();
        foreach (var child in list)
        {
            Cell c = child as Cell;
            if (c.Incubating)
            {
                return true;
            }
        }
        return false;
    }

    private List<Spawner> GetAllChildrenSpawners()
    {
        List<Spawner> result = new List<Spawner>();
        var list = GetChildren();

        foreach (var item in list)
        {
            if (item.GetType().IsSubclassOf(typeof(Spawner)))
            {
                result.Add(item as Spawner);
            }
        }

        return result;
    }
}
