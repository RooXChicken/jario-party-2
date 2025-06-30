class_name Flags extends Node

var file_path := "flags.txt";

var file: FileAccess;
var flags: Dictionary;

var save_time := 0.0;
var dirty := false;

func get_flag(path: String) -> Variant:
	return flags.get(path);

func set_flag(path: String, value: Variant) -> void:
	dirty = true;
	return flags.set(path, value);

func get_flag_or_default(path: String, default: Variant) -> Variant:
	if(flags.has(path)):
		return flags.get(path);
	else:
		return default;

static func init(path: String) -> Flags:
	var instance := Flags.new();
	
	if(!FileAccess.file_exists(path)):
		var file := FileAccess.open(path, FileAccess.WRITE);
		file.store_string("{}");
		file.close();
	
	instance.file = FileAccess.open(path, FileAccess.READ_WRITE);
	
	instance.file_path = path;
	instance.flags = JSON.parse_string(instance.file.get_as_text());
	
	return instance;

func _process(delta: float) -> void:
	save_time += delta;
	
	if(save_time > 5.0):
		# 'dirty' means our file has been modified
		
		if(dirty):
			save();
			dirty = false;
		
		save_time = 0.0;

func save() -> void:
	file.store_string(JSON.stringify(flags));
	file.flush();
