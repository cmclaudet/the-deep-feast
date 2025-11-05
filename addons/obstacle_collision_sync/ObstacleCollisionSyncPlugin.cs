#if TOOLS
using Godot;

[Tool]
public partial class ObstacleCollisionSyncPlugin : EditorPlugin
{
	private double _timeAccumulator = 0;

	public override void _EnterTree()
	{
		GD.Print("ObstacleCollisionSyncPlugin active");
	}

	public override void _ExitTree()
	{
		GD.Print("ObstacleCollisionSyncPlugin disabled");
	}

	public override void _Process(double delta)
	{
		_timeAccumulator += delta;
		if (_timeAccumulator < 0.5) // every 0.5s
			return;

		_timeAccumulator = 0;
		var root = GetEditorInterface().GetEditedSceneRoot();
		if (root != null)
			SyncAllObstacles(root);
	}

	private void SyncAllObstacles(Node node)
	{
		foreach (Node child in node.GetChildren())
			ProcessNode(child);
	}

	private void ProcessNode(Node node)
	{
		var script = node.GetScript();
		if (script.VariantType == Variant.Type.Object)
		{
			var godotObj = script.AsGodotObject();
			if (godotObj is Resource resource && resource.ResourcePath.EndsWith("Obstacle.cs"))
			{
				SyncObstacle(node);
			}
		}
		foreach (Node child in node.GetChildren())
		{
			ProcessNode(child);
		}
	}

	private void SyncObstacle(Node obstacleNode)
	{
		NodePath collisionPath = (NodePath)obstacleNode.Get("CollisionShapePath");

		Node collisionNode = obstacleNode.GetNodeOrNull(collisionPath);
		if (collisionNode != null)
		{
			float left = (float)(double)obstacleNode.Get("offset_left");
			float top = (float)(double)obstacleNode.Get("offset_top");
			float right = (float)(double)obstacleNode.Get("offset_right");
			float bottom = (float)(double)obstacleNode.Get("offset_bottom");

			Vector2 rectSize = new Vector2(right - left, bottom - top);

			var shape = (RectangleShape2D)collisionNode.Get("shape");
			Vector2 colliderShapeExtents = (Vector2)shape.Get("extents");
			Vector2 newExtents = rectSize / 2;

			if (!colliderShapeExtents.IsEqualApprox(newExtents))
			{
				GD.Print($"ObstacleCollisionSyncPlugin: extents differs, updating {obstacleNode.Name}");
				shape.Set("extents", newExtents);
				collisionNode.Set("position", newExtents);
				
				NodePath lightOccluderPath = (NodePath)obstacleNode.Get("LightOccluderPath");
				Node lightOccluderNode = obstacleNode.GetNodeOrNull(lightOccluderPath);
				if (lightOccluderNode == null)
				{
					return;
				}
				
				var occluderPolygon = (OccluderPolygon2D)lightOccluderNode.Get("occluder");
				Vector2[] polygonProperty = (Vector2[])occluderPolygon.Get("polygon");
				
				Vector2 half = rectSize / 2;

				polygonProperty[0] = new Vector2(-half.X, -half.Y);
				polygonProperty[1] = new Vector2(half.X, -half.Y);
				polygonProperty[2] = new Vector2(half.X, half.Y);
				polygonProperty[3] = new Vector2(-half.X, half.Y);

				lightOccluderNode.Set("position", newExtents);
				
				occluderPolygon.Set("polygon", polygonProperty);
			}
		}
	}
}
#endif
