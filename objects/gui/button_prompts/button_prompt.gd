@tool
extends Node2D

@export var label: Label;

@export var text: String = "":
	get:
		return text;
	
	set(value):
		text = value;
		update_text();

func _ready() -> void:
	update_text();
	
func update_text() -> void:
	# prevent godot from losing its mind as a tool
	if(label == null):
		return;
		
	for node in get_children():
		if(node is AnimatedSprite2D):
			node.queue_free();
	
	var buttons: Array[PromptData] = [];
	var final_msg := "";
	
	var i := 0;
	while(i < text.length()):
		var c := text[i];
		i += 1;

		if(c != "%"):
			final_msg += c;
			continue;
			
		if(i >= text.length()):
			continue;
		
		var data := PromptData.new(i, text[i]);
		buttons.append(data);
		
		final_msg += "    ";
		i += 1;
	
	label.text = final_msg;
	label.size = Vector2(label.text.length() * 35, label.size.y);
	
	var count: int = 0;
	for data in buttons:
		var x = 0;
		for c in range(0, data.pos):
			var rect := label.get_character_bounds(c);
			x += rect.size.x;
		
		var button := AnimatedSprite2D.new();
		button.sprite_frames = load(
			"res://sprites/gui/button_prompts/sprite_frames/button_" + data.character + ".tres");
		
		button.position = Vector2(x + 6 + (count * (4*5.125)), 18);
		button.play();
		add_child(button);
		
		count += 1;
