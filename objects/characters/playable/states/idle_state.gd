extends State

@onready var player: CharacterController = owner;

func enter(previous_state: String, data: Dictionary = {}) -> void:
	pass;

func phys_update(delta: float) -> void:
	if(Input.is_action_just_pressed("jump") && player.y <= 0 && player.has_ability(CharacterController.Ability.JUMP)):
		state_machine.set_state("Jumping", {}, StateMachine.TickNextState.PHYS_UPDATE, delta);
		return;
	
	var joy_axis := player.get_joy();
	
	if((abs(joy_axis.x) > player.deadzone || abs(joy_axis.y) > player.deadzone) && player.has_ability(CharacterController.Ability.MOVE)):
		state_machine.set_state("Walking", {}, StateMachine.TickNextState.PHYS_UPDATE, delta);
		return;
	
	player.sprite.frame = 1;
	player.move();
