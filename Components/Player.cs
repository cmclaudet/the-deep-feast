using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float MoveSpeed { get; set; } = 200.0f;
	[Export] public float JumpForce { get; set; } = 400.0f;
	[Export] public float Gravity { get; set; } = 900.0f;
	[Export] private Sprite2D CarriedObjectSprite;
	private AnimatedSprite2D animatedSprite;
	
	public bool IsCarryingObject => CarriedObjectSprite.Visible && CarriedObjectSprite != null;
	
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Play("idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;
		else if (velocity.Y > 0)
			velocity.Y = 0;

		float inputDir = Input.GetAxis("ui_left", "ui_right");
		velocity.X = inputDir * MoveSpeed;
		//
		// if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		// 	velocity.Y = -JumpForce;

		Velocity = velocity;
		MoveAndSlide();
	}
	
	public override void _Process(double delta)
	{
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
}
