class_name SettingsManager extends Object

var master_volume: int = 0:
	get: return master_volume;
	set(value):
		master_volume = value;
		AudioServer.set_bus_volume_db(0, master_volume);

var music_volume: int = 0:
	get: return music_volume;
	set(value):
		music_volume = value;
		AudioServer.set_bus_volume_db(1, music_volume);

var sound_volume: int = 0:
	get: return sound_volume;
	set(value):
		sound_volume = value;
		AudioServer.set_bus_volume_db(2, sound_volume);

var resolution: int = 1:
	get: return resolution;
	set(value):
		resolution = value;
		
		match resolution:
			0: GameManager.get_window().size = Vector2i(640, 360);
			1: GameManager.get_window().size = Vector2i(1280, 720);
			2: GameManager.get_window().size = Vector2i(1920, 1080);
			3: GameManager.get_window().size = Vector2i(2560, 1440);
			4: GameManager.get_window().size = Vector2i(3840, 2160);

var fullscreen: bool = false:
	get: return fullscreen;
	set(value):
		DisplayServer.window_set_mode(
			DisplayServer.WINDOW_MODE_FULLSCREEN if(fullscreen) else DisplayServer.WINDOW_MODE_WINDOWED);
		
		resolution = resolution;

func load_settings(path: String = "user://options.cfg") -> void:
	if(!FileAccess.file_exists(path)):
		print("The options file at " + path + " does not exist! Creating default.");
		save_settings(path);
		
	var file := FileAccess.open(path, FileAccess.READ);
	var data: Dictionary = JSON.parse_string(file.get_as_text());
	
	master_volume = data.get_or_add("master_volume", 0);
	music_volume = data.get_or_add("music_volume", 0);
	sound_volume = data.get_or_add("sound_volume", 0);
	
	resolution = data.get_or_add("resolution", 1);
	fullscreen = data.get_or_add("fullscreen", false);

func save_settings(path: String = "user://options.cfg") -> void:
	var file := FileAccess.open(path, FileAccess.WRITE);
	var data: Dictionary = {
		"master_volume": master_volume,
		"music_volume": music_volume,
		"sound_volume": sound_volume,
		
		"resolution": resolution,
		"fullscreen": fullscreen
	};
	
	file.store_string(JSON.stringify(data));
