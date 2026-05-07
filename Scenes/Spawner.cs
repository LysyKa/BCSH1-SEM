using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using static Wave;

public partial class Spawner : Node2D
{

	[Export]
	public int totalWaves = 5;
	[Export]
	public int enemyCount = 10;
	[Export]
	public double timerWaitTime = 0.5;
	public int currWaveCount = 5;
	public int currEnemyCount = 10;
	public Array<String> paths = new();
	public List<enemyStats> enemies = new();
	public Array<int> enemyCounts = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		paths.Resize(3);
		enemyCounts.Resize(3);
		GetNode<Label>("/root/Main_Scene/UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelCurrWave").Text = "Current wave: " + currWaveCount + "/" + totalWaves;
		var notifier = new VisibleOnScreenNotifier2D();
		AddChild(notifier);//GetNode<VisibilityNotifier2D>("VisibilityNotifier2D");
		notifier.ScreenEntered += notifierIn;
		notifier.ScreenExited += notifierOut;
	}

	public void notifierIn()
	{
		GetNode<Button>("/root/Main_Scene/UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/ButtonSpawner").Pressed += _on_button_pressed;
		askWaveData();

	}
	public void notifierOut()
	{
		GetNode<Button>("/root/Main_Scene/UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/ButtonSpawner").Pressed -= _on_button_pressed;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void setStats(enemyStats stats, Enemy enemy)
	{
		enemy.Life = stats.Life;
		enemy.Damage = stats.Damage;
		enemy.Bounty = stats.Bounty;
		enemy.Speed = stats.Speed;
		enemy.spritePath = stats.path;
	}

	void _on_timer_timeout()
	{
		Random random = new Random();
		int next = random.Next((enemyCounts[0] + enemyCounts[1] + enemyCounts[2]));
		PackedScene enemyScene = ResourceLoader.Load<PackedScene>("res://Enemies/Enemy.tscn");
		Enemy enemyInstance = enemyScene.Instantiate<Enemy>();
		if (next < enemyCounts[0])
		{
			enemyCounts[0]--;
			setStats(enemies.ElementAt(0), enemyInstance);


		}
		else if (next < (enemyCounts[0] + enemyCounts[1]))
		{
			enemyCounts[1]--;
			setStats(enemies.ElementAt(1), enemyInstance);


		}
		else
		{
			enemyCounts[2]--;
			setStats(enemies.ElementAt(2), enemyInstance);


		}
		next = random.Next(3);
		Path2D path = GetNode<Path2D>(paths[next]);
		GD.Print(paths[next].ToString());
		GD.Print(path.ToString());
		GD.Print(enemyInstance.ToString());
		var enemyPathFollow = new PathFollow2D();
		// var enemyScene = ResourceLoader.Load<PackedScene>("res://Enemies/Enemy.tscn");
		// var enemyInstance = enemyScene.Instantiate<Enemy>();
		path.AddChild(enemyPathFollow);
		/*for (int i = 0; i < totalWaves - currWaveCount; i++)
		{
			randomizeStats(enemyInstance);
		}*/
		enemyPathFollow.AddChild(enemyInstance);
		currEnemyCount--;
		//	GD.Print("Spawned enemy, remaining count: " + enemyCount);
		if (currEnemyCount <= 0)
		{
			SendAnotherWave();
		}
	}

	private void askWaveData()
	{
		((WaveData)GetParent().GetNode<Node2D>("WaveData")).GetAnotherWave();
		currEnemyCount = enemyCounts[0] + enemyCounts[1] + enemyCounts[2];

	}

	public void SendAnotherWave()
	{
		askWaveData();
		GetNode<Timer>("Timer").Stop();
		// currEnemyCount = enemyCount;
		GetNode<Timer>("Timer").Start(5);
		GetNode<Timer>("Timer").WaitTime = timerWaitTime;
		currWaveCount++;
		GetNode<Label>("/root/Main_Scene/UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelCurrWave").Text = "Current wave: " + currWaveCount + "/" + totalWaves;
		GD.Print("Wave " + currWaveCount + " completed. Remaining waves: " + (totalWaves - currWaveCount));
		if (currWaveCount >= totalWaves)
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
