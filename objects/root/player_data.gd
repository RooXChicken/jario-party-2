class_name PlayerData extends Object

var id := 0;
var ai := false;

var character_id := 0;
var coins := 0;
var stars := 0;

static func load(json: Dictionary) -> PlayerData:
	var id: int = json.get_or_add("id", 0);
	var ai: bool = json.get_or_add("ai", false);
	
	var character_id: int = json.get_or_add("character_id", 0);
	var coins: int = json.get_or_add("coins", 0);
	var stars: int = json.get_or_add("stars", 0);
	
	return init(id, ai, character_id, coins, stars);

static func init(id: int, ai: bool, character_id: int, coins: int = 0, stars: int = 0) -> PlayerData:
	var instance := PlayerData.new();
	
	instance.id = id;
	instance.ai = ai;
	
	instance.character_id = character_id;
	instance.coins = coins;
	instance.stars = stars;
	
	return instance;

static func load_players(json: Array) -> Array[PlayerData]:
	var players: Array[PlayerData];
	var player_count: int = json.size();
	
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
