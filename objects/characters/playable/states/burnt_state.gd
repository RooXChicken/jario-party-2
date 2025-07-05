extends State

@onready var player: CharacterController = owner;

func enter(old_state: String, data: Dictionary = {}):
	player.play_out();
	
	player.set_anim("burnt");
	player.anim.play("burnt");
	player.sprite.z_index = -2;

func phys_update(_delta: float) -> void:
	player.y_velocity = 0;
	player.move();

func play_star() -> void:
	SoundManager.play_sound("character_playable_star");
	
	var star: Node2D = load("res://scenes/minigames/jump_rope/star.tscn").instantiate();
	star.position = player.position;
	star.position.y -= 20;
	star.z_index = -2;
	
	add_child(star);
