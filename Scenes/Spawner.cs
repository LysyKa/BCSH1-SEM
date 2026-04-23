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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void _on_timer_timeout()
	{
		//GetNode<Timer>("Timer").WaitTime = 0.5;
		var path = ResourceLoader.Load<PackedScene>("res://Scenes/pathSceneTest.tscn");
		var enemyPath = path.Instantiate<Path2D>();
		AddChild(enemyPath);
		currEnemyCount--;
	//	GD.Print("Spawned enemy, remaining count: " + enemyCount);
		if (currEnemyCount <= 0)
		{
			GetNode<Timer>("Timer").Stop();
			GD.Print("All enemies spawned, stopping timer.");
			currEnemyCount = enemyCount;
			GetNode<Timer>("Timer").Start(5);
			GetNode<Timer>("Timer").WaitTime = timerWaitTime;
			currWaveCount--;
			GetNode<Label>("/root/Main_Scene/UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelCurrWave").Text = "Current wave: " + (totalWaves - currWaveCount) + "/" + totalWaves;
			GD.Print("Wave " + (totalWaves - currWaveCount) + " completed. Remaining waves: " + currWaveCount);
			if (currWaveCount <= 0)
			{
				GetNode<Timer>("Timer").Stop();
				GD.Print("All waves completed, stopping timer.");
				currWaveCount = totalWaves;
				_on_finished();
			}
		}
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
		// GetChildren().Clear();
	}

}
