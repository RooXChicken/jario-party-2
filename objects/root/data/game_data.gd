class_name GameData extends Object

var id := 0;

var turn_total := 0;
var current_turn := 0;

static func init(id: int, turn_total: int, current_turn: int = 0) -> GameData:
	var instance: GameData = GameData.new();
	
	instance.id = id;
	instance.turn_total = turn_total;
	instance.current_turn = current_turn;
	
	return instance;

static func from_string(data: String) -> GameData:
	return GameData.load(JSON.parse_string(data));

static func load(json: Dictionary) -> GameData:
	var id: int = json.get_or_add("id", 0);
	var turn_total: int = json.get_or_add("turn_total", 0);
	var current_turn: int = json.get_or_add("current_turn", 0);
	
	return init(id, turn_total, current_turn);

func save() -> Dictionary:
	var data: Dictionary = {
		"id": id,
		"turn_total": turn_total,
		"current_turn": current_turn
	};
	
	return data;

func _to_string() -> String:
	return JSON.stringify(save());
