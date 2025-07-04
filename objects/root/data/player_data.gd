class_name PlayerData extends Object

var id := 0;
var ai := false;

var character := Globals.CharacterType.JARIO;
var coins := 0;
var stars := 0;

static func load(json: Dictionary) -> PlayerData:
	var _id: int = json.get_or_add("id", 0);
	var _ai: bool = json.get_or_add("ai", false);
	
	var _character_id: Globals.CharacterType = Globals.CharacterType.get(json.get_or_add("character", Globals.CharacterType.JARIO));
	var _coins: int = json.get_or_add("coins", 0);
	var _stars: int = json.get_or_add("stars", 0);
	
	return PlayerData.new(_id, _ai, _character_id, _coins, _stars);

func _init(_id: int, _ai: bool, _character: Globals.CharacterType, _coins: int = 0, _stars: int = 0):
	id = _id;
	ai = _ai;
	
	character = _character;
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
		
		"character": character,
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
