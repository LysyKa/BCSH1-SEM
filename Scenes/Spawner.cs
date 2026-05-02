using Godot;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public partial class Spawner : Node2D
{

	[Export]
	private int totalWaves = 5;
	[Export]
	private int enemyCount = 10;
	[Export]
	private double timerWaitTime = 0.5;
	private int currWaveCount = 5;
	private int currEnemyCount = 10;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Label>("/root/Main_Scene/UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelCurrWave").Text = "Current wave: " + (totalWaves - currWaveCount) + "/" + totalWaves;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void _on_timer_timeout()
	{
		//GetNode<Timer>("Timer").WaitTime = 0.5;
		//var path = ResourceLoader.Load<PackedScene>("res://Scenes/pathSceneTest.tscn");
		var path = GetNode<Path2D>("Path2D");
		var enemyPathFollow = new PathFollow2D();
		var enemyScene = ResourceLoader.Load<PackedScene>("res://Enemies/Enemy.tscn");
		var enemyInstance = enemyScene.Instantiate<Enemy>();
		path.AddChild(enemyPathFollow);
		for (int i = 0; i < totalWaves - currWaveCount; i++)
		{
			randomizeStats(enemyInstance);
		}
		enemyPathFollow.AddChild(enemyInstance);
		currEnemyCount--;
		//	GD.Print("Spawned enemy, remaining count: " + enemyCount);
		if (currEnemyCount <= 0)
		{
			SendAnotherWave();
		}
	}
	public void SendAnotherWave()
	{
		GetNode<Timer>("Timer").Stop();
		currEnemyCount = enemyCount;
		GetNode<Timer>("Timer").Start(5);
		GetNode<Timer>("Timer").WaitTime = timerWaitTime;
		currWaveCount--;
		GetNode<Label>("/root/Main_Scene/UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelCurrWave").Text = "Current wave: " + (totalWaves - currWaveCount) + "/" + totalWaves;
		GD.Print("Wave " + (totalWaves - currWaveCount) + " completed. Remaining waves: " + currWaveCount);
		if (currWaveCount <= 0)
		{
			WavesCompleted();
		}
	}
	public void WavesCompleted()
	{
		GetNode<Timer>("Timer").Stop();
		GD.Print("All waves completed, stopping timer.");
		currWaveCount = totalWaves;
		_on_finished();
	}
	void _on_button_pressed()
	{
		if (GetNode<Timer>("Timer").IsStopped())
		{
			GetNode<Timer>("Timer").Start();
		}
		else
		{
			GetNode<Timer>("Timer").SetPaused(!GetNode<Timer>("Timer").IsPaused());
		}
	}

	void _on_finished()
	{
		GD.Print("Congratulations! You won");
	}

	public CharacterBody2D randomizeStats(Enemy enemy)
	{
		Random random = new Random();
		enemy.Damage = random.Next(enemy.Damage, 3 * enemy.Damage);
		enemy.Life = random.Next(enemy.Life, 3 * enemy.Life);
		enemy.Speed = random.Next(enemy.Speed, (int)(1.5f * enemy.Speed));
		enemy.Bounty = random.Next(enemy.Bounty, 5 * enemy.Bounty);
		return null;
	}


}
