class_name GameData extends Object

var id := 0;

var turn_total := 0;
var current_turn := 0;

func _init(_id: int, _turn_total: int, _current_turn: int = 0):
	id = _id;
	turn_total = _turn_total;
	current_turn = _current_turn;

static func from_string(data: String) -> GameData:
	return GameData.load(JSON.parse_string(data));

static func load(json: Dictionary) -> GameData:
	var _id: int = json.get_or_add("id", 0);
	var _turn_total: int = json.get_or_add("turn_total", 0);
	var _current_turn: int = json.get_or_add("current_turn", 0);
	
	return GameData.new(_id, _turn_total, _current_turn);

func save() -> Dictionary:
	var data: Dictionary = {
		"id": id,
		"turn_total": turn_total,
		"current_turn": current_turn
	};
	
	return data;

func _to_string() -> String:
	return JSON.stringify(save());
