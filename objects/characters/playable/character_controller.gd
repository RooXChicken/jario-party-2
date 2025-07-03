@tool
class_name CharacterController extends CharacterBody2D

enum Ability {
	ANIMATE,
	MOVE,
	JUMP,
	Y_MOVEMENT
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

@export_category("Abilities")
@export var abilities: Array[Ability];

var y := 0.0;
var y_velocity := 0.0;

var dir := "down";

const jump_height := -6.0;
const gravity_speed := 0.4;
const max_fall_speed := 320.0;

const joy_steps := 8;
const deadzone := 0.1;

const top_speed := 240.0;
const move_min_speed := 0.1;

const acceleration := 40.0;
const jump_acceleration := 15.0;
const deceleration := 35.0;

func _ready() -> void:
	get_character_data();
	set_sprite_frames();
	
	SoundManager.load_sound("character_playable_jump", "res://sounds/characters/playable/player_jump.wav");

func set_sprite_frames() -> void:
	if(sprite == null):
		return;
	
	sprite.sprite_frames = character_data.sprite;

func get_character_data() -> void:
	character_data = Globals.Characters[character];

func get_joy() -> Vector2:
	var raw_input = Vector2(Input.get_axis("left", "right"), Input.get_axis("up", "down"));
	
	var dir := rad_to_deg(atan2(raw_input.y, raw_input.x));
	var length := raw_input.length();
	
	const steps := 360.0/joy_steps;
	
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
	sprite.speed_scale = speed;

func move() -> void:
	if(has_ability(Ability.Y_MOVEMENT)):
		sprite.position.y = y;
	
	move_and_slide();

func has_ability(ability: Ability) -> bool:
	return abilities.has(ability);

#const SPEED = 300.0
#const JUMP_VELOCITY = -400.0
#
#
#func _physics_process(delta: float) -> void:
	## Add the gravity.
	#if not is_on_floor():
		#velocity += get_gravity() * delta
#
	## Handle jump.
	#if Input.is_action_just_pressed("ui_accept") and is_on_floor():
		#velocity.y = JUMP_VELOCITY
#
	## Get the input direction and handle the movement/deceleration.
	## As good practice, you should replace UI actions with custom gameplay actions.
	#var direction := Input.get_axis("ui_left", "ui_right")
	#if direction:
		#velocity.x = direction * SPEED
	#else:
		#velocity.x = move_toward(velocity.x, 0, SPEED)
#
	#move_and_slide()
