using Godot;

public class ImmuneSystemCell : CellAncestor
{
    [Export]
    private float Speed = 0f;

    [Export]
    private float PositionTiming;

    private Area2D detectionArea;
    private AnimatedSprite anim;
    private AnimatedSprite tail;
    private Virus _target;
    private Timer posTimer;
    private bool HasTarget => _target != null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        detectionArea = GetNode<Area2D>("Area2D");
        detectionArea.Monitoring = true;
        _ = detectionArea.Connect("area_entered", this, nameof(_OnAreaEnter));
        anim = GetNode<AnimatedSprite>("AnimatedSprite");
        tail = GetNode<AnimatedSprite>("Tail");
        InitPosTimer();
    }

    private void InitPosTimer()
    {
        posTimer = GetNode<Timer>("PositionTimer");
        posTimer.WaitTime = PositionTiming;
        posTimer.OneShot = false;
        posTimer.Connect("timeout", this, "_PosTimerElapsed");
        posTimer.Start();
    }

    private void _PosTimerElapsed()
    {
        if (HasTarget)
        {
            return;
        }
        Vector2 direction = GetRandomPositionInCircle(Position, 1000f);
        currTargetPosition = direction;
    }

    public Vector2 GetRandomPositionInCircle(Vector2 Position, float radius)
    {
        RandomPosition random = (RandomPosition)GetNode("/root/RandomPosition");
        return random.GetRandomPositionInCircle(Position, radius);
    }

    private void HandlePositionReached()
    {
        if (Position.DistanceTo(currTargetPosition) < 10)
        {
            _PosTimerElapsed();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (!detectionArea.Monitoring && !HasTarget)
        {
            detectionArea.Monitoring = true;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!IsInstanceValid(_target))
        {
            _target = null;
            return;
        }

        if (HasTarget)
        {
            currTargetPosition = _target.Position;
            detectionArea.Monitoring = false;
            ChaseTarget(currTargetPosition);
            Animate();
            return;
        }
        else
        {
            HandlePositionReached();
            ChaseTarget(currTargetPosition);
            Animate();
        }
    }

    private void Animate()
    {
        anim.Play();
        tail.Play();
    }

    private void SetTargetPos(Vector2 tPos)
    {
        currTargetPosition = tPos;
    }

    private void ChaseTarget()
    {
        if (IsInstanceValid(_target))
        {
            _target = null;
            return;
        }
        Vector2 temp = _target.Position;
        Vector2 tPos = (temp - Position).Normalized();
        if (Position.DistanceTo(temp) > 3)
        {
            KinematicCollision2D collisionresult = MoveAndCollide(tPos * Speed);
            LookAt(tPos);

            if (collisionresult != null)
            {
                HandleCol(collisionresult);
            }
        }
    }

    private void ChaseTarget(Vector2 destination)
    {
        Vector2 temp = destination;
        Vector2 tPos = (temp - Position).Normalized();
        if (Position.DistanceTo(temp) > 3)
        {
            if (Position.DistanceTo(temp) > 3)
            {
                KinematicCollision2D collisionresult = MoveAndCollide(tPos * Speed);
                LookAt(tPos);

                if (collisionresult != null)
                {
                    HandleCol(collisionresult);
                }
            }
        }
    }

    private void HandleCol(KinematicCollision2D collisionresult)
    {
        Godot.Object col = collisionresult.Collider;
        if (col.GetType().Equals(typeof(Virus)))
        {
            //GD.Print("Immune Collides with Virus ");
            HandleVirusCollision(col);
            return;
        }
        if (col.GetType().Equals(typeof(Cell)))
        {
            //GD.Print("Immune Collides with Cell ");
            HandleCellCol(col);
            return;
        }
        if (col.GetType().Equals(typeof(ImmuneSystemCell)))
        {
            //GD.Print("Immune Collides with Immune ");
            HandleImCellCol(col);
            return;
        }
    }

    private void HandleVirusCollision(Godot.Object col)
    {
        if (!IsInstanceValid(col))
        {
            return;
        }
        Virus c = col as Virus;
        if (c.Equals(_target))
        {
            _target.KillThis();
            _target = null;
        }
        else
        {
            c.KillThis();
        }
        RemoveSelf();
    }

    private void HandleCellCol(Godot.Object col)
    {
        Cell c = col as Cell;
        //_target = null;
        currTargetPosition = GetRandomPositionInCircle(-c.Position, 1000);
    }

    private void HandleImCellCol(Godot.Object col)
    {
        ImmuneSystemCell i = col as ImmuneSystemCell;
        //_target = null;
        currTargetPosition = GetRandomPositionInCircle(-i.Position, 1000);
    }

    private void RemoveSelf()
    {
        QueueFree();
    }


    private void _OnAreaEnter(Area2D area)
    {
        //GD.Print("Something entered");
        Node parent = area.GetParent();
        GD.Print(parent.GetType());
        if (parent.GetType().Equals(typeof(Virus)))
        {
            _target = parent as Virus;
        }
        //else if (parent.GetType().Equals(typeof(Cell)))
        //{
        //    _target = null;
        //}
        //else if (parent.GetType().Equals(typeof(ImmuneSystemCell)))
        //{
        //    _target = null;
        //}
    }

}
