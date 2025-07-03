extends Node2D

@export var anim: AnimationPlayer;
var streamSync: AudioStreamSynchronized;

var stage_count := 0;
var stage := 0;

var volume: Array[float] = [];

func _ready() -> void:
	streamSync = $MenuMusic.stream;
	stage_count = streamSync.stream_count;
	
	for i: int in range(0, stage_count):
		volume.append(1.0);
	
	volume[0] = 0.0;

func _process(delta: float) -> void:
	for i: int in range(0, stage_count):
		# uses a math formula to determine the X passed into the final formula
		volume[i] = clamp(volume[i] + (1 if i > stage else -1) * delta, 0, 0.5);
		
		# set the stream volume based on exponential growth
		streamSync.set_sync_stream_volume(i, (pow(4, volume[i]) - 1) * -80.0);
