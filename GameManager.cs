using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	[Export] private Prompt prompt;
	[Export] private Player player;

	public Prompt Prompt => prompt;
	public Player Player => player;

	public override void _Ready()
	{
		Instance = this;
	}
}
