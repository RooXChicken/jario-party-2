class_name CharacterData extends Object

var name: String;
var sprite: SpriteFrames;

var color: Color;

func _init(_name: String, sprite_path: String, _color: Color) -> void:
	name = _name;
	sprite = load(sprite_path);
	
	color = _color;
