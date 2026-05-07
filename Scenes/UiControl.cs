using Godot;
using System;

public partial class UiControl : Control
{
	public ButtonGroup buttonGroupTowers;
	public ButtonGroup buttonGroupSpeed;

	public bool simulateTower = false;
	public Tower2d currTower;
	public bool lastState = false;
	public bool destructMode = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Pass;
		buttonGroupSpeed = new ButtonGroup();
		foreach (Button button in GetNode("PanelPlayerStats/HBoxContainer/HBoxContainer/").GetChildren())
		{
			if (button is Button btn)
			{
				btn.ButtonGroup = buttonGroupSpeed;
			}
		}
		buttonGroupSpeed.Pressed += _on_button_speedup_pressed;

		buttonGroupTowers = new ButtonGroup();
		foreach (var container in GetNode("PanelTowerBuild/HBoxContainer/").GetChildren())
		{
			foreach (var button in container.GetNode("VBoxContainer4").GetChildren())
			{
				if (button is Button btn)
				{
					btn.ButtonGroup = buttonGroupTowers;
				}
			}

		}
		buttonGroupTowers.Pressed += _on_button_tower_pressed;
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
	public void _on_button_destruct_pressed()
	{
		destructMode = !destructMode;

		if (destructMode)
		{
			var tex = GD.Load<Texture2D>("res://ZPics/kenney_tower-defense-top-down/PNG/new/bomb.png");
			// Input.SetCustomMouseCursor(tex);
		}
		else
		{
			Input.SetCustomMouseCursor(null);
		}
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
			GetNode<Panel>("/root/Main_Scene/UICanvasLayer/UIControl/PanelTowerStats").Visible = false;
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
			lastState = true;
			simulateTower = true;
			String pathTemp = "/root/Main_Scene/UICanvasLayer/UIControl/PanelTowerStats/MarginContainer/VBoxContainer/";
			GetNode<Panel>("/root/Main_Scene/UICanvasLayer/UIControl/PanelTowerStats").Visible = true;
			GetNode<Label>(pathTemp + "HBoxContainer3/LabelDamage").Text = currTower.bulletDamage.ToString();
			GetNode<Label>(pathTemp + "HBoxContainer4/LabelAttackSpeed").Text = currTower.attackSpeed.ToString();
			GetNode<Label>(pathTemp + "HBoxContainer5/LabelTargetCount").Text = currTower.targetCount.ToString();
			GetNode<Label>(pathTemp + "HBoxContainer6/LabelRange").Text = currTower.range.ToString();
		}
	}

	public Tower2d createTower(
	double attackSpeed,
	 int bulletDamage,
	  int targetCount,
	   int range,
		int cost,
		 double fireAngle,
		  string spritePath,
		   string projectileSpritePath,
		 	float projectileSpeed = 500F)
	{
		Tower2d tower = (Tower2d)GD.Load<PackedScene>("res://Towers/Tower2D.tscn").Instantiate();
		tower.attackSpeed = attackSpeed;
		tower.bulletDamage = bulletDamage;
		tower.targetCount = targetCount;
		tower.range = range;
		tower.cost = cost;
		tower.fireAngleThreshold = fireAngle;
		tower.spritePath = spritePath;
		tower.projectilePath = projectileSpritePath;
		tower.isFake = true;
		tower.projectileSpeed = projectileSpeed;

		return tower;
	}

	public void _on_button_basic_tower_pressed(Button button)
	{
		String path = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile249.png";
		String projectileSpritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile251.png";
		currTower = createTower(
		attackSpeed: 1,
		 bulletDamage: 5,
		  targetCount: 1,
		   range: 400,
			cost: 5,
			 fireAngle: 0.2D,
			  spritePath: path,
			   projectileSpritePath: projectileSpritePath);

		GetNode<Node2D>("/root/Main_Scene/IngameItems/TowerContainer").AddChild(currTower);
	}

	public void _on_button_double_tower_pressed(Button button)
	{
		String path = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile250.png";
		String projectileSpritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile252.png";

		currTower = createTower(attackSpeed: 1, bulletDamage: 5, targetCount: 2, range: 450, cost: 20, fireAngle: 0.5D, spritePath: path, projectileSpritePath: projectileSpritePath);


		GetNode<Node2D>("/root/Main_Scene/IngameItems/TowerContainer").AddChild(currTower);
	}

	public void _on_button_fast_tower_pressed(Button button)
	{
		String path = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile291.png";
		String projectileSpritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile272.png";

		currTower = createTower(attackSpeed: 5, bulletDamage: 2, targetCount: 1, range: 250, cost: 30, fireAngle: 0.05D, spritePath: path, projectileSpritePath: projectileSpritePath);

		GetNode<Node2D>("/root/Main_Scene/IngameItems/TowerContainer").AddChild(currTower);
	}

	public void _on_button_rapid_tower_pressed(Button button)
	{
		String path = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile292.png";
		String projectileSpritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile273.png";

		currTower = createTower(attackSpeed: 10, bulletDamage: 2, targetCount: 2, range: 200, cost: 50, fireAngle: 0.05D, spritePath: path, projectileSpritePath: projectileSpritePath);


		GetNode<Node2D>("/root/Main_Scene/IngameItems/TowerContainer").AddChild(currTower);
	}

	public void _on_button_splash_tower_pressed(Button button)
	{
		String path = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile226.png";
		String projectileSpritePath = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile296.png";

		currTower = createTower(attackSpeed: 1, bulletDamage: 1, targetCount: 1, range: 400, cost: 100, fireAngle: 0.1D, spritePath: path, projectileSpritePath: projectileSpritePath, projectileSpeed: 200F);


		GetNode<Node2D>("/root/Main_Scene/IngameItems/TowerContainer").AddChild(currTower);
	}

	public override void _GuiInput(InputEvent e)
	{
		if (e is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.Pressed)
			{
				// Debugging this for hours before giving up and then noticing that return was IN the for cycle instead of outside week later
				/*	if (destructMode && currTower is null && (mouseEvent.ButtonIndex == MouseButton.Left || mouseEvent.ButtonIndex == MouseButton.Right))
					{
						var space = GetWorld2D().DirectSpaceState;
						var query = new PhysicsPointQueryParameters2D
						{
							Position = GetViewport().GetCamera2D().GetGlobalMousePosition(),
							CollideWithAreas = true,
							CollisionMask = uint.MaxValue
						};

						var result = space.IntersectPoint(query);
						foreach (var hit in result)
						{
							if (hit["collider"].As<Node2D>() is Area2D clickableArea && clickableArea.IsInGroup("clickable"))
							{
								GD.Print(clickableArea.Name);
								GetParent().GetParent().GetNode<Node2D>("./IngameItems/TowerContainer").RemoveChild(clickableArea.GetParent());
							}
							//return;
						}
						return;
					}*/
				if (mouseEvent.ButtonIndex == MouseButton.Left && currTower != null
				&& !GetNode<Panel>("PanelTowerBuild").GetGlobalRect().HasPoint(GetGlobalMousePosition())
				&& !GetNode<Panel>("PanelPlayerStats").GetGlobalRect().HasPoint(GetGlobalMousePosition())
				&& !GetNode<Panel>("PanelTowerStats").GetGlobalRect().HasPoint(GetGlobalMousePosition()))
				{
					if (currTower.cost <= GetNode<PlayerStats>("/root/Main_Scene/PlayerStats").playerGold)
					{
						currTower.isFake = false;
						currTower.Position = GetViewport().GetCamera2D().GetGlobalMousePosition();


						//currTower.Position = GetParent().GetParent().GetNode<MapLayer0>("IngameItems/MapLayer0").ChangeVectorToLocal(currTower.GlobalPosition);
						GetNode<PlayerStats>("/root/Main_Scene/PlayerStats").playerGold -= currTower.cost;
						GetNode<PlayerStats>("/root/Main_Scene/PlayerStats").updateGold();
						currTower = null;
						buttonGroupTowers.GetPressedButton().ButtonPressed = false;
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
					buttonGroupTowers.GetPressedButton().ButtonPressed = false;
					lastState = false;
					simulateTower = false;
					GetNode<Panel>("/root/Main_Scene/UICanvasLayer/UIControl/PanelTowerStats").Visible = false;
				}
				/*else
				{
					//GD.Print("Test");
				}*/
			}
		}
	}
	public void _on_button_speedup_pressed(BaseButton button)
	{

		GD.Print("It works");
		if (button.Name.Equals("ButtonPause"))
		{
			Engine.TimeScale = 0f;
			GD.Print("Paused");
		}
		else if (button.Name.Equals("ButtonRun"))
		{
			Engine.TimeScale = 1f;
			GD.Print("Running at normal speed");
		}
		else
		{
			Engine.TimeScale = 2f;
			GD.Print("Running at double speed");

		}
	}
}
