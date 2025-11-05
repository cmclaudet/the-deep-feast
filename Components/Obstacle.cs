using Godot;
using System;

public partial class Obstacle : ColorRect
{
	// [Export] public LightOccluder2D LightOccluder;
	// [Export] public CollisionShape2D CollisionShape;
	
	[Export] public NodePath LightOccluderPath;
	[Export] public NodePath CollisionShapePath;
}
