using Godot;
using System;
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

	public override void _Ready()
	{
		playerSprite = GetNode<AnimatedSprite2D>("CharacterSprite");
		label = GetNode<RichTextLabel>("ColorRect/RichTextLabel");

		acceleration = walkSpeed;
	}

	public override void _Process(double delta)
	{
		joyAxis = new Vector2(Input.GetJoyAxis(0, JoyAxis.LeftX), Input.GetJoyAxis(0, JoyAxis.LeftY));

		if(Math.Abs(joyAxis.X) < 0.08) joyAxis.X = 0;
		if(Math.Abs(joyAxis.Y) < 0.08) joyAxis.Y = 0;

		processAnimations();

		if(Input.IsActionPressed("menu"))
			acceleration = runSpeed;
		else
			acceleration = walkSpeed;
	}

	public override void _PhysicsProcess(double delta)
	{
		//increase velocity based on joy angle and acceleration
		velocity += joyAxis * (acceleration);

		//decelerate character
		velocity *= decelleration;

		//if the velocity is 'basically' zero, set it to zero
		if(Math.Abs(velocity.X) + Math.Abs(velocity.Y) < 0.1)
			velocity = Vector2.Zero;

		//set 'real' velocity to the one we control, then move
		Velocity = velocity;
		MoveAndSlide();

		label.Text = string.Format("Position: ({0:0.##}, {1:0.##})\nSpeed: ({2:0.##}, {3:0.##})", Position.X, Position.Y, velocity.X, velocity.Y);
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
	}

	public void playIfNot(string animation)
	{
		if(playerSprite.Animation != animation)
			playerSprite.Play(animation);
	}
}
