class_name Settings extends Flags

func _init(_path: String):
	super(_path);

func _on_flag_set(path: String, value: Variant) -> void:
	match path:
		"master_volume": master_volume_changed(value as int);
		"music_volume": music_volume_changed(value as int);
		"sound_volume": sound_volume_changed(value as int);
		
		"resolution": resolution_changed(value as int);
		"fullscreen": fullscreen_changed(value as bool);

func _on_init() -> void:
	master_volume_changed(get_flag_or_default("master_volume", 0));
	music_volume_changed(get_flag_or_default("music_volume", 0));
	sound_volume_changed(get_flag_or_default("sound_volume", 0));
	
	resolution_changed(get_flag_or_default("resolution", 0));
	fullscreen_changed(get_flag_or_default("fullscreen", false));

func master_volume_changed(value: int) -> void:
	AudioServer.set_bus_volume_db(0, value);

func music_volume_changed(value: int) -> void:
	AudioServer.set_bus_volume_db(1, value);

func sound_volume_changed(value: int) -> void:
	AudioServer.set_bus_volume_db(2, value);

func resolution_changed(value: int) -> void:
	match value:
		0: GameManager.get_window().size = Vector2i(640, 360);
		1: GameManager.get_window().size = Vector2i(1280, 720);
		2: GameManager.get_window().size = Vector2i(1920, 1080);
		3: GameManager.get_window().size = Vector2i(2560, 1440);
		4: GameManager.get_window().size = Vector2i(3840, 2160);

func fullscreen_changed(value: bool) -> void:
	DisplayServer.window_set_mode(
		DisplayServer.WINDOW_MODE_FULLSCREEN if(value) else DisplayServer.WINDOW_MODE_WINDOWED);
