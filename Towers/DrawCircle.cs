using Godot;
using System;

public partial class DrawCircle : Node2D
{
    public float Radius = 50f;
    public Color CircleColor = new Color(1, 1, 1, 0.2f); // transparent white

    public override void _Draw()
    {
        DrawCircle(Vector2.Zero, Radius, CircleColor);
    }

    public override void _Ready()
    {
	   QueueRedraw(); // forces _Draw to be called
    }
}
