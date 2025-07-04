extends State

@onready var player: CharacterController = owner;
var jump_flip := true;

func enter(previous_state: String, data: Dictionary = {}) -> void:
	player.y_velocity = player.jump_height;
	SoundManager.play_sound("character_playable_jump");
	
	jump_flip = !jump_flip;

func phys_update(delta: float) -> void:
	if(!Input.is_joy_button_pressed(player.controller_index, JOY_BUTTON_A) && player.y_velocity < 0):
		player.y_velocity += player.gravity_speed;
	
	var joy_axis := player.get_joy();
	
	if(abs(joy_axis.x) >= player.deadzone):
		player.velocity.x = move_toward(player.velocity.x, player.top_speed * joy_axis.x, player.jump_acceleration);
	else:
		player.velocity.x = move_toward(player.velocity.x, 0, player.jump_deceleration);
	
	if(abs(joy_axis.y) >= player.deadzone):
		player.velocity.y = move_toward(player.velocity.y, player.top_speed * joy_axis.y, player.jump_acceleration);
	else:
		player.velocity.y = move_toward(player.velocity.y, 0, player.jump_deceleration);
	
	player.y_velocity = move_toward(player.y_velocity, player.max_fall_speed, player.gravity_speed);
	player.y += player.y_velocity;
	
	player.sprite.frame = 0 if(player.y_velocity <= 0) else 1;
	
	if(player.y > 0):
		player.y = 0;
		
		player.animate("walk", joy_axis, 0);
		state_machine.set_state("Walking", {}, StateMachine.TickNextState.PHYS_UPDATE, delta);
		return;

	player.animate("jump", joy_axis, 0.0);
	
	match player.dir:
		"up": player.sprite.flip_h = jump_flip;
		"down": player.sprite.flip_h = jump_flip;
	
	player.move();
