extends State

func enter(old_state: String, data: Dictionary = {}) -> void:
	owner.stage = 0;

func update(delta: float) -> void:
	if(Input.is_action_just_pressed("select")):
		state_machine.set_state("CharacterSelect");
