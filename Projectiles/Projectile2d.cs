using Godot;
using System;
using System.Diagnostics.Metrics;

public partial class Projectile2d : CharacterBody2D
{
	public float Speed { get; set; } = 500.0f;
	public int Damage { get; set; } = 1;
	public Node2D CurrentTarget { get; set; }
	public int bounceCount = 0;
	bool haveCollided = false;
	bool canDelete = false;
	public Vector2 lastPosition;
	public String spritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile251.png";


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lastPosition = CurrentTarget.GlobalPosition;
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
		GetNode<Sprite2D>("Sprite").Texture = ResourceLoader.Load<Texture2D>(spritePath);
		// this.AddCollisionExceptionWith();
	}
	public override void _Process(double delta)
	{
		if (CurrentTarget is not null)
		{
			if (IsInstanceValid(CurrentTarget) && !CurrentTarget.IsQueuedForDeletion())
			{
				lastPosition = CurrentTarget.GlobalPosition;
				if (!haveCollided)
				{
					Vector2 diff = lastPosition - GlobalPosition;
					if (diff.LengthSquared() < 0.001f)
					{
						OnHit();
						return;
					}
					Vector2 direction = diff.Normalized();
					Velocity = direction * Speed;
					RotateTowards(lastPosition);
					MoveAndSlide();

					if (GlobalPosition.DistanceTo(lastPosition) < 25)
					{
						haveCollided = true;
						if (CurrentTarget is Enemy enemy)
						{
							int remainingLife = enemy.GetDamaged(Damage);
							//GD.Print("Projectile hit target: " + CurrentTarget.Name + " - Remaining Life " + remainingLife);
						}
						OnHit();
					}
					if (!IsInstanceValid(CurrentTarget) || CurrentTarget.IsQueuedForDeletion())
					{
						OnHit();
					}
				} else
				{
					//OnHit();
				}
			} else if (!IsInstanceValid(CurrentTarget) || CurrentTarget.IsQueuedForDeletion())
			{
				if (lastPosition == Vector2.Zero)
				{
					OnHit();
				}
				Vector2 diff = lastPosition - GlobalPosition;
				if (diff.LengthSquared() < 0.001f)
				{
					OnHit();
					return;
				}
				Vector2 direction = diff.Normalized();
				Velocity = direction * Speed;
				RotateTowards(lastPosition);
				MoveAndSlide();
				if (GlobalPosition.DistanceTo(lastPosition) < 10)
				{
					OnHit();
				}
			}
		}
		else
		{
			GD.Print("test");
			OnHit();
		}
	}
	public async void OnHit()
	{
		Speed = 0;

		GetNode<Sprite2D>("Sprite").Texture = ResourceLoader.Load<Texture2D>("res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile295.png");
		await ToSignal(GetTree().CreateTimer(0.2f, true), "timeout");
		GetNode<Sprite2D>("Sprite").Texture = ResourceLoader.Load<Texture2D>("res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile296.png");
		await ToSignal(GetTree().CreateTimer(0.2f, true), "timeout");
		GetNode<Sprite2D>("Sprite").Texture = ResourceLoader.Load<Texture2D>("res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile298.png");
		await ToSignal(GetTree().CreateTimer(0.2f, true), "timeout");
		Free();
	}

	private void RotateTowards(Vector2 targetPosition)
	{
		Vector2 direction = targetPosition - GlobalPosition;
		float angle = Mathf.Atan2(direction.Y, direction.X);
		Rotation = angle;
	}
}
