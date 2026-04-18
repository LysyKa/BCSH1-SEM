using Godot;
using System;

public partial class TowerContainer : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void _add_tower_at(Vector2 position)
	{
		var tower = ResourceLoader.Load<PackedScene>("res://Towers/Tower2D.tscn");
		var towerIn = tower.Instantiate<Tower2d>();
		towerIn.Position = position;
		AddChild(towerIn);
	}

}
