using Godot;
using TheDeepFeast;

public partial class RoundController : Node2D
{
	[Export] public int StealthModeStartRound = 3;
	[Export] public int BeastCanBeComfortedStartRound = 5;
	[Export] public int StartRound = 1;
	[Export] public BeastStealthMode BeastStealthMode;
	[Export] public Fish Beast;
	[Export] private bool testStealthMode;
	[Export] public RoundData[] RoundData;
	[Export] private Label RoundLabelDebug;
	
	private bool isStealthMode;
	private RoundData currentRoundData;

	public override void _Ready()
	{
		base._Ready();
		GameManagerScript.Instance.SetRoundController(this);
		CallDeferred("Init");
	}

	private void Init()
	{
		GD.Print("Start init");
		foreach (var data in RoundData)
		{
			data.Obstacles.Hide();
		}

		if (testStealthMode)
		{
			ShowDefaultObstacles();
			StartStealthMode();
		}
		else if (IsRoundWithStealth())
		{
			ShowCurrentObstacles();
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
		
		RoundLabelDebug.Text = $"Round: {GameStateScript.Instance.Round}";
		GD.Print($"Round: {GameStateScript.Instance.Round}");
	}

	private void ShowDefaultObstacles()
	{
		currentRoundData = RoundData[0];
		currentRoundData.Obstacles.Show();
	}

	private void ShowCurrentObstacles()
	{
		var stealthModeRoundIndex = GameStateScript.Instance.Round - StealthModeStartRound;

		if (RoundData.Length > stealthModeRoundIndex)
		{
			currentRoundData = RoundData[stealthModeRoundIndex];
		}
		else
		{
			currentRoundData = RoundData[^1];
		}
		currentRoundData.Obstacles.Show();
	}

	public void StartStealthMode()
	{
		Beast.Hide();
		BeastStealthMode.StartRoute(currentRoundData.Route);
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
