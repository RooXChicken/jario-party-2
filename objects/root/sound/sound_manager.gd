extends Node

const sound_play_not_loaded := "Failed to play sound {name}! This sound is not loaded!";

var sounds: Dictionary[String, SoundEffect] = {};

func load_sound(name: String, path: String, volume: float = 1.0) -> void:
	if(sounds.has(name)):
		return;
	
	sounds[name] = SoundEffect.init(name, path, volume);

func unload_sound(name: String) -> void:
	if(!sounds.has(name)):
		return;
	
	sounds.erase(name);

func play_sound(name: String) -> void:
	if(!sounds.has(name)):
		printerr(sound_play_not_loaded.format({ "name": name }));
		return;
	
	var stream := AudioStreamPlayer2D.new();
	
	stream.bus = "Sound";
	stream.stream = sounds[name].stream;
	stream.volume_db = sounds[name].volume;
	stream.name = name;
	
	stream.finished.connect(sound_stopped.bind(stream));
	
	add_child(stream);
	stream.play();

func sound_stopped(stream: AudioStreamPlayer2D) -> void:
	stream.queue_free();
