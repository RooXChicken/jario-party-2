extends State

@export var anim: AnimationPlayer;
var sound: AudioStreamPlayer;

func enter(_old_state: String, _data: Dictionary = {}) -> void:
	SoundManager.load_sound("intro_jingle", "res://sounds/misc/badge.mp3");
	sound = SoundManager.play_sound("intro_jingle");

func update(_delta: float) -> void:
	if(Input.is_action_just_pressed("select")):
		state_machine.set_state("TitleScreen", { "skipped": true });
		SoundManager.stop_sound(sound);
