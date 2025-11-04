using Godot;
using System;

public partial class RoundController : Node2D
{
	[Export] public int StealthModeStartRound = 3;
	[Export] public int BeastCanBeComfortedStartRound = 5;
	[Export] public int StartRound = 1;
	[Export] public BeastStealthMode BeastStealthMode;
	[Export] public Fish Beast;
	[Export] private bool testStealthMode;
	
	private bool isStealthMode;

	public override void _Ready()
	{
		base._Ready();
		GameStateScript.Instance.Round = StartRound;
		GameManagerScript.Instance.SetRoundController(this);
		if (testStealthMode)
		{
			StartStealthMode();
		}
		else if (IsRoundWithStealth())
		{
			if (GameStateScript.Instance.IsAllButBeastFed())
			{
				StartStealthMode();
			}
			else
			{
				BeastStealthMode.Hide();
				Beast.Hide();
			}
		}
		else
		{
			BeastStealthMode.Hide();
			Beast.Show();
		}
	}

	public void StartStealthMode()
	{
		Beast.Hide();
		BeastStealthMode.Show();
		BeastStealthMode.StartRoute();
		isStealthMode = true;
	}

	public bool IsRoundWithStealth()
	{
		return GameStateScript.Instance.Round >= StealthModeStartRound;
	}

	public bool IsBeastFeedable()
	{
		return !GameStateScript.Instance.IsBeastFed && (!IsRoundWithStealth() || isStealthMode);
	}
}
