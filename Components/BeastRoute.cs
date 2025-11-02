using Godot;
using System;

public partial class BeastRoute : Node
{
	[Export] public double lightMoveSpeed;
	[Export] public BeastWaypoint[] waypoints;
}
