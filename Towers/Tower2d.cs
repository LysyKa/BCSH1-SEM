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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isFake)
		{
			return;
		}
		targets = GetNode<Area2D>("TowerArea2D").GetOverlappingBodies();

		var actualTargets = targets.Where(target => target is Enemy).ToArray();

		targets = new Godot.Collections.Array<Node2D>(actualTargets);
		int currTargetCount = targetCount;

		while (targets.Count > 0 && currTargetCount > 0)
		{
			// currentTarget = targets[0];
			currentTarget = targets.OrderByDescending(target => target.GetParent().GetParent().GetNode<PathFollow2D>("PathFollow2D").ProgressRatio).FirstOrDefault();
			/*if (frameCounter % ((int)60 / attackSpeed) == 0)
			{
				currentTarget.GetParent().GetNode<Enemy>("Enemy").Life -= bulletDamage;
				GD.Print("Attacking target: " + currentTarget.Name + " - Remaining Life " + ((Enemy)currentTarget).Life);

			}*/

			RotateTowards(currentTarget.GlobalPosition);

			if (attackSpeed <= 0)
			{
				return; // Prevent division by zero or negative attack speed
			}
			if (frameCounter % (60 / attackSpeed) == 0)
			{
				Projectile2d proj = projectile.Instantiate<Projectile2d>();
				proj.GlobalPosition = GetNode<Marker2D>("Marker2D").GlobalPosition;
				proj.CurrentTarget = currentTarget;
				proj.Damage = bulletDamage;
				GetParent().AddChild(proj);
			}
			targets.Remove(currentTarget);
			currTargetCount--;

		}
		frameCounter++;
		if (framesForOutput > 0)
		{
			// framesForOutput--;
		}
		else
		{
			//GD.Print("Current targets: " + targets.Count + " - " + string.Join(", ", targets));
			//framesForOutput = 60;
		}
	}
		private void RotateTowards(Vector2 targetPosition)
	{
		Vector2 direction = targetPosition - GlobalPosition;
		float angle = Mathf.Atan2(direction.Y, direction.X);
		Rotation = angle;
	}

}
