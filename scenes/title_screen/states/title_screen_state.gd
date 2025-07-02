extends State

@export var anim: AnimationPlayer;
const main_menu_scene := "res://scenes/main_menu/main_menu.tscn";

func ready() -> void:
	ResourceLoader.load_threaded_request(main_menu_scene);

func enter(_previous_node: String, data: Dictionary = { "skipped": false }):
	if(data.get_or_add("skipped", false) as bool):
		anim.seek(2);

func update(_delta: float) -> void:
	if(Input.is_action_just_pressed("select")):
		load_game();

func load_game() -> void:
	var scene := (ResourceLoader.load_threaded_get(main_menu_scene) as PackedScene).instantiate();
	
	var screenshot := ImageTexture.create_from_image(
		get_viewport().get_texture().get_image());
	
	var transition_group: CanvasGroup = scene.get_node("TransitionGroup");
	transition_group.visible = true;
	transition_group.get_node("PreviousScreenSprite").texture = screenshot;
	
	owner.get_parent().add_child(scene);
	owner.queue_free();
