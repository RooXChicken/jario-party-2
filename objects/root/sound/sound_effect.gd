class_name SoundEffect extends Object

var sound_name := "";
var stream: AudioStream;

var volume := 1.0;

func _init(_sound_name: String, _path: String, _volume: float = 1.0):
	sound_name = _sound_name;
	stream = load(_path) as AudioStream;
	
	volume = _volume;
