using Godot;
using System;

public partial class FoodPile : Node2D
{
	private Area2D area2D;
	private bool canPickFood;
	[Export] private Texture2D foodTexture;
	
	public override void _Ready()
	{
		area2D = GetNode<Area2D>("Area2D");
		area2D.BodyEntered += OnBodyEntered;
		area2D.BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (Input.IsActionJustReleased("ui_accept") && canPickFood)
		{
			GD.Print("Picking food");
			GameManagerScript.Instance.Player.SetCarriedObjectSprite(foodTexture);
			SetCannotPickFood();
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player && !GameManagerScript.Instance.Player.IsCarryingObject)
		{
			GD.Print($"Trigger hit by Player");
			GameManagerScript.Instance.Prompt.SetText("PICK UP (SPACE)");
			GameManagerScript.Instance.Prompt.SetOver(this);
			GameManagerScript.Instance.Prompt.ToggleDisplay(true);
			canPickFood = true;
		}
	}
	
	private void OnBodyExited(Node2D body)
	{
		if (body is Player && !GameManagerScript.Instance.Player.IsCarryingObject)
		{
			SetCannotPickFood();
		}
	}

	private void SetCannotPickFood()
	{
		GameManagerScript.Instance.Prompt.ToggleDisplay(false);
		canPickFood = false;
	}
}
