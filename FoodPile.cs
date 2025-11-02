using Godot;
using System;

public partial class FoodPile : Node2D
{
	private Area2D area2D;
	public override void _Ready()
	{
		area2D = GetNode<Area2D>("Area2D");
		area2D.BodyEntered += OnBodyEntered;
		area2D.BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			GD.Print($"Trigger hit by Player");
			GameManager.Instance.Prompt.SetText("PICK UP (SPACE)");
			GameManager.Instance.Prompt.SetOver(this);
			GameManager.Instance.Prompt.ToggleDisplay(true);
		}
	}
	
	private void OnBodyExited(Node2D body)
	{
		if (body is Player)
		{
			GameManager.Instance.Prompt.ToggleDisplay(false);
		}
	}
}
