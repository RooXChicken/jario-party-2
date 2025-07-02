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
	
	flags.set(path, value);
	_on_flag_set(path, value);

func get_flag_or_default(path: String, default: Variant) -> Variant:
	if(flags.has(path)):
		return flags.get(path);
	else:
		return default;

func _on_flag_set(_path: String, _value: Variant) -> void:
	pass;

func _init(_path: String):
	if(!FileAccess.file_exists(_path)):
		var temp_file := FileAccess.open(_path, FileAccess.WRITE);
		
		temp_file.store_string("{}");
		temp_file.close();
	
	file_path = _path;
	
	file = FileAccess.open(file_path, FileAccess.READ);
	flags = JSON.parse_string(file.get_as_text());
	
	file.close();
	_on_init();

func _on_init() -> void:
	pass;

func _process(delta: float) -> void:
	save_time += delta;
	
	if(save_time > 5.0):
		# 'dirty' means our file has been modified
		
		if(dirty):
			save();
			dirty = false;
		
		save_time = 0.0;

func save() -> void:
	file = FileAccess.open(file_path, FileAccess.WRITE);
	
	file.store_string(JSON.stringify(flags));
	file.close();

func _exit_tree() -> void:
	save();
