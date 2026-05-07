using Godot;
using System;

public partial class WaveData : Node2D
{
	private Spawner spawner;
	private int currentWave = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawner = GetParent().GetNode<Spawner>("Spawner");
		spawner.totalWaves = GetChildCount();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void GetAnotherWave()
	{
		GD.Print("Cur wave:" + currentWave);
		var child = GetChild<Wave>(currentWave);
		GD.Print("Enems: " + child.enemies.ToString());
		spawner.enemies = child.enemies;	
		spawner.enemyCounts = child.enemyCounts;
		GD.Print(child.enemyCounts.ToString());

		spawner.paths = child.paths;
		GD.Print(child.paths.ToString());

		spawner.currWaveCount = currentWave;
		currentWave++;
	}

	public Enemy makeNewEnemy()
	{
		return null;
	}


}
