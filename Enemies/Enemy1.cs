using Godot;
using System;
using System.ComponentModel;

public partial class Enemy1 : CharacterBody2D
{

	[Export]
	public int Damage { get; set; } = 1;
	[Export]
	public int Speed { get; set; } = 1;
	[Export]
	int Bounty { get; set; } = 1;
	private int life = 10;
	[Export]
	public int Life
	{
		get => life;
		set
		{
			if (life == value)
				return;

			life = value;
			if (life <= 0)
			{
				// Emit a signal or call a method to handle enemy death
				//GD.Print("Enemy died");
				EmitSignal(SignalName.EnemyDied, Bounty);
				GetParent().RemoveChild(this);
			}
			EmitSignal(SignalName.HealthChanged);
		}
	}

	[Signal]
	public delegate void HealthChangedEventHandler(int newLife);
	[Signal]
	public delegate void EnemyDiedEventHandler(int bounty);
	[Signal]
	public delegate void EnemyPassedEventHandler(int damage);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		((PathFollow2D)GetParent()).Progress = 1;
		this.Speed = Speed + (int)(GD.Randf()*2 - 1); // Randomize speed a bit
		Connect(SignalName.EnemyDied, new Callable(GetNode<Node2D>("/root/Main_Scene/PlayerStats"),"_enemyDiedEventHandler"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		((PathFollow2D)GetParent()).ProgressRatio += (float)delta * Speed * 0.005f;
		if (((PathFollow2D)GetParent()).ProgressRatio >= 1)
		{
			//GD.Print("Enemy reached the end of the path");
			EmitSignal(SignalName.EnemyPassed, Damage);
			GetParent().RemoveChild(this);
			QueueFree();
		}
	}




}
