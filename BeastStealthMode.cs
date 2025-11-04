using System;
using System.Threading.Tasks;
using Godot;

public partial class BeastStealthMode : Node2D
{
	[Export] private Light2D light;
	[Export] private float speed;
	private int waypointIndex;
	private BeastWaypoint activeWaypoint;
	private bool shouldMoveToWaypoint;
	private bool shouldKillPlayer;
	private bool isReloading;
	private BeastRoute activeRoute;
	public bool IsRouteActive { get; private set; }
	
	private double timeSinceWaypoint;
	
	private float angleDiff;
	private float scaleDiff;
	private bool startHide;
	private double timeSinceStartHide;

	public void StartRoute(BeastRoute route)
	{
		activeRoute = route;
		waypointIndex = 0;
		SetActiveWaypoint(route.waypoints[waypointIndex], setScaleAndAngleImmediate: true);
		IsRouteActive = true;
		GameManagerScript.Instance.SetBeastStealthMode(this);
	}

	public override void _Process(double delta)
	{
		if (startHide)
		{
			timeSinceStartHide += delta;
			if (timeSinceStartHide > 5)
			{
				timeSinceStartHide = 0;
				Hide();
				startHide = false;
			}
		}
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
			var displacement = activeWaypoint.GlobalPosition - light.GlobalPosition;
			var angleGoal = -displacement.AngleTo(new Vector2(0, 1));
			
			var angleIncrement = angleDiff * (delta / activeWaypoint.lightMoveDuration);
			var scaleIncrement = scaleDiff * delta / activeWaypoint.lightMoveDuration ;

			if (Mathf.Abs(angleGoal - light.Rotation) > 0.02)
			{
				light.Rotation += (float)angleIncrement;
				light.Scale = new Vector2(light.Scale.X + (float)scaleIncrement, light.Scale.Y);
			}
			else
			{
				light.Rotation = angleGoal;
				light.Scale = new Vector2(activeWaypoint.coneScale, light.Scale.Y);
				shouldMoveToWaypoint = false;
			}
		} 
		else if (activeWaypoint != null)
		{
			timeSinceWaypoint += delta;
			if (timeSinceWaypoint >= activeWaypoint.duration)
			{
				timeSinceWaypoint = 0;
				waypointIndex = (waypointIndex + 1) % activeRoute.waypoints.Length;
				SetActiveWaypoint(activeRoute.waypoints[waypointIndex]);
			}
		}
	}

	public void StopRoute()
	{
		IsRouteActive = false;
		light.Hide();
		startHide = true;
	}

	public void OnPlayerIsLit()
	{
		GD.Print("Beast stealth mode OnPlayerIsLit");
		shouldMoveToWaypoint = false;
		activeWaypoint = null;
		shouldKillPlayer = true;
	}

	private void SetActiveWaypoint(BeastWaypoint waypoint, bool setScaleAndAngleImmediate = false)
	{
		activeWaypoint = waypoint;
		shouldMoveToWaypoint = true;
		var startAngle = light.Rotation;
		var startScale = light.Scale.X;
		
		var displacement = activeWaypoint.GlobalPosition - light.GlobalPosition;
		var angleGoal = -displacement.AngleTo(new Vector2(0, 1));

		var coneScaleGoal = activeWaypoint.coneScale;
		if (setScaleAndAngleImmediate)
		{
			light.Scale = new Vector2(coneScaleGoal, light.Scale.Y);
			scaleDiff = 0;
			light.Rotation = angleGoal;
			angleDiff = 0;
		}
		else
		{
			scaleDiff = coneScaleGoal - startScale;
			angleDiff = angleGoal - startAngle;
		}
	}
}
