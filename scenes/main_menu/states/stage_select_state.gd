extends State

func enter(_old_state: String, _data: Dictionary = {}) -> void:
	owner.stage = 3;

func update(_delta: float) -> void:
	if(Input.is_action_just_pressed("select")):
		# load game
		pass;
	
	if(Input.is_action_just_pressed("back")):
		state_machine.set_state("PlayerSettings");
