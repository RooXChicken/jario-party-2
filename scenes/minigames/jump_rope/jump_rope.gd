extends Minigame

@export var flame_curve: Curve;
@export var size_curve: Curve;

const fire_red := 2.8;

var heights := [0, 18, 32, 44, 48, 49, 49, 49, 48, 44, 32, 18, 0];
var time := 0.0;

@export var speed_mult := 0.03;
var speed := 0.6;

func _ready() -> void:
	#for i in range(0, 13):
		#(get_node("Flames/Flame" + str(i+1) + "/Sprite") as AnimatedSprite2D).speed_scale = 0.6 + (randf()*0.4);
	
	spawn_players();

func _physics_process(delta: float) -> void:
	var sample_time: float = time - floor(time);
	var sample := (flame_curve.sample(sample_time) - 0.5) * 2;
	var size := size_curve.sample(sample_time);
	
	for i in range(0, 13):
		var fire: Node2D = get_node("Flames/Flame" + str(i+1));
		fire.position.y = sample * heights[i];
		fire.scale = Vector2(size, size);
		
		fire.z_index = 2 if sample_time < 0.5 else 1;
		
		if(sample_time > 0.42 && sample_time < 0.58):
			fire.modulate.r = fire_red;
		else:
			fire.modulate.r = 1.0;
	
	time += delta * speed;
	speed += speed_mult * delta;
