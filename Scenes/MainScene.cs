using Godot;
using System;

public partial class MainScene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Node2D>("IngameItems").Visible = false;
		GetNode<Node2D>("PlayerStats").Visible = false;
		GetNode<CanvasLayer>("UICanvasLayer").Visible = false;
		LoadScreen("res://Title/TitleScreen.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LoadScreen(string scenePath)
	{
		var map = ResourceLoader.Load<PackedScene>(scenePath).Instantiate();
		AddChild(map);
	}


}
