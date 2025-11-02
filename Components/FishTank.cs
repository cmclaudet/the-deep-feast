using Godot;
using System;

public partial class FishTank : Node2D
{
	[Export] private CpuParticles2D fishFoodParticles;
	[Export] private Fish fish;
	public override void _Ready()
	{
		GameManagerScript.Instance.SetFishTank(this);
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
	}
}
