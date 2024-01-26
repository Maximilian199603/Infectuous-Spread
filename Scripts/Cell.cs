using Godot;
using System;

public class Cell : CellAncestor
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    private float Speed = 0.5f;

    [Export]
    private float IncubationTime = 0f;

    [Export]
    private float PositionTiming = 0f;

    [Export]
    private int _SpawnCount = 0;

    private bool mouseInsideArea;

    private AnimatedSprite anim;
    private Area2D collisionShape;
    private SelectionIndicator indicator;
    private Label incubationIndicator;
    private ProgressBar incubationBar;
    private Timer timer;
    private Timer posTimer;
    private AnimatedSprite tail;

    private float _clickCooldown = 0f;
    private float _clickCoolDownMaxInSec = 0.25f;


    public bool Incubating { get; private set; }

    public int Id = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetRandomMovementTimer();
        InitSprite();
        InitCollider();
        InitSelectionIndicator();
        InitIncubIndicator();
        InitIncubTimer();
        InitPosTimer();
    }

    private void SetRandomMovementTimer()
    {
        double r = GD.RandRange(0, 1);
        PositionTiming = Convert.ToSingle(PositionTiming + 2 * r);
    }

    private void InitIncubTimer()
    {
        timer = GetNode<Timer>("Timer");
        timer.OneShot = true;
        timer.WaitTime = IncubationTime;
        timer.Connect("timeout", this, "_OnIncubationTimeEnd");
    }

    private void InitIncubIndicator()
    {
        incubationIndicator = GetNode<Label>("Label");
        incubationBar = GetNode<ProgressBar>("ProgressBar");
        SetIncubationLabelColour(new Color(0, 0, 0, 1));
        HideIncubationLabel();
    }

    private void InitSelectionIndicator()
    {
        indicator = GetNode<SelectionIndicator>("Indicator");
        indicator.Visible = false;
    }

    private void InitCollider()
    {
        collisionShape = GetNode<Area2D>("Area2D");
        collisionShape.Connect("mouse_entered", this, "_OnMouseEntered");
        collisionShape.Connect("mouse_exited", this, "_OnMouseExit");
    }

    private void InitSprite()
    {
        anim = GetNode<AnimatedSprite>("AnimatedSprite");
        tail = GetNode<AnimatedSprite>("Tail");
    }

    private void InitPosTimer()
    {
        posTimer = GetNode<Timer>("PositionTimer");
        posTimer.WaitTime = PositionTiming;
        posTimer.OneShot = false;
        posTimer.Connect("timeout", this, "_PosTimerElapsed");
        posTimer.Start();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        if (mouseInsideArea && Input.IsMouseButtonPressed((int)ButtonList.Left) && _clickCooldown <= 0f)
        {
            if (!Equals(global.CurrentTarget))
            {
                SelectThisInstance(global);
            }
            else
            {
                UnSelectThisInstance(global);
            }
        }
        _clickCooldown = Mathf.Clamp(_clickCooldown - delta, -_clickCoolDownMaxInSec, _clickCoolDownMaxInSec);
        UpdateIndicator(global);
        HandleIncubationIndicator2();
    }

    public override void _PhysicsProcess(float delta)
    {
        HandlePositionReached();
        ChaseTarget(currTargetPosition);
        anim.Play();
        tail.Play();
    }

    private void HandleIncubationIndicator()
    {
        if (Incubating)
        {
            ShowIncubationLabel();
            incubationIndicator.Text = timer.TimeLeft.ToString("0.0");
        }
        else
        {
            HideIncubationLabel();
        }
    }

    private void HandleIncubationIndicator2()
    {
        if (Incubating)
        {
            incubationBar.Visible = true;
            float value = IncubationTime - timer.TimeLeft;
            incubationBar.Value = value;
        }
        else
        {
            incubationBar.Visible = false;
        }
    }

    private void HandlePositionReached()
    {
        if (Position.DistanceTo(currTargetPosition) < 10)
        {
            _PosTimerElapsed();
        }
    }

    private void _OnMouseEntered()
    {
        GD.Print($"Entered");
        mouseInsideArea = true;
    }
    private void _OnMouseExit()
    {
        GD.Print($"Exited");
        mouseInsideArea = false;
    }

    private void _PosTimerElapsed()
    {
        Vector2 direction = GetRandomPositionInCircle(Position, 1000f);
        GD.Randomize();
        float r = GD.Randf();
        if (r <= 0.5)
        {
            currTargetPosition = -direction;
            return;
        }
        currTargetPosition = direction;
    }

    public Vector2 GetRandomPositionInCircle(Vector2 circleCenter, float radius)
    {
        Vector2 rand = new Vector2(GD.Randf(), GD.Randf()) * 4.0f + circleCenter;
        RandomPosition random = (RandomPosition)GetNode("/root/RandomPosition");
        return random.GetRandomPositionInCircle(rand, radius);
    }

    private void SelectThisInstance(GlobalValues global)
    {
        //GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        _clickCooldown = _clickCoolDownMaxInSec;
        global.CurrentTarget = this;
        GD.Print($"Selected");
    }

    private void UnSelectThisInstance(GlobalValues global)
    {
        //GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        _clickCooldown = _clickCoolDownMaxInSec;
        global.CurrentTarget = null;
        GD.Print("Unselected");
    }

    private void SetIndicator(bool val)
    {
        indicator.Visible = val;
    }

    private void UpdateIndicator(GlobalValues global)
    {
        SetIndicator(Equals(global.CurrentTarget));
    }

    public void SetIncubating(bool value)
    {
        Incubating = value;
        timer.Start();
    }

    private void ChaseTarget(Vector2 destination)
    {
        Vector2 temp = destination;
        Vector2 tPos = (temp - Position).Normalized();
        if (Position.DistanceTo(temp) > 3)
        {
            LookAt(tPos);
            KinematicCollision2D collisionresult = MoveAndCollide(tPos * Speed);
            if (collisionresult != null)
            {
                Godot.Object col = collisionresult.Collider;
                if (col.GetType().Equals(typeof(Virus)))
                {
                    HandleVirusCol(col);
                }
                else if (col.GetType().Equals(typeof(Cell)))
                {
                    HandleCellCol(col);
                }
                else if (col.GetType().Equals(typeof(ImmuneSystemCell)))
                {
                    HandleImCellCol(col);
                }
            }
        }
    }

    private void HandleVirusCol(Godot.Object col)
    {
        Virus v = col as Virus;
        if (Incubating)
        {
            currTargetPosition = GetRandomPositionInCircle(-v.Position, 1000);
            return;
        }


        v.KillThis();
        SetIncubating(true);

    }

    private void HandleCellCol(Godot.Object col)
    {
        Cell c = col as Cell;
        currTargetPosition = GetRandomPositionInCircle(-c.Position, 1000);
    }

    private void HandleImCellCol(Godot.Object col)
    {
        ImmuneSystemCell i = col as ImmuneSystemCell;
        currTargetPosition = GetRandomPositionInCircle(-i.Position, 1000);
    }

    private void _OnIncubationTimeEnd()
    {
        GD.Print("IncubationTimeEnd");
        Incubating = false;
        //TODO: Handle the spawning Logic here
        //Spawn the SpawnTarget SpawnAmount times at current position a little offset so now stacking occurs

        //Target the VirusManager and add Spawnamount at current pos
        VirusManager manager = (VirusManager)GetNode("/root/VirusManager");
        //TODO: Generate an Random Position around the cell
        for (int i = 0; i < _SpawnCount; i++)
        {
            manager.Add(GetRandomPositionInCircle(Position, 100f));
        }
        GD.Print("Did The Spawning");
        GD.Print("Removing self from Scene");
        QueueFree();
    }

    private void HideIncubationLabel()
    {
        incubationIndicator.Visible = false;
    }

    private void ShowIncubationLabel()
    {
        incubationIndicator.Visible = true;
    }

    private void SetIncubationLabelColour(Color col)
    {
        incubationIndicator.Modulate = col;
    }
}
