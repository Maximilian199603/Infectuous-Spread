using Godot;

public class World : Node
{
    // Called when the node enters the scene tree for the first time.
    private CameraScript main;


    private bool _GameOver = false;
    public override void _Ready()
    {
        GD.Randomize();
        main = GetNode<CameraScript>("MoveableCamera");
        main.Current = true;
        SpawnInitialViruses();
        SpawnInitialCells();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_GameOver)
        {
            GetTree().Paused = true;
            return;
        }

        GlobalValues glob = (GlobalValues)GetNode("/root/GlobalValues");
        if (glob.AliveViruses <= 0)
        {
            CellManager manager = (CellManager)GetNode("/root/CellManager");
            if (!manager.IsAnynoneIncubatingFast())
            {
                Gameover();
            }
        }


    }

    private void Gameover()
    {
        _GameOver = true;
        main.GameOver = true;
    }

    public Vector2 GetRandomPositionInCircle(Vector2 circleCenter, float radius)
    {
        RandomPosition random = (RandomPosition)GetNode("/root/RandomPosition");
        return random.GetRandomPositionInCircle(circleCenter, radius);
    }

    private void SpawnInitialViruses()
    {
        RandomPosition rand = (RandomPosition)GetNode("/root/RandomPosition");
        VirusManager manager = (VirusManager)GetNode("/root/VirusManager");
        Vector2 zero = Vector2.Zero;
        manager.Add(zero);
        for (int i = 0; i < 3; i++)
        {
            Vector2 random = GetRandomPositionInCircle(Vector2.Zero, 300f);
            manager.Add(random);
        }
    }

    private void SpawnInitialCells()
    {
        RandomPosition rand = (RandomPosition)GetNode("/root/RandomPosition");
        CellManager manager = (CellManager)GetNode("/root/CellManager");

        for (int i = 0; i < 4; i++)
        {
            Vector2 random = GetRandomPositionInCircle(Vector2.Zero, 400f);
            manager.Add(random);
        }

        for (int i = 0; i < 10; i++)
        {
            Vector2 random = GetRandomPositionInCircle(Vector2.Zero, 800f);
            manager.Add(random);
        }
    }

    private void SpawnInitialImmuneCells()
    {
        RandomPosition rand = (RandomPosition)GetNode("/root/RandomPosition");
        ImmuneCellManager manager = (ImmuneCellManager)GetNode("/root/ImmuneCellManager");
        for (int i = 0; i < 10; i++)
        {
            Vector2 random = GetRandomPositionInCircle(Vector2.Zero, 200f);
            manager.Add(random);
        }
    }
}
