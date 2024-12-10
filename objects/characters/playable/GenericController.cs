using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;

class Ability
{
	public int id { get; private set; }
	public string name { get; private set; }

	private Ability(int _id, string _name) { id = _id; name = _name; }
	
	//component types (no manual creation)
	public static Ability ANIMATE = new Ability(0, "animate");
	public static Ability WALK = new Ability(1, "walk");
	public static Ability JUMP = new Ability(2, "jump");
	public static Ability Y_MOVEMENT = new Ability(3, "y_movement");
}

public partial class GenericController : CharacterBody2D
{
	/*
		Generic Character Controller
		Used for any minigame where you move around.

		Special functions are split into functions with checks for if the
		player has a certain ability.
		
		Abilities can be anything, and allow for the generic re-use of code (yippie!!)
		Abilities can only be manually defined to prevent random abilities from existing
	*/

	private List<Ability> abilities;

	private AnimatedSprite2D playerSprite;
	private RichTextLabel label;

	//movement related variables
	private float joystickDeadzone = 0.2f;
	
	private float acceleration = 0;
	private float decelleration = 0.6f;

	private float walkSpeed = 90;
	private float runSpeed = 140;

	//physics ticks without movement counter (60tps)
	private int ticksWithoutMovement = 0;
	private int longIdleTime = 600; //10 seconds (60tps * 10)

	//'fake y' related variables
	private float y = 0;
	private float oldY = 0;
	private float yVelocity = 0;
	private float gravitySpeed = 0.25f;

	private Vector2 velocity = new Vector2(0, 0);
	private Vector2 joyAxis = new Vector2(0, 0);

	public override void _Ready()
	{
		//create list of abilities
		abilities = new List<Ability>();

		//TODO: based off minigames
		abilities.Add(Ability.ANIMATE);
		abilities.Add(Ability.WALK);
		abilities.Add(Ability.JUMP);
		abilities.Add(Ability.Y_MOVEMENT);

		//assign variables
		playerSprite = GetNode<AnimatedSprite2D>("CharacterSprite");
		label = GetNode<RichTextLabel>("CharacterSprite/ColorRect/RichTextLabel");

		acceleration = walkSpeed;
	}

	public override void _Process(double delta)
	{
		//process framerate-independent actions
		processInput();
		processAnimations();

		//'fake y' related interactions
		if(hasAbility("y_movement"))
		{
			if(Input.IsActionJustPressed("select") && y == 0)
				yVelocity = -5f;

			//interpolate oldY and y to get framerate-independant y
			oldY = Mathf.Lerp(oldY, y, ((float)delta*10));
			playerSprite.Position = new Vector2(0, oldY);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		//process moving
		processMovement();
		processYMovement();

		//fancy debug text
		label.Text = string.Format("Position: ({0:0.##}, {1:0.##}, {2:0.##})\nSpeed: ({3:0.##}, {4:0.##}, {5:0.##})", Position.X, y, Position.Y, velocity.X, yVelocity, velocity.Y);
	}

	private void processInput()
	{
		if(!hasAbility("walk"))
			return;

		//get joystick input
		joyAxis = new Vector2(Input.GetJoyAxis(0, JoyAxis.LeftX), Input.GetJoyAxis(0, JoyAxis.LeftY));

		//forced deadzone (evil mode)
		if(Math.Abs(joyAxis.X) < joystickDeadzone) joyAxis.X = 0;
		if(Math.Abs(joyAxis.Y) < joystickDeadzone) joyAxis.Y = 0;

		//to run or not to run
		if(Input.IsActionPressed("menu"))
			acceleration = runSpeed;
		else
			acceleration = walkSpeed;
	}

	private void processMovement()
	{
		if(!hasAbility("walk"))
			return;

		//increment ticks without moving, if the joystick is neutral
		if(joyAxis.X == 0 && joyAxis.Y == 0)
			ticksWithoutMovement++;
		else
			ticksWithoutMovement = 0;

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
	}

	private void processYMovement()
	{
		if(!hasAbility("y_movement"))
			return;

		//for lerp
		oldY = y;

		//gravity
		yVelocity += gravitySpeed;

		y += yVelocity;

		//if the player is on or below the floor, stop their movement and snap their y to the floor
		if(y > 0)
		{
			y = 0;
			yVelocity = 0;
		}
	}

	public void processAnimations()
	{
		if(!hasAbility("animate"))
			return;

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

			//set speed based on velocity then play respective animaation
			playerSprite.SpeedScale = (Math.Abs(velocity.X) + Math.Abs(velocity.Y)) / walkSpeed;
			playIfNot(toPlay);
		}
		else if(ticksWithoutMovement < longIdleTime)
			playerSprite.Frame = 1;

		if(ticksWithoutMovement >= longIdleTime)
		{
			playerSprite.SpeedScale = 1;
			playIfNot("long_idle");
		}
	}

	public void playIfNot(string animation)
	{
		if(playerSprite.Animation != animation)
			playerSprite.Play(animation);
	}

	public bool hasAbility(int id)
	{
		foreach(Ability ability in abilities)
			if(ability.id == id) return true;

		return false;
	}

	public bool hasAbility(string name)
	{
		foreach(Ability ability in abilities)
			if(ability.name == name) return true;

		return false;
	}
}
