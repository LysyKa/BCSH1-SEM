using Godot;
using System;
using System.Collections;
using System.ComponentModel;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public int Damage { get; set; } = 1;
	[Export]
	public int Speed { get; set; } = 100;
	[Export]
	public int Bounty { get; set; } = 1;
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
				// GetParent().RemoveChild(this);
				this.GetParent().QueueFree();
				return;
			}
			EmitSignal(SignalName.HealthChanged);
		}
	}

	public String spritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile245.png";


	public int progress;
	[Signal]
	public delegate void HealthChangedEventHandler(int newLife);
	[Signal]
	public delegate void EnemyDiedEventHandler(int bounty);
	[Signal]
	public delegate void EnemyPassedEventHandler(int damage);

	/*public Enemy(int damage, int speed, int bounty, int life)
	{
		this.Damage = damage;
		this.Speed = speed;
		this.Bounty = bounty;
		this.life = life;
	}
	public Enemy(int damage, int speed, int bounty, int life, String spritepath)
	{
		this.Damage = damage;
		this.Speed = speed;
		this.Bounty = bounty;
		this.Life = life;
		this.spritePath = spritepath;
	}*/



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Sprite2D>("Sprite").Texture = ResourceLoader.Load<Texture2D>(spritePath);
		((PathFollow2D)GetParent()).Progress = 1;
		((PathFollow2D)GetParent()).Loop = false;
		
		Speed = Speed + (int)(GD.Randf() * 2 - 1); // Randomize speed a bit
		Connect(SignalName.EnemyDied, new Callable(GetNode<Node2D>("/root/Main_Scene/PlayerStats"), "_enemyDiedEventHandler"));
		Connect(SignalName.EnemyPassed, new Callable(GetNode<Node2D>("/root/Main_Scene/PlayerStats"), "_enemyPassedEventHandler"));

		//remember make healthbar
		//var screenPos = GetViewport().GetCamera2D().ToScreen(globalPosition);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		((PathFollow2D)GetParent()).ProgressRatio += (float)delta * Speed * 0.0005f;
		if (((PathFollow2D)GetParent()).ProgressRatio >= 1)
		{
			//GD.Print("Enemy reached the end of the path");
			EmitSignal(SignalName.EnemyPassed, Damage);
			//GetParent().RemoveChild(this);
			this.GetParent().QueueFree();
		}
	}


	public int GetDamaged(int damage)
	{
		Life -= damage;
		return Life;
	}


}
