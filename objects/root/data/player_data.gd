class_name PlayerData extends Object

var id := 0;
var ai := false;

var character_id := 0;
var coins := 0;
var stars := 0;

static func load(json: Dictionary) -> PlayerData:
	var _id: int = json.get_or_add("id", 0);
	var _ai: bool = json.get_or_add("ai", false);
	
	var _character_id: int = json.get_or_add("character_id", 0);
	var _coins: int = json.get_or_add("coins", 0);
	var _stars: int = json.get_or_add("stars", 0);
	
	return PlayerData.new(_id, _ai, _character_id, _coins, _stars);

func _init(_id: int, _ai: bool, _character_id: int, _coins: int = 0, _stars: int = 0):
	id = _id;
	ai = _ai;
	
	character_id = _character_id;
	coins = _coins;
	stars = _stars;

static func load_players(json: Array) -> Array[PlayerData]:
	var players: Array[PlayerData];
	
	for player in json:
		players.append(PlayerData.load(player));
	
	return players;

func save() -> Dictionary:
	var data: Dictionary = {
		"id": id,
		"ai": ai,
		
		"character_id": character_id,
		"coins": coins,
		"stars": stars
	};
	
	return data;

static func save_all(player_array: Array[PlayerData]) -> Array:
	var players: Array = [];
	
	for player in player_array:
		players.append(player.save());
	
	return players;

func _to_string() -> String:
	return JSON.stringify(save());
