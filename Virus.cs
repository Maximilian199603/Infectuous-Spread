using Godot;
using System;

public class Virus : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    private float Speed = 50f;
    [Export]
    private float RotationSpeed = 2.0f;

    private AnimatedSprite anim;
    private Node2D _target;
    private Area2D collisionShape;
    private Line2D indicator;

    private bool mouseInsideArea = false;
    private float _clickCooldown = 0f;
    private float _clickCoolDownMaxInSec = 0.25f;
    public int Id = -1;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite>("AnimatedSprite");
        collisionShape = GetNode<Area2D>("Area2D");
        indicator = GetNode<Line2D>("Line2D");
        indicator.Visible = false;
        collisionShape.Connect("mouse_entered", this, "_OnMouseEntered");
        collisionShape.Connect("mouse_exited",this,"_OnMouseExit");

        GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        Id = global.LastId + 1;
        global.LastId = Id;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
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
        _clickCooldown = Mathf.Clamp(_clickCooldown - delta, -_clickCoolDownMaxInSec,_clickCoolDownMaxInSec);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_target != null)
        {
            ChaseTarget();
            anim.Play();
        }
        else
        {
            //Rotate(RotationSpeed * delta);
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

    private void ChaseTarget()
    {
        Vector2 temp = _target.Position;
        Vector2 tPos = (temp - Position).Normalized();
        if (Position.DistanceTo(temp) > 3)
        {
            KinematicCollision2D result = MoveAndCollide(tPos * Speed);
            //LookAt(tPos);
            var col = result.Collider;
            //TODO:
            //On collision set the cell to incubating
            //Delete the colliding virus in this case it is this 
        }
    }
}
