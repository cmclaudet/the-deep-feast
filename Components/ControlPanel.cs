using Godot;
using System;

public partial class ControlPanel : ColorRect
{
	[Export] private Sprite2D foodSprite;
	private Area2D area2D;
	private bool canPlaceFood;
	private bool containsFood;
	private bool canFeed;

	public override void _Ready()
	{
		foodSprite.Visible = false;
		area2D = GetNode<Area2D>("Area2D");
		area2D.BodyEntered += OnBodyEntered;
		area2D.BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (Input.IsActionJustReleased("ui_accept"))
		{
			if (canFeed)
			{
				GD.Print("Show dialogue to choose fish to feed");
			}
			else if (canPlaceFood)
			{
				GD.Print("Placing food");
				GameManager.Instance.Player.DisableCarriedObjectSprite();
				SetCannotPlaceFood();
				foodSprite.Visible = true;
				containsFood = true;
				SetCanFeed();
			}
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body is Player)
		{
			SetCannotPlaceFood();
			SetCannotFeed();
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			var instancePrompt = GameManager.Instance.Prompt;
			if (containsFood)
			{
				SetCanFeed();
			}
			else if (GameManager.Instance.Player.IsCarryingObject)
			{
				instancePrompt.SetText("PLACE FOOD (SPACE)");
				instancePrompt.SetOver(area2D, -50);
				instancePrompt.ToggleDisplay(true);
				canPlaceFood = true;
			}
		}
	}

	private void SetCanFeed()
	{
		var instancePrompt = GameManager.Instance.Prompt;
		instancePrompt.SetText("FEED (SPACE)");
		instancePrompt.SetOver(area2D, -50);
		instancePrompt.ToggleDisplay(true);
		canFeed = true;
	}

	private void SetCannotPlaceFood()
	{
		GameManager.Instance.Prompt.ToggleDisplay(false);
		canPlaceFood = false;
	}
	
	private void SetCannotFeed()
	{
		GameManager.Instance.Prompt.ToggleDisplay(false);
		canFeed = false;
	}
}
