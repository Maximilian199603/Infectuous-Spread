using Godot;

public class GlobalValues : Node
{
    public Vector2 Value { get; set; }
    public Virus Selected { get; set; }
    public Node2D CurrentTarget { get; set; }
    public Vector2 MiddleOfViewPort { get; private set; } // Deprecated
    public int LastId { get; set; } = 0;
    public int AliveViruses { get; set; } = 0;
    public float CellSpawnInterval { get; set; } = 10f;
    public float ImmuneCellSpawnInterval { get; set; } = 35f;

    public int SpawnAmount = 2;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Vector2 dim = GetViewport().Size;
        MiddleOfViewPort = new Vector2(dim.x / 2, dim.y / 2);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
    }

    public void DecreaseAliveCount()
    {
        AliveViruses = Mathf.Clamp(AliveViruses - 1, 0, int.MaxValue);
    }

    public void IncreaseAliveCount()
    {
        AliveViruses = Mathf.Clamp(AliveViruses + 1, 0, int.MaxValue);
    }
}
