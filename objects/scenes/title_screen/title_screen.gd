extends Node2D

const main_menu_scene: String = "res://objects/scenes/main_menu/main_menu.tscn";

var anim: AnimationPlayer;
var bgm: AudioStreamPlayer2D;

func _ready() -> void:
	anim = get_node("AnimationPlayer");
	bgm = get_node("TitleTheme");
	
	# preload the next scene in the background
	ResourceLoader.load_threaded_request(main_menu_scene);

func _process(_delta: float) -> void:
	if(Input.is_action_just_pressed("select") && 
	(anim.current_animation == "logo_shine_loop" || anim.current_animation_position > 2)):
		var scene: Node = (ResourceLoader.load_threaded_get(main_menu_scene) as PackedScene).instantiate();
		
		var screenshot: ImageTexture = ImageTexture.create_from_image(
			get_viewport().get_texture().get_image());
		
		scene.get_node("TransitionGroup/PreviousScreenSprite").texture = screenshot;
		
		get_parent().add_child(scene);
		queue_free();
	
	# skip to title
	if(Input.is_action_just_pressed("select") && 
	(anim.current_animation != "logo_shine_loop" && anim.current_animation_position < 2)):
		anim.seek(2);

func play_looping_shine() -> void:
	anim.play("logo_shine_loop");
