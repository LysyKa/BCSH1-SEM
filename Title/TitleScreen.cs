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
		GetNode<PopupPanel>("UpgradesPopupPanel").Popup();
	}
	public void _on_button_settings_pressed(Button button)
	{
		GetNode<PopupPanel>("SettingsPopupPanel").Popup();
	}
	public void _on_button_exit_pressed(Button button)
	{
		GetWindow().GetTree().Quit();
	}
	public void _on_button_upgrade0_pressed(TextureButton button)
	{
		GetNode<PlayerStats>("/root/Main_Scene/PlayerStats").updateUpgrades(0, true, 500);
	}
	public void _on_button_upgrade1_pressed(TextureButton button)
	{
		GetNode<PlayerStats>("/root/Main_Scene/PlayerStats").updateUpgrades(1, true, 2500);
	}
	public void _on_button_upgrade2_pressed(TextureButton button)
	{
		GetNode<PlayerStats>("/root/Main_Scene/PlayerStats").updateUpgrades(2, true, 10000);
	}
	public void _on_button_close_pressed(Button button)
	{
		button.GetParent().GetParent<PopupPanel>().Visible = false;
	}
	public void _on_checkbox_sound_pressed(Button button)
	{
		AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), !AudioServer.IsBusMute(AudioServer.GetBusIndex("Master")));
		GD.Print($"Audiobus Mute is now {AudioServer.IsBusMute(AudioServer.GetBusIndex("Master"))}");
	}
	
	public void _on_popup_button_pressed(Node button, int mapNumber, Vector2I size)
	{
		GetWindow().ContentScaleSize = size;

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
		Hide();
		mapNumber--;
		String pathToMap = "/root/Main_Scene/IngameItems/MapLayer" + mapNumber;
		GetNode<Camera2D>(pathToMap + "/Camera2D").Enabled = true;
		GetNode<Camera2D>(pathToMap + "/Camera2D").MakeCurrent();
		GetNode<TileMapLayer>(pathToMap).Visible = true;

		QueueFree();
	}

}
