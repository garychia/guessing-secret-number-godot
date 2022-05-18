using System;
using System.Collections.Generic;
using Godot;

public class NumberPanel : GridContainer
{
	// Label that displays the number the user guessed.
	private Label displayLabel;
	// Buttons that represent numbers from 0 to 9 and other controls.
	private List<Button> panelButtons = new List<Button>();
	// Number the user is currently inputting.
	private int _displayNumber = 0;
	// Number that the display label will be displaying.
	private int DisplayNumber
	{
		get => _displayNumber;
		set
		{
			_displayNumber = value;
			displayLabel.Text = _displayNumber.ToString();
		}
	}
	// The Secret Number.
	private int secretNumber;

	// Called when a button is pressed.
	// param buttonText: the text of the pressed button.
	private void OnNumberButtonPressed(string buttonText)
	{
		// Change the state of the game based on the text
		// of the pressed button.
		switch (buttonText)
		{
			// Clear Button
			case "Clear":
				DisplayNumber = 0;
				break;
			// Confirm Button
			case "Confirm":
				CheckUserGuess();
				break;
			// Restart Button
			case "Restart":
				RestartGame();
				break;
			// Other Number Buttons (0 ~ 9)
			default:
				try
				{
					var buttonNumber = int.Parse(buttonText);
					DisplayNumber = DisplayNumber * 10 + buttonNumber;
				}
				catch (Exception)
				{
					GD.Print("Unrecognized button: " + buttonText);
				}
				break;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Find the nodes of the label and buttons.
		displayLabel = GetNode<Label>("../DisplayLabel");
		FindNumberButtons();
		SetupNumberButtonSignals();
		// Generate a secret number randomly.
		GenerateSecretNumber();
	}

	// Randomly generate a secret number between 1 and 100, inclusively.
	private void GenerateSecretNumber()
	{
		var rand = new Random();
		secretNumber = rand.Next(1, 100);
	}

	// Check if the user guesses the secret number correctly.
	private void CheckUserGuess()
	{
		string result;
		if (DisplayNumber > secretNumber)
			result = "Too Large.";
		else if (DisplayNumber < secretNumber)
			result = "Too Small.";
		else
			result = "You Won!";
		DisplayNumber = 0;
		displayLabel.Text = result;
	}

	// Reset the state of the game.
	private void RestartGame()
	{
		DisplayNumber = 0;
		GenerateSecretNumber();
	}

	// Find the nodes of the buttons.
	private void FindNumberButtons()
	{
		var panelRows = GetChildren();
		foreach (var panelRow in panelRows)
			if (panelRow is Node)
			{
				var children = ((Node)panelRow).GetChildren();
				foreach (var child in children)
					if (child is Button)
						panelButtons.Add(child as Button);
			}
	}

	// Setup the signal of each button.
	private void SetupNumberButtonSignals()
	{
		foreach (var button in panelButtons)
			button.Connect(
				"pressed", this,
				"OnNumberButtonPressed",
				new Godot.Collections.Array { button.Text });
	}
}
