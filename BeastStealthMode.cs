using Godot;
using System;

public partial class BeastStealthMode : Node2D
{
	[Export] private Light2D light;
	[Export] private BeastRoute route;
	private int waypointIndex;
	private BeastWaypoint activeWaypoint;
	private bool shouldMoveToWaypoint;
	
	private double timeSinceWaypoint;
	
	public void StartRoute()
	{
		waypointIndex = 0;
		activeWaypoint = route.waypoints[waypointIndex];
		shouldMoveToWaypoint = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (shouldMoveToWaypoint)
		{
			var direction = activeWaypoint.GlobalPosition - light.GlobalPosition;
			var angleGoal = -direction.AngleTo(new Vector2(0, 1));
			var lightAngleDiff = angleGoal - light.Rotation;

			if (Mathf.Abs(lightAngleDiff) > 0.05)
			{
				light.Rotation += (lightAngleDiff < 0 ? -1 : 1) * (float)route.lightMoveSpeed * (float)delta;
			}
			else
			{
				light.Rotation = angleGoal;
				shouldMoveToWaypoint = false;
			}
		} 
		else if (activeWaypoint != null)
		{
			timeSinceWaypoint += delta;
			if (timeSinceWaypoint >= activeWaypoint.duration)
			{
				timeSinceWaypoint = 0;
				waypointIndex = (waypointIndex + 1) % route.waypoints.Length;
				activeWaypoint = route.waypoints[waypointIndex];
				shouldMoveToWaypoint = true;
			}
		}
	}
}
