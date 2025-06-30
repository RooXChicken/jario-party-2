extends State

@export var anim: AnimationPlayer;

func update(delta: float) -> void:
	if(Input.is_action_just_pressed("select")):
		state_machine.set_state("TitleScreen", { "skipped": true });
