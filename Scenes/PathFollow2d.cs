using Godot;
using System;

public partial class PathFollow2d : PathFollow2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var enemy = ResourceLoader.Load<PackedScene>("res://Enemies/Enemy1.tscn");
		var enemyIn = enemy.Instantiate<CharacterBody2D>();
		AddChild(enemyIn);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GetChildCount() == 0)
		{
			GetParent().RemoveChild(this);
			QueueFree();
		}
	}
}
