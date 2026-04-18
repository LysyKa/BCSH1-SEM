using Godot;
using System;
using System.Collections;
using System.Linq.Expressions;

public partial class MainCamera2d : Camera2D
{

	private bool dragging = false;
	private Vector2 dragOffset;
	private float minZoom = 0.4F;
	private float maxZoom = 2F;

	public override void _Ready()
	{
		PositionSmoothingEnabled = true;
		DragHorizontalEnabled = false;
		DragVerticalEnabled = false;
		//this.RotationSmoothingEnabled = false;
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Middle)
		{
			if (mb.Pressed)
			{
				dragging = true;
				dragOffset = GlobalPosition - GetGlobalMousePosition();
			}
			else
			{
				dragging = false;
			}
		} else if (@event is InputEventMouseButton wheel)
		{
			if (wheel.ButtonIndex == MouseButton.WheelDown && wheel.Pressed)
			{
				SetZoom(Zoom * 0.9f); // Zoom in
			}
			else if (wheel.ButtonIndex == MouseButton.WheelUp && wheel.Pressed)
			{
				SetZoom(Zoom * 1.1f); // Zoom out
			}
		}
	}

	private new void SetZoom(Vector2 newZoom)
	{
		newZoom.X = Mathf.Clamp(newZoom.X, minZoom, maxZoom);
		newZoom.Y = Mathf.Clamp(newZoom.Y, minZoom, maxZoom);
		Zoom = newZoom;
	}

	public override void _Process(double delta)
	{
		if (dragging)
		{
			GlobalPosition = GetGlobalMousePosition() + dragOffset;
		}
	}

}
