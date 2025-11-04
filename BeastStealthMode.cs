using System;
using System.Threading.Tasks;
using Godot;

public partial class BeastStealthMode : Node2D
{
	[Export] private Light2D light;
	[Export] private BeastRoute route;
	[Export] private float speed;
	private int waypointIndex;
	private BeastWaypoint activeWaypoint;
	private bool shouldMoveToWaypoint;
	private bool shouldKillPlayer;
	private bool isReloading;
	public bool IsRouteActive { get; private set; }
	
	private double timeSinceWaypoint;
	
	public void StartRoute()
	{
		waypointIndex = 0;
		activeWaypoint = route.waypoints[waypointIndex];
		shouldMoveToWaypoint = true;
		IsRouteActive = true;
		GameManagerScript.Instance.SetBeastStealthMode(this);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (shouldKillPlayer)
		{
			IsRouteActive = false;
			var distance = GameManagerScript.Instance.Player.GlobalPosition - GlobalPosition;
			GlobalPosition += distance.Normalized() * speed * (float)delta;
			if (distance.Length() < 300)
			{
				if (!isReloading)
				{
					GameManagerScript.Instance.Reload();
					isReloading = true;
				}
			}
		}
		else if (shouldMoveToWaypoint)
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

	public void StopRoute()
	{
		IsRouteActive = false;
		light.Hide();
		Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith(_ => Hide());
	}

	public void OnPlayerIsLit()
	{
		GD.Print("Beast stealth mode OnPlayerIsLit");
		shouldMoveToWaypoint = false;
		activeWaypoint = null;
		shouldKillPlayer = true;
	}
}
