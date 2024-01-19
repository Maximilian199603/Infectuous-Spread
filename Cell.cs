using Godot;
using System;

public class Cell : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    private float Speed = 0.5f;

    private bool mouseInsideArea;

    private AnimatedSprite anim;
    private Area2D collisionShape;
    private Line2D indicator;

    private float _clickCooldown = 0f;
    private float _clickCoolDownMaxInSec = 0.25f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite>("AnimatedSprite");
        collisionShape = GetNode<Area2D>("Area2D");
        indicator = GetNode<Line2D>("Line2D");
        indicator.Visible = false;
        collisionShape.Connect("mouse_entered", this, "_OnMouseEntered");
        collisionShape.Connect("mouse_exited", this, "_OnMouseExit");
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
    }

    public override void _PhysicsProcess(float delta)
    {
        GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        Vector2 direction = RandomDirection();
        ChaseTarget(direction);
        anim.Play();
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

    private Vector2 RandomDirection()
    {
        RNGSUS rngsus = (RNGSUS)GetNode("/root/Rngsus");

        int first = rngsus.RandomInt();
        int second = rngsus.RandomInt();
        return new Vector2(first, second);
    }

    private void ChaseTarget(Vector2 destination)
    {
        Vector2 temp = destination;
        Vector2 tPos = (temp - Position).Normalized();
        if (Position.DistanceTo(temp) > 3)
        {
            var result = MoveAndCollide(tPos * Speed);
            //LookAt(tPos);
        }
    }
}
