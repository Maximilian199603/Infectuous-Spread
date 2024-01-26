using Godot;

public class CameraScript : Camera2D
{
    [Export]
    private float Speed = 0f;

    private bool MouseFollow = false;

    private float _LockCooldownBase = 0.099f;

    private float _LockCooldown = 0f;

    private Node2D _Target = null;

    private Panel _GOver = null;

    public bool GameOver { get; set; } = false;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _GOver = GetNode<Panel>("GameOver");
        _GOver.Visible = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GlobalValues global = (GlobalValues)GetNode("/root/GlobalValues");
        _GOver.Visible = GameOver;
        //Movement Logic
        HandleInput(delta, global);
    }


    private void HandleInput(float delta, GlobalValues global)
    {
        if (GameOver)
        {
            return;
        }

        if (Input.IsActionPressed("camera_to_mouse"))
        {
            _LockCooldown -= delta;
            if (_LockCooldown <= 0)
            {
                MouseFollow = !MouseFollow;
                _LockCooldown = _LockCooldownBase;
            }
        }

        if (MouseFollow)
        {
            Position = GetViewport().GetMousePosition();
            HandleCameraCenterInput();
            return;
        }

        if (Input.IsActionPressed("camera_center"))
        {
            Position = new Vector2(0, 0);
            MouseFollow = false;
            return;
        }

        Vector2 pos = new Vector2();
        if (Input.IsActionPressed("ui_up"))
        {
            pos.y += -Speed;
        }
        if (Input.IsActionPressed("ui_down"))
        {
            pos.y += Speed;
        }

        if (Input.IsActionPressed("ui_left"))
        {
            pos.x += -Speed;
        }
        if (Input.IsActionPressed("ui_right"))
        {
            pos.x += Speed;
        }
        Position += pos.Normalized() * Speed;
    }

    public void HandleCameraCenterInput()
    {
        if (Input.IsActionPressed("camera_center"))
        {
            Position = new Vector2(0, 0);
            MouseFollow = false;
            return;
        }
    }
}
