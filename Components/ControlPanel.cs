using Godot;
using System;
using DialogueManagerRuntime;

public partial class ControlPanel : ColorRect
{
	[Export] private Sprite2D foodSprite;
	private Area2D area2D;
	private bool canPlaceFood;
	private bool containsFood;
	private bool canFeed;

	public override void _Ready()
	{
		GameManagerScript.Instance.SetControlPanel(this);
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
				var dialogue = GD.Load<Resource>("res://Dialogue/ChooseFish.dialogue");
				DialogueManager.ShowExampleDialogueBalloon(dialogue, "feedFish");
			}
			else if (canPlaceFood)
			{
				GD.Print("Placing food");
				GameManagerScript.Instance.Player.DisableCarriedObjectSprite();
				SetCannotPlaceFood();
				foodSprite.Visible = true;
				containsFood = true;
				SetCanFeed();
			}
		}
	}

	public void OnFishFeed()
	{
		SetCannotFeed();
		SetCannotPlaceFood();
		foodSprite.Visible = false;
		containsFood = false;
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
			var instancePrompt = GameManagerScript.Instance.Prompt;
			if (containsFood && !GameManagerScript.Instance.Player.IsCarryingObject)
			{
				SetCanFeed();
			}
			else if (GameManagerScript.Instance.Player.IsCarryingObject)
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
		var instancePrompt = GameManagerScript.Instance.Prompt;
		instancePrompt.SetText("FEED (SPACE)");
		instancePrompt.SetOver(area2D, -50);
		instancePrompt.ToggleDisplay(true);
		canFeed = true;
	}

	private void SetCannotPlaceFood()
	{
		GameManagerScript.Instance.Prompt.ToggleDisplay(false);
		canPlaceFood = false;
	}
	
	private void SetCannotFeed()
	{
		GameManagerScript.Instance.Prompt.ToggleDisplay(false);
		canFeed = false;
	}
}
