using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Schema;

class Ability
{
	/*
		Stores information on each of the 'abilities'
		Abilities are any action that can be defined
		Think of this as a poor person's component system (single class so component system is unnecessary :P)

		Are pre-defined, so there can't be a random ability with invalid data
	*/

	public int id { get; private set; }
	public string name { get; private set; }

	private Ability(int _id, string _name) { id = _id; name = _name; }
	
	//ability types (no manual creation)
	public static Ability ANIMATE = new Ability(0, "animate");
	public static Ability WALK = new Ability(1, "walk");
	public static Ability JUMP = new Ability(2, "jump");
	public static Ability Y_MOVEMENT = new Ability(3, "y_movement");
	public static Ability LONG_IDLE = new Ability(4, "long_idle");
}

public class Character
{
	/*
		Stores information on each of the characters
		Any amount of character-specific variables can be stored here and loaded easily!

		Are pre-defined, so there can't be a random Jaroi with invalid data
	*/

	public int id { get; private set; }
	public string name { get; private set; }
	public SpriteFrames spriteFrames { get; private set; }

	private Character(int _id, string _name, string _spriteFramesPath) { id = _id; name = _name; spriteFrames = GD.Load<SpriteFrames>(_spriteFramesPath); }
	
	//character types (no manual creation)
	public static Character JARIO = new Character(1, "Jario", "res://assets/sprites/characters/playable/jario_sprite_frames.tres");
	public static Character WOOIGI = new Character(2, "Wooigi", "res://assets/sprites/characters/playable/wooigi_sprite_frames.tres");
	public static Character GRAPEJUICE = new Character(3, "Grapejuice", "res://assets/sprites/characters/playable/grapejuice_sprite_frames.tres");
	public static Character JOSH = new Character(4, "Josh", "res://assets/sprites/characters/playable/josh_sprite_frames.tres");

	//used for index -> character
	public static Character[] characters = new Character[] { JARIO, WOOIGI, GRAPEJUICE, JOSH };
}

public class JoystickButton
{
	/*
		Used only for AI
		Simulates joystick buttons
	*/

	public string action { get; private set; } //corresponds to GD action name (to keep consistency)
	public int duration = -2; //used to set how long the button is 'held'
	public bool justPressed { get; private set; } = true;

	public JoystickButton(string _action, int _duration) { action = _action; duration = _duration; }
	public void tick() { justPressed = false; duration--; }
}

//marked as tool so that the sprites can update in realtime
[Tool]
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

	//hiden 'real' character index
	private int _characterIndex = 0;

	//make character index visible to editor
	[ExportCategory("Character Data")]
	[Export(PropertyHint.Range, "0,3,")]
	private int characterIndex { get { return _characterIndex; } set { _characterIndex = value; updateCharacter(); } }

	[Export]
	private bool ai = false;

	//if dense, the character is a dummy, and has no logic
	[Export]
	private bool dense = false;

	//registered character
	public Character character { get; private set; }
	private List<Ability> abilities;

	private AnimatedSprite2D playerSprite;
	private RichTextLabel label;

	//movement related variables
	private float joystickDeadzone = 0.08f;
	
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

	private string direction = "down";
	private string action = "walk";

	private List<JoystickButton> joystickButtons;

	public override void _Ready()
	{
		if(Engine.IsEditorHint())
			return;

		//assign variables
		playerSprite = GetNode<AnimatedSprite2D>("CharacterSprite");
		label = GetNode<RichTextLabel>("CharacterSprite/ColorRect/RichTextLabel");

		//set character (manually for now)
		character = Character.characters[characterIndex];
		playerSprite.SpriteFrames = character.spriteFrames;

		//load sounds
		SoundManager.loadSound("character_playable_jump", "res://assets/sound/effects/characters/playable/player_jump.wav", 12);

		//create list of abilities
		abilities = new List<Ability>();

		//TODO: based off minigames
		abilities.Add(Ability.ANIMATE);
		abilities.Add(Ability.WALK);
		abilities.Add(Ability.JUMP);
		abilities.Add(Ability.Y_MOVEMENT);
		abilities.Add(Ability.LONG_IDLE);

		acceleration = walkSpeed;

		joystickButtons = new List<JoystickButton>();
	}

	public override void _Process(double delta)
	{
		if(Engine.IsEditorHint() || dense)
			return;

		//process framerate-independent actions
		processInput();
		processAnimations();

		if(hasAbility("y_movement"))
		{
			//interpolate oldY and y to get framerate-independant y
			oldY = Mathf.Lerp(oldY, y, ((float)delta*10));
			playerSprite.Position = new Vector2(0, oldY);

			if(yVelocity != 0)
				ticksWithoutMovement = 0;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if(Engine.IsEditorHint() || dense)
			return;

		//process moving
		processMovement();
		processYMovement();

		if(ai)
		{
			for(int i = 0; i < joystickButtons.Count - 1; i++)
			{
				joystickButtons[i].tick();

				//removes at -1 to allow for justReleased
				if(joystickButtons[i].duration < -1)
					joystickButtons.RemoveAt(i);
			}
		}

		//fancy debug text
		label.Text = string.Format("Position: ({0:0.##}, {1:0.##}, {2:0.##})\nSpeed: ({3:0.##}, {4:0.##}, {5:0.##})", Position.X, y, Position.Y, velocity.X, yVelocity, velocity.Y);
	}

	private void processInput()
	{
		if(!hasAbility("walk"))
			return;

		if(!ai)
		{
			//get joystick input
			Vector2 rawInput = Input.GetVector("left", "right", "up", "down");

			//forced deadzone (evil mode)
			if(Math.Abs(rawInput.X) < joystickDeadzone) rawInput.X = 0;
			if(Math.Abs(rawInput.Y) < joystickDeadzone) rawInput.Y = 0;

			float combinedDirection = MathF.Abs(rawInput.X) + Mathf.Abs(rawInput.Y);
			joyAxis = rawInput / ((combinedDirection != 0) ? combinedDirection : 1);
		}
		//if ai, write to joyAxis manually :P

		if(ai)
			simulateAction("jump", 1);

		//to run or not to run
		if(getActionState("menu") >= 0)
			acceleration = runSpeed;
		else
			acceleration = walkSpeed;

		//'fake y' related interactions
		if(hasAbility("y_movement"))
		{
			if(getActionState("jump") == 1 && y == 0)
			{
				SoundManager.playSound("character_playable_jump");
				yVelocity = -5f;
			}
		}
	}

	private void processMovement()
	{
		if(!hasAbility("walk"))
			return;

		if(hasAbility("long_idle"))
		{
			//increment ticks without moving, if the joystick is neutral
			if(joyAxis.X == 0 && joyAxis.Y == 0)
				ticksWithoutMovement++;
			else
				ticksWithoutMovement = 0;
		}

		//increase velocity based on joy angle and acceleration
		velocity += joyAxis * acceleration;

		//decelerate character
		velocity *= decelleration;

		float combinedSpeed = Math.Abs(velocity.X) + Math.Abs(velocity.Y);

		//if the velocity is 'basically' zero, set it to zero
		if(combinedSpeed < 0.1)
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

		//hitbox 'height'
		if(y < -32)
		{
			CollisionLayer = 2;
			CollisionMask = 2;
		}
		else
		{
			CollisionLayer = 1;
			CollisionMask = 1;
		}
	}

	public void processAnimations()
	{
		if(!hasAbility("animate"))
			return;

		float combinedSpeed = Math.Abs(velocity.X) + Math.Abs(velocity.Y);
		if(combinedSpeed != 0)
		{
			if(velocity.X > velocity.Y && velocity.X > -velocity.Y)
			{
				direction = "left";
				playerSprite.FlipH = true;
			}
			else if(-velocity.X > velocity.Y && -velocity.X > -velocity.Y)
			{
				direction = "left";
				playerSprite.FlipH = false;
			}
			else if(-velocity.Y > velocity.X && -velocity.Y > -velocity.X)
			{
				direction = "up";
				playerSprite.FlipH = false;
			}
			else if(velocity.Y > velocity.X && velocity.Y > -velocity.X)
			{
				direction = "down";
				playerSprite.FlipH = false;
			}
		}

		if(yVelocity == 0 && y == 0)
		{
			if(velocity != Vector2.Zero)
			{
				//set speed based on velocity then play respective animaation
				playerSprite.SpeedScale = (Math.Abs(velocity.X) + Math.Abs(velocity.Y)) / walkSpeed;
				action = "walk_" + direction;
			}
			else if(ticksWithoutMovement < longIdleTime)
				playerSprite.Frame = 0;

			action = "walk_" + direction;
		}
		else
		{
			playerSprite.SpeedScale = 1;
			action = "jump_" + direction;
		}

		//play long idle if the timer is above the threshold
		if(ticksWithoutMovement >= longIdleTime)
		{
			playerSprite.SpeedScale = 1;
			action = "long_idle";
		}

		playIfNot(action);
	}

	public void playIfNot(string animation)
	{
		if(playerSprite.Animation != animation)
		{
			playerSprite.Play(animation);
			playerSprite.Frame = 0;
		}
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

	private void updateCharacter()
	{
		//if the node hasn't been initialized yet, make sure godot isn't angry
		if(!HasNode("CharacterSprite"))
			return;

		//update character sprite frames to new ones
		playerSprite = GetNode<AnimatedSprite2D>("CharacterSprite");

		character = Character.characters[characterIndex];
		playerSprite.SpriteFrames = character.spriteFrames;
	}

	public void simulateAction(string action, int duration)
	{
		//override current action if it already exists
		foreach(JoystickButton button in joystickButtons)
			if(button.action == action)
			{
				button.duration = duration;
				return;
			}

		joystickButtons.Add(new JoystickButton(action, duration));
	}

	/*

		Used for allowing the same code to work for AIs and players

		****
		* Steps to puppet an AI:

		* Write to joyAxis as a vector between representing the direction of the stick
		-1 <- 0 -> 1

		* Call simulateAction with the action name, and it's duration
		****

		1 is just pressed
		0 is pressed
		-1 is just released

		Use >= 0 to detect if it is pressed in general
	*/

	public int getActionState(string action)
	{
		if(!ai)
		{
			if(Input.IsActionJustPressed(action)) return 1;
			if(Input.IsActionPressed(action)) return 0;
			if(Input.IsActionJustReleased(action)) return -1;

			return -2;
		}
		else
		{
			foreach(JoystickButton button in joystickButtons)
			{
				if(button.action != action) continue;

				if(button.justPressed) return 1;
				if(button.duration > 0) return 0;
				if(button.duration == -1) return -1;

			}
		}

		return -2;
	}
}
