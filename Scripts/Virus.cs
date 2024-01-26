using Godot;

public class Virus : KinematicBody2D
{
    [Export]
    private float Speed = 50f;
    [Export]
    private float RotationSpeed = 2.0f;
    [Export]
    private float DecayTime = 60f;

    private AnimatedSprite anim;
    private Area2D collisionShape;
    private SelectionIndicator indicator;
    private Node2D _target;
    private Vector2 _Position;
    private ProgressBar progressBar;
    private Timer decayTimer;
    private bool _HasDestination = false;


    private bool mouseInsideArea = false;
    private float _clickCooldown = 0f;
    private float _clickCoolDownMaxInSec = 0.25f;
    public int Id = -1;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        progressBar = GetNode<ProgressBar>("ProgressBar");
        InitDecayTimer();

        anim = GetNode<AnimatedSprite>("AnimatedSprite");
        collisionShape = GetNode<Area2D>("Area2D");
        indicator = GetNode<SelectionIndicator>("Indicator");
        indicator.Visible = false;
        collisionShape.Connect("mouse_entered", this, "_OnMouseEntered");
        collisionShape.Connect("mouse_exited", this, "_OnMouseExit");

        GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        Id = global.LastId + 1;
        global.LastId = Id;
        global.IncreaseAliveCount();
    }

    private void InitDecayTimer()
    {
        decayTimer = GetNode<Timer>("Timer");
        decayTimer.WaitTime = DecayTime;
        decayTimer.Connect("timeout", this, "_OnDecayTimeElapse");
        decayTimer.Start();
    }

    private void _OnDecayTimeElapse()
    {
        KillThis();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        UpdateProgressbar();
        GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        if (mouseInsideArea && Input.IsMouseButtonPressed((int)ButtonList.Left) && _clickCooldown <= 0f)
        {
            if (!Equals(global.Selected))
            {
                SelectThisInstance(global);
            }
            else
            {
                UnSelectThisInstance(global);
            }
        }
        UpdateIndicator(global);
        _clickCooldown = Mathf.Clamp(_clickCooldown - delta, -_clickCoolDownMaxInSec, _clickCoolDownMaxInSec);
    }

    private void UpdateProgressbar()
    {
        progressBar.Value = DecayTime - decayTimer.TimeLeft;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_target != null)
        {
            if (IsInstanceValid(_target))
            {
                ChaseTarget();
                anim.Play();
            }
        }
        else
        {
            Rotate(RotationSpeed * delta);
            anim.Play();
        }
    }

    private void _OnMouseEntered()
    {
        GD.Print($"Entered {Id}");
        mouseInsideArea = true;
    }
    private void _OnMouseExit()
    {
        GD.Print($"Exited {Id}");
        mouseInsideArea = false;
    }

    public void KillThis()
    {
        RemoveSelf();
    }

    private void SetIndicator(bool val)
    {
        indicator.Visible = val;
    }

    private void SelectThisInstance(GlobalValues global)
    {
        //GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        _clickCooldown = _clickCoolDownMaxInSec;
        global.Selected = this;
        GD.Print($"Active Virus {Id}");
    }

    private void UnSelectThisInstance(GlobalValues global)
    {
        //GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        _clickCooldown = _clickCoolDownMaxInSec;
        global.Selected = null;
        GD.Print("Unselected This");
    }

    private void UpdateIndicator(GlobalValues global)
    {
        SetIndicator(Equals(global.Selected));
    }

    public bool SetTarget(Node2D target)
    {
        _target = target;
        return true;
    }

    public bool SetDestination(Vector2 targetPosition)
    {
        _Position = targetPosition;
        return true;
    }

    public bool SetDestination(Node2D targetNode)
    {
        _Position = targetNode.Position;
        return true;
    }

    private void ChaseTarget()
    {
        Vector2 temp;
        if (!IsInstanceValid(_target))
        {
            _target = null;
            return;
        }
        else
        {
            temp = _target.Position;
        }
        Vector2 tPos = (temp - Position).Normalized();
        if (Position.DistanceTo(temp) > 3)
        {
            KinematicCollision2D collisionresult = MoveAndCollide(tPos * Speed);
            LookAt(tPos);
            if (collisionresult != null)
            {
                Godot.Object col = collisionresult.Collider;
                if (col.GetType().Equals(typeof(Cell)))
                {
                    HandleCellCollission(col);
                }
                else if (col.GetType().Equals(typeof(ImmuneSystemCell)))
                {
                    HandleImmuneSystemCellCollision(col);
                }
            }
        }
    }

    private void HandleCellCollission(Godot.Object col)
    {
        Cell partner = col as Cell;
        if (partner.Incubating)
        {
            return;
        }
        partner.SetIncubating(true);
        RemoveSelf();
        //TODO:
        //Delete the colliding virus in this case it is this 
    }

    private void HandleImmuneSystemCellCollision(Godot.Object col)
    {
        ImmuneSystemCell partner = col as ImmuneSystemCell;
        GD.Print("ImmuneCollision Happened");
        GD.Print("Prepping Remove Self");
        RemoveSelf();
        GD.Print("Queued Self for removal");
    }

    private void RemoveSelf()
    {
        GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        global.AliveViruses = Mathf.Clamp(global.AliveViruses - 1, 0, int.MaxValue);
        QueueFree();
    }
}
