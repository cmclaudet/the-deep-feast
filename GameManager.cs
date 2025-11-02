using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	[Export] private Prompt prompt;

	public Prompt Prompt => prompt;

	public override void _Ready()
	{
		Instance = this;
	}
}
