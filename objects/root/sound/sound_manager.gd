extends Node

const sound_play_not_loaded := "Failed to play sound {sound_name}! This sound is not loaded!";

var sounds: Dictionary[String, SoundEffect] = {};

func load_sound(sound_name: String, path: String, volume: float = 1.0) -> void:
	if(sounds.has(sound_name)):
		return;
	
	sounds[sound_name] = SoundEffect.new(sound_name, path, volume);

func unload_sound(sound_name: String) -> void:
	if(!sounds.has(sound_name)):
		return;
	
	sounds.erase(sound_name);

func play_sound(sound_name: String) -> void:
	if(!sounds.has(sound_name)):
		printerr(sound_play_not_loaded.format({ "sound_name": sound_name }));
		return;
	
	var stream := AudioStreamPlayer2D.new();
	
	stream.bus = "Sound";
	stream.stream = sounds[sound_name].stream;
	stream.volume_db = sounds[sound_name].volume;
	stream.name = sound_name;
	
	stream.finished.connect(sound_stopped.bind(stream));
	
	add_child(stream);
	stream.play();

func sound_stopped(stream: AudioStreamPlayer2D) -> void:
	stream.queue_free();
