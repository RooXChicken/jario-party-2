class_name State extends Node

var state_machine: StateMachine;

func _ready() -> void:
	await get_parent().ready;
	
	state_machine = get_parent();
	ready();

func ready() -> void:
	pass;

func enter(old_state: String, data: Dictionary = {}) -> void:
	pass;

func exit(new_state: String, data: Dictionary = {}) -> void:
	pass;

func update(delta: float) -> void:
	pass;

func phys_update(delta: float) -> void:
	pass;
