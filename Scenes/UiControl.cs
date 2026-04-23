using Godot;
using System;

public partial class UiControl : Control
{
	ButtonGroup buttonGroup;
	bool simulateTower = false;
	Tower2d currTower;
	bool lastState = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		buttonGroup = new ButtonGroup();
		foreach (var container in GetNode("PanelTowerBuild/HBoxContainer/").GetChildren())
		{
			foreach (var button in container.GetNode("VBoxContainer4").GetChildren())
			{
				if (button is Button btn)
				{
					btn.ButtonGroup = buttonGroup;
				}
			}

		}
		buttonGroup.Pressed += _on_button_tower_pressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (simulateTower)
		{
			currTower.GlobalPosition = GetParent().GetParent().GetNode<Camera2D>("IngameItems/MapLayer0/Camera2D").GetGlobalMousePosition();
		}
	}

	public void _on_button_build_pressed()
	{
		var pabel = GetNode<Panel>("PanelTowerBuild");
		pabel.Visible = !pabel.Visible;
	}

	public void _on_button_tower_pressed(BaseButton button)
	{

		if (button.ButtonPressed && lastState)
		{
			button.ButtonPressed = false;
			simulateTower = false;
			lastState = false;
			if (currTower is not null)
			{
				GetNode<Node2D>("/root/Main_Scene/IngameItems/TowerContainer").RemoveChild(currTower);
			}
			currTower = null;
		}
		else
		{
			switch (button.Name)
			{
				case "ButtonBasicTower":
					_on_button_basic_tower_pressed((Button)button);
					break;
				case "ButtonDoubleTower":
					_on_button_double_tower_pressed((Button)button);
					break;
				case "ButtonFastTower":
					_on_button_fast_tower_pressed((Button)button);
					break;
				case "ButtonRapidTower":
					_on_button_rapid_tower_pressed((Button)button);
					break;
				case "ButtonSplashTower":
					_on_button_splash_tower_pressed((Button)button);
					break;
				default:
					GD.Print("Mohl by být error, investigate!");
					break;
			}
		}
	}


	public void _on_button_basic_tower_pressed(Button button)
	{

			var tower = (Tower2d)GD.Load<PackedScene>("res://Towers/Tower2D.tscn").Instantiate();

			tower.attackSpeed = 1;
			tower.bulletDamage = 5;
			tower.targetCount = 1;
			tower.range = 400;
			tower.cost = 5;

			lastState = true;
			simulateTower = true;
			tower.isFake = true;
			tower.spritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile249.png";
			currTower = tower;


			GetNode<Node2D>("/root/Main_Scene/IngameItems/TowerContainer").AddChild(currTower);
	}

	public void _on_button_double_tower_pressed(Button button)
	{
			var tower = (Tower2d)GD.Load<PackedScene>("res://Towers/Tower2D.tscn").Instantiate();
			tower.attackSpeed = 1;
			tower.bulletDamage = 5;
			tower.targetCount = 2;
			tower.range = 400;
			tower.cost = 20;

			lastState = true;
			simulateTower = true;
			tower.isFake = true;
			tower.spritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile250.png";
			currTower = tower;

			GetNode<Node2D>("/root/Main_Scene/IngameItems/TowerContainer").AddChild(currTower);
	}

	public void _on_button_fast_tower_pressed(Button button)
	{
			var tower = (Tower2d)GD.Load<PackedScene>("res://Towers/Tower2D.tscn").Instantiate();
			tower.attackSpeed = 5;
			tower.bulletDamage = 2;
			tower.targetCount = 1;
			tower.range = 400;
			tower.cost = 30;

			lastState = true;
			simulateTower = true;
			tower.isFake = true;
			tower.spritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile291.png";

			currTower = tower;

			GetNode<Node2D>("/root/Main_Scene/IngameItems/TowerContainer").AddChild(currTower);
	}

	public void _on_button_rapid_tower_pressed(Button button)
	{
			var tower = (Tower2d)GD.Load<PackedScene>("res://Towers/Tower2D.tscn").Instantiate();
			tower.attackSpeed = 10;
			tower.bulletDamage = 2;
			tower.targetCount = 1;
			tower.range = 400;
			tower.cost = 50;

			lastState = true;
			simulateTower = true;
			tower.isFake = true;
			tower.spritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile292.png";

			currTower = tower;

			GetNode<Node2D>("/root/Main_Scene/IngameItems/TowerContainer").AddChild(currTower);
	}

	public void _on_button_splash_tower_pressed(Button button)
	{
			var tower = (Tower2d)GD.Load<PackedScene>("res://Towers/Tower2D.tscn").Instantiate();
			tower.attackSpeed = 1;
			tower.bulletDamage = 1;
			tower.targetCount = 1;
			tower.range = 400;
			tower.cost = 100;

			lastState = true;
			simulateTower = true;
			tower.isFake = true;
			tower.spritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile226.png";

			currTower = tower;

			GetNode<Node2D>("/root/Main_Scene/IngameItems/TowerContainer").AddChild(currTower);
	}

	public override void _Input(InputEvent e)
	{
		if (e is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.Pressed)
			{

				if (mouseEvent.ButtonIndex == MouseButton.Left && currTower != null)
				{
					if (currTower.cost <= GetNode<PlayerStats>("/root/Main_Scene/PlayerStats").playerGold)
					{
						currTower.isFake = false;
						currTower.Position = GetParent().GetParent().GetNode<MapLayer0>("IngameItems/MapLayer0").ChangeVectorToLocal(currTower.GlobalPosition);
						GetNode<PlayerStats>("/root/Main_Scene/PlayerStats").playerGold -= currTower.cost;
						GetNode<PlayerStats>("/root/Main_Scene/PlayerStats").updateGold();
						currTower = null;
						buttonGroup.GetPressedButton().ButtonPressed = false;
						lastState = false;
						simulateTower = false;
					}
					else
					{
						GD.Print("NOT ENOUGH GOLD TO BUILD THIS TOWER");
					}

				}
				else if (mouseEvent.ButtonIndex == MouseButton.Right && currTower != null)
				{
					GetParent().GetParent().GetNode<Node2D>("./IngameItems/TowerContainer").RemoveChild(currTower);
					currTower = null;
					buttonGroup.GetPressedButton().ButtonPressed = false;
					lastState = false;
					simulateTower = false;
				}
			}
		}
	}
}
