extends State

@onready var player: CharacterController = owner;

func enter(previous_state: String, data: Dictionary = {}) -> void:
	pass;

func phys_update(delta: float) -> void:
	if(Input.is_action_just_pressed("jump") && player.y <= 0 && player.has_ability(CharacterController.Ability.JUMP)):
		state_machine.set_state("Jumping", {}, StateMachine.TickNextState.PHYS_UPDATE, delta);
		return;
	
	var joy_axis := player.get_joy();
	
	if(abs(joy_axis.x) >= player.deadzone):
		player.velocity.x = move_toward(player.velocity.x, player.top_speed * joy_axis.x, player.acceleration);
	else:
		player.velocity.x = move_toward(player.velocity.x, 0, player.deceleration);
	
	if(abs(joy_axis.y) >= player.deadzone):
		player.velocity.y = move_toward(player.velocity.y, player.top_speed * joy_axis.y, player.acceleration);
	else:
		player.velocity.y = move_toward(player.velocity.y, 0, player.deceleration);
		
	if(abs(player.velocity.length()) < player.move_min_speed):
		player.velocity = Vector2(0, 0);
		state_machine.set_state("Idle");
		return;
	
	var combined_speed: float = abs(joy_axis.x) + abs(joy_axis.y);
	if(combined_speed > 0.04):
		player.animate("walk", joy_axis, combined_speed);
	
	player.move();
