using Godot;
using System;

public partial class TitleScreen : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		GetNode<Camera2D>("Camera2D").Enabled = true;
		GetNode<Camera2D>("Camera2D").MakeCurrent();
		GetWindow().ContentScaleSize = new Vector2I(800, 600);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_button_new_game_pressed(Button button)
	{
		GetNode<PopupPanel>("PopupPanel").Popup();
	}
	public void _on_button_load_game_pressed(Button button)
	{
	}
	public void _on_button_upgrades_pressed(Button button)
	{
	}
	public void _on_button_settings_pressed(Button button)
	{
	}
	public void _on_button_exit_pressed(Button button)
	{
		GetWindow().GetTree().Quit();
	}

	public void _on_popup_button_pressed(int mapNumber, Vector2I size, Button button)
	{
		GetWindow().ContentScaleSize = size;
		// GetWindow().ContentScaleSize = new Vector2I(1600, 800);

		var mainScene = GetNode<Node2D>("/root/Main_Scene");
		foreach (var item in mainScene.GetChildren())
		{
			if (item is CanvasLayer canvasItem)
			{
				canvasItem.Visible = true;
			}
			else if (item is Node2D node)
			{
				node.Visible = true;
			}
		}
		GetNode<PopupPanel>("PopupPanel").Hide();
		this.Hide();
		GetNode<Camera2D>("/root/Main_Scene/IngameItems/MapLayer0/Camera2D").Enabled = true;
		// GetNode<Camera2D>("/root/Main_Scene/IngameItems/MapLayer"+(mapNumber-1).ToString()+"/Camera2D").Enabled = true;
		GetNode<Camera2D>("/root/Main_Scene/IngameItems/MapLayer0/Camera2D").MakeCurrent();
		// GetNode<Camera2D>("/root/Main_Scene/IngameItems/MapLayer"+(mapNumber-1).ToString()+"/Camera2D").MakeCurrent();
		this.QueueFree();
		// ((Node2D)GetParent()).Hide();
	}

}
