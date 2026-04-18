using Godot;
using System;

public partial class ClickableArea2d : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left)
			{
				GD.Print("Tower clicked!");
				draw = true;
				/*var areaTest = this.GetParent().GetNode<Area2D>("TowerArea2D");
				var collisionShape = areaTest.GetNode<CollisionShape2D>("TowerRangeCollisionShape2D");
				// var collisionShape = areaTest.GetNode<CollisionShape2d>("TowerArea2D/TowerRangeCollisionShape2D");
				
				CircleShape2D test = (CircleShape2D)collisionShape.Shape;
				GD.Print("Collision shape radius: " + test.Radius);*/

				Radius = (GetParent().GetNode<CollisionShape2D>("TowerArea2D/TowerRangeCollisionShape2D").Shape as CircleShape2D).Radius;
				// Radius = GetParent().GetNode<Tower2d>("Tower2D").range;
				GD.Print("Drawing circle with radius: " + Radius);

				GetNode<Panel>("/root/Main_Scene/UICanvasLayer/UIControl/PanelTowerStats").Visible = true;


				GetNode<Label>("/root/Main_Scene/UICanvasLayer/UIControl/PanelTowerStats/MarginContainer/VBoxContainer/HBoxContainer3/LabelDamage").Text = ((Tower2d)GetParent()).bulletDamage.ToString();
				GetNode<Label>("/root/Main_Scene/UICanvasLayer/UIControl/PanelTowerStats/MarginContainer/VBoxContainer/HBoxContainer4/LabelAttackSpeed").Text = ((Tower2d)GetParent()).attackSpeed.ToString();
				GetNode<Label>("/root/Main_Scene/UICanvasLayer/UIControl/PanelTowerStats/MarginContainer/VBoxContainer/HBoxContainer5/LabelTargetCount").Text = ((Tower2d)GetParent()).targetCount.ToString();
				GetNode<Label>("/root/Main_Scene/UICanvasLayer/UIControl/PanelTowerStats/MarginContainer/VBoxContainer/HBoxContainer6/LabelRange").Text = ((Tower2d)GetParent()).range.ToString();

				QueueRedraw();
			}
			else if (mouseEvent.ButtonIndex == MouseButton.Right)
			{
				draw = false;
				GetNode<Panel>("/root/Main_Scene/UICanvasLayer/UIControl/PanelTowerStats").Visible = false;

				QueueRedraw();
			}
		}
	}

	public override void _Draw()
	{
		if (draw)
		{
			DrawCircle(Vector2.Zero, Radius, CircleColor);
		}
		else
		{
			GetChildren().Clear();
		}
	}
	public bool draw = false;
	public float Radius = 0;
	public Color CircleColor = new Color(0.2F, 0.2F, 1F, 0.2F);

}
