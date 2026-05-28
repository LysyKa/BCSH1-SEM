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
		// GD.Print(this.GetChildCount());
		// GD.Print("Cur wave:" + (currentWave));
		if (currentWave >= GetChildCount())
		{
			return;
		}
		var child = GetChild<Wave>(currentWave);
		spawner.enemies = child.enemies;	
		spawner.enemyCounts = child.enemyCounts;
		spawner.paths = child.paths;
		spawner.currWaveCount = currentWave;
		currentWave++;
	}

	public Enemy makeNewEnemy()
	{
		return null;
	}


}
