using Godot;
using System;

public partial class Prompt : Node2D
{
	private Label label;
	
	public override void _Ready()
	{
		GameManagerScript.Instance.SetPrompt(this);
		label = GetNode<Label>("Label");
		ToggleDisplay(false);
	}

	public void ToggleDisplay(bool shouldDisplay)
	{
		Visible = shouldDisplay;
	}

	public void SetText(string text)
	{
		label.Text = text;
	}

	public void SetOver(Node2D node, float yOffset = -10)
	{
		SetPosition(node.GlobalPosition + new Vector2(0, yOffset));
	}
}
