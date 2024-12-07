using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class GenericController : RigidBody2D
{
	private AnimatedSprite2D playerSprite;
	private float moveSpeed = 85;

	private Vector2 joyAxis = new Vector2(0, 0);

	public override void _Ready()
	{
		playerSprite = GetNode<AnimatedSprite2D>("CharacterSprite");
	}

	public override void _Process(double delta)
	{
		Vector2 rJoyAxis = new Vector2(Input.GetJoyAxis(0, JoyAxis.RightX), Input.GetJoyAxis(0, JoyAxis.RightY));
		float distanceMoved = Math.Abs(rJoyAxis.X) + Math.Abs(rJoyAxis.Y);
		if(distanceMoved > 0.5)
		{

		}

		if(Input.IsActionPressed("menu"))
			moveSpeed = 150;
		else
			moveSpeed = 85;
	}

	public override void _PhysicsProcess(double delta)
	{
		joyAxis = new Vector2(Input.GetJoyAxis(0, JoyAxis.LeftX), Input.GetJoyAxis(0, JoyAxis.LeftY));
		float distanceMoved = Math.Abs(joyAxis.X) + Math.Abs(joyAxis.Y);

		if(distanceMoved < 0.5)
			joyAxis = new Vector2(0, 0);

		ApplyCentralImpulse(joyAxis*moveSpeed);

		processAnimations();

		playerSprite.SpeedScale = distanceMoved;
		if(distanceMoved < 0.5)
			playerSprite.Frame = 1;
	}

	public void processAnimations()
	{
		if(joyAxis.X > joyAxis.Y && joyAxis.X > -joyAxis.Y)
		{
			playIfNot("walk_left");
			playerSprite.FlipH = true;
		}
		else if(-joyAxis.X > joyAxis.Y && -joyAxis.X > -joyAxis.Y)
		{
			playIfNot("walk_left");
			playerSprite.FlipH = false;
		}
		else if(-joyAxis.Y > joyAxis.X && -joyAxis.Y > -joyAxis.X)
			playIfNot("walk_up");
		else if(joyAxis.Y > joyAxis.X && joyAxis.Y > -joyAxis.X)
			playIfNot("walk_down");
	}

	public void playIfNot(string animation)
	{
		if(playerSprite.Animation != animation)
			playerSprite.Play(animation);
	}
}
