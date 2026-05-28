using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using static Wave;

public partial class Spawner : Node2D
{

	[Export]
	public int totalWaves = 5;
	[Export]
	public int enemyCount = 10;
	[Export]
	public double timerWaitTime = 0.5;
	public int currWaveCount = 0;
	public int currEnemyCount = 10;
	public Array<String> paths = new();
	public List<enemyStats> enemies = new();
	public Array<int> enemyCounts = new();
	private bool finished = false;
	private bool started = false;
	private Random rng = new Random();
	private bool spawnLocked = false;
	private Timer timer = new Timer();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer.Autostart = false;
		timer.WaitTime = 0.5D;
		timer.Timeout += _on_timer_timeout;
		this.AddChild(timer);
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
	}
	public void notifierOut()
	{
		GetNode<Button>("/root/Main_Scene/UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/ButtonSpawner").Pressed -= _on_button_pressed;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (finished && this.GetNode<Path2D>("Path2D").GetChildCount() == 0)
		{
			_on_finished();
			finished = false;
		}
	}

	public async void ShowWaveBanner(string text)
	{
		var panel = new Panel();
		panel.Size = new Vector2(400, 100);
		panel.Modulate = new Color(1, 1, 1, 0);
		panel.CustomMinimumSize = new Vector2(400, 100);

		panel.AddThemeStyleboxOverride("panel", new StyleBoxFlat()
		{
			BgColor = new Color(0, 0, 0, 0.7f),
			CornerRadiusTopLeft = 8,
			CornerRadiusTopRight = 8,
			CornerRadiusBottomLeft = 8,
			CornerRadiusBottomRight = 8
		});

		var label = new Label();

		label.Text = text;
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.VerticalAlignment = VerticalAlignment.Center;
		label.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
		label.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
		label.AnchorLeft = 0;
		label.AnchorTop = 0;
		label.AnchorRight = 1;
		label.AnchorBottom = 1;
		label.OffsetLeft = 0;
		label.OffsetTop = 0;
		label.OffsetRight = 0;
		label.OffsetBottom = 0;
		label.AddThemeFontSizeOverride("font_size", 32);
		panel.AddChild(label);

		GetNode("/root/Main_Scene/UICanvasLayer/UIControl/PanelPlayerStats").AddChild(panel);
		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;

		panel.Position = new Vector2(
			(viewportSize.X - panel.Size.X) / 2f,
			-panel.Size.Y - 20
		);

		Vector2 targetPos = new Vector2((viewportSize.X - panel.Size.X) / 2f, 20);

		var tween = CreateTween();
		tween.TweenProperty(panel, "position", targetPos, 0.4);
		tween.TweenProperty(panel, "modulate:a", 1.0f, 0.4);

		await ToSignal(tween, "finished");
		await ToSignal(GetTree().CreateTimer(3.5), "timeout");
		var tween2 = CreateTween();
		tween2.TweenProperty(panel, "modulate:a", 0.0f, 0.5);
		await ToSignal(tween2, "finished");

		panel.QueueFree();
	}




	public void setStats(enemyStats stats, Enemy enemy)
	{
		enemy.Life = stats.Life;
		enemy.Damage = stats.Damage;
		enemy.Bounty = stats.Bounty;
		enemy.Speed = stats.Speed;
		enemy.spritePath = stats.path;
	}
	private void UnlockSpawn()
	{
		spawnLocked = false;
	}
	void _on_timer_timeout()
	{
		int next = rng.Next((enemyCounts[0] + enemyCounts[1] + enemyCounts[2]));
		PackedScene enemyScene = ResourceLoader.Load<PackedScene>("res://Enemies/Enemy.tscn");
		Enemy enemyInstance = enemyScene.Instantiate<Enemy>();
		//GD.Print(enemyCounts[0] + enemyCounts[1] + enemyCounts[2]);
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
		next = rng.Next(3);
		Path2D path = GetNode<Path2D>(paths[next]);
		var enemyPathFollow = new PathFollow2D();
		path.AddChild(enemyPathFollow);
		/*for (int i = 0; i < totalWaves - currWaveCount; i++)
		{
			randomizeStats(enemyInstance);
		}*/
		enemyPathFollow.AddChild(enemyInstance);
		currEnemyCount--;
		//	GD.Print("Spawned enemy, remaining count: " + enemyCount);
		if (currEnemyCount <= 0 && !finished)
		{
			SendAnotherWave();
		}
	}

	private void askWaveData()
	{
		((WaveData)GetParent().GetNode<Node2D>("WaveData")).GetAnotherWave();
		ShowWaveBanner(("Wave " + (currWaveCount + 1) + " incoming!"));
		currEnemyCount = enemyCounts[0] + enemyCounts[1] + enemyCounts[2];

	}

	public void SendAnotherWave()
	{
		timer.Stop();

		// currEnemyCount = enemyCount;
		timer.Start(5);
		timer.WaitTime = 0.5D;
		currWaveCount++;
		GetNode<Label>("/root/Main_Scene/UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelCurrWave").Text = "Current wave: " + (currWaveCount + 1) + "/" + totalWaves;
		GD.Print("Wave " + (currWaveCount) + " completed. Remaining waves: " + (totalWaves - currWaveCount));
		if ((currWaveCount) >= totalWaves)
		{
			WavesCompleted();
			finished = true;
			return;
		}
		askWaveData();

	}
	public void WavesCompleted()
	{
		timer.Stop();
		GD.Print("All waves completed, stopping timer.");
		// currWaveCount = totalWaves;
	}
	void _on_button_pressed()
	{
		if (timer.IsStopped())
		{
			timer.Start();
		}
		else
		{
			timer.SetPaused(!timer.IsPaused());
		}
		if (!started)
		{
			askWaveData();
			started = true;
		}
	}

	void _on_finished()
	{
		ShowWaveBanner("Congratulations! You won!");
		GetNode<PlayerStats>("/root/Main_Scene/PlayerStats").onFinished();
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
