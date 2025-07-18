class_name StateMachine extends Node

enum TickNextState {
	NONE,
	UPDATE,
	PHYS_UPDATE
}

@export var initial_state_name := "null";
var state: State;

const initial_state_error := "Failed to set initial state to '{initial_state}'! State does not exist!";
const state_not_found := "Failed to find any state!";
const node_not_state := "Node '{node_name}' not a state!";
const set_state_error := "Failed to switch states to '{new_state}' from '{old_state}'! State '{new_state}' does not exist! Attempting to load first node.";

func _ready() -> void:
	state = get_initial_state();
	state.enter("");

func get_initial_state() -> State:
	if(check_state_name_valid(initial_state_name)):
		return get_node(initial_state_name);
	
	printerr(initial_state_error.format({ "initial_state": initial_state_name }));
	
	var initial_state: State = get_child(0);
	if(check_state_valid(initial_state)):
		return initial_state;
	
	printerr(state_not_found);
	return null;

func check_state_name_valid(state_name: String) -> bool:
	if(!has_node(state_name)):
		return false;
	
	return check_state_valid(get_node(state_name));

func check_state_valid(_state: State) -> bool:
	return (_state != null && _state is State);

func set_state(new_state_name: String, data: Dictionary = {}, tick := TickNextState.NONE, delta := 0.0) -> void:
	var old_state_name := state.name;
	
	if(!has_node(new_state_name)):
		printerr(set_state_error.format({ "new_state": new_state_name, "old_state": old_state_name }));
		return;
		
	var new_state: State = get_node(new_state_name);
	
	if(!check_state_valid(new_state)):
		printerr(node_not_state.format({ "node_name": new_state_name }));
		return;
	
	state.exit(new_state_name, data);
	state = new_state;
	
	state.enter(old_state_name, data);
	
	match tick:
		TickNextState.NONE: pass;
		TickNextState.UPDATE: state.update(delta);
		TickNextState.PHYS_UPDATE: state.phys_update(delta);

func _process(delta: float) -> void:
	state.update(delta);
	post_update(delta);

func _physics_process(delta: float) -> void:
	state.phys_update(delta);
	post_phys_update(delta);

func post_update(_delta: float) -> void:
	pass;

func post_phys_update(_delta: float) -> void:
	pass;
