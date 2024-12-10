using Godot;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class GenericController : CharacterBody2D
{
	private AnimatedSprite2D playerSprite;
	private RichTextLabel label;

	private float acceleration = 0;
	private float decelleration = 0.6f;

	private float walkSpeed = 90;
	private float runSpeed = 140;

	private Vector2 velocity = new Vector2(0, 0);
	private Vector2 joyAxis = new Vector2(0, 0);

	//fake y value
	private float y = 0;
	private float oldY = 0;
	private float yVelocity = 0;
	private float gravitySpeed = 0.25f;

	public override void _Ready()
	{
		playerSprite = GetNode<AnimatedSprite2D>("CharacterSprite");
		label = GetNode<RichTextLabel>("CharacterSprite/ColorRect/RichTextLabel");

		acceleration = walkSpeed;
	}

	public override void _Process(double delta)
	{
		//get joystick input
		joyAxis = new Vector2(Input.GetJoyAxis(0, JoyAxis.LeftX), Input.GetJoyAxis(0, JoyAxis.LeftY));

		//forced deadzone (evil mode)
		if(Math.Abs(joyAxis.X) < 0.08) joyAxis.X = 0;
		if(Math.Abs(joyAxis.Y) < 0.08) joyAxis.Y = 0;

		//obvious...
		processAnimations();

		if(Input.IsActionPressed("menu"))
			acceleration = runSpeed;
		else
			acceleration = walkSpeed;

		if(Input.IsActionJustPressed("select") && y == 0)
			yVelocity = -5f;

		//interpolate oldY and y to get framerate-independant y
		oldY = Mathf.Lerp(oldY, y, ((float)delta*10));
		playerSprite.Position = new Vector2(0, oldY);
	}

	public override void _PhysicsProcess(double delta)
	{
		//for lerp
		oldY = y;

		//increase velocity based on joy angle and acceleration
		velocity += joyAxis * (acceleration);

		//decelerate character
		velocity *= decelleration;

		//if the velocity is 'basically' zero, set it to zero
		if(Math.Abs(velocity.X) + Math.Abs(velocity.Y) < 0.1)
			velocity = Vector2.Zero;

		//gravity
		yVelocity += gravitySpeed;

		y += yVelocity;

		//if the player is on or below the floor, stop their movement and snap their y to the floor
		if(y > 0)
		{
			y = 0;
			yVelocity = 0;
		}

		//set 'real' velocity to the one we control, then move
		Velocity = velocity;
		MoveAndSlide();

		//fancy debug text
		label.Text = string.Format("Position: ({0:0.##}, {1:0.##}, {2:0.##})\nSpeed: ({3:0.##}, {4:0.##}, {5:0.##})", Position.X, y, Position.Y, velocity.X, yVelocity, velocity.Y);
	}

	public void processAnimations()
	{
		string toPlay = "idle";

		if(velocity != Vector2.Zero)
		{
			if(velocity.X > velocity.Y && velocity.X > -velocity.Y)
			{
				toPlay = "walk_left";
				playerSprite.FlipH = true;
			}
			else if(-velocity.X > velocity.Y && -velocity.X > -velocity.Y)
			{
				toPlay = "walk_left";
				playerSprite.FlipH = false;
			}
			else if(-velocity.Y > velocity.X && -velocity.Y > -velocity.X)
				toPlay = "walk_up";
			else if(velocity.Y > velocity.X && velocity.Y > -velocity.X)
				toPlay = "walk_down";

			playIfNot(toPlay);
		}
		else
			playerSprite.Frame = 1;

		playerSprite.SpeedScale = (Math.Abs(velocity.X) + Math.Abs(velocity.Y)/2) / walkSpeed;
		//((ShaderMaterial)playerSprite.Material).SetShaderParameter("vSpeed", 1+(Math.Abs(yVelocity)/20));
	}

	public void playIfNot(string animation)
	{
		if(playerSprite.Animation != animation)
			playerSprite.Play(animation);
	}
}
