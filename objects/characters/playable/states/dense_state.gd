extends State

@onready var player: CharacterController = owner;

func phys_update(_delta: float) -> void:
	player.velocity.x = move_toward(player.velocity.x, 0, player.deceleration);
	player.velocity.y = move_toward(player.velocity.y, 0, player.deceleration);
	
	player.move();
