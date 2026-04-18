using Godot;
using System;

public partial class Projectile2d : CharacterBody2D
{
	public float Speed { get; set; } = 300.0f;
	public int Damage { get; set; } = 1;
	public Node2D CurrentTarget { get; set; }

	bool haveCollided = false;
	bool canDelete = false;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
	public override void _Process(double delta)
	{
		if (CurrentTarget is not null && IsInstanceValid(CurrentTarget))
		{
			if (!haveCollided)
			{
				Vector2 direction = (CurrentTarget.GlobalPosition - GlobalPosition).Normalized();
				Velocity = direction * Speed;
				RotateTowards(CurrentTarget.GlobalPosition);
				bool collision = MoveAndSlide();

				if (GlobalPosition.DistanceTo(CurrentTarget.GlobalPosition) < 50)
				// if (collision)
				{
					haveCollided = true;
					if (CurrentTarget is Enemy enemy)
					{
						int remainingLife = enemy.GetDamaged(Damage);
						GD.Print("Projectile hit target: " + CurrentTarget.Name + " - Remaining Life " + remainingLife);
					}
					this.GetParent().RemoveChild(this);
					QueueFree(); // Destroy the projectile after hitting the target
				}
			}

		} else
		{
			this.GetParent().RemoveChild(this);
			QueueFree(); // Destroy the projectile if there is no target
		}
	}

	private void RotateTowards(Vector2 targetPosition)
	{
		Vector2 direction = targetPosition - GlobalPosition;
		float angle = Mathf.Atan2(direction.Y, direction.X);
		Rotation = angle;
	}
}
