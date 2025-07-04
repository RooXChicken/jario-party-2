extends Node

var options_menu: Node;
var settings: Settings;

var game_data := GameData.new(0, 0, 0);
var players: Array[PlayerData];

var flags: Flags;

func _enter_tree() -> void:
	flags = Flags.new("flags.txt");
	settings = Settings.new("user://options.cfg");
	
	add_child(flags);
	add_child(settings);
	
	for i in range(0, 4):
		players.append(PlayerData.new(i, i != 0, i, 0, 0));
	
	options_menu = (ResourceLoader.load("res://objects/gui/options/options_menu.tscn") as PackedScene).instantiate();
	add_child(options_menu);

func _process(_delta: float) -> void:
	if(Input.is_action_just_pressed("options")):
		var layer: CanvasLayer = options_menu.get_node("Layer");
		layer.visible = !layer.visible;

func load_game(path: String) -> void:
	if(!FileAccess.file_exists(path)):
		printerr("The save file at " + path + " does not exist!");
		return;
	
	var file := FileAccess.open(path, FileAccess.READ);
	var content: Dictionary = JSON.parse_string(file.get_as_text());
	
	game_data = GameData.load(content["game_data"]);
	players = PlayerData.load_players(content["players"]);

func save_game(path: String) -> void:
	var file := FileAccess.open(path, FileAccess.WRITE);
	
	var content: Dictionary[String, Variant] = {
		"game_data": game_data.save(),
		"players": PlayerData.save_all(players)
	};
	
	file.store_string(JSON.stringify(content));
