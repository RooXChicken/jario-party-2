extends State

@onready var player: CharacterController = owner;
var idle_timer := 0.0;

func enter(previous_state: String, data: Dictionary = {}) -> void:
	idle_timer = 0.0;

func phys_update(delta: float) -> void:
	if(player.try_jump(delta)):
		return;
	
	var joy_axis := player.get_joy();
	
	if((abs(joy_axis.x) > player.deadzone || abs(joy_axis.y) > player.deadzone) && player.has_ability(CharacterController.Ability.MOVE)):
		state_machine.set_state("Walking", {}, StateMachine.TickNextState.PHYS_UPDATE, delta);
		return;
	
	player.velocity.x = move_toward(player.velocity.x, 0, player.deceleration);
	player.velocity.y = move_toward(player.velocity.y, 0, player.deceleration);
	
	if(player.sprite.animation != "long_idle"):
		player.sprite.frame = 0;
		
		idle_timer += delta;
		if(idle_timer >= 10.0):
			player.set_anim("long_idle");
			player.dir = "down";
			player.sprite.speed_scale = 1.0;
	
	player.move();
