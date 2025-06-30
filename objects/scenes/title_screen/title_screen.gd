extends Node2D

const main_menu_scene := "res://objects/scenes/main_menu/main_menu.tscn";

var anim: AnimationPlayer;
var bgm: AudioStreamPlayer2D;

var state := 0;

func _ready() -> void:
	anim = get_node("AnimationPlayer");
	bgm = get_node("TitleTheme");
	
	# preload the next scene in the background
	ResourceLoader.load_threaded_request(main_menu_scene);

func _process(_delta: float) -> void:
	if(Input.is_action_just_pressed("select")):
		match(state):
			0:  state_0();

func state_0() -> void:
	if(anim.current_animation == "logo_shine_loop" || anim.current_animation_position > 2):
		if(!(GameManager.flags.get_flag_or_default("has_played", false) as bool)):
			load_game();
		else:
			state = 1;
			anim.seek(7.5, true);
			anim.play("mode_select");
	else:
		# skip to title
		anim.seek(2);

func load_game() -> void:
	var scene := (ResourceLoader.load_threaded_get(main_menu_scene) as PackedScene).instantiate();
	
	var screenshot := ImageTexture.create_from_image(
		get_viewport().get_texture().get_image());
	
	scene.get_node("TransitionGroup/PreviousScreenSprite").texture = screenshot;
	
	get_parent().add_child(scene);
	queue_free();

func play_looping_shine() -> void:
	if(state == 0):
		anim.play("logo_shine_loop");
