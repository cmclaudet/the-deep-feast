using Godot;
using System;
using System.Linq;

public partial class GameManagerScript : Node
{
	public static GameManagerScript Instance { get; private set; }

	private Prompt prompt;
	private Player player;
	private ControlPanel controlPanel;
	private FishTank[] fishTanks = [];
	private Exit exit;
	private FadeOut fadeOut;
	private RoundController roundController;

	public Prompt Prompt => prompt;
	public Player Player => player;
	public FadeOut FadeOut => fadeOut;

	public override void _Ready()
	{
		Instance = this;
	}
	
	public void SetRoundController(RoundController roundController) => this.roundController = roundController;
	
	public void SetFadeOut(FadeOut fadeOut) => this.fadeOut = fadeOut;

	public void SetPrompt(Prompt prompt)
	{
		this.prompt = prompt;
	}

	public void SetFishTank(FishTank fishTank)
	{
		fishTanks = fishTanks.Append(fishTank).ToArray();
	}

	public void SetPlayer(Player player)
	{
		this.player = player;
	}

	public void SetControlPanel(ControlPanel controlPanel)
	{
		this.controlPanel = controlPanel;
	}
	
	public void SetExit(Exit exit) => this.exit = exit;

	public void FeedFish(string fishName)
	{
		GD.Print("Try to feed fish " + fishName);
		foreach (var fishTank in fishTanks)
		{
			var isFish = fishTank.IsFish(fishName);
			if (isFish)
			{
				GD.Print("Found fish tank for fish " + fishTank.Name);
				fishTank.FeedFish();
				break;
			}
		}

		controlPanel.OnFishFeed();
		if (roundController.IsRoundWithStealth() && GameStateScript.Instance.IsAllButBeastFed())
		{
			roundController.StartStealthMode();
		}
		else if (GameStateScript.Instance.IsAllFishFed())
		{
			exit.SetUnlocked();
		}
	}

	public bool IsBeastFeedable()
	{
		return roundController.IsBeastFeedable();
	}

	public bool IsRoundWithStealth()
	{
		return roundController.IsRoundWithStealth();
	}
}
