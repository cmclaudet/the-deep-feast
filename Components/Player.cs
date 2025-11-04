using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public PointLight2D Light;
	[Export] private BeastStealthMode beastStealthMode;

	[Export] public float MoveSpeed { get; set; } = 200.0f;
	[Export] public float JumpForce { get; set; } = 400.0f;
	[Export] public float Gravity { get; set; } = 900.0f;
	[Export] private Sprite2D CarriedObjectSprite;
	[Signal] public delegate void PlayerIsLitEventHandler();
	private AnimatedSprite2D animatedSprite;
	
	private bool canControl = true;
	
	public bool IsCarryingObject => CarriedObjectSprite.Visible && CarriedObjectSprite != null;
	
	public override void _Ready()
	{
		GameManagerScript.Instance.SetPlayer(this);
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Play("idle");
		ToggleControl(true);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!canControl)
		{
			return;
		}
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;
		else if (velocity.Y > 0)
			velocity.Y = 0;

		float inputDir = Input.GetAxis("ui_left", "ui_right");
		velocity.X = inputDir * MoveSpeed;
		// disable jump for now, maybe we turn it on later
		// if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		// 	velocity.Y = -JumpForce;

		Velocity = velocity;
		MoveAndSlide();
	}
	
	public override void _Process(double delta)
	{
		if (!canControl)
		{
			return;
		}
		if (Velocity.X > 0)
		{
			animatedSprite.FlipH = false;
			animatedSprite.Play("walk");
		}
		else if (Velocity.X < 0)
		{
			animatedSprite.FlipH = true;
			animatedSprite.Play("walk");
		}
		else 
		{
			animatedSprite.Play("idle");
		}

		if (Input.IsActionPressed("ui_accept") && IsCarryingObject)
		{
			DisableCarriedObjectSprite();
		}
		
		bool lit = beastStealthMode.IsRouteActive && IsPlayerLit();

		if (lit)
		{
			ToggleControl(false);
			EmitSignalPlayerIsLit();
			GD.Print("Player lit");
		}
	}

	public void ToggleControl(bool value)
	{
		if (!value)
		{
			Velocity = Vector2.Zero;
			animatedSprite.Play("idle");
		}
		canControl = value;
	}

	public void SetCarriedObjectSprite(Texture2D carriedObject)
	{
		CarriedObjectSprite.Texture = carriedObject;
		CarriedObjectSprite.Show();
	}

	public void DisableCarriedObjectSprite()
	{
		CarriedObjectSprite.Texture = null;
		CarriedObjectSprite.Hide();
	}
	
	private bool IsPlayerLit()
	{
		if (Light == null)
			return false;
		
		Vector2 lightSize = Light.Texture.GetSize() * Light.Scale;
		var playerToLight = GlobalPosition - Light.GlobalPosition;
		var playerToLightAngle = -playerToLight.AngleTo(new Vector2(0, 1));
		
		var lightConeAngle = Mathf.Atan(lightSize.X / (2 * lightSize.Y));

		var withinMinAngleRange = playerToLightAngle >  Light.Rotation - lightConeAngle;
		var withinMaxAngleRange = playerToLightAngle < Light.Rotation + lightConeAngle;

		if (withinMinAngleRange && withinMaxAngleRange)
		{
			var spaceState = GetWorld2D().DirectSpaceState;
			var query = PhysicsRayQueryParameters2D.Create(Light.GlobalPosition, GlobalPosition);
			query.CollisionMask = 1 << 1;

			var result = spaceState.IntersectRay(query);
			
			return result.Count == 0;
		}
		return false;
	}
}
