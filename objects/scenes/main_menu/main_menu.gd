extends Node2D

const stage_count := 4;
var stage := 0;

var streamSync: AudioStreamSynchronized;
var volume: Array[float] = [];

var anim: AnimationPlayer;

func _ready() -> void:
	anim = get_node("AnimationPlayer");
	
	streamSync = (get_node("MenuMusic").stream as AudioStreamSynchronized);
	
	for i in range(0, stage_count):
		volume.append(1.0);

func _process(delta: float) -> void:
	if(Input.is_action_just_pressed("select")):
		if(stage < stage_count-1):
			stage += 1;
			anim.play("menu_stage_" + str(stage));
	
	if(Input.is_action_just_pressed("back")):
		if(stage > 0):
			anim.play_backwards("menu_stage_" + str(stage));
			stage -= 1;
	
	for i in range(0, stage_count):
		# uses a math formula to determine the X passed into the final formula
		volume[i] = clamp(volume[i] + (1 if i > stage else -1) * delta, 0, 0.5);
		
		# set the stream volume based on exponential growth
		streamSync.set_sync_stream_volume(i, (pow(4, volume[i]) - 1) * -80.0);

func remove_transition() -> void:
	get_node("TransitionGroup").queue_free();
