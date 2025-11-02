using Godot;
using System;
using System.Threading.Tasks;

public partial class FadeOut : ColorRect
{
	[Export] private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		base._Ready();
		GameManagerScript.Instance.SetFadeOut(this);
	}

	public async Task DoFadeOut()
	{
		GD.Print("DoFadeOut");
		animationPlayer.Play("fade_out");
		await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	}

	public async Task DoFadeIn()
	{
		animationPlayer.Play("fade_in");
		await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	}
}
