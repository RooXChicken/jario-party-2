extends State

func enter(old_state: String, data: Dictionary = {}) -> void:
	owner.stage = 3;

func update(delta: float) -> void:
	if(Input.is_action_just_pressed("select")):
		# load game
		pass;
	
	if(Input.is_action_just_pressed("back")):
		state_machine.set_state("PlayerSettings");
