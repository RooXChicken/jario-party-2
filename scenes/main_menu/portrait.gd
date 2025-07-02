@tool
extends Node2D

@export var data: String = "null";

@export var background_color: Vector3 = Vector3(0.5, 0.5, 0.5):
	get: return background_color;
	set(value):
		background_color = value;
		set_uniform("color", background_color);

@export var background_scale: float = 1.0:
	get: return background_scale;
	set(value):
		background_scale = value;
		if(has_node("Background")):
			(get_node("Background") as Sprite2D).scale = Vector2(background_scale, background_scale);

@export var scroll_speed: float = 0.1:
	get: return scroll_speed;
	set(value):
		scroll_speed = value;
		set_uniform("speed", scroll_speed);

@export var icon: Texture2D:
	get: return icon;
	set(value):
		icon = value;
		update_texture("Icon", icon);

@export var background: Texture2D:
	get: return background;
	set(value):
		background = value;
		update_texture("Background", background);

@export var border: Texture2D:
	get: return border;
	set(value):
		border = value;
		update_texture("Border", border);

func update_texture(sprite: String, texture: Texture2D) -> void:
	if(!has_node(sprite)):
		return;
	
	(get_node(sprite) as Sprite2D).texture = texture;

func set_uniform(uniform_name: String, value: Variant) -> void:
	if(!has_node("Background")):
		return;
	
	(get_node("Background") as Sprite2D).set_instance_shader_parameter(uniform_name, value);
