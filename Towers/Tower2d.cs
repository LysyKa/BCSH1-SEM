using Godot;
using System;
using System.Linq;
using System.Linq.Expressions;

public partial class Tower2d : StaticBody2D
{

	public int framesForOutput = 60;

	public int frameCounter = 0; //  attack speed frames counter
	[Export]
	public int bulletDamage = 5; // damage per attack
	[Export]
	public double attackSpeed = 2; // attacks per second
	[Export]
	public int targetCount = 1; // number of targets to attack simultaneously
	[Export]
	public int range = 500; // range in pixels
	[Export]
	public int cost = 5; // gold cost
	[Export]
	public double rotationSpeed = 3D; // radians per second
	[Export]
	public double fireAngleThreshold = 0.1D; // how close to target angle before firing
	public Timer attackTimer;
	private double currDelta;
	private double currAngleDifference;

	public bool isFake = false;
	public Godot.Collections.Array<Node2D> targets = new Godot.Collections.Array<Node2D>();
	public Node2D currentTarget;

	public PackedScene projectile = ResourceLoader.Load<PackedScene>("res://Projectiles/Projectile2D.tscn");
	public String projectilePath = "";
	public String spritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile249.png";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Area2D>("TowerArea2D").GetNode<CollisionShape2D>("TowerRangeCollisionShape2D").Shape = new CircleShape2D() { Radius = range };
		// GetNode<Sprite2D>("Sprite").Texture.ResourcePath = spritePath;
		GetNode<Sprite2D>("Sprite").Texture = ResourceLoader.Load<Texture2D>(spritePath);
		attackTimer = new Timer();
		if (attackSpeed > 0)attackTimer.WaitTime = 1/attackSpeed; else attackTimer.WaitTime = 0;
		attackTimer.Timeout += tryShoot;
		this.AddChild(attackTimer);
		attackTimer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		currDelta = delta;
		if (isFake)
		{
			return;
		}
		targets = GetNode<Area2D>("TowerArea2D").GetOverlappingBodies();
		targets = new Godot.Collections.Array<Node2D>(targets.Where(target => target is Enemy).ToArray());

		if (targets.Count > 0)
		{
			currentTarget = targets.OrderByDescending(target => target.GetParent().GetParent().GetNode<PathFollow2D>("PathFollow2D").ProgressRatio).FirstOrDefault();
			RotateTowards(currentTarget.GlobalPosition, currDelta);
		}

	}
	private void tryShoot()
	{
		if (isFake)
		{
			return;
		}
		int currTargetCount = targetCount;
		var currTargets = targets;
		while (currTargets.Count > 0 && currTargetCount > 0)
		{
			currentTarget = currTargets.OrderByDescending(target => target.GetParent().GetParent().GetNode<PathFollow2D>("PathFollow2D").ProgressRatio).FirstOrDefault();
			currAngleDifference = RotateTowards(currentTarget.GlobalPosition, currDelta);

			if (currAngleDifference < fireAngleThreshold)
			{
				Projectile2d proj = projectile.Instantiate<Projectile2d>();
				proj.GlobalPosition = GetNode<Marker2D>("Marker2D").GlobalPosition;
				proj.CurrentTarget = currentTarget;
				proj.Damage = bulletDamage;
				GetParent().AddChild(proj);
			}
			currTargets.Remove(currentTarget);
			currTargetCount--;
		}
	}
	private double RotateTowards(Vector2 targetPosition, double delta)
	{
		// Vector2 direction = targetPosition - GlobalPosition;
		// float angle = Mathf.Atan2(direction.Y, direction.X);
		// Rotation = angle;

		Vector2 direction = targetPosition - GlobalPosition;
		double targetAngle = Mathf.Atan2(direction.Y, direction.X);
		Rotation = (float)Mathf.RotateToward(Rotation, targetAngle, rotationSpeed * delta);
		return Mathf.Abs(Mathf.AngleDifference(Rotation, targetAngle));

	}





}
