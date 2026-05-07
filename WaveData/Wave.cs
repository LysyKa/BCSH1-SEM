using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Wave : Node2D
{
	[Export]
	public int Damage1 { get; set; } = 1;
	[Export]
	public int Speed1 { get; set; } = 100;
	[Export]
	public int Bounty1 { get; set; } = 1;
	[Export]
	public int Life1 { get; set; } = 10;
	[Export]
	public int Damage2 { get; set; } = 1;
	[Export]
	public int Speed2 { get; set; } = 100;
	[Export]
	public int Bounty2 { get; set; } = 1;
	[Export]
	public int Life2 { get; set; } = 10;
	[Export]
	public int Damage3 { get; set; } = 1;
	[Export]
	public int Speed3 { get; set; } = 100;
	[Export]
	public int Bounty3 { get; set; } = 1;
	[Export]
	public int Life3 { get; set; } = 10;
	[Export]
	public int enemy1Count = 10;
	[Export]
	public int enemy2Count = 0;
	[Export]
	public int enemy3Count = 0;
	[Export]
	public bool enemy1Present = true;
	[Export]
	public bool enemy2Present = false;
	[Export]
	public bool enemy3Present = false;
	[Export]
	public String path1 = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile245.png";
	[Export]
	public String path2 = "";
	[Export]
	public String path3 = "";
	[Export]
	public String pathName1 = "Path2D";
	[Export]
	public String pathName2 = "Path2D";
	[Export]
	public String pathName3 = "Path2D";

	public Array<String> paths = new();
	public List<enemyStats> enemies = new();
	public Array<int> enemyCounts = new();

	public class enemyStats
	{
		public int Damage { get; set; } = 1;
		public int Speed { get; set; } = 100;
		public int Bounty { get; set; } = 1;
		public int Life { get; set; } = 10;
		public String path = "res://ZPics/kenney_tower-defense-top-down/PNG/Default size/towerDefense_tile245.png";
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		paths.Resize(3);
		enemyCounts.Resize(3);
		var enemyIn1 = new enemyStats();
		var enemyIn2 = new enemyStats();
		var enemyIn3 = new enemyStats();
		enemyIn2.Damage = Damage2;
		enemyIn3.Damage = Damage3;
		enemyIn1.Bounty = Bounty1;
		enemyIn2.Bounty = Bounty2;
		enemyIn3.Bounty = Bounty3;
		enemyIn1.Speed = Speed1;
		enemyIn2.Speed = Speed2;
		enemyIn3.Speed = Speed3;
		enemyIn1.Life = Life1;
		enemyIn2.Life = Life2;
		enemyIn3.Life = Life3;
		enemyIn1.path = path1;
		enemyIn2.path = path2;
		enemyIn3.path = path3;

		enemies.Add(enemyIn1);
		enemies.Add(enemyIn2);
		enemies.Add(enemyIn3);
		paths[0] = pathName1;
		paths[1] = pathName2;
		paths[2] = pathName3;
		enemyCounts[0] = enemy1Count;
		enemyCounts[1] = enemy2Count;
		enemyCounts[2] = enemy3Count;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
