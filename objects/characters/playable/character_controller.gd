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
@export var abilities: Array[Ability]

const deadzone := 0.1;

func _ready() -> void:
	get_character_data();
	set_sprite_frames();
	
	SoundManager.load_sound("character_playable_jump", "res://sounds/character/playable/player_jump.wav");

func set_sprite_frames() -> void:
	if(sprite == null):
		return;
	
	sprite.sprite_frames = character_data.sprite;

func get_character_data() -> void:
	character_data = Globals.Characters[character];

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
