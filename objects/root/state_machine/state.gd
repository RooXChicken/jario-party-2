class_name State extends Node

var state_machine: StateMachine;

func _ready() -> void:
	await get_parent().ready;
	
	state_machine = get_parent();
	ready();

func ready() -> void:
	pass;

func enter(_old_state: String, _data: Dictionary = {}) -> void:
	pass;

func exit(_new_state: String, _data: Dictionary = {}) -> void:
	pass;

func update(_delta: float) -> void:
	pass;

func phys_update(_delta: float) -> void:
	pass;
