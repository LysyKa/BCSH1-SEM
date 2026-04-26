using Godot;
using System;
using System.IO;
using System.Numerics;

public partial class PlayerStats : Node2D
{
	public String playerName = "Default";
	public int playerScore = 1;
	public int victories = 0;
	public int playerHealth = 10;
	public int playerGold = 500;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		tryLoadingSavedData();
		trySavingData();
		GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelLivesLeft").Text = "Lives left: " + playerHealth;
		updateGold();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void updateGold()
	{
		GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelGold").Text = "Current gold: " + playerGold;
	}


	public void _enemyDiedEventHandler(int bounty)
	{
		// Handle enemy death, e.g., increase player score by bounty
		GD.Print("Enemy died, bounty: " + bounty);
		playerGold += bounty;
		GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelGold").Text = "Current gold: " + playerGold;
		GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelLivesLeft").Text = "Lives left: " + playerHealth;
	}

	public void _enemyPassedEventHandler(int damage)
	{
		// Handle enemy passing, e.g., decrease player health by damage
		GD.Print("Enemy passed, damage: " + damage);
		playerHealth -= damage;
		GetNode<Label>("../UICanvasLayer/UIControl/PanelPlayerStats/HBoxContainer/LabelLivesLeft").Text = "Lives left: " + playerHealth;

		if (playerHealth <= 0)
		{
			GD.Print("Game Over!");
		}
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
