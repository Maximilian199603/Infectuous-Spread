using Godot;
using System;

public class RandomPosition : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public Vector2 GetRandomPositionInCircle(Vector2 circleCenter, float radius)
    {
        GD.Randomize();
        float angle = (float)(GD.Randf() * 2 * Math.PI);
        float x = circleCenter.x + (float)(radius * Math.Cos(angle));
        float y = circleCenter.y + (float)(radius * Math.Sin(angle));

        return new Vector2(x, y);
    }

    private (float first, float second) GetTup(Random gen)
    {
        double first = GetValue(GetRandoms(gen)) ?? throw new ArgumentNullException();
        double second = GetValue(GetRandoms(gen)) ?? throw new ArgumentNullException();
        return (Convert.ToSingle(first), Convert.ToSingle(second));
    }

    private double[] GetRandoms(Random gen)
    {
        double[] working = new double[2];
        for (int i = 0; i < working.Length; i++)
        {
            working[i] = gen.NextDouble();
        }
        return working;
    }

    private double? GetValue(double[] arr)
    {
        if (arr.Length != 2)
        {
            return null;
        }

        if (arr[0] < 0.5)
        {
            return arr[1];
        }
        else
        {
            return -arr[1];
        }
    }
}
