class_name SoundEffect extends Object

var name := "";
var stream: AudioStream;

var volume := 1.0;

static func init(name: String, path: String, volume: float = 1.0) -> SoundEffect:
	var instance := SoundEffect.new();
	
	instance.name = name;
	instance.stream = load(path) as AudioStream;
	
	instance.volume = volume;
	
	return instance;
