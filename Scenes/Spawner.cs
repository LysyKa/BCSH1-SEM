using Godot;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public partial class Spawner : Node2D
{

	private int totalWaves = 5;
	private int enemyCount = 10;
	private int waveCount = 5;
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
		enemyCount--;
	//	GD.Print("Spawned enemy, remaining count: " + enemyCount);
		if (enemyCount <= 0)
		{
			GetNode<Timer>("Timer").Stop();
			GD.Print("All enemies spawned, stopping timer.");
			enemyCount = 10;
			GetNode<Timer>("Timer").Start(5);
			GetNode<Timer>("Timer").WaitTime = 0.5;
			waveCount--;
			GetNode<Label>("/root/Main_Scene/UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelCurrWave").Text = "Current wave: " + (totalWaves - waveCount + 1);
			GD.Print("Wave " + (totalWaves - waveCount) + " completed. Remaining waves: " + waveCount);
			if (waveCount <= 0)
			{
				GetNode<Timer>("Timer").Stop();
				GD.Print("All waves completed, stopping timer.");
				waveCount = totalWaves;
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
