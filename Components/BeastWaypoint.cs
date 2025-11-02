using Godot;

public partial class BeastWaypoint : Node2D
{
	[Export] private Area2D area2D;
	[Export] public double duration;
	private bool isActive;

	public void ToggleActive(bool active)
	{
		isActive = active;
		area2D.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (isActive && body is Player)
		{
			GD.Print("Player dies!");
		}
	}
}
