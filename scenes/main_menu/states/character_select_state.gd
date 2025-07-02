extends State

func enter(_old_state: String, _data: Dictionary = {}) -> void:
	owner.stage = 1;

func update(_delta: float) -> void:
	if(Input.is_action_just_pressed("select")):
		state_machine.set_state("PlayerSettings");
	
	if(Input.is_action_just_pressed("back")):
		state_machine.set_state("ModeSelect");
