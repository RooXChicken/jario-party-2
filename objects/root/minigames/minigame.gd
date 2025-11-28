@tool
class_name Minigame extends Node2D

enum Mode {
	TIMER,
	LAST_PLAYER,
	TIMED_LAST_PLAYER,
	CUSTOM,
}

@export_category("Minigame Paramaters")
@export var minigame_name := "";
@export var win_type := Mode.LAST_PLAYER;
@export var timer := 60;

@export_category("Player Settings")
@export var abilities: Array[CharacterController.Ability];
@export_tool_button("Add spawn points", "Callable") var spawn_point_button := generate_spawn_points;

func generate_spawn_points() -> void:
	for i in range(0, 4):
		var spawn_name := "SpawnPoint" + str(i);
		
		if($Characters.has_node(spawn_name)):
			continue;
			
		var spawn := Node2D.new();
		spawn.name = spawn_name;
		spawn.position = Vector2((i+1)*10, 0);
		$Characters.add_child(spawn);
		
		spawn.set_owner(self);

func spawn_players() -> void:
	var loaded_character := load("res://objects/characters/playable/character_controller.tscn");
	for i in $Characters.get_child_count():
		var spawn := $Characters.get_child(i);
		
		var character: CharacterController = loaded_character.instantiate();
		character.position = spawn.position;
		character.abilities = abilities;
		character.name = "Character" + str(i);
		
		character.set_player_data(GameManager.players[i]);
		$Characters.add_child(character);
		
