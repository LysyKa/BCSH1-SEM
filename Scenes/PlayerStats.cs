using Godot;
using System;
using System.IO;
using System.Numerics;

public partial class PlayerStats : Node2D
{
	public String playerName = "Default";
	public int playerScore = 0;
	public int victories = 0;
	public int playerHealth = 20;
	public int playerGold = 10;


	public int[] upgrades = { 0, 0, 0 };


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		tryLoadingSavedData();
		// trySavingData();
		GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelLivesLeft").Text = "Lives left: " + playerHealth;
		GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelHighscore").Text = "Score: " + playerScore;
		updateGold();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void updateUpgrades(int upNumber, bool valid, int scoreValue)
	{
		if (playerScore >= scoreValue)
		{
			switch (upNumber)
			{
				case 0:
					GD.Print("Upgrade 0 activated");
					updateGold();
					playerGold = 20;
					break;
				case 1:
					GD.Print("Upgrade 1 activated");
					GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelLivesLeft").Text = "Lives left: " + playerHealth;
					playerHealth = 40;
					break;
			}
		}
		else
		{
			GD.Print("Not enough score");
		}
	}
	public void updateGold()
	{
		GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelGold").Text = "Current gold: " + playerGold;
	}


	public void _enemyDiedEventHandler(int bounty)
	{
		//GD.Print("Enemy died, bounty: " + bounty);
		playerGold += bounty;
		playerScore += bounty;
		GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelGold").Text = "Current gold: " + playerGold;
		GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelLivesLeft").Text = "Lives left: " + playerHealth;
		GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelHighscore").Text = "Score: " + playerScore;
	}

	public void _enemyPassedEventHandler(int damage)
	{
		//GD.Print("Enemy passed, damage: " + damage);
		playerHealth -= damage;
		GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelLivesLeft").Text = "Lives left: " + playerHealth;

		if (playerHealth <= 0)
		{
			Engine.TimeScale = 0f;
			GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelHighscore").Text = "Score: " + playerScore;
			string text = $"Game Over! Your highscore is: {playerScore} and your victories: {victories}";
			AcceptDialog gameOverDialog = new AcceptDialog();
			gameOverDialog.DialogText = text;
			gameOverDialog.Title = "Game Over";
			this.AddChild(gameOverDialog);
			gameOverDialog.PopupCentered();
			trySavingData();
			gameOverDialog.Confirmed += () => GetTree().Quit();
		}
	}
	public void onFinished()
	{
		Engine.TimeScale = 0f;
		victories++;
		playerScore += 100;
		string text = $"You are victorious!\n Your highscore is: {playerScore} and your victories: {victories}";
		AcceptDialog gameOverDialog = new AcceptDialog();
		gameOverDialog.DialogText = text;
		gameOverDialog.Title = "Victory";
		this.AddChild(gameOverDialog);
		gameOverDialog.PopupCentered();
		trySavingData();
		gameOverDialog.Confirmed += () => GetTree().Quit();
	}

	private bool tryLoadingSavedData()
	{
		string line;

		try
		{

			using (StreamReader reader = File.OpenText("./saveData.txt"))
			{
				string name = "Default";
				int highScore = 0;
				int victoryCount = 0;
				while ((line = reader.ReadLine()) != null)
				{

					string[] parts = line.Split(';');
					name = parts[0];
					highScore = int.Parse(parts[1]);
					victoryCount = int.Parse(parts[2]);
				}
				reader.Close();
				playerName = name;
				playerScore = highScore;
				victories = victoryCount;
				GD.Print("Loaded data successfully.");
				return true;

			}
		}
		catch (Exception e)
		{
			GD.Print("Failed to load saved data: " + e.Message);
			playerName = "Default";
			playerScore = 0;
			victories = 0;
		}

		return false;
	}

	public void trySavingData()
	{
		try
		{
			using (StreamWriter writer = File.CreateText("./saveData.txt"))
			{
				writer.WriteLine($"{playerName};{playerScore};{victories}");
				writer.Close();
				GD.Print("Saved data successfully.");
			}
		}
		catch (Exception e)
		{
			GD.Print("Failed to save data: " + e.Message);
		}
	}




}
