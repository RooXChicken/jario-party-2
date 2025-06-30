extends State

func enter(old_state: String, data: Dictionary = {}) -> void:
	print("entering " + name);

func exit(new_state: String, data: Dictionary = {}) -> void:
	data["test"] = "this is a test";
	print("exiting " + name + " to " + new_state);

func update(delta: float) -> void:
	if(Input.is_action_just_pressed("left")):
		SoundManager.load_sound("test", "res://sounds/characters/playable/player_jump.wav");
	
	if(Input.is_action_just_pressed("select")):
		SoundManager.play_sound("test");
	
	if(Input.is_action_just_pressed("back")):
		SoundManager.unload_sound("test");
