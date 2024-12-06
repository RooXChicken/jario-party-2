using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class GenericController : CharacterBody2D
{
	private AnimatedSprite2D playerSprite;
	private float moveSpeed = 200;

	public override void _Ready()
	{
		playerSprite = GetNode<AnimatedSprite2D>("CharacterSprite");
	}

	// public override void _Process(double delta)
	// {

	// }

	public override void _PhysicsProcess(double delta)
	{
		Velocity = new Vector2(Input.GetJoyAxis(0, JoyAxis.LeftX), Input.GetJoyAxis(0, JoyAxis.LeftY)) * moveSpeed;
		MoveAndSlide();


		float distanceMoved = Math.Abs(Velocity.X) + Math.Abs(Velocity.Y);

		if(Velocity.Y > moveSpeed/10)
			playIfNot("walk_down");

		playerSprite.SpeedScale = distanceMoved/moveSpeed;

		if(distanceMoved < moveSpeed/10)
			playIfNot("idle");
	}

	public void playIfNot(string animation)
	{
		if(playerSprite.Animation != animation)
			playerSprite.Play(animation);
	}
}
