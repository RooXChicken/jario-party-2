@tool
class_name CharacterController extends RigidBody2D

enum Ability {
	ANIMATE,
	MOVE,
	JUMP,
	Y_MOVEMENT,
	DEBUG
}

@export_category("References")
@export var sprite: AnimatedSprite2D;
@export var debug_label: RichTextLabel;

@export_category("Character Data")
@export var character := Globals.CharacterType.JARIO:
	get(): return character;
	set(value):
		character = value;
		
		get_character_data();
		set_sprite_frames();

var character_data: CharacterData;
@export var ai := false;
@export var dense := false;
@export var controller_index := 0;

@export_category("Abilities")
@export var abilities: Array[Ability];

var velocity := Vector2.ZERO;
var old_position := Vector2.ZERO;

var y := 0.0;
var y_velocity := 0.0;

var dir := "down";
var jump_sound := "character_playable_jump";

const jump_height := -6.0;
const gravity_speed := 0.4;
const max_fall_speed := 320.0;

const joy_steps := 8;
const steps := 360.0/joy_steps;
const deadzone := 0.1;

const top_speed := 180.0;
const move_min_speed := 0.1;

const acceleration := 60.0;
const deceleration := 80.0;

const jump_acceleration := 20.0;
const jump_deceleration := 4.0;

const push_force := 0.68;

func _ready() -> void:
	get_character_data();
	set_sprite_frames();
	
	if(!Engine.is_editor_hint()):
		if(has_ability(Ability.DEBUG)):
			debug_label.get_parent().visible = true;
		
		if(dense):
			$StateMachine.set_state("Dense");
		
		set_anim("walk_down");
		sprite.speed_scale = 0.0;
		
		SoundManager.load_sound("character_playable_jump", "res://sounds/characters/playable/player_jump.wav", 6);
		SoundManager.load_sound("character_playable_jump_GIRLY", "res://sounds/characters/playable/player_jump_GIRLY.wav", 6);
		
		if(character == Globals.CharacterType.GRAPEJUICE):
			jump_sound = "character_playable_jump_GIRLY";

func set_sprite_frames() -> void:
	if(sprite == null):
		return;
	
	sprite.sprite_frames = character_data.sprite;

func get_character_data() -> void:
	character_data = Globals.Characters[character];

func get_joy() -> Vector2:
	var raw_input = Vector2(Input.get_joy_axis(controller_index, JOY_AXIS_LEFT_X), Input.get_joy_axis(controller_index, JOY_AXIS_LEFT_Y));
	if(abs(raw_input.x) < deadzone):
		raw_input.x = 0;
	if(abs(raw_input.y) < deadzone):
		raw_input.y = 0;
	
	var dir := rad_to_deg(atan2(raw_input.y, raw_input.x));
	var length := raw_input.length();

	dir = deg_to_rad(round(dir/steps) * steps);
	
	return Vector2(cos(dir), sin(dir)) * length;

func set_anim(anim: String) -> void:
	if(sprite.animation != anim):
		sprite.play(anim);

func animate(state: String, joy_axis: Vector2, speed: float):
	if(!has_ability(Ability.ANIMATE)):
		return;
	
	if(joy_axis.length() > 0.04):
		if(joy_axis.x >= joy_axis.y && joy_axis.x >= -joy_axis.y):
			dir = "left";
			sprite.flip_h = true;
		elif(-joy_axis.x >= joy_axis.y && -joy_axis.x >= -joy_axis.y):
			dir = "left";
			sprite.flip_h = false;
			
		elif(-joy_axis.y >= joy_axis.x && -joy_axis.y >= -joy_axis.x):
			dir = "up";
			sprite.flip_h = false;
		elif(joy_axis.y >= joy_axis.x && joy_axis.y >= -joy_axis.x):
			dir = "down";
			sprite.flip_h = false;
		else:
			pass;
	
	set_anim(state + "_" + dir);
	sprite.speed_scale = speed * 1.3;

func move() -> void:
	if(has_ability(Ability.Y_MOVEMENT)):
		sprite.position.y = y;
		
		if(y < -32):
			collision_layer = 2;
			collision_mask = 2;
		else:
			collision_layer = 1;
			collision_mask = 1;
	
	apply_central_force(velocity * 60);
	
	var speed := position - old_position;
	old_position = Vector2(position);
	
	if(has_ability(Ability.DEBUG)):
		debug_label.text = "Speed: {speed}".format({ "speed": (speed*60) });

func has_ability(ability: Ability) -> bool:
	return abilities.has(ability);
