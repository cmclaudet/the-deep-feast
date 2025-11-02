using Godot;
using System;
using System.Threading.Tasks;

public partial class FishTank : Node2D
{
	[Export] private CpuParticles2D fishFoodParticles;
	[Export] private Fish fish;
	[Export] private Label fishLabel;
	public override void _Ready()
	{
		GameManagerScript.Instance.SetFishTank(this);
		fishLabel.Text = fish.FishName;
	}

	public bool IsFish(string fishName)
	{
		return fishName.Equals(fish.FishName);
	}

	public void FeedFish()
	{
		GameStateScript.Instance.SetFishFed(fish.FishName);
		fishFoodParticles.Visible = true;
		
		GD.Print($"Feeding {fish.FishName}");
		
		Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith(_ => fishFoodParticles.Emitting = false);
	}
}
