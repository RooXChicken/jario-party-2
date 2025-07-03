extends State

@export var anim: AnimationPlayer;

func enter(old_state: String, data: Dictionary) -> void:
	

func update(_delta: float) -> void:
	if(Input.is_action_just_pressed("select")):
		state_machine.set_state("TitleScreen", { "skipped": true });
